namespace IndexadorIA.Pantallas.Reportes
{
    partial class FrmAuditoriaUsuarios
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
            lblUsuario = new Label();
            cboUsuarios = new ComboBox();
            lblFechaDesde = new Label();
            dtpFechaDesde = new DateTimePicker();
            lblFechaHasta = new Label();
            dtpFechaHasta = new DateTimePicker();
            chkDetallePorFecha = new CheckBox();
            btnBuscar = new Button();
            groupBoxGrilla = new GroupBox();
            dgvAuditoria = new DataGridView();
            panelAcciones = new Panel();
            btnCerrar = new Button();
            btnExportarExcel = new Button();
            groupBoxFiltros.SuspendLayout();
            groupBoxGrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAuditoria).BeginInit();
            panelAcciones.SuspendLayout();
            SuspendLayout();
            //
            // groupBoxFiltros
            //
            groupBoxFiltros.Controls.Add(lblUsuario);
            groupBoxFiltros.Controls.Add(cboUsuarios);
            groupBoxFiltros.Controls.Add(lblFechaDesde);
            groupBoxFiltros.Controls.Add(dtpFechaDesde);
            groupBoxFiltros.Controls.Add(lblFechaHasta);
            groupBoxFiltros.Controls.Add(dtpFechaHasta);
            groupBoxFiltros.Controls.Add(chkDetallePorFecha);
            groupBoxFiltros.Controls.Add(btnBuscar);
            groupBoxFiltros.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxFiltros.ForeColor = Color.White;
            groupBoxFiltros.Dock = DockStyle.Top;
            groupBoxFiltros.Location = new Point(0, 0);
            groupBoxFiltros.Margin = new Padding(3, 2, 3, 2);
            groupBoxFiltros.Name = "groupBoxFiltros";
            groupBoxFiltros.Padding = new Padding(9, 24, 9, 8);
            groupBoxFiltros.Size = new Size(1010, 90);
            groupBoxFiltros.TabIndex = 0;
            groupBoxFiltros.TabStop = false;
            groupBoxFiltros.Text = "Filtros";
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.ForeColor = Color.Silver;
            lblUsuario.Location = new Point(12, 30);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(53, 15);
            lblUsuario.TabIndex = 0;
            lblUsuario.Text = "Usuario:";
            //
            // cboUsuarios
            //
            cboUsuarios.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUsuarios.FormattingEnabled = true;
            cboUsuarios.Location = new Point(12, 48);
            cboUsuarios.Margin = new Padding(3, 2, 3, 2);
            cboUsuarios.Name = "cboUsuarios";
            cboUsuarios.Size = new Size(260, 23);
            cboUsuarios.TabIndex = 1;
            //
            // lblFechaDesde
            //
            lblFechaDesde.AutoSize = true;
            lblFechaDesde.ForeColor = Color.Silver;
            lblFechaDesde.Location = new Point(290, 30);
            lblFechaDesde.Name = "lblFechaDesde";
            lblFechaDesde.Size = new Size(75, 15);
            lblFechaDesde.TabIndex = 2;
            lblFechaDesde.Text = "Fecha Desde:";
            //
            // dtpFechaDesde
            //
            dtpFechaDesde.Format = DateTimePickerFormat.Short;
            dtpFechaDesde.Location = new Point(290, 48);
            dtpFechaDesde.Margin = new Padding(3, 2, 3, 2);
            dtpFechaDesde.Name = "dtpFechaDesde";
            dtpFechaDesde.Size = new Size(150, 23);
            dtpFechaDesde.TabIndex = 3;
            //
            // lblFechaHasta
            //
            lblFechaHasta.AutoSize = true;
            lblFechaHasta.ForeColor = Color.Silver;
            lblFechaHasta.Location = new Point(456, 30);
            lblFechaHasta.Name = "lblFechaHasta";
            lblFechaHasta.Size = new Size(72, 15);
            lblFechaHasta.TabIndex = 4;
            lblFechaHasta.Text = "Fecha Hasta:";
            //
            // dtpFechaHasta
            //
            dtpFechaHasta.Format = DateTimePickerFormat.Short;
            dtpFechaHasta.Location = new Point(456, 48);
            dtpFechaHasta.Margin = new Padding(3, 2, 3, 2);
            dtpFechaHasta.Name = "dtpFechaHasta";
            dtpFechaHasta.Size = new Size(150, 23);
            dtpFechaHasta.TabIndex = 5;
            //
            // chkDetallePorFecha
            //
            chkDetallePorFecha.AutoSize = true;
            chkDetallePorFecha.ForeColor = Color.Silver;
            chkDetallePorFecha.Location = new Point(622, 30);
            chkDetallePorFecha.Margin = new Padding(3, 2, 3, 2);
            chkDetallePorFecha.Name = "chkDetallePorFecha";
            chkDetallePorFecha.Size = new Size(130, 19);
            chkDetallePorFecha.TabIndex = 6;
            chkDetallePorFecha.Text = "Detalle por Fecha";
            chkDetallePorFecha.UseVisualStyleBackColor = true;
            //
            // btnBuscar
            //
            btnBuscar.BackColor = Color.FromArgb(0, 122, 204);
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(790, 47);
            btnBuscar.Margin = new Padding(3, 2, 3, 2);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(150, 26);
            btnBuscar.TabIndex = 7;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            //
            // groupBoxGrilla
            //
            groupBoxGrilla.Controls.Add(dgvAuditoria);
            groupBoxGrilla.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxGrilla.ForeColor = Color.White;
            groupBoxGrilla.Dock = DockStyle.Fill;
            groupBoxGrilla.Location = new Point(0, 90);
            groupBoxGrilla.Margin = new Padding(3, 2, 3, 2);
            groupBoxGrilla.Name = "groupBoxGrilla";
            groupBoxGrilla.Padding = new Padding(9, 24, 9, 8);
            groupBoxGrilla.Size = new Size(1010, 338);
            groupBoxGrilla.TabIndex = 1;
            groupBoxGrilla.TabStop = false;
            groupBoxGrilla.Text = "Auditoría por Usuario";
            //
            // dgvAuditoria
            //
            dgvAuditoria.AllowUserToAddRows = false;
            dgvAuditoria.AllowUserToDeleteRows = false;
            dgvAuditoria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAuditoria.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvAuditoria.BorderStyle = BorderStyle.None;
            dgvAuditoria.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvAuditoria.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAuditoria.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAuditoria.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvAuditoria.DefaultCellStyle.ForeColor = Color.White;
            dgvAuditoria.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvAuditoria.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvAuditoria.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvAuditoria.EnableHeadersVisualStyles = false;
            dgvAuditoria.GridColor = Color.FromArgb(63, 63, 70);
            dgvAuditoria.ReadOnly = true;
            dgvAuditoria.AllowUserToOrderColumns = false;
            dgvAuditoria.MultiSelect = false;
            dgvAuditoria.Dock = DockStyle.Fill;
            dgvAuditoria.Location = new Point(9, 24);
            dgvAuditoria.Margin = new Padding(3, 2, 3, 2);
            dgvAuditoria.Name = "dgvAuditoria";
            dgvAuditoria.RowHeadersWidth = 51;
            dgvAuditoria.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAuditoria.Size = new Size(992, 306);
            dgvAuditoria.TabIndex = 0;
            //
            // panelAcciones
            //
            panelAcciones.Controls.Add(btnCerrar);
            panelAcciones.Controls.Add(btnExportarExcel);
            panelAcciones.BackColor = Color.FromArgb(45, 45, 48);
            panelAcciones.Dock = DockStyle.Bottom;
            panelAcciones.Location = new Point(0, 428);
            panelAcciones.Margin = new Padding(3, 2, 3, 2);
            panelAcciones.Name = "panelAcciones";
            panelAcciones.Size = new Size(1010, 46);
            panelAcciones.TabIndex = 2;
            //
            // btnCerrar
            //
            btnCerrar.BackColor = Color.FromArgb(96, 96, 96);
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(18, 8);
            btnCerrar.Margin = new Padding(3, 2, 3, 2);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(150, 30);
            btnCerrar.TabIndex = 0;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            //
            // btnExportarExcel
            //
            btnExportarExcel.BackColor = Color.FromArgb(16, 124, 16);
            btnExportarExcel.FlatStyle = FlatStyle.Flat;
            btnExportarExcel.ForeColor = Color.White;
            btnExportarExcel.Location = new Point(180, 8);
            btnExportarExcel.Margin = new Padding(3, 2, 3, 2);
            btnExportarExcel.Name = "btnExportarExcel";
            btnExportarExcel.Size = new Size(150, 30);
            btnExportarExcel.TabIndex = 1;
            btnExportarExcel.Text = "Exportar a Excel";
            btnExportarExcel.UseVisualStyleBackColor = false;
            btnExportarExcel.Click += btnExportarExcel_Click;
            //
            // FrmAuditoriaUsuarios
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(1010, 474);
            Controls.Add(groupBoxGrilla);
            Controls.Add(groupBoxFiltros);
            Controls.Add(panelAcciones);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmAuditoriaUsuarios";
            Text = "Auditoría x Usuario";
            Load += FrmAuditoriaUsuarios_Load;
            groupBoxFiltros.ResumeLayout(false);
            groupBoxFiltros.PerformLayout();
            groupBoxGrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAuditoria).EndInit();
            panelAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxFiltros;
        private Label lblUsuario;
        private ComboBox cboUsuarios;
        private Label lblFechaDesde;
        private DateTimePicker dtpFechaDesde;
        private Label lblFechaHasta;
        private DateTimePicker dtpFechaHasta;
        private CheckBox chkDetallePorFecha;
        private Button btnBuscar;
        private GroupBox groupBoxGrilla;
        private DataGridView dgvAuditoria;
        private Panel panelAcciones;
        private Button btnCerrar;
        private Button btnExportarExcel;
    }
}
