using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    /// <summary>
    /// Pantalla de Envío de lotes finalizados (cdEstado=10, "Pendiente de Enviar"):
    /// permite mover los PDFs finales y los registros de índice CSV correspondientes
    /// a la carpeta de envío final, y marcar los lotes (y sus páginas) como "Enviado"
    /// (cdEstado=11 en TD_LOTE / cdEstado=8 en TD_ARCHIVOS_PAGINAS).
    /// </summary>
    public partial class FrmEnvioLote : Form
    {
        private const int CD_ESTADO_PENDIENTE_ENVIAR = 10;
        private const string CLAVE_PARAMETRO_RUTA_FINAL_ENVIO = "RUTA_FINAL_ENVIO";

        private List<LoteEnvioGridDto> _lotesActuales = new();

        public FrmEnvioLote()
        {
            InitializeComponent();
        }

        private void FrmEnvioLote_Load(object sender, EventArgs e)
        {
            try
            {
                dtpFechaDesde.Value = DateTime.Now.AddDays(-30);
                dtpFechaHasta.Value = DateTime.Now;
                chkFiltrarFecha.Checked = false;
                dtpFechaDesde.Enabled = false;
                dtpFechaHasta.Enabled = false;

                progressBar.Visible = false;
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Value = 0;

                ConfigurarDataGridView();
                CargarRutaSalida();
                CargarLotes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvLotes.AutoGenerateColumns = false;
            dgvLotes.Columns.Clear();

            dgvLotes.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "snSeleccionado",
                DataPropertyName = "SnSeleccionado",
                HeaderText = string.Empty,
                Width = 40
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "cdLote",
                DataPropertyName = "CdLote",
                HeaderText = "ID Lote",
                Width = 70,
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsNombreLote",
                DataPropertyName = "DsNombreLote",
                HeaderText = "Lote",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsUsuarioAsignado",
                DataPropertyName = "DsUsuarioAsignado",
                HeaderText = "Usuario Control",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadPlanos",
                DataPropertyName = "NuCantidadPlanos",
                HeaderText = "Cant. Planos",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadControlado",
                DataPropertyName = "NuCantidadControlado",
                HeaderText = "Controlado",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadPendiente",
                DataPropertyName = "NuCantidadPendiente",
                HeaderText = "Pendiente",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadPaginaIlegible",
                DataPropertyName = "NuCantidadPaginaIlegible",
                HeaderText = "Pág. Ilegible",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadDatosIlegibles",
                DataPropertyName = "NuCantidadDatosIlegibles",
                HeaderText = "Datos Ilegibles",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadCorregido",
                DataPropertyName = "NuCantidadCorregido",
                HeaderText = "Corregido",
                ReadOnly = true
            });
        }

        private void CargarRutaSalida()
        {
            var parametrosDAL = new ParametrosDAL();
            txtRutaSalida.Text = parametrosDAL.ObtenerValor(CLAVE_PARAMETRO_RUTA_FINAL_ENVIO) ?? string.Empty;
        }

        private void CargarLotes()
        {
            try
            {
                var loteDAL = new LoteDAL();

                string? dsNombreLote = string.IsNullOrWhiteSpace(txtNombreLote.Text) ? null : txtNombreLote.Text.Trim();
                DateTime? feControlDesde = chkFiltrarFecha.Checked ? dtpFechaDesde.Value : null;
                DateTime? feControlHasta = chkFiltrarFecha.Checked ? dtpFechaHasta.Value : null;

                _lotesActuales = loteDAL.ObtenerLotesPendientesEnviarConEstadisticas(
                    dsNombreLote, feControlDesde, feControlHasta, CD_ESTADO_PENDIENTE_ENVIAR);

                dgvLotes.DataSource = null;
                dgvLotes.DataSource = _lotesActuales;

                ActualizarTotalizador();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar lotes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarTotalizador()
        {
            lblTotalizadorLotes.Text = $"Total: {_lotesActuales.Count}";
        }

        private void chkFiltrarFecha_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaDesde.Enabled = chkFiltrarFecha.Checked;
            dtpFechaHasta.Enabled = chkFiltrarFecha.Checked;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLotes();
        }

        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtNombreLote.Text = string.Empty;
            chkFiltrarFecha.Checked = false;
            dtpFechaDesde.Value = DateTime.Now.AddDays(-30);
            dtpFechaHasta.Value = DateTime.Now;
            CargarLotes();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSeleccionarTodo_Click(object sender, EventArgs e)
        {
            foreach (var lote in _lotesActuales)
                lote.SnSeleccionado = true;

            dgvLotes.Refresh();
        }

        private void btnDeseleccionarTodo_Click(object sender, EventArgs e)
        {
            foreach (var lote in _lotesActuales)
                lote.SnSeleccionado = false;

            dgvLotes.Refresh();
        }

        private void btnSeleccionarSalida_Click(object sender, EventArgs e)
        {
            using var dialogo = new FolderBrowserDialog
            {
                Description = "Seleccione la ruta final de salida de los archivos a enviar",
                UseDescriptionForTitle = true
            };

            if (!string.IsNullOrWhiteSpace(txtRutaSalida.Text) && Directory.Exists(txtRutaSalida.Text))
                dialogo.SelectedPath = txtRutaSalida.Text;

            if (dialogo.ShowDialog(this) != DialogResult.OK)
                return;

            txtRutaSalida.Text = dialogo.SelectedPath;

            int cdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0;
            var parametrosDAL = new ParametrosDAL();
            parametrosDAL.ActualizarValor(CLAVE_PARAMETRO_RUTA_FINAL_ENVIO, dialogo.SelectedPath, cdUsuario);
        }

        private async void btnMarcarEnviado_Click(object sender, EventArgs e)
        {
            var seleccionados = _lotesActuales.Where(l => l.SnSeleccionado).ToList();

            if (seleccionados.Count == 0)
            {
                MessageBox.Show("Seleccione al menos un lote para enviar.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRutaSalida.Text) || !Directory.Exists(txtRutaSalida.Text))
            {
                MessageBox.Show("Debe seleccionar una Ruta de Salida válida.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombreCarpetaSalida.Text))
            {
                MessageBox.Show("Debe ingresar el Nombre de la Carpeta de Salida.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string nombreCarpetaSalida = txtNombreCarpetaSalida.Text.Trim();
            string carpetaEnvioBase = Path.Combine(txtRutaSalida.Text, nombreCarpetaSalida);
            string? observacionesEnvio = string.IsNullOrWhiteSpace(txtObservacionesEnvio.Text)
                ? null : txtObservacionesEnvio.Text.Trim();

            var confirmacion = MessageBox.Show(
                $"Se procesarán {seleccionados.Count} lote(s): se moverán los archivos PDF y los " +
                $"registros de índice a la carpeta '{carpetaEnvioBase}', y los lotes quedarán marcados como Enviados.\n\n" +
                "¿Desea continuar?",
                "Marcar Enviado", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            btnMarcarEnviado.Enabled = false;
            btnBuscar.Enabled = false;
            progressBar.Visible = true;
            progressBar.Value = 0;
            progressBar.Maximum = seleccionados.Count;

            var loteDAL = new LoteDAL();
            var resultadoIADAL = new ResultadoIADAL();
            var loteEnvioBL = new LoteEnvioBL();
            int cdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0;

            var lotesConError = new List<string>();
            var lotesConAdvertencia = new List<string>();

            try
            {
                if (!Directory.Exists(carpetaEnvioBase))
                    Directory.CreateDirectory(carpetaEnvioBase);

                foreach (var lote in seleccionados)
                {
                    try
                    {
                        var filas = await Task.Run(() => ObtenerFilasDelLote(loteDAL, resultadoIADAL, lote.CdLote));

                        if (filas.Count == 0)
                        {
                            lotesConError.Add($"Lote {lote.CdLote}: no tiene registros para procesar.");
                            continue;
                        }

                        var discrepancias = await Task.Run(() => loteEnvioBL.EnviarLote(
                            lote.CdLote, filas, cdUsuario, carpetaEnvioBase, nombreCarpetaSalida, observacionesEnvio));

                        if (discrepancias.Count > 0)
                        {
                            lotesConAdvertencia.Add($"Lote {lote.CdLote}: " + string.Join(" ", discrepancias));
                        }
                    }
                    catch (Exception ex)
                    {
                        loteEnvioBL.RegistrarError("FrmEnvioLote.btnMarcarEnviado_Click", ex, cdUsuario);
                        lotesConError.Add($"Lote {lote.CdLote}: {ex.Message}");
                    }
                    finally
                    {
                        progressBar.Value = Math.Min(progressBar.Value + 1, progressBar.Maximum);
                    }
                }

                if (lotesConError.Count > 0 || lotesConAdvertencia.Count > 0)
                {
                    var mensajes = new List<string>();
                    if (lotesConError.Count > 0)
                        mensajes.AddRange(lotesConError);
                    if (lotesConAdvertencia.Count > 0)
                        mensajes.AddRange(lotesConAdvertencia);

                    MessageBox.Show(
                        "Se procesaron los lotes con las siguientes advertencias/errores:\n\n" +
                        string.Join("\n", mensajes),
                        "Envío con observaciones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Los lotes seleccionados fueron enviados correctamente.",
                        "Marcar Enviado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                txtNombreCarpetaSalida.Text = string.Empty;
                txtObservacionesEnvio.Text = string.Empty;
                CargarLotes();
            }
            finally
            {
                progressBar.Visible = false;
                btnMarcarEnviado.Enabled = true;
                btnBuscar.Enabled = true;
            }
        }

        private static List<LoteEnvioBL.FilaEnvio> ObtenerFilasDelLote(
            LoteDAL loteDAL, ResultadoIADAL resultadoIADAL, int cdLote)
        {
            var resultados = resultadoIADAL.ObtenerPorLote(cdLote, mostrarTodos: true);
            var archivosPorPagina = loteDAL.ObtenerArchivosPaginasPorLote(cdLote)
                .ToDictionary(a => a.CdArchivoPagina, a => a);

            return resultados
                .Where(r => archivosPorPagina.ContainsKey(r.CdArchivoPagina))
                .Select(r => new LoteEnvioBL.FilaEnvio
                {
                    Archivo = archivosPorPagina[r.CdArchivoPagina],
                    Resultado = r
                }).ToList();
        }
    }
}
