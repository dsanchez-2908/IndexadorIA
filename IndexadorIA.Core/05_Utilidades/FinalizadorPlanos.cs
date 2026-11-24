using System.Text;
using IndexadorIA.Entidades;
using iText.Kernel.Pdf;

namespace IndexadorIA.Utilidades
{
    /// <summary>Genera los PDF finales (unificando paginas) y registra cada uno en el CSV indice de la carpeta.</summary>
    public static class FinalizadorPlanos
    {
        public const string CarpetaProcesados = "Planos Procesados FINAL";
        public const string CarpetaIlegibles = "Planos ILEGIBLES FINAL";
        private const int MaxLargoDireccionEnNombre = 100;
        private const string Separador = ";";
        private static readonly object _lock = new();

        public class ResultadoFinal
        {
            public string RutaPdf { get; set; } = string.Empty;
            public string NombreNuevo { get; set; } = string.Empty;
            public string Carpeta { get; set; } = string.Empty;
            public string RutaCsv { get; set; } = string.Empty;
        }

        /// <summary>Crea el PDF con las paginas indicadas (en orden de pagina) y agrega la linea al CSV.</summary>
        public static ResultadoFinal Generar(string rutaArchivoOriginal, string nombreArchivoOriginal,
            IReadOnlyList<PaginaFinalDto> paginas)
        {
            if (paginas.Count == 0) throw new InvalidOperationException("No hay paginas para generar el plano.");

            var m = paginas[0];
            bool ilegible = m.EsIlegible;
            string carpetaMadre = Path.GetDirectoryName(rutaArchivoOriginal)
                ?? throw new InvalidOperationException("Ruta del archivo madre invalida.");
            string nombreCarpeta = ilegible ? CarpetaIlegibles : CarpetaProcesados;
            string carpeta = Path.Combine(carpetaMadre, nombreCarpeta);
            string rutaCsv = Path.Combine(carpetaMadre, nombreCarpeta + ".csv");

            string baseNombre = ConstruirNombreBase(m, ilegible);

            lock (_lock)
            {
                Directory.CreateDirectory(carpeta);
                string nombre = NombreUnico(carpeta, baseNombre);
                string rutaPdf = Path.Combine(carpeta, nombre);

                UnirPdf(paginas.OrderBy(p => p.NuPagina).Select(p => p.DsRutaPagina).ToList(), rutaPdf);

                try
                {
                    AgregarLineaCsv(rutaCsv, rutaArchivoOriginal, nombreArchivoOriginal, nombre, paginas);
                }
                catch
                {
                    TryDelete(rutaPdf);
                    throw;
                }

                return new ResultadoFinal { RutaPdf = rutaPdf, NombreNuevo = nombre, Carpeta = carpeta, RutaCsv = rutaCsv };
            }
        }

        /// <summary>Deshace un plano generado (borra el PDF y su ultima linea del CSV).</summary>
        public static void Deshacer(ResultadoFinal r)
        {
            lock (_lock)
            {
                TryDelete(r.RutaPdf);
                try
                {
                    if (!File.Exists(r.RutaCsv)) return;
                    var lineas = File.ReadAllLines(r.RutaCsv, Encoding.UTF8).ToList();
                    int idx = lineas.FindLastIndex(l => l.Contains(Escapar(r.NombreNuevo)));
                    if (idx > 0)
                    {
                        lineas.RemoveAt(idx);
                        File.WriteAllLines(r.RutaCsv, lineas, new UTF8Encoding(true));
                    }
                }
                catch { /* mejor esfuerzo */ }
            }
        }

        public static string ConstruirNombreBase(PaginaFinalDto m, bool ilegible)
        {
            var partes = new List<string>();
            void Agregar(string? s)
            {
                string v = Limpiar(s);
                if (v.Length > 0) partes.Add(v);
            }

            Agregar(m.DsCategoria);
            Agregar(m.DsNumeroPlano);
            string direccion = Limpiar(m.DsDireccion);
            if (direccion.Length > MaxLargoDireccionEnNombre) direccion = direccion[..MaxLargoDireccionEnNombre].TrimEnd();
            Agregar(direccion);
            Agregar($"{Limpiar(m.DsSeccion)}-{Limpiar(m.DsManzana)}-{Limpiar(m.DsParcela)}");
            Agregar(m.DsExpediente);

            string nombre = string.Join("_", partes);
            if (ilegible) nombre += "_ILEGIBLE";
            return nombre + ".pdf";
        }

