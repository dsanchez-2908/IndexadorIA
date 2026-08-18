namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmMonitorLotesDetalle
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
            lblEstado = new Label();
            rbTodo = new RadioButton();
            rbPendienteControl = new RadioButton();
            rbControlado = new RadioButton();
            groupBoxLotes = new GroupBox();
            dgvLotes = new DataGridView();
            lblTotalizadorLotes = new Label();
            panelAcciones = new Panel();
            lblReasignarA = new Label();
            cboUsuarios = new ComboBox();
            btnReasignarControl = new Button();
            btnCerrar = new Button();
            groupBoxFiltros.SuspendLayout();
            groupBoxLotes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLotes).BeginInit();
            panelAcciones.SuspendLayout();
            SuspendLayout();
            //
            // groupBoxFiltros
            //
            groupBoxFiltros.Controls.Add(lblUsuario);
            groupBoxFiltros.Controls.Add(lblEstado);
            groupBoxFiltros.Controls.Add(rbTodo);
            groupBoxFiltros.Controls.Add(rbPendienteControl);
            groupBoxFiltros.Controls.Add(rbControlado);
            groupBoxFiltros.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxFiltros.ForeColor = Color.White;
            groupBoxFiltros.Dock = DockStyle.Top;
            groupBoxFiltros.Location = new Point(0, 0);
            groupBoxFiltros.Margin = new Padding(3, 2, 3, 2);
            groupBoxFiltros.Name = "groupBoxFiltros";
            groupBoxFiltros.Padding = new Padding(3, 2, 3, 2);
            groupBoxFiltros.Size = new Size(1010, 70);
            groupBoxFiltros.TabIndex = 0;
            groupBoxFiltros.TabStop = false;
            groupBoxFiltros.Text = "Filtros";
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.BackColor = Color.FromArgb(45, 45, 48);
            lblUsuario.ForeColor = Color.White;
            lblUsuario.Location = new Point(18, 22);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(70, 15);
            lblUsuario.TabIndex = 0;
            lblUsuario.Text = "Usuario:";
            //
            // lblEstado
            //
            lblEstado.AutoSize = true;
            lblEstado.BackColor = Color.FromArgb(45, 45, 48);
            lblEstado.ForeColor = Color.White;
            lblEstado.Location = new Point(18, 44);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(50, 15);
            lblEstado.TabIndex = 1;
            lblEstado.Text = "Estado:";
            //
            // rbTodo
            //
            rbTodo.AutoSize = true;
            rbTodo.ForeColor = Color.White;
            rbTodo.Location = new Point(114, 43);
            rbTodo.Name = "rbTodo";
            rbTodo.Size = new Size(52, 19);
            rbTodo.TabIndex = 2;
            rbTodo.TabStop = true;
            rbTodo.Text = "Todo";
            rbTodo.UseVisualStyleBackColor = true;
            rbTodo.CheckedChanged += rbFiltro_CheckedChanged;
            //
            // rbPendienteControl
            //
            rbPendienteControl.AutoSize = true;
            rbPendienteControl.ForeColor = Color.White;
            rbPendienteControl.Location = new Point(200, 43);
            rbPendienteControl.Name = "rbPendienteControl";
            rbPendienteControl.Size = new Size(140, 19);
            rbPendienteControl.TabIndex = 3;
            rbPendienteControl.Text = "Pendiente de Control";
            rbPendienteControl.UseVisualStyleBackColor = true;
            rbPendienteControl.CheckedChanged += rbFiltro_CheckedChanged;
            //
            // rbControlado
            //
            rbControlado.AutoSize = true;
            rbControlado.ForeColor = Color.White;
            rbControlado.Location = new Point(370, 43);
            rbControlado.Name = "rbControlado";
            rbControlado.Size = new Size(180, 19);
            rbControlado.TabIndex = 4;
            rbControlado.Text = "Controlado (incl. Finalizado)";
            rbControlado.UseVisualStyleBackColor = true;
            rbControlado.CheckedChanged += rbFiltro_CheckedChanged;
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
            groupBoxLotes.Size = new Size(1010, 358);
            groupBoxLotes.TabIndex = 1;
            groupBoxLotes.TabStop = false;
            groupBoxLotes.Text = "Lotes del Usuario";
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
            dgvLotes.ReadOnly = false;
            dgvLotes.AllowUserToOrderColumns = false;
            dgvLotes.MultiSelect = true;
            dgvLotes.Dock = DockStyle.Fill;
            dgvLotes.Location = new Point(9, 24);
            dgvLotes.Margin = new Padding(3, 2, 3, 2);
            dgvLotes.Name = "dgvLotes";
            dgvLotes.RowHeadersWidth = 51;
            dgvLotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLotes.Size = new Size(992, 326);
            dgvLotes.TabIndex = 0;
            dgvLotes.CellContentClick += dgvLotes_CellContentClick;
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
            panelAcciones.Controls.Add(lblReasignarA);
            panelAcciones.Controls.Add(cboUsuarios);
            panelAcciones.Controls.Add(btnReasignarControl);
            panelAcciones.Controls.Add(btnCerrar);
            panelAcciones.BackColor = Color.FromArgb(45, 45, 48);
            panelAcciones.Dock = DockStyle.Bottom;
            panelAcciones.Location = new Point(0, 428);
            panelAcciones.Margin = new Padding(3, 2, 3, 2);
            panelAcciones.Name = "panelAcciones";
            panelAcciones.Size = new Size(1010, 46);
            panelAcciones.TabIndex = 2;
            //
            // lblReasignarA
            //
            lblReasignarA.AutoSize = true;
            lblReasignarA.BackColor = Color.FromArgb(45, 45, 48);
            lblReasignarA.ForeColor = Color.White;
            lblReasignarA.Location = new Point(18, 16);
            lblReasignarA.Name = "lblReasignarA";
            lblReasignarA.Size = new Size(100, 15);
            lblReasignarA.TabIndex = 0;
            lblReasignarA.Text = "Reasignar a:";
            //
            // cboUsuarios
            //
            cboUsuarios.BackColor = Color.FromArgb(30, 30, 30);
            cboUsuarios.ForeColor = Color.White;
            cboUsuarios.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUsuarios.FormattingEnabled = true;
            cboUsuarios.Location = new Point(124, 12);
            cboUsuarios.Margin = new Padding(3, 2, 3, 2);
            cboUsuarios.Name = "cboUsuarios";
            cboUsuarios.Size = new Size(220, 23);
            cboUsuarios.TabIndex = 1;
            //
            // btnReasignarControl
            //
            btnReasignarControl.BackColor = Color.FromArgb(0, 122, 204);
            btnReasignarControl.ForeColor = Color.White;
            btnReasignarControl.FlatStyle = FlatStyle.Flat;
            btnReasignarControl.Location = new Point(360, 8);
            btnReasignarControl.Margin = new Padding(3, 2, 3, 2);
            btnReasignarControl.Name = "btnReasignarControl";
            btnReasignarControl.Size = new Size(170, 30);
            btnReasignarControl.TabIndex = 2;
            btnReasignarControl.Text = "Reasignar Control";
            btnReasignarControl.UseVisualStyleBackColor = false;
            btnReasignarControl.Click += btnReasignarControl_Click;
            //
            // btnCerrar
            //
            btnCerrar.BackColor = Color.FromArgb(150, 40, 40);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Location = new Point(842, 8);
            btnCerrar.Margin = new Padding(3, 2, 3, 2);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(150, 30);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmMonitorLotesDetalle
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ForeColor = Color.White;
            ClientSize = new Size(1010, 474);
            Controls.Add(groupBoxLotes);
            Controls.Add(panelAcciones);
            Controls.Add(groupBoxFiltros);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmMonitorLotesDetalle";
            Text = "Detalle de Lotes";
            Load += FrmMonitorLotesDetalle_Load;
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
        private Label lblUsuario;
        private Label lblEstado;
        private RadioButton rbTodo;
        private RadioButton rbPendienteControl;
        private RadioButton rbControlado;
        private GroupBox groupBoxLotes;
        private DataGridView dgvLotes;
        private Label lblTotalizadorLotes;
        private Panel panelAcciones;
        private Label lblReasignarA;
        private ComboBox cboUsuarios;
        private Button btnReasignarControl;
        private Button btnCerrar;
    }
}
