namespace IndexadorIA.Pantallas
{
    partial class FrmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelPrincipal = new Panel();
            panelFormulario = new Panel();
            btnIngresar = new Button();
            txtClave = new TextBox();
            lblClave = new Label();
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
            panelPrincipal.Padding = new Padding(50);
            panelPrincipal.Size = new Size(500, 400);
            panelPrincipal.TabIndex = 0;
            // 
            // panelFormulario
            // 
            panelFormulario.BackColor = Color.FromArgb(45, 45, 48);
            panelFormulario.Controls.Add(btnIngresar);
            panelFormulario.Controls.Add(txtClave);
            panelFormulario.Controls.Add(lblClave);
            panelFormulario.Controls.Add(txtUsuario);
            panelFormulario.Controls.Add(lblUsuario);
            panelFormulario.Controls.Add(lblTitulo);
            panelFormulario.Dock = DockStyle.Fill;
            panelFormulario.Location = new Point(50, 50);
            panelFormulario.Name = "panelFormulario";
            panelFormulario.Padding = new Padding(40);
            panelFormulario.Size = new Size(400, 300);
            panelFormulario.TabIndex = 0;
            // 
            // btnIngresar
            // 
            btnIngresar.BackColor = Color.FromArgb(0, 122, 204);
            btnIngresar.Cursor = Cursors.Hand;
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnIngresar.ForeColor = Color.White;
            btnIngresar.Location = new Point(40, 230);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(320, 40);
            btnIngresar.TabIndex = 3;
            btnIngresar.Text = "INGRESAR";
            btnIngresar.UseVisualStyleBackColor = false;
            btnIngresar.Click += btnIngresar_Click;
            // 
            // txtClave
            // 
            txtClave.BackColor = Color.FromArgb(62, 62, 66);
            txtClave.BorderStyle = BorderStyle.None;
            txtClave.Font = new Font("Segoe UI", 12F);
            txtClave.ForeColor = Color.White;
            txtClave.Location = new Point(40, 175);
            txtClave.Name = "txtClave";
            txtClave.PasswordChar = '●';
            txtClave.Size = new Size(320, 27);
            txtClave.TabIndex = 2;
            txtClave.KeyPress += txtClave_KeyPress;
            // 
            // lblClave
            // 
            lblClave.AutoSize = true;
            lblClave.Font = new Font("Segoe UI", 10F);
            lblClave.ForeColor = Color.White;
            lblClave.Location = new Point(40, 145);
            lblClave.Name = "lblClave";
            lblClave.Size = new Size(95, 23);
            lblClave.TabIndex = 3;
            lblClave.Text = "Contraseña";
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = Color.FromArgb(62, 62, 66);
            txtUsuario.BorderStyle = BorderStyle.None;
            txtUsuario.Font = new Font("Segoe UI", 12F);
            txtUsuario.ForeColor = Color.White;
            txtUsuario.Location = new Point(40, 105);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(320, 27);
            txtUsuario.TabIndex = 1;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Segoe UI", 10F);
            lblUsuario.ForeColor = Color.White;
            lblUsuario.Location = new Point(40, 75);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(67, 23);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Usuario";
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(40, 40);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(320, 35);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "IndexadorIA";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 400);
            Controls.Add(panelPrincipal);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "IndexadorIA - Login";
            panelPrincipal.ResumeLayout(false);
            panelFormulario.ResumeLayout(false);
            panelFormulario.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelPrincipal;
        private Panel panelFormulario;
        private Label lblTitulo;
        private TextBox txtUsuario;
        private Label lblUsuario;
        private TextBox txtClave;
        private Label lblClave;
        private Button btnIngresar;
    }
}
