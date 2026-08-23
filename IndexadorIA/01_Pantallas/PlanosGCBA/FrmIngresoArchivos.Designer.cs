namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmIngresoArchivos
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
            panelSuperior = new Panel();
            btnCerrar2 = new Button();
            lblTitulo = new Label();
            panelContenido = new Panel();
            panelGrid = new Panel();
            dgvArchivos = new DataGridView();
            panelProgreso = new Panel();
            lblProgreso = new Label();
            progressBar = new ProgressBar();
            panelOpciones = new Panel();
            btnIngresar = new Button();
            btnBuscar = new Button();
            chkIncluirSubcarpetas = new CheckBox();
            btnSeleccionarCarpeta = new Button();
            txtRutaOrigen = new TextBox();
            lblRutaOrigen = new Label();
            cboProyecto = new ComboBox();
            lblProyecto = new Label();
            panelSuperior.SuspendLayout();
            panelContenido.SuspendLayout();
            panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArchivos).BeginInit();
            panelProgreso.SuspendLayout();
            panelOpciones.SuspendLayout();
            SuspendLayout();
            // 
            // panelSuperior
            // 
            panelSuperior.BackColor = Color.FromArgb(45, 45, 48);
            panelSuperior.Controls.Add(btnCerrar2);
            panelSuperior.Controls.Add(lblTitulo);
            panelSuperior.Dock = DockStyle.Top;
            panelSuperior.Location = new Point(0, 0);
            panelSuperior.Margin = new Padding(3, 2, 3, 2);
            panelSuperior.Name = "panelSuperior";
            panelSuperior.Padding = new Padding(18, 15, 18, 15);
            panelSuperior.Size = new Size(875, 60);
            panelSuperior.TabIndex = 0;
            // 
            // btnCerrar2
            // 
            btnCerrar2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar2.BackColor = Color.FromArgb(150, 30, 30);
            btnCerrar2.FlatStyle = FlatStyle.Flat;
            btnCerrar2.ForeColor = Color.White;
            btnCerrar2.Location = new Point(729, 8);
            btnCerrar2.Margin = new Padding(3, 3, 8, 3);
            btnCerrar2.Name = "btnCerrar2";
            btnCerrar2.Size = new Size(120, 34);
            btnCerrar2.TabIndex = 8;
            btnCerrar2.Text = "Cerrar";
            btnCerrar2.UseVisualStyleBackColor = false;
            btnCerrar2.Click += btnCerrar2_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(18, 19);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(218, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Ingreso de Archivos";
            // 
            // panelContenido
            // 
            panelContenido.BackColor = Color.FromArgb(32, 32, 32);
            panelContenido.Controls.Add(panelGrid);
            panelContenido.Controls.Add(panelProgreso);
            panelContenido.Controls.Add(panelOpciones);
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.Location = new Point(0, 60);
            panelContenido.Margin = new Padding(3, 2, 3, 2);
            panelContenido.Name = "panelContenido";
            panelContenido.Padding = new Padding(18, 15, 18, 15);
            panelContenido.Size = new Size(875, 390);
            panelContenido.TabIndex = 1;
            // 
            // panelGrid
            // 
            panelGrid.Controls.Add(dgvArchivos);
            panelGrid.Dock = DockStyle.Fill;
            panelGrid.Location = new Point(18, 150);
            panelGrid.Margin = new Padding(3, 2, 3, 2);
            panelGrid.Name = "panelGrid";
            panelGrid.Size = new Size(839, 180);
            panelGrid.TabIndex = 2;
            // 
            // dgvArchivos
            // 
            dgvArchivos.AllowUserToAddRows = false;
            dgvArchivos.AllowUserToDeleteRows = false;
            dgvArchivos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvArchivos.BackgroundColor = Color.FromArgb(45, 45, 48);
            dgvArchivos.BorderStyle = BorderStyle.None;
            dgvArchivos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArchivos.Dock = DockStyle.Fill;
            dgvArchivos.Location = new Point(0, 0);
            dgvArchivos.Margin = new Padding(3, 2, 3, 2);
            dgvArchivos.Name = "dgvArchivos";
            dgvArchivos.ReadOnly = true;
            dgvArchivos.RowHeadersWidth = 51;
            dgvArchivos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArchivos.Size = new Size(839, 180);
            dgvArchivos.TabIndex = 0;
            // 
            // panelProgreso
            // 
            panelProgreso.Controls.Add(lblProgreso);
            panelProgreso.Controls.Add(progressBar);
            panelProgreso.Dock = DockStyle.Bottom;
            panelProgreso.Location = new Point(18, 330);
            panelProgreso.Margin = new Padding(3, 2, 3, 2);
            panelProgreso.Name = "panelProgreso";
            panelProgreso.Padding = new Padding(0, 8, 0, 0);
            panelProgreso.Size = new Size(839, 45);
            panelProgreso.TabIndex = 3;
            // 
            // lblProgreso
            // 
            lblProgreso.Dock = DockStyle.Top;
            lblProgreso.Font = new Font("Segoe UI", 9F);
            lblProgreso.ForeColor = Color.White;
            lblProgreso.Location = new Point(0, 8);
            lblProgreso.Name = "lblProgreso";
            lblProgreso.Size = new Size(839, 15);
            lblProgreso.TabIndex = 1;
            lblProgreso.TextAlign = ContentAlignment.MiddleLeft;
            lblProgreso.Visible = false;
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Bottom;
            progressBar.Location = new Point(0, 26);
            progressBar.Margin = new Padding(3, 2, 3, 2);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(839, 19);
            progressBar.TabIndex = 0;
            progressBar.Visible = false;
            // 
            // panelOpciones
            // 
            panelOpciones.Controls.Add(btnIngresar);
            panelOpciones.Controls.Add(btnBuscar);
            panelOpciones.Controls.Add(chkIncluirSubcarpetas);
            panelOpciones.Controls.Add(btnSeleccionarCarpeta);
            panelOpciones.Controls.Add(txtRutaOrigen);
            panelOpciones.Controls.Add(lblRutaOrigen);
            panelOpciones.Controls.Add(cboProyecto);
            panelOpciones.Controls.Add(lblProyecto);
            panelOpciones.Dock = DockStyle.Top;
            panelOpciones.Location = new Point(18, 15);
            panelOpciones.Margin = new Padding(3, 2, 3, 2);
            panelOpciones.Name = "panelOpciones";
            panelOpciones.Size = new Size(839, 135);
            panelOpciones.TabIndex = 1;
            // 
            // btnIngresar
            // 
            btnIngresar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnIngresar.BackColor = Color.FromArgb(0, 122, 204);
            btnIngresar.Cursor = Cursors.Hand;
            btnIngresar.Enabled = false;
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnIngresar.ForeColor = Color.White;
            btnIngresar.Location = new Point(712, 89);
            btnIngresar.Margin = new Padding(3, 2, 3, 2);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(114, 30);
            btnIngresar.TabIndex = 7;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = false;
            btnIngresar.Click += btnIngresar_Click;
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBuscar.BackColor = Color.FromArgb(0, 122, 204);
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(712, 47);
            btnBuscar.Margin = new Padding(3, 2, 3, 2);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(114, 30);
            btnBuscar.TabIndex = 6;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // chkIncluirSubcarpetas
            // 
            chkIncluirSubcarpetas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkIncluirSubcarpetas.AutoSize = true;
            chkIncluirSubcarpetas.Font = new Font("Segoe UI", 10F);
            chkIncluirSubcarpetas.ForeColor = Color.White;
            chkIncluirSubcarpetas.Location = new Point(564, 44);
            chkIncluirSubcarpetas.Margin = new Padding(3, 2, 3, 2);
            chkIncluirSubcarpetas.Name = "chkIncluirSubcarpetas";
            chkIncluirSubcarpetas.Size = new Size(142, 23);
            chkIncluirSubcarpetas.TabIndex = 5;
            chkIncluirSubcarpetas.Text = "Incluir subcarpetas";
            chkIncluirSubcarpetas.UseVisualStyleBackColor = true;
            // 
            // btnSeleccionarCarpeta
            // 
            btnSeleccionarCarpeta.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSeleccionarCarpeta.BackColor = Color.FromArgb(62, 62, 66);
            btnSeleccionarCarpeta.Cursor = Cursors.Hand;
            btnSeleccionarCarpeta.FlatAppearance.BorderSize = 0;
            btnSeleccionarCarpeta.FlatStyle = FlatStyle.Flat;
            btnSeleccionarCarpeta.Font = new Font("Segoe UI", 10F);
            btnSeleccionarCarpeta.ForeColor = Color.White;
            btnSeleccionarCarpeta.Location = new Point(712, 9);
            btnSeleccionarCarpeta.Margin = new Padding(3, 2, 3, 2);
            btnSeleccionarCarpeta.Name = "btnSeleccionarCarpeta";
            btnSeleccionarCarpeta.Size = new Size(114, 32);
            btnSeleccionarCarpeta.TabIndex = 4;
            btnSeleccionarCarpeta.Text = "Seleccionar...";
            btnSeleccionarCarpeta.UseVisualStyleBackColor = false;
            btnSeleccionarCarpeta.Click += btnSeleccionarCarpeta_Click;
            // 
            // txtRutaOrigen
            // 
            txtRutaOrigen.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtRutaOrigen.BackColor = Color.FromArgb(62, 62, 66);
            txtRutaOrigen.BorderStyle = BorderStyle.FixedSingle;
            txtRutaOrigen.Font = new Font("Segoe UI", 10F);
            txtRutaOrigen.ForeColor = Color.White;
            txtRutaOrigen.Location = new Point(12, 69);
            txtRutaOrigen.Margin = new Padding(3, 2, 3, 2);
            txtRutaOrigen.Multiline = true;
            txtRutaOrigen.Name = "txtRutaOrigen";
            txtRutaOrigen.ReadOnly = true;
            txtRutaOrigen.Size = new Size(694, 50);
            txtRutaOrigen.TabIndex = 3;
            // 
            // lblRutaOrigen
            // 
            lblRutaOrigen.AutoSize = true;
            lblRutaOrigen.Font = new Font("Segoe UI", 10F);
            lblRutaOrigen.ForeColor = Color.White;
            lblRutaOrigen.Location = new Point(12, 48);
            lblRutaOrigen.Name = "lblRutaOrigen";
            lblRutaOrigen.Size = new Size(119, 19);
            lblRutaOrigen.TabIndex = 2;
            lblRutaOrigen.Text = "Carpeta de origen";
            // 
            // cboProyecto
            // 
            cboProyecto.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboProyecto.BackColor = Color.FromArgb(62, 62, 66);
            cboProyecto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProyecto.FlatStyle = FlatStyle.Flat;
            cboProyecto.Font = new Font("Segoe UI", 10F);
            cboProyecto.ForeColor = Color.White;
            cboProyecto.FormattingEnabled = true;
            cboProyecto.Location = new Point(12, 23);
            cboProyecto.Margin = new Padding(3, 2, 3, 2);
            cboProyecto.Name = "cboProyecto";
            cboProyecto.Size = new Size(349, 25);
            cboProyecto.TabIndex = 1;
            // 
            // lblProyecto
            // 
            lblProyecto.AutoSize = true;
            lblProyecto.Font = new Font("Segoe UI", 10F);
            lblProyecto.ForeColor = Color.White;
            lblProyecto.Location = new Point(12, 1);
            lblProyecto.Name = "lblProyecto";
            lblProyecto.Size = new Size(63, 19);
            lblProyecto.TabIndex = 0;
            lblProyecto.Text = "Proyecto";
            // 
            // FrmIngresoArchivos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(875, 450);
            Controls.Add(panelContenido);
            Controls.Add(panelSuperior);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmIngresoArchivos";
            Text = "Ingreso de Archivos";
            Load += FrmIngresoArchivos_Load;
            panelSuperior.ResumeLayout(false);
            panelSuperior.PerformLayout();
            panelContenido.ResumeLayout(false);
            panelGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvArchivos).EndInit();
            panelProgreso.ResumeLayout(false);
            panelOpciones.ResumeLayout(false);
            panelOpciones.PerformLayout();
            ResumeLayout(false);
        }

        private Panel panelSuperior;
        private Label lblTitulo;
        private Panel panelContenido;
        private Panel panelOpciones;
        private Label lblProyecto;
        private ComboBox cboProyecto;
        private Label lblRutaOrigen;
        private TextBox txtRutaOrigen;
        private Button btnSeleccionarCarpeta;
        private CheckBox chkIncluirSubcarpetas;
        private Button btnBuscar;
        private Button btnIngresar;
        private Panel panelGrid;
        private DataGridView dgvArchivos;
        private Panel panelProgreso;
        private ProgressBar progressBar;
        private Label lblProgreso;
        private Button btnCerrar2;
    }
}
