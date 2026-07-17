using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Negocio
{
    /// <summary>
    /// Clase de lógica de negocio para usuarios
    /// </summary>
    public class UsuarioBL
    {
        private readonly UsuarioDAL _usuarioDAL;

        public UsuarioBL()
        {
            _usuarioDAL = new UsuarioDAL();
        }

        /// <summary>
        /// Valida las credenciales de login
        /// </summary>
        public (bool exito, string mensaje, Usuario? usuario) ValidarLogin(string dsUsuario, string dsClave)
        {
            if (string.IsNullOrWhiteSpace(dsUsuario))
                return (false, "El usuario es requerido", null);

            if (string.IsNullOrWhiteSpace(dsClave))
                return (false, "La contraseña es requerida", null);

            Usuario? usuario = _usuarioDAL.ValidarLogin(dsUsuario, dsClave);

            if (usuario == null)
                return (false, "Usuario o contraseña incorrectos", null);

            return (true, "Login exitoso", usuario);
        }

        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        public List<Usuario> ObtenerTodos()
        {
            return _usuarioDAL.ObtenerTodos();
        }

        /// <summary>
        /// Obtiene un usuario por su código
        /// </summary>
        public Usuario? ObtenerPorCodigo(int cdUsuario)
        {
            return _usuarioDAL.ObtenerPorCodigo(cdUsuario);
        }

        /// <summary>
        /// Crea un nuevo usuario con clave manual
        /// </summary>
        public (bool exito, string mensaje, int cdUsuario) Crear(string dsUsuario, string dsNombreCompleto, string claveTemporal, int idRol, int cdUsuarioCreador)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(dsUsuario))
                return (false, "El nombre de usuario es requerido", 0);

            if (string.IsNullOrWhiteSpace(dsNombreCompleto))
                return (false, "El nombre completo es requerido", 0);

            if (string.IsNullOrWhiteSpace(claveTemporal))
                return (false, "La clave temporal es requerida", 0);

            if (claveTemporal.Length < 3)
                return (false, "La clave temporal debe tener al menos 3 caracteres", 0);

            if (idRol <= 0)
                return (false, "Debe seleccionar un rol", 0);

            if (_usuarioDAL.ExisteUsuario(dsUsuario))
                return (false, "El nombre de usuario ya existe", 0);

            Usuario nuevoUsuario = new Usuario
            {
                DsUsuario = dsUsuario.Trim(),
                DsClave = Seguridad.EncriptarSHA256(claveTemporal.Trim()),
                DsNombreCompleto = dsNombreCompleto.Trim(),
                SnClaveTemporal = true,
                SnPrimerIngreso = true,
                IdRol = idRol,
                CdEstado = 1, // Activo
                FeAlta = DateTime.Now,
                CdUsuarioAlta = cdUsuarioCreador
            };

            int cdUsuario = _usuarioDAL.Insertar(nuevoUsuario);

            if (cdUsuario > 0)
            {
                return (true, $"Usuario creado exitosamente. Clave temporal asignada: {claveTemporal.Trim()}", cdUsuario);
            }

            return (false, "Error al crear el usuario", 0);
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        public (bool exito, string mensaje) Actualizar(int cdUsuario, string dsUsuario, string dsNombreCompleto, int idRol, int cdEstado)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(dsUsuario))
                return (false, "El nombre de usuario es requerido");

            if (string.IsNullOrWhiteSpace(dsNombreCompleto))
                return (false, "El nombre completo es requerido");

            if (idRol <= 0)
                return (false, "Debe seleccionar un rol");

            Usuario? usuario = _usuarioDAL.ObtenerPorCodigo(cdUsuario);
            if (usuario == null)
                return (false, "Usuario no encontrado");

            if (_usuarioDAL.ExisteUsuario(dsUsuario, cdUsuario))
                return (false, "El nombre de usuario ya existe");

            usuario.DsUsuario = dsUsuario.Trim();
            usuario.DsNombreCompleto = dsNombreCompleto.Trim();
            usuario.IdRol = idRol;
            usuario.CdEstado = cdEstado;

            bool resultado = _usuarioDAL.Actualizar(usuario);

            if (resultado)
                return (true, "Usuario actualizado exitosamente");

            return (false, "Error al actualizar el usuario");
        }

        /// <summary>
        /// Elimina (desactiva) un usuario
        /// </summary>
        public (bool exito, string mensaje) Eliminar(int cdUsuario)
        {
            Usuario? usuario = _usuarioDAL.ObtenerPorCodigo(cdUsuario);
            if (usuario == null)
                return (false, "Usuario no encontrado");

            bool resultado = _usuarioDAL.Eliminar(cdUsuario);

            if (resultado)
                return (true, "Usuario desactivado exitosamente");

            return (false, "Error al desactivar el usuario");
        }

        /// <summary>
        /// Cambia la contraseña de un usuario
        /// </summary>
        public (bool exito, string mensaje) CambiarClave(int cdUsuario, string claveActual, string nuevaClave, string confirmarClave)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(nuevaClave))
                return (false, "La nueva contraseña es requerida");

            if (nuevaClave.Length < 4)
                return (false, "La contraseña debe tener al menos 4 caracteres");

            if (nuevaClave != confirmarClave)
                return (false, "Las contraseñas no coinciden");

            // Validar clave actual
            Usuario? usuario = _usuarioDAL.ObtenerPorCodigo(cdUsuario);
            if (usuario == null)
                return (false, "Usuario no encontrado");

            string claveActualEncriptada = Seguridad.EncriptarSHA256(claveActual);
            if (usuario.DsClave != claveActualEncriptada)
                return (false, "La contraseña actual es incorrecta");

            // Cambiar clave
            bool resultado = _usuarioDAL.CambiarClave(cdUsuario, nuevaClave, false);

            if (resultado)
                return (true, "Contraseña cambiada exitosamente");

            return (false, "Error al cambiar la contraseña");
        }

        /// <summary>
        /// Restablece la contraseña de un usuario (genera clave temporal)
        /// </summary>
        public (bool exito, string mensaje, string? claveTemporal) RestablecerClave(int cdUsuario)
        {
            Usuario? usuario = _usuarioDAL.ObtenerPorCodigo(cdUsuario);
            if (usuario == null)
                return (false, "Usuario no encontrado", null);

            string claveTemporal = Seguridad.GenerarClaveTemporal();
            bool resultado = _usuarioDAL.CambiarClave(cdUsuario, claveTemporal, true);

            if (resultado)
                return (true, "Contraseña restablecida exitosamente", claveTemporal);

            return (false, "Error al restablecer la contraseña", null);
        }
    }
}
