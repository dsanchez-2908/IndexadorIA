using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    public class ArchivoPaginaDAL
    {
        private readonly string _cadenaConexion;

        public ArchivoPaginaDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene la siguiente secuencia global de nombres de archivo
        /// </summary>
        /// <param name="cantidad">Cantidad de secuencias a reservar</param>
        /// <returns>Número inicial de la secuencia</returns>
        public int ObtenerSiguienteSecuencia(int cantidad = 1)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand("SP_OBTENER_SIGUIENTE_SECUENCIA", conexion)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                comando.Parameters.AddWithValue("@cantidadSolicitada", cantidad);
                var parametroSalida = new SqlParameter("@secuenciaInicial", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                comando.Parameters.Add(parametroSalida);

                conexion.Open();
                comando.ExecuteNonQuery();

                return (int)parametroSalida.Value;
            }
        }

        /// <summary>
        /// Inserta un lote de páginas de archivo
        /// </summary>
        public int InsertarLote(List<ArchivoPagina> paginas)
        {
            int insertados = 0;

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                foreach (var pagina in paginas)
                {
                    var comando = new SqlCommand(
                        @"INSERT INTO TD_ARCHIVOS_PAGINAS 
                          (cdArchivoOriginal, nuPagina, dsNombreArchivoPagina, dsRutaCompleta, 
                           snGirada, snPosibleBlanca, dsProceso, cdEstado, feAlta, cdUsuarioAlta)
                          OUTPUT INSERTED.cdArchivoPagina
                          VALUES 
                          (@cdArchivoOriginal, @nuPagina, @dsNombreArchivoPagina, @dsRutaCompleta,
                           @snGirada, @snPosibleBlanca, @dsProceso, @cdEstado, GETDATE(), @cdUsuarioAlta)", conexion);

                    comando.Parameters.AddWithValue("@cdArchivoOriginal", pagina.CdArchivoOriginal);
                    comando.Parameters.AddWithValue("@nuPagina", pagina.NuPagina);
                    comando.Parameters.AddWithValue("@dsNombreArchivoPagina", pagina.DsNombreArchivoPagina);
                    comando.Parameters.AddWithValue("@dsRutaCompleta", pagina.DsRutaCompleta);
                    comando.Parameters.AddWithValue("@snGirada", pagina.SnGirada);
                    comando.Parameters.AddWithValue("@snPosibleBlanca", pagina.SnPosibleBlanca);
                    comando.Parameters.AddWithValue("@dsProceso", pagina.DsProceso);
                    comando.Parameters.AddWithValue("@cdEstado", pagina.CdEstado);
                    comando.Parameters.AddWithValue("@cdUsuarioAlta", pagina.CdUsuarioAlta);

                    // Obtener el ID generado y asignarlo a la entidad
                    var cdArchivoPagina = comando.ExecuteScalar();
                    if (cdArchivoPagina != null)
                    {
                        pagina.CdArchivoPagina = Convert.ToInt32(cdArchivoPagina);
                    }

                    insertados++;
                }
            }

            return insertados;
        }

        /// <summary>
        /// Obtiene todas las páginas de un archivo original
        /// </summary>
        public List<ArchivoPagina> ObtenerPorArchivoOriginal(int cdArchivoOriginal)
        {
            var paginas = new List<ArchivoPagina>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(
                    @"SELECT p.cdArchivoPagina, p.cdArchivoOriginal, p.nuPagina, p.dsNombreArchivoPagina, 
                             p.dsRutaCompleta, p.snGirada, p.snPosibleBlanca, p.dsProceso, p.cdEstado, 
                             p.feAlta, p.cdUsuarioAlta,
                             a.dsNombreArchivo, e.dsEstado
                      FROM TD_ARCHIVOS_PAGINAS p
                      INNER JOIN TD_ARCHIVOS_ORIGINAL a ON p.cdArchivoOriginal = a.cdArchivo
                      INNER JOIN TD_ESTADOS e ON p.dsProceso = e.dsProceso AND p.cdEstado = e.cdEstado
                      WHERE p.cdArchivoOriginal = @cdArchivoOriginal
                      ORDER BY p.nuPagina", conexion);

                comando.Parameters.AddWithValue("@cdArchivoOriginal", cdArchivoOriginal);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        paginas.Add(new ArchivoPagina
                        {
                            CdArchivoPagina = lector.GetInt32(0),
                            CdArchivoOriginal = lector.GetInt32(1),
                            NuPagina = lector.GetInt32(2),
                            DsNombreArchivoPagina = lector.GetString(3),
                            DsRutaCompleta = lector.GetString(4),
                            SnGirada = lector.GetString(5),
                            SnPosibleBlanca = lector.GetString(6),
                            DsProceso = lector.GetString(7),
                            CdEstado = lector.GetInt32(8),
                            FeAlta = lector.GetDateTime(9),
                            CdUsuarioAlta = lector.GetInt32(10),
                            DsArchivoOriginal = lector.GetString(11),
                            DsEstado = lector.GetString(12)
                        });
                    }
                }
            }

            return paginas;
        }

        /// <summary>
        /// Actualiza el estado de un archivo/página
        /// </summary>
        public int ActualizarEstado(int cdArchivoPagina, int cdEstado, int cdUsuario)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(@"
                    UPDATE TD_ARCHIVOS_PAGINAS 
                    SET cdEstado = @cdEstado
                    WHERE cdArchivoPagina = @cdArchivoPagina", conexion);

                comando.Parameters.AddWithValue("@cdArchivoPagina", cdArchivoPagina);
                comando.Parameters.AddWithValue("@cdEstado", cdEstado);

                conexion.Open();
                return comando.ExecuteNonQuery();
            }
        }
    }
}
