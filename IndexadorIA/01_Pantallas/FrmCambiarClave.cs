using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas
{
    /// <summary>
    /// Formulario para cambiar la contraseña
    /// </summary>
    public partial class FrmCambiarClave : Form
    {
        private readonly UsuarioBL _usuarioBL;
        private readonly int _cdUsuario;
        private readonly bool _esPrimerIngreso;

        public FrmCambiarClave(int cdUsuario, bool esPrimerIngreso = false)
        {
            InitializeComponent();
            _usuarioBL = new UsuarioBL();
            _cdUsuario = cdUsuario;
            _esPrimerIngreso = esPrimerIngreso;

            // Si es primer ingreso, ocultar campo de clave actual
            if (_esPrimerIngreso)
            {
                lblActual.Visible = false;
                txtActual.Visible = false;

                // Ajustar posición de los controles
                lblNueva.Top = lblActual.Top;
                txtNueva.Top = txtActual.Top;
                lblConfirmar.Top = lblNueva.Top + 60;
                txtConfirmar.Top = txtNueva.Top + 60;
                btnGuardar.Top = txtConfirmar.Top + 45;
                btnCancelar.Top = txtConfirmar.Top + 45;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string claveActual = txtActual.Text;
            string nuevaClave = txtNueva.Text;
            string confirmarClave = txtConfirmar.Text;

            // Validaciones básicas
            if (!_esPrimerIngreso && string.IsNullOrEmpty(claveActual))
            {
                MessageBox.Show("Ingrese la contraseña actual", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtActual.Focus();
                return;
            }

            if (string.IsNullOrEmpty(nuevaClave))
            {
                MessageBox.Show("Ingrese la nueva contraseña", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNueva.Focus();
                return;
            }

            if (nuevaClave.Length < 4)
            {
                MessageBox.Show("La contraseña debe tener al menos 4 caracteres", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNueva.Focus();
                return;
            }

            if (string.IsNullOrEmpty(confirmarClave))
            {
                MessageBox.Show("Confirme la nueva contraseña", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmar.Focus();
                return;
            }

            if (nuevaClave != confirmarClave)
            {
                MessageBox.Show("Las contraseñas no coinciden", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmar.Text = "";
                txtConfirmar.Focus();
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                // Si es primer ingreso, no validar clave actual
                if (_esPrimerIngreso)
                {
                    // Simplemente actualizar la clave sin validar la actual
                    bool resultado = _usuarioBL.CambiarClave(_cdUsuario, "", nuevaClave, confirmarClave).exito;

                    if (!resultado)
                    {
                        // Si falla la validación normal, usar método directo
                        var usuario = _usuarioBL.ObtenerPorCodigo(_cdUsuario);
                        if (usuario != null && nuevaClave == confirmarClave)
                        {
                            usuario.DsClave = Datos.Seguridad.EncriptarSHA256(nuevaClave);
                            usuario.SnClaveTemporal = false;
                            usuario.SnPrimerIngreso = false;
                            resultado = _usuarioBL.Actualizar(usuario.CdUsuario, usuario.DsUsuario, 
                                usuario.DsNombreCompleto, usuario.IdRol, usuario.CdEstado).exito;
                        }
                    }

                    if (resultado)
                    {
                        MessageBox.Show("Contraseña cambiada exitosamente", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Error al cambiar la contraseña", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var (exito, mensaje) = _usuarioBL.CambiarClave(_cdUsuario, claveActual, nuevaClave, confirmarClave);

                    if (exito)
                    {
                        MessageBox.Show("Contraseña cambiada exitosamente", "Éxito", 
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
                MessageBox.Show($"Error al cambiar la contraseña: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (_esPrimerIngreso)
            {
                var result = MessageBox.Show("Debe cambiar su contraseña temporal para continuar. ¿Está seguro que desea cancelar?", 
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
