using System.Security.Cryptography;
using System.Text;

namespace IndexadorIA.Datos
{
    /// <summary>
    /// Clase para operaciones de seguridad y encriptación
    /// </summary>
    public static class Seguridad
    {
        /// <summary>
        /// Encripta una cadena usando SHA256
        /// </summary>
        /// <param name="texto">Texto a encriptar</param>
        /// <returns>Hash SHA256 del texto</returns>
        public static string EncriptarSHA256(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Genera una clave temporal aleatoria
        /// </summary>
        /// <param name="longitud">Longitud de la clave</param>
        /// <returns>Clave temporal generada</returns>
        public static string GenerarClaveTemporal(int longitud = 8)
        {
            const string caracteresValidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder resultado = new StringBuilder();
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (longitud-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    resultado.Append(caracteresValidos[(int)(num % (uint)caracteresValidos.Length)]);
                }
            }
            return resultado.ToString();
        }
    }
}
