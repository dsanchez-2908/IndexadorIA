using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para la tabla TD_CATEGORIA_PLANO
    /// </summary>
    public class CategoriaPlanoDAL
    {
        private readonly string _cadenaConexion;

        public CategoriaPlanoDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene todas las categorías de plano activas
        /// </summary>
        public List<CategoriaPlano> ObtenerTodos()
        {
            List<CategoriaPlano> categorias = new();

            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    SELECT cdCategoriaPlano, dsCategoriaPlano, snActivo, dsDescripcion, feAlta 
                    FROM TD_CATEGORIA_PLANO 
                    WHERE snActivo = 1 
                    ORDER BY dsCategoriaPlano", conn);

                conn.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categorias.Add(new CategoriaPlano
                    {
                        CdCategoriaPlano = reader.GetInt32(0),
                        DsCategoriaPlano = reader.GetString(1),
                        SnActivo = reader.GetBoolean(2),
                        DsDescripcion = reader.IsDBNull(3) ? null : reader.GetString(3),
                        FeAlta = reader.GetDateTime(4)
                    });
                }

                return categorias;
            }
            catch (Exception ex)
            {
                var logDAL = new LogDAL();
                logDAL.Insertar(new LogRegistro
                {
                    DsNivel = LogRegistro.Niveles.ERROR,
                    DsModulo = "CategoriaPlanoDAL.ObtenerTodos",
                    DsMensaje = $"Error al obtener categorías de plano: {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }

        /// <summary>
        /// Busca una categoría de plano por su descripción (case/acentos/espacios insensible)
        /// </summary>
        public CategoriaPlano? BuscarPorNombre(string dsCategoriaPlano)
        {
            try
            {
                using var conn = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand(@"
                    SELECT cdCategoriaPlano, dsCategoriaPlano, snActivo, dsDescripcion, feAlta 
                    FROM TD_CATEGORIA_PLANO 
                    WHERE UPPER(LTRIM(RTRIM(dsCategoriaPlano))) = UPPER(LTRIM(RTRIM(@dsCategoriaPlano)))", conn);

                cmd.Parameters.AddWithValue("@dsCategoriaPlano", dsCategoriaPlano);

                conn.Open();
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new CategoriaPlano
                    {
                        CdCategoriaPlano = reader.GetInt32(0),
                        DsCategoriaPlano = reader.GetString(1),
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
                    DsModulo = "CategoriaPlanoDAL.BuscarPorNombre",
                    DsMensaje = $"Error al buscar categoría de plano '{dsCategoriaPlano}': {ex.Message}",
                    DsExcepcion = ex.ToString()
                });
                throw;
            }
        }
    }
}
