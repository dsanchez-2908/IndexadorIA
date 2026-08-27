namespace IndexadorIA.Pantallas
{
    partial class FrmPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            menuStrip = new MenuStrip();
            menuLotes = new ToolStripMenuItem();
            menuMonitorLotes = new ToolStripMenuItem();
            menuPlanosGCBA = new ToolStripMenuItem();
            menuIngresoArchivos = new ToolStripMenuItem();
            menuSeparacionImagenes = new ToolStripMenuItem();
            menuPreparacionLotes = new ToolStripMenuItem();
            menuPreparacionImagenes = new ToolStripMenuItem();
            menuProcesamientoIA = new ToolStripMenuItem();
            menuAsignacionLote = new ToolStripMenuItem();
            menuControlFinalizacion = new ToolStripMenuItem();
            menuFinalizarLote = new ToolStripMenuItem();
            menuConsultas = new ToolStripMenuItem();
            menuReportes = new ToolStripMenuItem();
            menuConsumosIA = new ToolStripMenuItem();
            menuConfiguracion = new ToolStripMenuItem();
            menuProyectos = new ToolStripMenuItem();
            menuUsuarios = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            menuCambiarClave = new ToolStripMenuItem();
            menuCerrarSesion = new ToolStripMenuItem();
            statusStrip = new StatusStrip();
            lblUsuario = new ToolStripStatusLabel();
            lblBaseDatos = new ToolStripStatusLabel();
            lblFecha = new ToolStripStatusLabel();
            panelContenido = new Panel();
            lblBienvenida = new Label();
            menuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            panelContenido.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.BackColor = Color.FromArgb(45, 45, 48);
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { menuLotes, menuPlanosGCBA, menuConsultas, menuReportes, menuConfiguracion });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(5, 2, 0, 2);
            menuStrip.Size = new Size(1050, 27);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            // 
            // menuLotes
            // 
            menuLotes.DropDownItems.AddRange(new ToolStripItem[] { menuMonitorLotes });
            menuLotes.Font = new Font("Segoe UI", 10F);
            menuLotes.ForeColor = Color.White;
            menuLotes.Name = "menuLotes";
            menuLotes.Size = new Size(54, 23);
            menuLotes.Text = "Lotes";
            // 
            // menuMonitorLotes
            // 
            menuMonitorLotes.BackColor = Color.FromArgb(45, 45, 48);
            menuMonitorLotes.ForeColor = Color.White;
            menuMonitorLotes.Name = "menuMonitorLotes";
            menuMonitorLotes.Size = new Size(184, 24);
            menuMonitorLotes.Text = "Monitor de Lotes";
            menuMonitorLotes.Click += menuMonitorLotes_Click;
            // 
            // menuPlanosGCBA
            // 
            menuPlanosGCBA.DropDownItems.AddRange(new ToolStripItem[] { menuIngresoArchivos, menuSeparacionImagenes, menuPreparacionLotes, menuPreparacionImagenes, menuProcesamientoIA, menuAsignacionLote, menuControlFinalizacion, menuFinalizarLote });
            menuPlanosGCBA.Font = new Font("Segoe UI", 10F);
            menuPlanosGCBA.ForeColor = Color.White;
            menuPlanosGCBA.Name = "menuPlanosGCBA";
            menuPlanosGCBA.Size = new Size(120, 23);
            menuPlanosGCBA.Text = "Planos de GCBA";
            // 
            // menuIngresoArchivos
            // 
            menuIngresoArchivos.BackColor = Color.FromArgb(45, 45, 48);
            menuIngresoArchivos.ForeColor = Color.White;
            menuIngresoArchivos.Name = "menuIngresoArchivos";
            menuIngresoArchivos.Size = new Size(244, 24);
            menuIngresoArchivos.Text = "Ingreso de Archivos";
            menuIngresoArchivos.Click += menuIngresoArchivos_Click;
            // 
            // menuSeparacionImagenes
            // 
            menuSeparacionImagenes.BackColor = Color.FromArgb(45, 45, 48);
            menuSeparacionImagenes.ForeColor = Color.White;
            menuSeparacionImagenes.Name = "menuSeparacionImagenes";
            menuSeparacionImagenes.Size = new Size(244, 24);
            menuSeparacionImagenes.Text = "Separación de Imágenes";
            menuSeparacionImagenes.Click += menuSeparacionImagenes_Click;
            // 
            // menuPreparacionLotes
            // 
            menuPreparacionLotes.BackColor = Color.FromArgb(45, 45, 48);
            menuPreparacionLotes.ForeColor = Color.White;
            menuPreparacionLotes.Name = "menuPreparacionLotes";
            menuPreparacionLotes.Size = new Size(244, 24);
            menuPreparacionLotes.Text = "Preparación de Lotes";
            menuPreparacionLotes.Click += menuPreparacionLotes_Click;
            // 
            // menuPreparacionImagenes
            // 
            menuPreparacionImagenes.BackColor = Color.FromArgb(45, 45, 48);
            menuPreparacionImagenes.ForeColor = Color.White;
            menuPreparacionImagenes.Name = "menuPreparacionImagenes";
            menuPreparacionImagenes.Size = new Size(244, 24);
            menuPreparacionImagenes.Text = "Preparación de Imágenes";
            menuPreparacionImagenes.Click += menuPreparacionImagenes_Click;
            // 
            // menuProcesamientoIA
            // 
            menuProcesamientoIA.BackColor = Color.FromArgb(45, 45, 48);
            menuProcesamientoIA.ForeColor = Color.White;
            menuProcesamientoIA.Name = "menuProcesamientoIA";
            menuProcesamientoIA.Size = new Size(244, 24);
            menuProcesamientoIA.Text = "Procesamiento por OpenIA";
            menuProcesamientoIA.Click += menuProcesamientoIA_Click;
            // 
            // menuAsignacionLote
            // 
            menuAsignacionLote.BackColor = Color.FromArgb(45, 45, 48);
            menuAsignacionLote.ForeColor = Color.White;
            menuAsignacionLote.Name = "menuAsignacionLote";
            menuAsignacionLote.Size = new Size(244, 24);
            menuAsignacionLote.Text = "Asignación de Lotes";
            menuAsignacionLote.Click += menuAsignacionLote_Click;
            // 
            // menuControlFinalizacion
            // 
            menuControlFinalizacion.BackColor = Color.FromArgb(45, 45, 48);
            menuControlFinalizacion.ForeColor = Color.White;
            menuControlFinalizacion.Name = "menuControlFinalizacion";
            menuControlFinalizacion.Size = new Size(244, 24);
            menuControlFinalizacion.Text = "Control de Lotes";
            menuControlFinalizacion.Click += menuControlFinalizacion_Click;
            // 
            // menuFinalizarLote
            // 
            menuFinalizarLote.BackColor = Color.FromArgb(45, 45, 48);
            menuFinalizarLote.ForeColor = Color.White;
            menuFinalizarLote.Name = "menuFinalizarLote";
            menuFinalizarLote.Size = new Size(244, 24);
            menuFinalizarLote.Text = "Finalizar Lote";
            menuFinalizarLote.Click += menuFinalizarLote_Click;
            // 
            // menuConsultas
            // 
            menuConsultas.Font = new Font("Segoe UI", 10F);
            menuConsultas.ForeColor = Color.White;
            menuConsultas.Name = "menuConsultas";
            menuConsultas.Size = new Size(81, 23);
            menuConsultas.Text = "Consultas";
            // 
            // menuReportes
            // 
            menuReportes.DropDownItems.AddRange(new ToolStripItem[] { menuConsumosIA });
            menuReportes.Font = new Font("Segoe UI", 10F);
            menuReportes.ForeColor = Color.White;
            menuReportes.Name = "menuReportes";
            menuReportes.Size = new Size(75, 23);
            menuReportes.Text = "Reportes";
            // 
            // menuConsumosIA
            // 
            menuConsumosIA.BackColor = Color.FromArgb(45, 45, 48);
            menuConsumosIA.ForeColor = Color.White;
            menuConsumosIA.Name = "menuConsumosIA";
            menuConsumosIA.Size = new Size(160, 24);
            menuConsumosIA.Text = "Consumos IA";
            menuConsumosIA.Click += menuConsumosIA_Click;
            // 
            // menuConfiguracion
            // 
            menuConfiguracion.DropDownItems.AddRange(new ToolStripItem[] { menuProyectos, menuUsuarios, toolStripSeparator1, menuCambiarClave, menuCerrarSesion });
            menuConfiguracion.Font = new Font("Segoe UI", 10F);
            menuConfiguracion.ForeColor = Color.White;
            menuConfiguracion.Name = "menuConfiguracion";
            menuConfiguracion.Size = new Size(106, 23);
            menuConfiguracion.Text = "Configuración";
            // 
            // menuProyectos
            // 
            menuProyectos.BackColor = Color.FromArgb(45, 45, 48);
            menuProyectos.ForeColor = Color.White;
            menuProyectos.Name = "menuProyectos";
            menuProyectos.Size = new Size(166, 24);
            menuProyectos.Text = "Proyectos";
            menuProyectos.Click += menuProyectos_Click;
            // 
            // menuUsuarios
            // 
            menuUsuarios.BackColor = Color.FromArgb(45, 45, 48);
            menuUsuarios.ForeColor = Color.White;
            menuUsuarios.Name = "menuUsuarios";
            menuUsuarios.Size = new Size(166, 24);
            menuUsuarios.Text = "Usuarios";
            menuUsuarios.Click += menuUsuarios_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.BackColor = Color.FromArgb(45, 45, 48);
            toolStripSeparator1.ForeColor = Color.White;
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(163, 6);
            // 
            // menuCambiarClave
            // 
            menuCambiarClave.BackColor = Color.FromArgb(45, 45, 48);
            menuCambiarClave.ForeColor = Color.White;
            menuCambiarClave.Name = "menuCambiarClave";
            menuCambiarClave.Size = new Size(166, 24);
            menuCambiarClave.Text = "Cambiar Clave";
            menuCambiarClave.Click += menuCambiarClave_Click;
            // 
            // menuCerrarSesion
            // 
            menuCerrarSesion.BackColor = Color.FromArgb(45, 45, 48);
            menuCerrarSesion.ForeColor = Color.White;
            menuCerrarSesion.Name = "menuCerrarSesion";
            menuCerrarSesion.Size = new Size(166, 24);
            menuCerrarSesion.Text = "Cerrar Sesión";
            menuCerrarSesion.Click += menuCerrarSesion_Click;
            // 
            // statusStrip
            // 
            statusStrip.BackColor = Color.FromArgb(45, 45, 48);
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { lblUsuario, lblBaseDatos, lblFecha });
            statusStrip.Location = new Point(0, 503);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 12, 0);
            statusStrip.Size = new Size(1050, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            // 
            // lblUsuario
            // 
            lblUsuario.Font = new Font("Segoe UI", 9F);
            lblUsuario.ForeColor = Color.White;
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(50, 17);
            lblUsuario.Text = "Usuario:";
            // 
            // lblBaseDatos
            // 
            lblBaseDatos.Font = new Font("Segoe UI", 9F);
            lblBaseDatos.ForeColor = Color.White;
            lblBaseDatos.Name = "lblBaseDatos";
            lblBaseDatos.Size = new Size(82, 17);
            lblBaseDatos.Text = "Base de datos:";
            // 
            // lblFecha
            // 
            lblFecha.Font = new Font("Segoe UI", 9F);
            lblFecha.ForeColor = Color.White;
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(905, 17);
            lblFecha.Spring = true;
            lblFecha.Text = "Fecha";
            lblFecha.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panelContenido
            // 
            panelContenido.BackColor = Color.FromArgb(32, 32, 32);
            panelContenido.Controls.Add(lblBienvenida);
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.Location = new Point(0, 27);
            panelContenido.Margin = new Padding(3, 2, 3, 2);
            panelContenido.Name = "panelContenido";
            panelContenido.Size = new Size(1050, 476);
            panelContenido.TabIndex = 2;
            // 
            // lblBienvenida
            // 
            lblBienvenida.Dock = DockStyle.Fill;
            lblBienvenida.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblBienvenida.ForeColor = Color.White;
            lblBienvenida.Location = new Point(0, 0);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(1050, 476);
            lblBienvenida.TabIndex = 0;
            lblBienvenida.Text = "Bienvenido a IndexadorIA";
            lblBienvenida.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1050, 525);
            Controls.Add(panelContenido);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "IndexadorIA - Sistema de Indexación con IA (v20260827)";
            WindowState = FormWindowState.Maximized;
            FormClosing += FrmPrincipal_FormClosing;
            Load += FrmPrincipal_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            panelContenido.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private MenuStrip menuStrip;
        private ToolStripMenuItem menuPlanosGCBA;
        private ToolStripMenuItem menuIngresoArchivos;
        private ToolStripMenuItem menuSeparacionImagenes;
        private ToolStripMenuItem menuPreparacionLotes;
        private ToolStripMenuItem menuPreparacionImagenes;
        private ToolStripMenuItem menuProcesamientoIA;
        private ToolStripMenuItem menuAsignacionLote;
        private ToolStripMenuItem menuControlFinalizacion;
        private ToolStripMenuItem menuFinalizarLote;
        private ToolStripMenuItem menuLotes;
        private ToolStripMenuItem menuMonitorLotes;
        private ToolStripMenuItem menuConsultas;
        private ToolStripMenuItem menuReportes;
        private ToolStripMenuItem menuConsumosIA;
        private ToolStripMenuItem menuConfiguracion;
        private ToolStripMenuItem menuProyectos;
        private ToolStripMenuItem menuUsuarios;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem menuCambiarClave;
        private ToolStripMenuItem menuCerrarSesion;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblUsuario;
        private ToolStripStatusLabel lblBaseDatos;
        private ToolStripStatusLabel lblFecha;
        private Panel panelContenido;
        private Label lblBienvenida;
    }
}
