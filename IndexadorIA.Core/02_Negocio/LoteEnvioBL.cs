using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Negocio
{
    /// <summary>
    /// Lógica de negocio para el envío de lotes finalizados (pantalla "Envio"):
    /// mueve los PDFs finales y los registros de los CSV de índice correspondientes
    /// SOLO a los lotes seleccionados hacia la carpeta de envío final, y marca los
    /// lotes (y sus páginas) como "Enviado".
    /// </summary>
    public class LoteEnvioBL
    {
        /// <summary>
        /// Estado de lote "Enviado" (dsProceso = 'LOTE' en TD_ESTADOS).
        /// </summary>
        public const int EstadoLoteEnviado = 11;

        /// <summary>
        /// Estado de lote "Pendiente de Enviar" (dsProceso = 'LOTE' en TD_ESTADOS), consumido
        /// por la pantalla FrmEnvioLote una vez finalizado el lote.
        /// </summary>
        public const int EstadoLotePendienteEnviar = 10;

        /// <summary>
        /// Estado de página "Enviado" (dsProceso = 'ARCHIVO_PAGINA' en TD_ESTADOS).
        /// </summary>
        public const int EstadoPaginaEnviada = 8;

        public const string CarpetaProcesados = LoteFinalizacionBL.CarpetaProcesados;
        public const string CarpetaIlegibles = LoteFinalizacionBL.CarpetaIlegibles;

        private readonly LoteDAL _loteDAL;
        private readonly ArchivoPaginaDAL _archivoPaginaDAL;
        private readonly LogDAL _logDAL;

        public LoteEnvioBL()
        {
            _loteDAL = new LoteDAL();
            _archivoPaginaDAL = new ArchivoPaginaDAL();
            _logDAL = new LogDAL();
        }

        /// <summary>
        /// Fila combinada de archivo/página + resultado de IA usada para el proceso de envío.
        /// </summary>
        public class FilaEnvio
        {
            public ArchivoPagina Archivo { get; set; } = null!;
            public ResultadoIA Resultado { get; set; } = null!;
        }

        private static bool EsIlegible(ResultadoIA resultado)
        {
            return resultado.CdEstadoControl == ResultadoIA.EstadosControl.PaginaIlegible
                || resultado.CdEstadoControl == ResultadoIA.EstadosControl.DatosIlegibles;
        }

        /// <summary>
        /// Obtiene la carpeta "Planos Procesados"/"Planos ILEGIBLES" donde reside actualmente
        /// el PDF final de una página, calculada igual que las queries de referencia:
        /// dirname(dsRutaCompleta DEL ARCHIVO ORIGINAL) + "\Planos Procesados" o "\Planos ILEGIBLES".
        /// </summary>
        private static string ObtenerCarpetaOrigen(ArchivoPagina archivo, bool esIlegible)
        {
            string carpetaBase = Path.GetDirectoryName(archivo.DsRutaCompletaOriginal) ?? string.Empty;
            return Path.Combine(carpetaBase, esIlegible ? CarpetaIlegibles : CarpetaProcesados);
        }

        /// <summary>
        /// Resultado de mover un archivo/registro de un lote hacia la carpeta de envío.
        /// </summary>
        public class RegistroEnvio
        {
            public int CdArchivoPagina { get; set; }
            public string CarpetaOrigen { get; set; } = string.Empty;
            public string CarpetaDestino { get; set; } = string.Empty;
            public string NombreArchivoFinal { get; set; } = string.Empty;
            public bool EsIlegible { get; set; }
        }

        /// <summary>
        /// Mueve los PDFs finales de las filas indicadas (localizados en la subcarpeta
        /// "Planos Procesados"/"Planos ILEGIBLES", con el nombre persistido en
        /// dsNombreArchivoFinal) hacia la carpeta de envío indicada. No copia: mueve el
        /// archivo físico.
        /// </summary>
        public List<RegistroEnvio> MoverArchivos(List<FilaEnvio> filas, string carpetaEnvioBase)
        {
            var movimientos = new List<RegistroEnvio>();

            foreach (var fila in filas)
            {
                string nombreArchivoFinal = fila.Archivo.DsNombreArchivoFinal ?? string.Empty;

                if (string.IsNullOrWhiteSpace(nombreArchivoFinal))
                    throw new InvalidOperationException(
                        $"El archivo/página {fila.Archivo.CdArchivoPagina} no tiene nombre de archivo final registrado (dsNombreArchivoFinal).");

                bool esIlegible = EsIlegible(fila.Resultado);
                string carpetaOrigen = ObtenerCarpetaOrigen(fila.Archivo, esIlegible);
                string rutaOrigen = Path.Combine(carpetaOrigen, nombreArchivoFinal);

                if (!File.Exists(rutaOrigen))
                    throw new FileNotFoundException(
                        $"No se encontró el archivo físico del archivo/página {fila.Archivo.CdArchivoPagina}: {rutaOrigen}");

                string carpetaDestino = Path.Combine(carpetaEnvioBase, esIlegible ? CarpetaIlegibles : CarpetaProcesados);

                if (!Directory.Exists(carpetaDestino))
                    Directory.CreateDirectory(carpetaDestino);

                string rutaDestino = Path.Combine(carpetaDestino, nombreArchivoFinal);

                if (!string.Equals(rutaOrigen, rutaDestino, StringComparison.OrdinalIgnoreCase))
                {
                    if (File.Exists(rutaDestino))
                        throw new IOException($"Ya existe un archivo con el mismo nombre en el destino: {rutaDestino}");

                    File.Move(rutaOrigen, rutaDestino);
                }

                movimientos.Add(new RegistroEnvio
                {
                    CdArchivoPagina = fila.Archivo.CdArchivoPagina,
                    CarpetaOrigen = carpetaOrigen,
                    CarpetaDestino = carpetaDestino,
                    NombreArchivoFinal = nombreArchivoFinal,
                    EsIlegible = esIlegible
                });
            }

            return movimientos;
        }

        /// <summary>
        /// Nombre del CSV de índice ubicado en la carpeta de origen (misma carpeta donde
        /// están los PDFs "Planos Procesados"/"Planos ILEGIBLES"), generado durante la
        /// finalización del lote: "[carpeta entrega].csv" o "[carpeta entrega]_ILEGIBLE.csv".
        /// </summary>
        private static string ObtenerNombreCsvOrigen(string carpetaOrigen, bool esIlegible)
        {
            var directorioOrigen = new DirectoryInfo(carpetaOrigen);
            string nombreCarpetaBase = directorioOrigen.Parent != null ? directorioOrigen.Parent.Name : directorioOrigen.Name;

            return esIlegible ? $"{nombreCarpetaBase}_ILEGIBLE.csv" : $"{nombreCarpetaBase}.csv";
        }

        /// <summary>
        /// Mueve SOLO los registros de los CSV de índice (ubicados en la misma carpeta de
        /// origen de los PDFs) cuya columna "Nombre Nuevo" coincida con el nombre de archivo
        /// final movido, hacia el CSV de índice en la carpeta de envío. Los registros de
        /// otros lotes que compartan la misma carpeta permanecen intactos en el CSV de origen.
        /// El CSV de destino se nombra "{nombreCarpetaSalida}.csv".
        /// </summary>
        public void MoverRegistrosCsv(List<RegistroEnvio> movimientos, string nombreCarpetaSalida)
        {
            var porCarpetaOrigen = movimientos.GroupBy(m => (m.CarpetaOrigen, m.EsIlegible), m => m);

            foreach (var grupo in porCarpetaOrigen)
            {
                string carpetaOrigen = grupo.Key.CarpetaOrigen;
                bool esIlegible = grupo.Key.EsIlegible;

                string nombreCsvOrigen = ObtenerNombreCsvOrigen(carpetaOrigen, esIlegible);
                string rutaCsvOrigen = Path.Combine(carpetaOrigen, nombreCsvOrigen);

                if (!File.Exists(rutaCsvOrigen))
                    throw new FileNotFoundException($"No se encontró el CSV de índice de origen: {rutaCsvOrigen}");

                var nombresAMover = new HashSet<string>(
                    grupo.Select(m => m.NombreArchivoFinal), StringComparer.OrdinalIgnoreCase);

                var todasLasLineas = File.ReadAllLines(rutaCsvOrigen).ToList();
                if (todasLasLineas.Count == 0)
                    continue;

                string encabezado = todasLasLineas[0];
                var lineasRestantes = new List<string>();
                var lineasAMover = new List<string>();

                for (int i = 1; i < todasLasLineas.Count; i++)
                {
                    string linea = todasLasLineas[i];
                    if (string.IsNullOrWhiteSpace(linea))
                        continue;

                    string nombreNuevo = ObtenerCampoNombreNuevo(linea);

                    if (!string.IsNullOrEmpty(nombreNuevo) && nombresAMover.Contains(nombreNuevo))
                    {
                        lineasAMover.Add(linea);
                    }
                    else
                    {
                        lineasRestantes.Add(linea);
                    }
                }

                if (lineasAMover.Count == 0)
                    continue;

                // Reescribe el CSV de origen sin los registros movidos
                var lineasFinalesOrigen = new List<string> { encabezado };
                lineasFinalesOrigen.AddRange(lineasRestantes);
                File.WriteAllLines(rutaCsvOrigen, lineasFinalesOrigen, System.Text.Encoding.UTF8);

                // Agrega los registros movidos al CSV de destino (carpeta de envío)
                string carpetaDestino = grupo.First().CarpetaDestino;
                string nombreCsvDestino = $"{nombreCarpetaSalida}.csv";
                string rutaCsvDestino = Path.Combine(carpetaDestino, nombreCsvDestino);
                bool destinoExiste = File.Exists(rutaCsvDestino);

                using var writer = new StreamWriter(rutaCsvDestino, append: true, System.Text.Encoding.UTF8);

                if (!destinoExiste)
                    writer.WriteLine(encabezado);

                foreach (var linea in lineasAMover)
                    writer.WriteLine(linea);
            }
        }

        /// <summary>
        /// Extrae el campo "Nombre Nuevo" (tercera columna, separada por ';') de una línea
        /// del CSV de índice, respetando comillas si el campo está entre comillas.
        /// </summary>
        private static string ObtenerCampoNombreNuevo(string linea)
        {
            var campos = DividirLineaCsv(linea);
            return campos.Count > 2 ? campos[2] : string.Empty;
        }

        private static List<string> DividirLineaCsv(string linea)
        {
            var campos = new List<string>();
            bool dentroDeComillas = false;
            var actual = new System.Text.StringBuilder();

            for (int i = 0; i < linea.Length; i++)
            {
                char c = linea[i];

                if (c == '"')
                {
                    dentroDeComillas = !dentroDeComillas;
                }
                else if (c == ';' && !dentroDeComillas)
                {
                    campos.Add(actual.ToString());
                    actual.Clear();
                }
                else
                {
                    actual.Append(c);
                }
            }

            campos.Add(actual.ToString());
            return campos;
        }

        /// <summary>
        /// Valida que la cantidad de archivos PDF presentes en cada carpeta de destino
        /// (Procesados/Ilegibles) coincida con la cantidad de registros de datos (sin
        /// contar encabezado) del CSV de índice ubicado en esa misma carpeta. Devuelve una
        /// lista de mensajes describiendo las discrepancias encontradas (vacía si todo coincide).
        /// </summary>
        public List<string> ValidarCantidadArchivosVsCsv(IEnumerable<string> carpetasDestino)
        {
            var discrepancias = new List<string>();

            foreach (var carpeta in carpetasDestino.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (!Directory.Exists(carpeta))
                    continue;

                int cantidadPdf = Directory.GetFiles(carpeta, "*.pdf").Length;

                int cantidadRegistrosCsv = 0;
                foreach (var rutaCsv in Directory.GetFiles(carpeta, "*.csv"))
                {
                    int cantidadLineas = File.ReadAllLines(rutaCsv).Count(l => !string.IsNullOrWhiteSpace(l));
                    cantidadRegistrosCsv += Math.Max(cantidadLineas - 1, 0);
                }

                if (cantidadPdf != cantidadRegistrosCsv)
                {
                    discrepancias.Add(
                        $"La carpeta '{carpeta}' tiene {cantidadPdf} archivo(s) PDF pero " +
                        $"{cantidadRegistrosCsv} registro(s) en el/los CSV de índice.");
                }
            }

            return discrepancias;
        }

        /// <summary>
        /// Marca el lote (y todas sus páginas) como "Enviado".
        /// </summary>
        public void MarcarLoteEnviado(int cdLote, int cdUsuario, string dsCarpetaEnvio, string? dsObservacionesEnvio)
        {
            _loteDAL.MarcarLoteEnviado(cdLote, cdUsuario, dsCarpetaEnvio, dsObservacionesEnvio);
            _archivoPaginaDAL.ActualizarEstadoPorLote(cdLote, EstadoPaginaEnviada);
        }

        /// <summary>
        /// Orquesta el proceso completo de envío de un lote:
        /// 1) Mueve los PDFs finales hacia la carpeta de envío.
        /// 2) Mueve solo los registros CSV correspondientes a ese lote.
        /// 3) Valida que la cantidad de PDFs y de registros CSV coincidan en las carpetas
        ///    de destino. Si NO coinciden, el lote NO se marca como enviado.
        /// 4) Si todo coincide, marca el lote (y sus páginas) como "Enviado".
        /// Devuelve la lista de discrepancias PDF/CSV detectadas (vacía si todo salió bien).
        /// </summary>
        public List<string> EnviarLote(int cdLote, List<FilaEnvio> filas, int cdUsuario,
            string carpetaEnvioBase, string dsCarpetaEnvio, string? dsObservacionesEnvio)
        {
            var movimientos = MoverArchivos(filas, carpetaEnvioBase);
            MoverRegistrosCsv(movimientos, dsCarpetaEnvio);

            var carpetasDestino = movimientos.Select(m => m.CarpetaDestino);
            var discrepancias = ValidarCantidadArchivosVsCsv(carpetasDestino);

            if (discrepancias.Count > 0)
                return discrepancias;

            MarcarLoteEnviado(cdLote, cdUsuario, dsCarpetaEnvio, dsObservacionesEnvio);

            return discrepancias;
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
                DsMensaje = $"Error en envío de lote: {ex.Message}",
                DsExcepcion = ex.ToString(),
                CdUsuario = cdUsuario
            });
        }
    }
}
