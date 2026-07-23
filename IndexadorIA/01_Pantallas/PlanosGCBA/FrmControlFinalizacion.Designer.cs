namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmControlFinalizacion
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
            lblNombreLote = new Label();
            txtNombreLote = new TextBox();
            lblFechaDesde = new Label();
            dtpFechaDesde = new DateTimePicker();
            lblFechaHasta = new Label();
            dtpFechaHasta = new DateTimePicker();
            chkFiltrarFecha = new CheckBox();
            btnBuscar = new Button();
            btnLimpiarFiltros = new Button();
            groupBoxLotes = new GroupBox();
            dgvLotes = new DataGridView();
            lblTotalizadorLotes = new Label();
            panelAcciones = new Panel();
            btnVerLote = new Button();
            groupBoxFiltros.SuspendLayout();
            groupBoxLotes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLotes).BeginInit();
            panelAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxFiltros
            // 
            groupBoxFiltros.Controls.Add(lblNombreLote);
            groupBoxFiltros.Controls.Add(txtNombreLote);
            groupBoxFiltros.Controls.Add(lblFechaDesde);
            groupBoxFiltros.Controls.Add(dtpFechaDesde);
            groupBoxFiltros.Controls.Add(lblFechaHasta);
            groupBoxFiltros.Controls.Add(dtpFechaHasta);
            groupBoxFiltros.Controls.Add(chkFiltrarFecha);
            groupBoxFiltros.Controls.Add(btnBuscar);
            groupBoxFiltros.Controls.Add(btnLimpiarFiltros);
            groupBoxFiltros.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxFiltros.ForeColor = Color.White;
            groupBoxFiltros.Dock = DockStyle.Top;
            groupBoxFiltros.Location = new Point(0, 0);
            groupBoxFiltros.Margin = new Padding(3, 2, 3, 2);
            groupBoxFiltros.Name = "groupBoxFiltros";
            groupBoxFiltros.Padding = new Padding(3, 2, 3, 2);
            groupBoxFiltros.Size = new Size(957, 70);
            groupBoxFiltros.TabIndex = 0;
            groupBoxFiltros.TabStop = false;
            groupBoxFiltros.Text = "Filtros";
            // 
            // lblNombreLote
            // 
            lblNombreLote.AutoSize = true;
            lblNombreLote.BackColor = Color.FromArgb(45, 45, 48);
            lblNombreLote.ForeColor = Color.White;
            lblNombreLote.Location = new Point(18, 28);
            lblNombreLote.Name = "lblNombreLote";
            lblNombreLote.Size = new Size(90, 15);
            lblNombreLote.TabIndex = 0;
            lblNombreLote.Text = "Nombre de Lote:";
            // 
            // txtNombreLote
            // 
            txtNombreLote.BackColor = Color.FromArgb(30, 30, 30);
            txtNombreLote.ForeColor = Color.White;
            txtNombreLote.BorderStyle = BorderStyle.FixedSingle;
            txtNombreLote.Location = new Point(114, 25);
            txtNombreLote.Margin = new Padding(3, 2, 3, 2);
            txtNombreLote.Name = "txtNombreLote";
            txtNombreLote.Size = new Size(180, 23);
            txtNombreLote.TabIndex = 1;
            // 
            // lblFechaDesde
            // 
            lblFechaDesde.AutoSize = true;
            lblFechaDesde.BackColor = Color.FromArgb(45, 45, 48);
            lblFechaDesde.ForeColor = Color.White;
            lblFechaDesde.Location = new Point(320, 28);
            lblFechaDesde.Name = "lblFechaDesde";
            lblFechaDesde.Size = new Size(75, 15);
            lblFechaDesde.TabIndex = 2;
            lblFechaDesde.Text = "Fecha Desde:";
            // 
            // dtpFechaDesde
            // 
            dtpFechaDesde.CalendarMonthBackground = Color.FromArgb(45, 45, 48);
            dtpFechaDesde.CalendarForeColor = Color.White;
            dtpFechaDesde.Format = DateTimePickerFormat.Short;
            dtpFechaDesde.Location = new Point(401, 25);
            dtpFechaDesde.Margin = new Padding(3, 2, 3, 2);
            dtpFechaDesde.Name = "dtpFechaDesde";
            dtpFechaDesde.Size = new Size(120, 23);
            dtpFechaDesde.TabIndex = 3;
            // 
            // lblFechaHasta
            // 
            lblFechaHasta.AutoSize = true;
            lblFechaHasta.BackColor = Color.FromArgb(45, 45, 48);
            lblFechaHasta.ForeColor = Color.White;
            lblFechaHasta.Location = new Point(535, 28);
            lblFechaHasta.Name = "lblFechaHasta";
            lblFechaHasta.Size = new Size(70, 15);
            lblFechaHasta.TabIndex = 4;
            lblFechaHasta.Text = "Fecha Hasta:";
            // 
            // dtpFechaHasta
            // 
            dtpFechaHasta.CalendarMonthBackground = Color.FromArgb(45, 45, 48);
            dtpFechaHasta.CalendarForeColor = Color.White;
            dtpFechaHasta.Format = DateTimePickerFormat.Short;
            dtpFechaHasta.Location = new Point(611, 25);
            dtpFechaHasta.Margin = new Padding(3, 2, 3, 2);
            dtpFechaHasta.Name = "dtpFechaHasta";
            dtpFechaHasta.Size = new Size(120, 23);
            dtpFechaHasta.TabIndex = 5;
            // 
            // chkFiltrarFecha
            // 
            chkFiltrarFecha.AutoSize = true;
            chkFiltrarFecha.ForeColor = Color.White;
            chkFiltrarFecha.Location = new Point(114, 4);
            chkFiltrarFecha.Name = "chkFiltrarFecha";
            chkFiltrarFecha.Size = new Size(133, 19);
            chkFiltrarFecha.TabIndex = 6;
            chkFiltrarFecha.Text = "Filtrar por fecha de alta";
            chkFiltrarFecha.UseVisualStyleBackColor = true;
            chkFiltrarFecha.CheckedChanged += chkFiltrarFecha_CheckedChanged;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = Color.FromArgb(0, 122, 204);
            btnBuscar.ForeColor = Color.White;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Location = new Point(750, 24);
            btnBuscar.Margin = new Padding(3, 2, 3, 2);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(90, 26);
            btnBuscar.TabIndex = 7;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnLimpiarFiltros
            // 
            btnLimpiarFiltros.BackColor = Color.FromArgb(0, 122, 204);
            btnLimpiarFiltros.ForeColor = Color.White;
            btnLimpiarFiltros.FlatStyle = FlatStyle.Flat;
            btnLimpiarFiltros.Location = new Point(846, 24);
            btnLimpiarFiltros.Margin = new Padding(3, 2, 3, 2);
            btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            btnLimpiarFiltros.Size = new Size(90, 26);
            btnLimpiarFiltros.TabIndex = 8;
            btnLimpiarFiltros.Text = "Limpiar";
            btnLimpiarFiltros.UseVisualStyleBackColor = false;
            btnLimpiarFiltros.Click += btnLimpiarFiltros_Click;
            // 
            // groupBoxLotes
            // 
            groupBoxLotes.Controls.Add(dgvLotes);
            groupBoxLotes.Controls.Add(lblTotalizadorLotes);
            groupBoxLotes.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxLotes.ForeColor = Color.White;
            groupBoxLotes.Dock = DockStyle.Fill;
            groupBoxLotes.Location = new Point(0, 70);
            groupBoxLotes.Margin = new Padding(3, 2, 3, 2);
            groupBoxLotes.Name = "groupBoxLotes";
            groupBoxLotes.Padding = new Padding(9, 24, 9, 8);
            groupBoxLotes.Size = new Size(957, 358);
            groupBoxLotes.TabIndex = 1;
            groupBoxLotes.TabStop = false;
            groupBoxLotes.Text = "Lotes Pendientes de Control y Finalización";
            // 
            // dgvLotes
            // 
            dgvLotes.AllowUserToAddRows = false;
            dgvLotes.AllowUserToDeleteRows = false;
            dgvLotes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLotes.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvLotes.BorderStyle = BorderStyle.None;
            dgvLotes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvLotes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLotes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLotes.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvLotes.DefaultCellStyle.ForeColor = Color.White;
            dgvLotes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvLotes.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvLotes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvLotes.EnableHeadersVisualStyles = false;
            dgvLotes.GridColor = Color.FromArgb(63, 63, 70);
            dgvLotes.ReadOnly = true;
            dgvLotes.AllowUserToOrderColumns = false;
            dgvLotes.MultiSelect = false;
            dgvLotes.Dock = DockStyle.Fill;
            dgvLotes.Location = new Point(9, 24);
            dgvLotes.Margin = new Padding(3, 2, 3, 2);
            dgvLotes.Name = "dgvLotes";
            dgvLotes.RowHeadersWidth = 51;
            dgvLotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLotes.Size = new Size(939, 296);
            dgvLotes.TabIndex = 0;
            dgvLotes.SelectionChanged += dgvLotes_SelectionChanged;
            dgvLotes.CellDoubleClick += dgvLotes_CellDoubleClick;
            // 
            // lblTotalizadorLotes
            // 
            lblTotalizadorLotes.AutoSize = true;
            lblTotalizadorLotes.BackColor = Color.FromArgb(45, 45, 48);
            lblTotalizadorLotes.ForeColor = Color.White;
            lblTotalizadorLotes.Dock = DockStyle.Bottom;
            lblTotalizadorLotes.Location = new Point(9, 320);
            lblTotalizadorLotes.Name = "lblTotalizadorLotes";
            lblTotalizadorLotes.Size = new Size(60, 15);
            lblTotalizadorLotes.TabIndex = 1;
            lblTotalizadorLotes.Text = "Total: 0";
            lblTotalizadorLotes.TextAlign = ContentAlignment.MiddleLeft;
            lblTotalizadorLotes.Padding = new Padding(0, 4, 0, 4);
            // 
            // panelAcciones
            // 
            panelAcciones.Controls.Add(btnVerLote);
            panelAcciones.BackColor = Color.FromArgb(45, 45, 48);
            panelAcciones.Dock = DockStyle.Bottom;
            panelAcciones.Location = new Point(0, 428);
            panelAcciones.Margin = new Padding(3, 2, 3, 2);
            panelAcciones.Name = "panelAcciones";
            panelAcciones.Size = new Size(957, 46);
            panelAcciones.TabIndex = 2;
            // 
            // btnVerLote
            // 
            btnVerLote.BackColor = Color.FromArgb(0, 122, 204);
            btnVerLote.ForeColor = Color.White;
            btnVerLote.FlatStyle = FlatStyle.Flat;
            btnVerLote.Location = new Point(18, 8);
            btnVerLote.Margin = new Padding(3, 2, 3, 2);
            btnVerLote.Name = "btnVerLote";
            btnVerLote.Size = new Size(150, 30);
            btnVerLote.TabIndex = 0;
            btnVerLote.Text = "Ver Lote";
            btnVerLote.UseVisualStyleBackColor = false;
            btnVerLote.Enabled = false;
            btnVerLote.Click += btnVerLote_Click;
            // 
            // FrmControlFinalizacion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ForeColor = Color.White;
            ClientSize = new Size(957, 474);
            Controls.Add(groupBoxLotes);
            Controls.Add(panelAcciones);
            Controls.Add(groupBoxFiltros);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmControlFinalizacion";
            Text = "Control y Finalización";
            Load += FrmControlFinalizacion_Load;
            groupBoxFiltros.ResumeLayout(false);
            groupBoxFiltros.PerformLayout();
            groupBoxLotes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLotes).EndInit();
            panelAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxFiltros;
        private Label lblNombreLote;
        private TextBox txtNombreLote;
        private Label lblFechaDesde;
        private DateTimePicker dtpFechaDesde;
        private Label lblFechaHasta;
        private DateTimePicker dtpFechaHasta;
        private CheckBox chkFiltrarFecha;
        private Button btnBuscar;
        private Button btnLimpiarFiltros;
        private GroupBox groupBoxLotes;
        private DataGridView dgvLotes;
        private Label lblTotalizadorLotes;
        private Panel panelAcciones;
        private Button btnVerLote;
    }
}
