using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;
using IndexadorIA.Negocio.Api;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    /// <summary>
    /// Pantalla de Control y Finalización de lotes asignados para control (cdEstado=6)
    /// </summary>
    public partial class FrmControlFinalizacion : Form
    {
        private const int CD_ESTADO_CONTROLANDO = 6;
        private List<Lote> _lotesActuales = new();

        public FrmControlFinalizacion()
        {
            InitializeComponent();
        }

        private void FrmControlFinalizacion_Load(object sender, EventArgs e)
        {
            try
            {
                dtpFechaDesde.Value = DateTime.Now.AddDays(-30);
                dtpFechaHasta.Value = DateTime.Now;
                chkFiltrarFecha.Checked = false;
                dtpFechaDesde.Enabled = false;
                dtpFechaHasta.Enabled = false;

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

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "cdLote",
                DataPropertyName = "CdLote",
                HeaderText = "ID Lote",
                Width = 70
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsNombreLote",
                DataPropertyName = "DsNombreLote",
                HeaderText = "Nombre Lote"
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadArchivos",
                DataPropertyName = "NuCantidadArchivos",
                HeaderText = "Cant. Archivos"
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuControlados",
                DataPropertyName = "NuControlados",
                HeaderText = "Controlado"
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuPendientes",
                DataPropertyName = "NuPendientes",
                HeaderText = "Pendiente"
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsEstado",
                DataPropertyName = "DsEstado",
                HeaderText = "Estado"
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "feAltaLote",
                DataPropertyName = "FeAltaLote",
                HeaderText = "Fecha Alta",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });
        }

        private void CargarLotes()
        {
            try
            {
                string? dsNombreLote = string.IsNullOrWhiteSpace(txtNombreLote.Text) ? null : txtNombreLote.Text.Trim();
                DateTime? feAltaDesde = chkFiltrarFecha.Checked ? dtpFechaDesde.Value : null;
                DateTime? feAltaHasta = chkFiltrarFecha.Checked ? dtpFechaHasta.Value : null;

                if (SesionApi.ModoRemoto)
                {
                    var apiCliente = new ApiClienteServicio();
                    List<LoteResumenApiDto> lotesRemotos = apiCliente
                        .ObtenerLotesEnControlAsync(dsNombreLote, feAltaDesde, feAltaHasta)
                        .GetAwaiter().GetResult();

                    _lotesActuales = lotesRemotos.Select(l => new Lote
                    {
                        CdLote = l.CdLote,
                        DsNombreLote = l.DsNombreLote,
                        NuCantidadArchivos = l.NuCantidadArchivos,
                        CdEstadoLote = l.CdEstadoLote,
                        DsEstado = l.DsEstado,
                        FeAltaLote = l.FeAltaLote,
                        NuControlados = l.NuControlados,
                        NuPendientes = l.NuPendientes
                    }).ToList();
                }
                else
                {
                    var loteDAL = new LoteDAL();
                    int? cdUsuarioAsignado = SesionActual.UsuarioActual?.CdUsuario;

                    _lotesActuales = loteDAL.ObtenerLotesPorEstadoFiltrado(
                        CD_ESTADO_CONTROLANDO, dsNombreLote, feAltaDesde, feAltaHasta, cdUsuarioAsignado);
                }

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

        private void dgvLotes_SelectionChanged(object sender, EventArgs e)
        {
            btnVerLote.Enabled = dgvLotes.SelectedRows.Count > 0;
        }

        private void dgvLotes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                AbrirVentanaVerLote();
            }
        }

        private void btnVerLote_Click(object sender, EventArgs e)
        {
            AbrirVentanaVerLote();
        }

        private void AbrirVentanaVerLote()
        {
            if (dgvLotes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un lote para ver su detalle.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var lote = (Lote)dgvLotes.SelectedRows[0].DataBoundItem;

            using var frmVerLote = new FrmVerLote(lote.CdLote);
            frmVerLote.ShowDialog(this);

            CargarLotes();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
