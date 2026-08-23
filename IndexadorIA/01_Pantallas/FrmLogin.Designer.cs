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
            lblBaseDatos = new Label();
            chkIngresoRemoto = new CheckBox();
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
            panelPrincipal.Margin = new Padding(3, 2, 3, 2);
            panelPrincipal.Name = "panelPrincipal";
            panelPrincipal.Padding = new Padding(44, 38, 44, 38);
            panelPrincipal.Size = new Size(438, 330);
            panelPrincipal.TabIndex = 0;
            // 
            // panelFormulario
            // 
            panelFormulario.BackColor = Color.FromArgb(45, 45, 48);
            panelFormulario.Controls.Add(lblBaseDatos);
            panelFormulario.Controls.Add(chkIngresoRemoto);
            panelFormulario.Controls.Add(btnIngresar);
            panelFormulario.Controls.Add(txtClave);
            panelFormulario.Controls.Add(lblClave);
            panelFormulario.Controls.Add(txtUsuario);
            panelFormulario.Controls.Add(lblUsuario);
            panelFormulario.Controls.Add(lblTitulo);
            panelFormulario.Dock = DockStyle.Fill;
            panelFormulario.Location = new Point(44, 38);
            panelFormulario.Margin = new Padding(3, 2, 3, 2);
            panelFormulario.Name = "panelFormulario";
            panelFormulario.Padding = new Padding(35, 30, 35, 30);
            panelFormulario.Size = new Size(350, 254);
            panelFormulario.TabIndex = 0;
            // 
            // lblBaseDatos
            // 
            lblBaseDatos.AutoSize = true;
            lblBaseDatos.Font = new Font("Segoe UI", 8F);
            lblBaseDatos.ForeColor = Color.Gainsboro;
            lblBaseDatos.Location = new Point(6, 229);
            lblBaseDatos.Name = "lblBaseDatos";
            lblBaseDatos.Size = new Size(85, 13);
            lblBaseDatos.TabIndex = 5;
            lblBaseDatos.Text = "Base de datos: ";
            // 
            // chkIngresoRemoto
            // 
            chkIngresoRemoto.AutoSize = true;
            chkIngresoRemoto.Cursor = Cursors.Hand;
            chkIngresoRemoto.Font = new Font("Segoe UI", 9F);
            chkIngresoRemoto.ForeColor = Color.White;
            chkIngresoRemoto.Location = new Point(35, 160);
            chkIngresoRemoto.Margin = new Padding(3, 2, 3, 2);
            chkIngresoRemoto.Name = "chkIngresoRemoto";
            chkIngresoRemoto.Size = new Size(136, 19);
            chkIngresoRemoto.TabIndex = 4;
            chkIngresoRemoto.Text = "Ingreso remoto (API)";
            chkIngresoRemoto.UseVisualStyleBackColor = true;
            // 
            // btnIngresar
            // 
            btnIngresar.BackColor = Color.FromArgb(0, 122, 204);
            btnIngresar.Cursor = Cursors.Hand;
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnIngresar.ForeColor = Color.White;
            btnIngresar.Location = new Point(35, 187);
            btnIngresar.Margin = new Padding(3, 2, 3, 2);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(280, 30);
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
            txtClave.Location = new Point(35, 131);
            txtClave.Margin = new Padding(3, 2, 3, 2);
            txtClave.Name = "txtClave";
            txtClave.PasswordChar = '●';
            txtClave.Size = new Size(280, 22);
            txtClave.TabIndex = 2;
            txtClave.KeyPress += txtClave_KeyPress;
            // 
            // lblClave
            // 
            lblClave.AutoSize = true;
            lblClave.Font = new Font("Segoe UI", 10F);
            lblClave.ForeColor = Color.White;
            lblClave.Location = new Point(35, 109);
            lblClave.Name = "lblClave";
            lblClave.Size = new Size(79, 19);
            lblClave.TabIndex = 3;
            lblClave.Text = "Contraseña";
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = Color.FromArgb(62, 62, 66);
            txtUsuario.BorderStyle = BorderStyle.None;
            txtUsuario.Font = new Font("Segoe UI", 12F);
            txtUsuario.ForeColor = Color.White;
            txtUsuario.Location = new Point(35, 79);
            txtUsuario.Margin = new Padding(3, 2, 3, 2);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(280, 22);
            txtUsuario.TabIndex = 1;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Segoe UI", 10F);
            lblUsuario.ForeColor = Color.White;
            lblUsuario.Location = new Point(35, 56);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(56, 19);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Usuario";
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(35, 30);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(280, 26);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "IndexadorIA";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(438, 330);
            Controls.Add(panelPrincipal);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 2, 3, 2);
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
        private CheckBox chkIngresoRemoto;
        private Label lblBaseDatos;
    }
}
