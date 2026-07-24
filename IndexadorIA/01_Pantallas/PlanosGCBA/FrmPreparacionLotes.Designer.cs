namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmPreparacionLotes
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
            panelFiltros = new Panel();
            txtFiltroArchivo = new TextBox();
            lblFiltroArchivo = new Label();
            txtFiltroCarpeta = new TextBox();
            lblFiltroCarpeta = new Label();
            dtpFeAltaDesde = new DateTimePicker();
            lblFeAltaDesde = new Label();
            dtpFeAltaHasta = new DateTimePicker();
            lblFeAltaHasta = new Label();
            btnBuscar = new Button();
            btnLimpiarFiltros = new Button();
            btnCerrar = new Button();
            dgvArchivos = new DataGridView();
            panelAcciones = new Panel();
            btnSeleccionarTodo = new Button();
            btnDeseleccionarTodo = new Button();
            btnSeleccionar50 = new Button();
            txtArchivosPorLote = new TextBox();
            lblArchivosPorLote = new Label();
            btnCrearLotes = new Button();
            lblInfo = new Label();
            panelFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArchivos).BeginInit();
            panelAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // panelFiltros
            // 
            panelFiltros.BackColor = Color.FromArgb(45, 45, 48);
            panelFiltros.Controls.Add(txtFiltroArchivo);
            panelFiltros.Controls.Add(lblFiltroArchivo);
            panelFiltros.Controls.Add(txtFiltroCarpeta);
            panelFiltros.Controls.Add(lblFiltroCarpeta);
            panelFiltros.Controls.Add(dtpFeAltaDesde);
            panelFiltros.Controls.Add(lblFeAltaDesde);
            panelFiltros.Controls.Add(dtpFeAltaHasta);
            panelFiltros.Controls.Add(lblFeAltaHasta);
            panelFiltros.Controls.Add(btnBuscar);
            panelFiltros.Controls.Add(btnLimpiarFiltros);
            panelFiltros.Controls.Add(btnCerrar);
            panelFiltros.Dock = DockStyle.Top;
            panelFiltros.Location = new Point(0, 0);
            panelFiltros.Name = "panelFiltros";
            panelFiltros.Padding = new Padding(10);
            panelFiltros.Size = new Size(1400, 100);
            panelFiltros.TabIndex = 0;
            // 
            // txtFiltroArchivo
            // 
            txtFiltroArchivo.Location = new Point(150, 20);
            txtFiltroArchivo.Name = "txtFiltroArchivo";
            txtFiltroArchivo.Size = new Size(300, 27);
            txtFiltroArchivo.TabIndex = 1;
            // 
            // lblFiltroArchivo
            // 
            lblFiltroArchivo.AutoSize = true;
            lblFiltroArchivo.ForeColor = Color.White;
            lblFiltroArchivo.Location = new Point(20, 23);
            lblFiltroArchivo.Name = "lblFiltroArchivo";
            lblFiltroArchivo.Size = new Size(124, 20);
            lblFiltroArchivo.TabIndex = 0;
            lblFiltroArchivo.Text = "Nombre Archivo:";
            // 
            // txtFiltroCarpeta
            // 
            txtFiltroCarpeta.Location = new Point(650, 60);
            txtFiltroCarpeta.Name = "txtFiltroCarpeta";
            txtFiltroCarpeta.Size = new Size(200, 27);
            txtFiltroCarpeta.TabIndex = 6;
            // 
            // lblFiltroCarpeta
            // 
            lblFiltroCarpeta.AutoSize = true;
            lblFiltroCarpeta.ForeColor = Color.White;
            lblFiltroCarpeta.Location = new Point(570, 63);
            lblFiltroCarpeta.Name = "lblFiltroCarpeta";
            lblFiltroCarpeta.Size = new Size(70, 20);
            lblFiltroCarpeta.TabIndex = 0;
            lblFiltroCarpeta.Text = "Carpeta:";
            // 
            // dtpFeAltaDesde
            // 
            dtpFeAltaDesde.Format = DateTimePickerFormat.Short;
            dtpFeAltaDesde.Location = new Point(150, 60);
            dtpFeAltaDesde.Name = "dtpFeAltaDesde";
            dtpFeAltaDesde.Size = new Size(130, 27);
            dtpFeAltaDesde.TabIndex = 3;
            // 
            // lblFeAltaDesde
            // 
            lblFeAltaDesde.AutoSize = true;
            lblFeAltaDesde.ForeColor = Color.White;
            lblFeAltaDesde.Location = new Point(20, 63);
            lblFeAltaDesde.Name = "lblFeAltaDesde";
            lblFeAltaDesde.Size = new Size(88, 20);
            lblFeAltaDesde.TabIndex = 2;
            lblFeAltaDesde.Text = "Fecha Desde:";
            // 
            // dtpFeAltaHasta
            // 
            dtpFeAltaHasta.Format = DateTimePickerFormat.Short;
            dtpFeAltaHasta.Location = new Point(390, 60);
            dtpFeAltaHasta.Name = "dtpFeAltaHasta";
            dtpFeAltaHasta.Size = new Size(130, 27);
            dtpFeAltaHasta.TabIndex = 5;
            // 
            // lblFeAltaHasta
            // 
            lblFeAltaHasta.AutoSize = true;
            lblFeAltaHasta.ForeColor = Color.White;
            lblFeAltaHasta.Location = new Point(300, 63);
            lblFeAltaHasta.Name = "lblFeAltaHasta";
            lblFeAltaHasta.Size = new Size(84, 20);
            lblFeAltaHasta.TabIndex = 4;
            lblFeAltaHasta.Text = "Fecha Hasta:";
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = Color.FromArgb(0, 122, 204);
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(950, 20);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(100, 35);
            btnBuscar.TabIndex = 7;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnLimpiarFiltros
            // 
            btnLimpiarFiltros.BackColor = Color.FromArgb(60, 60, 63);
            btnLimpiarFiltros.FlatAppearance.BorderSize = 0;
            btnLimpiarFiltros.FlatStyle = FlatStyle.Flat;
            btnLimpiarFiltros.ForeColor = Color.White;
            btnLimpiarFiltros.Location = new Point(1060, 20);
            btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            btnLimpiarFiltros.Size = new Size(100, 35);
            btnLimpiarFiltros.TabIndex = 8;
            btnLimpiarFiltros.Text = "Limpiar";
            btnLimpiarFiltros.UseVisualStyleBackColor = false;
            btnLimpiarFiltros.Click += btnLimpiarFiltros_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.FromArgb(150, 40, 40);
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(1170, 20);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 35);
            btnCerrar.TabIndex = 9;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // dgvArchivos
            // 
            dgvArchivos.AllowUserToAddRows = false;
            dgvArchivos.AllowUserToDeleteRows = false;
            dgvArchivos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvArchivos.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvArchivos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArchivos.Dock = DockStyle.Fill;
            dgvArchivos.Location = new Point(0, 100);
            dgvArchivos.Name = "dgvArchivos";
            dgvArchivos.RowHeadersWidth = 51;
            dgvArchivos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArchivos.Size = new Size(1400, 450);
            dgvArchivos.TabIndex = 1;
            // 
            // panelAcciones
            // 
            panelAcciones.BackColor = Color.FromArgb(45, 45, 48);
            panelAcciones.Controls.Add(btnSeleccionarTodo);
            panelAcciones.Controls.Add(btnDeseleccionarTodo);
            panelAcciones.Controls.Add(btnSeleccionar50);
            panelAcciones.Controls.Add(lblArchivosPorLote);
            panelAcciones.Controls.Add(txtArchivosPorLote);
            panelAcciones.Controls.Add(btnCrearLotes);
            panelAcciones.Controls.Add(lblInfo);
            panelAcciones.Dock = DockStyle.Bottom;
            panelAcciones.Location = new Point(0, 550);
            panelAcciones.Name = "panelAcciones";
            panelAcciones.Padding = new Padding(10);
            panelAcciones.Size = new Size(1400, 80);
            panelAcciones.TabIndex = 2;
            // 
            // btnSeleccionarTodo
            // 
            btnSeleccionarTodo.BackColor = Color.FromArgb(60, 60, 63);
            btnSeleccionarTodo.FlatAppearance.BorderSize = 0;
            btnSeleccionarTodo.FlatStyle = FlatStyle.Flat;
            btnSeleccionarTodo.ForeColor = Color.White;
            btnSeleccionarTodo.Location = new Point(20, 20);
            btnSeleccionarTodo.Name = "btnSeleccionarTodo";
            btnSeleccionarTodo.Size = new Size(150, 40);
            btnSeleccionarTodo.TabIndex = 0;
            btnSeleccionarTodo.Text = "Seleccionar Todo";
            btnSeleccionarTodo.UseVisualStyleBackColor = false;
            btnSeleccionarTodo.Click += btnSeleccionarTodo_Click;
            // 
            // btnDeseleccionarTodo
            // 
            btnDeseleccionarTodo.BackColor = Color.FromArgb(60, 60, 63);
            btnDeseleccionarTodo.FlatAppearance.BorderSize = 0;
            btnDeseleccionarTodo.FlatStyle = FlatStyle.Flat;
            btnDeseleccionarTodo.ForeColor = Color.White;
            btnDeseleccionarTodo.Location = new Point(180, 20);
            btnDeseleccionarTodo.Name = "btnDeseleccionarTodo";
            btnDeseleccionarTodo.Size = new Size(170, 40);
            btnDeseleccionarTodo.TabIndex = 1;
            btnDeseleccionarTodo.Text = "Deseleccionar Todo";
            btnDeseleccionarTodo.UseVisualStyleBackColor = false;
            btnDeseleccionarTodo.Click += btnDeseleccionarTodo_Click;
            // 
            // btnSeleccionar50
            // 
            btnSeleccionar50.BackColor = Color.FromArgb(60, 60, 63);
            btnSeleccionar50.FlatAppearance.BorderSize = 0;
            btnSeleccionar50.FlatStyle = FlatStyle.Flat;
            btnSeleccionar50.ForeColor = Color.White;
            btnSeleccionar50.Location = new Point(360, 20);
            btnSeleccionar50.Name = "btnSeleccionar50";
            btnSeleccionar50.Size = new Size(130, 40);
            btnSeleccionar50.TabIndex = 2;
            btnSeleccionar50.Text = "Seleccionar 50";
            btnSeleccionar50.UseVisualStyleBackColor = false;
            btnSeleccionar50.Click += btnSeleccionar50_Click;
            // 
            // lblArchivosPorLote
            // 
            lblArchivosPorLote.AutoSize = true;
            lblArchivosPorLote.ForeColor = Color.White;
            lblArchivosPorLote.Location = new Point(520, 30);
            lblArchivosPorLote.Name = "lblArchivosPorLote";
            lblArchivosPorLote.Size = new Size(134, 20);
            lblArchivosPorLote.TabIndex = 3;
            lblArchivosPorLote.Text = "Archivos por Lote:";
            // 
            // txtArchivosPorLote
            // 
            txtArchivosPorLote.Location = new Point(660, 27);
            txtArchivosPorLote.Name = "txtArchivosPorLote";
            txtArchivosPorLote.Size = new Size(80, 27);
            txtArchivosPorLote.TabIndex = 4;
            txtArchivosPorLote.Text = "100";
            txtArchivosPorLote.TextAlign = HorizontalAlignment.Center;
            // 
            // btnCrearLotes
            // 
            btnCrearLotes.BackColor = Color.FromArgb(0, 122, 204);
            btnCrearLotes.FlatAppearance.BorderSize = 0;
            btnCrearLotes.FlatStyle = FlatStyle.Flat;
            btnCrearLotes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCrearLotes.ForeColor = Color.White;
            btnCrearLotes.Location = new Point(770, 20);
            btnCrearLotes.Name = "btnCrearLotes";
            btnCrearLotes.Size = new Size(150, 40);
            btnCrearLotes.TabIndex = 5;
            btnCrearLotes.Text = "Crear Lotes";
            btnCrearLotes.UseVisualStyleBackColor = false;
            btnCrearLotes.Click += btnCrearLotes_Click;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.ForeColor = Color.White;
            lblInfo.Location = new Point(950, 30);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(0, 20);
            lblInfo.TabIndex = 6;
            // 
            // FrmPreparacionLotes
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1400, 630);
            Controls.Add(dgvArchivos);
            Controls.Add(panelAcciones);
            Controls.Add(panelFiltros);
            Name = "FrmPreparacionLotes";
            Text = "Preparación de Lotes";
            Load += FrmPreparacionLotes_Load;
            panelFiltros.ResumeLayout(false);
            panelFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArchivos).EndInit();
            panelAcciones.ResumeLayout(false);
            panelAcciones.PerformLayout();
            ResumeLayout(false);
        }

        private Panel panelFiltros;
        private TextBox txtFiltroArchivo;
        private Label lblFiltroArchivo;
        private TextBox txtFiltroCarpeta;
        private Label lblFiltroCarpeta;
        private DateTimePicker dtpFeAltaDesde;
        private Label lblFeAltaDesde;
        private DateTimePicker dtpFeAltaHasta;
        private Label lblFeAltaHasta;
        private Button btnCerrar;
        private Button btnBuscar;
        private Button btnLimpiarFiltros;
        private DataGridView dgvArchivos;
        private Panel panelAcciones;
        private Button btnSeleccionarTodo;
        private Button btnDeseleccionarTodo;
        private Button btnSeleccionar50;
        private Label lblArchivosPorLote;
        private TextBox txtArchivosPorLote;
        private Button btnCrearLotes;
        private Label lblInfo;
    }
}
