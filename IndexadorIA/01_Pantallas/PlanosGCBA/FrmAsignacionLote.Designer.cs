namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmAsignacionLote
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
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
            btnCerrar = new Button();
            btnAsignarLotes = new Button();
            cboUsuarios = new ComboBox();
            lblAsignarAUsuarios = new Label();
            btnSeleccionarTodos = new Button();
            btnDeseleccionarTodo = new Button();
            btnSeleccionar10 = new Button();
            groupBoxFiltros.SuspendLayout();
            groupBoxLotes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLotes).BeginInit();
            panelAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxFiltros
            // 
            groupBoxFiltros.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxFiltros.Controls.Add(lblNombreLote);
            groupBoxFiltros.Controls.Add(txtNombreLote);
            groupBoxFiltros.Controls.Add(lblFechaDesde);
            groupBoxFiltros.Controls.Add(dtpFechaDesde);
            groupBoxFiltros.Controls.Add(lblFechaHasta);
            groupBoxFiltros.Controls.Add(dtpFechaHasta);
            groupBoxFiltros.Controls.Add(chkFiltrarFecha);
            groupBoxFiltros.Controls.Add(btnBuscar);
            groupBoxFiltros.Controls.Add(btnLimpiarFiltros);
            groupBoxFiltros.Dock = DockStyle.Top;
            groupBoxFiltros.ForeColor = Color.White;
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
            lblNombreLote.Location = new Point(12, 18);
            lblNombreLote.Name = "lblNombreLote";
            lblNombreLote.Size = new Size(96, 15);
            lblNombreLote.TabIndex = 0;
            lblNombreLote.Text = "Nombre de Lote:";
            // 
            // txtNombreLote
            // 
            txtNombreLote.BackColor = Color.FromArgb(30, 30, 30);
            txtNombreLote.BorderStyle = BorderStyle.FixedSingle;
            txtNombreLote.ForeColor = Color.White;
            txtNombreLote.Location = new Point(12, 35);
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
            lblFechaDesde.Location = new Point(351, 18);
            lblFechaDesde.Name = "lblFechaDesde";
            lblFechaDesde.Size = new Size(76, 15);
            lblFechaDesde.TabIndex = 2;
            lblFechaDesde.Text = "Fecha Desde:";
            // 
            // dtpFechaDesde
            // 
            dtpFechaDesde.CalendarForeColor = Color.White;
            dtpFechaDesde.CalendarMonthBackground = Color.FromArgb(45, 45, 48);
            dtpFechaDesde.Format = DateTimePickerFormat.Short;
            dtpFechaDesde.Location = new Point(351, 35);
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
            lblFechaHasta.Location = new Point(477, 18);
            lblFechaHasta.Name = "lblFechaHasta";
            lblFechaHasta.Size = new Size(74, 15);
            lblFechaHasta.TabIndex = 4;
            lblFechaHasta.Text = "Fecha Hasta:";
            // 
            // dtpFechaHasta
            // 
            dtpFechaHasta.CalendarForeColor = Color.White;
            dtpFechaHasta.CalendarMonthBackground = Color.FromArgb(45, 45, 48);
            dtpFechaHasta.Format = DateTimePickerFormat.Short;
            dtpFechaHasta.Location = new Point(477, 35);
            dtpFechaHasta.Margin = new Padding(3, 2, 3, 2);
            dtpFechaHasta.Name = "dtpFechaHasta";
            dtpFechaHasta.Size = new Size(120, 23);
            dtpFechaHasta.TabIndex = 5;
            // 
            // chkFiltrarFecha
            // 
            chkFiltrarFecha.AutoSize = true;
            chkFiltrarFecha.ForeColor = Color.White;
            chkFiltrarFecha.Location = new Point(198, 35);
            chkFiltrarFecha.Name = "chkFiltrarFecha";
            chkFiltrarFecha.Size = new Size(147, 19);
            chkFiltrarFecha.TabIndex = 6;
            chkFiltrarFecha.Text = "Filtrar por fecha de alta";
            chkFiltrarFecha.UseVisualStyleBackColor = true;
            chkFiltrarFecha.CheckedChanged += chkFiltrarFecha_CheckedChanged;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = Color.FromArgb(0, 122, 204);
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.White;
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
            btnLimpiarFiltros.FlatStyle = FlatStyle.Flat;
            btnLimpiarFiltros.ForeColor = Color.White;
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
            groupBoxLotes.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxLotes.Controls.Add(dgvLotes);
            groupBoxLotes.Controls.Add(lblTotalizadorLotes);
            groupBoxLotes.Dock = DockStyle.Fill;
            groupBoxLotes.ForeColor = Color.White;
            groupBoxLotes.Location = new Point(0, 70);
            groupBoxLotes.Margin = new Padding(3, 2, 3, 2);
            groupBoxLotes.Name = "groupBoxLotes";
            groupBoxLotes.Padding = new Padding(9, 24, 9, 8);
            groupBoxLotes.Size = new Size(957, 328);
            groupBoxLotes.TabIndex = 1;
            groupBoxLotes.TabStop = false;
            groupBoxLotes.Text = "Lotes Procesados por IA (pendientes de asignar)";
            // 
            // dgvLotes
            // 
            dgvLotes.AllowUserToAddRows = false;
            dgvLotes.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(40, 40, 40);
            dgvLotes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvLotes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLotes.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvLotes.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(60, 60, 60);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.White;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvLotes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvLotes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(30, 30, 30);
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = Color.White;
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dataGridViewCellStyle6.SelectionForeColor = Color.White;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvLotes.DefaultCellStyle = dataGridViewCellStyle6;
            dgvLotes.Dock = DockStyle.Fill;
            dgvLotes.EnableHeadersVisualStyles = false;
            dgvLotes.GridColor = Color.FromArgb(63, 63, 70);
            dgvLotes.Location = new Point(9, 40);
            dgvLotes.Margin = new Padding(3, 2, 3, 2);
            dgvLotes.MultiSelect = false;
            dgvLotes.Name = "dgvLotes";
            dgvLotes.RowHeadersWidth = 51;
            dgvLotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLotes.Size = new Size(939, 257);
            dgvLotes.TabIndex = 0;
            dgvLotes.CellContentClick += dgvLotes_CellContentClick;
            // 
            // lblTotalizadorLotes
            // 
            lblTotalizadorLotes.AutoSize = true;
            lblTotalizadorLotes.BackColor = Color.FromArgb(45, 45, 48);
            lblTotalizadorLotes.Dock = DockStyle.Bottom;
            lblTotalizadorLotes.ForeColor = Color.White;
            lblTotalizadorLotes.Location = new Point(9, 297);
            lblTotalizadorLotes.Name = "lblTotalizadorLotes";
            lblTotalizadorLotes.Padding = new Padding(0, 4, 0, 4);
            lblTotalizadorLotes.Size = new Size(45, 23);
            lblTotalizadorLotes.TabIndex = 1;
            lblTotalizadorLotes.Text = "Total: 0";
            lblTotalizadorLotes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelAcciones
            // 
            panelAcciones.BackColor = Color.FromArgb(45, 45, 48);
            panelAcciones.Controls.Add(btnSeleccionarTodos);
            panelAcciones.Controls.Add(btnDeseleccionarTodo);
            panelAcciones.Controls.Add(btnSeleccionar10);
            panelAcciones.Controls.Add(lblAsignarAUsuarios);
            panelAcciones.Controls.Add(cboUsuarios);
            panelAcciones.Controls.Add(btnAsignarLotes);
            panelAcciones.Controls.Add(btnCerrar);
            panelAcciones.Dock = DockStyle.Bottom;
            panelAcciones.Location = new Point(0, 398);
            panelAcciones.Margin = new Padding(3, 2, 3, 2);
            panelAcciones.Name = "panelAcciones";
            panelAcciones.Size = new Size(957, 46);
            panelAcciones.TabIndex = 2;
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.FromArgb(150, 40, 40);
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.Location = new Point(819, 8);
            btnCerrar.Margin = new Padding(3, 2, 3, 2);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(120, 30);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // btnAsignarLotes
            // 
            btnAsignarLotes.BackColor = Color.FromArgb(0, 122, 204);
            btnAsignarLotes.FlatStyle = FlatStyle.Flat;
            btnAsignarLotes.ForeColor = Color.White;
            btnAsignarLotes.Location = new Point(674, 8);
            btnAsignarLotes.Margin = new Padding(3, 2, 3, 2);
            btnAsignarLotes.Name = "btnAsignarLotes";
            btnAsignarLotes.Size = new Size(120, 30);
            btnAsignarLotes.TabIndex = 2;
            btnAsignarLotes.Text = "Asignar Lotes";
            btnAsignarLotes.UseVisualStyleBackColor = false;
            btnAsignarLotes.Click += btnAsignarLotes_Click;
            // 
            // cboUsuarios
            // 
            cboUsuarios.BackColor = Color.FromArgb(30, 30, 30);
            cboUsuarios.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUsuarios.FlatStyle = FlatStyle.Flat;
            cboUsuarios.ForeColor = Color.White;
            cboUsuarios.Location = new Point(516, 11);
            cboUsuarios.Margin = new Padding(3, 2, 3, 2);
            cboUsuarios.Name = "cboUsuarios";
            cboUsuarios.Size = new Size(150, 23);
            cboUsuarios.TabIndex = 1;
            // 
            // lblAsignarAUsuarios
            // 
            lblAsignarAUsuarios.AutoSize = true;
            lblAsignarAUsuarios.BackColor = Color.FromArgb(45, 45, 48);
            lblAsignarAUsuarios.ForeColor = Color.White;
            lblAsignarAUsuarios.Location = new Point(410, 15);
            lblAsignarAUsuarios.Name = "lblAsignarAUsuarios";
            lblAsignarAUsuarios.Size = new Size(102, 15);
            lblAsignarAUsuarios.TabIndex = 0;
            lblAsignarAUsuarios.Text = "Asignar a Usuario:";
            // 
            // btnSeleccionarTodos
            // 
            btnSeleccionarTodos.BackColor = Color.FromArgb(62, 62, 66);
            btnSeleccionarTodos.FlatStyle = FlatStyle.Flat;
            btnSeleccionarTodos.ForeColor = Color.White;
            btnSeleccionarTodos.Location = new Point(18, 8);
            btnSeleccionarTodos.Margin = new Padding(3, 2, 3, 2);
            btnSeleccionarTodos.Name = "btnSeleccionarTodos";
            btnSeleccionarTodos.Size = new Size(120, 30);
            btnSeleccionarTodos.TabIndex = 4;
            btnSeleccionarTodos.Text = "Seleccionar Todos";
            btnSeleccionarTodos.UseVisualStyleBackColor = false;
            btnSeleccionarTodos.Click += btnSeleccionarTodos_Click;
            // 
            // btnDeseleccionarTodo
            // 
            btnDeseleccionarTodo.BackColor = Color.FromArgb(62, 62, 66);
            btnDeseleccionarTodo.FlatStyle = FlatStyle.Flat;
            btnDeseleccionarTodo.ForeColor = Color.White;
            btnDeseleccionarTodo.Location = new Point(146, 8);
            btnDeseleccionarTodo.Margin = new Padding(3, 2, 3, 2);
            btnDeseleccionarTodo.Name = "btnDeseleccionarTodo";
            btnDeseleccionarTodo.Size = new Size(130, 30);
            btnDeseleccionarTodo.TabIndex = 5;
            btnDeseleccionarTodo.Text = "Deseleccionar Todo";
            btnDeseleccionarTodo.UseVisualStyleBackColor = false;
            btnDeseleccionarTodo.Click += btnDeseleccionarTodo_Click;
            // 
            // btnSeleccionar10
            // 
            btnSeleccionar10.BackColor = Color.FromArgb(62, 62, 66);
            btnSeleccionar10.FlatStyle = FlatStyle.Flat;
            btnSeleccionar10.ForeColor = Color.White;
            btnSeleccionar10.Location = new Point(284, 8);
            btnSeleccionar10.Margin = new Padding(3, 2, 3, 2);
            btnSeleccionar10.Name = "btnSeleccionar10";
            btnSeleccionar10.Size = new Size(110, 30);
            btnSeleccionar10.TabIndex = 6;
            btnSeleccionar10.Text = "Seleccionar 10";
            btnSeleccionar10.UseVisualStyleBackColor = false;
            btnSeleccionar10.Click += btnSeleccionar10_Click;
            // 
            // FrmAsignacionLote
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(957, 444);
            Controls.Add(groupBoxLotes);
            Controls.Add(panelAcciones);
            Controls.Add(groupBoxFiltros);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmAsignacionLote";
            Text = "Asignación de Lotes";
            Load += FrmAsignacionLote_Load;
            groupBoxFiltros.ResumeLayout(false);
            groupBoxFiltros.PerformLayout();
            groupBoxLotes.ResumeLayout(false);
            groupBoxLotes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLotes).EndInit();
            panelAcciones.ResumeLayout(false);
            panelAcciones.PerformLayout();
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
        private Label lblAsignarAUsuarios;
        private ComboBox cboUsuarios;
        private Button btnAsignarLotes;
        private Button btnCerrar;
        private Button btnSeleccionarTodos;
        private Button btnDeseleccionarTodo;
        private Button btnSeleccionar10;
    }
}
