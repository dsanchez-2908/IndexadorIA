namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmSeparacionImagenes
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
            pnlTop = new Panel();
            btnCerrar2 = new Button();
            lblTitulo = new Label();
            dgvArchivos = new DataGridView();
            colSeleccion = new DataGridViewCheckBoxColumn();
            pnlBotones = new Panel();
            btnSeleccionarTodo = new Button();
            btnDeseleccionarTodo = new Button();
            btnMarcar50 = new Button();
            pnlContadores = new Panel();
            lblTotalRegistros = new Label();
            lblRegistrosSeleccionados = new Label();
            pnlOpciones = new Panel();
            chkEliminarOriginal = new CheckBox();
            chkHabilitarGiro = new CheckBox();
            rbGirarAutomatico = new RadioButton();
            rbGirar90Derecha = new RadioButton();
            rbGirar90Izquierda = new RadioButton();
            chkMarcaBlanco = new CheckBox();
            btnProcesar = new Button();
            progressBar = new ProgressBar();
            lblProgreso = new Label();
            pnlFiltros = new Panel();
            chkFiltroDesde = new CheckBox();
            dtpFiltroDesde = new DateTimePicker();
            chkFiltroHasta = new CheckBox();
            dtpFiltroHasta = new DateTimePicker();
            lblFiltroCarpeta = new Label();
            txtFiltroCarpeta = new TextBox();
            btnFiltrar = new Button();
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArchivos).BeginInit();
            pnlBotones.SuspendLayout();
            pnlContadores.SuspendLayout();
            pnlOpciones.SuspendLayout();
            pnlFiltros.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(45, 45, 48);
            pnlTop.Controls.Add(btnCerrar2);
            pnlTop.Controls.Add(lblTitulo);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1200, 50);
            pnlTop.TabIndex = 0;
            // 
            // btnCerrar2
            // 
            btnCerrar2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar2.BackColor = Color.FromArgb(150, 30, 30);
            btnCerrar2.FlatStyle = FlatStyle.Flat;
            btnCerrar2.ForeColor = Color.White;
            btnCerrar2.Location = new Point(1063, 7);
            btnCerrar2.Margin = new Padding(3, 3, 8, 3);
            btnCerrar2.Name = "btnCerrar2";
            btnCerrar2.Size = new Size(120, 34);
            btnCerrar2.TabIndex = 9;
            btnCerrar2.Text = "Cerrar";
            btnCerrar2.UseVisualStyleBackColor = false;
            btnCerrar2.Click += btnCerrar2_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(228, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Separación de Imágenes";
            // 
            // dgvArchivos
            // 
            dgvArchivos.AllowUserToAddRows = false;
            dgvArchivos.AllowUserToDeleteRows = false;
            dgvArchivos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvArchivos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvArchivos.BackgroundColor = Color.FromArgb(37, 37, 38);
            dgvArchivos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArchivos.Columns.AddRange(new DataGridViewColumn[] { colSeleccion });
            dgvArchivos.Location = new Point(12, 175);
            dgvArchivos.Name = "dgvArchivos";
            dgvArchivos.RowHeadersVisible = false;
            dgvArchivos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArchivos.Size = new Size(1176, 305);
            dgvArchivos.TabIndex = 1;
            dgvArchivos.CellContentClick += dgvArchivos_CellContentClick;
            // 
            // colSeleccion
            // 
            colSeleccion.FillWeight = 15F;
            colSeleccion.HeaderText = "Seleccionar";
            colSeleccion.Name = "colSeleccion";
            // 
            // pnlBotones
            // 
            pnlBotones.Controls.Add(btnSeleccionarTodo);
            pnlBotones.Controls.Add(btnDeseleccionarTodo);
            pnlBotones.Controls.Add(btnMarcar50);
            pnlBotones.Location = new Point(12, 55);
            pnlBotones.Name = "pnlBotones";
            pnlBotones.Size = new Size(500, 40);
            pnlBotones.TabIndex = 2;
            // 
            // btnSeleccionarTodo
            // 
            btnSeleccionarTodo.BackColor = Color.FromArgb(0, 122, 204);
            btnSeleccionarTodo.FlatAppearance.BorderSize = 0;
            btnSeleccionarTodo.FlatStyle = FlatStyle.Flat;
            btnSeleccionarTodo.ForeColor = Color.White;
            btnSeleccionarTodo.Location = new Point(4, 5);
            btnSeleccionarTodo.Name = "btnSeleccionarTodo";
            btnSeleccionarTodo.Size = new Size(130, 30);
            btnSeleccionarTodo.TabIndex = 0;
            btnSeleccionarTodo.Text = "Seleccionar Todo";
            btnSeleccionarTodo.UseVisualStyleBackColor = false;
            btnSeleccionarTodo.Click += btnSeleccionarTodo_Click;
            // 
            // btnDeseleccionarTodo
            // 
            btnDeseleccionarTodo.BackColor = Color.FromArgb(63, 63, 70);
            btnDeseleccionarTodo.FlatAppearance.BorderSize = 0;
            btnDeseleccionarTodo.FlatStyle = FlatStyle.Flat;
            btnDeseleccionarTodo.ForeColor = Color.White;
            btnDeseleccionarTodo.Location = new Point(142, 5);
            btnDeseleccionarTodo.Name = "btnDeseleccionarTodo";
            btnDeseleccionarTodo.Size = new Size(150, 30);
            btnDeseleccionarTodo.TabIndex = 1;
            btnDeseleccionarTodo.Text = "Deseleccionar Todo";
            btnDeseleccionarTodo.UseVisualStyleBackColor = false;
            btnDeseleccionarTodo.Click += btnDeseleccionarTodo_Click;
            // 
            // btnMarcar50
            // 
            btnMarcar50.BackColor = Color.FromArgb(63, 63, 70);
            btnMarcar50.FlatAppearance.BorderSize = 0;
            btnMarcar50.FlatStyle = FlatStyle.Flat;
            btnMarcar50.ForeColor = Color.White;
            btnMarcar50.Location = new Point(302, 5);
            btnMarcar50.Name = "btnMarcar50";
            btnMarcar50.Size = new Size(100, 30);
            btnMarcar50.TabIndex = 2;
            btnMarcar50.Text = "Marcar 50";
            btnMarcar50.UseVisualStyleBackColor = false;
            btnMarcar50.Click += btnMarcar50_Click;
            // 
            // pnlContadores
            // 
            pnlContadores.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlContadores.Controls.Add(lblTotalRegistros);
            pnlContadores.Controls.Add(lblRegistrosSeleccionados);
            pnlContadores.Location = new Point(900, 55);
            pnlContadores.Name = "pnlContadores";
            pnlContadores.Size = new Size(288, 60);
            pnlContadores.TabIndex = 3;
            // 
            // lblTotalRegistros
            // 
            lblTotalRegistros.AutoSize = true;
            lblTotalRegistros.ForeColor = Color.White;
            lblTotalRegistros.Location = new Point(10, 10);
            lblTotalRegistros.Name = "lblTotalRegistros";
            lblTotalRegistros.Size = new Size(109, 15);
            lblTotalRegistros.TabIndex = 0;
            lblTotalRegistros.Text = "Total de registros: 0";
            // 
            // lblRegistrosSeleccionados
            // 
            lblRegistrosSeleccionados.AutoSize = true;
            lblRegistrosSeleccionados.ForeColor = Color.White;
            lblRegistrosSeleccionados.Location = new Point(10, 35);
            lblRegistrosSeleccionados.Name = "lblRegistrosSeleccionados";
            lblRegistrosSeleccionados.Size = new Size(144, 15);
            lblRegistrosSeleccionados.TabIndex = 1;
            lblRegistrosSeleccionados.Text = "Registros seleccionados: 0";
            // 
            // pnlOpciones
            // 
            pnlOpciones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pnlOpciones.Controls.Add(chkEliminarOriginal);
            pnlOpciones.Controls.Add(chkHabilitarGiro);
            pnlOpciones.Controls.Add(rbGirarAutomatico);
            pnlOpciones.Controls.Add(rbGirar90Derecha);
            pnlOpciones.Controls.Add(rbGirar90Izquierda);
            pnlOpciones.Controls.Add(chkMarcaBlanco);
            pnlOpciones.Controls.Add(btnProcesar);
            pnlOpciones.Location = new Point(12, 490);
            pnlOpciones.Name = "pnlOpciones";
            pnlOpciones.Size = new Size(700, 90);
            pnlOpciones.TabIndex = 4;
            // 
            // chkEliminarOriginal
            // 
            chkEliminarOriginal.AutoSize = true;
            chkEliminarOriginal.ForeColor = Color.White;
            chkEliminarOriginal.Location = new Point(10, 10);
            chkEliminarOriginal.Name = "chkEliminarOriginal";
            chkEliminarOriginal.Size = new Size(154, 19);
            chkEliminarOriginal.TabIndex = 0;
            chkEliminarOriginal.Text = "Eliminar archivo original";
            chkEliminarOriginal.UseVisualStyleBackColor = true;
            // 
            // chkHabilitarGiro
            // 
            chkHabilitarGiro.AutoSize = true;
            chkHabilitarGiro.ForeColor = Color.White;
            chkHabilitarGiro.Location = new Point(10, 35);
            chkHabilitarGiro.Name = "chkHabilitarGiro";
            chkHabilitarGiro.Size = new Size(96, 19);
            chkHabilitarGiro.TabIndex = 1;
            chkHabilitarGiro.Text = "Habilitar Giro";
            chkHabilitarGiro.UseVisualStyleBackColor = true;
            chkHabilitarGiro.CheckedChanged += chkHabilitarGiro_CheckedChanged;
            // 
            // rbGirarAutomatico
            // 
            rbGirarAutomatico.AutoSize = true;
            rbGirarAutomatico.Checked = true;
            rbGirarAutomatico.Enabled = false;
            rbGirarAutomatico.ForeColor = Color.White;
            rbGirarAutomatico.Location = new Point(30, 58);
            rbGirarAutomatico.Name = "rbGirarAutomatico";
            rbGirarAutomatico.Size = new Size(149, 19);
            rbGirarAutomatico.TabIndex = 2;
            rbGirarAutomatico.TabStop = true;
            rbGirarAutomatico.Text = "Girar Automáticamente";
            rbGirarAutomatico.UseVisualStyleBackColor = true;
            // 
            // rbGirar90Derecha
            // 
            rbGirar90Derecha.AutoSize = true;
            rbGirar90Derecha.Enabled = false;
            rbGirar90Derecha.ForeColor = Color.White;
            rbGirar90Derecha.Location = new Point(200, 58);
            rbGirar90Derecha.Name = "rbGirar90Derecha";
            rbGirar90Derecha.Size = new Size(136, 19);
            rbGirar90Derecha.TabIndex = 3;
            rbGirar90Derecha.Text = "Girar 90° a la derecha";
            rbGirar90Derecha.UseVisualStyleBackColor = true;
            // 
            // rbGirar90Izquierda
            // 
            rbGirar90Izquierda.AutoSize = true;
            rbGirar90Izquierda.Enabled = false;
            rbGirar90Izquierda.ForeColor = Color.White;
            rbGirar90Izquierda.Location = new Point(370, 58);
            rbGirar90Izquierda.Name = "rbGirar90Izquierda";
            rbGirar90Izquierda.Size = new Size(142, 19);
            rbGirar90Izquierda.TabIndex = 4;
            rbGirar90Izquierda.Text = "Girar 90° a la izquierda";
            rbGirar90Izquierda.UseVisualStyleBackColor = true;
            // 
            // chkMarcaBlanco
            // 
            chkMarcaBlanco.AutoSize = true;
            chkMarcaBlanco.ForeColor = Color.White;
            chkMarcaBlanco.Location = new Point(250, 10);
            chkMarcaBlanco.Name = "chkMarcaBlanco";
            chkMarcaBlanco.Size = new Size(211, 19);
            chkMarcaBlanco.TabIndex = 5;
            chkMarcaBlanco.Text = "Marcar como posiblemente blanco";
            chkMarcaBlanco.UseVisualStyleBackColor = true;
            // 
            // btnProcesar
            // 
            btnProcesar.BackColor = Color.FromArgb(0, 122, 204);
            btnProcesar.FlatAppearance.BorderSize = 0;
            btnProcesar.FlatStyle = FlatStyle.Flat;
            btnProcesar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnProcesar.ForeColor = Color.White;
            btnProcesar.Location = new Point(550, 25);
            btnProcesar.Name = "btnProcesar";
            btnProcesar.Size = new Size(120, 40);
            btnProcesar.TabIndex = 6;
            btnProcesar.Text = "Procesar";
            btnProcesar.UseVisualStyleBackColor = false;
            btnProcesar.Click += btnProcesar_Click;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(12, 580);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1176, 23);
            progressBar.TabIndex = 5;
            // 
            // lblProgreso
            // 
            lblProgreso.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblProgreso.ForeColor = Color.White;
            lblProgreso.Location = new Point(12, 610);
            lblProgreso.Name = "lblProgreso";
            lblProgreso.Size = new Size(1176, 20);
            lblProgreso.TabIndex = 6;
            lblProgreso.Text = "Listo para procesar";
            lblProgreso.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlFiltros
            // 
            pnlFiltros.BackColor = Color.FromArgb(45, 45, 48);
            pnlFiltros.Controls.Add(chkFiltroDesde);
            pnlFiltros.Controls.Add(dtpFiltroDesde);
            pnlFiltros.Controls.Add(chkFiltroHasta);
            pnlFiltros.Controls.Add(dtpFiltroHasta);
            pnlFiltros.Controls.Add(lblFiltroCarpeta);
            pnlFiltros.Controls.Add(txtFiltroCarpeta);
            pnlFiltros.Controls.Add(btnFiltrar);
            pnlFiltros.Location = new Point(12, 120);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Size = new Size(1176, 45);
            pnlFiltros.TabIndex = 7;
            // 
            // chkFiltroDesde
            // 
            chkFiltroDesde.AutoSize = true;
            chkFiltroDesde.ForeColor = Color.White;
            chkFiltroDesde.Location = new Point(10, 10);
            chkFiltroDesde.Name = "chkFiltroDesde";
            chkFiltroDesde.Size = new Size(116, 19);
            chkFiltroDesde.TabIndex = 0;
            chkFiltroDesde.Text = "Desde Fecha Alta";
            chkFiltroDesde.UseVisualStyleBackColor = true;
            chkFiltroDesde.CheckedChanged += chkFiltroDesde_CheckedChanged;
            // 
            // dtpFiltroDesde
            // 
            dtpFiltroDesde.Enabled = false;
            dtpFiltroDesde.Format = DateTimePickerFormat.Short;
            dtpFiltroDesde.Location = new Point(150, 8);
            dtpFiltroDesde.Name = "dtpFiltroDesde";
            dtpFiltroDesde.Size = new Size(120, 23);
            dtpFiltroDesde.TabIndex = 1;
            // 
            // chkFiltroHasta
            // 
            chkFiltroHasta.AutoSize = true;
            chkFiltroHasta.ForeColor = Color.White;
            chkFiltroHasta.Location = new Point(290, 10);
            chkFiltroHasta.Name = "chkFiltroHasta";
            chkFiltroHasta.Size = new Size(114, 19);
            chkFiltroHasta.TabIndex = 2;
            chkFiltroHasta.Text = "Hasta Fecha Alta";
            chkFiltroHasta.UseVisualStyleBackColor = true;
            chkFiltroHasta.CheckedChanged += chkFiltroHasta_CheckedChanged;
            // 
            // dtpFiltroHasta
            // 
            dtpFiltroHasta.Enabled = false;
            dtpFiltroHasta.Format = DateTimePickerFormat.Short;
            dtpFiltroHasta.Location = new Point(430, 8);
            dtpFiltroHasta.Name = "dtpFiltroHasta";
            dtpFiltroHasta.Size = new Size(120, 23);
            dtpFiltroHasta.TabIndex = 3;
            // 
            // lblFiltroCarpeta
            // 
            lblFiltroCarpeta.AutoSize = true;
            lblFiltroCarpeta.ForeColor = Color.White;
            lblFiltroCarpeta.Location = new Point(570, 12);
            lblFiltroCarpeta.Name = "lblFiltroCarpeta";
            lblFiltroCarpeta.Size = new Size(133, 15);
            lblFiltroCarpeta.TabIndex = 4;
            lblFiltroCarpeta.Text = "Nombre última carpeta:";
            // 
            // txtFiltroCarpeta
            // 
            txtFiltroCarpeta.Location = new Point(709, 8);
            txtFiltroCarpeta.Name = "txtFiltroCarpeta";
            txtFiltroCarpeta.Size = new Size(191, 23);
            txtFiltroCarpeta.TabIndex = 5;
            // 
            // btnFiltrar
            // 
            btnFiltrar.BackColor = Color.FromArgb(63, 63, 70);
            btnFiltrar.FlatAppearance.BorderSize = 0;
            btnFiltrar.FlatStyle = FlatStyle.Flat;
            btnFiltrar.ForeColor = Color.White;
            btnFiltrar.Location = new Point(920, 6);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(100, 30);
            btnFiltrar.TabIndex = 6;
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.UseVisualStyleBackColor = false;
            btnFiltrar.Click += btnFiltrar_Click;
            // 
            // FrmSeparacionImagenes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(37, 37, 38);
            ClientSize = new Size(1200, 640);
            Controls.Add(lblProgreso);
            Controls.Add(progressBar);
            Controls.Add(pnlOpciones);
            Controls.Add(pnlContadores);
            Controls.Add(pnlBotones);
            Controls.Add(dgvArchivos);
            Controls.Add(pnlFiltros);
            Controls.Add(pnlTop);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmSeparacionImagenes";
            Text = "Separación de Imágenes";
            Load += FrmSeparacionImagenes_Load;
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArchivos).EndInit();
            pnlBotones.ResumeLayout(false);
            pnlContadores.ResumeLayout(false);
            pnlContadores.PerformLayout();
            pnlOpciones.ResumeLayout(false);
            pnlOpciones.PerformLayout();
            pnlFiltros.ResumeLayout(false);
            pnlFiltros.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvArchivos;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSeleccion;
        private System.Windows.Forms.Panel pnlBotones;
        private System.Windows.Forms.Button btnSeleccionarTodo;
        private System.Windows.Forms.Button btnDeseleccionarTodo;
        private System.Windows.Forms.Button btnMarcar50;
        private System.Windows.Forms.Panel pnlContadores;
        private System.Windows.Forms.Label lblTotalRegistros;
        private System.Windows.Forms.Label lblRegistrosSeleccionados;
        private System.Windows.Forms.Panel pnlOpciones;
        private System.Windows.Forms.CheckBox chkEliminarOriginal;
        private System.Windows.Forms.CheckBox chkHabilitarGiro;
        private System.Windows.Forms.RadioButton rbGirarAutomatico;
        private System.Windows.Forms.RadioButton rbGirar90Derecha;
        private System.Windows.Forms.RadioButton rbGirar90Izquierda;
        private System.Windows.Forms.CheckBox chkMarcaBlanco;
        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblProgreso;
        private System.Windows.Forms.Panel pnlFiltros;
        private System.Windows.Forms.CheckBox chkFiltroDesde;
        private System.Windows.Forms.DateTimePicker dtpFiltroDesde;
        private System.Windows.Forms.CheckBox chkFiltroHasta;
        private System.Windows.Forms.DateTimePicker dtpFiltroHasta;
        private System.Windows.Forms.Label lblFiltroCarpeta;
        private System.Windows.Forms.TextBox txtFiltroCarpeta;
        private System.Windows.Forms.Button btnFiltrar;
        private Button btnCerrar2;
    }
}
