using IndexadorIA.Negocio;
using IndexadorIA.Negocio.Api;

namespace IndexadorIA.Pantallas
{
    /// <summary>
    /// Formulario para cambiar la contraseña
    /// </summary>
    public partial class FrmCambiarClave : Form
    {
        private readonly UsuarioBL _usuarioBL;
        private readonly ApiClienteServicio? _apiCliente;
        private readonly int _cdUsuario;
        private readonly bool _esPrimerIngreso;
        private readonly bool _usarApi;
        private readonly string? _dsUsuarioApi;
        private readonly string? _claveTemporalApi;

        /// <summary>
        /// Cuando el formulario se usó en modo API y el cambio fue exitoso,
        /// contiene la respuesta de login (con el token JWT) devuelta por la API.
        /// </summary>
        public LoginApiResponseDto? RespuestaLogin { get; private set; }

        public FrmCambiarClave(int cdUsuario, bool esPrimerIngreso = false)
        {
            InitializeComponent();
            _usuarioBL = new UsuarioBL();
            _cdUsuario = cdUsuario;
            _esPrimerIngreso = esPrimerIngreso;
            _usarApi = SesionApi.ModoRemoto;

            if (_usarApi)
                _apiCliente = new ApiClienteServicio();

            AjustarLayoutPrimerIngreso();
        }

        /// <summary>
        /// Constructor para modo remoto (API): cambia la clave temporal de un usuario
        /// que aún no tiene sesión local, usando el endpoint /api/auth/cambiar-clave-temporal.
        /// </summary>
        public FrmCambiarClave(string dsUsuarioApi, string claveTemporalApi)
        {
            InitializeComponent();
            _usuarioBL = new UsuarioBL();
            _apiCliente = new ApiClienteServicio();
            _esPrimerIngreso = true;
            _usarApi = true;
            _dsUsuarioApi = dsUsuarioApi;
            _claveTemporalApi = claveTemporalApi;

            AjustarLayoutPrimerIngreso();
        }

        private void AjustarLayoutPrimerIngreso()
        {
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

            if (_usarApi)
            {
                if (_dsUsuarioApi != null)
                    CambiarClaveTemporalApiAsync(nuevaClave, confirmarClave).ConfigureAwait(true);
                else
                    CambiarClaveApiAsync(claveActual, nuevaClave, confirmarClave).ConfigureAwait(true);

                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                // Si es primer ingreso, no validar clave actual
                if (_esPrimerIngreso)
                {
                    var (resultado, mensajeError) = _usuarioBL.CambiarClavePrimerIngreso(_cdUsuario, nuevaClave, confirmarClave);

                    if (resultado)
                    {
                        MessageBox.Show("Contraseña cambiada exitosamente", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show(mensajeError, "Error", 
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

        private async Task CambiarClaveApiAsync(string claveActual, string nuevaClave, string confirmarClave)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                btnGuardar.Enabled = false;

                await _apiCliente!.CambiarClaveAsync(claveActual, nuevaClave, confirmarClave);

                MessageBox.Show("Contraseña cambiada exitosamente", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ApiException ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGuardar.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar la contraseña: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGuardar.Enabled = true;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async Task CambiarClaveTemporalApiAsync(string nuevaClave, string confirmarClave)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                btnGuardar.Enabled = false;

                RespuestaLogin = await _apiCliente!.CambiarClaveTemporalAsync(
                    _dsUsuarioApi!, _claveTemporalApi!, nuevaClave, confirmarClave);

                MessageBox.Show("Contraseña cambiada exitosamente", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ApiException ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGuardar.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar la contraseña: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGuardar.Enabled = true;
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
