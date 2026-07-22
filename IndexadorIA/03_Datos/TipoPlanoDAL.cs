using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para la tabla TD_TIPOS_PLANO
    /// </summary>
    public class TipoPlanoDAL
    {
        private readonly string _cadenaConexion;

        public TipoPlanoDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene todos los tipos de plano activos
        /// </summary>
        public List<TipoPlano> ObtenerTodos()
        {
            List<TipoPlano> tipos = new();

            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    SELECT cdTipoPlano, dsTipoPlano, snActivo, dsDescripcion, feAlta 
                    FROM TD_TIPOS_PLANO 
                    WHERE snActivo = 1 
                    ORDER BY dsTipoPlano", conn);

                conn.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tipos.Add(new TipoPlano
                    {
                        CdTipoPlano = reader.GetInt32(0),
                        DsTipoPlano = reader.GetString(1),
                        SnActivo = reader.GetBoolean(2),
                        DsDescripcion = reader.IsDBNull(3) ? null : reader.GetString(3),
                        FeAlta = reader.GetDateTime(4)
                    });
                }

                return tipos;
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "TipoPlanoDAL.ObtenerTodos",
                    DsMensaje = $"Error al obtener tipos de plano: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }

        /// <summary>
        /// Busca un tipo de plano por su descripción (case insensitive)
        /// </summary>
        public TipoPlano? BuscarPorNombre(string dsTipoPlano)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    SELECT cdTipoPlano, dsTipoPlano, snActivo, dsDescripcion, feAlta 
                    FROM TD_TIPOS_PLANO 
                    WHERE LOWER(dsTipoPlano) = LOWER(@dsTipoPlano)", conn);

                cmd.Parameters.AddWithValue("@dsTipoPlano", dsTipoPlano);

                conn.Open();
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new TipoPlano
                    {
                        CdTipoPlano = reader.GetInt32(0),
                        DsTipoPlano = reader.GetString(1),
                        SnActivo = reader.GetBoolean(2),
                        DsDescripcion = reader.IsDBNull(3) ? null : reader.GetString(3),
                        FeAlta = reader.GetDateTime(4)
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "TipoPlanoDAL.BuscarPorNombre",
                    DsMensaje = $"Error al buscar tipo de plano '{dsTipoPlano}': {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }
    }
}
