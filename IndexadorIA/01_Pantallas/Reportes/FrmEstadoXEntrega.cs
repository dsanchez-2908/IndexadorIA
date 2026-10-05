using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Pantallas.Reportes
{
    /// <summary>
    /// Pantalla de reporte "Estado x Entrega": muestra, por cada entrega
    /// (TD_ARCHIVOS_PAGINAS.dsEntrega), el total de lotes/planos, cuantos estan
    /// pendientes (cdEstadoLote IN 1..10) y cuantos fueron enviados (cdEstadoLote = 11).
    /// Al seleccionar una entrega en la grilla de Lotes, el boton "Ver Lotes Pendientes"
    /// abre FrmLotesPendientesEntrega con el detalle de esa entrega.
    /// Pantalla exclusiva del rol Administrador; solo funciona en modo local (no tiene
    /// soporte remoto via API).
    /// </summary>
    public partial class FrmEstadoXEntrega : Form
    {
        public FrmEstadoXEntrega()
        {
            InitializeComponent();
        }

        private void FrmEstadoXEntrega_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarGrillaLotes();
                ConfigurarGrillaPlanos();
                CargarResumen();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el estado por entrega: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrillaLotes()
        {
            dgvLotes.AutoGenerateColumns = false;
            dgvLotes.Columns.Clear();

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsEntrega",
                DataPropertyName = "DsEntrega",
                HeaderText = "Entrega",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuTotalLotes",
                DataPropertyName = "NuTotalLotes",
                HeaderText = "Total de Lotes",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuLotesPendientes",
                DataPropertyName = "NuLotesPendientes",
                HeaderText = "Lotes Pendientes",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuLotesEnviados",
                DataPropertyName = "NuLotesEnviados",
                HeaderText = "Lotes Enviados",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
        }

        private void ConfigurarGrillaPlanos()
        {
            dgvPlanos.AutoGenerateColumns = false;
            dgvPlanos.Columns.Clear();

            dgvPlanos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsEntrega",
                DataPropertyName = "DsEntrega",
                HeaderText = "Entrega",
                ReadOnly = true
            });

            dgvPlanos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuTotalPlanos",
                DataPropertyName = "NuTotalPlanos",
                HeaderText = "Total de Planos",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvPlanos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuPlanosPendientes",
                DataPropertyName = "NuPlanosPendientes",
                HeaderText = "Planos Pendientes",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvPlanos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuPlanosEnviados",
                DataPropertyName = "NuPlanosEnviados",
                HeaderText = "Planos Enviados",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
        }

        private void CargarResumen()
        {
            var estadoXEntregaDAL = new EstadoXEntregaDAL();
            var resumen = estadoXEntregaDAL.ObtenerResumenPorEntrega();

            dgvLotes.DataSource = null;
            dgvLotes.DataSource = resumen;

            dgvPlanos.DataSource = null;
            dgvPlanos.DataSource = resumen;

            btnVerLotesPendientes.Enabled = false;
        }

        private void dgvLotes_SelectionChanged(object sender, EventArgs e)
        {
            btnVerLotesPendientes.Enabled = dgvLotes.CurrentRow?.DataBoundItem is EstadoXEntregaDto;
        }

        private void btnVerLotesPendientes_Click(object sender, EventArgs e)
        {
            if (dgvLotes.CurrentRow?.DataBoundItem is not EstadoXEntregaDto entregaSeleccionada)
            {
                MessageBox.Show("Seleccione una entrega para ver los lotes pendientes.",
                    "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var frmLotesPendientes = new FrmLotesPendientesEntrega(entregaSeleccionada.DsEntrega);
            frmLotesPendientes.ShowDialog(this);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
