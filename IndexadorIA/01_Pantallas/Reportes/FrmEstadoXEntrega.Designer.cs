namespace IndexadorIA.Pantallas.Reportes
{
    partial class FrmEstadoXEntrega
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
            groupBoxLotes = new GroupBox();
            dgvLotes = new DataGridView();
            groupBoxPlanos = new GroupBox();
            dgvPlanos = new DataGridView();
            panelAcciones = new Panel();
            btnCerrar = new Button();
            btnVerLotesPendientes = new Button();
            groupBoxLotes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLotes).BeginInit();
            groupBoxPlanos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPlanos).BeginInit();
            panelAcciones.SuspendLayout();
            SuspendLayout();
            //
            // groupBoxLotes
            //
            groupBoxLotes.Controls.Add(dgvLotes);
            groupBoxLotes.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxLotes.ForeColor = Color.White;
            groupBoxLotes.Dock = DockStyle.Top;
            groupBoxLotes.Location = new Point(0, 0);
            groupBoxLotes.Margin = new Padding(3, 2, 3, 2);
            groupBoxLotes.Name = "groupBoxLotes";
            groupBoxLotes.Padding = new Padding(9, 24, 9, 8);
            groupBoxLotes.Size = new Size(1100, 260);
            groupBoxLotes.TabIndex = 0;
            groupBoxLotes.TabStop = false;
            groupBoxLotes.Text = "Lotes";
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
            dgvLotes.Size = new Size(1082, 228);
            dgvLotes.TabIndex = 0;
            dgvLotes.SelectionChanged += dgvLotes_SelectionChanged;
            //
            // groupBoxPlanos
            //
            groupBoxPlanos.Controls.Add(dgvPlanos);
            groupBoxPlanos.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxPlanos.ForeColor = Color.White;
            groupBoxPlanos.Dock = DockStyle.Fill;
            groupBoxPlanos.Location = new Point(0, 260);
            groupBoxPlanos.Margin = new Padding(3, 2, 3, 2);
            groupBoxPlanos.Name = "groupBoxPlanos";
            groupBoxPlanos.Padding = new Padding(9, 24, 9, 8);
            groupBoxPlanos.Size = new Size(1100, 298);
            groupBoxPlanos.TabIndex = 1;
            groupBoxPlanos.TabStop = false;
            groupBoxPlanos.Text = "Planos";
            //
            // dgvPlanos
            //
            dgvPlanos.AllowUserToAddRows = false;
            dgvPlanos.AllowUserToDeleteRows = false;
            dgvPlanos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPlanos.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvPlanos.BorderStyle = BorderStyle.None;
            dgvPlanos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvPlanos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPlanos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPlanos.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvPlanos.DefaultCellStyle.ForeColor = Color.White;
            dgvPlanos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvPlanos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvPlanos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvPlanos.EnableHeadersVisualStyles = false;
            dgvPlanos.GridColor = Color.FromArgb(63, 63, 70);
            dgvPlanos.ReadOnly = true;
            dgvPlanos.AllowUserToOrderColumns = false;
            dgvPlanos.MultiSelect = false;
            dgvPlanos.Dock = DockStyle.Fill;
            dgvPlanos.Location = new Point(9, 24);
            dgvPlanos.Margin = new Padding(3, 2, 3, 2);
            dgvPlanos.Name = "dgvPlanos";
            dgvPlanos.RowHeadersWidth = 51;
            dgvPlanos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPlanos.Size = new Size(1082, 266);
            dgvPlanos.TabIndex = 0;
            //
            // panelAcciones
            //
            panelAcciones.Controls.Add(btnCerrar);
            panelAcciones.Controls.Add(btnVerLotesPendientes);
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
            // btnVerLotesPendientes
            //
            btnVerLotesPendientes.BackColor = Color.FromArgb(0, 122, 204);
            btnVerLotesPendientes.Enabled = false;
            btnVerLotesPendientes.FlatStyle = FlatStyle.Flat;
            btnVerLotesPendientes.ForeColor = Color.White;
            btnVerLotesPendientes.Location = new Point(180, 8);
            btnVerLotesPendientes.Margin = new Padding(3, 2, 3, 2);
            btnVerLotesPendientes.Name = "btnVerLotesPendientes";
            btnVerLotesPendientes.Size = new Size(190, 30);
            btnVerLotesPendientes.TabIndex = 1;
            btnVerLotesPendientes.Text = "Ver Lotes Pendientes";
            btnVerLotesPendientes.UseVisualStyleBackColor = false;
            btnVerLotesPendientes.Click += btnVerLotesPendientes_Click;
            //
            // FrmEstadoXEntrega
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(1100, 604);
            Controls.Add(groupBoxPlanos);
            Controls.Add(groupBoxLotes);
            Controls.Add(panelAcciones);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmEstadoXEntrega";
            Text = "Estado x Entrega";
            Load += FrmEstadoXEntrega_Load;
            groupBoxLotes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLotes).EndInit();
            groupBoxPlanos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPlanos).EndInit();
            panelAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxLotes;
        private DataGridView dgvLotes;
        private GroupBox groupBoxPlanos;
        private DataGridView dgvPlanos;
        private Panel panelAcciones;
        private Button btnCerrar;
        private Button btnVerLotesPendientes;
    }
}
