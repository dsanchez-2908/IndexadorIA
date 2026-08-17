using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Utilidades;

namespace IndexadorIA.Negocio
{
    /// <summary>
    /// Lógica de negocio para la finalización de un lote (pantalla "Finalizar Lote"):
    /// validación de campos obligatorios y de archivos abiertos, renombrado/movimiento
    /// de PDFs según categoría y estado de control, generación de los CSV de metadatos
    /// y actualización del estado del lote a "Finalizado".
    /// </summary>
    public class LoteFinalizacionBL
    {
        /// <summary>
        /// Estado de lote "Finalizado" (dsProceso = 'LOTE' en TD_ESTADOS)
        /// </summary>
        public const int EstadoLoteFinalizado = 3;

        /// <summary>
        /// Estado de lote "Pendiente de Finalizar" (dsProceso = 'LOTE' en TD_ESTADOS)
        /// </summary>
        public const int EstadoLotePendienteFinalizar = 7;

        /// <summary>
        /// Nombre de la subcarpeta donde se mueven los archivos ilegibles.
        /// </summary>
        public const string CarpetaIlegibles = "ILEGIBLES";

        private readonly LoteDAL _loteDAL;
        private readonly LogDAL _logDAL;

        public LoteFinalizacionBL()
        {
            _loteDAL = new LoteDAL();
            _logDAL = new LogDAL();
        }

        /// <summary>
        /// Fila combinada de archivo/página + resultado de IA usada para el proceso de finalización.
        /// </summary>
        public class FilaFinalizacion
        {
            public ArchivoPagina Archivo { get; set; } = null!;
            public ResultadoIA Resultado { get; set; } = null!;
        }

        private static bool EsControlable(ResultadoIA resultado)
        {
            return resultado.CdEstadoControl == ResultadoIA.EstadosControl.PendienteControl
                || resultado.CdEstadoControl == ResultadoIA.EstadosControl.Controlado;
        }

        private static bool EsIlegible(ResultadoIA resultado)
        {
            return resultado.CdEstadoControl == ResultadoIA.EstadosControl.PaginaIlegible
                || resultado.CdEstadoControl == ResultadoIA.EstadosControl.DatosIlegibles;
        }

        /// <summary>
        /// Control 1: valida que los registros en estado Pendiente de Control o Controlado
        /// tengan completos los campos obligatorios: Categoría, Tipo de Plano, Sección,
        /// Manzana, Parcela y Dirección. Devuelve la lista de filas con algún campo faltante.
        /// </summary>
        public List<FilaFinalizacion> ValidarCamposObligatorios(List<FilaFinalizacion> filas)
        {
            var faltantes = new List<FilaFinalizacion>();

            foreach (var fila in filas)
            {
                if (!EsControlable(fila.Resultado))
                    continue;

                bool completo = fila.Resultado.CdCategoriaPlano.HasValue
                    && fila.Resultado.CdTipoPlano.HasValue
                    && !string.IsNullOrWhiteSpace(fila.Resultado.DsSeccion)
                    && !string.IsNullOrWhiteSpace(fila.Resultado.DsManzana)
                    && !string.IsNullOrWhiteSpace(fila.Resultado.DsParcela)
                    && !string.IsNullOrWhiteSpace(fila.Resultado.DsDireccion);

                if (!completo)
                    faltantes.Add(fila);
            }

            return faltantes;
        }

