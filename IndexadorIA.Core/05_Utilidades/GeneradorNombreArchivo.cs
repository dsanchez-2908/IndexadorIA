using System.Text;

namespace IndexadorIA.Utilidades
{
    /// <summary>
    /// Utilidad para normalizar y construir nombres de archivo finales al mover
    /// los PDFs de un lote hacia la subcarpeta "Planos", según las reglas de negocio:
    /// - Todo en mayúsculas
    /// - Sin espacios en blanco (reemplazados por "-")
    /// - "/" reemplazado por "-"
    /// - "°", "." y "," se quitan (se reemplazan por vacío)
    /// - Resto de caracteres especiales no válidos en nombres de archivo eliminados/reemplazados
    /// - Colisiones de nombre resueltas agregando (1), (2), (3)...
    /// </summary>
    public static class GeneradorNombreArchivo
    {
        /// <summary>
        /// Normaliza un valor de texto para ser usado como parte de un nombre de archivo:
        /// mayúsculas, sin espacios (reemplazados por -), "/" reemplazado por "-",
        /// "°", "." y "," eliminados, y sin caracteres inválidos para nombres de archivo.
        /// </summary>
        public static string Normalizar(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            string texto = valor.Trim().ToUpperInvariant();
            texto = texto.Replace("/", "-");
            texto = texto.Replace("°", "").Replace(".", "").Replace(",", "");

            var invalidos = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(texto.Length);

            foreach (char c in texto)
            {
                if (char.IsWhiteSpace(c))
                {
                    sb.Append('-');
                }
                else if (invalidos.Contains(c) || c == '\\' || c == ':' || c == '*' ||
                         c == '?' || c == '"' || c == '<' || c == '>' || c == '|')
                {
                    // Caracter especial no permitido en nombres de archivo: se omite
                }
                else
                {
                    sb.Append(c);
                }
            }

            // Colapsar múltiples guiones consecutivos generados por reemplazos
            string resultado = sb.ToString();
            while (resultado.Contains("--"))
                resultado = resultado.Replace("--", "-");

            return resultado.Trim('-');
        }

        /// <summary>
        /// Nombre de la categoria "CATASTRO" tal cual se usa en TD_CATEGORIA_PLANO.
        /// </summary>
        public const string CategoriaCatastro = "CATASTRO";

        /// <summary>
        /// Determina si una categoria de plano corresponde a Catastro (case-insensitive).
        /// </summary>
        public static bool EsCategoriaCatastro(string? dsCategoriaPlano)
        {
            return !string.IsNullOrWhiteSpace(dsCategoriaPlano)
                && dsCategoriaPlano.Trim().Equals(CategoriaCatastro, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Construye el nombre de archivo final para un registro controlado
        /// (estados 1 - Pendiente de Control o 2 - Controlado), aplicando la
        /// estructura correspondiente segun la categoria del plano:
        /// - OBRAS/INSTALACIONES: [CATEGORIA]_[TIPO]_[DIRECCION]_[SECCION]-[MANZANA]-[PARCELA]_[EXPEDIENTE].pdf
        /// - CATASTRO: CATASTRO_[NUMERO DE PLANO]_[DIRECCION]_[SECCION]-[MANZANA]-[PARCELA]_[EXPEDIENTE].pdf
        /// La nomenclatura (Seccion-Manzana-Parcela) se une con guion medio "-".
        /// </summary>
        public static string ConstruirNombreControlado(string? dsCategoriaPlano, string? tipoPlano,
            string? numeroPlano, string? direccion, string? seccion, string? manzana, string? parcela,
            string? expediente)
        {
            // La direccion, seccion, manzana y parcela se usan tal cual vienen de la base
            // de datos, sin normalizar (sin mayusculas forzadas, sin reemplazo de espacios
            // o caracteres especiales).
            string dir = direccion ?? string.Empty;
            string sec = seccion ?? string.Empty;
            string mza = manzana ?? string.Empty;
            string parc = parcela ?? string.Empty;
            string exp = Normalizar(expediente);

            // Nomenclatura: Seccion-Manzana-Parcela unidos con guion medio "-"
            string nomenclatura = $"{sec}-{mza}-{parc}";

            string nombre;

            if (EsCategoriaCatastro(dsCategoriaPlano))
            {
                string numero = Normalizar(numeroPlano);
                nombre = $"{CategoriaCatastro}_{numero}_{dir}_{nomenclatura}_{exp}";
            }
            else
            {
                string categoria = Normalizar(dsCategoriaPlano);
                string tipo = Normalizar(tipoPlano);
                nombre = $"{categoria}_{tipo}_{dir}_{nomenclatura}_{exp}";
            }

            // Colapsar guiones bajos repetidos generados por campos vacios
            while (nombre.Contains("__"))
                nombre = nombre.Replace("__", "_");

            nombre = nombre.Trim('_');

            return nombre + ".pdf";
        }

        /// <summary>
        /// Construye el nombre de archivo final para un registro en estado ilegible
        /// (estados 3 - Página Ilegible o 4 - Datos Ilegibles), reutilizando la misma
        /// estructura que el nombre controlado pero agregando el sufijo _ILEGIBLE.
        /// Los campos pueden venir vacios si no se pudieron extraer.
        /// </summary>
        public static string ConstruirNombreIlegible(string? dsCategoriaPlano, string? tipoPlano,
            string? numeroPlano, string? direccion, string? seccion, string? manzana, string? parcela,
            string? expediente)
        {
            string sinExtension = ConstruirNombreControlado(dsCategoriaPlano, tipoPlano, numeroPlano,
                direccion, seccion, manzana, parcela, expediente);

            sinExtension = Path.GetFileNameWithoutExtension(sinExtension);

            return $"{sinExtension}_ILEGIBLE.pdf";
        }

        /// <summary>
        /// Dada una carpeta destino y un nombre de archivo propuesto, devuelve un nombre
        /// único agregando (1), (2), (3)... si ya existe un archivo con ese nombre.
        /// </summary>
        public static string ResolverColision(string carpetaDestino, string nombreArchivo)
        {
            string extension = Path.GetExtension(nombreArchivo);
            string nombreBase = Path.GetFileNameWithoutExtension(nombreArchivo);

            string candidato = nombreArchivo;
            int contador = 1;

            while (File.Exists(Path.Combine(carpetaDestino, candidato)))
            {
                candidato = $"{nombreBase}({contador}){extension}";
                contador++;
            }

            return candidato;
        }
    }
}
