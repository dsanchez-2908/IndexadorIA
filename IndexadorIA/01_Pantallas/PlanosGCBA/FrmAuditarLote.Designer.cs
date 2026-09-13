namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmAuditarLote
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
            groupBoxFiltros = new GroupBox();
            lblEstadoControl = new Label();
            cboEstadoControl = new ComboBox();
            btnBuscar = new Button();
            btnLimpiarFiltros = new Button();
            groupBoxRegistros = new GroupBox();
            dgvRegistros = new DataGridView();
            lblTotalizadorRegistros = new Label();
            panelAcciones = new Panel();
            btnCerrar = new Button();
            btnMarcarLoteAuditado = new Button();
            btnMostrar = new Button();
            btnAnalizar = new Button();
            groupBoxFiltros.SuspendLayout();
            groupBoxRegistros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRegistros).BeginInit();
            panelAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxFiltros
            // 
            groupBoxFiltros.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxFiltros.Controls.Add(lblEstadoControl);
            groupBoxFiltros.Controls.Add(cboEstadoControl);
            groupBoxFiltros.Controls.Add(btnBuscar);
            groupBoxFiltros.Controls.Add(btnLimpiarFiltros);
            groupBoxFiltros.Dock = DockStyle.Top;
            groupBoxFiltros.ForeColor = Color.White;
            groupBoxFiltros.Location = new Point(0, 0);
            groupBoxFiltros.Margin = new Padding(3, 2, 3, 2);
            groupBoxFiltros.Name = "groupBoxFiltros";
            groupBoxFiltros.Padding = new Padding(3, 2, 3, 2);
            groupBoxFiltros.Size = new Size(1100, 70);
            groupBoxFiltros.TabIndex = 0;
            groupBoxFiltros.TabStop = false;
            groupBoxFiltros.Text = "Filtros";
            // 
            // lblEstadoControl
            // 
            lblEstadoControl.AutoSize = true;
            lblEstadoControl.BackColor = Color.FromArgb(45, 45, 48);
            lblEstadoControl.ForeColor = Color.White;
            lblEstadoControl.Location = new Point(18, 28);
            lblEstadoControl.Name = "lblEstadoControl";
            lblEstadoControl.Size = new Size(97, 15);
            lblEstadoControl.TabIndex = 0;
            lblEstadoControl.Text = "Estado de Control:";
            // 
            // cboEstadoControl
            // 
            cboEstadoControl.BackColor = Color.FromArgb(30, 30, 30);
            cboEstadoControl.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstadoControl.FlatStyle = FlatStyle.Flat;
            cboEstadoControl.ForeColor = Color.White;
            cboEstadoControl.Location = new Point(121, 25);
            cboEstadoControl.Margin = new Padding(3, 2, 3, 2);
            cboEstadoControl.Name = "cboEstadoControl";
            cboEstadoControl.Size = new Size(220, 23);
            cboEstadoControl.TabIndex = 1;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = Color.FromArgb(0, 122, 204);
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(900, 24);
            btnBuscar.Margin = new Padding(3, 2, 3, 2);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(90, 26);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnLimpiarFiltros
            // 
            btnLimpiarFiltros.BackColor = Color.FromArgb(0, 122, 204);
            btnLimpiarFiltros.FlatStyle = FlatStyle.Flat;
            btnLimpiarFiltros.ForeColor = Color.White;
            btnLimpiarFiltros.Location = new Point(996, 24);
            btnLimpiarFiltros.Margin = new Padding(3, 2, 3, 2);
            btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            btnLimpiarFiltros.Size = new Size(90, 26);
            btnLimpiarFiltros.TabIndex = 3;
            btnLimpiarFiltros.Text = "Limpiar";
            btnLimpiarFiltros.UseVisualStyleBackColor = false;
            btnLimpiarFiltros.Click += btnLimpiarFiltros_Click;
            // 
            // groupBoxRegistros
            // 
            groupBoxRegistros.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxRegistros.Controls.Add(dgvRegistros);
            groupBoxRegistros.Controls.Add(lblTotalizadorRegistros);
            groupBoxRegistros.Dock = DockStyle.Fill;
            groupBoxRegistros.ForeColor = Color.White;
            groupBoxRegistros.Location = new Point(0, 70);
            groupBoxRegistros.Margin = new Padding(3, 2, 3, 2);
            groupBoxRegistros.Name = "groupBoxRegistros";
            groupBoxRegistros.Padding = new Padding(9, 24, 9, 8);
            groupBoxRegistros.Size = new Size(1100, 428);
            groupBoxRegistros.TabIndex = 1;
            groupBoxRegistros.TabStop = false;
            groupBoxRegistros.Text = "Registros del Lote";
            // 
            // dgvRegistros
            // 
            dgvRegistros.AllowUserToAddRows = false;
            dgvRegistros.AllowUserToDeleteRows = false;
            dgvRegistros.AllowUserToOrderColumns = false;
            dgvRegistros.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvRegistros.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvRegistros.BorderStyle = BorderStyle.None;
            dgvRegistros.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvRegistros.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRegistros.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRegistros.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvRegistros.DefaultCellStyle.ForeColor = Color.White;
            dgvRegistros.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvRegistros.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvRegistros.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvRegistros.EnableHeadersVisualStyles = false;
            dgvRegistros.GridColor = Color.FromArgb(63, 63, 70);
            dgvRegistros.ReadOnly = true;
            dgvRegistros.MultiSelect = false;
            dgvRegistros.Dock = DockStyle.Fill;
            dgvRegistros.Location = new Point(9, 24);
            dgvRegistros.Margin = new Padding(3, 2, 3, 2);
            dgvRegistros.Name = "dgvRegistros";
            dgvRegistros.RowHeadersWidth = 51;
            dgvRegistros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRegistros.Size = new Size(1082, 366);
            dgvRegistros.TabIndex = 0;
            dgvRegistros.CellFormatting += dgvRegistros_CellFormatting;
            dgvRegistros.CellDoubleClick += dgvRegistros_CellDoubleClick;
            // 
            // lblTotalizadorRegistros
            // 
            lblTotalizadorRegistros.AutoSize = true;
            lblTotalizadorRegistros.BackColor = Color.FromArgb(45, 45, 48);
            lblTotalizadorRegistros.Dock = DockStyle.Bottom;
            lblTotalizadorRegistros.ForeColor = Color.White;
            lblTotalizadorRegistros.Location = new Point(9, 390);
            lblTotalizadorRegistros.Name = "lblTotalizadorRegistros";
            lblTotalizadorRegistros.Padding = new Padding(0, 4, 0, 4);
            lblTotalizadorRegistros.Size = new Size(45, 23);
            lblTotalizadorRegistros.TabIndex = 1;
            lblTotalizadorRegistros.Text = "Total: 0";
            lblTotalizadorRegistros.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelAcciones
            // 
            panelAcciones.BackColor = Color.FromArgb(45, 45, 48);
            panelAcciones.Controls.Add(btnAnalizar);
            panelAcciones.Controls.Add(btnMostrar);
            panelAcciones.Controls.Add(btnMarcarLoteAuditado);
            panelAcciones.Controls.Add(btnCerrar);
            panelAcciones.Dock = DockStyle.Bottom;
            panelAcciones.Location = new Point(0, 498);
            panelAcciones.Margin = new Padding(3, 2, 3, 2);
            panelAcciones.Name = "panelAcciones";
            panelAcciones.Size = new Size(1100, 46);
            panelAcciones.TabIndex = 2;
            // 
            // btnAnalizar
            // 
            btnAnalizar.BackColor = Color.FromArgb(0, 122, 204);
            btnAnalizar.FlatStyle = FlatStyle.Flat;
            btnAnalizar.ForeColor = Color.White;
            btnAnalizar.Location = new Point(18, 8);
            btnAnalizar.Margin = new Padding(3, 2, 3, 2);
            btnAnalizar.Name = "btnAnalizar";
            btnAnalizar.Size = new Size(150, 30);
            btnAnalizar.TabIndex = 0;
            btnAnalizar.Text = "Analizar";
            btnAnalizar.UseVisualStyleBackColor = false;
            btnAnalizar.Click += btnAnalizar_Click;
            // 
            // btnMostrar
            // 
            btnMostrar.BackColor = Color.FromArgb(62, 62, 66);
            btnMostrar.FlatStyle = FlatStyle.Flat;
            btnMostrar.ForeColor = Color.White;
            btnMostrar.Location = new Point(176, 8);
            btnMostrar.Margin = new Padding(3, 2, 3, 2);
            btnMostrar.Name = "btnMostrar";
            btnMostrar.Size = new Size(150, 30);
            btnMostrar.TabIndex = 1;
            btnMostrar.Text = "Mostrar";
            btnMostrar.UseVisualStyleBackColor = false;
            btnMostrar.Click += btnMostrar_Click;
            // 
            // btnMarcarLoteAuditado
            // 
            btnMarcarLoteAuditado.BackColor = Color.FromArgb(0, 150, 80);
            btnMarcarLoteAuditado.FlatStyle = FlatStyle.Flat;
            btnMarcarLoteAuditado.ForeColor = Color.White;
            btnMarcarLoteAuditado.Location = new Point(760, 8);
            btnMarcarLoteAuditado.Margin = new Padding(3, 2, 3, 2);
            btnMarcarLoteAuditado.Name = "btnMarcarLoteAuditado";
            btnMarcarLoteAuditado.Size = new Size(180, 30);
            btnMarcarLoteAuditado.TabIndex = 2;
            btnMarcarLoteAuditado.Text = "Marcar Lote como Auditado";
            btnMarcarLoteAuditado.UseVisualStyleBackColor = false;
            btnMarcarLoteAuditado.Click += btnMarcarLoteAuditado_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.FromArgb(150, 40, 40);
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.Location = new Point(962, 8);
            btnCerrar.Margin = new Padding(3, 2, 3, 2);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(120, 30);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // FrmAuditarLote
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(1100, 544);
            Controls.Add(groupBoxRegistros);
            Controls.Add(panelAcciones);
            Controls.Add(groupBoxFiltros);
            ForeColor = Color.White;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmAuditarLote";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auditar Lote";
            WindowState = FormWindowState.Maximized;
            Load += FrmAuditarLote_Load;
            groupBoxFiltros.ResumeLayout(false);
            groupBoxFiltros.PerformLayout();
            groupBoxRegistros.ResumeLayout(false);
            groupBoxRegistros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRegistros).EndInit();
            panelAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxFiltros;
        private Label lblEstadoControl;
        private ComboBox cboEstadoControl;
        private Button btnBuscar;
        private Button btnLimpiarFiltros;
        private GroupBox groupBoxRegistros;
        private DataGridView dgvRegistros;
        private Label lblTotalizadorRegistros;
        private Panel panelAcciones;
        private Button btnCerrar;
        private Button btnMarcarLoteAuditado;
        private Button btnMostrar;
        private Button btnAnalizar;
    }
}
