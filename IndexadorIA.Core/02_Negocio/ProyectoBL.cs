using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Negocio
{
    public class ProyectoBL
    {
        private readonly ProyectoDAL _proyectoDAL;

        public ProyectoBL()
        {
            _proyectoDAL = new ProyectoDAL();
        }

        public List<Proyecto> ObtenerActivos()
        {
            return _proyectoDAL.ObtenerActivos();
        }

        public List<Proyecto> ObtenerTodos()
        {
            return _proyectoDAL.ObtenerTodos();
        }

        public Proyecto? ObtenerPorCodigo(int cdProyecto)
        {
            return _proyectoDAL.ObtenerPorCodigo(cdProyecto);
        }

        public (bool exito, string mensaje) Crear(string dsProyecto, bool snActivo, int cdUsuarioAlta)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(dsProyecto))
            {
                return (false, "El nombre del proyecto es obligatorio.");
            }

            if (dsProyecto.Length > 255)
            {
                return (false, "El nombre del proyecto no puede exceder 255 caracteres.");
            }

            if (_proyectoDAL.ExisteProyecto(dsProyecto))
            {
                return (false, "Ya existe un proyecto con ese nombre.");
            }

            try
            {
                _proyectoDAL.Insertar(dsProyecto, snActivo, cdUsuarioAlta);
                return (true, "Proyecto creado exitosamente.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear el proyecto: {ex.Message}");
            }
        }

        public (bool exito, string mensaje) Actualizar(int cdProyecto, string dsProyecto, bool snActivo, int cdUsuarioModificacion)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(dsProyecto))
            {
                return (false, "El nombre del proyecto es obligatorio.");
            }

            if (dsProyecto.Length > 255)
            {
                return (false, "El nombre del proyecto no puede exceder 255 caracteres.");
            }

            if (_proyectoDAL.ExisteProyecto(dsProyecto, cdProyecto))
            {
                return (false, "Ya existe otro proyecto con ese nombre.");
            }

            try
            {
                _proyectoDAL.Actualizar(cdProyecto, dsProyecto, snActivo, cdUsuarioModificacion);
                return (true, "Proyecto actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar el proyecto: {ex.Message}");
            }
        }

        public (bool exito, string mensaje) Eliminar(int cdProyecto)
        {
            try
            {
                _proyectoDAL.Eliminar(cdProyecto);
                return (true, "Proyecto eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar el proyecto: {ex.Message}");
            }
        }
    }
}
