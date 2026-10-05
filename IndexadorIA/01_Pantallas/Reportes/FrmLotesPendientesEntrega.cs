using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Pantallas.Reportes
{
    /// <summary>
    /// Pantalla de detalle del reporte "Estado x Entrega": para la entrega seleccionada
    /// en FrmEstadoXEntrega, muestra la agrupacion de lotes pendientes por estado
    /// (dgvEstados) y, al seleccionar un estado, el detalle de lotes filtrado por
    /// entrega y estado (dgvDetalle). Pantalla exclusiva del rol Administrador; solo
    /// funciona en modo local (no tiene soporte remoto via API).
    /// </summary>
    public partial class FrmLotesPendientesEntrega : Form
    {
        private readonly string _dsEntrega;

        public FrmLotesPendientesEntrega(string dsEntrega)
        {
            InitializeComponent();
            _dsEntrega = dsEntrega;
        }

        private void FrmLotesPendientesEntrega_Load(object sender, EventArgs e)
        {
            Text = $"Lotes Pendientes por Entrega - {_dsEntrega}";

            try
            {
                ConfigurarGrillaEstados();
                ConfigurarGrillaDetalle();
                CargarEstados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los lotes pendientes de la entrega: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrillaEstados()
        {
            dgvEstados.AutoGenerateColumns = false;
            dgvEstados.Columns.Clear();

            dgvEstados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsEstadoLote",
                DataPropertyName = "DsEstadoLote",
                HeaderText = "Estado",
                ReadOnly = true
            });

            dgvEstados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadLotes",
                DataPropertyName = "NuCantidadLotes",
                HeaderText = "Cantidad de Lotes",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
        }

        private void ConfigurarGrillaDetalle()
        {
            dgvDetalle.AutoGenerateColumns = false;
            dgvDetalle.Columns.Clear();

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = "cdLote", DataPropertyName = "CdLote", HeaderText = "Lote", ReadOnly = true });
            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = "dsEstadoLote", DataPropertyName = "DsEstadoLote", HeaderText = "Estado", ReadOnly = true });
            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = "nuCantidadArchivos", DataPropertyName = "NuCantidadArchivos", HeaderText = "Cant. Archivos", ReadOnly = true });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = "dsUsuarioAsignadoControl", DataPropertyName = "DsUsuarioAsignadoControl", HeaderText = "Asignado Control", ReadOnly = true });
            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = "feFinControl", DataPropertyName = "FeFinControl", HeaderText = "Fin Control", ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = "dsUsuarioControl", DataPropertyName = "DsUsuarioControl", HeaderText = "Usuario Control", ReadOnly = true });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = "dsUsuarioAsignadoAuditoria", DataPropertyName = "DsUsuarioAsignadoAuditoria", HeaderText = "Asignado Auditoria", ReadOnly = true });
            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = "feFinAuditoria", DataPropertyName = "FeFinAuditoria", HeaderText = "Fin Auditoria", ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = "dsUsuarioAuditor", DataPropertyName = "DsUsuarioAuditor", HeaderText = "Usuario Auditor", ReadOnly = true });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = "feFinalizado", DataPropertyName = "FeFinalizado", HeaderText = "Finalizado", ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = "dsUsuarioFinalizado", DataPropertyName = "DsUsuarioFinalizado", HeaderText = "Usuario Finalizado", ReadOnly = true });
        }

        private void CargarEstados()
        {
            var estadoXEntregaDAL = new EstadoXEntregaDAL();
            var estados = estadoXEntregaDAL.ObtenerLotesPendientesPorEstado(_dsEntrega);

            dgvEstados.DataSource = null;
            dgvEstados.DataSource = estados;

            dgvDetalle.DataSource = null;
        }

        private void dgvEstados_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEstados.CurrentRow?.DataBoundItem is not LoteEstadoAgrupadoDto estadoSeleccionado)
            {
                dgvDetalle.DataSource = null;
                return;
            }

            try
            {
                var estadoXEntregaDAL = new EstadoXEntregaDAL();
                var detalle = estadoXEntregaDAL.ObtenerDetalleLotesPorEntregaYEstado(_dsEntrega, estadoSeleccionado.CdEstadoLote);

                dgvDetalle.DataSource = null;
                dgvDetalle.DataSource = detalle;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el detalle de lotes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
