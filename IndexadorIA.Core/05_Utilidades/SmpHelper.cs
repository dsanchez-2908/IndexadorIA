using System.Text.RegularExpressions;
using IndexadorIA.Entidades;

namespace IndexadorIA.Utilidades
{
    /// <summary>
    /// Valida y arma el SMP (Seccion-Manzana-Parcela) a consultar en la API.
    /// Cada parte debe tener entre 3 y 4 caracteres alfanumericos y un unico valor.
    /// </summary>
    public static class SmpHelper
    {
        private static readonly Regex ParteValida = new(@"^[\p{L}\d]{3,4}$", RegexOptions.Compiled);

        /// <summary>Completa DsSmp (si es valido) o DsMotivoOmision (si debe omitirse).</summary>
        public static void Evaluar(SmpPendiente p)
        {
            p.DsSmp = null;
            p.DsMotivoOmision = null;

            string? seccion = Normalizar(p.DsSeccion);
            string? manzana = Normalizar(p.DsManzana);
            string? parcela = Normalizar(p.DsParcela);

            var faltantes = new List<string>();
            if (seccion == null) faltantes.Add("seccion");
            if (manzana == null) faltantes.Add("manzana");
            if (parcela == null) faltantes.Add("parcela");
            if (faltantes.Count > 0)
            {
                p.DsMotivoOmision = "Falta: " + string.Join(", ", faltantes);
                return;
            }

            var invalidos = new List<string>();
            Validar("seccion", seccion!, invalidos);
            Validar("manzana", manzana!, invalidos);
            Validar("parcela", parcela!, invalidos);
            if (invalidos.Count > 0)
            {
                p.DsMotivoOmision = string.Join("; ", invalidos);
                return;
            }

            p.DsSmp = $"{seccion}-{manzana}-{parcela}";
        }

        private static string? Normalizar(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return null;
            return valor.Replace(" ", string.Empty).ToUpperInvariant();
        }

        private static void Validar(string nombre, string valor, List<string> invalidos)
        {
            if (valor.IndexOfAny(new[] { '.', '-', '/', ',', ';', '_', '\\' }) >= 0)
            {
                invalidos.Add($"{nombre} con mas de un valor ({valor})");
            }
            else if (!ParteValida.IsMatch(valor))
            {
                invalidos.Add($"{nombre} debe tener 3 o 4 caracteres ({valor})");
            }
        }
    }
}
