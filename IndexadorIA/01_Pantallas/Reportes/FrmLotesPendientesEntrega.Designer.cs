namespace IndexadorIA.Pantallas.Reportes
{
    partial class FrmLotesPendientesEntrega
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
            groupBoxEstados = new GroupBox();
            dgvEstados = new DataGridView();
            groupBoxDetalle = new GroupBox();
            dgvDetalle = new DataGridView();
            panelAcciones = new Panel();
            btnCerrar = new Button();
            groupBoxEstados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEstados).BeginInit();
            groupBoxDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            panelAcciones.SuspendLayout();
            SuspendLayout();
            //
            // groupBoxEstados
            //
            groupBoxEstados.Controls.Add(dgvEstados);
            groupBoxEstados.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxEstados.ForeColor = Color.White;
            groupBoxEstados.Dock = DockStyle.Top;
            groupBoxEstados.Location = new Point(0, 0);
            groupBoxEstados.Margin = new Padding(3, 2, 3, 2);
            groupBoxEstados.Name = "groupBoxEstados";
            groupBoxEstados.Padding = new Padding(9, 24, 9, 8);
            groupBoxEstados.Size = new Size(1100, 220);
            groupBoxEstados.TabIndex = 0;
            groupBoxEstados.TabStop = false;
            groupBoxEstados.Text = "Lotes Pendientes por Estado";
            //
            // dgvEstados
            //
            dgvEstados.AllowUserToAddRows = false;
            dgvEstados.AllowUserToDeleteRows = false;
            dgvEstados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEstados.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvEstados.BorderStyle = BorderStyle.None;
            dgvEstados.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvEstados.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEstados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEstados.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvEstados.DefaultCellStyle.ForeColor = Color.White;
            dgvEstados.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvEstados.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvEstados.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvEstados.EnableHeadersVisualStyles = false;
            dgvEstados.GridColor = Color.FromArgb(63, 63, 70);
            dgvEstados.ReadOnly = true;
            dgvEstados.AllowUserToOrderColumns = false;
            dgvEstados.MultiSelect = false;
            dgvEstados.Dock = DockStyle.Fill;
            dgvEstados.Location = new Point(9, 24);
            dgvEstados.Margin = new Padding(3, 2, 3, 2);
            dgvEstados.Name = "dgvEstados";
            dgvEstados.RowHeadersWidth = 51;
            dgvEstados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstados.Size = new Size(1082, 188);
            dgvEstados.TabIndex = 0;
            dgvEstados.SelectionChanged += dgvEstados_SelectionChanged;
            //
            // groupBoxDetalle
            //
            groupBoxDetalle.Controls.Add(dgvDetalle);
            groupBoxDetalle.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxDetalle.ForeColor = Color.White;
            groupBoxDetalle.Dock = DockStyle.Fill;
            groupBoxDetalle.Location = new Point(0, 220);
            groupBoxDetalle.Margin = new Padding(3, 2, 3, 2);
            groupBoxDetalle.Name = "groupBoxDetalle";
            groupBoxDetalle.Padding = new Padding(9, 24, 9, 8);
            groupBoxDetalle.Size = new Size(1100, 338);
            groupBoxDetalle.TabIndex = 1;
            groupBoxDetalle.TabStop = false;
            groupBoxDetalle.Text = "Detalle de Lotes";
            //
            // dgvDetalle
            //
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.AllowUserToDeleteRows = false;
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvDetalle.BorderStyle = BorderStyle.None;
            dgvDetalle.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvDetalle.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalle.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvDetalle.DefaultCellStyle.ForeColor = Color.White;
            dgvDetalle.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvDetalle.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvDetalle.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvDetalle.EnableHeadersVisualStyles = false;
            dgvDetalle.GridColor = Color.FromArgb(63, 63, 70);
            dgvDetalle.ReadOnly = true;
            dgvDetalle.AllowUserToOrderColumns = false;
            dgvDetalle.MultiSelect = false;
            dgvDetalle.Dock = DockStyle.Fill;
            dgvDetalle.Location = new Point(9, 24);
            dgvDetalle.Margin = new Padding(3, 2, 3, 2);
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.RowHeadersWidth = 51;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.Size = new Size(1082, 306);
            dgvDetalle.TabIndex = 0;
            //
            // panelAcciones
            //
            panelAcciones.Controls.Add(btnCerrar);
            panelAcciones.BackColor = Color.FromArgb(45, 45, 48);
            panelAcciones.Dock = DockStyle.Bottom;
            panelAcciones.Location = new Point(0, 558);
            panelAcciones.Margin = new Padding(3, 2, 3, 2);
            panelAcciones.Name = "panelAcciones";
            panelAcciones.Size = new Size(1100, 46);
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
            // FrmLotesPendientesEntrega
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(1100, 604);
            Controls.Add(groupBoxDetalle);
            Controls.Add(groupBoxEstados);
            Controls.Add(panelAcciones);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmLotesPendientesEntrega";
            Text = "Lotes Pendientes por Entrega";
            Load += FrmLotesPendientesEntrega_Load;
            groupBoxEstados.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEstados).EndInit();
            groupBoxDetalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            panelAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxEstados;
        private DataGridView dgvEstados;
        private GroupBox groupBoxDetalle;
        private DataGridView dgvDetalle;
        private Panel panelAcciones;
        private Button btnCerrar;
    }
}
