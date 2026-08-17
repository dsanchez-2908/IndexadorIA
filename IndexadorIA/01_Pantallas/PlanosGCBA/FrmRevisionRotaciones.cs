using IndexadorIA.Utilidades;
using System.Linq;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    public partial class FrmRevisionRotaciones : Form
    {
        /// <summary>
        /// Datos de una página a revisar: su PDF real en disco y la imagen temporal
        /// (PNG) usada para previsualizarla en esta pantalla.
        /// </summary>
        public class PaginaRevision
        {
            public string RutaPdf { get; set; } = string.Empty;
            public string RutaImagen { get; set; } = string.Empty;
            public string NombreArchivo { get; set; } = string.Empty;

            /// <summary>Grados adicionales acumulados rotados manualmente en esta pantalla.</summary>
            public int GradosRotadosManualmente { get; set; }
        }

        private const int PaginasPorPantalla = 10;

        private readonly List<PaginaRevision> _paginas;
        private int _indicePrimeraPaginaMostrada;

        public FrmRevisionRotaciones(List<PaginaRevision> paginas)
        {
            InitializeComponent();
            _paginas = paginas ?? new List<PaginaRevision>();
            _indicePrimeraPaginaMostrada = 0;
        }

        private void FrmRevisionRotaciones_Load(object sender, EventArgs e)
        {
            MostrarPaginaActual();
        }

        private void MostrarPaginaActual()
        {
            pnlPaginas.SuspendLayout();
            pnlPaginas.Controls.Clear();

            int cantidadAMostrar = Math.Min(PaginasPorPantalla, _paginas.Count - _indicePrimeraPaginaMostrada);

            for (int i = 0; i < cantidadAMostrar; i++)
            {
                int indice = _indicePrimeraPaginaMostrada + i;
                var paginaRevision = _paginas[indice];

                var contenedor = CrearMiniatura(paginaRevision);

                // Grilla fija de 5 columnas x 2 filas: las primeras 5 miniaturas van
                // arriba y las siguientes 5 abajo.
                int columna = i % pnlPaginas.ColumnCount;
                int fila = i / pnlPaginas.ColumnCount;
                contenedor.Dock = DockStyle.Fill;
                pnlPaginas.Controls.Add(contenedor, columna, fila);
            }

            pnlPaginas.ResumeLayout();

            int totalPaginasDePantalla = _paginas.Count == 0
                ? 1
                : (int)Math.Ceiling(_paginas.Count / (double)PaginasPorPantalla);
            int paginaActualDePantalla = (_indicePrimeraPaginaMostrada / PaginasPorPantalla) + 1;

            lblPagina.Text = $"Página {paginaActualDePantalla} de {totalPaginasDePantalla} ({_paginas.Count} imagen(es) para revisar)";

            btnAnterior.Enabled = _indicePrimeraPaginaMostrada > 0;
            btnSiguiente.Enabled = _indicePrimeraPaginaMostrada + PaginasPorPantalla < _paginas.Count;
        }

        private Panel CrearMiniatura(PaginaRevision paginaRevision)
        {
            var panel = new Panel
            {
                Margin = new Padding(6),
                BackColor = Color.FromArgb(45, 45, 48),
                BorderStyle = BorderStyle.FixedSingle,
                Tag = paginaRevision
            };

            var lblNombre = new Label
            {
                Text = paginaRevision.NombreArchivo,
                ForeColor = Color.White,
                Dock = DockStyle.Bottom,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoEllipsis = true
            };

            var pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Hand,
                Tag = paginaRevision
            };

            try
            {
                using (var stream = new FileStream(paginaRevision.RutaImagen, FileMode.Open, FileAccess.Read))
                {
                    pictureBox.Image = Image.FromStream(stream);
                }
            }
            catch
            {
                // Si no se puede cargar la imagen, se deja el PictureBox vacío
            }

            pictureBox.Click += (s, e) => RotarPagina(paginaRevision, pictureBox);

            panel.Controls.Add(pictureBox);
            panel.Controls.Add(lblNombre);

            return panel;
        }

        private void RotarPagina(PaginaRevision paginaRevision, PictureBox pictureBox)
        {
            try
            {
                // Por rendimiento, NO se rota el PDF en disco en cada clic (eso implica
                // reescribir el archivo completo con iText7 y resulta muy lento si el
                // usuario gira varias páginas). Solo se acumulan los grados pendientes
                // y se rota la imagen en memoria para dar feedback inmediato. Los PDF
                // se rotan recién al presionar "Finalizar", en background y con barra
                // de progreso.
                paginaRevision.GradosRotadosManualmente = (paginaRevision.GradosRotadosManualmente + 90) % 360;

                var imagenAnterior = pictureBox.Image;
                if (imagenAnterior != null)
                {
                    var bitmap = new Bitmap(imagenAnterior);
                    bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    pictureBox.Image = bitmap;
                    imagenAnterior.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo rotar la página: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (_indicePrimeraPaginaMostrada - PaginasPorPantalla >= 0)
            {
                _indicePrimeraPaginaMostrada -= PaginasPorPantalla;
                MostrarPaginaActual();
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (_indicePrimeraPaginaMostrada + PaginasPorPantalla < _paginas.Count)
            {
                _indicePrimeraPaginaMostrada += PaginasPorPantalla;
                MostrarPaginaActual();
            }
        }

        private async void btnFinalizar_Click(object sender, EventArgs e)
        {
            var paginasConGiroPendiente = _paginas.Where(p => p.GradosRotadosManualmente != 0).ToList();

            if (paginasConGiroPendiente.Count > 0)
            {
                btnFinalizar.Enabled = false;
                btnAnterior.Enabled = false;
                btnSiguiente.Enabled = false;

                progressBarFinalizar.Visible = true;
                progressBarFinalizar.Minimum = 0;
                progressBarFinalizar.Maximum = paginasConGiroPendiente.Count;
                progressBarFinalizar.Value = 0;
                lblProgresoFinalizar.Visible = true;

                int procesadas = 0;
                int errores = 0;

                await Task.Run(() =>
                {
                    foreach (var paginaRevision in paginasConGiroPendiente)
                    {
                        try
                        {
                            ProcesamientoPaginas.RotarPaginaPdfEnDisco(paginaRevision.RutaPdf, paginaRevision.GradosRotadosManualmente);
                        }
                        catch (Exception ex)
                        {
                            errores++;
                            Datos.LogDAL.RegistrarLog(
                                Entidades.LogRegistro.Niveles.ERROR,
                                "FrmRevisionRotaciones.btnFinalizar_Click",
                                $"Error al aplicar rotación manual al PDF '{paginaRevision.RutaPdf}'",
                                ex);
                        }

                        procesadas++;
                        int procesadasCapturado = procesadas;
                        BeginInvoke(new Action(() =>
                        {
                            progressBarFinalizar.Value = Math.Min(procesadasCapturado, progressBarFinalizar.Maximum);
                            lblProgresoFinalizar.Text = $"Aplicando rotaciones: {procesadasCapturado} de {paginasConGiroPendiente.Count}...";
                        }));
                    }
                });

                if (errores > 0)
                {
                    MessageBox.Show(
                        $"Se aplicaron las rotaciones, pero {errores} página(s) tuvieron errores. Revise el log para más detalles.",
                        "Advertencia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }

            // Liberar las imágenes cargadas antes de borrar los archivos temporales
            foreach (Control control in pnlPaginas.Controls)
            {
                if (control is Panel panel)
                {
                    foreach (Control hijo in panel.Controls)
                    {
                        if (hijo is PictureBox pictureBox && pictureBox.Image != null)
                        {
                            pictureBox.Image.Dispose();
                            pictureBox.Image = null;
                        }
                    }
                }
            }

            ProcesamientoPaginas.LimpiarCarpetaRevisionGiro();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
