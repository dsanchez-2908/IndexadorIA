using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Negocio
{
    /// <summary>
    /// Orquesta el Validador API SMP en 3 pasos independientes (cada uno con su VPN):
    /// Descargar (BD) -> Validar (API) -> Sincronizar (BD).
    /// </summary>
    public class SmpValidacionBL
    {
        private const int TamanoLoteLocal = 5000;
        private const int TamanoLoteSync = 500;
        private const int MaxFallasConsecutivas = 5;

        private readonly SmpLocalStore _local = new();

        public SmpContadores ObtenerContadores() => _local.ObtenerContadores();

        /// <summary>Paso 1 (VPN BD): copia al almacen local los registros nuevos o modificados.</summary>
        public int Descargar(IProgress<string>? progreso, CancellationToken ct)
        {
            var dal = new SmpValidacionDAL();
            var buffer = new List<SmpPendiente>(TamanoLoteLocal);
            int total = 0;

            foreach (var p in dal.ObtenerPendientes())
            {
                ct.ThrowIfCancellationRequested();
                buffer.Add(p);
                if (buffer.Count >= TamanoLoteLocal)
                {
                    _local.Incorporar(buffer);
                    total += buffer.Count;
                    buffer.Clear();
                    progreso?.Report($"Descargados {total:N0} registros...");
                }
            }
            if (buffer.Count > 0)
            {
                _local.Incorporar(buffer);
                total += buffer.Count;
            }
            return total;
        }

        /// <summary>Paso 2 (VPN API): consulta la API de a un SMP por vez. Reanudable.</summary>
        public async Task<int> ValidarAsync(string urlBase, IProgress<string>? progreso, CancellationToken ct)
        {
            var cliente = new SmpApiClient(urlBase);
            int procesados = 0;
            int fallas = 0;

            while (!ct.IsCancellationRequested)
            {
                var lote = _local.ObtenerPendientes(200);
                if (lote.Count == 0) break;

                foreach (var p in lote)
                {
                    if (ct.IsCancellationRequested) break;
                    try
                    {
                        var resultado = await cliente.ConsultarAsync(p, ct);
                        _local.GuardarResultado(resultado);
                        procesados++;
                        fallas = 0;
                        if (procesados % 20 == 0)
                        {
                            progreso?.Report($"Validados {procesados:N0} en esta ejecucion...");
                        }
                    }
                    catch (OperationCanceledException) when (ct.IsCancellationRequested)
                    {
                        break;
                    }
                    catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
                    {
                        fallas++;
                        if (fallas >= MaxFallasConsecutivas)
                        {
                            throw new InvalidOperationException(
                                "Se interrumpio por fallas consecutivas de conexion con la API. " +
                                "Verifique la VPN (FortiClient). Lo procesado quedo guardado.", ex);
                        }
                    }
                }
            }
            return procesados;
        }

        /// <summary>Paso 3 (VPN BD): sube a SQL Server los resultados aun no sincronizados.</summary>
        public int Sincronizar(IProgress<string>? progreso, CancellationToken ct)
        {
            var dal = new SmpValidacionDAL();
            int total = 0;

            while (!ct.IsCancellationRequested)
            {
                var lote = _local.ObtenerSinSincronizar(TamanoLoteSync);
                if (lote.Count == 0) break;

                dal.Guardar(lote);
                _local.MarcarSincronizados(lote.Select(x => x.CdResultado));
                total += lote.Count;
                progreso?.Report($"Sincronizados {total:N0} registros...");
            }
            return total;
        }
    }
}
