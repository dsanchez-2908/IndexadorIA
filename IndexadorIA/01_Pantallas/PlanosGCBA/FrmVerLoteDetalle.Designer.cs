namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmVerLoteDetalle
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            groupBoxDetalle = new GroupBox();
            dgvDetalle = new DataGridView();
            lblTotalizadorDetalle = new Label();
            panelAccionesDetalle = new Panel();
            btnCerrarDetalle = new Button();
            groupBoxDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
            panelAccionesDetalle.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxDetalle
            // 
            groupBoxDetalle.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxDetalle.Controls.Add(dgvDetalle);
            groupBoxDetalle.Controls.Add(lblTotalizadorDetalle);
            groupBoxDetalle.Dock = DockStyle.Fill;
            groupBoxDetalle.ForeColor = Color.White;
            groupBoxDetalle.Location = new Point(0, 0);
            groupBoxDetalle.Margin = new Padding(3, 2, 3, 2);
            groupBoxDetalle.Name = "groupBoxDetalle";
            groupBoxDetalle.Padding = new Padding(9, 24, 9, 8);
            groupBoxDetalle.Size = new Size(1000, 458);
            groupBoxDetalle.TabIndex = 0;
            groupBoxDetalle.TabStop = false;
            groupBoxDetalle.Text = "Registros del Lote";
            // 
            // dgvDetalle
            // 
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.AllowUserToDeleteRows = false;
            dgvDetalle.ReadOnly = true;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(40, 40, 40);
            dgvDetalle.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalle.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvDetalle.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(60, 60, 60);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvDetalle.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvDetalle.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(30, 30, 30);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvDetalle.DefaultCellStyle = dataGridViewCellStyle3;
            dgvDetalle.Dock = DockStyle.Fill;
            dgvDetalle.EnableHeadersVisualStyles = false;
            dgvDetalle.GridColor = Color.FromArgb(63, 63, 70);
            dgvDetalle.Location = new Point(9, 24);
            dgvDetalle.Margin = new Padding(3, 2, 3, 2);
            dgvDetalle.MultiSelect = false;
            dgvDetalle.Name = "dgvDetalle";
            dgvDetalle.RowHeadersWidth = 51;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.Size = new Size(982, 403);
            dgvDetalle.TabIndex = 0;
            // 
            // lblTotalizadorDetalle
            // 
            lblTotalizadorDetalle.AutoSize = true;
            lblTotalizadorDetalle.BackColor = Color.FromArgb(45, 45, 48);
            lblTotalizadorDetalle.Dock = DockStyle.Bottom;
            lblTotalizadorDetalle.ForeColor = Color.White;
            lblTotalizadorDetalle.Location = new Point(9, 427);
            lblTotalizadorDetalle.Name = "lblTotalizadorDetalle";
            lblTotalizadorDetalle.Padding = new Padding(0, 4, 0, 4);
            lblTotalizadorDetalle.Size = new Size(45, 23);
            lblTotalizadorDetalle.TabIndex = 1;
            lblTotalizadorDetalle.Text = "Total: 0";
            lblTotalizadorDetalle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelAccionesDetalle
            // 
            panelAccionesDetalle.BackColor = Color.FromArgb(45, 45, 48);
            panelAccionesDetalle.Controls.Add(btnCerrarDetalle);
            panelAccionesDetalle.Dock = DockStyle.Bottom;
            panelAccionesDetalle.Location = new Point(0, 458);
            panelAccionesDetalle.Margin = new Padding(3, 2, 3, 2);
            panelAccionesDetalle.Name = "panelAccionesDetalle";
            panelAccionesDetalle.Size = new Size(1000, 46);
            panelAccionesDetalle.TabIndex = 1;
            // 
            // btnCerrarDetalle
            // 
            btnCerrarDetalle.BackColor = Color.FromArgb(150, 40, 40);
            btnCerrarDetalle.FlatStyle = FlatStyle.Flat;
            btnCerrarDetalle.ForeColor = Color.White;
            btnCerrarDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrarDetalle.Location = new Point(862, 8);
            btnCerrarDetalle.Margin = new Padding(3, 2, 3, 2);
            btnCerrarDetalle.Name = "btnCerrarDetalle";
            btnCerrarDetalle.Size = new Size(120, 30);
            btnCerrarDetalle.TabIndex = 0;
            btnCerrarDetalle.Text = "Cerrar";
            btnCerrarDetalle.UseVisualStyleBackColor = false;
            btnCerrarDetalle.Click += btnCerrarDetalle_Click;
            // 
            // FrmVerLoteDetalle
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(1000, 504);
            Controls.Add(groupBoxDetalle);
            Controls.Add(panelAccionesDetalle);
            ForeColor = Color.White;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmVerLoteDetalle";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Detalle del Lote";
            Load += FrmVerLoteDetalle_Load;
            groupBoxDetalle.ResumeLayout(false);
            groupBoxDetalle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
            panelAccionesDetalle.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxDetalle;
        private DataGridView dgvDetalle;
        private Label lblTotalizadorDetalle;
        private Panel panelAccionesDetalle;
        private Button btnCerrarDetalle;
    }
}
