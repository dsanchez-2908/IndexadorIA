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

                AplicarColorEntornoSegunBaseDeDatos(nombreBaseDeDatos);
            }

            lblFecha.Text = DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy");

            AplicarPermisosPorRol();
        }

        /// <summary>
        /// Si la aplicación está conectada localmente (no por API) a una base de datos cuyo
        /// nombre contiene "QA" o "Test" (sin distinguir mayúsculas/minúsculas), pinta el menú
        /// y la barra de estado de color naranja para advertir claramente que no es producción.
        /// </summary>
        private void AplicarColorEntornoSegunBaseDeDatos(string? nombreBaseDeDatos)
        {
            bool esEntornoNoProductivo = !string.IsNullOrEmpty(nombreBaseDeDatos) &&
                (nombreBaseDeDatos.Contains("QA", StringComparison.OrdinalIgnoreCase) ||
                 nombreBaseDeDatos.Contains("Test", StringComparison.OrdinalIgnoreCase));

            Color colorFondo = esEntornoNoProductivo
                ? Color.FromArgb(230, 126, 34)
                : Color.FromArgb(45, 45, 48);

            menuStrip.BackColor = colorFondo;
            statusStrip.BackColor = colorFondo;
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

                // Oculta los menús de nivel superior "Consultas" y "Reportes" (y cualquier
                // menú nuevo que se agregue a futuro a nivel del menuStrip queda oculto por defecto)
                menuConsultas.Visible = false;
                menuReportes.Visible = false;

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

            // "Producción x Usuarios" es un reporte local (no tiene soporte remoto vía API)
            // y solo debe estar disponible para usuarios con rol ADMIN.
            bool esAdmin = string.Equals(SesionActual.UsuarioActual.CdRol, "ADMIN", StringComparison.OrdinalIgnoreCase);
            menuProduccionUsuarios.Visible = esAdmin && !SesionApi.ModoRemoto;

            // "Estado Proyecto Planos" solo debe estar disponible para usuarios con rol ADMIN.
            menuEstadoProyecto.Visible = esAdmin;

            // "Ayuda Control" es un mantenimiento solo para el rol ADMIN (funciona tanto local
            // como remoto, ya que el texto de ayuda se lee/escribe también vía API).
            menuAyudaControl.Visible = esAdmin;
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

        private void menuAsignarAuditoria_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new PlanosGCBA.FrmAsignarAuditoria());
        }

        private void menuAuditar_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new PlanosGCBA.FrmAuditar());
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

        private void menuProduccionUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Reportes.FrmProduccionUsuarios());
        }

        private void menuEstadoProyecto_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Reportes.FrmEstadoProyecto());
        }

        private void menuAyudaControl_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmAyudaControl());
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
