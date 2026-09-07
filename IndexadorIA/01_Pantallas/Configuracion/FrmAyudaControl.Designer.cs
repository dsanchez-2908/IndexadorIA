namespace IndexadorIA.Pantallas.Configuracion
{
    partial class FrmAyudaControl
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
            tableLayoutCampos = new TableLayoutPanel();
            lblCategoriaPlano = new Label();
            txtCategoriaPlano = new TextBox();
            lblTipoPlano = new Label();
            txtTipoPlano = new TextBox();
            lblExpediente = new Label();
            txtExpediente = new TextBox();
            lblSeccion = new Label();
            txtSeccion = new TextBox();
            lblManzana = new Label();
            txtManzana = new TextBox();
            lblParcela = new Label();
            txtParcela = new TextBox();
            lblDireccion = new Label();
            txtDireccion = new TextBox();
            lblNumeroPlano = new Label();
            txtNumeroPlano = new TextBox();
            lblObservaciones = new Label();
            txtObservaciones = new TextBox();
            panelBotones = new Panel();
            btnGuardar = new Button();
            btnCerrar = new Button();
            lblTitulo = new Label();
            panelPrincipal.SuspendLayout();
            tableLayoutCampos.SuspendLayout();
            panelBotones.SuspendLayout();
            SuspendLayout();
            //
            // panelPrincipal
            //
            panelPrincipal.BackColor = Color.FromArgb(32, 32, 32);
            panelPrincipal.Controls.Add(tableLayoutCampos);
            panelPrincipal.Controls.Add(panelBotones);
            panelPrincipal.Controls.Add(lblTitulo);
            panelPrincipal.Dock = DockStyle.Fill;
            panelPrincipal.Location = new Point(0, 0);
            panelPrincipal.Name = "panelPrincipal";
            panelPrincipal.Padding = new Padding(20);
            panelPrincipal.Size = new Size(900, 700);
            panelPrincipal.TabIndex = 0;
            //
            // lblTitulo
            //
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(860, 40);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Ayuda Control";
            //
            // tableLayoutCampos
            //
            tableLayoutCampos.AutoScroll = true;
            tableLayoutCampos.ColumnCount = 2;
            tableLayoutCampos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tableLayoutCampos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutCampos.Dock = DockStyle.Fill;
            tableLayoutCampos.Location = new Point(20, 60);
            tableLayoutCampos.Name = "tableLayoutCampos";
            tableLayoutCampos.Padding = new Padding(0, 10, 0, 10);
            tableLayoutCampos.RowCount = 9;
            for (int i = 0; i < 9; i++)
                tableLayoutCampos.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutCampos.Size = new Size(860, 570);
            tableLayoutCampos.TabIndex = 1;

            ConfigurarFila(0, lblCategoriaPlano, "Categoría de Plano:", txtCategoriaPlano);
            ConfigurarFila(1, lblTipoPlano, "Tipo de Plano:", txtTipoPlano);
            ConfigurarFila(2, lblExpediente, "Expediente:", txtExpediente);
            ConfigurarFila(3, lblSeccion, "Sección:", txtSeccion);
            ConfigurarFila(4, lblManzana, "Manzana:", txtManzana);
            ConfigurarFila(5, lblParcela, "Parcela:", txtParcela);
            ConfigurarFila(6, lblDireccion, "Dirección:", txtDireccion);
            ConfigurarFila(7, lblNumeroPlano, "Número de Plano:", txtNumeroPlano);
            ConfigurarFila(8, lblObservaciones, "Observaciones:", txtObservaciones);
            //
            // panelBotones
            //
            panelBotones.Controls.Add(btnGuardar);
            panelBotones.Controls.Add(btnCerrar);
            panelBotones.Dock = DockStyle.Bottom;
            panelBotones.Location = new Point(20, 630);
            panelBotones.Name = "panelBotones";
            panelBotones.Size = new Size(860, 50);
            panelBotones.TabIndex = 2;
            //
            // btnGuardar
            //
            btnGuardar.BackColor = Color.FromArgb(0, 122, 204);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(600, 8);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 35);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCerrar
            //
            btnCerrar.BackColor = Color.FromArgb(60, 60, 63);
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(730, 8);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(120, 35);
            btnCerrar.TabIndex = 1;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmAyudaControl
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(900, 700);
            Controls.Add(panelPrincipal);
            MinimumSize = new Size(700, 500);
            Name = "FrmAyudaControl";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ayuda Control";
            Load += FrmAyudaControl_Load;
            panelPrincipal.ResumeLayout(false);
            tableLayoutCampos.ResumeLayout(false);
            tableLayoutCampos.PerformLayout();
            panelBotones.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void ConfigurarFila(int fila, Label label, string texto, TextBox textBox)
        {
            label.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            label.AutoSize = true;
            label.ForeColor = Color.White;
            label.Location = new Point(3, 3);
            label.Name = $"lbl{fila}";
            label.Text = texto;

            textBox.BackColor = Color.FromArgb(45, 45, 48);
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Dock = DockStyle.Fill;
            textBox.ForeColor = Color.White;
            textBox.Margin = new Padding(3, 3, 3, 10);
            textBox.Multiline = true;
            textBox.Name = $"txt{fila}";
            textBox.ScrollBars = ScrollBars.Vertical;

            tableLayoutCampos.Controls.Add(label, 0, fila);
            tableLayoutCampos.Controls.Add(textBox, 1, fila);
        }

        private Panel panelPrincipal;
        private TableLayoutPanel tableLayoutCampos;
        private Label lblTitulo;
        private Label lblCategoriaPlano;
        private TextBox txtCategoriaPlano;
        private Label lblTipoPlano;
        private TextBox txtTipoPlano;
        private Label lblExpediente;
        private TextBox txtExpediente;
        private Label lblSeccion;
        private TextBox txtSeccion;
        private Label lblManzana;
        private TextBox txtManzana;
        private Label lblParcela;
        private TextBox txtParcela;
        private Label lblDireccion;
        private TextBox txtDireccion;
        private Label lblNumeroPlano;
        private TextBox txtNumeroPlano;
        private Label lblObservaciones;
        private TextBox txtObservaciones;
        private Panel panelBotones;
        private Button btnGuardar;
        private Button btnCerrar;
    }
}