        /// <summary>
        /// Control 2: valida que ninguno de los archivos PDF a renombrar/mover esté
        /// actualmente abierto/bloqueado por otro proceso. Devuelve la lista de rutas
        /// que no se pudieron abrir en modo exclusivo.
        /// </summary>
        public List<string> ValidarArchivosNoAbiertos(List<FilaFinalizacion> filas)
        {
            var bloqueados = new List<string>();

            foreach (var fila in filas)
            {
                string ruta = fila.Archivo.DsRutaCompleta;

                if (string.IsNullOrWhiteSpace(ruta) || !File.Exists(ruta))
                    continue;

                try
                {
                    using var stream = File.Open(ruta, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
                catch (IOException)
                {
                    bloqueados.Add(ruta);
                }
            }

            return bloqueados;
        }

        /// <summary>
        /// Cuenta la cantidad de registros en estado Pendiente de Control (aún no controlados).
        /// </summary>
        public int ContarPendientesControl(List<FilaFinalizacion> filas)
        {
            return filas.Count(f => f.Resultado.CdEstadoControl == ResultadoIA.EstadosControl.PendienteControl);
        }

        /// <summary>
        /// Marca el lote con el estado "Finalizado".
        /// </summary>
        public void MarcarLoteFinalizado(int cdLote, int cdUsuario)
        {
            _loteDAL.ActualizarEstado(cdLote, EstadoLoteFinalizado, cdUsuario);
        }

        /// <summary>
        /// Marca el lote con el estado "Pendiente de Finalizar" (usado desde FrmVerLote
        /// cuando el control de todas las páginas del lote se completó).
        /// </summary>
        public void MarcarLotePendienteFinalizar(int cdLote, int cdUsuario)
        {
            _loteDAL.ActualizarEstado(cdLote, EstadoLotePendienteFinalizar, cdUsuario);
        }

        /// <summary>
        /// Información sobre el resultado de mover/renombrar un archivo.
        /// </summary>
        public class RegistroMovimiento
        {
            public int CdArchivoPagina { get; set; }
            public string CarpetaDestino { get; set; } = string.Empty;
            public string NombreArchivoNuevo { get; set; } = string.Empty;
            public bool EsIlegible { get; set; }
        }

        /// <summary>
        /// Renombra (estados 1/2) o mueve y renombra (estados 3/4, hacia la subcarpeta
        /// "ILEGIBLES") los archivos PDF de las filas indicadas, según las reglas de
        /// nomenclatura por categoría de plano, y persiste el nombre final en
        /// TD_ARCHIVOS_PAGINAS.dsNombreArchivoFinal.
        /// </summary>
        public List<RegistroMovimiento> MoverYRenombrarArchivos(List<FilaFinalizacion> filas)
        {
            var movimientos = new List<RegistroMovimiento>();

            foreach (var fila in filas)
            {
                string rutaOrigen = fila.Archivo.DsRutaCompleta;

                if (string.IsNullOrWhiteSpace(rutaOrigen) || !File.Exists(rutaOrigen))
                    throw new FileNotFoundException($"No se encontró el archivo físico: {rutaOrigen}");

                string carpetaOrigen = Path.GetDirectoryName(rutaOrigen) ?? string.Empty;
                bool esIlegible = EsIlegible(fila.Resultado);

                string carpetaDestino = esIlegible
                    ? Path.Combine(carpetaOrigen, CarpetaIlegibles)
                    : carpetaOrigen;

                if (esIlegible && !Directory.Exists(carpetaDestino))
                    Directory.CreateDirectory(carpetaDestino);

                string nombreNuevo = esIlegible
                    ? GeneradorNombreArchivo.ConstruirNombreIlegible(
                        fila.Resultado.DsCategoriaPlano,
                        fila.Resultado.DsTipoPlano,
                        fila.Resultado.DsNumeroPlano,
                        fila.Resultado.DsDireccion,
                        fila.Resultado.DsSeccion,
                        fila.Resultado.DsManzana,
                        fila.Resultado.DsParcela,
                        fila.Resultado.DsExpediente)
                    : GeneradorNombreArchivo.ConstruirNombreControlado(
                        fila.Resultado.DsCategoriaPlano,
                        fila.Resultado.DsTipoPlano,
                        fila.Resultado.DsNumeroPlano,
                        fila.Resultado.DsDireccion,
                        fila.Resultado.DsSeccion,
                        fila.Resultado.DsManzana,
                        fila.Resultado.DsParcela,
                        fila.Resultado.DsExpediente);

                nombreNuevo = GeneradorNombreArchivo.ResolverColision(carpetaDestino, nombreNuevo);

                string rutaDestino = Path.Combine(carpetaDestino, nombreNuevo);

                if (!string.Equals(rutaOrigen, rutaDestino, StringComparison.OrdinalIgnoreCase))
                    File.Move(rutaOrigen, rutaDestino);

                _loteDAL.ActualizarNombreArchivoFinal(fila.Archivo.CdArchivoPagina, nombreNuevo);

                movimientos.Add(new RegistroMovimiento
                {
                    CdArchivoPagina = fila.Archivo.CdArchivoPagina,
                    CarpetaDestino = carpetaDestino,
                    NombreArchivoNuevo = nombreNuevo,
                    EsIlegible = esIlegible
                });
            }

            return movimientos;
        }

        private static readonly string[] EncabezadosCsv =
        {
            "Ruta",
            "Nombre de archivo original",
            "Nombre Nuevo",
            "Categoria",
            "Tipo de Plano",
            "Acronimo",
            "Direccion",
            "Seccion",
            "Manzana",
            "Parcela",
            "Expediente",
            "Numero de Plano",
            "Observaciones"
        };

        /// <summary>
        /// Genera los CSV de metadatos del lote a partir de la vista VW_001_RESULTADO_IA
        /// (que ya refleja los nombres finales persistidos por MoverYRenombrarArchivos):
        /// un CSV "[carpeta].csv" para los registros correctos (estados 1/2) en la carpeta
        /// de origen, y un CSV "[carpeta]_ILEGIBLE.csv" para los registros ilegibles
        /// (estados 3/4) dentro de la subcarpeta ILEGIBLES.
        /// </summary>
        public void GenerarCsvMetadatos(int cdLote)
        {
            var filas = _loteDAL.ObtenerMetadatosParaCsv(cdLote);

            var correctas = filas.Where(f =>
                f.CdEstadoControl == ResultadoIA.EstadosControl.PendienteControl
                || f.CdEstadoControl == ResultadoIA.EstadosControl.Controlado).ToList();

            var ilegibles = filas.Where(f =>
                f.CdEstadoControl == ResultadoIA.EstadosControl.PaginaIlegible
                || f.CdEstadoControl == ResultadoIA.EstadosControl.DatosIlegibles).ToList();

            EscribirCsvPorCarpeta(correctas, esIlegible: false);
            EscribirCsvPorCarpeta(ilegibles, esIlegible: true);
        }

        private void EscribirCsvPorCarpeta(List<ResultadoIAMetadataDto> filas, bool esIlegible)
        {
            var porCarpeta = filas
                .Where(f => !string.IsNullOrWhiteSpace(f.DsRutaCompleta))
                .GroupBy(f => Path.GetDirectoryName(f.DsRutaCompleta) ?? string.Empty);

            foreach (var grupo in porCarpeta)
            {
                string carpetaOrigen = grupo.Key;
                string nombreCarpetaOrigen = new DirectoryInfo(carpetaOrigen).Name;

                string carpetaCsv = esIlegible ? Path.Combine(carpetaOrigen, CarpetaIlegibles) : carpetaOrigen;

                if (esIlegible && !Directory.Exists(carpetaCsv))
                    Directory.CreateDirectory(carpetaCsv);

                string nombreCsv = esIlegible
                    ? $"{nombreCarpetaOrigen}_ILEGIBLE.csv"
                    : $"{nombreCarpetaOrigen}.csv";

                string rutaCsv = Path.Combine(carpetaCsv, nombreCsv);

                using var writer = new StreamWriter(rutaCsv, append: false, System.Text.Encoding.UTF8);
                writer.WriteLine(string.Join(";", EncabezadosCsv));

                foreach (var f in grupo)
                {
                    bool esCatastro = GeneradorNombreArchivo.EsCategoriaCatastro(f.DsCategoriaPlano);

                    var campos = new[]
                    {
                        EscaparCampoCsv(f.DsRutaCompleta),
                        EscaparCampoCsv(f.DsNombreArchivoOriginal),
                        EscaparCampoCsv(f.DsNombreArchivoFinal),
                        EscaparCampoCsv(f.DsCategoriaPlano),
                        EscaparCampoCsv(f.DsTipoPlano),
                        EscaparCampoCsv(f.DsAcronimo),
                        EscaparCampoCsv(f.DsDireccion),
                        EscaparCampoCsv(f.DsSeccion),
                        EscaparCampoCsv(f.DsManzana),
                        EscaparCampoCsv(f.DsParcela),
                        EscaparCampoCsv(f.DsExpediente),
                        EscaparCampoCsv(esCatastro ? f.DsNumeroPlano : string.Empty),
                        string.Empty
                    };

                    writer.WriteLine(string.Join(";", campos));
                }
            }
        }

        private static string EscaparCampoCsv(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            if (valor.Contains(';') || valor.Contains('"') || valor.Contains('\n'))
                return "\"" + valor.Replace("\"", "\"\"") + "\"";

            return valor;
        }

        /// <summary>
        /// Orquesta el proceso completo de finalización de un lote:
        /// 1) Mueve/renombra los PDFs de las filas indicadas según su estado.
        /// 2) Genera los CSV de metadatos (correctos e ilegibles).
        /// 3) Marca el lote con el estado "Finalizado".
        /// Se asume que la validación de campos obligatorios, de archivos abiertos y la
        /// confirmación del usuario ya fueron realizadas previamente por el llamador.
        /// </summary>
        public void FinalizarLote(int cdLote, List<FilaFinalizacion> filas, int cdUsuario)
        {
            MoverYRenombrarArchivos(filas);
            GenerarCsvMetadatos(cdLote);
            MarcarLoteFinalizado(cdLote, cdUsuario);
        }

        /// <summary>
        /// Registra un error en el log del sistema.
        /// </summary>
        public void RegistrarError(string modulo, Exception ex, int cdUsuario)
        {
            _logDAL.Insertar(new LogRegistro
            {
                DsNivel = LogRegistro.Niveles.ERROR,
                DsModulo = modulo,
                DsMensaje = $"Error en finalización de lote: {ex.Message}",
                DsExcepcion = ex.ToString(),
                CdUsuario = cdUsuario
            });
        }
    }
}
