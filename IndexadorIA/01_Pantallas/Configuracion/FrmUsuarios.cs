using IndexadorIA.Entidades;
using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.Configuracion
{
    /// <summary>
    /// Formulario para gestionar usuarios
    /// </summary>
    public partial class FrmUsuarios : Form
    {
        private readonly UsuarioBL _usuarioBL;

        public FrmUsuarios()
        {
            InitializeComponent();
            _usuarioBL = new UsuarioBL();
            PersonalizarGrid();
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void PersonalizarGrid()
        {
            // Personalizar colores del DataGridView
            dgvUsuarios.BackgroundColor = Color.FromArgb(45, 45, 48);
            dgvUsuarios.ForeColor = Color.White;
            dgvUsuarios.GridColor = Color.FromArgb(62, 62, 66);

            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(62, 62, 66);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            dgvUsuarios.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvUsuarios.DefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 52);
            dgvUsuarios.EnableHeadersVisualStyles = false;
        }

        private void CargarUsuarios()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var usuarios = _usuarioBL.ObtenerTodos();

                dgvUsuarios.DataSource = usuarios;

                // Ocultar columna CdEstado (código numérico)
                if (dgvUsuarios.Columns["CdEstado"] != null)
                {
                    dgvUsuarios.Columns["CdEstado"].Visible = false;
                }

                // Ocultar columnas de ID que no necesita ver el usuario
                if (dgvUsuarios.Columns["IdRol"] != null)
                    dgvUsuarios.Columns["IdRol"].Visible = false;
                if (dgvUsuarios.Columns["CdRol"] != null)
                    dgvUsuarios.Columns["CdRol"].Visible = false;
                if (dgvUsuarios.Columns["DsClave"] != null)
                    dgvUsuarios.Columns["DsClave"].Visible = false;
                if (dgvUsuarios.Columns["SnClaveTemporal"] != null)
                    dgvUsuarios.Columns["SnClaveTemporal"].Visible = false;
                if (dgvUsuarios.Columns["SnPrimerIngreso"] != null)
                    dgvUsuarios.Columns["SnPrimerIngreso"].Visible = false;
                if (dgvUsuarios.Columns["CdUsuarioAlta"] != null)
                    dgvUsuarios.Columns["CdUsuarioAlta"].Visible = false;

                // Orden de columnas: primero datos, luego botones
                int indexColumna = 0;

                if (dgvUsuarios.Columns["CdUsuario"] != null)
                {
                    dgvUsuarios.Columns["CdUsuario"].DisplayIndex = indexColumna++;
                    dgvUsuarios.Columns["CdUsuario"].HeaderText = "Código";
                    dgvUsuarios.Columns["CdUsuario"].Width = 60;
                }
                if (dgvUsuarios.Columns["DsUsuario"] != null)
                {
                    dgvUsuarios.Columns["DsUsuario"].DisplayIndex = indexColumna++;
                    dgvUsuarios.Columns["DsUsuario"].HeaderText = "Usuario";
                    dgvUsuarios.Columns["DsUsuario"].Width = 120;
                }
                if (dgvUsuarios.Columns["DsNombreCompleto"] != null)
                {
                    dgvUsuarios.Columns["DsNombreCompleto"].DisplayIndex = indexColumna++;
                    dgvUsuarios.Columns["DsNombreCompleto"].HeaderText = "Nombre Completo";
                    dgvUsuarios.Columns["DsNombreCompleto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                if (dgvUsuarios.Columns["DsRol"] != null)
                {
                    dgvUsuarios.Columns["DsRol"].DisplayIndex = indexColumna++;
                    dgvUsuarios.Columns["DsRol"].HeaderText = "Rol";
                    dgvUsuarios.Columns["DsRol"].Width = 120;
                }
                if (dgvUsuarios.Columns["DsEstado"] != null)
                {
                    dgvUsuarios.Columns["DsEstado"].DisplayIndex = indexColumna++;
                    dgvUsuarios.Columns["DsEstado"].HeaderText = "Estado";
                    dgvUsuarios.Columns["DsEstado"].Width = 80;
                }
                if (dgvUsuarios.Columns["FeAlta"] != null)
                {
                    dgvUsuarios.Columns["FeAlta"].DisplayIndex = indexColumna++;
                    dgvUsuarios.Columns["FeAlta"].HeaderText = "Fecha Alta";
                    dgvUsuarios.Columns["FeAlta"].Width = 100;
                    dgvUsuarios.Columns["FeAlta"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }

                // Botones al final
                if (dgvUsuarios.Columns["colEditar"] != null)
                    dgvUsuarios.Columns["colEditar"].DisplayIndex = indexColumna++;
                if (dgvUsuarios.Columns["colRestablecer"] != null)
                    dgvUsuarios.Columns["colRestablecer"].DisplayIndex = indexColumna++;
                if (dgvUsuarios.Columns["colEliminar"] != null)
                    dgvUsuarios.Columns["colEliminar"].DisplayIndex = indexColumna++;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            // Cerrar el formulario (se limpiará del panel)
            this.Close();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (SesionActual.UsuarioActual == null) return;

            using (FrmUsuarioDetalle frm = new FrmUsuarioDetalle())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    CargarUsuarios();
                }
            }
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int cdUsuario = Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells["CdUsuario"].Value);

            // Botón Editar
            if (e.ColumnIndex == dgvUsuarios.Columns["colEditar"].Index)
            {
                EditarUsuario(cdUsuario);
            }
            // Botón Restablecer
            else if (e.ColumnIndex == dgvUsuarios.Columns["colRestablecer"].Index)
            {
                RestablecerClave(cdUsuario);
            }
            // Botón Eliminar
            else if (e.ColumnIndex == dgvUsuarios.Columns["colEliminar"].Index)
            {
                EliminarUsuario(cdUsuario);
            }
        }

        private void EditarUsuario(int cdUsuario)
        {
            using (FrmUsuarioDetalle frm = new FrmUsuarioDetalle(cdUsuario))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    CargarUsuarios();
                }
            }
        }

        private void RestablecerClave(int cdUsuario)
        {
            var result = MessageBox.Show("¿Está seguro que desea restablecer la contraseña de este usuario?", 
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;

                    var (exito, mensaje, claveTemporal) = _usuarioBL.RestablecerClave(cdUsuario);

                    if (exito && claveTemporal != null)
                    {
                        MessageBox.Show($"Contraseña restablecida exitosamente.\n\nClave temporal: {claveTemporal}", 
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarUsuarios();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al restablecer contraseña: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void EliminarUsuario(int cdUsuario)
        {
            // No permitir eliminar al propio usuario
            if (SesionActual.UsuarioActual?.CdUsuario == cdUsuario)
            {
                MessageBox.Show("No puede eliminar su propio usuario", "Advertencia", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("¿Está seguro que desea desactivar este usuario?", 
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;

                    var (exito, mensaje) = _usuarioBL.Eliminar(cdUsuario);

                    if (exito)
                    {
                        MessageBox.Show("Usuario desactivado exitosamente", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarUsuarios();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al desactivar usuario: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }
    }
}
