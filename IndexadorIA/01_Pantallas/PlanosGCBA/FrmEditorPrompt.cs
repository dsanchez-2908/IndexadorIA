using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    public partial class FrmEditorPrompt : Form
    {
        private readonly int _cdProyecto;
        private Prompt? _promptActual;

        public FrmEditorPrompt(int cdProyecto)
        {
            InitializeComponent();
            _cdProyecto = cdProyecto;
        }

        private void FrmEditorPrompt_Load(object sender, EventArgs e)
        {
            try
            {
                AplicarTemaOscuro();
                CargarPrompt();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el prompt: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarTemaOscuro()
        {
            // Colores del tema oscuro
            Color fondoOscuro = Color.FromArgb(45, 45, 48);
            Color fondoControles = Color.FromArgb(30, 30, 30);
            Color textoClaro = Color.White;
            Color bordeLeve = Color.FromArgb(63, 63, 70);

            // Aplicar al formulario
            this.BackColor = fondoOscuro;
            this.ForeColor = textoClaro;

            // Aplicar a los GroupBox
            foreach (Control control in this.Controls)
            {
                if (control is GroupBox groupBox)
                {
                    groupBox.BackColor = fondoOscuro;
                    groupBox.ForeColor = textoClaro;
                    AplicarTemaOscuroAControles(groupBox.Controls, fondoOscuro, fondoControles, textoClaro, bordeLeve);
                }
            }
        }

        private void AplicarTemaOscuroAControles(Control.ControlCollection controls, Color fondoOscuro, Color fondoControles, Color textoClaro, Color bordeLeve)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl is Label)
                {
                    ctrl.BackColor = fondoOscuro;
                    ctrl.ForeColor = textoClaro;
                }
                else if (ctrl is TextBox textBox)
                {
                    textBox.BackColor = fondoControles;
                    textBox.ForeColor = textoClaro;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (ctrl is Button button)
                {
                    button.BackColor = Color.FromArgb(0, 122, 204);
                    button.ForeColor = textoClaro;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                }

                // Recursivo para controles contenedores
                if (ctrl.HasChildren)
                {
                    AplicarTemaOscuroAControles(ctrl.Controls, fondoOscuro, fondoControles, textoClaro, bordeLeve);
                }
            }
        }

        private void CargarPrompt()
        {
            var promptDAL = new PromptDAL();
            _promptActual = promptDAL.ObtenerPorProyecto(_cdProyecto);

            if (_promptActual != null)
            {
                txtPrompt.Text = _promptActual.DsPrompt;

                if (_promptActual.FeUltimaModificacion.HasValue)
                {
                    lblUltimaModificacion.Text = 
                        $"Última modificación: {_promptActual.FeUltimaModificacion:dd/MM/yyyy HH:mm}";
                }
                else
                {
                    lblUltimaModificacion.Text = "Última modificación: N/A";
                }
            }
            else
            {
                MessageBox.Show("No se encontró un prompt configurado para este proyecto.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPrompt.Text = string.Empty;
                lblUltimaModificacion.Text = "Última modificación: N/A";
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPrompt.Text))
                {
                    MessageBox.Show("El prompt no puede estar vacío.",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_promptActual == null)
                {
                    MessageBox.Show("No se puede guardar el prompt porque no existe un registro base.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int cdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 1;

                var promptDAL = new PromptDAL();
                promptDAL.ActualizarPrompt(
                    _promptActual.CdPrompt,
                    txtPrompt.Text.Trim(),
                    cdUsuario);

                MessageBox.Show("Prompt guardado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el prompt: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
