using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    public class ArchivoOriginalDAL
    {
        private readonly string _cadenaConexion;

        public ArchivoOriginalDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        public int InsertarLote(List<ArchivoOriginal> archivos)
        {
            int insertados = 0;

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                foreach (var archivo in archivos)
                {
                    var comando = new SqlCommand(
                        @"INSERT INTO TD_ARCHIVOS_ORIGINAL 
                          (cdProyecto, dsNombreArchivo, dsExtension, dsRutaCompleta, dsNombreUltimaCarpeta,
                           nuCantidadPaginas, dsProceso, cdEstadoArchivo, feAlta, cdUsuarioAlta, nuTamanoBytes, feModificacionArchivo)
                          VALUES 
                          (@cdProyecto, @dsNombreArchivo, @dsExtension, @dsRutaCompleta, @dsNombreUltimaCarpeta,
                           @nuCantidadPaginas, @dsProceso, @cdEstadoArchivo, GETDATE(), @cdUsuarioAlta, @nuTamanoBytes, @feModificacionArchivo)", conexion);

                    comando.Parameters.AddWithValue("@cdProyecto", archivo.CdProyecto);
                    comando.Parameters.AddWithValue("@dsNombreArchivo", archivo.DsNombreArchivo);
                    comando.Parameters.AddWithValue("@dsExtension", archivo.DsExtension);
                    comando.Parameters.AddWithValue("@dsRutaCompleta", archivo.DsRutaCompleta);
                    comando.Parameters.AddWithValue("@dsNombreUltimaCarpeta", (object?)archivo.DsNombreUltimaCarpeta ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@nuCantidadPaginas", archivo.NuCantidadPaginas);
                    comando.Parameters.AddWithValue("@dsProceso", archivo.DsProceso);
                    comando.Parameters.AddWithValue("@cdEstadoArchivo", archivo.CdEstadoArchivo);
                    comando.Parameters.AddWithValue("@cdUsuarioAlta", archivo.CdUsuarioAlta);
                    comando.Parameters.AddWithValue("@nuTamanoBytes", archivo.NuTamanoBytes);
                    comando.Parameters.AddWithValue("@feModificacionArchivo", archivo.FeModificacionArchivo);

                    comando.ExecuteNonQuery();
                    insertados++;
                }
            }

            return insertados;
        }

        public List<ArchivoOriginal> ObtenerPorProyecto(int cdProyecto)
        {
            var archivos = new List<ArchivoOriginal>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                var comando = new SqlCommand(
                    @"SELECT a.cdArchivo, a.cdProyecto, a.dsNombreArchivo, a.dsExtension, a.dsRutaCompleta,
                             a.dsNombreUltimaCarpeta, a.nuCantidadPaginas, a.dsProceso, a.cdEstadoArchivo, a.feAlta,
                             a.cdUsuarioAlta, a.feUltimaModificacion, a.cdUsuarioModificacion,
                             a.nuTamanoBytes, a.feModificacionArchivo,
                             p.dsProyecto, e.dsEstado
                      FROM TD_ARCHIVOS_ORIGINAL a
                      INNER JOIN TD_PROYECTOS p ON a.cdProyecto = p.cdProyecto
                      INNER JOIN TD_ESTADOS e ON a.dsProceso = e.dsProceso AND a.cdEstadoArchivo = e.cdEstado
                      WHERE a.cdProyecto = @cdProyecto
                      ORDER BY a.feAlta DESC", conexion);

                comando.Parameters.AddWithValue("@cdProyecto", cdProyecto);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        archivos.Add(new ArchivoOriginal
                        {
                            CdArchivo = lector.GetInt32(0),
                            CdProyecto = lector.GetInt32(1),
                            DsNombreArchivo = lector.GetString(2),
                            DsExtension = lector.GetString(3),
                            DsRutaCompleta = lector.GetString(4),
                            DsNombreUltimaCarpeta = lector.IsDBNull(5) ? null : lector.GetString(5),
                            NuCantidadPaginas = lector.GetInt32(6),
                            DsProceso = lector.GetString(7),
                            CdEstadoArchivo = lector.GetInt32(8),
                            FeAlta = lector.GetDateTime(9),
                            CdUsuarioAlta = lector.GetInt32(10),
                            FeUltimaModificacion = lector.IsDBNull(11) ? null : lector.GetDateTime(11),
                            CdUsuarioModificacion = lector.IsDBNull(12) ? null : lector.GetInt32(12),
                            NuTamanoBytes = lector.GetInt64(13),
                            FeModificacionArchivo = lector.GetDateTime(14),
                            DsProyecto = lector.GetString(15),
                            DsEstado = lector.GetString(16)
                        });
                    }
                }
            }

            return archivos;
        }
    }
}
