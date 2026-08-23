using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    public partial class FrmProcesamientoIA : Form
    {
        private List<Lote> _lotesDisponibles = new();
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _procesando = false;

        public FrmProcesamientoIA()
        {
            InitializeComponent();
        }

        private void FrmProcesamientoIA_Load(object sender, EventArgs e)
        {
            try
            {
                // AplicarTemaOscuro(); // Ya no es necesario, estilos aplicados en el Designer
                CargarProyectos();
                ConfigurarDataGridView();
                CargarBatchesEnviados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarTemaOscuro()
        {
            // Colores del tema oscuro
            Color fondoOscuro = Color.FromArgb(45, 45, 48);
            Color fondoControles = Color.FromArgb(30, 30, 30);
            Color textoClaro = Color.White;
            Color bordeLeve = Color.FromArgb(63, 63, 70);

            // Aplicar al formulario
            this.BackColor = fondoOscuro;
            this.ForeColor = textoClaro;

            // Aplicar a los GroupBox
            foreach (Control control in this.Controls)
            {
                if (control is GroupBox groupBox)
                {
                    groupBox.BackColor = fondoOscuro;
                    groupBox.ForeColor = textoClaro;
                    AplicarTemaOscuroAControles(groupBox.Controls, fondoOscuro, fondoControles, textoClaro, bordeLeve);
                }
            }

            // DataGridView
            dgvLotes.BackgroundColor = fondoControles;
            dgvLotes.ForeColor = textoClaro;
            dgvLotes.GridColor = bordeLeve;
            dgvLotes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvLotes.ColumnHeadersDefaultCellStyle.ForeColor = textoClaro;
            dgvLotes.DefaultCellStyle.BackColor = fondoControles;
            dgvLotes.DefaultCellStyle.ForeColor = textoClaro;
            dgvLotes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvLotes.DefaultCellStyle.SelectionForeColor = textoClaro;
            dgvLotes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvLotes.EnableHeadersVisualStyles = false;
            dgvLotes.BorderStyle = BorderStyle.None;

            // DataGridView Tracking
            dgvTracking.BackgroundColor = fondoControles;
            dgvTracking.ForeColor = textoClaro;
            dgvTracking.GridColor = bordeLeve;
            dgvTracking.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvTracking.ColumnHeadersDefaultCellStyle.ForeColor = textoClaro;
            dgvTracking.DefaultCellStyle.BackColor = fondoControles;
            dgvTracking.DefaultCellStyle.ForeColor = textoClaro;
            dgvTracking.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvTracking.DefaultCellStyle.SelectionForeColor = textoClaro;
            dgvTracking.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvTracking.EnableHeadersVisualStyles = false;
            dgvTracking.BorderStyle = BorderStyle.None;

            // Panel de selección
            panelSeleccion.BackColor = fondoOscuro;

            // Panel de botones de tracking
            panelTrackingButtons.BackColor = fondoOscuro;
        }

        private void AplicarTemaOscuroAControles(Control.ControlCollection controls, Color fondoOscuro, Color fondoControles, Color textoClaro, Color bordeLeve)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl is Label)
                {
                    ctrl.BackColor = fondoOscuro;
                    ctrl.ForeColor = textoClaro;
                }
                else if (ctrl is TextBox textBox)
                {
                    textBox.BackColor = fondoControles;
                    textBox.ForeColor = textoClaro;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (ctrl is ComboBox comboBox)
                {
                    comboBox.BackColor = fondoControles;
                    comboBox.ForeColor = textoClaro;
                    comboBox.FlatStyle = FlatStyle.Flat;
                }
                else if (ctrl is Button button)
                {
                    button.BackColor = Color.FromArgb(0, 122, 204);
                    button.ForeColor = textoClaro;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                }
                else if (ctrl is ProgressBar progressBar)
                {
                    progressBar.BackColor = fondoControles;
                    progressBar.ForeColor = Color.FromArgb(0, 122, 204);
                }

                // Recursivo para controles contenedores
                if (ctrl.HasChildren)
                {
                    AplicarTemaOscuroAControles(ctrl.Controls, fondoOscuro, fondoControles, textoClaro, bordeLeve);
                }
            }
        }

        private void CargarProyectos()
        {
            try
            {
                var proyectoDAL = new ProyectoDAL();
                var proyectos = proyectoDAL.ObtenerTodos();

                cboProyecto.DataSource = proyectos;
                cboProyecto.DisplayMember = "DsProyecto";
                cboProyecto.ValueMember = "CdProyecto";

                if (proyectos.Count > 0)
                {
                    cboProyecto.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar proyectos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvLotes.AutoGenerateColumns = false;
            dgvLotes.Columns.Clear();

            // Columna de selección
            dgvLotes.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "Seleccionar",
                HeaderText = "Sel.",
                Width = 50,
                ReadOnly = false
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CdLote",
                HeaderText = "ID Lote",
                DataPropertyName = "CdLote",
                Width = 80,
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DsNombreLote",
                HeaderText = "Nombre Lote",
                DataPropertyName = "DsNombreLote",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NuCantidadArchivos",
                HeaderText = "Cant. Archivos",
                DataPropertyName = "NuCantidadArchivos",
                Width = 120,
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DsEstado",
                HeaderText = "Estado",
                DataPropertyName = "DsEstado",
                Width = 150,
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FeAltaLote",
                HeaderText = "Fecha Alta",
                DataPropertyName = "FeAltaLote",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" },
                ReadOnly = true
            });
        }

        private void btnCargarLotes_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboProyecto.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un proyecto", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int cdProyecto = Convert.ToInt32(cboProyecto.SelectedValue);
                var loteDAL = new LoteDAL();

                // Cargar lotes con estado 2 (Listo para IA)
                _lotesDisponibles = loteDAL.ObtenerLotesPorEstado(2, cdProyecto);

                dgvLotes.DataSource = null;
                dgvLotes.DataSource = _lotesDisponibles;

                lblEstado.Text = $"Estado: {_lotesDisponibles.Count} lote(s) disponible(s) para procesamiento";
                ActualizarTotalizadorLotes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar lotes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVerPrompt_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboProyecto.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un proyecto", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int cdProyecto = Convert.ToInt32(cboProyecto.SelectedValue);

                using var frmPrompt = new FrmEditorPrompt(cdProyecto);
                frmPrompt.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir editor de prompt: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnProcesar_Click(object sender, EventArgs e)
        {
            if (_procesando)
            {
                // Cancelar procesamiento
                _cancellationTokenSource?.Cancel();
                return;
            }

            try
            {
                if (cboProyecto.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un proyecto", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener lotes seleccionados
                var lotesSeleccionados = new List<Lote>();
                foreach (DataGridViewRow row in dgvLotes.Rows)
                {
                    if (row.Cells["Seleccionar"].Value != null &&
                        (bool)row.Cells["Seleccionar"].Value == true)
                    {
                        int cdLote = (int)row.Cells["CdLote"].Value;
                        var lote = _lotesDisponibles.FirstOrDefault(l => l.CdLote == cdLote);
                        if (lote != null)
                        {
                            lotesSeleccionados.Add(lote);
                        }
                    }
                }

                if (lotesSeleccionados.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar al menos un lote para procesar", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    $"¿Está seguro que desea procesar {lotesSeleccionados.Count} lote(s) con OpenAI?\n\n" +
                    "Este proceso puede tardar varios minutos y consumirá tokens de la API.",
                    "Confirmar Procesamiento",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                int cdProyecto = Convert.ToInt32(cboProyecto.SelectedValue);
                int cdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 1;

                // Iniciar procesamiento
                _procesando = true;
                _cancellationTokenSource = new CancellationTokenSource();
                btnProcesar.Text = "Cancelar Procesamiento";
                btnProcesar.BackColor = Color.FromArgb(150, 40, 40);
                btnCargarLotes.Enabled = false;
                btnVerPrompt.Enabled = false;
                cboProyecto.Enabled = false;
                progressBar.Value = 0;
                progressBar.Maximum = lotesSeleccionados.Count;

                var progreso = new Progress<string>(mensaje =>
                {
                    lblEstado.Text = $"Estado: {mensaje}";
                });

                int exitosos = 0;
                int fallidos = 0;

                foreach (var lote in lotesSeleccionados)
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        lblEstado.Text = "Estado: Procesamiento cancelado por el usuario";
                        break;
                    }

                    lblEstado.Text = $"Estado: Procesando lote {lote.CdLote} ({lote.DsNombreLote})...";
                    Application.DoEvents();

                    bool resultado = await OpenAIBL.ProcesarLoteAsync(
                        lote.CdLote,
                        cdProyecto,
                        cdUsuario,
                        progreso,
                        _cancellationTokenSource.Token);

                    if (resultado)
                        exitosos++;
                    else
                        fallidos++;

                    progressBar.Value++;
                }

                // Recargar lotes y tracking
                btnCargarLotes_Click(sender, e);
                CargarBatchesEnviados();

                string mensajeFinal;
                if (fallidos > 0)
                {
                    mensajeFinal = $"Procesamiento finalizado.\n\n" +
                                  $"Lotes enviados a OpenAI: {exitosos}\n" +
                                  $"Lotes con error (no enviados): {fallidos}\n\n" +
                                  $"Los lotes enviados continuarán procesándose en OpenAI.\n" +
                                  $"Use la grilla 'Batches Enviados' y el botón 'Verificar Estado' para monitorear el progreso.";
                }
                else
                {
                    mensajeFinal = $"Procesamiento finalizado.\n\n" +
                                  $"Todos los lotes ({exitosos}) fueron enviados exitosamente a OpenAI.\n\n" +
                                  $"Use la grilla 'Batches Enviados' y el botón 'Verificar Estado' para monitorear el progreso.";
                }

                MessageBox.Show(mensajeFinal, "Resultado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblEstado.Text = $"Estado: {exitosos} lote(s) enviado(s) a OpenAI, {fallidos} con error";
            }
            catch (OperationCanceledException)
            {
                lblEstado.Text = "Estado: Procesamiento cancelado";
                MessageBox.Show("El procesamiento fue cancelado", "Cancelado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante el procesamiento: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = $"Estado: Error - {ex.Message}";
            }
            finally
            {
                _procesando = false;
                btnProcesar.Text = "Procesar Lotes";
                btnProcesar.BackColor = Color.FromArgb(0, 122, 204);
                btnCargarLotes.Enabled = true;
                btnVerPrompt.Enabled = true;
                cboProyecto.Enabled = true;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (_procesando)
            {
                MessageBox.Show("No se puede cerrar mientras hay un procesamiento en curso.\n" +
                              "Por favor, cancele el procesamiento primero.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Close();
        }

        private void btnSeleccionarTodo_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvLotes.Rows)
            {
                row.Cells["Seleccionar"].Value = true;
            }
            ActualizarTotalizadorLotes();
        }

        private void btnDeseleccionarTodo_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvLotes.Rows)
            {
                row.Cells["Seleccionar"].Value = false;
            }
            ActualizarTotalizadorLotes();
        }

        private void btnSeleccionar50_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (DataGridViewRow row in dgvLotes.Rows)
            {
                if (count < 50)
                {
                    row.Cells["Seleccionar"].Value = true;
                    count++;
                }
                else
                {
                    row.Cells["Seleccionar"].Value = false;
                }
            }
            ActualizarTotalizadorLotes();
        }

        private void dgvLotes_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarTotalizadorLotes();
        }

        private void ActualizarTotalizadorLotes()
        {
            int total = dgvLotes.Rows.Count;
            int seleccionados = 0;

            foreach (DataGridViewRow row in dgvLotes.Rows)
            {
                if (row.Cells["Seleccionar"].Value != null && 
                    Convert.ToBoolean(row.Cells["Seleccionar"].Value))
                {
                    seleccionados++;
                }
            }

            lblTotalizadorLotes.Text = $"Total: {total} | Seleccionados: {seleccionados}";
        }

        #region Tracking de Batches

        private void btnActualizarTracking_Click(object sender, EventArgs e)
        {
            try
            {
                CargarBatchesEnviados();
                lblEstado.Text = "Lista de batches actualizada";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la lista de batches: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnVerificarEstado_Click(object sender, EventArgs e)
        {
            try
            {
                lblEstado.Text = "Verificando estado de todos los batches pendientes...";
                btnVerificarEstado.Enabled = false;
                progressBar.Maximum = 100;
                progressBar.Value = 0;

                // Obtener todos los batches pendientes de la grilla actual
                var batchesPendientes = new List<(string batchId, int rowIndex)>();

                for (int i = 0; i < dgvTracking.Rows.Count; i++)
                {
                    var row = dgvTracking.Rows[i];
                    string estado = row.Cells["dsEstado"].Value?.ToString() ?? "";
                    string batchId = row.Cells["dsBatchId"].Value?.ToString() ?? "";

                    if (!string.IsNullOrEmpty(batchId) && 
                        (estado == "created" || estado == "validating" || estado == "in_progress" || estado == "finalizing"))
                    {
                        batchesPendientes.Add((batchId, i));
                    }
                }

                if (batchesPendientes.Count == 0)
                {
                    MessageBox.Show("No hay batches pendientes para verificar.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblEstado.Text = "No hay batches pendientes";
                    return;
                }

                int total = batchesPendientes.Count;
                int procesados = 0;
                int completados = 0;

                foreach (var (batchId, rowIndex) in batchesPendientes)
                {
                    try
                    {
                        lblEstado.Text = $"Verificando batch {procesados + 1}/{total}: {batchId}";

                        var (estado, outputFileId, errorFileId, totalRequests, completedRequests, failedRequests) =
                            await OpenAIBL.VerificarEstadoBatchAsync(batchId);

                        // Actualizar la grilla
                        dgvTracking.Rows[rowIndex].Cells["dsEstado"].Value = estado;
                        dgvTracking.Rows[rowIndex].Cells["dsOutputFileId"].Value = outputFileId;
                        dgvTracking.Rows[rowIndex].Cells["dsErrorFileId"].Value = errorFileId;
                        dgvTracking.Rows[rowIndex].Cells["nuTotalRequests"].Value = totalRequests;
                        dgvTracking.Rows[rowIndex].Cells["nuCompletedRequests"].Value = completedRequests;
                        dgvTracking.Rows[rowIndex].Cells["nuFailedRequests"].Value = failedRequests;

                        // Actualizar en BD
                        ActualizarEstadoBatch(batchId, estado, outputFileId, errorFileId, totalRequests, completedRequests, failedRequests);

                        if (estado == "completed")
                        {
                            completados++;
                        }

                        procesados++;
                        progressBar.Value = (int)((procesados / (float)total) * 100);
                    }
                    catch (Exception ex)
                    {
                        // Registrar error pero continuar con siguiente batch
                        var logDAL = new Datos.LogDAL();
                        logDAL.Insertar(new Entidades.LogRegistro
                        {
                            DsNivel = Entidades.LogRegistro.Niveles.ERROR,
                            DsModulo = "FrmProcesamientoIA.btnVerificarEstado_Click",
                            DsMensaje = $"Error al verificar batch {batchId}: {ex.Message}"
                        });
                    }
                }

                progressBar.Value = 100;
                lblEstado.Text = $"Verificación completada: {procesados} batches verificados, {completados} completados";

                MessageBox.Show($"Verificación completada:\n\n" +
                    $"- Batches verificados: {procesados}\n" +
                    $"- Batches completados: {completados}\n\n" +
                    (completados > 0 ? "Puede procesar los resultados ahora." : ""),
                    "Verificación Completa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recargar grilla para refrescar
                CargarBatchesEnviados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar los batches: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Error al verificar batches";
            }
            finally
            {
                btnVerificarEstado.Enabled = true;
                progressBar.Value = 0;
            }
        }

        private async void btnProcesarResultados_Click(object sender, EventArgs e)
        {
            try
            {
                lblEstado.Text = "Buscando batches completados para procesar...";
                progressBar.Maximum = 100;
                progressBar.Value = 0;

                // Obtener todos los batches completados que NO han sido procesados
                var batchesParaProcesar = new List<(string batchId, int cdLote, int rowIndex)>();

                for (int i = 0; i < dgvTracking.Rows.Count; i++)
                {
                    var row = dgvTracking.Rows[i];
                    string estado = row.Cells["dsEstado"].Value?.ToString() ?? "";
                    string batchId = row.Cells["dsBatchId"].Value?.ToString() ?? "";
                    string procesado = row.Cells["snResultadoProcesado"].Value?.ToString() ?? "NO";

                    if (!string.IsNullOrEmpty(batchId) && 
                        estado == "completed" && 
                        procesado == "NO")
                    {
                        int cdLote = Convert.ToInt32(row.Cells["cdLote"].Value);
                        batchesParaProcesar.Add((batchId, cdLote, i));
                    }
                }

                if (batchesParaProcesar.Count == 0)
                {
                    MessageBox.Show("No hay batches completados pendientes de procesar.\n\n" +
                        "Asegúrese de que los batches estén en estado 'completed' y no hayan sido procesados previamente.\n\n" +
                        "Use el botón 'Verificar Estado' si algunos batches aún están en progreso.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblEstado.Text = "No hay batches para procesar";
                    return;
                }

                var confirmResult = MessageBox.Show(
                    $"Se encontraron {batchesParaProcesar.Count} batch(es) completado(s) para procesar.\n\n" +
                    $"¿Desea procesar todos los resultados?\n\n" +
                    $"Esto descargará y guardará los resultados de OpenAI en la base de datos.",
                    "Confirmar Procesamiento Masivo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes)
                    return;

                btnProcesarResultados.Enabled = false;

                int total = batchesParaProcesar.Count;
                int procesados = 0;
                int exitosos = 0;
                int fallidos = 0;

                foreach (var (batchId, cdLote, rowIndex) in batchesParaProcesar)
                {
                    try
                    {
                        lblEstado.Text = $"Procesando batch {procesados + 1}/{total}: {batchId}";

                        await OpenAIBL.ProcesarResultadosBatchAsync(batchId, cdLote, SesionActual.UsuarioActual?.CdUsuario ?? 1);

                        // Actualizar la grilla para mostrar como procesado
                        dgvTracking.Rows[rowIndex].Cells["snResultadoProcesado"].Value = "SI";

                        exitosos++;
                        procesados++;
                        progressBar.Value = (int)((procesados / (float)total) * 100);
                    }
                    catch (Exception ex)
                    {
                        fallidos++;
                        procesados++;

                        // Registrar error pero continuar con siguiente batch
                        var logDAL = new Datos.LogDAL();
                        logDAL.Insertar(new Entidades.LogRegistro
                        {
                            DsNivel = Entidades.LogRegistro.Niveles.ERROR,
                            DsModulo = "FrmProcesamientoIA.btnProcesarResultados_Click",
                            DsMensaje = $"Error al procesar batch {batchId} (Lote {cdLote}): {ex.Message}"
                        });

                        progressBar.Value = (int)((procesados / (float)total) * 100);
                    }
                }

                progressBar.Value = 100;
                lblEstado.Text = $"Procesamiento completado: {exitosos} exitosos, {fallidos} con errores";

                string mensaje = $"Procesamiento de resultados completado:\n\n" +
                    $"- Total procesados: {procesados}\n" +
                    $"- Exitosos: {exitosos}\n" +
                    $"- Con errores: {fallidos}";

                if (fallidos > 0)
                {
                    mensaje += "\n\nRevisar el log de errores para más detalles sobre los fallos.";
                }

                MessageBox.Show(mensaje, "Procesamiento Completo",
                    MessageBoxButtons.OK,
                    fallidos > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                // Recargar ambas grillas
                CargarBatchesEnviados();
                btnCargarLotes_Click(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar los resultados: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblEstado.Text = "Error al procesar resultados";
            }
            finally
            {
                btnProcesarResultados.Enabled = true;
                progressBar.Value = 0;
            }
        }

        private void CargarBatchesEnviados()
        {
            try
            {
                using var connection = new SqlConnection(Datos.Configuracion.CadenaConexion);
                connection.Open();

                string filtroResultadoProcesado = chkMostrarProcesados.Checked 
                    ? "" 
                    : "AND bt.snResultadoProcesado = 'NO'";

                string query = $@"
                    SELECT 
                        bt.cdBatchTracking,
                        bt.cdLote,
                        l.dsNombreLote,
                        bt.dsBatchId,
                        bt.dsFileId,
                        bt.dsEstado,
                        bt.nuTotalRequests,
                        bt.nuCompletedRequests,
                        bt.nuFailedRequests,
                        bt.dsOutputFileId,
                        bt.dsErrorFileId,
                        bt.snResultadoProcesado,
                        bt.feAlta,
                        bt.feUltimaConsulta
                    FROM TD_BATCH_TRACKING bt
                    INNER JOIN TD_LOTE l ON l.cdLote = bt.cdLote
                    WHERE (bt.dsEstado IN ('created', 'validating', 'in_progress', 'finalizing', 'completed', 'failed', 'expired', 'cancelling', 'cancelled')
                           OR bt.dsEstado IS NULL)
                      {filtroResultadoProcesado}
                    ORDER BY bt.feAlta DESC";

                using var cmd = new SqlCommand(query, connection);
                using var adapter = new SqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);

                dgvTracking.DataSource = table;

                // Configurar columnas
                if (dgvTracking.Columns.Count > 0)
                {
                    dgvTracking.Columns["cdBatchTracking"].Visible = false;
                    dgvTracking.Columns["cdLote"].HeaderText = "Cód. Lote";
                    dgvTracking.Columns["cdLote"].Width = 80;
                    dgvTracking.Columns["dsNombreLote"].HeaderText = "Nombre Lote";
                    dgvTracking.Columns["dsNombreLote"].Width = 150;
                    dgvTracking.Columns["dsBatchId"].HeaderText = "Batch ID";
                    dgvTracking.Columns["dsFileId"].HeaderText = "File ID";
                    dgvTracking.Columns["dsEstado"].HeaderText = "Estado";
                    dgvTracking.Columns["nuTotalRequests"].HeaderText = "Total";
                    dgvTracking.Columns["nuTotalRequests"].Width = 60;
                    dgvTracking.Columns["nuCompletedRequests"].HeaderText = "Completados";
                    dgvTracking.Columns["nuCompletedRequests"].Width = 100;
                    dgvTracking.Columns["nuFailedRequests"].HeaderText = "Fallidos";
                    dgvTracking.Columns["nuFailedRequests"].Width = 70;
                    dgvTracking.Columns["dsOutputFileId"].HeaderText = "Output File";
                    dgvTracking.Columns["dsErrorFileId"].HeaderText = "Error File";
                    dgvTracking.Columns["snResultadoProcesado"].HeaderText = "Procesado";
                    dgvTracking.Columns["snResultadoProcesado"].Width = 80;
                    dgvTracking.Columns["feAlta"].HeaderText = "Fecha Envío";
                    dgvTracking.Columns["feAlta"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    dgvTracking.Columns["feUltimaConsulta"].HeaderText = "Última Consulta";
                    dgvTracking.Columns["feUltimaConsulta"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                    dgvTracking.Columns["dsFileId"].Width = 150;
                    dgvTracking.Columns["dsBatchId"].Width = 150;
                    dgvTracking.Columns["dsOutputFileId"].Width = 150;
                    dgvTracking.Columns["dsErrorFileId"].Width = 150;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar batches enviados: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkMostrarProcesados_CheckedChanged(object sender, EventArgs e)
        {
            CargarBatchesEnviados();
        }

        private void ActualizarEstadoBatch(string batchId, string estado, string? outputFileId = null,
            string? errorFileId = null, int? totalRequests = null, int? completedRequests = null, int? failedRequests = null)
        {
            try
            {
                using var connection = new SqlConnection(Datos.Configuracion.CadenaConexion);
                connection.Open();

                string query = @"
                    UPDATE TD_BATCH_TRACKING 
                    SET dsEstado = @estado,
                        feUltimaConsulta = GETDATE(),
                        dsOutputFileId = COALESCE(@outputFileId, dsOutputFileId),
                        dsErrorFileId = COALESCE(@errorFileId, dsErrorFileId),
                        nuTotalRequests = COALESCE(@totalRequests, nuTotalRequests),
                        nuCompletedRequests = COALESCE(@completedRequests, nuCompletedRequests),
                        nuFailedRequests = COALESCE(@failedRequests, nuFailedRequests),
                        feCompleted = CASE WHEN @estado = 'completed' AND feCompleted IS NULL THEN GETDATE() ELSE feCompleted END,
                        feFailed = CASE WHEN @estado = 'failed' AND feFailed IS NULL THEN GETDATE() ELSE feFailed END,
                        feInProgress = CASE WHEN @estado = 'in_progress' AND feInProgress IS NULL THEN GETDATE() ELSE feInProgress END
                    WHERE dsBatchId = @batchId";

                using var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@estado", estado);
                cmd.Parameters.AddWithValue("@batchId", batchId);
                cmd.Parameters.AddWithValue("@outputFileId", (object?)outputFileId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@errorFileId", (object?)errorFileId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@totalRequests", (object?)totalRequests ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@completedRequests", (object?)completedRequests ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@failedRequests", (object?)failedRequests ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar estado del batch en BD: {ex.Message}", ex);
            }
        }

        #endregion

        private void groupBoxAcciones_Enter(object sender, EventArgs e)
        {

        }
    }
}
