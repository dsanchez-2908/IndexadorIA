using IndexadorIA.Entidades;
using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas
{
    /// <summary>
    /// Formulario de Login
    /// </summary>
    public partial class FrmLogin : Form
    {
        private readonly UsuarioBL _usuarioBL;

        public FrmLogin()
        {
            InitializeComponent();
            _usuarioBL = new UsuarioBL();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            RealizarLogin();
        }

        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                RealizarLogin();
            }
        }

        private void RealizarLogin()
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text;

            if (string.IsNullOrEmpty(usuario))
            {
                MessageBox.Show("Por favor ingrese el usuario", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return;
            }

            if (string.IsNullOrEmpty(clave))
            {
                MessageBox.Show("Por favor ingrese la contraseña", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClave.Focus();
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                btnIngresar.Enabled = false;

                var (exito, mensaje, usuarioObj) = _usuarioBL.ValidarLogin(usuario, clave);

                if (exito && usuarioObj != null)
                {
                    SesionActual.UsuarioActual = usuarioObj;

                    // Verificar si es primer ingreso o tiene clave temporal
                    if (usuarioObj.SnPrimerIngreso || usuarioObj.SnClaveTemporal)
                    {
                        MessageBox.Show("Debe cambiar su contraseña temporal", "Primer Ingreso", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        using (FrmCambiarClave frmCambiar = new FrmCambiarClave(usuarioObj.CdUsuario, true))
                        {
                            if (frmCambiar.ShowDialog() == DialogResult.OK)
                            {
                                AbrirPrincipal();
                            }
                            else
                            {
                                SesionActual.CerrarSesion();
                                btnIngresar.Enabled = true;
                                Cursor = Cursors.Default;
                            }
                        }
                    }
                    else
                    {
                        AbrirPrincipal();
                    }
                }
                else
                {
                    MessageBox.Show(mensaje, "Error de Login", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtClave.Text = "";
                    txtClave.Focus();
                    btnIngresar.Enabled = true;
                    Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar ingresar: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIngresar.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void AbrirPrincipal()
        {
            this.Hide();
            FrmPrincipal frmPrincipal = new FrmPrincipal();
            frmPrincipal.FormClosed += (s, args) => this.Close();
            frmPrincipal.Show();
        }
    }
}
