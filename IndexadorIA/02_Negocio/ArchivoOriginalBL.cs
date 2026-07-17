using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Negocio
{
    public class ArchivoOriginalBL
    {
        private readonly ArchivoOriginalDAL _archivoDAL;

        public ArchivoOriginalBL()
        {
            _archivoDAL = new ArchivoOriginalDAL();
        }

        public (bool exito, string mensaje, int cantidad) GuardarLote(List<ArchivoOriginal> archivos)
        {
            if (archivos == null || archivos.Count == 0)
            {
                return (false, "No hay archivos para guardar.", 0);
            }

            try
            {
                int insertados = _archivoDAL.InsertarLote(archivos);
                return (true, $"Se guardaron {insertados} archivos exitosamente.", insertados);
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar archivos: {ex.Message}", 0);
            }
        }

        public List<ArchivoOriginal> ObtenerPorProyecto(int cdProyecto)
        {
            return _archivoDAL.ObtenerPorProyecto(cdProyecto);
        }
    }
}
