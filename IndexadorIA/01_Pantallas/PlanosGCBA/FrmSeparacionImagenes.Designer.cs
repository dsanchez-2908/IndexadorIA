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
            this.chkGirarAutomaticamente = new System.Windows.Forms.CheckBox();
            this.chkMarcaBlanco = new System.Windows.Forms.CheckBox();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgreso = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArchivos)).BeginInit();
            this.pnlBotones.SuspendLayout();
            this.pnlContadores.SuspendLayout();
            this.pnlOpciones.SuspendLayout();
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
            this.dgvArchivos.Location = new System.Drawing.Point(12, 130);
            this.dgvArchivos.Name = "dgvArchivos";
            this.dgvArchivos.RowHeadersVisible = false;
            this.dgvArchivos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvArchivos.Size = new System.Drawing.Size(1176, 350);
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
            this.pnlBotones.Location = new System.Drawing.Point(12, 60);
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
            this.pnlContadores.Location = new System.Drawing.Point(900, 60);
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
            this.pnlOpciones.Controls.Add(this.chkGirarAutomaticamente);
            this.pnlOpciones.Controls.Add(this.chkMarcaBlanco);
            this.pnlOpciones.Controls.Add(this.btnProcesar);
            this.pnlOpciones.Location = new System.Drawing.Point(12, 490);
            this.pnlOpciones.Name = "pnlOpciones";
            this.pnlOpciones.Size = new System.Drawing.Size(700, 80);
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
            // chkGirarAutomaticamente
            // 
            this.chkGirarAutomaticamente.AutoSize = true;
            this.chkGirarAutomaticamente.ForeColor = System.Drawing.Color.White;
            this.chkGirarAutomaticamente.Location = new System.Drawing.Point(10, 35);
            this.chkGirarAutomaticamente.Name = "chkGirarAutomaticamente";
            this.chkGirarAutomaticamente.Size = new System.Drawing.Size(168, 19);
            this.chkGirarAutomaticamente.TabIndex = 1;
            this.chkGirarAutomaticamente.Text = "Girar Automáticamente";
            this.chkGirarAutomaticamente.UseVisualStyleBackColor = true;
            // 
            // chkMarcaBlanco
            // 
            this.chkMarcaBlanco.AutoSize = true;
            this.chkMarcaBlanco.ForeColor = System.Drawing.Color.White;
            this.chkMarcaBlanco.Location = new System.Drawing.Point(250, 10);
            this.chkMarcaBlanco.Name = "chkMarcaBlanco";
            this.chkMarcaBlanco.Size = new System.Drawing.Size(210, 19);
            this.chkMarcaBlanco.TabIndex = 2;
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
            this.btnProcesar.Location = new System.Drawing.Point(550, 10);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(120, 40);
            this.btnProcesar.TabIndex = 3;
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
        private System.Windows.Forms.CheckBox chkGirarAutomaticamente;
        private System.Windows.Forms.CheckBox chkMarcaBlanco;
        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblProgreso;
    }
}
