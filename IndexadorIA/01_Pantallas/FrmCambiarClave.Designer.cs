namespace IndexadorIA.Pantallas
{
    partial class FrmCambiarClave
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
            btnCancelar = new Button();
            btnGuardar = new Button();
            txtConfirmar = new TextBox();
            lblConfirmar = new Label();
            txtNueva = new TextBox();
            lblNueva = new Label();
            txtActual = new TextBox();
            lblActual = new Label();
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
            panelPrincipal.Size = new Size(450, 380);
            panelPrincipal.TabIndex = 0;
            // 
            // panelFormulario
            // 
            panelFormulario.BackColor = Color.FromArgb(45, 45, 48);
            panelFormulario.Controls.Add(btnCancelar);
            panelFormulario.Controls.Add(btnGuardar);
            panelFormulario.Controls.Add(txtConfirmar);
            panelFormulario.Controls.Add(lblConfirmar);
            panelFormulario.Controls.Add(txtNueva);
            panelFormulario.Controls.Add(lblNueva);
            panelFormulario.Controls.Add(txtActual);
            panelFormulario.Controls.Add(lblActual);
            panelFormulario.Controls.Add(lblTitulo);
            panelFormulario.Dock = DockStyle.Fill;
            panelFormulario.Location = new Point(30, 30);
            panelFormulario.Name = "panelFormulario";
            panelFormulario.Padding = new Padding(30);
            panelFormulario.Size = new Size(390, 320);
            panelFormulario.TabIndex = 0;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(62, 62, 66);
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            panelFormulario.Controls.Add(lblTitulo);
            btnCancelar.Location = new Point(200, 260);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(150, 35);
            btnCancelar.TabIndex = 5;
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
            btnGuardar.Location = new Point(40, 260);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(150, 35);
            btnGuardar.TabIndex = 4;
            btnGuardar.Text = "GUARDAR";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // txtConfirmar
            // 
            txtConfirmar.BackColor = Color.FromArgb(62, 62, 66);
            txtConfirmar.BorderStyle = BorderStyle.None;
            txtConfirmar.Font = new Font("Segoe UI", 11F);
            txtConfirmar.ForeColor = Color.White;
            txtConfirmar.Location = new Point(40, 215);
            txtConfirmar.Name = "txtConfirmar";
            txtConfirmar.PasswordChar = '●';
            txtConfirmar.Size = new Size(310, 25);
            txtConfirmar.TabIndex = 3;
            // 
            // lblConfirmar
            // 
            lblConfirmar.AutoSize = true;
            lblConfirmar.Font = new Font("Segoe UI", 9F);
            lblConfirmar.ForeColor = Color.White;
            lblConfirmar.Location = new Point(40, 190);
            lblConfirmar.Name = "lblConfirmar";
            lblConfirmar.Size = new Size(152, 20);
            lblConfirmar.TabIndex = 6;
            lblConfirmar.Text = "Confirmar Contraseña";
            // 
            // txtNueva
            // 
            txtNueva.BackColor = Color.FromArgb(62, 62, 66);
            txtNueva.BorderStyle = BorderStyle.None;
            txtNueva.Font = new Font("Segoe UI", 11F);
            txtNueva.ForeColor = Color.White;
            txtNueva.Location = new Point(40, 155);
            txtNueva.Name = "txtNueva";
            txtNueva.PasswordChar = '●';
            txtNueva.Size = new Size(310, 25);
            txtNueva.TabIndex = 2;
            // 
            // lblNueva
            // 
            lblNueva.AutoSize = true;
            lblNueva.Font = new Font("Segoe UI", 9F);
            lblNueva.ForeColor = Color.White;
            lblNueva.Location = new Point(40, 130);
            lblNueva.Name = "lblNueva";
            lblNueva.Size = new Size(131, 20);
            lblNueva.TabIndex = 4;
            lblNueva.Text = "Nueva Contraseña";
            // 
            // txtActual
            // 
            txtActual.BackColor = Color.FromArgb(62, 62, 66);
            txtActual.BorderStyle = BorderStyle.None;
            txtActual.Font = new Font("Segoe UI", 11F);
            txtActual.ForeColor = Color.White;
            txtActual.Location = new Point(40, 95);
            txtActual.Name = "txtActual";
            txtActual.PasswordChar = '●';
            txtActual.Size = new Size(310, 25);
            txtActual.TabIndex = 1;
            // 
            // lblActual
            // 
            lblActual.AutoSize = true;
            lblActual.Font = new Font("Segoe UI", 9F);
            lblActual.ForeColor = Color.White;
            lblActual.Location = new Point(40, 70);
            lblActual.Name = "lblActual";
            lblActual.Size = new Size(132, 20);
            lblActual.TabIndex = 2;
            lblActual.Text = "Contraseña Actual";
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(30, 30);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(330, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Cambiar Contraseña";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmCambiarClave
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 380);
            Controls.Add(panelPrincipal);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCambiarClave";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cambiar Contraseña";
            panelPrincipal.ResumeLayout(false);
            panelFormulario.ResumeLayout(false);
            panelFormulario.PerformLayout();
            ResumeLayout(false);
        }

        private Panel panelPrincipal;
        private Panel panelFormulario;
        private Label lblTitulo;
        private TextBox txtActual;
        private Label lblActual;
        private TextBox txtNueva;
        private Label lblNueva;
        private TextBox txtConfirmar;
        private Label lblConfirmar;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
