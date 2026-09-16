using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Utilidades;
using System.Linq;

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
        /// Estado de lote "Finalizado" / "Pendiente de Enviar" (dsProceso = 'LOTE' en TD_ESTADOS)
        /// </summary>
        public const int EstadoLoteFinalizado = 10;

        /// <summary>
        /// Estado de lote "Pendiente Asignar Auditoria" (dsProceso = 'LOTE' en TD_ESTADOS),
        /// utilizado al completar el control (MarcarLotePendienteFinalizar) para habilitar la
        /// asignacion de auditoria del lote.
        /// </summary>
        public const int EstadoLotePendienteFinalizar = 7;

        /// <summary>
        /// Estado de lote "Pendiente de Finalizar" (dsProceso = 'LOTE' en TD_ESTADOS), consumido
        /// por la pantalla FrmFinalizarLote una vez completada la auditoria del lote.
        /// </summary>
        public const int EstadoLotePendienteFinalizarAuditado = 9;

        /// <summary>
        /// Nombre de la subcarpeta donde se generan los archivos originales separados
        /// (creada por la pantalla de Separación de Imágenes).
        /// </summary>
        public const string CarpetaOriginales = "Planos Originales";

        /// <summary>
        /// Nombre de la subcarpeta donde se mueven los archivos finalizados correctamente.
        /// </summary>
        public const string CarpetaProcesados = "Planos Procesados";

        /// <summary>
        /// Nombre de la subcarpeta donde se mueven los archivos ilegibles.
        /// </summary>
        public const string CarpetaIlegibles = "Planos ILEGIBLES";

        /// <summary>
        /// Longitud maxima permitida para el nombre de archivo PDF final (sin ruta).
        /// </summary>
        public const int MaxLongitudNombreArchivo = 260;

        /// <summary>
        /// Texto agregado a las observaciones cuando el nombre final del PDF debio
        /// truncarse por exceder MaxLongitudNombreArchivo.
        /// </summary>
        public const string ObservacionNombreIncompleto = "NOMBRE DE ARCHIVO INCOMPLETO";

        /// <summary>
        /// Nombre (parcial, case-insensitive) de la carpeta a partir de la cual se
        /// acorta la ruta mostrada en la columna "Ruta" del CSV.
        /// </summary>
        public const string PalabraClaveCarpetaEntrega = "Entrega";

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
        /// Marca el lote con el estado "Finalizado" / "Pendiente de Enviar", dejando registro
        /// de la fecha y el usuario que finalizo el lote.
        /// </summary>
        public void MarcarLoteFinalizado(int cdLote, int cdUsuario)
        {
            _loteDAL.MarcarLoteFinalizado(cdLote, cdUsuario);
        }

        /// <summary>
        /// Marca el lote con el estado "Pendiente de Finalizar" (usado desde FrmVerLote
        /// cuando el control de todas las páginas del lote se completó), dejando registro
        /// de la fecha/hora y el usuario que finalizó el control.
        /// </summary>
        public void MarcarLotePendienteFinalizar(int cdLote, int cdUsuario)
        {
            _loteDAL.MarcarFinControl(cdLote, EstadoLotePendienteFinalizar, cdUsuario);
        }

        /// <summary>
        /// Información sobre el resultado de mover/renombrar un archivo.
        /// </summary>
        public class RegistroMovimiento
        {
            public int CdArchivoPagina { get; set; }
            public string CarpetaDestino { get; set; } = string.Empty;
            public string CarpetaBase { get; set; } = string.Empty;
            public string NombreArchivoNuevo { get; set; } = string.Empty;
            public bool EsIlegible { get; set; }
        }

        /// <summary>
        /// Renombra (estados 1/2) o mueve y renombra (estados 3/4, hacia la subcarpeta
        /// "ILEGIBLES") los archivos PDF de las filas indicadas, según las reglas de
        /// nomenclatura por categoría de plano, y persiste el nombre final en
        /// TD_ARCHIVOS_PAGINAS.dsNombreArchivoFinal.
        /// </summary>
        /// <summary>
        /// Dada la carpeta donde reside actualmente el PDF de origen (normalmente la
        /// subcarpeta "Planos Originales" creada en la separación de imágenes), obtiene
        /// la carpeta base de la entrega, es decir, la carpeta padre de "Planos Originales".
        /// Si el archivo no está dentro de una carpeta "Planos Originales", se usa la
        /// carpeta de origen como base (comportamiento de respaldo).
        /// </summary>
        private static string ObtenerCarpetaBase(string carpetaOrigen)
        {
            var directorioOrigen = new DirectoryInfo(carpetaOrigen);

            if (string.Equals(directorioOrigen.Name, CarpetaOriginales, StringComparison.OrdinalIgnoreCase)
                && directorioOrigen.Parent != null)
            {
                return directorioOrigen.Parent.FullName;
            }

            return carpetaOrigen;
        }

        /// <summary>
        /// Acorta una ruta completa para la columna "Ruta" del CSV: recorre las carpetas
        /// del path hasta encontrar una que CONTENGA la palabra "Entrega" (sin distinguir
        /// mayusculas/minusculas) y devuelve la ruta desde esa carpeta en adelante,
        /// prefijada con "\\". Si no se encuentra ninguna carpeta con esa palabra,
        /// devuelve la ruta completa original sin modificar.
        /// </summary>
        private static string AcortarRutaPorEntrega(string rutaCompleta)
        {
            if (string.IsNullOrWhiteSpace(rutaCompleta))
                return rutaCompleta;

            string[] partes = rutaCompleta.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            int indiceEntrega = -1;
            for (int i = 0; i < partes.Length; i++)
            {
                if (partes[i].Contains(PalabraClaveCarpetaEntrega, StringComparison.OrdinalIgnoreCase))
                {
                    indiceEntrega = i;
                    break;
                }
            }

            if (indiceEntrega < 0)
                return rutaCompleta;

            string resto = string.Join(Path.DirectorySeparatorChar, partes.Skip(indiceEntrega));
            return "\\\\" + resto;
        }

        /// <summary>
        /// Nota: los archivos temporales de trabajo (.jpg y .b64) generados durante la
        /// separacion/preparacion de imagenes en la subcarpeta "Planos Originales" ya NO se
        /// eliminan durante la finalizacion del lote (se conservan por pedido del usuario).
        /// </summary>

        /// <summary>
        /// Longitud maxima permitida para la ruta completa (carpeta + nombre de archivo)
        /// del PDF final, para evitar errores de Windows por exceder el limite de ruta.
        /// </summary>
        public const int MaxLongitudRutaCompleta = 259;

        /// <summary>
        /// Prefijo de la subcarpeta temporal de trabajo usada durante la finalizacion de un
        /// lote, para aislar sus PDFs y su CSV de los de otros lotes hasta que se valide que
        /// coinciden entre si. Se combina con el numero de lote, por ejemplo "cdLote123".
        /// </summary>
        public const string PrefijoCarpetaTemporalLote = "cdLote";

        /// <summary>
        /// Devuelve el nombre de la subcarpeta temporal de trabajo para el lote indicado.
        /// </summary>
        public static string ObtenerNombreCarpetaTemporalLote(int cdLote) => PrefijoCarpetaTemporalLote + cdLote;

        public List<RegistroMovimiento> MoverYRenombrarArchivos(List<FilaFinalizacion> filas, int cdLote)
        {
            var movimientos = new List<RegistroMovimiento>();
            var archivosCopiados = new List<string>();

            // Actualizaciones de BD (nombre final / observacion) que solo se aplican una vez
            // que TODOS los archivos del lote se copiaron correctamente, para evitar dejar
            // el registro apuntando a un archivo final que en realidad no llego a existir
            // si el proceso se corta a mitad de camino.
            var actualizacionesPendientes = new List<(int CdArchivoPagina, string NombreNuevo, bool NombreIncompleto)>();

            try
            {
                foreach (var fila in filas)
                {
                    string rutaOrigen = fila.Archivo.DsRutaCompleta;

                    if (string.IsNullOrWhiteSpace(rutaOrigen) || !File.Exists(rutaOrigen))
                        throw new FileNotFoundException($"No se encontró el archivo físico: {rutaOrigen}");

                    string carpetaOrigen = Path.GetDirectoryName(rutaOrigen) ?? string.Empty;
                    string carpetaBase = ObtenerCarpetaBase(carpetaOrigen);
                    bool esIlegible = EsIlegible(fila.Resultado);

                    string carpetaDestino = Path.Combine(carpetaBase, esIlegible ? CarpetaIlegibles : CarpetaProcesados,
                        ObtenerNombreCarpetaTemporalLote(cdLote));

                    if (!Directory.Exists(carpetaDestino))
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

                    // Sanea el nombre para que sea un nombre de archivo valido en Windows
                    // (elimina caracteres invalidos y espacios/puntos finales que provocan
                    // errores de "sintaxis del nombre no correcta").
                    nombreNuevo = GeneradorNombreArchivo.SanearNombreArchivo(nombreNuevo);

                    bool nombreIncompleto = false;

                    // Limite de longitud del nombre en si mismo
                    if (nombreNuevo.Length > MaxLongitudNombreArchivo)
                    {
                        string extension = Path.GetExtension(nombreNuevo);
                        nombreNuevo = nombreNuevo.Substring(0, MaxLongitudNombreArchivo - extension.Length) + extension;
                        nombreIncompleto = true;
                    }

                    // Limite de longitud de la ruta completa (carpeta destino + nombre)
                    int longitudMaximaNombrePorRuta = MaxLongitudRutaCompleta - carpetaDestino.Length - 1;
                    if (nombreNuevo.Length > longitudMaximaNombrePorRuta && longitudMaximaNombrePorRuta > 0)
                    {
                        string extension = Path.GetExtension(nombreNuevo);
                        int longitudDisponible = Math.Max(longitudMaximaNombrePorRuta - extension.Length, 1);
                        nombreNuevo = nombreNuevo.Substring(0, longitudDisponible) + extension;
                        nombreIncompleto = true;
                    }

                    nombreNuevo = GeneradorNombreArchivo.SanearNombreArchivo(nombreNuevo);
                    nombreNuevo = GeneradorNombreArchivo.ResolverColision(carpetaDestino, nombreNuevo);

                    string rutaDestino = Path.Combine(carpetaDestino, nombreNuevo);

                    if (!string.Equals(rutaOrigen, rutaDestino, StringComparison.OrdinalIgnoreCase))
                    {
                        File.Copy(rutaOrigen, rutaDestino, overwrite: false);
                        archivosCopiados.Add(rutaDestino);
                    }

                    actualizacionesPendientes.Add((fila.Archivo.CdArchivoPagina, nombreNuevo, nombreIncompleto));

                    movimientos.Add(new RegistroMovimiento
                    {
                        CdArchivoPagina = fila.Archivo.CdArchivoPagina,
                        CarpetaDestino = carpetaDestino,
                        CarpetaBase = carpetaBase,
                        NombreArchivoNuevo = nombreNuevo,
                        EsIlegible = esIlegible
                    });
                }
            }
            catch
            {
                // Rollback: borra los archivos que llegaron a copiarse en esta ejecucion
                // antes de que se produjera el error, para no dejar el destino a medio
                // procesar ni archivos duplicados/huerfanos.
                foreach (var rutaCopiada in archivosCopiados)
                {
                    try
                    {
                        if (File.Exists(rutaCopiada))
                            File.Delete(rutaCopiada);
                    }
                    catch
                    {
                        // Si no se puede borrar (archivo bloqueado, etc.) se ignora: el
                        // error original ya sera informado al usuario/log.
                    }
                }

                throw;
            }

            // Solo si TODOS los archivos se copiaron correctamente se persisten los cambios
            // en TD_ARCHIVOS_PAGINAS / TD_001_RESULTADO_IA.
            foreach (var actualizacion in actualizacionesPendientes)
            {
                _loteDAL.ActualizarNombreArchivoFinal(actualizacion.CdArchivoPagina, actualizacion.NombreNuevo);

                if (actualizacion.NombreIncompleto)
                    _loteDAL.AgregarObservacion(actualizacion.CdArchivoPagina, ObservacionNombreIncompleto);
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

            EscribirCsvPorCarpeta(correctas, esIlegible: false, cdLote);
            EscribirCsvPorCarpeta(ilegibles, esIlegible: true, cdLote);
        }

        private void EscribirCsvPorCarpeta(List<ResultadoIAMetadataDto> filas, bool esIlegible, int cdLote)
        {
            var porCarpeta = filas
                .Where(f => !string.IsNullOrWhiteSpace(f.DsRutaCompleta))
                .GroupBy(f => ObtenerCarpetaBase(Path.GetDirectoryName(f.DsRutaCompleta) ?? string.Empty));

            foreach (var grupo in porCarpeta)
            {
                string carpetaBase = grupo.Key;
                string nombreCarpetaBase = new DirectoryInfo(carpetaBase).Name;

                string carpetaCsv = Path.Combine(carpetaBase, esIlegible ? CarpetaIlegibles : CarpetaProcesados,
                    ObtenerNombreCarpetaTemporalLote(cdLote));

                if (!Directory.Exists(carpetaCsv))
                    Directory.CreateDirectory(carpetaCsv);

                string nombreCsv = esIlegible
                    ? $"{nombreCarpetaBase}_ILEGIBLE.csv"
                    : $"{nombreCarpetaBase}.csv";

                string rutaCsv = Path.Combine(carpetaCsv, nombreCsv);
                bool archivoExiste = File.Exists(rutaCsv);

                using var writer = new StreamWriter(rutaCsv, append: true, System.Text.Encoding.UTF8);

                if (!archivoExiste)
                    writer.WriteLine(string.Join(";", EncabezadosCsv));

                foreach (var f in grupo)
                {
                    bool esCatastro = GeneradorNombreArchivo.EsCategoriaCatastro(f.DsCategoriaPlano);

                    var campos = new[]
                    {
                        EscaparCampoCsv(AcortarRutaPorEntrega(f.DsRutaCompleta)),
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
                        EscaparCampoCsv(f.DsObservaciones ?? string.Empty)
                    };

                    writer.WriteLine(string.Join(";", campos));
                }
            }
        }

        private static string EscaparCampoCsv(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            // Los saltos de linea embebidos en un campo (por ejemplo en Direccion u
            // Observaciones) rompen el supuesto de "una fila = una linea" que usa el resto
            // del proceso (lectura con File.ReadAllLines, comparacion PDF vs CSV, union de
            // CSV de varios lotes), generando filas cortadas o en blanco en el CSV final.
            // Se reemplazan por espacio para evitar el problema en origen.
            if (valor.Contains('\n') || valor.Contains('\r'))
                valor = valor.Replace("\r\n", " ").Replace('\n', ' ').Replace('\r', ' ');

            if (valor.Contains(';') || valor.Contains('"'))
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
        /// <summary>
        /// Valida que la cantidad de archivos PDF presentes en cada carpeta destino
        /// (Procesados/Ilegibles) coincida con la cantidad de registros de datos (sin contar
        /// encabezado) del/de los CSV de indice ubicados en esa misma carpeta. Devuelve una
        /// lista de mensajes describiendo las discrepancias encontradas (vacia si todo coincide).
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
                    int cantidadLineas = File.ReadAllLines(rutaCsv).Length;
                    cantidadRegistrosCsv += Math.Max(cantidadLineas - 1, 0);
                }

                if (cantidadPdf != cantidadRegistrosCsv)
                {
                    discrepancias.Add(
                        $"La carpeta '{carpeta}' tiene {cantidadPdf} archivo(s) PDF pero " +
                        $"{cantidadRegistrosCsv} registro(s) en el/los CSV de indice.");
                }
            }

            return discrepancias;
        }

        /// <summary>
        /// Cantidad maxima de nombres de archivo a listar en el detalle de una discrepancia,
        /// para no generar mensajes de error excesivamente largos.
        /// </summary>
        /// <summary>
        /// Parsea una linea de CSV respetando campos entre comillas dobles (que pueden
        /// contener el separador ';' o comillas escapadas como ""), tal como los escribe
        /// EscaparCampoCsv. Un Split(';') simple corta mal estos campos (por ejemplo la
        /// columna "Ruta", que suele contener ';' dentro de nombres de calles/direcciones),
        /// desplazando el resto de las columnas y produciendo falsos positivos al comparar
        /// PDF vs CSV.
        /// </summary>
        private static string[] ParsearLineaCsv(string linea)
        {
            var campos = new List<string>();
            var actual = new System.Text.StringBuilder();
            bool dentroDeComillas = false;

            for (int i = 0; i < linea.Length; i++)
            {
                char c = linea[i];

                if (dentroDeComillas)
                {
                    if (c == '"')
                    {
                        if (i + 1 < linea.Length && linea[i + 1] == '"')
                        {
                            actual.Append('"');
                            i++;
                        }
                        else
                        {
                            dentroDeComillas = false;
                        }
                    }
                    else
                    {
                        actual.Append(c);
                    }
                }
                else
                {
                    if (c == '"')
                    {
                        dentroDeComillas = true;
                    }
                    else if (c == ';')
                    {
                        campos.Add(actual.ToString());
                        actual.Clear();
                    }
                    else
                    {
                        actual.Append(c);
                    }
                }
            }

            campos.Add(actual.ToString());
            return campos.ToArray();
        }

        private const int MaxNombresEnDetalleDiscrepancia = 15;

        /// <summary>
        /// Resultado de comparar, dentro de una carpeta, los PDFs fisicos contra los
        /// registros del CSV de indice (columna "Nombre Nuevo").
        /// </summary>
        public class DiscrepanciaDetalle
        {
            public string Carpeta { get; set; } = string.Empty;
            public int CantidadPdf { get; set; }
            public int CantidadRegistrosCsv { get; set; }
            public List<string> PdfSinRegistroCsv { get; set; } = new();
            public List<string> RegistrosCsvSinPdf { get; set; } = new();

            public bool HayDiscrepancia => PdfSinRegistroCsv.Count > 0 || RegistrosCsvSinPdf.Count > 0;
        }

        /// <summary>
        /// Compara, dentro de una carpeta, el conjunto de archivos PDF fisicos contra el
        /// conjunto de nombres registrados en la columna "Nombre Nuevo" de los CSV de indice
        /// presentes en esa misma carpeta. Identifica CUALES archivos/registros especificos
        /// no tienen contraparte. Devuelve null si no hay diferencias.
        /// </summary>
        public static DiscrepanciaDetalle? AnalizarDiscrepancia(string carpeta)
        {
            if (!Directory.Exists(carpeta))
                return null;

            var nombresPdf = Directory.GetFiles(carpeta, "*.pdf")
                .Select(Path.GetFileName)
                .Where(n => n != null)
                .Select(n => n!)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var nombresCsv = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var rutaCsv in Directory.GetFiles(carpeta, "*.csv"))
            {
                var lineas = File.ReadAllLines(rutaCsv);
                if (lineas.Length == 0)
                    continue;

                var encabezados = ParsearLineaCsv(lineas[0]);
                int indiceNombreNuevo = Array.FindIndex(encabezados,
                    h => string.Equals(h.Trim(), "Nombre Nuevo", StringComparison.OrdinalIgnoreCase));

                if (indiceNombreNuevo < 0)
                    continue;

                for (int i = 1; i < lineas.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lineas[i]))
                        continue;

                    var campos = ParsearLineaCsv(lineas[i]);
                    if (indiceNombreNuevo < campos.Length)
                    {
                        string nombre = campos[indiceNombreNuevo].Trim();
                        if (!string.IsNullOrWhiteSpace(nombre))
                            nombresCsv.Add(nombre);
                    }
                }
            }

            var pdfSinRegistro = nombresPdf.Except(nombresCsv, StringComparer.OrdinalIgnoreCase)
                .OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList();
            var registrosSinPdf = nombresCsv.Except(nombresPdf, StringComparer.OrdinalIgnoreCase)
                .OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList();

            if (pdfSinRegistro.Count == 0 && registrosSinPdf.Count == 0)
                return null;

            return new DiscrepanciaDetalle
            {
                Carpeta = carpeta,
                CantidadPdf = nombresPdf.Count,
                CantidadRegistrosCsv = nombresCsv.Count,
                PdfSinRegistroCsv = pdfSinRegistro,
                RegistrosCsvSinPdf = registrosSinPdf
            };
        }

        /// <summary>
        /// Nombre del archivo de texto con el detalle de la discrepancia, escrito en la
        /// carpeta "Planos Procesados" para que el usuario pueda ver exactamente que
        /// archivo(s) rompen la coincidencia entre PDF y CSV.
        /// </summary>
        private static string NombreArchivoReporteDiscrepancia(int cdLote) => $"DISCREPANCIA_Lote{cdLote}.txt";

        /// <summary>
        /// Escribe, dentro de la carpeta "Planos Procesados" de la carpetaBase indicada, un
        /// archivo de texto con el detalle de la discrepancia detectada para el lote: cuales
        /// PDF no tienen registro en el CSV y cuales registros del CSV no tienen PDF fisico.
        /// </summary>
        private void EscribirReporteDiscrepancia(string carpetaBase, int cdLote, List<DiscrepanciaDetalle> discrepancias)
        {
            string carpetaProcesados = Path.Combine(carpetaBase, CarpetaProcesados);

            if (!Directory.Exists(carpetaProcesados))
                Directory.CreateDirectory(carpetaProcesados);

            string rutaReporte = Path.Combine(carpetaProcesados, NombreArchivoReporteDiscrepancia(cdLote));

            var lineas = new List<string>
            {
                $"Lote {cdLote}",
                $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}",
                ""
            };

            foreach (var discrepancia in discrepancias)
            {
                lineas.Add($"Carpeta: {discrepancia.Carpeta}");
                lineas.Add($"PDF: {discrepancia.CantidadPdf} | Registros CSV: {discrepancia.CantidadRegistrosCsv}");
                lineas.Add("");

                lineas.Add($"Archivos PDF que NO estan en el CSV ({discrepancia.PdfSinRegistroCsv.Count}):");
                if (discrepancia.PdfSinRegistroCsv.Count == 0)
                    lineas.Add("  (ninguno)");
                else
                    lineas.AddRange(discrepancia.PdfSinRegistroCsv.Select(n => "  - " + n));

                lineas.Add("");

                lineas.Add($"Archivos que estan en el CSV pero NO esta el PDF ({discrepancia.RegistrosCsvSinPdf.Count}):");
                if (discrepancia.RegistrosCsvSinPdf.Count == 0)
                    lineas.Add("  (ninguno)");
                else
                    lineas.AddRange(discrepancia.RegistrosCsvSinPdf.Select(n => "  - " + n));

                lineas.Add("");
                lineas.Add(new string('-', 60));
                lineas.Add("");
            }

            File.WriteAllLines(rutaReporte, lineas, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Construye el mensaje de error a mostrar al usuario a partir de las discrepancias
        /// detectadas, indicando ademas donde quedo el archivo de texto con el detalle.
        /// </summary>
        private static string ConstruirMensajeDiscrepancia(List<DiscrepanciaDetalle> discrepancias, string rutaReporte)
        {
            var detalle = new List<string>();

            foreach (var discrepancia in discrepancias)
            {
                detalle.Add($"Carpeta '{discrepancia.Carpeta}': {discrepancia.CantidadPdf} PDF vs " +
                    $"{discrepancia.CantidadRegistrosCsv} registro(s) en CSV.");

                if (discrepancia.PdfSinRegistroCsv.Count > 0)
                {
                    detalle.Add($"  - PDF(s) sin registro en el CSV ({discrepancia.PdfSinRegistroCsv.Count}): " +
                        string.Join(", ", discrepancia.PdfSinRegistroCsv.Take(MaxNombresEnDetalleDiscrepancia)) +
                        (discrepancia.PdfSinRegistroCsv.Count > MaxNombresEnDetalleDiscrepancia ? ", ..." : ""));
                }

                if (discrepancia.RegistrosCsvSinPdf.Count > 0)
                {
                    detalle.Add($"  - Registro(s) del CSV sin PDF f\u00EDsico ({discrepancia.RegistrosCsvSinPdf.Count}): " +
                        string.Join(", ", discrepancia.RegistrosCsvSinPdf.Take(MaxNombresEnDetalleDiscrepancia)) +
                        (discrepancia.RegistrosCsvSinPdf.Count > MaxNombresEnDetalleDiscrepancia ? ", ..." : ""));
                }
            }

            detalle.Add("");
            detalle.Add($"Detalle completo guardado en: {rutaReporte}");

            return string.Join(Environment.NewLine, detalle);
        }

        /// <summary>
        /// Compara, dentro de una carpeta, el conjunto de archivos PDF fisicos contra el
        /// conjunto de nombres registrados en la columna "Nombre Nuevo" de los CSV de indice
        /// presentes en esa misma carpeta. A diferencia de ValidarCantidadArchivosVsCsv (que
        /// solo compara cantidades), este metodo identifica CUALES archivos/registros
        /// especificos no tienen contraparte, para poder informarle al usuario la causa
        /// concreta de la discrepancia. Devuelve un mensaje vacio si no hay diferencias.
        /// </summary>
        public static string DetallarDiscrepancia(string carpeta)
        {
            var discrepancia = AnalizarDiscrepancia(carpeta);
            if (discrepancia == null)
                return string.Empty;

            return ConstruirMensajeDiscrepancia(new List<DiscrepanciaDetalle> { discrepancia }, string.Empty);
        }


        public List<string> FinalizarLote(int cdLote, List<FilaFinalizacion> filas, int cdUsuario)
        {
            // Paso 1: copia y renombra los PDFs a una subcarpeta temporal aislada del lote
            // (Planos Procesados\cdLoteN / Planos ILEGIBLES\cdLoteN) para no mezclarlos con
            // los de otros lotes mientras se valida que todo este correcto.
            var movimientos = MoverYRenombrarArchivos(filas, cdLote);

            // Paso 2: genera el/los CSV de indice dentro de esa misma carpeta temporal.
            GenerarCsvMetadatos(cdLote);

            // Paso 3: valida, dentro de cada carpeta temporal, que la cantidad y los nombres
            // de PDF coincidan exactamente con los registros del CSV. Si hay una diferencia
            // esto es un ERROR (no una advertencia): se informa el detalle exacto (que
            // archivos/registros sobran o faltan), se deja un archivo de texto en
            // "Planos Procesados" con el detalle, y se descarta todo lo generado para este
            // lote, sin tocar la carpeta compartida ni marcar el lote como finalizado.
            var carpetasBaseInvolucradas = movimientos
                .Select(m => m.CarpetaBase)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var carpetasTemporales = movimientos
                .Select(m => m.CarpetaDestino)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var discrepanciasTemp = carpetasTemporales
                .Select(AnalizarDiscrepancia)
                .Where(d => d != null)
                .Select(d => d!)
                .ToList();

            if (discrepanciasTemp.Count > 0)
            {
                string carpetaBaseReporte = carpetasBaseInvolucradas.First();
                EscribirReporteDiscrepancia(carpetaBaseReporte, cdLote, discrepanciasTemp);
                string rutaReporte = Path.Combine(carpetaBaseReporte, CarpetaProcesados, NombreArchivoReporteDiscrepancia(cdLote));

                foreach (var carpetaTemp in carpetasTemporales)
                {
                    if (Directory.Exists(carpetaTemp))
                        Directory.Delete(carpetaTemp, recursive: true);
                }

                throw new InvalidOperationException(
                    "Se detecto una discrepancia entre los PDF generados y el CSV de indice " +
                    "para este lote; no se realizaron cambios en la carpeta final. Detalle:" +
                    Environment.NewLine + ConstruirMensajeDiscrepancia(discrepanciasTemp, rutaReporte));
            }

            // Paso 4: la carpeta temporal es consistente; se mueve todo a la carpeta final
            // compartida (sin pisar archivos existentes) y se unifican los CSV.
            var gruposDestino = movimientos
                .GroupBy(m => (m.CarpetaBase, m.EsIlegible));

            var carpetasFinales = new List<string>();

            foreach (var grupo in gruposDestino)
            {
                string carpetaFinal = ConsolidarCarpetaTemporal(
                    grupo.ToList(), grupo.Key.CarpetaBase, grupo.Key.EsIlegible, cdLote);

                carpetasFinales.Add(carpetaFinal);
            }

            // Paso 5 ya ocurre dentro de ConsolidarCarpetaTemporal (borra la carpeta temporal
            // una vez movidos los archivos y unificado el CSV).

            // Paso 6: vuelve a comparar la carpeta final consolidada; si aun asi hay una
            // diferencia, se informa en detalle al usuario como error, dejando tambien el
            // archivo de texto con el detalle en "Planos Procesados".
            var discrepanciasFinal = carpetasFinales
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(AnalizarDiscrepancia)
                .Where(d => d != null)
                .Select(d => d!)
                .ToList();

            if (discrepanciasFinal.Count > 0)
            {
                string carpetaBaseReporte = carpetasBaseInvolucradas.First();
                EscribirReporteDiscrepancia(carpetaBaseReporte, cdLote, discrepanciasFinal);
                string rutaReporte = Path.Combine(carpetaBaseReporte, CarpetaProcesados, NombreArchivoReporteDiscrepancia(cdLote));

                throw new InvalidOperationException(
                    "Los archivos de este lote se movieron correctamente, pero se detecto una " +
                    "discrepancia en la carpeta final (posiblemente originada por otro lote). " +
                    "El lote NO fue marcado como finalizado. Detalle:" +
                    Environment.NewLine + ConstruirMensajeDiscrepancia(discrepanciasFinal, rutaReporte));
            }

            MarcarLoteFinalizado(cdLote, cdUsuario);

            return new List<string>();
        }

        /// <summary>
        /// Mueve todos los PDFs de la carpeta temporal de un lote (para una carpetaBase y
        /// categoria -normal/ilegible- dadas) a la carpeta final compartida, sin pisar
        /// archivos existentes (resolviendo colisiones de nombre), unifica el CSV de indice
        /// temporal con el CSV final consolidado, y borra la carpeta temporal al terminar.
        /// Si ocurre un error a mitad de camino, revierte (borra) los archivos que esta
        /// llamada ya habia movido a la carpeta final, dejando la carpeta temporal intacta
        /// para diagnostico, y relanza la excepcion.
        /// </summary>
        private string ConsolidarCarpetaTemporal(
            List<RegistroMovimiento> movimientosCarpeta, string carpetaBase, bool esIlegible, int cdLote)
        {
            string nombreSubcarpeta = esIlegible ? CarpetaIlegibles : CarpetaProcesados;
            string carpetaFinal = Path.Combine(carpetaBase, nombreSubcarpeta);
            string carpetaTemporal = Path.Combine(carpetaFinal, ObtenerNombreCarpetaTemporalLote(cdLote));

            if (!Directory.Exists(carpetaTemporal))
                return carpetaFinal;

            var archivosMovidos = new List<string>();
            var renombrados = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                foreach (var movimiento in movimientosCarpeta)
                {
                    string origenTemp = Path.Combine(carpetaTemporal, movimiento.NombreArchivoNuevo);

                    if (!File.Exists(origenTemp))
                        continue;

                    string nombreFinal = movimiento.NombreArchivoNuevo;
                    string destino = Path.Combine(carpetaFinal, nombreFinal);

                    if (File.Exists(destino))
                    {
                        nombreFinal = GeneradorNombreArchivo.ResolverColision(carpetaFinal, nombreFinal);
                        destino = Path.Combine(carpetaFinal, nombreFinal);
                    }

                    File.Move(origenTemp, destino);
                    archivosMovidos.Add(destino);

                    if (!string.Equals(nombreFinal, movimiento.NombreArchivoNuevo, StringComparison.OrdinalIgnoreCase))
                    {
                        renombrados[movimiento.NombreArchivoNuevo] = nombreFinal;
                        _loteDAL.ActualizarNombreArchivoFinal(movimiento.CdArchivoPagina, nombreFinal);
                        movimiento.NombreArchivoNuevo = nombreFinal;
                    }
                }

                string nombreCarpetaBase = new DirectoryInfo(carpetaBase).Name;
                string nombreCsv = esIlegible ? $"{nombreCarpetaBase}_ILEGIBLE.csv" : $"{nombreCarpetaBase}.csv";

                UnificarCsv(carpetaTemporal, carpetaFinal, nombreCsv, renombrados);

                Directory.Delete(carpetaTemporal, recursive: true);

                return carpetaFinal;
            }
            catch
            {
                foreach (var rutaMovida in archivosMovidos)
                {
                    try
                    {
                        if (File.Exists(rutaMovida))
                            File.Delete(rutaMovida);
                    }
                    catch
                    {
                        // Se ignora: el error original ya sera informado al usuario/log.
                    }
                }

                throw;
            }
        }

        /// <summary>
        /// Anexa las filas de datos del CSV temporal (sin encabezado) al CSV final
        /// consolidado (creando el encabezado si el CSV final aun no existe), aplicando el
        /// mapeo de renombrados a la columna "Nombre Nuevo" cuando corresponda por
        /// colisiones resueltas al mover el archivo a la carpeta final.
        /// </summary>
        private static void UnificarCsv(
            string carpetaTemporal, string carpetaFinal, string nombreCsv, Dictionary<string, string> renombrados)
        {
            string rutaCsvTemp = Path.Combine(carpetaTemporal, nombreCsv);

            if (!File.Exists(rutaCsvTemp))
                return;

            var lineas = File.ReadAllLines(rutaCsvTemp);
            if (lineas.Length == 0)
                return;

            string encabezado = lineas[0];
            var encabezados = ParsearLineaCsv(encabezado);
            int indiceNombreNuevo = renombrados.Count > 0
                ? Array.FindIndex(encabezados, h => string.Equals(h.Trim(), "Nombre Nuevo", StringComparison.OrdinalIgnoreCase))
                : -1;

            string rutaCsvFinal = Path.Combine(carpetaFinal, nombreCsv);
            bool existeCsvFinal = File.Exists(rutaCsvFinal);

            using var writer = new StreamWriter(rutaCsvFinal, append: true, System.Text.Encoding.UTF8);

            if (!existeCsvFinal)
                writer.WriteLine(encabezado);

            for (int i = 1; i < lineas.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lineas[i]))
                    continue;

                string linea = lineas[i];

                if (indiceNombreNuevo >= 0)
                {
                    var campos = ParsearLineaCsv(linea);
                    if (indiceNombreNuevo < campos.Length)
                    {
                        string nombreActual = campos[indiceNombreNuevo].Trim();
                        if (renombrados.TryGetValue(nombreActual, out var nombreNuevo))
                        {
                            campos[indiceNombreNuevo] = nombreNuevo;
                            linea = string.Join(";", campos.Select(EscaparCampoCsv));
                        }
                    }
                }

                writer.WriteLine(linea);
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
                DsMensaje = $"Error en finalización de lote: {ex.Message}",
                DsExcepcion = ex.ToString(),
                CdUsuario = cdUsuario
            });
        }
    }
}