        private static string Limpiar(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            var sb = new StringBuilder();
            var invalidos = Path.GetInvalidFileNameChars();
            foreach (char c in s.Trim()) sb.Append(Array.IndexOf(invalidos, c) >= 0 ? '-' : c);
            return sb.ToString().Trim();
        }

        private static string NombreUnico(string carpeta, string nombrePdf)
        {
            if (!File.Exists(Path.Combine(carpeta, nombrePdf))) return nombrePdf;
            string baseSin = Path.GetFileNameWithoutExtension(nombrePdf);
            for (int i = 1; ; i++)
            {
                string candidato = $"{baseSin} ({i}).pdf";
                if (!File.Exists(Path.Combine(carpeta, candidato))) return candidato;
            }
        }

        private static void UnirPdf(IReadOnlyList<string> origenes, string destino)
        {
            foreach (string o in origenes)
            {
                if (!File.Exists(o)) throw new FileNotFoundException($"No se encontro el PDF de la pagina: {o}");
            }

            try
            {
                using var writer = new PdfWriter(destino);
                using var pdf = new PdfDocument(writer);
                foreach (string o in origenes)
                {
                    using var lector = new PdfDocument(new PdfReader(o));
                    lector.CopyPagesTo(1, lector.GetNumberOfPages(), pdf);
                }
            }
            catch
            {
                TryDelete(destino);
                throw;
            }
        }

        private const int SegmentosRaizOmitidos = 4;

        /// <summary>Quita la raiz (P:\PROYECTOS ACTIVOS\DGROC\PLANOS - DGROC) y el nombre del archivo madre: \6ta entrega\PARTE 4\</summary>
        private static string RutaRelativaMadre(string rutaArchivoMadre)
        {
            string? dir = Path.GetDirectoryName(rutaArchivoMadre);
            if (string.IsNullOrEmpty(dir)) return string.Empty;
            var partes = dir.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Skip(SegmentosRaizOmitidos);
            string rel = string.Join("\\", partes);
            return rel.Length == 0 ? "\\" : "\\" + rel + "\\";
        }

        private static void AgregarLineaCsv(string rutaCsv, string rutaMadre, string nombreOriginal, string nombreNuevo,
            IReadOnlyList<PaginaFinalDto> paginas)
        {
            var m = paginas[0];
            string observaciones = string.Join(" | ", paginas
                .Select(p => (p.DsObservaciones ?? string.Empty).Trim())
                .Where(o => o.Length > 0).Distinct());

            var campos = new[]
            {
                RutaRelativaMadre(rutaMadre), nombreOriginal, nombreNuevo, m.DsCategoria, m.DsTipo, m.DsAcronimo,
                m.DsDireccion, m.DsSeccion, m.DsManzana, m.DsParcela, m.DsExpediente, m.DsNumeroPlano, observaciones
            };

            bool nuevo = !File.Exists(rutaCsv) || new FileInfo(rutaCsv).Length == 0;
            using var sw = new StreamWriter(rutaCsv, append: true, new UTF8Encoding(true));
            if (nuevo)
            {
                sw.WriteLine(string.Join(Separador, new[]
                {
                    "Ruta", "Nombre de archivo original", "Nombre Nuevo", "Categoria", "Tipo de Plano", "Acronimo",
                    "Direccion", "Seccion", "Manzana", "Parcela", "Expediente", "Numero de Plano", "Observaciones"
                }));
            }
            sw.WriteLine(string.Join(Separador, campos.Select(Escapar)));
        }

        private static string Escapar(string? v)
        {
            v ??= string.Empty;
            v = v.Replace("\r", " ").Replace("\n", " ");
            return v.Contains(Separador) || v.Contains('"') ? $"\"{v.Replace("\"", "\"\"")}\"" : v;
        }

        private static void TryDelete(string ruta)
        {
            try { if (File.Exists(ruta)) File.Delete(ruta); } catch { /* mejor esfuerzo */ }
        }
    }
}
