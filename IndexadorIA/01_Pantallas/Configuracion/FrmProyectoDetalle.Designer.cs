namespace IndexadorIA.Pantallas.Configuracion
{
    partial class FrmProyectoDetalle
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
            panelSuperior = new Panel();
            lblTitulo = new Label();
            panelContenido = new Panel();
            chkActivo = new CheckBox();
            txtNombre = new TextBox();
            lblNombre = new Label();
            panelBotones = new Panel();
            btnCancelar = new Button();
            btnGuardar = new Button();
            panelSuperior.SuspendLayout();
            panelContenido.SuspendLayout();
            panelBotones.SuspendLayout();
            SuspendLayout();
            // 
            // panelSuperior
            // 
            panelSuperior.BackColor = Color.FromArgb(45, 45, 48);
            panelSuperior.Controls.Add(lblTitulo);
            panelSuperior.Dock = DockStyle.Top;
            panelSuperior.Location = new Point(0, 0);
            panelSuperior.Name = "panelSuperior";
            panelSuperior.Padding = new Padding(20);
            panelSuperior.Size = new Size(500, 70);
            panelSuperior.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(183, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Nuevo Proyecto";
            // 
            // panelContenido
            // 
            panelContenido.BackColor = Color.FromArgb(32, 32, 32);
            panelContenido.Controls.Add(chkActivo);
            panelContenido.Controls.Add(txtNombre);
            panelContenido.Controls.Add(lblNombre);
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.Location = new Point(0, 70);
            panelContenido.Name = "panelContenido";
            panelContenido.Padding = new Padding(20);
            panelContenido.Size = new Size(500, 180);
            panelContenido.TabIndex = 1;
            // 
            // chkActivo
            // 
            chkActivo.AutoSize = true;
            chkActivo.Font = new Font("Segoe UI", 10F);
            chkActivo.ForeColor = Color.White;
            chkActivo.Location = new Point(20, 110);
            chkActivo.Name = "chkActivo";
            chkActivo.Size = new Size(78, 27);
            chkActivo.TabIndex = 2;
            chkActivo.Text = "Activo";
            chkActivo.UseVisualStyleBackColor = true;
            // 
            // txtNombre
            // 
            txtNombre.BackColor = Color.FromArgb(62, 62, 66);
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Font = new Font("Segoe UI", 10F);
            txtNombre.ForeColor = Color.White;
            txtNombre.Location = new Point(20, 60);
            txtNombre.MaxLength = 255;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(460, 30);
            txtNombre.TabIndex = 1;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI", 10F);
            lblNombre.ForeColor = Color.White;
            lblNombre.Location = new Point(20, 30);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(171, 23);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre del Proyecto";
            // 
            // panelBotones
            // 
            panelBotones.BackColor = Color.FromArgb(45, 45, 48);
            panelBotones.Controls.Add(btnCancelar);
            panelBotones.Controls.Add(btnGuardar);
            panelBotones.Dock = DockStyle.Bottom;
            panelBotones.Location = new Point(0, 250);
            panelBotones.Name = "panelBotones";
            panelBotones.Padding = new Padding(20);
            panelBotones.Size = new Size(500, 80);
            panelBotones.TabIndex = 2;
            // 
            // btnCancelar
            // 
            btnCancelar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancelar.BackColor = Color.FromArgb(62, 62, 66);
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(250, 20);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 40);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGuardar.BackColor = Color.FromArgb(0, 122, 204);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(370, 20);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 40);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // FrmProyectoDetalle
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 330);
            Controls.Add(panelContenido);
            Controls.Add(panelBotones);
            Controls.Add(panelSuperior);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmProyectoDetalle";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Proyecto";
            Load += FrmProyectoDetalle_Load;
            panelSuperior.ResumeLayout(false);
            panelSuperior.PerformLayout();
            panelContenido.ResumeLayout(false);
            panelContenido.PerformLayout();
            panelBotones.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel panelSuperior;
        private Label lblTitulo;
        private Panel panelContenido;
        private Label lblNombre;
        private TextBox txtNombre;
        private CheckBox chkActivo;
        private Panel panelBotones;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
