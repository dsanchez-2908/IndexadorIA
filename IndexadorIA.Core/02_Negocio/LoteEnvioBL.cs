using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Utilidades;

namespace IndexadorIA.Negocio
{
    /// <summary>
    /// Lógica de negocio para el envío de lotes finalizados (pantalla "Envio"):
    /// copia los PDFs finales y unifica los registros de los CSV de índice correspondientes
    /// SOLO a los lotes seleccionados hacia la carpeta de envío final, y marca los
    /// lotes (y sus páginas) como "Enviado". Los archivos originales en "Terminado\{cdLote}"
    /// NO se eliminan (se copian, no se mueven).
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
        /// el PDF final de una página, dentro de la carpeta aislada del lote generada en la
        /// finalización: dirname(dsRutaCompleta DEL ARCHIVO ORIGINAL) + "\Terminado\{cdLote}\"
        /// + "Planos Procesados" o "Planos ILEGIBLES".
        /// </summary>
        private static string ObtenerCarpetaOrigen(ArchivoPagina archivo, bool esIlegible, int cdLote)
        {
            string carpetaBase = Path.GetDirectoryName(archivo.DsRutaCompletaOriginal) ?? string.Empty;
            return Path.Combine(LoteFinalizacionBL.ObtenerCarpetaLote(carpetaBase, cdLote),
                esIlegible ? CarpetaIlegibles : CarpetaProcesados);
        }

        /// <summary>
        /// Obtiene el valor de dsEntrega comun a todas las filas del lote (todas las filas
        /// de un mismo lote comparten el mismo valor). Devuelve cadena vacia si no hay filas
        /// o el valor no esta informado.
        /// </summary>
        public static string ObtenerEntregaDelLote(List<FilaEnvio> filas)
        {
            return filas
                .Select(f => f.Archivo.DsEntrega)
                .FirstOrDefault(e => !string.IsNullOrWhiteSpace(e)) ?? string.Empty;
        }

        /// <summary>
        /// Valida (sin tocar disco ni base de datos) que un lote este en condiciones de
        /// enviarse correctamente: que tenga filas, que todas tengan dsEntrega y
        /// dsNombreArchivoFinal informados, que el PDF fisico de origen exista, y que el
        /// CSV de indice de origen (generado en la finalizacion) exista. Pensada para
        /// ejecutarse ANTES de procesar el envio real de una tanda grande de lotes, de
        /// forma de detectar de una sola pasada todos los problemas y evitar arrancar un
        /// proceso largo (copia de miles de archivos) que termine fallando a la mitad.
        /// </summary>
        public List<string> ValidarLotePrevioEnvio(int cdLote, List<FilaEnvio> filas)
        {
            var errores = new List<string>();

            if (filas.Count == 0)
            {
                errores.Add($"Lote {cdLote}: no tiene registros para procesar.");
                return errores;
            }

            string dsEntrega = ObtenerEntregaDelLote(filas);
            if (string.IsNullOrWhiteSpace(dsEntrega))
            {
                errores.Add($"Lote {cdLote}: no tiene dsEntrega informado en ninguna de sus filas.");
            }

            var carpetasCsvValidadas = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var fila in filas)
            {
                string nombreArchivoFinal = fila.Archivo.DsNombreArchivoFinal ?? string.Empty;

                if (string.IsNullOrWhiteSpace(nombreArchivoFinal))
                {
                    errores.Add(
                        $"Lote {cdLote}: el archivo/pagina {fila.Archivo.CdArchivoPagina} no tiene " +
                        "nombre de archivo final registrado (dsNombreArchivoFinal).");
                    continue;
                }

                bool esIlegible = EsIlegible(fila.Resultado);
                string carpetaOrigen = ObtenerCarpetaOrigen(fila.Archivo, esIlegible, cdLote);
                string rutaOrigen = Path.Combine(carpetaOrigen, nombreArchivoFinal);

                if (!File.Exists(rutaOrigen))
                {
                    errores.Add(
                        $"Lote {cdLote}: no se encontro el archivo fisico del archivo/pagina " +
                        $"{fila.Archivo.CdArchivoPagina}: {rutaOrigen}");
                }

                if (carpetasCsvValidadas.Add(carpetaOrigen))
                {
                    string nombreCsvOrigen = ObtenerNombreCsvOrigen(carpetaOrigen, esIlegible);
                    string rutaCsvOrigen = Path.Combine(carpetaOrigen, nombreCsvOrigen);

                    if (!File.Exists(rutaCsvOrigen))
                    {
                        errores.Add(
                            $"Lote {cdLote}: no se encontro el CSV de indice de origen: {rutaCsvOrigen}");
                    }
                }
            }

