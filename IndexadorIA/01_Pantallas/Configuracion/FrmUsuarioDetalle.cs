using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.Configuracion
{
    /// <summary>
    /// Formulario para crear o editar un usuario
    /// </summary>
    public partial class FrmUsuarioDetalle : Form
    {
        private readonly UsuarioBL _usuarioBL;
        private readonly int? _cdUsuario;
        private Usuario? _usuario;

        public FrmUsuarioDetalle(int? cdUsuario = null)
        {
            InitializeComponent();
            _usuarioBL = new UsuarioBL();
            _cdUsuario = cdUsuario;
        }

        private void FrmUsuarioDetalle_Load(object sender, EventArgs e)
        {
            CargarRoles();
            CargarEstados();

            if (_cdUsuario.HasValue)
            {
                // Modo Edición
                lblTitulo.Text = "Editar Usuario";
                CargarUsuario(_cdUsuario.Value);

                // Ocultar campo de clave temporal en modo edición
                lblClaveTemporal.Visible = false;
                txtClaveTemporal.Visible = false;
            }
            else
            {
                // Modo Nuevo
                lblTitulo.Text = "Nuevo Usuario";
                cboEstado.SelectedValue = 1; // Activo por defecto
                lblEstado.Visible = false;
                cboEstado.Visible = false;

                // Mostrar campo de clave temporal en modo creación
                lblClaveTemporal.Visible = true;
                txtClaveTemporal.Visible = true;
            }
        }

        private void CargarRoles()
        {
            try
            {
                var roles = RolDAL.ObtenerActivos();
                cboRol.DataSource = roles;
                cboRol.DisplayMember = "DsRol";
                cboRol.ValueMember = "IdRol";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar roles: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEstados()
        {
            var estados = new List<object>
            {
                new { Valor = 1, Texto = "Activo" },
                new { Valor = 0, Texto = "Inactivo" }
            };

            cboEstado.DataSource = estados;
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
        }

        private void CargarUsuario(int cdUsuario)
        {
            try
            {
                _usuario = _usuarioBL.ObtenerPorCodigo(cdUsuario);

                if (_usuario != null)
                {
                    txtUsuario.Text = _usuario.DsUsuario;
                    txtNombreCompleto.Text = _usuario.DsNombreCompleto;
                    cboRol.SelectedValue = _usuario.IdRol;
                    cboEstado.SelectedValue = _usuario.CdEstado;
                }
                else
                {
                    MessageBox.Show("Usuario no encontrado", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuario: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos()) return;

            try
            {
                Cursor = Cursors.WaitCursor;

                if (_cdUsuario.HasValue)
                {
                    // Actualizar
                    var (exito, mensaje) = _usuarioBL.Actualizar(
                        _cdUsuario.Value,
                        txtUsuario.Text.Trim(),
                        txtNombreCompleto.Text.Trim(),
                        Convert.ToInt32(cboRol.SelectedValue),
                        Convert.ToInt32(cboEstado.SelectedValue)
                    );

                    if (exito)
                    {
                        MessageBox.Show("Usuario actualizado exitosamente", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Crear
                    if (SesionActual.UsuarioActual == null)
                    {
                        MessageBox.Show("Error: No hay sesión activa", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var (exito, mensaje, cdUsuarioNuevo) = _usuarioBL.Crear(
                        txtUsuario.Text.Trim(),
                        txtNombreCompleto.Text.Trim(),
                        txtClaveTemporal.Text.Trim(),
                        Convert.ToInt32(cboRol.SelectedValue),
                        SesionActual.UsuarioActual.CdUsuario
                    );

                    if (exito)
                    {
                        MessageBox.Show(mensaje, "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar usuario: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Ingrese el nombre de usuario", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombreCompleto.Text))
            {
                MessageBox.Show("Ingrese el nombre completo", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreCompleto.Focus();
                return false;
            }

            // Validar clave temporal solo en modo creación
            if (!_cdUsuario.HasValue)
            {
                if (string.IsNullOrWhiteSpace(txtClaveTemporal.Text))
                {
                    MessageBox.Show("Ingrese una clave temporal", "Validación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClaveTemporal.Focus();
                    return false;
                }

                if (txtClaveTemporal.Text.Trim().Length < 3)
                {
                    MessageBox.Show("La clave temporal debe tener al menos 3 caracteres", "Validación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClaveTemporal.Focus();
                    return false;
                }
            }

            if (cboRol.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un rol", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboRol.Focus();
                return false;
            }

            return true;
        }
    }
}
