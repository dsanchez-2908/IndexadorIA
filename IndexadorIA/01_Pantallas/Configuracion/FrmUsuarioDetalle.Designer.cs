namespace IndexadorIA.Pantallas.Configuracion
{
    partial class FrmUsuarioDetalle
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
            panelPrincipal = new Panel();
            panelFormulario = new Panel();
            txtClaveTemporal = new TextBox();
            lblClaveTemporal = new Label();
            cboRol = new ComboBox();
            lblRol = new Label();
            cboEstado = new ComboBox();
            lblEstado = new Label();
            btnCancelar = new Button();
            btnGuardar = new Button();
            txtNombreCompleto = new TextBox();
            lblNombreCompleto = new Label();
            txtUsuario = new TextBox();
            lblUsuario = new Label();
            lblTitulo = new Label();
            panelPrincipal.SuspendLayout();
            panelFormulario.SuspendLayout();
            SuspendLayout();
            // 
            // panelPrincipal
            // 
            panelPrincipal.BackColor = Color.FromArgb(32, 32, 32);
            panelPrincipal.Controls.Add(panelFormulario);
            panelPrincipal.Dock = DockStyle.Fill;
            panelPrincipal.Location = new Point(0, 0);
            panelPrincipal.Name = "panelPrincipal";
            panelPrincipal.Padding = new Padding(30);
            panelPrincipal.Size = new Size(500, 550);
            panelPrincipal.TabIndex = 0;
            // 
            // panelFormulario
            // 
            panelFormulario.BackColor = Color.FromArgb(45, 45, 48);
            panelFormulario.Controls.Add(txtClaveTemporal);
            panelFormulario.Controls.Add(lblClaveTemporal);
            panelFormulario.Controls.Add(cboRol);
            panelFormulario.Controls.Add(lblRol);
            panelFormulario.Controls.Add(cboEstado);
            panelFormulario.Controls.Add(lblEstado);
            panelFormulario.Controls.Add(btnCancelar);
            panelFormulario.Controls.Add(btnGuardar);
            panelFormulario.Controls.Add(txtNombreCompleto);
            panelFormulario.Controls.Add(lblNombreCompleto);
            panelFormulario.Controls.Add(txtUsuario);
            panelFormulario.Controls.Add(lblUsuario);
            panelFormulario.Controls.Add(lblTitulo);
            panelFormulario.Dock = DockStyle.Fill;
            panelFormulario.Location = new Point(30, 30);
            panelFormulario.Name = "panelFormulario";
            panelFormulario.Padding = new Padding(30);
            panelFormulario.Size = new Size(440, 490);
            panelFormulario.TabIndex = 0;
            // 
            // txtClaveTemporal
            // 
            txtClaveTemporal.BackColor = Color.FromArgb(62, 62, 66);
            txtClaveTemporal.BorderStyle = BorderStyle.None;
            txtClaveTemporal.Font = new Font("Segoe UI", 11F);
            txtClaveTemporal.ForeColor = Color.White;
            txtClaveTemporal.Location = new Point(30, 225);
            txtClaveTemporal.Name = "txtClaveTemporal";
            txtClaveTemporal.Size = new Size(380, 25);
            txtClaveTemporal.TabIndex = 3;
            // 
            // lblClaveTemporal
            // 
            lblClaveTemporal.AutoSize = true;
            lblClaveTemporal.Font = new Font("Segoe UI", 9F);
            lblClaveTemporal.ForeColor = Color.White;
            lblClaveTemporal.Location = new Point(30, 200);
            lblClaveTemporal.Name = "lblClaveTemporal";
            lblClaveTemporal.Size = new Size(113, 20);
            lblClaveTemporal.TabIndex = 11;
            lblClaveTemporal.Text = "Clave Temporal";
            // 
            // cboRol
            // 
            cboRol.BackColor = Color.FromArgb(62, 62, 66);
            cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRol.FlatStyle = FlatStyle.Flat;
            cboRol.Font = new Font("Segoe UI", 11F);
            cboRol.ForeColor = Color.White;
            cboRol.FormattingEnabled = true;
            cboRol.Location = new Point(30, 285);
            cboRol.Name = "cboRol";
            cboRol.Size = new Size(380, 33);
            cboRol.TabIndex = 4;
            // 
            // lblRol
            // 
            lblRol.AutoSize = true;
            lblRol.Font = new Font("Segoe UI", 9F);
            lblRol.ForeColor = Color.White;
            lblRol.Location = new Point(30, 260);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(31, 20);
            lblRol.TabIndex = 9;
            lblRol.Text = "Rol";
            // 
            // cboEstado
            // 
            cboEstado.BackColor = Color.FromArgb(62, 62, 66);
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.FlatStyle = FlatStyle.Flat;
            cboEstado.Font = new Font("Segoe UI", 11F);
            cboEstado.ForeColor = Color.White;
            cboEstado.FormattingEnabled = true;
            cboEstado.Location = new Point(30, 345);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(380, 33);
            cboEstado.TabIndex = 5;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI", 9F);
            lblEstado.ForeColor = Color.White;
            lblEstado.Location = new Point(30, 320);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(54, 20);
            lblEstado.TabIndex = 7;
            lblEstado.Text = "Estado";
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(62, 62, 66);
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(220, 400);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(180, 40);
            btnCancelar.TabIndex = 7;
            btnCancelar.Text = "CANCELAR";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(0, 122, 204);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(30, 400);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(180, 40);
            btnGuardar.TabIndex = 6;
            btnGuardar.Text = "GUARDAR";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // txtNombreCompleto
            // 
            txtNombreCompleto.BackColor = Color.FromArgb(62, 62, 66);
            txtNombreCompleto.BorderStyle = BorderStyle.None;
            txtNombreCompleto.Font = new Font("Segoe UI", 11F);
            txtNombreCompleto.ForeColor = Color.White;
            txtNombreCompleto.Location = new Point(30, 165);
            txtNombreCompleto.Name = "txtNombreCompleto";
            txtNombreCompleto.Size = new Size(380, 25);
            txtNombreCompleto.TabIndex = 2;
            // 
            // lblNombreCompleto
            // 
            lblNombreCompleto.AutoSize = true;
            lblNombreCompleto.Font = new Font("Segoe UI", 9F);
            lblNombreCompleto.ForeColor = Color.White;
            lblNombreCompleto.Location = new Point(30, 140);
            lblNombreCompleto.Name = "lblNombreCompleto";
            lblNombreCompleto.Size = new Size(134, 20);
            lblNombreCompleto.TabIndex = 3;
            lblNombreCompleto.Text = "Nombre Completo";
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = Color.FromArgb(62, 62, 66);
            txtUsuario.BorderStyle = BorderStyle.None;
            txtUsuario.Font = new Font("Segoe UI", 11F);
            txtUsuario.ForeColor = Color.White;
            txtUsuario.Location = new Point(30, 105);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(380, 25);
            txtUsuario.TabIndex = 1;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Segoe UI", 9F);
            lblUsuario.ForeColor = Color.White;
            lblUsuario.Location = new Point(30, 80);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(59, 20);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Usuario";
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(30, 30);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(380, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Nuevo Usuario";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmUsuarioDetalle
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 550);
            Controls.Add(panelPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmUsuarioDetalle";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Usuario";
            Load += FrmUsuarioDetalle_Load;
            panelPrincipal.ResumeLayout(false);
            panelFormulario.ResumeLayout(false);
            panelFormulario.PerformLayout();
            ResumeLayout(false);
        }

        private Panel panelPrincipal;
        private Panel panelFormulario;
        private Label lblTitulo;
        private TextBox txtUsuario;
        private Label lblUsuario;
        private TextBox txtNombreCompleto;
        private Label lblNombreCompleto;
        private Button btnGuardar;
        private Button btnCancelar;
        private ComboBox cboEstado;
        private Label lblEstado;
        private ComboBox cboRol;
        private Label lblRol;
        private TextBox txtClaveTemporal;
        private Label lblClaveTemporal;
    }
}
