using Microsoft.Data.SqlClient;
using IndexadorIA.Entidades;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Capa de acceso a datos para TD_AYUDA_CONTROL: texto de ayuda global configurado
    /// por el administrador para cada uno de los 9 campos del panel de detalle de FrmVerLote.
    /// </summary>
    public class AyudaControlDAL
    {
        private readonly string _cadenaConexion;

        public AyudaControlDAL()
        {
            _cadenaConexion = Configuracion.CadenaConexion;
        }

        /// <summary>
        /// Obtiene el único registro de ayuda de control, o null si todavía no fue configurado.
        /// </summary>
        public AyudaControl? Obtener()
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            var comando = new SqlCommand(@"
                SELECT TOP 1
                    cdAyudaControl, dsCategoriaPlano, dsTipoPlano, dsExpediente, dsSeccion,
                    dsManzana, dsParcela, dsDireccion, dsNumeroPlano, dsObservaciones
                FROM TD_AYUDA_CONTROL
                ORDER BY cdAyudaControl", conexion);

            conexion.Open();
            using var reader = comando.ExecuteReader();
            if (!reader.Read())
                return null;

            return new AyudaControl
            {
                CdAyudaControl = reader.GetInt32(0),
                DsCategoriaPlano = reader.IsDBNull(1) ? null : reader.GetString(1),
                DsTipoPlano = reader.IsDBNull(2) ? null : reader.GetString(2),
                DsExpediente = reader.IsDBNull(3) ? null : reader.GetString(3),
                DsSeccion = reader.IsDBNull(4) ? null : reader.GetString(4),
                DsManzana = reader.IsDBNull(5) ? null : reader.GetString(5),
                DsParcela = reader.IsDBNull(6) ? null : reader.GetString(6),
                DsDireccion = reader.IsDBNull(7) ? null : reader.GetString(7),
                DsNumeroPlano = reader.IsDBNull(8) ? null : reader.GetString(8),
                DsObservaciones = reader.IsDBNull(9) ? null : reader.GetString(9)
            };
        }

        /// <summary>
        /// Guarda (inserta o actualiza) el único registro de ayuda de control.
        /// </summary>
        public void Guardar(AyudaControl ayuda)
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            conexion.Open();

            int? cdExistente;
            using (var comandoBuscar = new SqlCommand("SELECT TOP 1 cdAyudaControl FROM TD_AYUDA_CONTROL ORDER BY cdAyudaControl", conexion))
            {
                var resultado = comandoBuscar.ExecuteScalar();
                cdExistente = resultado == null ? null : Convert.ToInt32(resultado);
            }

            SqlCommand comando;
            if (cdExistente.HasValue)
            {
                comando = new SqlCommand(@"
                    UPDATE TD_AYUDA_CONTROL SET
                        dsCategoriaPlano = @dsCategoriaPlano,
                        dsTipoPlano = @dsTipoPlano,
                        dsExpediente = @dsExpediente,
                        dsSeccion = @dsSeccion,
                        dsManzana = @dsManzana,
                        dsParcela = @dsParcela,
                        dsDireccion = @dsDireccion,
                        dsNumeroPlano = @dsNumeroPlano,
                        dsObservaciones = @dsObservaciones
                    WHERE cdAyudaControl = @cdAyudaControl", conexion);
                comando.Parameters.AddWithValue("@cdAyudaControl", cdExistente.Value);
            }
            else
            {
                comando = new SqlCommand(@"
                    INSERT INTO TD_AYUDA_CONTROL
                        (dsCategoriaPlano, dsTipoPlano, dsExpediente, dsSeccion, dsManzana, dsParcela, dsDireccion, dsNumeroPlano, dsObservaciones)
                    VALUES
                        (@dsCategoriaPlano, @dsTipoPlano, @dsExpediente, @dsSeccion, @dsManzana, @dsParcela, @dsDireccion, @dsNumeroPlano, @dsObservaciones)", conexion);
            }

            comando.Parameters.AddWithValue("@dsCategoriaPlano", (object?)ayuda.DsCategoriaPlano ?? DBNull.Value);
            comando.Parameters.AddWithValue("@dsTipoPlano", (object?)ayuda.DsTipoPlano ?? DBNull.Value);
            comando.Parameters.AddWithValue("@dsExpediente", (object?)ayuda.DsExpediente ?? DBNull.Value);
            comando.Parameters.AddWithValue("@dsSeccion", (object?)ayuda.DsSeccion ?? DBNull.Value);
            comando.Parameters.AddWithValue("@dsManzana", (object?)ayuda.DsManzana ?? DBNull.Value);
            comando.Parameters.AddWithValue("@dsParcela", (object?)ayuda.DsParcela ?? DBNull.Value);
            comando.Parameters.AddWithValue("@dsDireccion", (object?)ayuda.DsDireccion ?? DBNull.Value);
            comando.Parameters.AddWithValue("@dsNumeroPlano", (object?)ayuda.DsNumeroPlano ?? DBNull.Value);
            comando.Parameters.AddWithValue("@dsObservaciones", (object?)ayuda.DsObservaciones ?? DBNull.Value);

            using (comando)
            {
                comando.ExecuteNonQuery();
            }
        }
    }
}