            return errores;
        }

        /// <summary>
        /// Arma la carpeta de la Entrega dentro de la carpeta de envío base
        /// (txtRutaSalida\txtNombreCarpetaSalida\{dsEntrega}).
        /// </summary>
        public static string ObtenerCarpetaEntrega(string carpetaEnvioBase, string dsEntrega)
        {
            return Path.Combine(carpetaEnvioBase, dsEntrega);
        }

        /// <summary>
        /// Resultado de mover un archivo/registro de un lote hacia la carpeta de envío.
        /// </summary>
        public class RegistroEnvio
        {
            public int CdArchivoPagina { get; set; }
            public string CarpetaOrigen { get; set; } = string.Empty;
            public string CarpetaDestino { get; set; } = string.Empty;

            /// <summary>
            /// Nombre de archivo tal como esta persistido en dsNombreArchivoFinal (y como
            /// figura en el CSV de indice de origen), previo a resolver colisiones en el
            /// destino de envio.
            /// </summary>
            public string NombreArchivoOriginal { get; set; } = string.Empty;

            /// <summary>
            /// Nombre de archivo final efectivamente usado en la carpeta de destino de
            /// envio, luego de resolver colisiones (puede tener sufijo "(1)", "(2)", etc.).
            /// </summary>
            public string NombreArchivoFinal { get; set; } = string.Empty;
            public bool EsIlegible { get; set; }
        }

        /// <summary>
        /// Copia (y renombra si corresponde) los PDFs finales de las filas indicadas
        /// (localizados en "Terminado\{cdLote}\Planos Procesados"/"Planos ILEGIBLES", con
        /// el nombre persistido en dsNombreArchivoFinal) hacia la carpeta de envio de la
        /// Entrega correspondiente. No mueve: copia el archivo fisico, dejando intacto el
        /// original en "Terminado\{cdLote}\..." hasta confirmar que el envio es correcto.
        /// </summary>
        public List<RegistroEnvio> MoverArchivos(List<FilaEnvio> filas, string carpetaEntrega, int cdLote)
        {
            var movimientos = new List<RegistroEnvio>();

            foreach (var fila in filas)
            {
                string nombreArchivoFinal = fila.Archivo.DsNombreArchivoFinal ?? string.Empty;

                if (string.IsNullOrWhiteSpace(nombreArchivoFinal))
                    throw new InvalidOperationException(
                        $"El archivo/página {fila.Archivo.CdArchivoPagina} no tiene nombre de archivo final registrado (dsNombreArchivoFinal).");

                bool esIlegible = EsIlegible(fila.Resultado);
                string carpetaOrigen = ObtenerCarpetaOrigen(fila.Archivo, esIlegible, cdLote);
                string rutaOrigen = Path.Combine(carpetaOrigen, nombreArchivoFinal);

                if (!File.Exists(rutaOrigen))
                    throw new FileNotFoundException(
                        $"No se encontró el archivo físico del archivo/página {fila.Archivo.CdArchivoPagina}: {rutaOrigen}");

                string carpetaDestino = Path.Combine(carpetaEntrega, esIlegible ? CarpetaIlegibles : CarpetaProcesados);

                if (!Directory.Exists(carpetaDestino))
                    Directory.CreateDirectory(carpetaDestino);

                string rutaDestino = Path.Combine(carpetaDestino, nombreArchivoFinal);
                string nombreArchivoDestino = nombreArchivoFinal;

                if (!string.Equals(rutaOrigen, rutaDestino, StringComparison.OrdinalIgnoreCase))
                {
                    // Varios lotes (en especial los ilegibles, cuyo nombre suele ser generico)
                    // pueden terminar aportando archivos con el mismo nombre a una misma
                    // carpeta de Entrega. Se resuelve la colision agregando (1), (2), (3)...,
                    // igual que en la finalizacion, para no fallar el envio.
                    nombreArchivoDestino = GeneradorNombreArchivo.ResolverColision(carpetaDestino, nombreArchivoFinal);
                    rutaDestino = Path.Combine(carpetaDestino, nombreArchivoDestino);

                    File.Copy(rutaOrigen, rutaDestino);
                }

                movimientos.Add(new RegistroEnvio
                {
                    CdArchivoPagina = fila.Archivo.CdArchivoPagina,
                    CarpetaOrigen = carpetaOrigen,
                    CarpetaDestino = carpetaDestino,
                    NombreArchivoOriginal = nombreArchivoFinal,
                    NombreArchivoFinal = nombreArchivoDestino,
                    EsIlegible = esIlegible
                });
            }

            return movimientos;
        }

        /// <summary>
        /// Nombre del CSV de indice ubicado en la carpeta de origen (misma carpeta donde
        /// estan los PDFs "Planos Procesados"/"Planos ILEGIBLES", dentro de
        /// "Terminado\{cdLote}"), generado durante la finalizacion del lote:
        /// "[carpeta entrega].csv" o "[carpeta entrega]_ILEGIBLE.csv". El nombre se basa en
        /// la carpeta base de la entrega (dos niveles arriba de la carpeta de origen: sube
        /// de "Planos Procesados/ILEGIBLES" a "Terminado\{cdLote}" y de ahi a la carpeta base).
        /// </summary>
        private static string ObtenerNombreCsvOrigen(string carpetaOrigen, bool esIlegible)
        {
            var directorioOrigen = new DirectoryInfo(carpetaOrigen);
            var carpetaLote = directorioOrigen.Parent; // Terminado\{cdLote}
            var carpetaTerminado = carpetaLote?.Parent; // Terminado
            var carpetaBase = carpetaTerminado?.Parent; // carpeta base de la entrega

            string nombreCarpetaBase = carpetaBase != null ? carpetaBase.Name : directorioOrigen.Name;

            return esIlegible ? $"{nombreCarpetaBase}_ILEGIBLE.csv" : $"{nombreCarpetaBase}.csv";
        }

        /// <summary>
        /// Mueve (fusiona) TODOS los registros del CSV de indice del lote (ubicado en
        /// "Terminado\{cdLote}\Planos Procesados"/"Planos ILEGIBLES") hacia el CSV
        /// consolidado de la carpeta de Entrega correspondiente. Como el CSV de origen es
        /// exclusivo de ese lote (ya no se comparte entre lotes), se fusiona completo.
        /// El CSV consolidado se nombra "{nombreCarpetaSalida}.csv" (Procesados) o
        /// "{nombreCarpetaSalida}-ILEGIBLES.csv" (Ilegibles).
        /// </summary>
        public void MoverRegistrosCsv(List<RegistroEnvio> movimientos, string nombreCarpetaSalida)
        {
            var porCarpetaOrigen = movimientos.GroupBy(m => (m.CarpetaOrigen, m.CarpetaDestino, m.EsIlegible), m => m);

            foreach (var grupo in porCarpetaOrigen)
            {
                string carpetaOrigen = grupo.Key.CarpetaOrigen;
                bool esIlegible = grupo.Key.EsIlegible;

                string nombreCsvOrigen = ObtenerNombreCsvOrigen(carpetaOrigen, esIlegible);
                string rutaCsvOrigen = Path.Combine(carpetaOrigen, nombreCsvOrigen);

                if (!File.Exists(rutaCsvOrigen))
                    throw new FileNotFoundException($"No se encontró el CSV de índice de origen: {rutaCsvOrigen}");

                var todasLasLineas = File.ReadAllLines(rutaCsvOrigen).ToList();
                if (todasLasLineas.Count == 0)
                    continue;

                string encabezado = todasLasLineas[0];
                var lineasDatos = todasLasLineas.Skip(1).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

                if (lineasDatos.Count == 0)
                    continue;

                // Si alguno de los archivos de este grupo debio renombrarse por colision
                // ((1), (2), (3)... en el destino de envio), se refleja el mismo nombre
                // final en la columna "Nombre Nuevo" del CSV consolidado.
                var renombres = grupo
                    .Where(m => !string.Equals(m.NombreArchivoOriginal, m.NombreArchivoFinal, StringComparison.Ordinal))
                    .GroupBy(m => m.NombreArchivoOriginal)
                    .ToDictionary(g => g.Key, g => g.First().NombreArchivoFinal, StringComparer.Ordinal);

                if (renombres.Count > 0)
                {
                    lineasDatos = lineasDatos
                        .Select(linea => AplicarRenombreEnLineaCsv(linea, renombres))
                        .ToList();
                }

                string carpetaDestino = grupo.Key.CarpetaDestino;
                string nombreCsvDestino = esIlegible
                    ? $"{nombreCarpetaSalida}-ILEGIBLES.csv"
                    : $"{nombreCarpetaSalida}.csv";

                // La carpeta destino es "{carpetaEntrega}\Planos Procesados"/"...ILEGIBLES";
                // el CSV consolidado se ubica un nivel arriba, directo en la carpeta de Entrega.
                string carpetaEntrega = new DirectoryInfo(carpetaDestino).Parent?.FullName ?? carpetaDestino;
                string rutaCsvDestino = Path.Combine(carpetaEntrega, nombreCsvDestino);
                bool destinoExiste = File.Exists(rutaCsvDestino);

                using var writer = new StreamWriter(rutaCsvDestino, append: true, System.Text.Encoding.UTF8);

                if (!destinoExiste)
                    writer.WriteLine(encabezado);

                foreach (var linea in lineasDatos)
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

        /// <summary>
        /// Si la línea del CSV de índice referencia (columna "Nombre Nuevo") a un archivo
        /// que debió renombrarse por colisión en el destino de envío, reescribe esa
        /// columna con el nombre final efectivamente usado, dejando el resto de las
        /// columnas intactas.
        /// </summary>
        private static string AplicarRenombreEnLineaCsv(string linea, Dictionary<string, string> renombres)
        {
            string nombreNuevo = ObtenerCampoNombreNuevo(linea);

            if (string.IsNullOrEmpty(nombreNuevo) || !renombres.TryGetValue(nombreNuevo, out string? nombreFinal))
                return linea;

            var campos = DividirLineaCsv(linea);
            if (campos.Count <= 2)
                return linea;

            campos[2] = EscaparCampoCsv(nombreFinal);

            return string.Join(";", campos);
        }

        private static string EscaparCampoCsv(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            if (valor.Contains(';') || valor.Contains('"'))
                return "\"" + valor.Replace("\"", "\"\"") + "\"";

            return valor;
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
        /// contar encabezado) del CSV consolidado de índice ubicado en la carpeta de Entrega
        /// (un nivel arriba de la carpeta de PDFs), y que el nombre de cada PDF físico
        /// coincida exactamente con alguno de los nombres registrados en la columna
        /// "Nombre Nuevo" del CSV. Devuelve una lista de mensajes describiendo las
        /// discrepancias encontradas (vacía si todo coincide).
        /// </summary>
        public List<string> ValidarCantidadArchivosVsCsv(IEnumerable<(string CarpetaPdfs, string RutaCsv)> carpetasDestino)
        {
            var discrepancias = new List<string>();

            foreach (var item in carpetasDestino.Distinct())
            {
                string carpeta = item.CarpetaPdfs;
                string rutaCsv = item.RutaCsv;

                if (!Directory.Exists(carpeta))
                    continue;

                var archivosPdf = Directory.GetFiles(carpeta, "*.pdf")
                    .Select(Path.GetFileName)
                    .Where(n => !string.IsNullOrEmpty(n))
                    .Select(n => n!)
                    .ToList();

                var nombresCsv = new List<string>();
                if (File.Exists(rutaCsv))
                {
                    var lineasDatos = File.ReadAllLines(rutaCsv)
                        .Where(l => !string.IsNullOrWhiteSpace(l))
                        .Skip(1)
                        .ToList();

                    nombresCsv = lineasDatos
                        .Select(ObtenerCampoNombreNuevo)
                        .Where(n => !string.IsNullOrWhiteSpace(n))
                        .ToList();
                }

                if (archivosPdf.Count != nombresCsv.Count)
                {
                    discrepancias.Add(
                        $"La carpeta '{carpeta}' tiene {archivosPdf.Count} archivo(s) PDF pero " +
                        $"{nombresCsv.Count} registro(s) en el CSV de índice '{rutaCsv}'.");
                }

                var soloEnCarpeta = archivosPdf.Except(nombresCsv, StringComparer.OrdinalIgnoreCase).ToList();
                var soloEnCsv = nombresCsv.Except(archivosPdf, StringComparer.OrdinalIgnoreCase).ToList();

                if (soloEnCarpeta.Count > 0)
                {
                    discrepancias.Add(
                        $"La carpeta '{carpeta}' tiene archivo(s) PDF que no figuran en el CSV '{rutaCsv}': " +
                        string.Join(", ", soloEnCarpeta));
                }

                if (soloEnCsv.Count > 0)
                {
                    discrepancias.Add(
                        $"El CSV de índice '{rutaCsv}' tiene registro(s) cuyo archivo PDF no se encuentra en la carpeta '{carpeta}': " +
                        string.Join(", ", soloEnCsv));
                }
            }

            return discrepancias;
        }

        /// <summary>
        /// Escribe (o agrega, si ya existe) un informe en texto plano con las
        /// discrepancias detectadas entre los PDFs físicos y el CSV de índice de una
        /// carpeta de Entrega. El archivo se ubica directamente en la carpeta de envío
        /// base (txtRutaSalida + txtNombreCarpetaSalida), con el nombre
        /// "Informe_Discrepancias.txt".
        /// </summary>
        private static void EscribirInformeDiscrepancias(string carpetaEnvioBase, string carpetaEntrega,
            List<string> discrepancias)
        {
            if (discrepancias.Count == 0)
                return;

            if (!Directory.Exists(carpetaEnvioBase))
                Directory.CreateDirectory(carpetaEnvioBase);

            string rutaInforme = Path.Combine(carpetaEnvioBase, "Informe_Discrepancias.txt");

            using var writer = new StreamWriter(rutaInforme, append: true, System.Text.Encoding.UTF8);
            writer.WriteLine($"=== {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Carpeta de Entrega: {carpetaEntrega} ===");
            foreach (var discrepancia in discrepancias)
                writer.WriteLine(discrepancia);
            writer.WriteLine();
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
            string dsEntrega = ObtenerEntregaDelLote(filas);
            string carpetaEntrega = ObtenerCarpetaEntrega(carpetaEnvioBase, dsEntrega);

            var movimientos = MoverArchivos(filas, carpetaEntrega, cdLote);
            MoverRegistrosCsv(movimientos, dsCarpetaEnvio);

            var carpetasDestino = movimientos
                .Select(m => (
                    CarpetaPdfs: m.CarpetaDestino,
                    RutaCsv: Path.Combine(carpetaEntrega, m.EsIlegible
                        ? $"{dsCarpetaEnvio}-ILEGIBLES.csv"
                        : $"{dsCarpetaEnvio}.csv")))
                .Distinct();
            var discrepancias = ValidarCantidadArchivosVsCsv(carpetasDestino);

            if (discrepancias.Count > 0)
            {
                EscribirInformeDiscrepancias(carpetaEnvioBase, carpetaEntrega, discrepancias);
                return discrepancias;
            }

            RegistrarNombresArchivoFinalEnviado(movimientos);
            MarcarLoteEnviado(cdLote, cdUsuario, dsCarpetaEnvio, dsObservacionesEnvio);

            return discrepancias;
        }

        /// <summary>
        /// Persiste en TD_ARCHIVOS_PAGINAS.dsNombreArchivoFinalEnviado el nombre de PDF
        /// efectivamente entregado en la carpeta de destino de envio. Se registra null
        /// cuando no hubo colision (el nombre enviado coincide con dsNombreArchivoFinal),
        /// y el nombre con sufijo "(1)", "(2)", etc. cuando hubo que renombrar por colision,
        /// de forma de no dejar desactualizado el nombre realmente enviado si el lote se
        /// reenvia o se consulta mas adelante.
        /// </summary>
        private void RegistrarNombresArchivoFinalEnviado(List<RegistroEnvio> movimientos)
        {
            foreach (var movimiento in movimientos)
            {
                string? nombreEnviado = string.Equals(
                    movimiento.NombreArchivoOriginal, movimiento.NombreArchivoFinal, StringComparison.OrdinalIgnoreCase)
                    ? null
                    : movimiento.NombreArchivoFinal;

                _archivoPaginaDAL.ActualizarNombreArchivoFinalEnviado(movimiento.CdArchivoPagina, nombreEnviado);
            }
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
