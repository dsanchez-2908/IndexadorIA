using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    public class RotacionDAL
    {
        private readonly string _cadenaConexion;

        public RotacionDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Inserta un registro de rotación
        /// </summary>
        public int Insertar(Rotacion rotacion)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(
                    @"INSERT INTO TD_ROTACIONES 
                      (cdArchivoPagina, snRotacionAutomatica, nuRotacionAplicada, 
                       snRotacionManual, feRotacion, cdUsuario)
                      OUTPUT INSERTED.cdRotacion
                      VALUES 
                      (@cdArchivoPagina, @snRotacionAutomatica, @nuRotacionAplicada,
                       @snRotacionManual, GETDATE(), @cdUsuario)", conexion);

                comando.Parameters.AddWithValue("@cdArchivoPagina", rotacion.CdArchivoPagina);
                comando.Parameters.AddWithValue("@snRotacionAutomatica", rotacion.SnRotacionAutomatica);
                comando.Parameters.AddWithValue("@nuRotacionAplicada", rotacion.NuRotacionAplicada);
                comando.Parameters.AddWithValue("@snRotacionManual", rotacion.SnRotacionManual);
                comando.Parameters.AddWithValue("@cdUsuario", rotacion.CdUsuario);

                conexion.Open();
                var resultado = comando.ExecuteScalar();
                return resultado != null ? Convert.ToInt32(resultado) : 0;
            }
        }

        /// <summary>
        /// Inserta un lote de rotaciones
        /// </summary>
        public int InsertarLote(List<Rotacion> rotaciones)
        {
            int insertados = 0;

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                foreach (var rotacion in rotaciones)
                {
                    var comando = new SqlCommand(
                        @"INSERT INTO TD_ROTACIONES 
                          (cdArchivoPagina, snRotacionAutomatica, nuRotacionAplicada, 
                           snRotacionManual, feRotacion, cdUsuario)
                          VALUES 
                          (@cdArchivoPagina, @snRotacionAutomatica, @nuRotacionAplicada,
                           @snRotacionManual, GETDATE(), @cdUsuario)", conexion);

                    comando.Parameters.AddWithValue("@cdArchivoPagina", rotacion.CdArchivoPagina);
                    comando.Parameters.AddWithValue("@snRotacionAutomatica", rotacion.SnRotacionAutomatica);
                    comando.Parameters.AddWithValue("@nuRotacionAplicada", rotacion.NuRotacionAplicada);
                    comando.Parameters.AddWithValue("@snRotacionManual", rotacion.SnRotacionManual);
                    comando.Parameters.AddWithValue("@cdUsuario", rotacion.CdUsuario);

                    comando.ExecuteNonQuery();
                    insertados++;
                }
            }

            return insertados;
        }

        /// <summary>
        /// Obtiene todas las rotaciones de una página
        /// </summary>
        public List<Rotacion> ObtenerPorPagina(int cdArchivoPagina)
        {
            var rotaciones = new List<Rotacion>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(
                    @"SELECT r.cdRotacion, r.cdArchivoPagina, r.snRotacionAutomatica, 
                             r.nuRotacionAplicada, r.snRotacionManual, r.feRotacion, r.cdUsuario,
                             p.dsNombreArchivoPagina, u.dsUsuario
                      FROM TD_ROTACIONES r
                      INNER JOIN TD_ARCHIVOS_PAGINAS p ON r.cdArchivoPagina = p.cdArchivoPagina
                      INNER JOIN TD_USUARIOS u ON r.cdUsuario = u.cdUsuario
                      WHERE r.cdArchivoPagina = @cdArchivoPagina
                      ORDER BY r.feRotacion DESC", conexion);

                comando.Parameters.AddWithValue("@cdArchivoPagina", cdArchivoPagina);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        rotaciones.Add(new Rotacion
                        {
                            CdRotacion = lector.GetInt32(0),
                            CdArchivoPagina = lector.GetInt32(1),
                            SnRotacionAutomatica = lector.GetString(2),
                            NuRotacionAplicada = lector.GetInt32(3),
                            SnRotacionManual = lector.GetString(4),
                            FeRotacion = lector.GetDateTime(5),
                            CdUsuario = lector.GetInt32(6),
                            DsArchivoPagina = lector.GetString(7),
                            DsUsuario = lector.GetString(8)
                        });
                    }
                }
            }

            return rotaciones;
        }
    }
}
