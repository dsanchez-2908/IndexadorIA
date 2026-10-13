using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para la tabla TV_CALLES, usada para validar nombres de
    /// calles ingresados en el campo Direccion de FrmVerLote.
    /// </summary>
    public class CalleDAL
    {
        private readonly string _cadenaConexion;

        public CalleDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene el listado completo de nombres de calles (nom_mapa) de TV_CALLES,
        /// para ser cacheado en memoria y usado como validacion rapida.
        /// </summary>
        public HashSet<string> ObtenerNombresCalles()
        {
            var nombres = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand("SELECT nom_mapa FROM TV_CALLES", conn);

                conn.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        string nombre = reader.GetString(0).Trim();
                        if (nombre.Length > 0)
                            nombres.Add(nombre);
                    }
                }

                return nombres;
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "CalleDAL.ObtenerNombresCalles",
                    DsMensaje = $"Error al obtener nombres de calles: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });

                return nombres;
            }
        }
    }
}
