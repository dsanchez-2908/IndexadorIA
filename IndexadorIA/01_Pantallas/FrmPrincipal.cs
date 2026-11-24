using IndexadorIA.Negocio;
using IndexadorIA.Negocio.Api;
using IndexadorIA.Pantallas.Configuracion;
using IndexadorIA.Pantallas.FinalizacionPlanos;
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
        ///   Consultas / Planos
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

                // Oculta los menús de nivel superior "Reportes" (y cualquier
                // menú nuevo que se agregue a futuro a nivel del menuStrip queda oculto por defecto).
                // "Consultas / Planos" si tiene soporte remoto via API, por lo que queda visible.
                menuReportes.Visible = false;
                menuFinalizacionPlanos.Visible = false;

                // En "Planos de GCBA" solo deja visible "Control y Finalización" y "Auditar"
                menuIngresoArchivos.Visible = false;
                menuSeparacionImagenes.Visible = false;
                menuPreparacionLotes.Visible = false;
                menuPreparacionImagenes.Visible = false;
                menuProcesamientoIA.Visible = false;
                menuAsignacionLote.Visible = false;
                menuAsignarAuditoria.Visible = false;
                menuAuditar.Visible = true;
                menuFinalizarLote.Visible = false;
                menuEnvio.Visible = false;

                // En "Configuración" solo deja visible "Cambiar Clave" y "Cerrar Sesión"
                menuProyectos.Visible = false;
                menuUsuarios.Visible = false;
                toolStripSeparator1.Visible = false;
            }

            // "Producción x Usuarios" es un reporte local (no tiene soporte remoto vía API)
            // y solo debe estar disponible para usuarios con rol ADMIN.
            bool esAdmin = string.Equals(SesionActual.UsuarioActual.CdRol, "ADMIN", StringComparison.OrdinalIgnoreCase);
            menuProduccionUsuarios.Visible = esAdmin && !SesionApi.ModoRemoto;

            // "Monitor Auditoria" es una pantalla local (no tiene soporte remoto vía API)
            // y solo debe estar disponible para usuarios con rol ADMIN.
            menuMonitorAuditoria.Visible = esAdmin && !SesionApi.ModoRemoto;

            // "Estado Proyecto Planos" solo debe estar disponible para usuarios con rol ADMIN.
            menuEstadoProyecto.Visible = esAdmin;

            // "Validador API SMP" es local (usa BD directa + SQLite) y solo para ADMIN.
            // "Control Final" es local por ahora (el modo remoto via API se incorpora despues).
            bool esDataEntry = string.Equals(SesionActual.UsuarioActual.CdRol, "DATAENTRY", StringComparison.OrdinalIgnoreCase);
            menuFinalizacionPlanos.Visible = (esAdmin || esDataEntry) && !SesionApi.ModoRemoto;
            menuValidadorApiSmp.Visible = esAdmin;
            menuCrearLotesFinal.Visible = esAdmin;
            menuMonitorLotesFinales.Visible = esAdmin;
            menuControlFinal.Visible = esAdmin || esDataEntry;

            // "Planos para enviar" solo debe estar disponible para usuarios con rol ADMIN.
            menuPlanosParaEnviar.Visible = esAdmin;

            // "Ayuda Control" es un mantenimiento solo para el rol ADMIN (funciona tanto local
            // como remoto, ya que el texto de ayuda se lee/escribe tambien via API).
            menuAyudaControl.Visible = esAdmin;

            // "Estado x Entrega" es un reporte local (no tiene soporte remoto via API)
            // y solo debe estar disponible para usuarios con rol ADMIN.
            menuEstadoXEntrega.Visible = esAdmin && !SesionApi.ModoRemoto;
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

        private void menuEnvio_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new PlanosGCBA.FrmEnvioLote());
        }

        private void menuMonitorLotes_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new PlanosGCBA.FrmMonitorLotes());
        }

        private void menuMonitorAuditoria_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new PlanosGCBA.FrmMonitorAuditoria());
        }

        private void menuMonitorLotesFinales_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmMonitorLotesFinales());
        }

        private void menuControlFinal_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmControlFinal());
        }

        private void menuCrearLotesFinal_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmCrearLotesFinal());
        }

        private void menuValidadorApiSmp_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmValidadorApiSmp());
        }

        private void menuPlanos_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Consultas.FrmConsultaPlanos());
        }

        private void menuConsumosIA_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Reportes.FrmConsumosIA());
        }

        private void menuProduccionUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Reportes.FrmProduccionUsuarios());
        }

        private void menuAuditoriaUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Reportes.FrmAuditoriaUsuarios());
        }

        private void menuEstadoProyecto_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Reportes.FrmEstadoProyecto());
        }

        private void menuPlanosParaEnviar_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Reportes.FrmPlanosParaEnviar());
        }

        private void menuEstadoXEntrega_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Reportes.FrmEstadoXEntrega());
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
