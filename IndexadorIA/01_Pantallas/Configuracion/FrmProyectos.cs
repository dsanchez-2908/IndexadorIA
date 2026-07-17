using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.Configuracion
{
    public partial class FrmProyectos : Form
    {
        private readonly ProyectoBL _proyectoBL;

        public FrmProyectos()
        {
            InitializeComponent();
            _proyectoBL = new ProyectoBL();
        }

        private void FrmProyectos_Load(object sender, EventArgs e)
        {
            CargarProyectos();
        }

        private void CargarProyectos()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var proyectos = _proyectoBL.ObtenerTodos();

                dgvProyectos.DataSource = proyectos;

                // Ocultar columnas innecesarias
                if (dgvProyectos.Columns["FeAlta"] != null)
                    dgvProyectos.Columns["FeAlta"].Visible = false;
                if (dgvProyectos.Columns["CdUsuarioAlta"] != null)
                    dgvProyectos.Columns["CdUsuarioAlta"].Visible = false;
                if (dgvProyectos.Columns["FeUltimaModificacion"] != null)
                    dgvProyectos.Columns["FeUltimaModificacion"].Visible = false;
                if (dgvProyectos.Columns["CdUsuarioModificacion"] != null)
                    dgvProyectos.Columns["CdUsuarioModificacion"].Visible = false;

                // Configurar columnas visibles
                int indexColumna = 0;

                if (dgvProyectos.Columns["CdProyecto"] != null)
                {
                    dgvProyectos.Columns["CdProyecto"].DisplayIndex = indexColumna++;
                    dgvProyectos.Columns["CdProyecto"].HeaderText = "Código";
                    dgvProyectos.Columns["CdProyecto"].Width = 80;
                }

                if (dgvProyectos.Columns["DsProyecto"] != null)
                {
                    dgvProyectos.Columns["DsProyecto"].DisplayIndex = indexColumna++;
                    dgvProyectos.Columns["DsProyecto"].HeaderText = "Proyecto";
                    dgvProyectos.Columns["DsProyecto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                if (dgvProyectos.Columns["SnActivo"] != null)
                {
                    dgvProyectos.Columns["SnActivo"].DisplayIndex = indexColumna++;
                    dgvProyectos.Columns["SnActivo"].HeaderText = "Activo";
                    dgvProyectos.Columns["SnActivo"].Width = 80;
                }

                // Botones al final
                if (dgvProyectos.Columns["colEditar"] != null)
                    dgvProyectos.Columns["colEditar"].DisplayIndex = indexColumna++;
                if (dgvProyectos.Columns["colEliminar"] != null)
                    dgvProyectos.Columns["colEliminar"].DisplayIndex = indexColumna++;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar proyectos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            var formDetalle = new FrmProyectoDetalle();
            if (formDetalle.ShowDialog() == DialogResult.OK)
            {
                CargarProyectos();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvProyectos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dgvProyectos.Columns["colEditar"].Index)
            {
                EditarProyecto(e.RowIndex);
            }
            else if (e.ColumnIndex == dgvProyectos.Columns["colEliminar"].Index)
            {
                EliminarProyecto(e.RowIndex);
            }
        }

        private void EditarProyecto(int rowIndex)
        {
            try
            {
                var cdProyecto = Convert.ToInt32(dgvProyectos.Rows[rowIndex].Cells["CdProyecto"].Value);
                var proyecto = _proyectoBL.ObtenerPorCodigo(cdProyecto);

                if (proyecto != null)
                {
                    var formDetalle = new FrmProyectoDetalle(proyecto);
                    if (formDetalle.ShowDialog() == DialogResult.OK)
                    {
                        CargarProyectos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar proyecto: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarProyecto(int rowIndex)
        {
            try
            {
                var cdProyecto = Convert.ToInt32(dgvProyectos.Rows[rowIndex].Cells["CdProyecto"].Value);
                var dsProyecto = dgvProyectos.Rows[rowIndex].Cells["DsProyecto"].Value?.ToString();

                var confirmacion = MessageBox.Show(
                    $"¿Está seguro de eliminar el proyecto '{dsProyecto}'?{Environment.NewLine}" +
                    "Esta acción no se puede deshacer.",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmacion == DialogResult.Yes)
                {
                    var resultado = _proyectoBL.Eliminar(cdProyecto);

                    if (resultado.exito)
                    {
                        MessageBox.Show(resultado.mensaje, "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarProyectos();
                    }
                    else
                    {
                        MessageBox.Show(resultado.mensaje, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar proyecto: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
