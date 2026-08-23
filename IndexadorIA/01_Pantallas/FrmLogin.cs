using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;
using IndexadorIA.Negocio.Api;
using ConfiguracionCore = IndexadorIA.Datos.Configuracion;

namespace IndexadorIA.Pantallas
{
    /// <summary>
    /// Formulario de Login
    /// </summary>
    public partial class FrmLogin : Form
    {
        private readonly UsuarioBL _usuarioBL;
        private readonly ApiClienteServicio _apiCliente;

        public FrmLogin()
        {
            InitializeComponent();
            _usuarioBL = new UsuarioBL();
            _apiCliente = new ApiClienteServicio();
            string nombreBaseDeDatos = ConfiguracionCore.ObtenerNombreBaseDeDatos();
            lblBaseDatos.Text = string.IsNullOrEmpty(nombreBaseDeDatos)
                ? string.Empty
                : $"Base de datos: {nombreBaseDeDatos}";

            chkIngresoRemoto.CheckedChanged += ChkIngresoRemoto_CheckedChanged;
        }

        private void ChkIngresoRemoto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIngresoRemoto.Checked)
            {
                lblBaseDatos.Text = "Base de datos: Remoto (API)";
                return;
            }

            string nombreBaseDeDatos = ConfiguracionCore.ObtenerNombreBaseDeDatos();
            lblBaseDatos.Text = string.IsNullOrEmpty(nombreBaseDeDatos)
                ? string.Empty
                : $"Base de datos: {nombreBaseDeDatos}";
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

            if (chkIngresoRemoto.Checked)
            {
                RealizarLoginRemotoAsync(usuario, clave).ConfigureAwait(true);
                return;
            }

            RealizarLoginLocal(usuario, clave);
        }

        private void RealizarLoginLocal(string usuario, string clave)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                btnIngresar.Enabled = false;

                var (exito, mensaje, usuarioObj) = _usuarioBL.ValidarLogin(usuario, clave);

                if (exito && usuarioObj != null)
                {
                    SesionActual.UsuarioActual = usuarioObj;
                    SesionApi.ModoRemoto = false;

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

        private async Task RealizarLoginRemotoAsync(string usuario, string clave)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                btnIngresar.Enabled = false;

                LoginApiResponseDto respuesta = await _apiCliente.LoginAsync(usuario, clave);

                if (respuesta.RequiereCambioClave)
                {
                    MessageBox.Show("Debe cambiar su contraseña temporal", "Primer Ingreso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    using (FrmCambiarClave frmCambiar = new FrmCambiarClave(usuario, clave))
                    {
                        if (frmCambiar.ShowDialog() == DialogResult.OK && frmCambiar.RespuestaLogin != null)
                        {
                            AplicarSesionRemota(frmCambiar.RespuestaLogin);
                            AbrirPrincipal();
                        }
                        else
                        {
                            btnIngresar.Enabled = true;
                            Cursor = Cursors.Default;
                        }
                    }

                    return;
                }

                AplicarSesionRemota(respuesta);
                AbrirPrincipal();
            }
            catch (ApiException ex)
            {
                MessageBox.Show(ex.Message, "Error de Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtClave.Text = "";
                txtClave.Focus();
                btnIngresar.Enabled = true;
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar ingresar en modo remoto: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIngresar.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Construye un Usuario local mínimo a partir de la respuesta de login de la API,
        /// para que el resto de la aplicación (menús, permisos por rol) siga funcionando igual.
        /// </summary>
        private static void AplicarSesionRemota(LoginApiResponseDto respuesta)
        {
            SesionActual.UsuarioActual = new Usuario
            {
                CdUsuario = respuesta.CdUsuario,
                DsUsuario = respuesta.DsUsuario,
                DsNombreCompleto = respuesta.DsNombreCompleto,
                CdRol = respuesta.CdRol,
                DsRol = respuesta.DsRol,
                CdEstado = 1
            };

            SesionApi.ModoRemoto = true;
            SesionApi.Token = respuesta.Token;
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
