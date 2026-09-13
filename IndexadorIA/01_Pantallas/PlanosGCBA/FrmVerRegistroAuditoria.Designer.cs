namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmVerRegistroAuditoria
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelIzquierdo = new Panel();
            groupBoxDatos = new GroupBox();
            lblCategoriaPlano = new Label();
            cboCategoriaPlano = new ComboBox();
            lblTipoPlano = new Label();
            cboTipoPlano = new ComboBox();
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
            lblEstado = new Label();
            cboEstado = new ComboBox();
            panelBotones = new Panel();
            btnCerrar = new Button();
            btnGuardar = new Button();
            panelDerecho = new Panel();
            panelImagenScroll = new Panel();
            pictureBoxImagen = new PictureBox();
            panelZoom = new Panel();
            lblSnGirada = new Label();
            lblZoom = new Label();
            btnZoomAjustar = new Button();
            btnZoomMenos = new Button();
            btnZoomMas = new Button();
            btnVerImagenPDF = new Button();
            btnGirarIzquierda = new Button();
            btnGirarDerecha = new Button();
            panelIzquierdo.SuspendLayout();
            groupBoxDatos.SuspendLayout();
            panelBotones.SuspendLayout();
            panelDerecho.SuspendLayout();
            panelImagenScroll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImagen).BeginInit();
            panelZoom.SuspendLayout();
            SuspendLayout();
            // 
            // panelIzquierdo
            // 
            panelIzquierdo.BackColor = Color.FromArgb(45, 45, 48);
            panelIzquierdo.Controls.Add(groupBoxDatos);
            panelIzquierdo.Controls.Add(panelBotones);
            panelIzquierdo.Dock = DockStyle.Left;
            panelIzquierdo.Location = new Point(0, 0);
            panelIzquierdo.Margin = new Padding(3, 2, 3, 2);
            panelIzquierdo.Name = "panelIzquierdo";
            panelIzquierdo.Size = new Size(430, 690);
            panelIzquierdo.TabIndex = 0;
            // 
            // groupBoxDatos
            // 
            groupBoxDatos.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxDatos.Controls.Add(lblCategoriaPlano);
            groupBoxDatos.Controls.Add(cboCategoriaPlano);
            groupBoxDatos.Controls.Add(lblTipoPlano);
            groupBoxDatos.Controls.Add(cboTipoPlano);
            groupBoxDatos.Controls.Add(lblExpediente);
            groupBoxDatos.Controls.Add(txtExpediente);
            groupBoxDatos.Controls.Add(lblSeccion);
            groupBoxDatos.Controls.Add(txtSeccion);
            groupBoxDatos.Controls.Add(lblManzana);
            groupBoxDatos.Controls.Add(txtManzana);
            groupBoxDatos.Controls.Add(lblParcela);
            groupBoxDatos.Controls.Add(txtParcela);
            groupBoxDatos.Controls.Add(lblDireccion);
            groupBoxDatos.Controls.Add(txtDireccion);
            groupBoxDatos.Controls.Add(lblNumeroPlano);
            groupBoxDatos.Controls.Add(txtNumeroPlano);
            groupBoxDatos.Controls.Add(lblObservaciones);
            groupBoxDatos.Controls.Add(txtObservaciones);
            groupBoxDatos.Controls.Add(lblEstado);
            groupBoxDatos.Controls.Add(cboEstado);
            groupBoxDatos.Dock = DockStyle.Fill;
            groupBoxDatos.ForeColor = Color.White;
            groupBoxDatos.Location = new Point(0, 0);
            groupBoxDatos.Margin = new Padding(3, 2, 3, 2);
            groupBoxDatos.Name = "groupBoxDatos";
            groupBoxDatos.Padding = new Padding(3, 2, 3, 2);
            groupBoxDatos.Size = new Size(430, 644);
            groupBoxDatos.TabIndex = 0;
            groupBoxDatos.TabStop = false;
            groupBoxDatos.Text = "Datos del Registro";
            // 
            // lblCategoriaPlano
            // 
            lblCategoriaPlano.AutoSize = true;
            lblCategoriaPlano.ForeColor = Color.White;
            lblCategoriaPlano.Location = new Point(18, 30);
            lblCategoriaPlano.Name = "lblCategoriaPlano";
            lblCategoriaPlano.Size = new Size(93, 15);
            lblCategoriaPlano.TabIndex = 0;
            lblCategoriaPlano.Text = "Categoría de Plano";
            // 
            // cboCategoriaPlano
            // 
            cboCategoriaPlano.BackColor = Color.FromArgb(30, 30, 30);
            cboCategoriaPlano.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoriaPlano.FlatStyle = FlatStyle.Flat;
            cboCategoriaPlano.ForeColor = Color.White;
            cboCategoriaPlano.Location = new Point(18, 48);
            cboCategoriaPlano.Margin = new Padding(3, 2, 3, 2);
            cboCategoriaPlano.Name = "cboCategoriaPlano";
            cboCategoriaPlano.Size = new Size(380, 23);
            cboCategoriaPlano.TabIndex = 1;
            cboCategoriaPlano.SelectedIndexChanged += cboCategoriaPlano_SelectedIndexChanged;
            // 
            // lblTipoPlano
            // 
            lblTipoPlano.AutoSize = true;
            lblTipoPlano.ForeColor = Color.White;
            lblTipoPlano.Location = new Point(18, 80);
            lblTipoPlano.Name = "lblTipoPlano";
            lblTipoPlano.Size = new Size(63, 15);
            lblTipoPlano.TabIndex = 2;
            lblTipoPlano.Text = "Tipo Plano";
            // 
            // cboTipoPlano
            // 
            cboTipoPlano.BackColor = Color.FromArgb(30, 30, 30);
            cboTipoPlano.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoPlano.FlatStyle = FlatStyle.Flat;
            cboTipoPlano.ForeColor = Color.White;
            cboTipoPlano.Location = new Point(18, 98);
            cboTipoPlano.Margin = new Padding(3, 2, 3, 2);
            cboTipoPlano.Name = "cboTipoPlano";
            cboTipoPlano.Size = new Size(380, 23);
            cboTipoPlano.TabIndex = 3;
            // 
            // lblExpediente
            // 
            lblExpediente.AutoSize = true;
            lblExpediente.ForeColor = Color.White;
            lblExpediente.Location = new Point(18, 130);
            lblExpediente.Name = "lblExpediente";
            lblExpediente.Size = new Size(64, 15);
            lblExpediente.TabIndex = 4;
            lblExpediente.Text = "Expediente";
            // 
            // txtExpediente
            // 
            txtExpediente.BackColor = Color.FromArgb(30, 30, 30);
            txtExpediente.BorderStyle = BorderStyle.FixedSingle;
            txtExpediente.ForeColor = Color.White;
            txtExpediente.Location = new Point(18, 148);
            txtExpediente.Margin = new Padding(3, 2, 3, 2);
            txtExpediente.Name = "txtExpediente";
            txtExpediente.Size = new Size(380, 23);
            txtExpediente.TabIndex = 5;
            // 
            // lblSeccion
            // 
            lblSeccion.AutoSize = true;
            lblSeccion.ForeColor = Color.White;
            lblSeccion.Location = new Point(18, 180);
            lblSeccion.Name = "lblSeccion";
            lblSeccion.Size = new Size(48, 15);
            lblSeccion.TabIndex = 6;
            lblSeccion.Text = "Sección";
            // 
            // txtSeccion
            // 
            txtSeccion.BackColor = Color.FromArgb(30, 30, 30);
            txtSeccion.BorderStyle = BorderStyle.FixedSingle;
            txtSeccion.ForeColor = Color.White;
            txtSeccion.Location = new Point(18, 198);
            txtSeccion.Margin = new Padding(3, 2, 3, 2);
            txtSeccion.Name = "txtSeccion";
            txtSeccion.Size = new Size(115, 23);
            txtSeccion.TabIndex = 7;
            // 
            // lblManzana
            // 
            lblManzana.AutoSize = true;
            lblManzana.ForeColor = Color.White;
            lblManzana.Location = new Point(153, 180);
            lblManzana.Name = "lblManzana";
            lblManzana.Size = new Size(55, 15);
            lblManzana.TabIndex = 8;
            lblManzana.Text = "Manzana";
            // 
            // txtManzana
            // 
            txtManzana.BackColor = Color.FromArgb(30, 30, 30);
            txtManzana.BorderStyle = BorderStyle.FixedSingle;
            txtManzana.ForeColor = Color.White;
            txtManzana.Location = new Point(153, 198);
            txtManzana.Margin = new Padding(3, 2, 3, 2);
            txtManzana.Name = "txtManzana";
            txtManzana.Size = new Size(115, 23);
            txtManzana.TabIndex = 9;
            // 
            // lblParcela
            // 
            lblParcela.AutoSize = true;
            lblParcela.ForeColor = Color.White;
            lblParcela.Location = new Point(283, 180);
            lblParcela.Name = "lblParcela";
            lblParcela.Size = new Size(46, 15);
            lblParcela.TabIndex = 10;
            lblParcela.Text = "Parcela";
            // 
            // txtParcela
            // 
            txtParcela.BackColor = Color.FromArgb(30, 30, 30);
            txtParcela.BorderStyle = BorderStyle.FixedSingle;
            txtParcela.ForeColor = Color.White;
            txtParcela.Location = new Point(283, 198);
            txtParcela.Margin = new Padding(3, 2, 3, 2);
            txtParcela.Name = "txtParcela";
            txtParcela.Size = new Size(115, 23);
            txtParcela.TabIndex = 11;
            // 
            // lblDireccion
            // 
            lblDireccion.AutoSize = true;
            lblDireccion.ForeColor = Color.White;
            lblDireccion.Location = new Point(18, 230);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new Size(58, 15);
            lblDireccion.TabIndex = 12;
            lblDireccion.Text = "Dirección";
            // 
            // txtDireccion
            // 
            txtDireccion.BackColor = Color.FromArgb(30, 30, 30);
            txtDireccion.BorderStyle = BorderStyle.FixedSingle;
            txtDireccion.ForeColor = Color.White;
            txtDireccion.Location = new Point(18, 248);
            txtDireccion.Margin = new Padding(3, 2, 3, 2);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(380, 23);
            txtDireccion.TabIndex = 13;
            // 
            // lblNumeroPlano
            // 
            lblNumeroPlano.AutoSize = true;
            lblNumeroPlano.ForeColor = Color.White;
            lblNumeroPlano.Location = new Point(18, 280);
            lblNumeroPlano.Name = "lblNumeroPlano";
            lblNumeroPlano.Size = new Size(88, 15);
            lblNumeroPlano.TabIndex = 14;
            lblNumeroPlano.Text = "Número de Plano";
            // 
            // txtNumeroPlano
            // 
            txtNumeroPlano.BackColor = Color.FromArgb(30, 30, 30);
            txtNumeroPlano.BorderStyle = BorderStyle.FixedSingle;
            txtNumeroPlano.ForeColor = Color.White;
            txtNumeroPlano.Location = new Point(18, 298);
            txtNumeroPlano.Margin = new Padding(3, 2, 3, 2);
            txtNumeroPlano.Name = "txtNumeroPlano";
            txtNumeroPlano.Size = new Size(380, 23);
            txtNumeroPlano.TabIndex = 15;
            // 
            // lblObservaciones
            // 
            lblObservaciones.AutoSize = true;
            lblObservaciones.ForeColor = Color.White;
            lblObservaciones.Location = new Point(18, 330);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(88, 15);
            lblObservaciones.TabIndex = 16;
            lblObservaciones.Text = "Observaciones";
            // 
            // txtObservaciones
            // 
            txtObservaciones.BackColor = Color.FromArgb(30, 30, 30);
            txtObservaciones.BorderStyle = BorderStyle.FixedSingle;
            txtObservaciones.ForeColor = Color.White;
            txtObservaciones.Location = new Point(18, 348);
            txtObservaciones.Margin = new Padding(3, 2, 3, 2);
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.Size = new Size(380, 90);
            txtObservaciones.TabIndex = 17;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.ForeColor = Color.White;
            lblEstado.Location = new Point(18, 450);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(41, 15);
            lblEstado.TabIndex = 18;
            lblEstado.Text = "Estado";
            // 
            // cboEstado
            // 
            cboEstado.BackColor = Color.FromArgb(30, 30, 30);
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.FlatStyle = FlatStyle.Flat;
            cboEstado.ForeColor = Color.White;
            cboEstado.Location = new Point(18, 468);
            cboEstado.Margin = new Padding(3, 2, 3, 2);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(380, 23);
            cboEstado.TabIndex = 19;
            // 
            // panelBotones
            // 
            panelBotones.BackColor = Color.FromArgb(45, 45, 48);
            panelBotones.Controls.Add(btnGuardar);
            panelBotones.Controls.Add(btnCerrar);
            panelBotones.Dock = DockStyle.Bottom;
            panelBotones.Location = new Point(0, 644);
            panelBotones.Margin = new Padding(3, 2, 3, 2);
            panelBotones.Name = "panelBotones";
            panelBotones.Size = new Size(430, 46);
            panelBotones.TabIndex = 1;
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.FromArgb(150, 40, 40);
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(290, 8);
            btnCerrar.Margin = new Padding(3, 2, 3, 2);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(120, 30);
            btnCerrar.TabIndex = 1;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(0, 150, 80);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(150, 8);
            btnGuardar.Margin = new Padding(3, 2, 3, 2);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 30);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // panelDerecho
            // 
            panelDerecho.BackColor = Color.FromArgb(30, 30, 30);
            panelDerecho.Controls.Add(panelImagenScroll);
            panelDerecho.Controls.Add(panelZoom);
            panelDerecho.Dock = DockStyle.Fill;
            panelDerecho.Location = new Point(430, 0);
            panelDerecho.Margin = new Padding(3, 2, 3, 2);
            panelDerecho.Name = "panelDerecho";
            panelDerecho.Size = new Size(670, 690);
            panelDerecho.TabIndex = 1;
            // 
            // panelImagenScroll
            // 
            panelImagenScroll.AutoScroll = true;
            panelImagenScroll.BackColor = Color.FromArgb(20, 20, 20);
            panelImagenScroll.Controls.Add(pictureBoxImagen);
            panelImagenScroll.Dock = DockStyle.Fill;
            panelImagenScroll.Location = new Point(0, 40);
            panelImagenScroll.Margin = new Padding(3, 2, 3, 2);
            panelImagenScroll.Name = "panelImagenScroll";
            panelImagenScroll.Size = new Size(670, 650);
            panelImagenScroll.TabIndex = 1;
            // 
            // pictureBoxImagen
            // 
            pictureBoxImagen.BackColor = Color.FromArgb(20, 20, 20);
            pictureBoxImagen.Location = new Point(0, 0);
            pictureBoxImagen.Margin = new Padding(3, 2, 3, 2);
            pictureBoxImagen.Name = "pictureBoxImagen";
            pictureBoxImagen.Size = new Size(665, 646);
            pictureBoxImagen.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImagen.TabIndex = 0;
            pictureBoxImagen.TabStop = false;
            // 
            // panelZoom
            // 
            panelZoom.BackColor = Color.FromArgb(45, 45, 48);
            panelZoom.Controls.Add(lblSnGirada);
            panelZoom.Controls.Add(lblZoom);
            panelZoom.Controls.Add(btnZoomAjustar);
            panelZoom.Controls.Add(btnZoomMenos);
            panelZoom.Controls.Add(btnZoomMas);
            panelZoom.Controls.Add(btnVerImagenPDF);
            panelZoom.Controls.Add(btnGirarIzquierda);
            panelZoom.Controls.Add(btnGirarDerecha);
            panelZoom.Dock = DockStyle.Top;
            panelZoom.Location = new Point(0, 0);
            panelZoom.Margin = new Padding(3, 2, 3, 2);
            panelZoom.Name = "panelZoom";
            panelZoom.Size = new Size(670, 40);
            panelZoom.TabIndex = 0;
            // 
            // lblSnGirada
            // 
            lblSnGirada.AutoSize = true;
            lblSnGirada.ForeColor = Color.Gainsboro;
            lblSnGirada.Location = new Point(590, 12);
            lblSnGirada.Name = "lblSnGirada";
            lblSnGirada.Size = new Size(52, 15);
            lblSnGirada.TabIndex = 7;
            lblSnGirada.Text = "Girada: -";
            // 
            // lblZoom
            // 
            lblZoom.AutoSize = true;
            lblZoom.ForeColor = Color.White;
            lblZoom.Location = new Point(196, 12);
            lblZoom.Name = "lblZoom";
            lblZoom.Size = new Size(38, 15);
            lblZoom.TabIndex = 3;
            lblZoom.Text = "100 %";
            // 
            // btnZoomAjustar
            // 
            btnZoomAjustar.BackColor = Color.FromArgb(70, 70, 70);
            btnZoomAjustar.FlatStyle = FlatStyle.Flat;
            btnZoomAjustar.ForeColor = Color.White;
            btnZoomAjustar.Location = new Point(96, 5);
            btnZoomAjustar.Margin = new Padding(3, 2, 3, 2);
            btnZoomAjustar.Name = "btnZoomAjustar";
            btnZoomAjustar.Size = new Size(90, 30);
            btnZoomAjustar.TabIndex = 2;
            btnZoomAjustar.Text = "Ajustar";
            btnZoomAjustar.UseVisualStyleBackColor = false;
            btnZoomAjustar.Click += btnZoomAjustar_Click;
            // 
            // btnZoomMenos
            // 
            btnZoomMenos.BackColor = Color.FromArgb(70, 70, 70);
            btnZoomMenos.FlatStyle = FlatStyle.Flat;
            btnZoomMenos.ForeColor = Color.White;
            btnZoomMenos.Location = new Point(54, 5);
            btnZoomMenos.Margin = new Padding(3, 2, 3, 2);
            btnZoomMenos.Name = "btnZoomMenos";
            btnZoomMenos.Size = new Size(36, 30);
            btnZoomMenos.TabIndex = 1;
            btnZoomMenos.Text = "-";
            btnZoomMenos.UseVisualStyleBackColor = false;
            btnZoomMenos.Click += btnZoomMenos_Click;
            // 
            // btnZoomMas
            // 
            btnZoomMas.BackColor = Color.FromArgb(70, 70, 70);
            btnZoomMas.FlatStyle = FlatStyle.Flat;
            btnZoomMas.ForeColor = Color.White;
            btnZoomMas.Location = new Point(12, 5);
            btnZoomMas.Margin = new Padding(3, 2, 3, 2);
            btnZoomMas.Name = "btnZoomMas";
            btnZoomMas.Size = new Size(36, 30);
            btnZoomMas.TabIndex = 0;
            btnZoomMas.Text = "+";
            btnZoomMas.UseVisualStyleBackColor = false;
            btnZoomMas.Click += btnZoomMas_Click;
            // 
            // btnVerImagenPDF
            // 
            btnVerImagenPDF.BackColor = Color.FromArgb(70, 70, 70);
            btnVerImagenPDF.FlatStyle = FlatStyle.Flat;
            btnVerImagenPDF.ForeColor = Color.White;
            btnVerImagenPDF.Location = new Point(252, 5);
            btnVerImagenPDF.Margin = new Padding(3, 2, 8, 2);
            btnVerImagenPDF.Name = "btnVerImagenPDF";
            btnVerImagenPDF.Size = new Size(120, 30);
            btnVerImagenPDF.TabIndex = 4;
            btnVerImagenPDF.Text = "Ver imagen PDF";
            btnVerImagenPDF.UseVisualStyleBackColor = false;
            btnVerImagenPDF.Click += btnVerImagenPDF_Click;
            // 
            // btnGirarIzquierda
            // 
            btnGirarIzquierda.BackColor = Color.FromArgb(70, 70, 70);
            btnGirarIzquierda.FlatStyle = FlatStyle.Flat;
            btnGirarIzquierda.ForeColor = Color.White;
            btnGirarIzquierda.Location = new Point(378, 5);
            btnGirarIzquierda.Margin = new Padding(3, 2, 8, 2);
            btnGirarIzquierda.Name = "btnGirarIzquierda";
            btnGirarIzquierda.Size = new Size(36, 30);
            btnGirarIzquierda.TabIndex = 5;
            btnGirarIzquierda.Text = "⟲";
            btnGirarIzquierda.UseVisualStyleBackColor = false;
            btnGirarIzquierda.Click += btnGirarIzquierda_Click;
            // 
            // btnGirarDerecha
            // 
            btnGirarDerecha.BackColor = Color.FromArgb(70, 70, 70);
            btnGirarDerecha.FlatStyle = FlatStyle.Flat;
            btnGirarDerecha.ForeColor = Color.White;
            btnGirarDerecha.Location = new Point(420, 5);
            btnGirarDerecha.Margin = new Padding(3, 2, 8, 2);
            btnGirarDerecha.Name = "btnGirarDerecha";
            btnGirarDerecha.Size = new Size(36, 30);
            btnGirarDerecha.TabIndex = 6;
            btnGirarDerecha.Text = "⟳";
            btnGirarDerecha.UseVisualStyleBackColor = false;
            btnGirarDerecha.Click += btnGirarDerecha_Click;
            // 
            // FrmVerRegistroAuditoria
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1100, 690);
            Controls.Add(panelDerecho);
            Controls.Add(panelIzquierdo);
            ForeColor = Color.White;
            Margin = new Padding(3, 2, 3, 2);
            MinimumSize = new Size(900, 500);
            Name = "FrmVerRegistroAuditoria";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ver Registro de Auditoría";
            WindowState = FormWindowState.Maximized;
            Load += FrmVerRegistroAuditoria_Load;
            panelIzquierdo.ResumeLayout(false);
            groupBoxDatos.ResumeLayout(false);
            groupBoxDatos.PerformLayout();
            panelBotones.ResumeLayout(false);
            panelDerecho.ResumeLayout(false);
            panelImagenScroll.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxImagen).EndInit();
            panelZoom.ResumeLayout(false);
            panelZoom.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelIzquierdo;
        private GroupBox groupBoxDatos;
        private Label lblCategoriaPlano;
        private ComboBox cboCategoriaPlano;
        private Label lblTipoPlano;
        private ComboBox cboTipoPlano;
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
        private Label lblEstado;
        private ComboBox cboEstado;
        private Panel panelBotones;
        private Button btnCerrar;
        private Button btnGuardar;
        private Panel panelDerecho;
        private Panel panelImagenScroll;
        private PictureBox pictureBoxImagen;
        private Panel panelZoom;
        private Label lblSnGirada;
        private Label lblZoom;
        private Button btnZoomAjustar;
        private Button btnZoomMenos;
        private Button btnZoomMas;
        private Button btnVerImagenPDF;
        private Button btnGirarIzquierda;
        private Button btnGirarDerecha;
    }
}
