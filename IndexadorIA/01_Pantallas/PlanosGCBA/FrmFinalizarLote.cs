using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    /// <summary>
    /// Pantalla de Finalización de lotes pendientes de finalizar (cdEstado=7):
    /// permite renombrar/mover los PDF de página, generar los CSV de metadatos
    /// y marcar el lote como Finalizado (cdEstado=3).
    /// </summary>
    public partial class FrmFinalizarLote : Form
    {
        private const int CD_ESTADO_PENDIENTE_FINALIZAR = 7;
        private List<LoteFinalizacionGridDto> _lotesActuales = new();

        public FrmFinalizarLote()
        {
            InitializeComponent();
        }

        private void FrmFinalizarLote_Load(object sender, EventArgs e)
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

        private void CargarLotes()
        {
            try
            {
                var loteDAL = new LoteDAL();

                string? dsNombreLote = string.IsNullOrWhiteSpace(txtNombreLote.Text) ? null : txtNombreLote.Text.Trim();
                DateTime? feControlDesde = chkFiltrarFecha.Checked ? dtpFechaDesde.Value : null;
                DateTime? feControlHasta = chkFiltrarFecha.Checked ? dtpFechaHasta.Value : null;

                _lotesActuales = loteDAL.ObtenerLotesPendientesFinalizarConEstadisticas(
                    dsNombreLote, feControlDesde, feControlHasta, CD_ESTADO_PENDIENTE_FINALIZAR);

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

        private async void btnFinalizarLote_Click(object sender, EventArgs e)
        {
            var seleccionados = _lotesActuales.Where(l => l.SnSeleccionado).ToList();

            if (seleccionados.Count == 0)
            {
                MessageBox.Show("Seleccione al menos un lote para finalizar.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"Se procesarán {seleccionados.Count} lote(s): se renombrarán/moverán los archivos PDF, " +
                "se generarán los CSV de metadatos y los lotes quedarán marcados como Finalizados.\n\n" +
                "¿Desea continuar?",
                "Finalizar Lote", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            btnFinalizarLote.Enabled = false;
            btnBuscar.Enabled = false;
            progressBar.Visible = true;
            progressBar.Value = 0;
            progressBar.Maximum = seleccionados.Count;

            var loteDAL = new LoteDAL();
            var resultadoIADAL = new ResultadoIADAL();
            var loteFinalizacionBL = new LoteFinalizacionBL();
            int cdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0;

            var lotesConError = new List<string>();

            try
            {
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

                        // Control 1: campos obligatorios
                        var faltantes = loteFinalizacionBL.ValidarCamposObligatorios(filas);
                        if (faltantes.Count > 0)
                        {
                            lotesConError.Add($"Lote {lote.CdLote}: {faltantes.Count} registro(s) sin completar " +
                                "campos obligatorios (Categoría, Tipo de Plano, Sección, Manzana, Parcela, Dirección).");
                            continue;
                        }

                        // Control 2: archivos abiertos/bloqueados
                        var bloqueados = await Task.Run(() => loteFinalizacionBL.ValidarArchivosNoAbiertos(filas));
                        if (bloqueados.Count > 0)
                        {
                            lotesConError.Add($"Lote {lote.CdLote}: {bloqueados.Count} archivo(s) están abiertos " +
                                "o bloqueados y deben cerrarse antes de finalizar.");
                            continue;
                        }

                        await Task.Run(() => loteFinalizacionBL.FinalizarLote(lote.CdLote, filas, cdUsuario));
                    }
                    catch (Exception ex)
                    {
                        loteFinalizacionBL.RegistrarError("FrmFinalizarLote.btnFinalizarLote_Click", ex, cdUsuario);
                        lotesConError.Add($"Lote {lote.CdLote}: {ex.Message}");
                    }
                    finally
                    {
                        progressBar.Value = Math.Min(progressBar.Value + 1, progressBar.Maximum);
                    }
                }

                if (lotesConError.Count > 0)
                {
                    MessageBox.Show(
                        "Se procesaron los lotes con las siguientes advertencias/errores:\n\n" +
                        string.Join("\n", lotesConError),
                        "Finalización con observaciones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Los lotes seleccionados fueron finalizados correctamente.",
                        "Finalizar Lote", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarLotes();
            }
            finally
            {
                progressBar.Visible = false;
                btnFinalizarLote.Enabled = true;
                btnBuscar.Enabled = true;
            }
        }

        private static List<LoteFinalizacionBL.FilaFinalizacion> ObtenerFilasDelLote(
            LoteDAL loteDAL, ResultadoIADAL resultadoIADAL, int cdLote)
        {
            var resultados = resultadoIADAL.ObtenerPorLote(cdLote, mostrarTodos: true);
            var archivosPorPagina = loteDAL.ObtenerArchivosPaginasPorLote(cdLote)
                .ToDictionary(a => a.CdArchivoPagina, a => a);

            return resultados
                .Where(r => archivosPorPagina.ContainsKey(r.CdArchivoPagina))
                .Select(r => new LoteFinalizacionBL.FilaFinalizacion
                {
                    Archivo = archivosPorPagina[r.CdArchivoPagina],
                    Resultado = r
                }).ToList();
        }
    }
}
