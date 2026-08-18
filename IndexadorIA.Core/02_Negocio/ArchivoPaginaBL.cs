using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Negocio
{
    public class ArchivoPaginaBL
    {
        private readonly ArchivoPaginaDAL _archivoDAL;
        private readonly RotacionDAL _rotacionDAL;

        public ArchivoPaginaBL()
        {
            _archivoDAL = new ArchivoPaginaDAL();
            _rotacionDAL = new RotacionDAL();
        }

        /// <summary>
        /// Obtiene la siguiente secuencia global para nombres de archivo
        /// </summary>
        public int ObtenerSiguienteSecuencia(int cantidad = 1)
        {
            try
            {
                return _archivoDAL.ObtenerSiguienteSecuencia(cantidad);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener secuencia: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Guarda un lote de páginas y sus rotaciones
        /// IMPORTANTE: Las rotaciones se insertan DESPUÉS de las páginas para obtener los IDs
        /// Las listas de páginas y rotaciones deben tener la misma cantidad y estar en el mismo orden
        /// </summary>
        public (bool exito, string mensaje, int totalGuardado) GuardarLote(
            List<ArchivoPagina> paginas, 
            List<Rotacion> rotaciones)
        {
            try
            {
                // Validar que haya páginas
                if (paginas == null || paginas.Count == 0)
                {
                    return (false, "No hay páginas para guardar", 0);
                }

                // Insertar páginas primero - esto asignará los cdArchivoPagina a cada entidad
                int paginasGuardadas = _archivoDAL.InsertarLote(paginas);

                // Insertar rotaciones DESPUÉS, ahora con los IDs de página asignados
                if (rotaciones != null && rotaciones.Count > 0)
                {
                    // Validar que haya el mismo número de rotaciones que de páginas
                    if (rotaciones.Count != paginas.Count)
                    {
                        LogDAL.RegistrarLog(
                            LogRegistro.Niveles.WARNING,
                            "ArchivoPaginaBL.GuardarLote",
                            $"Advertencia: Se recibieron {paginas.Count} páginas pero {rotaciones.Count} rotaciones. Se asociarán solo las primeras {Math.Min(paginas.Count, rotaciones.Count)}.");
                    }

                    // Asociar rotaciones con las páginas insertadas (1 a 1 en orden)
                    for (int i = 0; i < rotaciones.Count && i < paginas.Count; i++)
                    {
                        if (paginas[i].CdArchivoPagina > 0)
                        {
                            rotaciones[i].CdArchivoPagina = paginas[i].CdArchivoPagina;
                        }
                    }

                    // Filtrar rotaciones que tengan ID de página válido
                    var rotacionesValidas = rotaciones.Where(r => r.CdArchivoPagina > 0).ToList();

                    if (rotacionesValidas.Count > 0)
                    {
                        _rotacionDAL.InsertarLote(rotacionesValidas);
                    }
                }

                return (true, $"Se guardaron {paginasGuardadas} páginas exitosamente", paginasGuardadas);
            }
            catch (Exception ex)
            {
                // Registrar error en log
                LogDAL.RegistrarLog(
                    LogRegistro.Niveles.ERROR,
                    "ArchivoPaginaBL.GuardarLote",
                    $"Error al guardar páginas: {ex.Message}",
                    ex);

                return (false, $"Error al guardar páginas: {ex.Message}", 0);
            }
        }

        /// <summary>
        /// Obtiene todas las páginas de un archivo original
        /// </summary>
        public List<ArchivoPagina> ObtenerPorArchivoOriginal(int cdArchivoOriginal)
        {
            try
            {
                return _archivoDAL.ObtenerPorArchivoOriginal(cdArchivoOriginal);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener páginas: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene todas las rotaciones de una página
        /// </summary>
        public List<Rotacion> ObtenerRotacionesPorPagina(int cdArchivoPagina)
        {
            try
            {
                return _rotacionDAL.ObtenerPorPagina(cdArchivoPagina);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener rotaciones: {ex.Message}", ex);
            }
        }
    }
}
