using System.Globalization;
using System.Text.Json;
using IndexadorIA.Entidades;

namespace IndexadorIA.Negocio
{
    /// <summary>Cliente de la API SMP (consultas de a una, secuenciales).</summary>
    public class SmpApiClient
    {
        public const string UrlBaseDefault = "http://mdu01-webpdi.spdgiur.gcba.gov.ar/wfs/api_smp/?smp=";

        private static readonly HttpClient Http = CrearHttp();

        private static HttpClient CrearHttp()
        {
            var http = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
            http.DefaultRequestHeaders.UserAgent.ParseAdd("PostmanRuntime/7.39.0");
            http.DefaultRequestHeaders.Accept.ParseAdd("*/*");
            return http;
        }

        private readonly string _urlBase;

        public SmpApiClient(string urlBase)
        {
            _urlBase = urlBase;
        }

        /// <summary>
        /// Consulta un SMP. Lanza excepcion ante fallas de red (sin conexion/timeout).
        /// Reintenta ante HTTP 500.
        /// </summary>
        public async Task<SmpValidacion> ConsultarAsync(SmpPendiente pendiente, CancellationToken ct)
        {
            string url = _urlBase + Uri.EscapeDataString(pendiente.DsSmp!);
            int status = 0;
            string json = string.Empty;

            for (int intento = 0; intento < 3; intento++)
            {
                using var resp = await Http.GetAsync(url, ct);
                status = (int)resp.StatusCode;
                json = await resp.Content.ReadAsStringAsync(ct);
                if (status != 500) break;
                await Task.Delay(1000, ct);
            }

            var resultado = Parsear(json);
            resultado.CdResultado = pendiente.CdResultado;
            resultado.CdLote = pendiente.CdLote;
            resultado.CdArchivoPagina = pendiente.CdArchivoPagina;
            resultado.DsSeccion = pendiente.DsSeccion;
            resultado.DsManzana = pendiente.DsManzana;
            resultado.DsParcela = pendiente.DsParcela;
            resultado.DsSmp = pendiente.DsSmp;
            resultado.NuHttpStatus = status;
            resultado.FeConsulta = DateTime.Now;
            return resultado;
        }

        /// <summary>Parsea el JSON de la API. Si no es valido, devuelve estado error.</summary>
        public static SmpValidacion Parsear(string json)
        {
            var r = new SmpValidacion { DsJson = json };
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                r.DsEstado = Texto(root, "estado") ?? SmpEstado.Error;
                r.DsMensaje = Texto(root, "mensaje");

                if (root.TryGetProperty("parcelas_vigentes", out var lista) && lista.ValueKind == JsonValueKind.Array)
                {
                    foreach (var p in lista.EnumerateArray())
                    {
                        var parcela = new SmpParcelaVigente
                        {
                            DsSmp = Texto(p, "smp") ?? string.Empty,
                            DsCaso = Texto(p, "caso"),
                            DsObservacion = Texto(p, "observacion"),
                            DsSurgeDe = Unir(p, "surge_de", ", "),
                            DsDirecciones = Unir(p, "direcciones", " / ")
                        };
                        if (DateTime.TryParseExact(Texto(p, "fecha_registro"), "dd/MM/yyyy",
                                CultureInfo.InvariantCulture, DateTimeStyles.None, out var fe))
                        {
                            parcela.FeRegistro = fe;
                        }
                        r.Parcelas.Add(parcela);
                    }
                }
            }
            catch (JsonException)
            {
                r.DsEstado = SmpEstado.Error;
                r.DsMensaje = "Respuesta no es un JSON valido";
            }
            return r;
        }

        private static string? Texto(JsonElement e, string nombre)
        {
            return e.TryGetProperty(nombre, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;
        }

        private static string? Unir(JsonElement e, string nombre, string separador)
        {
            if (!e.TryGetProperty(nombre, out var v) || v.ValueKind != JsonValueKind.Array) return null;
            var items = v.EnumerateArray().Where(x => x.ValueKind == JsonValueKind.String).Select(x => x.GetString());
            return string.Join(separador, items);
        }
    }
}
