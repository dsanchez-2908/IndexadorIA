namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmPreparacionImagenes
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
            splitContainer1 = new SplitContainer();
            groupBoxPrincipal = new GroupBox();
            panelFiltros = new Panel();
            btnLimpiar = new Button();
            btnBuscar = new Button();
            dtpHasta = new DateTimePicker();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            lblDesde = new Label();
            txtFiltroNombre = new TextBox();
            lblFiltroNombre = new Label();
            panelBotonesSeleccion = new Panel();
            btnSeleccionar50 = new Button();
            btnDeseleccionarTodo = new Button();
            btnSeleccionarTodo = new Button();
            panelGrilla = new Panel();
            dgvLotes = new DataGridView();
            lblInfoSeleccion = new Label();
            panelDerecha = new Panel();
            groupBoxPreview = new GroupBox();
            panelVisor = new Panel();
            pictureBoxPreview = new PictureBox();
            panelBotonesPreview = new Panel();
            btnActualizarImagen = new Button();
            btnCargarPDF = new Button();
            groupBoxConfiguracion = new GroupBox();
            chkEjecutarOCR = new CheckBox();
            numPorcentajeHorizontal = new NumericUpDown();
            lblPorcentajeHorizontal = new Label();
            numPorcentajeVertical = new NumericUpDown();
            lblPorcentajeVertical = new Label();
            numDPI = new NumericUpDown();
            lblDPI = new Label();
            cboEsquina = new ComboBox();
            lblEsquina = new Label();
            panelInferior = new Panel();
            progressBar = new ProgressBar();
            lblProgresoTexto = new Label();
            btnCerrar = new Button();
            btnProcesar = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBoxPrincipal.SuspendLayout();
            panelFiltros.SuspendLayout();
            panelBotonesSeleccion.SuspendLayout();
            panelGrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLotes).BeginInit();
            panelDerecha.SuspendLayout();
            groupBoxPreview.SuspendLayout();
            panelVisor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreview).BeginInit();
            panelBotonesPreview.SuspendLayout();
            groupBoxConfiguracion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPorcentajeHorizontal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPorcentajeVertical).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDPI).BeginInit();
            panelInferior.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(10, 10);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(groupBoxPrincipal);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panelDerecha);
            splitContainer1.Size = new Size(1180, 650);
            splitContainer1.SplitterDistance = 700;
            splitContainer1.TabIndex = 0;
            // 
            // groupBoxPrincipal
            // 
            groupBoxPrincipal.Controls.Add(panelGrilla);
            groupBoxPrincipal.Controls.Add(panelBotonesSeleccion);
            groupBoxPrincipal.Controls.Add(panelFiltros);
            groupBoxPrincipal.Dock = DockStyle.Fill;
            groupBoxPrincipal.ForeColor = Color.White;
            groupBoxPrincipal.Location = new Point(0, 0);
            groupBoxPrincipal.Name = "groupBoxPrincipal";
            groupBoxPrincipal.Padding = new Padding(10);
            groupBoxPrincipal.Size = new Size(700, 650);
            groupBoxPrincipal.TabIndex = 0;
            groupBoxPrincipal.TabStop = false;
            groupBoxPrincipal.Text = "Lotes Disponibles";
            // 
            // panelFiltros
            // 
            panelFiltros.Controls.Add(btnLimpiar);
            panelFiltros.Controls.Add(btnBuscar);
            panelFiltros.Controls.Add(dtpHasta);
            panelFiltros.Controls.Add(dtpDesde);
            panelFiltros.Controls.Add(lblHasta);
            panelFiltros.Controls.Add(lblDesde);
            panelFiltros.Controls.Add(txtFiltroNombre);
            panelFiltros.Controls.Add(lblFiltroNombre);
            panelFiltros.Dock = DockStyle.Top;
            panelFiltros.Location = new Point(10, 71);
            panelFiltros.Name = "panelFiltros";
            panelFiltros.Size = new Size(680, 90);
            panelFiltros.TabIndex = 0;
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.FromArgb(60, 60, 63);
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.ForeColor = Color.White;
            btnLimpiar.Location = new Point(560, 50);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(100, 30);
            btnLimpiar.TabIndex = 7;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = Color.FromArgb(0, 122, 204);
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(450, 50);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(100, 30);
            btnBuscar.TabIndex = 6;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            // 
            // dtpHasta
            // 
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(350, 52);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(90, 23);
            dtpHasta.TabIndex = 5;
            // 
            // dtpDesde
            // 
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(200, 52);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(90, 23);
            dtpDesde.TabIndex = 4;
            // 
            // lblHasta
            // 
            lblHasta.AutoSize = true;
            lblHasta.Location = new Point(305, 56);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(40, 15);
            lblHasta.TabIndex = 3;
            lblHasta.Text = "Hasta:";
            // 
            // lblDesde
            // 
            lblDesde.AutoSize = true;
            lblDesde.Location = new Point(10, 56);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(163, 15);
            lblDesde.TabIndex = 2;
            lblDesde.Text = "Fecha de alta (Desde - Hasta):";
            // 
            // txtFiltroNombre
            // 
            txtFiltroNombre.Location = new Point(110, 10);
            txtFiltroNombre.Name = "txtFiltroNombre";
            txtFiltroNombre.Size = new Size(550, 23);
            txtFiltroNombre.TabIndex = 1;
            // 
            // lblFiltroNombre
            // 
            lblFiltroNombre.AutoSize = true;
            lblFiltroNombre.Location = new Point(10, 13);
            lblFiltroNombre.Name = "lblFiltroNombre";
            lblFiltroNombre.Size = new Size(93, 15);
            lblFiltroNombre.TabIndex = 0;
            lblFiltroNombre.Text = "Nombre de lote:";
            // 
            // panelBotonesSeleccion
            // 
            panelBotonesSeleccion.Controls.Add(btnSeleccionar50);
            panelBotonesSeleccion.Controls.Add(btnDeseleccionarTodo);
            panelBotonesSeleccion.Controls.Add(btnSeleccionarTodo);
            panelBotonesSeleccion.Dock = DockStyle.Top;
            panelBotonesSeleccion.Location = new Point(10, 26);
            panelBotonesSeleccion.Name = "panelBotonesSeleccion";
            panelBotonesSeleccion.Size = new Size(680, 45);
            panelBotonesSeleccion.TabIndex = 1;
            // 
            // btnSeleccionar50
            // 
            btnSeleccionar50.BackColor = Color.FromArgb(45, 45, 48);
            btnSeleccionar50.FlatStyle = FlatStyle.Flat;
            btnSeleccionar50.ForeColor = Color.White;
            btnSeleccionar50.Location = new Point(290, 8);
            btnSeleccionar50.Name = "btnSeleccionar50";
            btnSeleccionar50.Size = new Size(130, 30);
            btnSeleccionar50.TabIndex = 2;
            btnSeleccionar50.Text = "Seleccionar 50";
            btnSeleccionar50.UseVisualStyleBackColor = false;
            // 
            // btnDeseleccionarTodo
            // 
            btnDeseleccionarTodo.BackColor = Color.FromArgb(45, 45, 48);
            btnDeseleccionarTodo.FlatStyle = FlatStyle.Flat;
            btnDeseleccionarTodo.ForeColor = Color.White;
            btnDeseleccionarTodo.Location = new Point(150, 8);
            btnDeseleccionarTodo.Name = "btnDeseleccionarTodo";
            btnDeseleccionarTodo.Size = new Size(130, 30);
            btnDeseleccionarTodo.TabIndex = 1;
            btnDeseleccionarTodo.Text = "Deseleccionar Todo";
            btnDeseleccionarTodo.UseVisualStyleBackColor = false;
            // 
            // btnSeleccionarTodo
            // 
            btnSeleccionarTodo.BackColor = Color.FromArgb(45, 45, 48);
            btnSeleccionarTodo.FlatStyle = FlatStyle.Flat;
            btnSeleccionarTodo.ForeColor = Color.White;
            btnSeleccionarTodo.Location = new Point(10, 8);
            btnSeleccionarTodo.Name = "btnSeleccionarTodo";
            btnSeleccionarTodo.Size = new Size(130, 30);
            btnSeleccionarTodo.TabIndex = 0;
            btnSeleccionarTodo.Text = "Seleccionar Todo";
            btnSeleccionarTodo.UseVisualStyleBackColor = false;
            // 
            // panelGrilla
            // 
            panelGrilla.Controls.Add(dgvLotes);
            panelGrilla.Controls.Add(lblInfoSeleccion);
            panelGrilla.Dock = DockStyle.Fill;
            panelGrilla.Location = new Point(10, 26);
            panelGrilla.Name = "panelGrilla";
            panelGrilla.Padding = new Padding(0, 5, 0, 0);
            panelGrilla.Size = new Size(680, 614);
            panelGrilla.TabIndex = 2;
            // 
            // dgvLotes
            // 
            dgvLotes.AllowUserToAddRows = false;
            dgvLotes.AllowUserToDeleteRows = false;
            dgvLotes.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvLotes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLotes.Dock = DockStyle.Fill;
            dgvLotes.Location = new Point(0, 5);
            dgvLotes.Name = "dgvLotes";
            dgvLotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLotes.Size = new Size(680, 584);
            dgvLotes.TabIndex = 0;
            // 
            // lblInfoSeleccion
            // 
            lblInfoSeleccion.Dock = DockStyle.Bottom;
            lblInfoSeleccion.Location = new Point(0, 589);
            lblInfoSeleccion.Name = "lblInfoSeleccion";
            lblInfoSeleccion.Padding = new Padding(5);
            lblInfoSeleccion.Size = new Size(680, 25);
            lblInfoSeleccion.TabIndex = 1;
            lblInfoSeleccion.Text = "Lotes seleccionados: 0 | Total archivos: 0";
            // 
            // panelDerecha
            // 
            panelDerecha.Controls.Add(groupBoxPreview);
            panelDerecha.Controls.Add(groupBoxConfiguracion);
            panelDerecha.Dock = DockStyle.Fill;
            panelDerecha.Location = new Point(0, 0);
            panelDerecha.Name = "panelDerecha";
            panelDerecha.Padding = new Padding(5);
            panelDerecha.Size = new Size(476, 650);
            panelDerecha.TabIndex = 0;
            // 
            // groupBoxPreview
            // 
            groupBoxPreview.Controls.Add(panelVisor);
            groupBoxPreview.Controls.Add(panelBotonesPreview);
            groupBoxPreview.Dock = DockStyle.Fill;
            groupBoxPreview.ForeColor = Color.White;
            groupBoxPreview.Location = new Point(5, 245);
            groupBoxPreview.Name = "groupBoxPreview";
            groupBoxPreview.Padding = new Padding(10);
            groupBoxPreview.Size = new Size(466, 400);
            groupBoxPreview.TabIndex = 1;
            groupBoxPreview.TabStop = false;
            groupBoxPreview.Text = "Previsualización de Recorte";
            // 
            // panelVisor
            // 
            panelVisor.AutoScroll = true;
            panelVisor.BorderStyle = BorderStyle.FixedSingle;
            panelVisor.Controls.Add(pictureBoxPreview);
            panelVisor.Dock = DockStyle.Fill;
            panelVisor.Location = new Point(10, 26);
            panelVisor.Name = "panelVisor";
            panelVisor.Size = new Size(446, 314);
            panelVisor.TabIndex = 0;
            // 
            // pictureBoxPreview
            // 
            pictureBoxPreview.Dock = DockStyle.Fill;
            pictureBoxPreview.Location = new Point(0, 0);
            pictureBoxPreview.Name = "pictureBoxPreview";
            pictureBoxPreview.Size = new Size(444, 312);
            pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxPreview.TabIndex = 0;
            pictureBoxPreview.TabStop = false;
            // 
            // panelBotonesPreview
            // 
            panelBotonesPreview.Controls.Add(btnActualizarImagen);
            panelBotonesPreview.Controls.Add(btnCargarPDF);
            panelBotonesPreview.Dock = DockStyle.Bottom;
            panelBotonesPreview.Location = new Point(10, 340);
            panelBotonesPreview.Name = "panelBotonesPreview";
            panelBotonesPreview.Padding = new Padding(0, 5, 0, 0);
            panelBotonesPreview.Size = new Size(446, 50);
            panelBotonesPreview.TabIndex = 1;
            // 
            // btnActualizarImagen
            // 
            btnActualizarImagen.BackColor = Color.FromArgb(45, 45, 48);
            btnActualizarImagen.Dock = DockStyle.Right;
            btnActualizarImagen.FlatStyle = FlatStyle.Flat;
            btnActualizarImagen.ForeColor = Color.White;
            btnActualizarImagen.Location = new Point(246, 5);
            btnActualizarImagen.Name = "btnActualizarImagen";
            btnActualizarImagen.Size = new Size(200, 45);
            btnActualizarImagen.TabIndex = 1;
            btnActualizarImagen.Text = "Actualizar Imagen";
            btnActualizarImagen.UseVisualStyleBackColor = false;
            // 
            // btnCargarPDF
            // 
            btnCargarPDF.BackColor = Color.FromArgb(0, 122, 204);
            btnCargarPDF.Dock = DockStyle.Left;
            btnCargarPDF.FlatStyle = FlatStyle.Flat;
            btnCargarPDF.ForeColor = Color.White;
            btnCargarPDF.Location = new Point(0, 5);
            btnCargarPDF.Name = "btnCargarPDF";
            btnCargarPDF.Size = new Size(200, 45);
            btnCargarPDF.TabIndex = 0;
            btnCargarPDF.Text = "Cargar PDF para Preview";
            btnCargarPDF.UseVisualStyleBackColor = false;
            // 
            // groupBoxConfiguracion
            // 
            groupBoxConfiguracion.Controls.Add(chkEjecutarOCR);
            groupBoxConfiguracion.Controls.Add(numPorcentajeHorizontal);
            groupBoxConfiguracion.Controls.Add(lblPorcentajeHorizontal);
            groupBoxConfiguracion.Controls.Add(numPorcentajeVertical);
            groupBoxConfiguracion.Controls.Add(lblPorcentajeVertical);
            groupBoxConfiguracion.Controls.Add(numDPI);
            groupBoxConfiguracion.Controls.Add(lblDPI);
            groupBoxConfiguracion.Controls.Add(cboEsquina);
            groupBoxConfiguracion.Controls.Add(lblEsquina);
            groupBoxConfiguracion.Dock = DockStyle.Top;
            groupBoxConfiguracion.ForeColor = Color.White;
            groupBoxConfiguracion.Location = new Point(5, 5);
            groupBoxConfiguracion.Name = "groupBoxConfiguracion";
            groupBoxConfiguracion.Padding = new Padding(10);
            groupBoxConfiguracion.Size = new Size(466, 240);
            groupBoxConfiguracion.TabIndex = 0;
            groupBoxConfiguracion.TabStop = false;
            groupBoxConfiguracion.Text = "Configuración de Recorte";
            // 
            // chkEjecutarOCR
            // 
            chkEjecutarOCR.AutoSize = true;
            chkEjecutarOCR.Location = new Point(20, 200);
            chkEjecutarOCR.Name = "chkEjecutarOCR";
            chkEjecutarOCR.Size = new Size(95, 19);
            chkEjecutarOCR.TabIndex = 8;
            chkEjecutarOCR.Text = "Ejecutar OCR";
            chkEjecutarOCR.UseVisualStyleBackColor = true;
            // 
            // numPorcentajeHorizontal
            // 
            numPorcentajeHorizontal.DecimalPlaces = 2;
            numPorcentajeHorizontal.Location = new Point(20, 160);
            numPorcentajeHorizontal.Name = "numPorcentajeHorizontal";
            numPorcentajeHorizontal.Size = new Size(100, 23);
            numPorcentajeHorizontal.TabIndex = 7;
            numPorcentajeHorizontal.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // lblPorcentajeHorizontal
            // 
            lblPorcentajeHorizontal.AutoSize = true;
            lblPorcentajeHorizontal.Location = new Point(20, 140);
            lblPorcentajeHorizontal.Name = "lblPorcentajeHorizontal";
            lblPorcentajeHorizontal.Size = new Size(188, 15);
            lblPorcentajeHorizontal.TabIndex = 6;
            lblPorcentajeHorizontal.Text = "Recorte Porcentaje Horizontal (%):";
            // 
            // numPorcentajeVertical
            // 
            numPorcentajeVertical.DecimalPlaces = 2;
            numPorcentajeVertical.Location = new Point(20, 110);
            numPorcentajeVertical.Name = "numPorcentajeVertical";
            numPorcentajeVertical.Size = new Size(100, 23);
            numPorcentajeVertical.TabIndex = 5;
            numPorcentajeVertical.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // lblPorcentajeVertical
            // 
            lblPorcentajeVertical.AutoSize = true;
            lblPorcentajeVertical.Location = new Point(20, 90);
            lblPorcentajeVertical.Name = "lblPorcentajeVertical";
            lblPorcentajeVertical.Size = new Size(171, 15);
            lblPorcentajeVertical.TabIndex = 4;
            lblPorcentajeVertical.Text = "Recorte Porcentaje Vertical (%):";
            // 
            // numDPI
            // 
            numDPI.Increment = new decimal(new int[] { 50, 0, 0, 0 });
            numDPI.Location = new Point(120, 60);
            numDPI.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            numDPI.Minimum = new decimal(new int[] { 72, 0, 0, 0 });
            numDPI.Name = "numDPI";
            numDPI.Size = new Size(100, 23);
            numDPI.TabIndex = 3;
            numDPI.Value = new decimal(new int[] { 300, 0, 0, 0 });
            // 
            // lblDPI
            // 
            lblDPI.AutoSize = true;
            lblDPI.Location = new Point(20, 62);
            lblDPI.Name = "lblDPI";
            lblDPI.Size = new Size(28, 15);
            lblDPI.TabIndex = 2;
            lblDPI.Text = "DPI:";
            // 
            // cboEsquina
            // 
            cboEsquina.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEsquina.FormattingEnabled = true;
            cboEsquina.Location = new Point(150, 27);
            cboEsquina.Name = "cboEsquina";
            cboEsquina.Size = new Size(200, 23);
            cboEsquina.TabIndex = 1;
            // 
            // lblEsquina
            // 
            lblEsquina.AutoSize = true;
            lblEsquina.Location = new Point(20, 30);
            lblEsquina.Name = "lblEsquina";
            lblEsquina.Size = new Size(104, 15);
            lblEsquina.TabIndex = 0;
            lblEsquina.Text = "Esquina a recortar:";
            // 
            // panelInferior
            // 
            panelInferior.Controls.Add(progressBar);
            panelInferior.Controls.Add(lblProgresoTexto);
            panelInferior.Controls.Add(btnCerrar);
            panelInferior.Controls.Add(btnProcesar);
            panelInferior.Dock = DockStyle.Bottom;
            panelInferior.Location = new Point(10, 660);
            panelInferior.Name = "panelInferior";
            panelInferior.Padding = new Padding(0, 10, 0, 10);
            panelInferior.Size = new Size(1180, 90);
            panelInferior.TabIndex = 1;
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Fill;
            progressBar.Location = new Point(0, 30);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(730, 50);
            progressBar.TabIndex = 0;
            // 
            // lblProgresoTexto
            // 
            lblProgresoTexto.Dock = DockStyle.Top;
            lblProgresoTexto.Location = new Point(0, 10);
            lblProgresoTexto.Name = "lblProgresoTexto";
            lblProgresoTexto.Padding = new Padding(5, 0, 5, 0);
            lblProgresoTexto.Size = new Size(730, 20);
            lblProgresoTexto.TabIndex = 1;
            lblProgresoTexto.Text = "Listo para procesar";
            lblProgresoTexto.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.FromArgb(60, 60, 63);
            btnCerrar.Dock = DockStyle.Right;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 10F);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(730, 10);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(150, 70);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            // 
            // btnProcesar
            // 
            btnProcesar.BackColor = Color.FromArgb(0, 122, 204);
            btnProcesar.Dock = DockStyle.Right;
            btnProcesar.FlatStyle = FlatStyle.Flat;
            btnProcesar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnProcesar.ForeColor = Color.White;
            btnProcesar.Location = new Point(880, 10);
            btnProcesar.Name = "btnProcesar";
            btnProcesar.Size = new Size(300, 70);
            btnProcesar.TabIndex = 2;
            btnProcesar.Text = "Procesar Lotes Seleccionados";
            btnProcesar.UseVisualStyleBackColor = false;
            // 
            // FrmPreparacionImagenes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1200, 760);
            Controls.Add(splitContainer1);
            Controls.Add(panelInferior);
            ForeColor = Color.White;
            Name = "FrmPreparacionImagenes";
            Padding = new Padding(10);
            Text = "Preparación de Imágenes";
            Load += FrmPreparacionImagenes_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBoxPrincipal.ResumeLayout(false);
            panelFiltros.ResumeLayout(false);
            panelFiltros.PerformLayout();
            panelBotonesSeleccion.ResumeLayout(false);
            panelGrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLotes).EndInit();
            panelDerecha.ResumeLayout(false);
            groupBoxPreview.ResumeLayout(false);
            panelVisor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreview).EndInit();
            panelBotonesPreview.ResumeLayout(false);
            groupBoxConfiguracion.ResumeLayout(false);
            groupBoxConfiguracion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPorcentajeHorizontal).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPorcentajeVertical).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDPI).EndInit();
            panelInferior.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private GroupBox groupBoxPrincipal;
        private Panel panelFiltros;
        private TextBox txtFiltroNombre;
        private Label lblFiltroNombre;
        private DateTimePicker dtpDesde;
        private DateTimePicker dtpHasta;
        private Label lblDesde;
        private Label lblHasta;
        private Button btnBuscar;
        private Button btnLimpiar;
        private Panel panelBotonesSeleccion;
        private Button btnSeleccionarTodo;
        private Button btnDeseleccionarTodo;
        private Button btnSeleccionar50;
        private Panel panelGrilla;
        private DataGridView dgvLotes;
        private Label lblInfoSeleccion;
        private Panel panelDerecha;
        private GroupBox groupBoxConfiguracion;
        private Label lblEsquina;
        private ComboBox cboEsquina;
        private Label lblDPI;
        private NumericUpDown numDPI;
        private Label lblPorcentajeVertical;
        private NumericUpDown numPorcentajeVertical;
        private Label lblPorcentajeHorizontal;
        private NumericUpDown numPorcentajeHorizontal;
        private CheckBox chkEjecutarOCR;
        private GroupBox groupBoxPreview;
        private Panel panelVisor;
        private PictureBox pictureBoxPreview;
        private Panel panelBotonesPreview;
        private Button btnCargarPDF;
        private Button btnActualizarImagen;
        private Panel panelInferior;
        private ProgressBar progressBar;
        private Label lblProgresoTexto;
        private Button btnProcesar;
        private Button btnCerrar;
    }
}
