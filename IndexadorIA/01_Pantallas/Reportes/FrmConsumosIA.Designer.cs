namespace IndexadorIA.Pantallas.Reportes
{
    partial class FrmConsumosIA
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
            groupBoxResumen = new GroupBox();
            tableLayoutResumen = new TableLayoutPanel();
            lblTokenIngresoTitulo = new Label();
            lblTokenIngresoValor = new Label();
            lblTokenSalidaTitulo = new Label();
            lblTokenSalidaValor = new Label();
            lblTotalTokenTitulo = new Label();
            lblTotalTokenValor = new Label();
            lblPrecioIngresoTitulo = new Label();
            lblPrecioIngresoValor = new Label();
            lblPrecioSalidaTitulo = new Label();
            lblPrecioSalidaValor = new Label();
            lblTotalArchivosTitulo = new Label();
            lblTotalArchivosValor = new Label();
            panelTotalConsumido = new Panel();
            lblTotalConsumidoValor = new Label();
            lblTotalConsumidoTitulo = new Label();
            groupBoxResumenMensual = new GroupBox();
            dgvResumenMensual = new DataGridView();
            panelAcciones = new Panel();
            btnActualizar = new Button();
            btnCerrar = new Button();
            groupBoxResumen.SuspendLayout();
            tableLayoutResumen.SuspendLayout();
            panelTotalConsumido.SuspendLayout();
            groupBoxResumenMensual.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvResumenMensual).BeginInit();
            panelAcciones.SuspendLayout();
            SuspendLayout();
            //
            // groupBoxResumen
            //
            groupBoxResumen.Controls.Add(tableLayoutResumen);
            groupBoxResumen.Controls.Add(panelTotalConsumido);
            groupBoxResumen.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxResumen.ForeColor = Color.White;
            groupBoxResumen.Dock = DockStyle.Top;
            groupBoxResumen.Location = new Point(0, 0);
            groupBoxResumen.Margin = new Padding(3, 2, 3, 2);
            groupBoxResumen.Name = "groupBoxResumen";
            groupBoxResumen.Padding = new Padding(9, 24, 9, 8);
            groupBoxResumen.Size = new Size(1010, 220);
            groupBoxResumen.TabIndex = 0;
            groupBoxResumen.TabStop = false;
            groupBoxResumen.Text = "Resumen General de Consumo";
            //
            // tableLayoutResumen
            //
            tableLayoutResumen.ColumnCount = 3;
            tableLayoutResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tableLayoutResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tableLayoutResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tableLayoutResumen.Controls.Add(lblTokenIngresoTitulo, 0, 0);
            tableLayoutResumen.Controls.Add(lblTokenIngresoValor, 0, 1);
            tableLayoutResumen.Controls.Add(lblTokenSalidaTitulo, 1, 0);
            tableLayoutResumen.Controls.Add(lblTokenSalidaValor, 1, 1);
            tableLayoutResumen.Controls.Add(lblTotalTokenTitulo, 2, 0);
            tableLayoutResumen.Controls.Add(lblTotalTokenValor, 2, 1);
            tableLayoutResumen.Controls.Add(lblPrecioIngresoTitulo, 0, 2);
            tableLayoutResumen.Controls.Add(lblPrecioIngresoValor, 0, 3);
            tableLayoutResumen.Controls.Add(lblPrecioSalidaTitulo, 1, 2);
            tableLayoutResumen.Controls.Add(lblPrecioSalidaValor, 1, 3);
            tableLayoutResumen.Controls.Add(lblTotalArchivosTitulo, 2, 2);
            tableLayoutResumen.Controls.Add(lblTotalArchivosValor, 2, 3);
            tableLayoutResumen.Dock = DockStyle.Left;
            tableLayoutResumen.Location = new Point(9, 24);
            tableLayoutResumen.Name = "tableLayoutResumen";
            tableLayoutResumen.RowCount = 4;
            tableLayoutResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
            tableLayoutResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
            tableLayoutResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
            tableLayoutResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
            tableLayoutResumen.Size = new Size(650, 188);
            tableLayoutResumen.TabIndex = 0;
            //
            // lblTokenIngresoTitulo
            //
            lblTokenIngresoTitulo.AutoSize = true;
            lblTokenIngresoTitulo.ForeColor = Color.Silver;
            lblTokenIngresoTitulo.Location = new Point(3, 0);
            lblTokenIngresoTitulo.Name = "lblTokenIngresoTitulo";
            lblTokenIngresoTitulo.Size = new Size(107, 15);
            lblTokenIngresoTitulo.TabIndex = 0;
            lblTokenIngresoTitulo.Text = "Total Token Ingreso";
            //
            // lblTokenIngresoValor
            //
            lblTokenIngresoValor.AutoSize = true;
            lblTokenIngresoValor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTokenIngresoValor.ForeColor = Color.White;
            lblTokenIngresoValor.Location = new Point(3, 15);
            lblTokenIngresoValor.Name = "lblTokenIngresoValor";
            lblTokenIngresoValor.Size = new Size(24, 25);
            lblTokenIngresoValor.TabIndex = 1;
            lblTokenIngresoValor.Text = "0";
            //
            // lblTokenSalidaTitulo
            //
            lblTokenSalidaTitulo.AutoSize = true;
            lblTokenSalidaTitulo.ForeColor = Color.Silver;
            lblTokenSalidaTitulo.Location = new Point(219, 0);
            lblTokenSalidaTitulo.Name = "lblTokenSalidaTitulo";
            lblTokenSalidaTitulo.Size = new Size(97, 15);
            lblTokenSalidaTitulo.TabIndex = 2;
            lblTokenSalidaTitulo.Text = "Total Token Salida";
            //
            // lblTokenSalidaValor
            //
            lblTokenSalidaValor.AutoSize = true;
            lblTokenSalidaValor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTokenSalidaValor.ForeColor = Color.White;
            lblTokenSalidaValor.Location = new Point(219, 15);
            lblTokenSalidaValor.Name = "lblTokenSalidaValor";
            lblTokenSalidaValor.Size = new Size(24, 25);
            lblTokenSalidaValor.TabIndex = 3;
            lblTokenSalidaValor.Text = "0";
            //
            // lblTotalTokenTitulo
            //
            lblTotalTokenTitulo.AutoSize = true;
            lblTotalTokenTitulo.ForeColor = Color.Silver;
            lblTotalTokenTitulo.Location = new Point(435, 0);
            lblTotalTokenTitulo.Name = "lblTotalTokenTitulo";
            lblTotalTokenTitulo.Size = new Size(80, 15);
            lblTotalTokenTitulo.TabIndex = 4;
            lblTotalTokenTitulo.Text = "Total de Token";
            //
            // lblTotalTokenValor
            //
            lblTotalTokenValor.AutoSize = true;
            lblTotalTokenValor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTotalTokenValor.ForeColor = Color.White;
            lblTotalTokenValor.Location = new Point(435, 15);
            lblTotalTokenValor.Name = "lblTotalTokenValor";
            lblTotalTokenValor.Size = new Size(24, 25);
            lblTotalTokenValor.TabIndex = 5;
            lblTotalTokenValor.Text = "0";
            //
            // lblPrecioIngresoTitulo
            //
            lblPrecioIngresoTitulo.AutoSize = true;
            lblPrecioIngresoTitulo.ForeColor = Color.Silver;
            lblPrecioIngresoTitulo.Location = new Point(3, 47);
            lblPrecioIngresoTitulo.Name = "lblPrecioIngresoTitulo";
            lblPrecioIngresoTitulo.Size = new Size(139, 15);
            lblPrecioIngresoTitulo.TabIndex = 6;
            lblPrecioIngresoTitulo.Text = "Total Precio Ingreso USD";
            //
            // lblPrecioIngresoValor
            //
            lblPrecioIngresoValor.AutoSize = true;
            lblPrecioIngresoValor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblPrecioIngresoValor.ForeColor = Color.White;
            lblPrecioIngresoValor.Location = new Point(3, 62);
            lblPrecioIngresoValor.Name = "lblPrecioIngresoValor";
            lblPrecioIngresoValor.Size = new Size(45, 25);
            lblPrecioIngresoValor.TabIndex = 7;
            lblPrecioIngresoValor.Text = "$0.00";
            //
            // lblPrecioSalidaTitulo
            //
            lblPrecioSalidaTitulo.AutoSize = true;
            lblPrecioSalidaTitulo.ForeColor = Color.Silver;
            lblPrecioSalidaTitulo.Location = new Point(219, 47);
            lblPrecioSalidaTitulo.Name = "lblPrecioSalidaTitulo";
            lblPrecioSalidaTitulo.Size = new Size(130, 15);
            lblPrecioSalidaTitulo.TabIndex = 8;
            lblPrecioSalidaTitulo.Text = "Total Precio Salida USD";
            //
            // lblPrecioSalidaValor
            //
            lblPrecioSalidaValor.AutoSize = true;
            lblPrecioSalidaValor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblPrecioSalidaValor.ForeColor = Color.White;
            lblPrecioSalidaValor.Location = new Point(219, 62);
            lblPrecioSalidaValor.Name = "lblPrecioSalidaValor";
            lblPrecioSalidaValor.Size = new Size(45, 25);
            lblPrecioSalidaValor.TabIndex = 9;
            lblPrecioSalidaValor.Text = "$0.00";
            //
            // lblTotalArchivosTitulo
            //
            lblTotalArchivosTitulo.AutoSize = true;
            lblTotalArchivosTitulo.ForeColor = Color.Silver;
            lblTotalArchivosTitulo.Location = new Point(435, 47);
            lblTotalArchivosTitulo.Name = "lblTotalArchivosTitulo";
            lblTotalArchivosTitulo.Size = new Size(94, 15);
            lblTotalArchivosTitulo.TabIndex = 10;
            lblTotalArchivosTitulo.Text = "Total de Archivos";
            //
            // lblTotalArchivosValor
            //
            lblTotalArchivosValor.AutoSize = true;
            lblTotalArchivosValor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTotalArchivosValor.ForeColor = Color.White;
            lblTotalArchivosValor.Location = new Point(435, 62);
            lblTotalArchivosValor.Name = "lblTotalArchivosValor";
            lblTotalArchivosValor.Size = new Size(24, 25);
            lblTotalArchivosValor.TabIndex = 11;
            lblTotalArchivosValor.Text = "0";
            //
            // panelTotalConsumido
            //
            panelTotalConsumido.BackColor = Color.FromArgb(0, 122, 204);
            panelTotalConsumido.Controls.Add(lblTotalConsumidoValor);
            panelTotalConsumido.Controls.Add(lblTotalConsumidoTitulo);
            panelTotalConsumido.Dock = DockStyle.Right;
            panelTotalConsumido.Location = new Point(659, 24);
            panelTotalConsumido.Margin = new Padding(3, 2, 3, 2);
            panelTotalConsumido.Name = "panelTotalConsumido";
            panelTotalConsumido.Size = new Size(342, 188);
            panelTotalConsumido.TabIndex = 1;
            //
            // lblTotalConsumidoValor
            //
            lblTotalConsumidoValor.AutoSize = false;
            lblTotalConsumidoValor.Dock = DockStyle.Fill;
            lblTotalConsumidoValor.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            lblTotalConsumidoValor.ForeColor = Color.White;
            lblTotalConsumidoValor.Location = new Point(0, 30);
            lblTotalConsumidoValor.Name = "lblTotalConsumidoValor";
            lblTotalConsumidoValor.Size = new Size(342, 158);
            lblTotalConsumidoValor.TabIndex = 1;
            lblTotalConsumidoValor.Text = "$0.00";
            lblTotalConsumidoValor.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblTotalConsumidoTitulo
            //
            lblTotalConsumidoTitulo.AutoSize = false;
            lblTotalConsumidoTitulo.Dock = DockStyle.Top;
            lblTotalConsumidoTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalConsumidoTitulo.ForeColor = Color.White;
            lblTotalConsumidoTitulo.Location = new Point(0, 0);
            lblTotalConsumidoTitulo.Name = "lblTotalConsumidoTitulo";
            lblTotalConsumidoTitulo.Size = new Size(342, 30);
            lblTotalConsumidoTitulo.TabIndex = 0;
            lblTotalConsumidoTitulo.Text = "TOTAL CONSUMIDO USD";
            lblTotalConsumidoTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // groupBoxResumenMensual
            //
            groupBoxResumenMensual.Controls.Add(dgvResumenMensual);
            groupBoxResumenMensual.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxResumenMensual.ForeColor = Color.White;
            groupBoxResumenMensual.Dock = DockStyle.Fill;
            groupBoxResumenMensual.Location = new Point(0, 220);
            groupBoxResumenMensual.Margin = new Padding(3, 2, 3, 2);
            groupBoxResumenMensual.Name = "groupBoxResumenMensual";
            groupBoxResumenMensual.Padding = new Padding(9, 24, 9, 8);
            groupBoxResumenMensual.Size = new Size(1010, 208);
            groupBoxResumenMensual.TabIndex = 1;
            groupBoxResumenMensual.TabStop = false;
            groupBoxResumenMensual.Text = "Resumen por Mes y Modelo";
            //
            // dgvResumenMensual
            //
            dgvResumenMensual.AllowUserToAddRows = false;
            dgvResumenMensual.AllowUserToDeleteRows = false;
            dgvResumenMensual.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResumenMensual.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvResumenMensual.BorderStyle = BorderStyle.None;
            dgvResumenMensual.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvResumenMensual.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvResumenMensual.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResumenMensual.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvResumenMensual.DefaultCellStyle.ForeColor = Color.White;
            dgvResumenMensual.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvResumenMensual.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvResumenMensual.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvResumenMensual.EnableHeadersVisualStyles = false;
            dgvResumenMensual.GridColor = Color.FromArgb(63, 63, 70);
            dgvResumenMensual.ReadOnly = true;
            dgvResumenMensual.AllowUserToOrderColumns = false;
            dgvResumenMensual.MultiSelect = false;
            dgvResumenMensual.Dock = DockStyle.Fill;
            dgvResumenMensual.Location = new Point(9, 24);
            dgvResumenMensual.Margin = new Padding(3, 2, 3, 2);
            dgvResumenMensual.Name = "dgvResumenMensual";
            dgvResumenMensual.RowHeadersWidth = 51;
            dgvResumenMensual.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResumenMensual.Size = new Size(992, 176);
            dgvResumenMensual.TabIndex = 0;
            //
            // panelAcciones
            //
            panelAcciones.Controls.Add(btnActualizar);
            panelAcciones.Controls.Add(btnCerrar);
            panelAcciones.BackColor = Color.FromArgb(45, 45, 48);
            panelAcciones.Dock = DockStyle.Bottom;
            panelAcciones.Location = new Point(0, 428);
            panelAcciones.Margin = new Padding(3, 2, 3, 2);
            panelAcciones.Name = "panelAcciones";
            panelAcciones.Size = new Size(1010, 46);
            panelAcciones.TabIndex = 2;
            //
            // btnActualizar
            //
            btnActualizar.BackColor = Color.FromArgb(0, 122, 204);
            btnActualizar.ForeColor = Color.White;
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.Location = new Point(18, 8);
            btnActualizar.Margin = new Padding(3, 2, 3, 2);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(150, 30);
            btnActualizar.TabIndex = 0;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            btnActualizar.Click += btnActualizar_Click;
            //
            // btnCerrar
            //
            btnCerrar.BackColor = Color.FromArgb(96, 96, 96);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Location = new Point(176, 8);
            btnCerrar.Margin = new Padding(3, 2, 3, 2);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(150, 30);
            btnCerrar.TabIndex = 1;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmConsumosIA
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(1010, 474);
            Controls.Add(groupBoxResumenMensual);
            Controls.Add(groupBoxResumen);
            Controls.Add(panelAcciones);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmConsumosIA";
            Text = "Consumos IA";
            Load += FrmConsumosIA_Load;
            groupBoxResumen.ResumeLayout(false);
            tableLayoutResumen.ResumeLayout(false);
            tableLayoutResumen.PerformLayout();
            panelTotalConsumido.ResumeLayout(false);
            groupBoxResumenMensual.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvResumenMensual).EndInit();
            panelAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxResumen;
        private TableLayoutPanel tableLayoutResumen;
        private Label lblTokenIngresoTitulo;
        private Label lblTokenIngresoValor;
        private Label lblTokenSalidaTitulo;
        private Label lblTokenSalidaValor;
        private Label lblTotalTokenTitulo;
        private Label lblTotalTokenValor;
        private Label lblPrecioIngresoTitulo;
        private Label lblPrecioIngresoValor;
        private Label lblPrecioSalidaTitulo;
        private Label lblPrecioSalidaValor;
        private Label lblTotalArchivosTitulo;
        private Label lblTotalArchivosValor;
        private Panel panelTotalConsumido;
        private Label lblTotalConsumidoValor;
        private Label lblTotalConsumidoTitulo;
        private GroupBox groupBoxResumenMensual;
        private DataGridView dgvResumenMensual;
        private Panel panelAcciones;
        private Button btnActualizar;
        private Button btnCerrar;
    }
}
