using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para el reporte "Estado Proyecto Planos": resume el
    /// avance general de digitalización y control de planos del proyecto de GCBA
    /// (cdProyecto = 1).
    /// </summary>
    public class EstadoProyectoDAL
    {
        private readonly string _cadenaConexion;

        public EstadoProyectoDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene los indicadores del estado general del proyecto indicado.
        /// </summary>
        public EstadoProyectoDto ObtenerEstadoProyecto(int cdProyecto)
        {
            var dto = new EstadoProyectoDto();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                using (var cmd = new SqlCommand(
                    "SELECT ISNULL(nuTotalArchivos, 0), ISNULL(nuTotalPlanos, 0) FROM TD_PROYECTOS WHERE cdProyecto = @cdProyecto",
                    conexion))
                {
                    cmd.Parameters.AddWithValue("@cdProyecto", cdProyecto);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dto.TotalPlanosDelProyecto = reader.GetInt32(0);
                            dto.TotalPlanosPendientesDelProyecto = reader.GetInt32(1);
                        }
                    }
                }

                dto.TotalArchivosIngresados = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_ARCHIVOS_ORIGINAL");

                dto.TotalPaginasIngresadas = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_ARCHIVOS_PAGINAS");

                dto.TotalPlanosRevisados = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdEstadoControl IN (2,3,4) AND cdUsuarioControl IS NOT NULL");

                dto.TotalPlanosControlados = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdEstadoControl IN (2) AND cdUsuarioControl IS NOT NULL");

                dto.TotalPlanosFaltaDatos = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdEstadoControl IN (4) AND cdUsuarioControl IS NOT NULL");

                dto.TotalPlanosIlegibles = EjecutarEscalar(conexion,
                    "SELECT COUNT(*) FROM TD_001_RESULTADO_IA WHERE cdEstadoControl IN (3) AND cdUsuarioControl IS NOT NULL");

                dto.TotalPlanosAsignadosPendienteControl = EjecutarEscalar(conexion,
                    @"SELECT COUNT(*) FROM TD_001_RESULTADO_IA a
                      LEFT JOIN TD_LOTE b ON a.cdLote = b.cdLote
                      WHERE b.cdEstadoLote = 6 AND a.cdEstadoControl = 1");

                dto.TotalPlanosSinAsignarControl = EjecutarEscalar(conexion,
                    @"SELECT COUNT(*) FROM TD_001_RESULTADO_IA a
                      LEFT JOIN TD_LOTE b ON a.cdLote = b.cdLote
                      WHERE b.cdEstadoLote = 4 AND a.cdEstadoControl = 1");
            }

            dto.TotalPlanosPendientesIngresados = dto.TotalPaginasIngresadas - dto.TotalPlanosRevisados;
            dto.TotalPlanosPendientesProyecto = dto.TotalPlanosPendientesDelProyecto - dto.TotalPlanosRevisados;

            return dto;
        }

        private static int EjecutarEscalar(SqlConnection conexion, string sql)
        {
            using (var cmd = new SqlCommand(sql, conexion))
            {
                var resultado = cmd.ExecuteScalar();
                return resultado == null || resultado == DBNull.Value ? 0 : Convert.ToInt32(resultado);
            }
        }
    }
}
