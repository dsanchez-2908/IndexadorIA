using IndexadorIA.Negocio;
using IndexadorIA.Negocio.Api;
using IndexadorIA.Pantallas.Configuracion;
using IndexadorIA.Pantallas.PlanosGCBA;
using IndexadorIA.Pantallas.Reportes;
using ConfiguracionCore = IndexadorIA.Datos.Configuracion;

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

            if (SesionApi.ModoRemoto)
            {
                lblBaseDatos.Text = "Base de datos: Remoto (API)";
            }
            else
            {
                string nombreBaseDeDatos = ConfiguracionCore.ObtenerNombreBaseDeDatos();
                lblBaseDatos.Text = string.IsNullOrEmpty(nombreBaseDeDatos)
                    ? string.Empty
                    : $"Base de datos: {nombreBaseDeDatos}";
            }

            lblFecha.Text = DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy");

            AplicarPermisosPorRol();
        }

        /// <summary>
        /// Restringe la visibilidad de los menús según el rol del usuario autenticado.
        /// El rol "Data Entry" solo puede acceder a:
        ///   Planos de GCBA / Control y Finalización
        ///   Configuración / Cambiar Clave y Cerrar Sesión
        /// </summary>
        private void AplicarPermisosPorRol()
        {
            if (SesionActual.UsuarioActual == null)
            {
                return;
            }

            if (string.Equals(SesionActual.UsuarioActual.CdRol, "DATAENTRY", StringComparison.OrdinalIgnoreCase))
            {
                // Oculta el menú "Lotes" completo
                menuLotes.Visible = false;

                // En "Planos de GCBA" solo deja visible "Control y Finalización"
                menuIngresoArchivos.Visible = false;
                menuSeparacionImagenes.Visible = false;
                menuPreparacionLotes.Visible = false;
                menuPreparacionImagenes.Visible = false;
                menuProcesamientoIA.Visible = false;
                menuAsignacionLote.Visible = false;
                menuFinalizarLote.Visible = false;

                // En "Configuración" solo deja visible "Cambiar Clave" y "Cerrar Sesión"
                menuProyectos.Visible = false;
                menuUsuarios.Visible = false;
                toolStripSeparator1.Visible = false;
            }
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

        private void menuPreparacionLotes_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmPreparacionLotes());
        }

        private void menuPreparacionImagenes_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmPreparacionImagenes());
        }

        private void menuProcesamientoIA_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new PlanosGCBA.FrmProcesamientoIA());
        }

        private void menuAsignacionLote_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new PlanosGCBA.FrmAsignacionLote());
        }

        private void menuControlFinalizacion_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new PlanosGCBA.FrmControlFinalizacion());
        }

        private void menuFinalizarLote_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new PlanosGCBA.FrmFinalizarLote());
        }

        private void menuMonitorLotes_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new PlanosGCBA.FrmMonitorLotes());
        }

        private void menuConsumosIA_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Reportes.FrmConsumosIA());
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
