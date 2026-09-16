using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;
using IndexadorIA.Negocio.Api;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    /// <summary>
    /// Pantalla de Auditoria de lotes asignados al usuario auditor (cdEstado=8)
    /// </summary>
    public partial class FrmAuditar : Form
    {
        private const int CD_ESTADO_AUDITANDO = 8;
        private List<Lote> _lotesActuales = new();
        private readonly ApiClienteServicio? _apiCliente;

        public FrmAuditar()
        {
            InitializeComponent();
            _apiCliente = SesionApi.ModoRemoto ? new ApiClienteServicio() : null;
        }

        private void FrmAuditar_Load(object sender, EventArgs e)
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
                Name = "feUltimoCambio",
                DataPropertyName = "FeUltimoCambio",
                HeaderText = "Fecha de Asignaci\u00f3n",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuEstadoControlado",
                DataPropertyName = "NuEstadoControlado",
                HeaderText = "Controlado"
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuEstadoDatosIlegibles",
                DataPropertyName = "NuEstadoDatosIlegibles",
                HeaderText = "Datos Ilegibles"
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuEstadoPaginaIlegible",
                DataPropertyName = "NuEstadoPaginaIlegible",
                HeaderText = "Pagina Ilegible"
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsUsuarioFinControl",
                DataPropertyName = "DsUsuarioFinControl",
                HeaderText = "Usuario Controlador"
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsEstado",
                DataPropertyName = "DsEstado",
                HeaderText = "Estado"
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
                    var lotesApi = _apiCliente!.ObtenerLotesAuditoriaAsync(dsNombreLote, feAltaDesde, feAltaHasta)
                        .GetAwaiter().GetResult();

                    _lotesActuales = lotesApi.Select(l => new Lote
                    {
                        CdLote = l.CdLote,
                        DsNombreLote = l.DsNombreLote,
                        NuCantidadArchivos = l.NuCantidadArchivos,
                        FeUltimoCambio = l.FeUltimoCambio,
                        NuEstadoControlado = l.NuEstadoControlado,
                        NuEstadoDatosIlegibles = l.NuEstadoDatosIlegibles,
                        NuEstadoPaginaIlegible = l.NuEstadoPaginaIlegible,
                        DsUsuarioFinControl = l.DsUsuarioFinControl,
                        DsEstado = l.DsEstado
                    }).ToList();
                }
                else
                {
                    var loteDAL = new LoteDAL();
                    int? cdUsuarioAuditor = SesionActual.UsuarioActual?.CdUsuario;

                    _lotesActuales = loteDAL.ObtenerLotesPorEstadoFiltrado(
                        CD_ESTADO_AUDITANDO, dsNombreLote, feAltaDesde, feAltaHasta,
                        cdUsuarioAuditor: cdUsuarioAuditor, filtrarFechaPorUltimoCambio: true);
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
                AbrirVentanaAuditarLote();
            }
        }

        private void btnVerLote_Click(object sender, EventArgs e)
        {
            AbrirVentanaAuditarLote();
        }

        private void AbrirVentanaAuditarLote()
        {
            if (dgvLotes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un lote para auditar.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var lote = (Lote)dgvLotes.SelectedRows[0].DataBoundItem;

            using var frmAuditarLote = new FrmAuditarLote(lote.CdLote, lote.DsNombreLote);
            frmAuditarLote.ShowDialog(this);

            CargarLotes();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
