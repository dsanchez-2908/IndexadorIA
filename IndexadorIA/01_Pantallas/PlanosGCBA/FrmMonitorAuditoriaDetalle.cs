using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    /// <summary>
    /// Pantalla de detalle del Monitor de Auditoria: muestra los lotes de un usuario
    /// auditor en particular, con filtro por estado (Todo / Pendiente de Auditoria /
    /// Auditado) y permite reasignar a otro usuario los lotes que aun estan
    /// pendientes de auditoria.
    /// </summary>
    public partial class FrmMonitorAuditoriaDetalle : Form
    {
        private const int CD_ESTADO_AUDITANDO = 8;

        private readonly int _cdUsuarioAuditor;
        private readonly string _dsUsuario;
        private List<LoteMonitorAuditoriaDetalleDto> _lotesActuales = new();

        public FrmMonitorAuditoriaDetalle(int cdUsuarioAuditor, string dsUsuario)
        {
            InitializeComponent();
            _cdUsuarioAuditor = cdUsuarioAuditor;
            _dsUsuario = dsUsuario;
        }

        private void FrmMonitorAuditoriaDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                Text = $"Detalle de Auditoria - {_dsUsuario}";
                lblUsuario.Text = $"Usuario: {_dsUsuario}";

                rbTodo.Checked = true;

                ConfigurarDataGridView();
                CargarUsuarios();
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
                HeaderText = "Código Lote",
                Width = 90,
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
                Name = "dsEstadoLote",
                DataPropertyName = "DsEstadoLote",
                HeaderText = "Estado",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadPlanos",
                DataPropertyName = "NuCantidadPlanos",
                HeaderText = "Cant. Planos",
                ReadOnly = true
            });
        }

        private void CargarUsuarios()
        {
            var usuarioDAL = new UsuarioDAL();
            var usuarios = usuarioDAL.ObtenerTodos()
                .Where(u => u.CdUsuario != _cdUsuarioAuditor)
                .ToList();

            cboUsuarios.DisplayMember = "DsUsuario";
            cboUsuarios.ValueMember = "CdUsuario";
            cboUsuarios.DataSource = usuarios;
            cboUsuarios.SelectedIndex = -1;
        }

        private int? ObtenerFiltroEstadoSeleccionado()
        {
            if (rbPendienteAuditoria.Checked)
                return CD_ESTADO_AUDITANDO;

            if (rbAuditado.Checked)
                return 9;

            return null;
        }

        private void CargarLotes()
        {
            try
            {
                var loteDAL = new LoteDAL();
                _lotesActuales = loteDAL.ObtenerLotesAuditoriaPorUsuarioConEstadisticas(
                    _cdUsuarioAuditor, ObtenerFiltroEstadoSeleccionado());

                dgvLotes.DataSource = null;
                dgvLotes.DataSource = _lotesActuales;

                ActualizarTotalizador();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los lotes del usuario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarTotalizador()
        {
            lblTotalizadorLotes.Text = $"Total: {_lotesActuales.Count}";
        }

        private void rbFiltro_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                CargarLotes();
            }
        }

        private void dgvLotes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLotes.Columns[e.ColumnIndex].Name == "snSeleccionado")
            {
                dgvLotes.EndEdit();
            }
        }

        private void btnReasignarAuditoria_Click(object sender, EventArgs e)
        {
            try
            {
                var seleccionados = _lotesActuales
                    .Where(l => l.SnSeleccionado)
                    .ToList();

                if (seleccionados.Count == 0)
                {
                    MessageBox.Show("Seleccione al menos un lote para reasignar.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var noPendientes = seleccionados.Where(l => l.CdEstadoLote != CD_ESTADO_AUDITANDO).ToList();
                if (noPendientes.Count > 0)
                {
                    MessageBox.Show(
                        "Solo se pueden reasignar lotes que estén Pendientes de Auditoria. " +
                        $"Hay {noPendientes.Count} lote(s) seleccionado(s) que ya no están pendientes.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboUsuarios.SelectedValue is not int cdNuevoUsuario)
                {
                    MessageBox.Show("Seleccione el usuario al que desea reasignar los lotes.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var confirmacion = MessageBox.Show(
                    $"¿Desea reasignar {seleccionados.Count} lote(s) al usuario seleccionado?",
                    "Reasignar Auditoria", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes)
                    return;

                var loteDAL = new LoteDAL();
                loteDAL.AsignarAuditoria(seleccionados.Select(l => l.CdLote).ToList(), cdNuevoUsuario);

                MessageBox.Show("Los lotes fueron reasignados correctamente.",
                    "Reasignar Auditoria", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cboUsuarios.SelectedIndex = -1;
                CargarLotes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo reasignar la auditoria: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
