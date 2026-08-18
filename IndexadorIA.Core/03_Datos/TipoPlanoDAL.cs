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
                    SELECT cdTipoPlano, cdCategoriaPlano, dsTipoPlano, snActivo, dsAcronimo, feAlta 
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
                        CdCategoriaPlano = reader.GetInt32(1),
                        DsTipoPlano = reader.GetString(2),
                        SnActivo = reader.GetBoolean(3),
                        DsAcronimo = reader.IsDBNull(4) ? null : reader.GetString(4),
                        FeAlta = reader.GetDateTime(5)
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
        /// Busca un tipo de plano por su descripción (case/acentos/espacios insensible),
        /// opcionalmente restringido a una categoría de plano.
        /// </summary>
        public TipoPlano? BuscarPorNombre(string dsTipoPlano, int? cdCategoriaPlano = null)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand($@"
                    SELECT cdTipoPlano, cdCategoriaPlano, dsTipoPlano, snActivo, dsAcronimo, feAlta 
                    FROM TD_TIPOS_PLANO 
                    WHERE UPPER(LTRIM(RTRIM(dsTipoPlano))) = UPPER(LTRIM(RTRIM(@dsTipoPlano)))
                    {(cdCategoriaPlano.HasValue ? "AND cdCategoriaPlano = @cdCategoriaPlano" : "")}", conn);

                cmd.Parameters.AddWithValue("@dsTipoPlano", dsTipoPlano);
                if (cdCategoriaPlano.HasValue)
                {
                    cmd.Parameters.AddWithValue("@cdCategoriaPlano", cdCategoriaPlano.Value);
                }

                conn.Open();
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new TipoPlano
                    {
                        CdTipoPlano = reader.GetInt32(0),
                        CdCategoriaPlano = reader.GetInt32(1),
                        DsTipoPlano = reader.GetString(2),
                        SnActivo = reader.GetBoolean(3),
                        DsAcronimo = reader.IsDBNull(4) ? null : reader.GetString(4),
                        FeAlta = reader.GetDateTime(5)
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
