namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmEnvioLote
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
            progressBar = new ProgressBar();
            lblRutaSalida = new Label();
            txtRutaSalida = new TextBox();
            btnSeleccionarSalida = new Button();
            lblNombreCarpetaSalida = new Label();
            txtNombreCarpetaSalida = new TextBox();
            lblObservacionesEnvio = new Label();
            txtObservacionesEnvio = new TextBox();
            btnSeleccionarTodo = new Button();
            btnDeseleccionarTodo = new Button();
            btnMarcarEnviado = new Button();
            btnCerrar = new Button();
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
            groupBoxFiltros.Size = new Size(1010, 70);
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
            chkFiltrarFecha.Size = new Size(190, 19);
            chkFiltrarFecha.TabIndex = 6;
            chkFiltrarFecha.Text = "Filtrar por fecha de alta control";
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
            btnLimpiarFiltros.Size = new Size(140, 26);
            btnLimpiarFiltros.TabIndex = 8;
            btnLimpiarFiltros.Text = "Limpiar Filtros";
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
            groupBoxLotes.Size = new Size(1010, 268);
            groupBoxLotes.TabIndex = 1;
            groupBoxLotes.TabStop = false;
            groupBoxLotes.Text = "Lotes Pendientes de Enviar";
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
            dgvLotes.Size = new Size(992, 236);
            dgvLotes.TabIndex = 0;
            //
            // lblTotalizadorLotes
            //
            lblTotalizadorLotes.AutoSize = true;
            lblTotalizadorLotes.BackColor = Color.FromArgb(45, 45, 48);
            lblTotalizadorLotes.ForeColor = Color.White;
            lblTotalizadorLotes.Dock = DockStyle.Bottom;
            lblTotalizadorLotes.Location = new Point(9, 230);
            lblTotalizadorLotes.Name = "lblTotalizadorLotes";
            lblTotalizadorLotes.Size = new Size(60, 15);
            lblTotalizadorLotes.TabIndex = 1;
            lblTotalizadorLotes.Text = "Total: 0";
            lblTotalizadorLotes.TextAlign = ContentAlignment.MiddleLeft;
            lblTotalizadorLotes.Padding = new Padding(0, 4, 0, 4);
            //
            // panelAcciones
            //
            panelAcciones.Controls.Add(progressBar);
            panelAcciones.Controls.Add(lblRutaSalida);
            panelAcciones.Controls.Add(txtRutaSalida);
            panelAcciones.Controls.Add(btnSeleccionarSalida);
            panelAcciones.Controls.Add(lblNombreCarpetaSalida);
            panelAcciones.Controls.Add(txtNombreCarpetaSalida);
            panelAcciones.Controls.Add(lblObservacionesEnvio);
            panelAcciones.Controls.Add(txtObservacionesEnvio);
            panelAcciones.Controls.Add(btnSeleccionarTodo);
            panelAcciones.Controls.Add(btnDeseleccionarTodo);
            panelAcciones.Controls.Add(btnMarcarEnviado);
            panelAcciones.Controls.Add(btnCerrar);
            panelAcciones.BackColor = Color.FromArgb(45, 45, 48);
            panelAcciones.Dock = DockStyle.Bottom;
            panelAcciones.Location = new Point(0, 338);
            panelAcciones.Margin = new Padding(3, 2, 3, 2);
            panelAcciones.Name = "panelAcciones";
            panelAcciones.Size = new Size(1010, 136);
            panelAcciones.TabIndex = 2;
            //
            // progressBar
            //
            progressBar.Location = new Point(18, 8);
            progressBar.Margin = new Padding(3, 2, 3, 2);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(974, 12);
            progressBar.TabIndex = 0;
            //
            // lblRutaSalida
            //
            lblRutaSalida.AutoSize = true;
            lblRutaSalida.ForeColor = Color.White;
            lblRutaSalida.Location = new Point(18, 30);
            lblRutaSalida.Name = "lblRutaSalida";
            lblRutaSalida.Size = new Size(70, 15);
            lblRutaSalida.TabIndex = 1;
            lblRutaSalida.Text = "Ruta de Salida:";
            //
            // txtRutaSalida
            //
            txtRutaSalida.BackColor = Color.FromArgb(30, 30, 30);
            txtRutaSalida.ForeColor = Color.White;
            txtRutaSalida.BorderStyle = BorderStyle.FixedSingle;
            txtRutaSalida.Location = new Point(114, 27);
            txtRutaSalida.Margin = new Padding(3, 2, 3, 2);
            txtRutaSalida.Name = "txtRutaSalida";
            txtRutaSalida.ReadOnly = true;
            txtRutaSalida.Size = new Size(560, 23);
            txtRutaSalida.TabIndex = 2;
            //
            // btnSeleccionarSalida
            //
            btnSeleccionarSalida.BackColor = Color.FromArgb(0, 122, 204);
            btnSeleccionarSalida.ForeColor = Color.White;
            btnSeleccionarSalida.FlatStyle = FlatStyle.Flat;
            btnSeleccionarSalida.Location = new Point(682, 26);
            btnSeleccionarSalida.Margin = new Padding(3, 2, 3, 2);
            btnSeleccionarSalida.Name = "btnSeleccionarSalida";
            btnSeleccionarSalida.Size = new Size(150, 26);
            btnSeleccionarSalida.TabIndex = 3;
            btnSeleccionarSalida.Text = "Seleccionar Salida";
            btnSeleccionarSalida.UseVisualStyleBackColor = false;
            btnSeleccionarSalida.Click += btnSeleccionarSalida_Click;
            //
            // lblNombreCarpetaSalida
            //
            lblNombreCarpetaSalida.AutoSize = true;
            lblNombreCarpetaSalida.ForeColor = Color.White;
            lblNombreCarpetaSalida.Location = new Point(18, 60);
            lblNombreCarpetaSalida.Name = "lblNombreCarpetaSalida";
            lblNombreCarpetaSalida.Size = new Size(120, 15);
            lblNombreCarpetaSalida.TabIndex = 4;
            lblNombreCarpetaSalida.Text = "Nombre Carpeta Salida:";
            //
            // txtNombreCarpetaSalida
            //
            txtNombreCarpetaSalida.BackColor = Color.FromArgb(30, 30, 30);
            txtNombreCarpetaSalida.ForeColor = Color.White;
            txtNombreCarpetaSalida.BorderStyle = BorderStyle.FixedSingle;
            txtNombreCarpetaSalida.Location = new Point(160, 57);
            txtNombreCarpetaSalida.Margin = new Padding(3, 2, 3, 2);
            txtNombreCarpetaSalida.Name = "txtNombreCarpetaSalida";
            txtNombreCarpetaSalida.Size = new Size(300, 23);
            txtNombreCarpetaSalida.TabIndex = 5;
            //
            // lblObservacionesEnvio
            //
            lblObservacionesEnvio.AutoSize = true;
            lblObservacionesEnvio.ForeColor = Color.White;
            lblObservacionesEnvio.Location = new Point(480, 60);
            lblObservacionesEnvio.Name = "lblObservacionesEnvio";
            lblObservacionesEnvio.Size = new Size(120, 15);
            lblObservacionesEnvio.TabIndex = 6;
            lblObservacionesEnvio.Text = "Observaciones Envio:";
            //
            // txtObservacionesEnvio
            //
            txtObservacionesEnvio.BackColor = Color.FromArgb(30, 30, 30);
            txtObservacionesEnvio.ForeColor = Color.White;
            txtObservacionesEnvio.BorderStyle = BorderStyle.FixedSingle;
            txtObservacionesEnvio.Location = new Point(610, 57);
            txtObservacionesEnvio.Margin = new Padding(3, 2, 3, 2);
            txtObservacionesEnvio.Multiline = true;
            txtObservacionesEnvio.Name = "txtObservacionesEnvio";
            txtObservacionesEnvio.Size = new Size(382, 40);
            txtObservacionesEnvio.TabIndex = 7;
            //
            // btnSeleccionarTodo
            //
            btnSeleccionarTodo.BackColor = Color.FromArgb(0, 122, 204);
            btnSeleccionarTodo.ForeColor = Color.White;
            btnSeleccionarTodo.FlatStyle = FlatStyle.Flat;
            btnSeleccionarTodo.Location = new Point(18, 100);
            btnSeleccionarTodo.Margin = new Padding(3, 2, 3, 2);
            btnSeleccionarTodo.Name = "btnSeleccionarTodo";
            btnSeleccionarTodo.Size = new Size(140, 30);
            btnSeleccionarTodo.TabIndex = 8;
            btnSeleccionarTodo.Text = "Seleccionar Todo";
            btnSeleccionarTodo.UseVisualStyleBackColor = false;
            btnSeleccionarTodo.Click += btnSeleccionarTodo_Click;
            //
            // btnDeseleccionarTodo
            //
            btnDeseleccionarTodo.BackColor = Color.FromArgb(0, 122, 204);
            btnDeseleccionarTodo.ForeColor = Color.White;
            btnDeseleccionarTodo.FlatStyle = FlatStyle.Flat;
            btnDeseleccionarTodo.Location = new Point(166, 100);
            btnDeseleccionarTodo.Margin = new Padding(3, 2, 3, 2);
            btnDeseleccionarTodo.Name = "btnDeseleccionarTodo";
            btnDeseleccionarTodo.Size = new Size(150, 30);
            btnDeseleccionarTodo.TabIndex = 9;
            btnDeseleccionarTodo.Text = "Deseleccionar Todo";
            btnDeseleccionarTodo.UseVisualStyleBackColor = false;
            btnDeseleccionarTodo.Click += btnDeseleccionarTodo_Click;
            //
            // btnMarcarEnviado
            //
            btnMarcarEnviado.BackColor = Color.FromArgb(0, 150, 70);
            btnMarcarEnviado.ForeColor = Color.White;
            btnMarcarEnviado.FlatStyle = FlatStyle.Flat;
            btnMarcarEnviado.Location = new Point(692, 100);
            btnMarcarEnviado.Margin = new Padding(3, 2, 3, 2);
            btnMarcarEnviado.Name = "btnMarcarEnviado";
            btnMarcarEnviado.Size = new Size(150, 30);
            btnMarcarEnviado.TabIndex = 10;
            btnMarcarEnviado.Text = "Marcar Enviado";
            btnMarcarEnviado.UseVisualStyleBackColor = false;
            btnMarcarEnviado.Click += btnMarcarEnviado_Click;
            //
            // btnCerrar
            //
            btnCerrar.BackColor = Color.FromArgb(150, 40, 40);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Location = new Point(848, 100);
            btnCerrar.Margin = new Padding(3, 2, 3, 2);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(144, 30);
            btnCerrar.TabIndex = 11;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmEnvioLote
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1010, 474);
            Controls.Add(groupBoxLotes);
            Controls.Add(panelAcciones);
            Controls.Add(groupBoxFiltros);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmEnvioLote";
            Text = "Envio de Lotes";
            Load += FrmEnvioLote_Load;
            groupBoxFiltros.ResumeLayout(false);
            groupBoxFiltros.PerformLayout();
            groupBoxLotes.ResumeLayout(false);
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
        private ProgressBar progressBar;
        private Label lblRutaSalida;
        private TextBox txtRutaSalida;
        private Button btnSeleccionarSalida;
        private Label lblNombreCarpetaSalida;
        private TextBox txtNombreCarpetaSalida;
        private Label lblObservacionesEnvio;
        private TextBox txtObservacionesEnvio;
        private Button btnSeleccionarTodo;
        private Button btnDeseleccionarTodo;
        private Button btnMarcarEnviado;
        private Button btnCerrar;
    }
}
