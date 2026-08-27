using IndexadorIA.Entidades;
using IndexadorIA.Negocio;
using IndexadorIA.Utilidades;
using PdfiumViewer;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    public partial class FrmPreparacionImagenes : Form
    {
        private readonly PreparacionImagenesBL _bl;
        private List<LoteGridDto> _lotes;
        private ConfiguracionRecorte _configuracion;
        private Bitmap? _bitmapPreview;

        public FrmPreparacionImagenes()
        {
            InitializeComponent();
            _bl = new PreparacionImagenesBL();
            _lotes = new List<LoteGridDto>();
            _configuracion = new ConfiguracionRecorte();
        }

        private void FrmPreparacionImagenes_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarEstilos();
                ConfigurarDataGridView();
                ConfigurarComboEsquinas();
                CargarConfiguracion();
                CargarDatos();
                ConfigurarEventos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario:\n\n{ex.Message}\n\nStackTrace:\n{ex.StackTrace}", 
                    "Error de carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarEstilos()
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;

            // Establecer fechas por defecto
            dtpDesde.Value = DateTime.Now.AddMonths(-1);
            dtpHasta.Value = DateTime.Now;
        }

        private void ConfigurarDataGridView()
        {
            dgvLotes.AutoGenerateColumns = false;
            dgvLotes.AllowUserToAddRows = false;
            dgvLotes.AllowUserToDeleteRows = false;
            dgvLotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLotes.MultiSelect = false;
            dgvLotes.ReadOnly = false;
            dgvLotes.RowHeadersVisible = false;

            // Estilos - Hacer más visible la grilla
            dgvLotes.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvLotes.ForeColor = Color.White;
            dgvLotes.GridColor = Color.FromArgb(60, 60, 63);

            // Headers
            dgvLotes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvLotes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLotes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvLotes.EnableHeadersVisualStyles = false;

            // Celdas - Fondo más claro para mejor visibilidad
            dgvLotes.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvLotes.DefaultCellStyle.ForeColor = Color.White;
            dgvLotes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvLotes.DefaultCellStyle.SelectionForeColor = Color.White;

            // Alternating row color para mejor legibilidad
            dgvLotes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(37, 37, 38);
            dgvLotes.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;

            // Columnas
            dgvLotes.Columns.Clear();

            // Columna de selección (checkbox)
            var colSeleccion = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Sel",
                Name = "colSeleccionado",
                DataPropertyName = "Seleccionado",
                Width = 60,
                ReadOnly = false
            };
            dgvLotes.Columns.Add(colSeleccion);

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Código",
                DataPropertyName = "CdLote",
                Name = "colCdLote",
                Width = 80,
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Nombre de Lote",
                DataPropertyName = "DsNombreLote",
                Name = "colDsNombreLote",
                Width = 200,
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Fecha Alta",
                DataPropertyName = "FeAlta",
                Name = "colFeAlta",
                Width = 130,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" },
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Cantidad Archivos",
                DataPropertyName = "NuCantidadArchivos",
                Name = "colNuCantidadArchivos",
                Width = 120,
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Estado",
                DataPropertyName = "DsEstado",
                Name = "colDsEstado",
                Width = 200,
                ReadOnly = true
            });

            // Eventos para actualizar información inmediatamente
            dgvLotes.CellValueChanged += dgvLotes_CellValueChanged;
            dgvLotes.CurrentCellDirtyStateChanged += dgvLotes_CurrentCellDirtyStateChanged;
        }

        private void dgvLotes_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (dgvLotes.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvLotes.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvLotes_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0) // Columna de selección
            {
                ActualizarInfoSeleccion();
            }
        }

        private void ConfigurarComboEsquinas()
        {
            try
            {
                cboEsquina.Items.Clear();
                cboEsquina.Items.Add("Superior Izquierda");
                cboEsquina.Items.Add("Superior Derecha");
                cboEsquina.Items.Add("Inferior Izquierda");
                cboEsquina.Items.Add("Inferior Derecha");
                cboEsquina.SelectedIndex = 3; // Inferior Derecha por defecto
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar combo esquinas:\n\n{ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarEventos()
        {
            // Filtros y búsqueda
            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            txtFiltroNombre.KeyPress += (s, e) => { if (e.KeyChar == (char)Keys.Enter) btnBuscar_Click(s, e); };

            // Botones de selección
            btnSeleccionarTodo.Click += btnSeleccionarTodo_Click;
            btnDeseleccionarTodo.Click += btnDeseleccionarTodo_Click;
            btnSeleccionar50.Click += btnSeleccionar50_Click;

            // Preview
            btnCargarPDF.Click += btnCargarPDF_Click;
            btnActualizarImagen.Click += btnActualizarImagen_Click;

            // Procesamiento
            btnProcesar.Click += btnProcesar_Click;
            btnCerrar.Click += btnCerrar_Click;

            // Filtro de lotes ya preparados
            chkMostrarPreparados.CheckedChanged += (s, e) => CargarDatos();

            // Al cambiar configuración, guardar automáticamente
            cboEsquina.SelectedIndexChanged += (s, e) => GuardarConfiguracion();
            numDPI.ValueChanged += (s, e) => GuardarConfiguracion();
            numPorcentajeVertical.ValueChanged += (s, e) => GuardarConfiguracion();
            numPorcentajeHorizontal.ValueChanged += (s, e) => GuardarConfiguracion();
            chkEjecutarOCR.CheckedChanged += (s, e) => GuardarConfiguracion();

            // Al cerrar el formulario
            this.FormClosing += FrmPreparacionImagenes_FormClosing;
        }

        private void CargarConfiguracion()
        {
            try
            {
                _configuracion = ConfiguracionRecorteManager.CargarConfiguracion();

                // Aplicar configuración a los controles
                cboEsquina.SelectedIndex = (int)_configuracion.EsquinaRecorte;
                numDPI.Value = _configuracion.DPI;
                numPorcentajeVertical.Value = _configuracion.PorcentajeVertical;
                numPorcentajeHorizontal.Value = _configuracion.PorcentajeHorizontal;
                chkEjecutarOCR.Checked = _configuracion.EjecutarOCR;

                // Cargar PDF temporal si existe
                if (!string.IsNullOrEmpty(_configuracion.RutaPDFTemporal) && File.Exists(_configuracion.RutaPDFTemporal))
                {
                    CargarPDFEnVisor(_configuracion.RutaPDFTemporal);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar configuración: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GuardarConfiguracion()
        {
            try
            {
                _configuracion.EsquinaRecorte = (EsquinaRecorte)cboEsquina.SelectedIndex;
                _configuracion.DPI = (int)numDPI.Value;
                _configuracion.PorcentajeVertical = numPorcentajeVertical.Value;
                _configuracion.PorcentajeHorizontal = numPorcentajeHorizontal.Value;
                _configuracion.EjecutarOCR = chkEjecutarOCR.Checked;

                ConfiguracionRecorteManager.GuardarConfiguracion(_configuracion);
            }
            catch (Exception ex)
            {
                // Error silencioso al guardar configuración
                Console.WriteLine($"Error al guardar configuración: {ex.Message}");
            }
        }

        private void CargarDatos()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                _lotes = _bl.ObtenerLotesParaPreparacion(incluirYaPreparados: chkMostrarPreparados.Checked);

                if (_lotes == null || !_lotes.Any())
                {
                    MessageBox.Show("No se encontraron lotes en estado 'Preparado para imágenes'.\n\n" +
                        "Asegúrese de que existan lotes creados en 'Preparación de Lotes'.",
                        "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _lotes = new List<LoteGridDto>();
                }

                dgvLotes.DataSource = null;
                dgvLotes.DataSource = _lotes;
                dgvLotes.Refresh();

                ActualizarInfoSeleccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar lotes:\n\n{ex.Message}\n\nStackTrace:\n{ex.StackTrace}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                string? filtroNombre = string.IsNullOrWhiteSpace(txtFiltroNombre.Text) ? null : txtFiltroNombre.Text.Trim();
                DateTime? fechaDesde = dtpDesde.Checked ? dtpDesde.Value.Date : null;
                DateTime? fechaHasta = dtpHasta.Checked ? dtpHasta.Value.Date : null;

                _lotes = _bl.ObtenerLotesParaPreparacion(filtroNombre, fechaDesde, fechaHasta, chkMostrarPreparados.Checked);
                dgvLotes.DataSource = null;
                dgvLotes.DataSource = _lotes;

                ActualizarInfoSeleccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar lotes: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnLimpiar_Click(object? sender, EventArgs e)
        {
            txtFiltroNombre.Clear();
            dtpDesde.Value = DateTime.Now.AddMonths(-1);
            dtpHasta.Value = DateTime.Now;
            CargarDatos();
        }

        private void btnSeleccionarTodo_Click(object? sender, EventArgs e)
        {
            foreach (var lote in _lotes)
            {
                lote.Seleccionado = true;
            }
            dgvLotes.Refresh();
            ActualizarInfoSeleccion();
        }

        private void btnDeseleccionarTodo_Click(object? sender, EventArgs e)
        {
            foreach (var lote in _lotes)
            {
                lote.Seleccionado = false;
            }
            dgvLotes.Refresh();
            ActualizarInfoSeleccion();
        }

        private void btnSeleccionar50_Click(object? sender, EventArgs e)
        {
            int seleccionados = 0;
            foreach (var lote in _lotes)
            {
                if (seleccionados < 50)
                {
                    lote.Seleccionado = true;
                    seleccionados++;
                }
                else
                {
                    lote.Seleccionado = false;
                }
            }
            dgvLotes.Refresh();
            ActualizarInfoSeleccion();
        }

        private void ActualizarInfoSeleccion()
        {
            int lotesSeleccionados = _lotes.Count(l => l.Seleccionado);
            int totalArchivos = _lotes.Where(l => l.Seleccionado).Sum(l => l.NuCantidadArchivos);

            lblInfoSeleccion.Text = $"Lotes seleccionados: {lotesSeleccionados} | Total archivos: {totalArchivos}";
        }

        private void btnCargarPDF_Click(object? sender, EventArgs e)
        {
            try
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Archivos PDF (*.pdf)|*.pdf";
                    openFileDialog.Title = "Seleccionar PDF para Preview";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Copiar a carpeta temporal
                        string rutaTemporal = ConfiguracionRecorteManager.CopiarPDFTemporal(openFileDialog.FileName);
                        _configuracion.RutaPDFTemporal = rutaTemporal;
                        GuardarConfiguracion();

                        // Cargar en visor
                        CargarPDFEnVisor(rutaTemporal);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar PDF: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarPDFEnVisor(string rutaPDF)
        {
            try
            {
                // Liberar bitmap previo
                if (pictureBoxPreview.Image != null && pictureBoxPreview.Image != _bitmapPreview)
                {
                    pictureBoxPreview.Image.Dispose();
                }
                _bitmapPreview?.Dispose();
                _bitmapPreview = null;

                // Convertir PDF a bitmap
                _bitmapPreview = ProcesamientoImagenes.ConvertirPDFaBitmap(rutaPDF, (int)numDPI.Value);

                if (_bitmapPreview != null)
                {
                    // Mostrar en PictureBox con el recuadro de recorte ya dibujado
                    MostrarPreviewConRecuadro();
                    pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al renderizar PDF: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarPreviewConRecuadro()
        {
            if (_bitmapPreview == null)
            {
                return;
            }

            // Crear una copia exacta del bitmap original
            var bitmapConRecuadro = (Bitmap)_bitmapPreview.Clone();

            using (var graphics = Graphics.FromImage(bitmapConRecuadro))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Calcular rectángulo de recorte
                var rectangulo = ProcesamientoImagenes.CalcularRectanguloRecorte(
                    bitmapConRecuadro.Width,
                    bitmapConRecuadro.Height,
                    (EsquinaRecorte)cboEsquina.SelectedIndex,
                    numPorcentajeVertical.Value,
                    numPorcentajeHorizontal.Value);

                // Dibujar rectángulo rojo
                using (var pen = new Pen(Color.Red, 5))
                {
                    graphics.DrawRectangle(pen, rectangulo);
                }

                // Dibujar líneas diagonales para mejor visualización
                using (var penDiagonal = new Pen(Color.Red, 2))
                {
                    penDiagonal.DashStyle = DashStyle.Dash;
                    graphics.DrawLine(penDiagonal, 
                        rectangulo.Left, rectangulo.Top, 
                        rectangulo.Right, rectangulo.Bottom);
                    graphics.DrawLine(penDiagonal, 
                        rectangulo.Right, rectangulo.Top, 
                        rectangulo.Left, rectangulo.Bottom);
                }
            }

            // Disponer imagen anterior si existe y no es el bitmap original
            if (pictureBoxPreview.Image != null && pictureBoxPreview.Image != _bitmapPreview)
            {
                pictureBoxPreview.Image.Dispose();
            }

            // Mostrar imagen con recuadro
            pictureBoxPreview.Image = bitmapConRecuadro;
            pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void btnActualizarImagen_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_bitmapPreview == null)
                {
                    MessageBox.Show("Debe cargar un PDF primero", "Advertencia", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MostrarPreviewConRecuadro();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar imagen:\n\n{ex.Message}\n\nStackTrace:\n{ex.StackTrace}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnProcesar_Click(object? sender, EventArgs e)
        {
            try
            {
                var lotesSeleccionados = _lotes.Where(l => l.Seleccionado).ToList();

                if (lotesSeleccionados.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar al menos un lote", "Advertencia", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int totalArchivos = lotesSeleccionados.Sum(l => l.NuCantidadArchivos);

                var confirmacion = MessageBox.Show(
                    $"Se procesarán {lotesSeleccionados.Count} lotes con un total de {totalArchivos} archivos.\n\n" +
                    $"Configuración:\n" +
                    $"  - DPI: {numDPI.Value}\n" +
                    $"  - Recorte: {numPorcentajeHorizontal.Value}% H x {numPorcentajeVertical.Value}% V desde {cboEsquina.Text}\n" +
                    $"  - OCR: {(chkEjecutarOCR.Checked ? "Sí" : "No")}\n\n" +
                    $"¿Desea continuar?",
                    "Confirmar Procesamiento",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes)
                    return;

                // Deshabilitar controles
                btnProcesar.Enabled = false;
                dgvLotes.Enabled = false;
                progressBar.Value = 0;
                progressBar.Maximum = totalArchivos;

                // Guardar configuración actualizada
                GuardarConfiguracion();

                // Procesar en tarea asíncrona
                await Task.Run(() =>
                {
                    _bl.ProcesarLotesSeleccionados(lotesSeleccionados, _configuracion, ReportarProgreso);
                });

                // Finalizado
                MessageBox.Show($"Procesamiento completado exitosamente.\n\n{totalArchivos} archivos procesados.", 
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recargar datos
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante el procesamiento:\n\n{ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnProcesar.Enabled = true;
                dgvLotes.Enabled = true;
                progressBar.Value = 0;
                lblProgresoTexto.Text = "Listo para procesar";
            }
        }

        private void ReportarProgreso(int archivoActual, int totalArchivos, string mensaje)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ReportarProgreso(archivoActual, totalArchivos, mensaje)));
                return;
            }

            progressBar.Value = Math.Min(archivoActual, totalArchivos);
            lblProgresoTexto.Text = $"Procesando archivo {archivoActual} de {totalArchivos}: {mensaje}";
            Application.DoEvents();
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmPreparacionImagenes_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // Liberar recursos
            _bitmapPreview?.Dispose();
            pictureBoxPreview.Image?.Dispose();

            // Guardar configuración
            GuardarConfiguracion();

            // Limpiar archivos temporales antiguos
            ConfiguracionRecorteManager.LimpiarArchivosTemporales();
        }
    }
}
