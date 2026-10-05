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

        /// <summary>
        /// Reubica un conjunto de archivos página (todos pertenecientes al mismo lote de
        /// origen) hacia un nuevo lote, dejando el nuevo lote en cdEstadoLote=7
        /// (Pendiente Asignar Auditoria). No permite reubicar si el lote de origen está
        /// en cdEstadoLote=11 (Enviado).
        /// </summary>
        public (bool exito, string mensaje, int cdLoteNuevo) ReubicarEnNuevoLote(int cdLoteOrigen, List<int> cdArchivosPagina)
        {
            try
            {
                if (cdArchivosPagina == null || cdArchivosPagina.Count == 0)
                {
                    return (false, "Debe seleccionar al menos un registro para reubicar", 0);
                }

                int? cdEstadoLoteOrigen = _loteDAL.ObtenerEstadoLote(cdLoteOrigen);
                if (cdEstadoLoteOrigen is null)
                {
                    return (false, "No se encontró el lote de origen", 0);
                }

                if (cdEstadoLoteOrigen == 11)
                {
                    return (false, "No se puede reubicar planos de un lote en estado Enviado", 0);
                }

                int cdLoteNuevo = _loteDAL.ReubicarArchivosEnNuevoLote(
                    cdLoteOrigen, cdArchivosPagina, SesionActual.UsuarioActual?.CdUsuario ?? 0);

                string mensaje = $"Se reubicaron {cdArchivosPagina.Count} plano(s) del lote {cdLoteOrigen} al nuevo lote {cdLoteNuevo}";

                _logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "REUBICACION_LOTE",
                    DsMensaje = mensaje,
                    CdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0
                });

                return (true, mensaje, cdLoteNuevo);
            }
            catch (Exception ex)
            {
                string mensajeError = $"Error al reubicar planos en nuevo lote: {ex.Message}";

                _logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "REUBICACION_LOTE",
                    DsMensaje = mensajeError,
                    DsExcepcion = ex.ToString(),
                    CdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0
                });

                return (false, mensajeError, 0);
            }
        }

        /// <summary>
        /// Reubica planos de m\u00faltiples lotes de origen (presentes en la grilla de Consulta de
        /// Planos) hacia nuevos lotes: crea un nuevo lote por cada lote de origen distinto,
        /// asigna la descripci\u00f3n indicada al campo dsDescripcion de cada lote nuevo y mueve
        /// los archivos p\u00e1gina correspondientes. No valida que los archivos pertenezcan al
        /// mismo lote de origen entre s\u00ed.
        /// </summary>
        public (bool exito, string mensaje, int cantidadLotesCreados) ReubicarEnNuevosLotesMultiples(
            Dictionary<int, List<int>> archivosPorLoteOrigen, string dsDescripcionLote)
        {
            try
            {
                if (archivosPorLoteOrigen == null || archivosPorLoteOrigen.Count == 0)
                {
                    return (false, "Debe seleccionar al menos un registro para reubicar", 0);
                }

                if (string.IsNullOrWhiteSpace(dsDescripcionLote))
                {
                    return (false, "Debe ingresar una descripci\u00f3n de lote", 0);
                }

                int cantidadLotesCreados = _loteDAL.ReubicarArchivosEnNuevosLotesMultiples(
                    archivosPorLoteOrigen, dsDescripcionLote, SesionActual.UsuarioActual?.CdUsuario ?? 0);

                int totalArchivos = archivosPorLoteOrigen.Values.Sum(v => v.Count);
                string mensaje = $"Se crearon {cantidadLotesCreados} lote(s) nuevo(s) y se reubicaron {totalArchivos} plano(s)";

                _logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.INFO,
                    DsModulo = "REUBICACION_LOTE",
                    DsMensaje = mensaje,
                    CdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0
                });

                return (true, mensaje, cantidadLotesCreados);
            }
            catch (Exception ex)
            {
                string mensajeError = $"Error al reubicar planos en nuevos lotes: {ex.Message}";

                _logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "REUBICACION_LOTE",
                    DsMensaje = mensajeError,
                    DsExcepcion = ex.ToString(),
                    CdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0
                });

                return (false, mensajeError, 0);
            }
        }
    }
}
