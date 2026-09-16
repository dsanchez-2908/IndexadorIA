using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    /// <summary>
    /// Pantalla de Monitor de Auditoria: muestra la cantidad de lotes pendientes de
    /// asignar auditoria y, agrupado por usuario auditor, el resumen de lotes y planos
    /// asignados/auditados/pendientes. Permite ver el detalle de lotes de un auditor
    /// y reasignarlos a otro usuario. Uso local exclusivo para el rol administrador.
    /// </summary>
    public partial class FrmMonitorAuditoria : Form
    {
        private List<MonitorAuditoriaUsuarioDto> _resumenActual = new();

        public FrmMonitorAuditoria()
        {
            InitializeComponent();
        }

        private void FrmMonitorAuditoria_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarDataGridView();
                CargarDatos();
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
                HeaderText = "Total de Lotes Asignados",
                ReadOnly = true
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadPlanosAsignados",
                DataPropertyName = "NuCantidadPlanosAsignados",
                HeaderText = "Total de Planos Asignados",
                ReadOnly = true
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuTotalLotesAuditados",
                DataPropertyName = "NuTotalLotesAuditados",
                HeaderText = "Total de Lotes Auditados",
                ReadOnly = true
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadPlanosAuditados",
                DataPropertyName = "NuCantidadPlanosAuditados",
                HeaderText = "Total de Planos Auditados",
                ReadOnly = true
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuTotalLotesPendientes",
                DataPropertyName = "NuTotalLotesPendientes",
                HeaderText = "Total de Lotes Pendientes",
                ReadOnly = true
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadPlanosPendientes",
                DataPropertyName = "NuCantidadPlanosPendientes",
                HeaderText = "Total de Planos Pendientes",
                ReadOnly = true
            });
        }

        private void CargarDatos()
        {
            try
            {
                var loteDAL = new LoteDAL();

                int cdPendientesAsignar = loteDAL.ObtenerCantidadLotesPendientesAsignarAuditoria();
                lblPendientesAsignar.Text = $"Lotes pendientes de asignar auditoria: {cdPendientesAsignar}";

                _resumenActual = loteDAL.ObtenerMonitorAuditoriaPorUsuario();

                dgvUsuarios.DataSource = null;
                dgvUsuarios.DataSource = _resumenActual;

                ActualizarTotalizador();
                btnVerDetalle.Enabled = dgvUsuarios.SelectedRows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos del monitor de auditoria: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarTotalizador()
        {
            lblTotalizadorUsuarios.Text = $"Total: {_resumenActual.Count}";
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
                return;
            }

            if (dgvUsuarios.SelectedRows[0].DataBoundItem is not MonitorAuditoriaUsuarioDto seleccionado)
            {
                return;
            }

            using (var frm = new FrmMonitorAuditoriaDetalle(seleccionado.CdUsuario, seleccionado.DsUsuario))
            {
                frm.ShowDialog();
            }

            CargarDatos();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
