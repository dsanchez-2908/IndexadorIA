using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    /// <summary>
    /// Pantalla Monitor de Lotes: permite a un supervisor visualizar el estado de los
    /// lotes agrupados por usuario asignado (asignados, controlados y pendientes) y
    /// acceder al detalle de los lotes de un usuario en particular.
    /// </summary>
    public partial class FrmMonitorLotes : Form
    {
        private List<MonitorLoteUsuarioDto> _resumenActual = new();

        public FrmMonitorLotes()
        {
            InitializeComponent();
        }

        private void FrmMonitorLotes_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarDataGridView();
                CargarResumen();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvUsuarios.AutoGenerateColumns = false;
            dgvUsuarios.Columns.Clear();

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsUsuario",
                DataPropertyName = "DsUsuario",
                HeaderText = "Usuario",
                ReadOnly = true
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuTotalLotesAsignados",
                DataPropertyName = "NuTotalLotesAsignados",
                HeaderText = "Total Lotes Asignados",
                ReadOnly = true
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadPlanosAsignados",
                DataPropertyName = "NuCantidadPlanosAsignados",
                HeaderText = "Cant. Planos Asignados",
                ReadOnly = true
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuTotalLotesControlados",
                DataPropertyName = "NuTotalLotesControlados",
                HeaderText = "Total Lotes Controlados",
                ReadOnly = true
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadPlanosControlados",
                DataPropertyName = "NuCantidadPlanosControlados",
                HeaderText = "Cant. Planos Controlados",
                ReadOnly = true
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuTotalLotesPendientes",
                DataPropertyName = "NuTotalLotesPendientes",
                HeaderText = "Total Lotes Pendientes",
                ReadOnly = true
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadPlanosPendientes",
                DataPropertyName = "NuCantidadPlanosPendientes",
                HeaderText = "Cant. Planos Pendientes",
                ReadOnly = true
            });
        }

        private void CargarResumen()
        {
            try
            {
                var loteDAL = new LoteDAL();
                _resumenActual = loteDAL.ObtenerMonitorLotesPorUsuario();

                dgvUsuarios.DataSource = null;
                dgvUsuarios.DataSource = _resumenActual;

                ActualizarTotalizador();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el resumen de lotes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarTotalizador()
        {
            lblTotalizadorUsuarios.Text = $"Total: {_resumenActual.Count}";
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarResumen();
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            btnVerDetalle.Enabled = dgvUsuarios.SelectedRows.Count > 0;
        }

        private void dgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                AbrirDetalle();
            }
        }

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            AbrirDetalle();
        }

        private void AbrirDetalle()
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario para ver el detalle de sus lotes.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var usuario = (MonitorLoteUsuarioDto)dgvUsuarios.SelectedRows[0].DataBoundItem;

            using var frmDetalle = new FrmMonitorLotesDetalle(usuario.CdUsuario, usuario.DsUsuario);
            frmDetalle.ShowDialog(this);

            CargarResumen();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
