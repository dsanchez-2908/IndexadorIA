using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Negocio
{
    /// <summary>
    /// Lógica de negocio para lotes de archivos página
    /// </summary>
    public class LoteBL
    {
        private readonly LoteDAL _loteDAL;
        private readonly LogDAL _logDAL;

        public LoteBL()
        {
            _loteDAL = new LoteDAL();
            _logDAL = new LogDAL();
        }

        /// <summary>
        /// Obtiene archivos página disponibles para crear lotes
        /// </summary>
        public List<ArchivoPaginaGridDto> ObtenerArchivosPaginaParaLote(int cdProyecto, 
            string? dsNombreArchivo = null, DateTime? feAltaDesde = null, DateTime? feAltaHasta = null,
            string? dsCarpeta = null)
        {
            try
            {
                // cdEstado=1 son archivos en estado "Pendiente de Agrupar en Lote"
                return _loteDAL.ObtenerArchivosPaginaParaLote(cdProyecto, 1, dsNombreArchivo, feAltaDesde, feAltaHasta, dsCarpeta);
            }
            catch (Exception ex)
            {
                _logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "PREPARACION_LOTE",
                    DsMensaje = $"Error al obtener archivos página: {ex.Message}",
                    DsExcepcion = ex.ToString(),
                    CdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0
                });
                throw;
            }
        }

        /// <summary>
        /// Crea lotes agrupando archivos página
        /// </summary>
        public (bool exito, string mensaje) CrearLotes(List<int> cdArchivosPaginaSeleccionados, int archivosPorLote)
        {
            try
            {
                if (cdArchivosPaginaSeleccionados == null || cdArchivosPaginaSeleccionados.Count == 0)
                {
                    return (false, "Debe seleccionar al menos un archivo página");
                }

                if (archivosPorLote <= 0)
                {
                    return (false, "La cantidad de archivos por lote debe ser mayor a cero");
                }

                int cantidadLotesCreados = 0;
                int totalArchivos = cdArchivosPaginaSeleccionados.Count;

                // Agrupar archivos en lotes
                for (int i = 0; i < totalArchivos; i += archivosPorLote)
                {
                    var archivosDeLote = cdArchivosPaginaSeleccionados
                        .Skip(i)
                        .Take(archivosPorLote)
                        .ToList();

                    // Obtener siguiente secuencia para nombre de lote
                    int secuencia = _loteDAL.ObtenerSiguienteSecuencia();
                    string nombreLote = $"LOTE_{secuencia:D8}";

                    // Crear lote (cdEstadoLote=1: "Pendiente de Preparar imágenes")
                    int cdLote = _loteDAL.CrearLote(
                        nombreLote,
                        archivosDeLote.Count,
                        1, // cdEstadoLote
                        SesionActual.UsuarioActual?.CdUsuario ?? 0,
                        archivosDeLote
                    );

                    cantidadLotesCreados++;

                    // Log de creación de lote
                    _logDAL.Insertar(new LogRegistro
                    {
                        DsNivel = LogRegistro.Niveles.INFO,
                        DsModulo = "PREPARACION_LOTE",
                        DsMensaje = $"Lote creado: {nombreLote} - CdLote: {cdLote}, Cantidad archivos: {archivosDeLote.Count}",
                        CdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0
                    });
                }

                string mensaje = $"Se crearon {cantidadLotesCreados} lote(s) con {totalArchivos} archivo(s) página";

                _logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "PREPARACION_LOTE",
                    DsMensaje = mensaje,
                    CdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0
                });

                return (true, mensaje);
            }
            catch (Exception ex)
            {
                string mensajeError = $"Error al crear lotes: {ex.Message}";

                _logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "PREPARACION_LOTE",
                    DsMensaje = mensajeError,
                    DsExcepcion = ex.ToString(),
                    CdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0
                });

                return (false, mensajeError);
            }
        }
    }
}
