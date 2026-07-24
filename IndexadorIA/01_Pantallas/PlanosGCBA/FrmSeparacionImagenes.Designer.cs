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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dgvArchivos = new System.Windows.Forms.DataGridView();
            this.colSeleccion = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pnlBotones = new System.Windows.Forms.Panel();
            this.btnSeleccionarTodo = new System.Windows.Forms.Button();
            this.btnDeseleccionarTodo = new System.Windows.Forms.Button();
            this.btnMarcar50 = new System.Windows.Forms.Button();
            this.pnlContadores = new System.Windows.Forms.Panel();
            this.lblTotalRegistros = new System.Windows.Forms.Label();
            this.lblRegistrosSeleccionados = new System.Windows.Forms.Label();
            this.pnlOpciones = new System.Windows.Forms.Panel();
            this.chkEliminarOriginal = new System.Windows.Forms.CheckBox();
            this.chkHabilitarGiro = new System.Windows.Forms.CheckBox();
            this.rbGirarAutomatico = new System.Windows.Forms.RadioButton();
            this.rbGirar90Derecha = new System.Windows.Forms.RadioButton();
            this.rbGirar90Izquierda = new System.Windows.Forms.RadioButton();
            this.chkMarcaBlanco = new System.Windows.Forms.CheckBox();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgreso = new System.Windows.Forms.Label();
            this.pnlFiltros = new System.Windows.Forms.Panel();
            this.chkFiltroDesde = new System.Windows.Forms.CheckBox();
            this.dtpFiltroDesde = new System.Windows.Forms.DateTimePicker();
            this.chkFiltroHasta = new System.Windows.Forms.CheckBox();
            this.dtpFiltroHasta = new System.Windows.Forms.DateTimePicker();
            this.lblFiltroCarpeta = new System.Windows.Forms.Label();
            this.txtFiltroCarpeta = new System.Windows.Forms.TextBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArchivos)).BeginInit();
            this.pnlBotones.SuspendLayout();
            this.pnlContadores.SuspendLayout();
            this.pnlOpciones.SuspendLayout();
            this.pnlFiltros.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.pnlTop.Controls.Add(this.btnCerrar);
            this.pnlTop.Controls.Add(this.lblTitulo);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1200, 50);
            this.pnlTop.TabIndex = 0;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(1130, 10);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(60, 30);
            this.btnCerrar.TabIndex = 1;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(12, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(234, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Separación de Imágenes";
            // 
            // dgvArchivos
            // 
            this.dgvArchivos.AllowUserToAddRows = false;
            this.dgvArchivos.AllowUserToDeleteRows = false;
            this.dgvArchivos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvArchivos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvArchivos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.dgvArchivos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArchivos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSeleccion});
            this.dgvArchivos.Location = new System.Drawing.Point(12, 175);
            this.dgvArchivos.Name = "dgvArchivos";
            this.dgvArchivos.RowHeadersVisible = false;
            this.dgvArchivos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvArchivos.Size = new System.Drawing.Size(1176, 305);
            this.dgvArchivos.TabIndex = 1;
            this.dgvArchivos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArchivos_CellContentClick);
            // 
            // colSeleccion
            // 
            this.colSeleccion.HeaderText = "Seleccionar";
            this.colSeleccion.Name = "colSeleccion";
            this.colSeleccion.FillWeight = 15F;
            // 
            // pnlBotones
            // 
            this.pnlBotones.Controls.Add(this.btnSeleccionarTodo);
            this.pnlBotones.Controls.Add(this.btnDeseleccionarTodo);
            this.pnlBotones.Controls.Add(this.btnMarcar50);
            this.pnlBotones.Location = new System.Drawing.Point(12, 55);
            this.pnlBotones.Name = "pnlBotones";
            this.pnlBotones.Size = new System.Drawing.Size(500, 40);
            this.pnlBotones.TabIndex = 2;
            // 
            // btnSeleccionarTodo
            // 
            this.btnSeleccionarTodo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSeleccionarTodo.FlatAppearance.BorderSize = 0;
            this.btnSeleccionarTodo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSeleccionarTodo.ForeColor = System.Drawing.Color.White;
            this.btnSeleccionarTodo.Location = new System.Drawing.Point(0, 5);
            this.btnSeleccionarTodo.Name = "btnSeleccionarTodo";
            this.btnSeleccionarTodo.Size = new System.Drawing.Size(130, 30);
            this.btnSeleccionarTodo.TabIndex = 0;
            this.btnSeleccionarTodo.Text = "Seleccionar Todo";
            this.btnSeleccionarTodo.UseVisualStyleBackColor = false;
            this.btnSeleccionarTodo.Click += new System.EventHandler(this.btnSeleccionarTodo_Click);
            // 
            // btnDeseleccionarTodo
            // 
            this.btnDeseleccionarTodo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnDeseleccionarTodo.FlatAppearance.BorderSize = 0;
            this.btnDeseleccionarTodo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeseleccionarTodo.ForeColor = System.Drawing.Color.White;
            this.btnDeseleccionarTodo.Location = new System.Drawing.Point(140, 5);
            this.btnDeseleccionarTodo.Name = "btnDeseleccionarTodo";
            this.btnDeseleccionarTodo.Size = new System.Drawing.Size(150, 30);
            this.btnDeseleccionarTodo.TabIndex = 1;
            this.btnDeseleccionarTodo.Text = "Deseleccionar Todo";
            this.btnDeseleccionarTodo.UseVisualStyleBackColor = false;
            this.btnDeseleccionarTodo.Click += new System.EventHandler(this.btnDeseleccionarTodo_Click);
            // 
            // btnMarcar50
            // 
            this.btnMarcar50.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnMarcar50.FlatAppearance.BorderSize = 0;
            this.btnMarcar50.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarcar50.ForeColor = System.Drawing.Color.White;
            this.btnMarcar50.Location = new System.Drawing.Point(300, 5);
            this.btnMarcar50.Name = "btnMarcar50";
            this.btnMarcar50.Size = new System.Drawing.Size(100, 30);
            this.btnMarcar50.TabIndex = 2;
            this.btnMarcar50.Text = "Marcar 50";
            this.btnMarcar50.UseVisualStyleBackColor = false;
            this.btnMarcar50.Click += new System.EventHandler(this.btnMarcar50_Click);
            // 
            // pnlContadores
            // 
            this.pnlContadores.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContadores.Controls.Add(this.lblTotalRegistros);
            this.pnlContadores.Controls.Add(this.lblRegistrosSeleccionados);
            this.pnlContadores.Location = new System.Drawing.Point(900, 55);
            this.pnlContadores.Name = "pnlContadores";
            this.pnlContadores.Size = new System.Drawing.Size(288, 60);
            this.pnlContadores.TabIndex = 3;
            // 
            // lblTotalRegistros
            // 
            this.lblTotalRegistros.AutoSize = true;
            this.lblTotalRegistros.ForeColor = System.Drawing.Color.White;
            this.lblTotalRegistros.Location = new System.Drawing.Point(10, 10);
            this.lblTotalRegistros.Name = "lblTotalRegistros";
            this.lblTotalRegistros.Size = new System.Drawing.Size(122, 15);
            this.lblTotalRegistros.TabIndex = 0;
            this.lblTotalRegistros.Text = "Total de registros: 0";
            // 
            // lblRegistrosSeleccionados
            // 
            this.lblRegistrosSeleccionados.AutoSize = true;
            this.lblRegistrosSeleccionados.ForeColor = System.Drawing.Color.White;
            this.lblRegistrosSeleccionados.Location = new System.Drawing.Point(10, 35);
            this.lblRegistrosSeleccionados.Name = "lblRegistrosSeleccionados";
            this.lblRegistrosSeleccionados.Size = new System.Drawing.Size(163, 15);
            this.lblRegistrosSeleccionados.TabIndex = 1;
            this.lblRegistrosSeleccionados.Text = "Registros seleccionados: 0";
            // 
            // pnlOpciones
            // 
            this.pnlOpciones.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlOpciones.Controls.Add(this.chkEliminarOriginal);
            this.pnlOpciones.Controls.Add(this.chkHabilitarGiro);
            this.pnlOpciones.Controls.Add(this.rbGirarAutomatico);
            this.pnlOpciones.Controls.Add(this.rbGirar90Derecha);
            this.pnlOpciones.Controls.Add(this.rbGirar90Izquierda);
            this.pnlOpciones.Controls.Add(this.chkMarcaBlanco);
            this.pnlOpciones.Controls.Add(this.btnProcesar);
            this.pnlOpciones.Location = new System.Drawing.Point(12, 490);
            this.pnlOpciones.Name = "pnlOpciones";
            this.pnlOpciones.Size = new System.Drawing.Size(700, 90);
            this.pnlOpciones.TabIndex = 4;
            // 
            // chkEliminarOriginal
            // 
            this.chkEliminarOriginal.AutoSize = true;
            this.chkEliminarOriginal.ForeColor = System.Drawing.Color.White;
            this.chkEliminarOriginal.Location = new System.Drawing.Point(10, 10);
            this.chkEliminarOriginal.Name = "chkEliminarOriginal";
            this.chkEliminarOriginal.Size = new System.Drawing.Size(164, 19);
            this.chkEliminarOriginal.TabIndex = 0;
            this.chkEliminarOriginal.Text = "Eliminar archivo original";
            this.chkEliminarOriginal.UseVisualStyleBackColor = true;
            // 
            // chkHabilitarGiro
            // 
            this.chkHabilitarGiro.AutoSize = true;
            this.chkHabilitarGiro.ForeColor = System.Drawing.Color.White;
            this.chkHabilitarGiro.Location = new System.Drawing.Point(10, 35);
            this.chkHabilitarGiro.Name = "chkHabilitarGiro";
            this.chkHabilitarGiro.Size = new System.Drawing.Size(103, 19);
            this.chkHabilitarGiro.TabIndex = 1;
            this.chkHabilitarGiro.Text = "Habilitar Giro";
            this.chkHabilitarGiro.UseVisualStyleBackColor = true;
            this.chkHabilitarGiro.CheckedChanged += new System.EventHandler(this.chkHabilitarGiro_CheckedChanged);
            // 
            // rbGirarAutomatico
            // 
            this.rbGirarAutomatico.AutoSize = true;
            this.rbGirarAutomatico.Checked = true;
            this.rbGirarAutomatico.Enabled = false;
            this.rbGirarAutomatico.ForeColor = System.Drawing.Color.White;
            this.rbGirarAutomatico.Location = new System.Drawing.Point(30, 58);
            this.rbGirarAutomatico.Name = "rbGirarAutomatico";
            this.rbGirarAutomatico.Size = new System.Drawing.Size(160, 19);
            this.rbGirarAutomatico.TabIndex = 2;
            this.rbGirarAutomatico.TabStop = true;
            this.rbGirarAutomatico.Text = "Girar Automáticamente";
            this.rbGirarAutomatico.UseVisualStyleBackColor = true;
            // 
            // rbGirar90Derecha
            // 
            this.rbGirar90Derecha.AutoSize = true;
            this.rbGirar90Derecha.Enabled = false;
            this.rbGirar90Derecha.ForeColor = System.Drawing.Color.White;
            this.rbGirar90Derecha.Location = new System.Drawing.Point(200, 58);
            this.rbGirar90Derecha.Name = "rbGirar90Derecha";
            this.rbGirar90Derecha.Size = new System.Drawing.Size(160, 19);
            this.rbGirar90Derecha.TabIndex = 3;
            this.rbGirar90Derecha.Text = "Girar 90° a la derecha";
            this.rbGirar90Derecha.UseVisualStyleBackColor = true;
            // 
            // rbGirar90Izquierda
            // 
            this.rbGirar90Izquierda.AutoSize = true;
            this.rbGirar90Izquierda.Enabled = false;
            this.rbGirar90Izquierda.ForeColor = System.Drawing.Color.White;
            this.rbGirar90Izquierda.Location = new System.Drawing.Point(370, 58);
            this.rbGirar90Izquierda.Name = "rbGirar90Izquierda";
            this.rbGirar90Izquierda.Size = new System.Drawing.Size(160, 19);
            this.rbGirar90Izquierda.TabIndex = 4;
            this.rbGirar90Izquierda.Text = "Girar 90° a la izquierda";
            this.rbGirar90Izquierda.UseVisualStyleBackColor = true;
            // 
            // chkMarcaBlanco
            // 
            this.chkMarcaBlanco.AutoSize = true;
            this.chkMarcaBlanco.ForeColor = System.Drawing.Color.White;
            this.chkMarcaBlanco.Location = new System.Drawing.Point(250, 10);
            this.chkMarcaBlanco.Name = "chkMarcaBlanco";
            this.chkMarcaBlanco.Size = new System.Drawing.Size(210, 19);
            this.chkMarcaBlanco.TabIndex = 5;
            this.chkMarcaBlanco.Text = "Marcar como posiblemente blanco";
            this.chkMarcaBlanco.UseVisualStyleBackColor = true;
            // 
            // btnProcesar
            // 
            this.btnProcesar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnProcesar.FlatAppearance.BorderSize = 0;
            this.btnProcesar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcesar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnProcesar.ForeColor = System.Drawing.Color.White;
            this.btnProcesar.Location = new System.Drawing.Point(550, 25);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(120, 40);
            this.btnProcesar.TabIndex = 6;
            this.btnProcesar.Text = "Procesar";
            this.btnProcesar.UseVisualStyleBackColor = false;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 580);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1176, 23);
            this.progressBar.TabIndex = 5;
            // 
            // lblProgreso
            // 
            this.lblProgreso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgreso.ForeColor = System.Drawing.Color.White;
            this.lblProgreso.Location = new System.Drawing.Point(12, 610);
            this.lblProgreso.Name = "lblProgreso";
            this.lblProgreso.Size = new System.Drawing.Size(1176, 20);
            this.lblProgreso.TabIndex = 6;
            this.lblProgreso.Text = "Listo para procesar";
            this.lblProgreso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlFiltros
            // 
            this.pnlFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.pnlFiltros.Controls.Add(this.chkFiltroDesde);
            this.pnlFiltros.Controls.Add(this.dtpFiltroDesde);
            this.pnlFiltros.Controls.Add(this.chkFiltroHasta);
            this.pnlFiltros.Controls.Add(this.dtpFiltroHasta);
            this.pnlFiltros.Controls.Add(this.lblFiltroCarpeta);
            this.pnlFiltros.Controls.Add(this.txtFiltroCarpeta);
            this.pnlFiltros.Controls.Add(this.btnFiltrar);
            this.pnlFiltros.Location = new System.Drawing.Point(12, 120);
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Size = new System.Drawing.Size(1176, 45);
            this.pnlFiltros.TabIndex = 7;
            // 
            // chkFiltroDesde
            // 
            this.chkFiltroDesde.AutoSize = true;
            this.chkFiltroDesde.ForeColor = System.Drawing.Color.White;
            this.chkFiltroDesde.Location = new System.Drawing.Point(10, 10);
            this.chkFiltroDesde.Name = "chkFiltroDesde";
            this.chkFiltroDesde.Size = new System.Drawing.Size(120, 19);
            this.chkFiltroDesde.TabIndex = 0;
            this.chkFiltroDesde.Text = "Desde Fecha Alta";
            this.chkFiltroDesde.UseVisualStyleBackColor = true;
            this.chkFiltroDesde.CheckedChanged += new System.EventHandler(this.chkFiltroDesde_CheckedChanged);
            // 
            // dtpFiltroDesde
            // 
            this.dtpFiltroDesde.Enabled = false;
            this.dtpFiltroDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFiltroDesde.Location = new System.Drawing.Point(150, 8);
            this.dtpFiltroDesde.Name = "dtpFiltroDesde";
            this.dtpFiltroDesde.Size = new System.Drawing.Size(120, 23);
            this.dtpFiltroDesde.TabIndex = 1;
            // 
            // chkFiltroHasta
            // 
            this.chkFiltroHasta.AutoSize = true;
            this.chkFiltroHasta.ForeColor = System.Drawing.Color.White;
            this.chkFiltroHasta.Location = new System.Drawing.Point(290, 10);
            this.chkFiltroHasta.Name = "chkFiltroHasta";
            this.chkFiltroHasta.Size = new System.Drawing.Size(120, 19);
            this.chkFiltroHasta.TabIndex = 2;
            this.chkFiltroHasta.Text = "Hasta Fecha Alta";
            this.chkFiltroHasta.UseVisualStyleBackColor = true;
            this.chkFiltroHasta.CheckedChanged += new System.EventHandler(this.chkFiltroHasta_CheckedChanged);
            // 
            // dtpFiltroHasta
            // 
            this.dtpFiltroHasta.Enabled = false;
            this.dtpFiltroHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFiltroHasta.Location = new System.Drawing.Point(430, 8);
            this.dtpFiltroHasta.Name = "dtpFiltroHasta";
            this.dtpFiltroHasta.Size = new System.Drawing.Size(120, 23);
            this.dtpFiltroHasta.TabIndex = 3;
            // 
            // lblFiltroCarpeta
            // 
            this.lblFiltroCarpeta.AutoSize = true;
            this.lblFiltroCarpeta.ForeColor = System.Drawing.Color.White;
            this.lblFiltroCarpeta.Location = new System.Drawing.Point(570, 12);
            this.lblFiltroCarpeta.Name = "lblFiltroCarpeta";
            this.lblFiltroCarpeta.Size = new System.Drawing.Size(120, 15);
            this.lblFiltroCarpeta.TabIndex = 4;
            this.lblFiltroCarpeta.Text = "Nombre \u00faltima carpeta:";
            // 
            // txtFiltroCarpeta
            // 
            this.txtFiltroCarpeta.Location = new System.Drawing.Point(700, 8);
            this.txtFiltroCarpeta.Name = "txtFiltroCarpeta";
            this.txtFiltroCarpeta.Size = new System.Drawing.Size(200, 23);
            this.txtFiltroCarpeta.TabIndex = 5;
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnFiltrar.FlatAppearance.BorderSize = 0;
            this.btnFiltrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltrar.ForeColor = System.Drawing.Color.White;
            this.btnFiltrar.Location = new System.Drawing.Point(920, 6);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(100, 30);
            this.btnFiltrar.TabIndex = 6;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = false;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // FrmSeparacionImagenes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(1200, 640);
            this.Controls.Add(this.lblProgreso);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.pnlOpciones);
            this.Controls.Add(this.pnlContadores);
            this.Controls.Add(this.pnlBotones);
            this.Controls.Add(this.dgvArchivos);
            this.Controls.Add(this.pnlFiltros);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmSeparacionImagenes";
            this.Text = "Separación de Imágenes";
            this.Load += new System.EventHandler(this.FrmSeparacionImagenes_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArchivos)).EndInit();
            this.pnlBotones.ResumeLayout(false);
            this.pnlContadores.ResumeLayout(false);
            this.pnlContadores.PerformLayout();
            this.pnlOpciones.ResumeLayout(false);
            this.pnlOpciones.PerformLayout();
            this.pnlFiltros.ResumeLayout(false);
            this.pnlFiltros.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnCerrar;
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
    }
}
