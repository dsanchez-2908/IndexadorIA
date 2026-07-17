using IndexadorIA.Negocio;
using IndexadorIA.Pantallas.Configuracion;
using IndexadorIA.Pantallas.PlanosGCBA;

namespace IndexadorIA.Pantallas
{
    /// <summary>
    /// Formulario principal de la aplicación
    /// </summary>
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            // Mostrar información del usuario en la barra de estado
            if (SesionActual.UsuarioActual != null)
            {
                lblUsuario.Text = $"Usuario: {SesionActual.UsuarioActual.DsNombreCompleto}";
            }

            lblFecha.Text = DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy");
        }

        private void menuUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmUsuarios());
        }

        private void menuProyectos_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmProyectos());
        }

        private void menuIngresoArchivos_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmIngresoArchivos());
        }

        private void menuSeparacionImagenes_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmSeparacionImagenes());
        }

        private void menuCambiarClave_Click(object sender, EventArgs e)
        {
            if (SesionActual.UsuarioActual != null)
            {
                using (FrmCambiarClave frm = new FrmCambiarClave(SesionActual.UsuarioActual.CdUsuario, false))
                {
                    frm.ShowDialog();
                }
            }
        }

        private void menuCerrarSesion_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea cerrar sesión?", "Confirmar", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SesionActual.CerrarSesion();
                this.Close();
            }
        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SesionActual.EstaAutenticado)
            {
                var result = MessageBox.Show("¿Está seguro que desea salir?", "Confirmar", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Abre un formulario dentro del panel de contenido
        /// </summary>
        private void AbrirFormularioEnPanel(Form formulario)
        {
            // Limpiar el panel
            panelContenido.Controls.Clear();

            // Configurar el formulario
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;

            // Agregar al panel
            panelContenido.Controls.Add(formulario);
            formulario.Show();
        }
    }
}
