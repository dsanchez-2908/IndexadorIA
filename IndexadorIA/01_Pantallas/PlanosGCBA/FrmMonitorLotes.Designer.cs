namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmMonitorLotes
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
            groupBoxUsuarios = new GroupBox();
            dgvUsuarios = new DataGridView();
            lblTotalizadorUsuarios = new Label();
            panelAcciones = new Panel();
            btnVerDetalle = new Button();
            btnActualizar = new Button();
            btnCerrar = new Button();
            groupBoxUsuarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            panelAcciones.SuspendLayout();
            SuspendLayout();
            //
            // groupBoxUsuarios
            //
            groupBoxUsuarios.Controls.Add(dgvUsuarios);
            groupBoxUsuarios.Controls.Add(lblTotalizadorUsuarios);
            groupBoxUsuarios.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxUsuarios.ForeColor = Color.White;
            groupBoxUsuarios.Dock = DockStyle.Fill;
            groupBoxUsuarios.Location = new Point(0, 0);
            groupBoxUsuarios.Margin = new Padding(3, 2, 3, 2);
            groupBoxUsuarios.Name = "groupBoxUsuarios";
            groupBoxUsuarios.Padding = new Padding(9, 24, 9, 8);
            groupBoxUsuarios.Size = new Size(1010, 428);
            groupBoxUsuarios.TabIndex = 0;
            groupBoxUsuarios.TabStop = false;
            groupBoxUsuarios.Text = "Monitor de Lotes por Usuario";
            //
            // dgvUsuarios
            //
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvUsuarios.BorderStyle = BorderStyle.None;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvUsuarios.DefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.GridColor = Color.FromArgb(63, 63, 70);
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.AllowUserToOrderColumns = false;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Dock = DockStyle.Fill;
            dgvUsuarios.Location = new Point(9, 24);
            dgvUsuarios.Margin = new Padding(3, 2, 3, 2);
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.RowHeadersWidth = 51;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(992, 396);
            dgvUsuarios.TabIndex = 0;
            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
            dgvUsuarios.CellDoubleClick += dgvUsuarios_CellDoubleClick;
            //
            // lblTotalizadorUsuarios
            //
            lblTotalizadorUsuarios.AutoSize = true;
            lblTotalizadorUsuarios.BackColor = Color.FromArgb(45, 45, 48);
            lblTotalizadorUsuarios.ForeColor = Color.White;
            lblTotalizadorUsuarios.Dock = DockStyle.Bottom;
            lblTotalizadorUsuarios.Location = new Point(9, 390);
            lblTotalizadorUsuarios.Name = "lblTotalizadorUsuarios";
            lblTotalizadorUsuarios.Size = new Size(60, 15);
            lblTotalizadorUsuarios.TabIndex = 1;
            lblTotalizadorUsuarios.Text = "Total: 0";
            lblTotalizadorUsuarios.TextAlign = ContentAlignment.MiddleLeft;
            lblTotalizadorUsuarios.Padding = new Padding(0, 4, 0, 4);
            //
            // panelAcciones
            //
            panelAcciones.Controls.Add(btnVerDetalle);
            panelAcciones.Controls.Add(btnActualizar);
            panelAcciones.Controls.Add(btnCerrar);
            panelAcciones.BackColor = Color.FromArgb(45, 45, 48);
            panelAcciones.Dock = DockStyle.Bottom;
            panelAcciones.Location = new Point(0, 428);
            panelAcciones.Margin = new Padding(3, 2, 3, 2);
            panelAcciones.Name = "panelAcciones";
            panelAcciones.Size = new Size(1010, 46);
            panelAcciones.TabIndex = 1;
            //
            // btnVerDetalle
            //
            btnVerDetalle.BackColor = Color.FromArgb(0, 122, 204);
            btnVerDetalle.ForeColor = Color.White;
            btnVerDetalle.FlatStyle = FlatStyle.Flat;
            btnVerDetalle.Location = new Point(18, 8);
            btnVerDetalle.Margin = new Padding(3, 2, 3, 2);
            btnVerDetalle.Name = "btnVerDetalle";
            btnVerDetalle.Size = new Size(150, 30);
            btnVerDetalle.TabIndex = 0;
            btnVerDetalle.Text = "Ver Detalle";
            btnVerDetalle.UseVisualStyleBackColor = false;
            btnVerDetalle.Enabled = false;
            btnVerDetalle.Click += btnVerDetalle_Click;
            //
            // btnActualizar
            //
            btnActualizar.BackColor = Color.FromArgb(0, 122, 204);
            btnActualizar.ForeColor = Color.White;
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.Location = new Point(176, 8);
            btnActualizar.Margin = new Padding(3, 2, 3, 2);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(150, 30);
            btnActualizar.TabIndex = 1;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            btnActualizar.Click += btnActualizar_Click;
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
            btnCerrar.TabIndex = 2;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmMonitorLotes
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ForeColor = Color.White;
            ClientSize = new Size(1010, 474);
            Controls.Add(groupBoxUsuarios);
            Controls.Add(panelAcciones);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmMonitorLotes";
            Text = "Monitor de Lotes";
            Load += FrmMonitorLotes_Load;
            groupBoxUsuarios.ResumeLayout(false);
            groupBoxUsuarios.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            panelAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxUsuarios;
        private DataGridView dgvUsuarios;
        private Label lblTotalizadorUsuarios;
        private Panel panelAcciones;
        private Button btnVerDetalle;
        private Button btnActualizar;
        private Button btnCerrar;
    }
}
