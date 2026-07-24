namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmProcesamientoIA
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

        private void InitializeComponent()
        {
            panelSuperior = new Panel();
            groupBoxAcciones = new GroupBox();
            lblEstado = new Label();
            progressBar = new ProgressBar();
            btnProcesar = new Button();
            btnCerrar = new Button();
            groupBoxLotes = new GroupBox();
            dgvLotes = new DataGridView();
            panelSeleccion = new Panel();
            lblTotalizadorLotes = new Label();
            btnSeleccionarTodo = new Button();
            btnDeseleccionarTodo = new Button();
            btnSeleccionar50 = new Button();
            groupBoxFiltros = new GroupBox();
            lblProyecto = new Label();
            cboProyecto = new ComboBox();
            btnCargarLotes = new Button();
            btnVerPrompt = new Button();
            groupBoxTracking = new GroupBox();
            dgvTracking = new DataGridView();
            panelTrackingButtons = new Panel();
            chkMostrarProcesados = new CheckBox();
            btnVerificarEstado = new Button();
            btnProcesarResultados = new Button();
            btnActualizarTracking = new Button();
            panelSuperior.SuspendLayout();
            groupBoxAcciones.SuspendLayout();
            groupBoxLotes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLotes).BeginInit();
            panelSeleccion.SuspendLayout();
            groupBoxFiltros.SuspendLayout();
            groupBoxTracking.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTracking).BeginInit();
            panelTrackingButtons.SuspendLayout();
            SuspendLayout();
            // 
            // panelSuperior
            // 
            panelSuperior.Controls.Add(groupBoxAcciones);
            panelSuperior.Controls.Add(groupBoxLotes);
            panelSuperior.Controls.Add(groupBoxFiltros);
            panelSuperior.Dock = DockStyle.Top;
            panelSuperior.Location = new Point(9, 8);
            panelSuperior.Margin = new Padding(3, 2, 3, 2);
            panelSuperior.Name = "panelSuperior";
            panelSuperior.Size = new Size(857, 350);
            panelSuperior.TabIndex = 0;
            // 
            // groupBoxAcciones
            // 
            groupBoxAcciones.Controls.Add(lblEstado);
            groupBoxAcciones.Controls.Add(progressBar);
            groupBoxAcciones.Controls.Add(btnProcesar);
            groupBoxAcciones.Controls.Add(btnCerrar);
            groupBoxAcciones.Dock = DockStyle.Bottom;
            groupBoxAcciones.ForeColor = Color.White;
            groupBoxAcciones.Location = new Point(0, 282);
            groupBoxAcciones.Margin = new Padding(3, 2, 3, 2);
            groupBoxAcciones.Name = "groupBoxAcciones";
            groupBoxAcciones.Padding = new Padding(3, 2, 3, 2);
            groupBoxAcciones.Size = new Size(857, 68);
            groupBoxAcciones.TabIndex = 2;
            groupBoxAcciones.TabStop = false;
            groupBoxAcciones.Text = "Acciones";
            groupBoxAcciones.Enter += groupBoxAcciones_Enter;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.ForeColor = Color.White;
            lblEstado.Location = new Point(18, 20);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(45, 15);
            lblEstado.TabIndex = 0;
            lblEstado.Text = "Estado:";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(18, 38);
            progressBar.Margin = new Padding(3, 2, 3, 2);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(460, 19);
            progressBar.TabIndex = 1;
            // 
            // btnProcesar
            // 
            btnProcesar.ForeColor = Color.Black;
            btnProcesar.Location = new Point(490, 18);
            btnProcesar.Margin = new Padding(3, 2, 3, 2);
            btnProcesar.Name = "btnProcesar";
            btnProcesar.Size = new Size(175, 38);
            btnProcesar.TabIndex = 2;
            btnProcesar.Text = "Procesar Lotes";
            btnProcesar.UseVisualStyleBackColor = true;
            btnProcesar.Click += btnProcesar_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.ForeColor = Color.Black;
            btnCerrar.Location = new Point(675, 18);
            btnCerrar.Margin = new Padding(3, 2, 3, 2);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(155, 38);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Visible = true;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // groupBoxLotes
            // 
            groupBoxLotes.Controls.Add(dgvLotes);
            groupBoxLotes.Controls.Add(panelSeleccion);
            groupBoxLotes.Dock = DockStyle.Fill;
            groupBoxLotes.ForeColor = Color.White;
            groupBoxLotes.Location = new Point(0, 60);
            groupBoxLotes.Margin = new Padding(3, 2, 3, 2);
            groupBoxLotes.Name = "groupBoxLotes";
            groupBoxLotes.Padding = new Padding(9, 8, 9, 8);
            groupBoxLotes.Size = new Size(857, 290);
            groupBoxLotes.TabIndex = 1;
            groupBoxLotes.TabStop = false;
            groupBoxLotes.Text = "Lotes Pendientes de Procesamiento (Estado: Listo para IA)";
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
            dgvLotes.Dock = DockStyle.Fill;
            dgvLotes.Location = new Point(9, 54);
            dgvLotes.Margin = new Padding(3, 2, 3, 2);
            dgvLotes.MultiSelect = false;
            dgvLotes.Name = "dgvLotes";
            dgvLotes.RowHeadersWidth = 51;
            dgvLotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLotes.Size = new Size(839, 228);
            dgvLotes.TabIndex = 0;
            dgvLotes.SelectionChanged += dgvLotes_SelectionChanged;
            // 
            // panelSeleccion
            // 
            panelSeleccion.Controls.Add(lblTotalizadorLotes);
            panelSeleccion.Controls.Add(btnSeleccionarTodo);
            panelSeleccion.Controls.Add(btnDeseleccionarTodo);
            panelSeleccion.Controls.Add(btnSeleccionar50);
            panelSeleccion.BackColor = Color.FromArgb(45, 45, 48);
            panelSeleccion.Dock = DockStyle.Top;
            panelSeleccion.Location = new Point(9, 24);
            panelSeleccion.Margin = new Padding(3, 2, 3, 2);
            panelSeleccion.Name = "panelSeleccion";
            panelSeleccion.Size = new Size(839, 30);
            panelSeleccion.TabIndex = 1;
            // 
            // btnSeleccionarTodo
            // 
            btnSeleccionarTodo.BackColor = Color.FromArgb(0, 122, 204);
            btnSeleccionarTodo.ForeColor = Color.White;
            btnSeleccionarTodo.FlatStyle = FlatStyle.Flat;
            btnSeleccionarTodo.Location = new Point(4, 4);
            btnSeleccionarTodo.Margin = new Padding(3, 2, 3, 2);
            btnSeleccionarTodo.Name = "btnSeleccionarTodo";
            btnSeleccionarTodo.Size = new Size(114, 22);
            btnSeleccionarTodo.TabIndex = 0;
            btnSeleccionarTodo.Text = "Seleccionar Todo";
            btnSeleccionarTodo.UseVisualStyleBackColor = false;
            btnSeleccionarTodo.Click += btnSeleccionarTodo_Click;
            // 
            // btnDeseleccionarTodo
            // 
            btnDeseleccionarTodo.BackColor = Color.FromArgb(0, 122, 204);
            btnDeseleccionarTodo.ForeColor = Color.White;
            btnDeseleccionarTodo.FlatStyle = FlatStyle.Flat;
            btnDeseleccionarTodo.Location = new Point(127, 4);
            btnDeseleccionarTodo.Margin = new Padding(3, 2, 3, 2);
            btnDeseleccionarTodo.Name = "btnDeseleccionarTodo";
            btnDeseleccionarTodo.Size = new Size(131, 22);
            btnDeseleccionarTodo.TabIndex = 1;
            btnDeseleccionarTodo.Text = "Deseleccionar Todo";
            btnDeseleccionarTodo.UseVisualStyleBackColor = false;
            btnDeseleccionarTodo.Click += btnDeseleccionarTodo_Click;
            // 
            // btnSeleccionar50
            // 
            btnSeleccionar50.BackColor = Color.FromArgb(0, 122, 204);
            btnSeleccionar50.ForeColor = Color.White;
            btnSeleccionar50.FlatStyle = FlatStyle.Flat;
            btnSeleccionar50.Location = new Point(267, 4);
            btnSeleccionar50.Margin = new Padding(3, 2, 3, 2);
            btnSeleccionar50.Name = "btnSeleccionar50";
            btnSeleccionar50.Size = new Size(114, 22);
            btnSeleccionar50.TabIndex = 2;
            btnSeleccionar50.Text = "Seleccionar 50";
            btnSeleccionar50.UseVisualStyleBackColor = false;
            btnSeleccionar50.Click += btnSeleccionar50_Click;
            // 
            // lblTotalizadorLotes
            // 
            lblTotalizadorLotes.AutoSize = true;
            lblTotalizadorLotes.BackColor = Color.FromArgb(45, 45, 48);
            lblTotalizadorLotes.ForeColor = Color.White;
            lblTotalizadorLotes.Location = new Point(400, 8);
            lblTotalizadorLotes.Name = "lblTotalizadorLotes";
            lblTotalizadorLotes.Size = new Size(150, 15);
            lblTotalizadorLotes.TabIndex = 3;
            lblTotalizadorLotes.Text = "Total: 0 | Seleccionados: 0";
            // 
            // groupBoxFiltros
            //
            groupBoxFiltros.Controls.Add(lblProyecto);
            groupBoxFiltros.Controls.Add(cboProyecto);
            groupBoxFiltros.Controls.Add(btnCargarLotes);
            groupBoxFiltros.Controls.Add(btnVerPrompt);
            groupBoxFiltros.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxFiltros.ForeColor = Color.White;
            groupBoxFiltros.Dock = DockStyle.Top;
            groupBoxFiltros.Location = new Point(0, 0);
            groupBoxFiltros.Margin = new Padding(3, 2, 3, 2);
            groupBoxFiltros.Name = "groupBoxFiltros";
            groupBoxFiltros.Padding = new Padding(3, 2, 3, 2);
            groupBoxFiltros.Size = new Size(857, 60);
            groupBoxFiltros.TabIndex = 0;
            groupBoxFiltros.TabStop = false;
            groupBoxFiltros.Text = "Filtros";
            // 
            // lblProyecto
            // 
            lblProyecto.AutoSize = true;
            lblProyecto.BackColor = Color.FromArgb(45, 45, 48);
            lblProyecto.ForeColor = Color.White;
            lblProyecto.Location = new Point(18, 26);
            lblProyecto.Name = "lblProyecto";
            lblProyecto.Size = new Size(57, 15);
            lblProyecto.TabIndex = 0;
            lblProyecto.Text = "Proyecto:";
            // 
            // cboProyecto
            // 
            cboProyecto.BackColor = Color.FromArgb(30, 30, 30);
            cboProyecto.ForeColor = Color.White;
            cboProyecto.FlatStyle = FlatStyle.Flat;
            cboProyecto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProyecto.FormattingEnabled = true;
            cboProyecto.Location = new Point(88, 24);
            cboProyecto.Margin = new Padding(3, 2, 3, 2);
            cboProyecto.Name = "cboProyecto";
            cboProyecto.Size = new Size(263, 23);
            cboProyecto.TabIndex = 1;
            // 
            // btnCargarLotes
            // 
            btnCargarLotes.BackColor = Color.FromArgb(0, 122, 204);
            btnCargarLotes.ForeColor = Color.White;
            btnCargarLotes.FlatStyle = FlatStyle.Flat;
            btnCargarLotes.Location = new Point(368, 22);
            btnCargarLotes.Margin = new Padding(3, 2, 3, 2);
            btnCargarLotes.Name = "btnCargarLotes";
            btnCargarLotes.Size = new Size(131, 24);
            btnCargarLotes.TabIndex = 2;
            btnCargarLotes.Text = "Cargar Lotes";
            btnCargarLotes.UseVisualStyleBackColor = false;
            btnCargarLotes.Click += btnCargarLotes_Click;
            // 
            // btnVerPrompt
            // 
            btnVerPrompt.BackColor = Color.FromArgb(0, 122, 204);
            btnVerPrompt.ForeColor = Color.White;
            btnVerPrompt.FlatStyle = FlatStyle.Flat;
            btnVerPrompt.Location = new Point(516, 22);
            btnVerPrompt.Margin = new Padding(3, 2, 3, 2);
            btnVerPrompt.Name = "btnVerPrompt";
            btnVerPrompt.Size = new Size(131, 24);
            btnVerPrompt.TabIndex = 3;
            btnVerPrompt.Text = "Ver/Editar Prompt";
            btnVerPrompt.UseVisualStyleBackColor = false;
            btnVerPrompt.Click += btnVerPrompt_Click;
            // 
            // groupBoxTracking
            // 
            groupBoxTracking.Controls.Add(dgvTracking);
            groupBoxTracking.Controls.Add(panelTrackingButtons);
            groupBoxTracking.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxTracking.ForeColor = Color.White;
            groupBoxTracking.Dock = DockStyle.Fill;
            groupBoxTracking.Location = new Point(9, 358);
            groupBoxTracking.Margin = new Padding(3, 2, 3, 2);
            groupBoxTracking.Name = "groupBoxTracking";
            groupBoxTracking.Padding = new Padding(9, 8, 9, 8);
            groupBoxTracking.Size = new Size(857, 159);
            groupBoxTracking.TabIndex = 1;
            groupBoxTracking.TabStop = false;
            groupBoxTracking.Text = "Batches Enviados a OpenAI";
            // 
            // dgvTracking
            // 
            dgvTracking.AllowUserToAddRows = false;
            dgvTracking.AllowUserToDeleteRows = false;
            dgvTracking.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTracking.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvTracking.BorderStyle = BorderStyle.None;
            dgvTracking.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvTracking.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTracking.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTracking.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvTracking.DefaultCellStyle.ForeColor = Color.White;
            dgvTracking.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvTracking.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvTracking.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvTracking.EnableHeadersVisualStyles = false;
            dgvTracking.GridColor = Color.FromArgb(63, 63, 70);
            dgvTracking.Dock = DockStyle.Fill;
            dgvTracking.Location = new Point(9, 54);
            dgvTracking.Margin = new Padding(3, 2, 3, 2);
            dgvTracking.MultiSelect = false;
            dgvTracking.Name = "dgvTracking";
            dgvTracking.ReadOnly = true;
            dgvTracking.RowHeadersWidth = 51;
            dgvTracking.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTracking.Size = new Size(839, 97);
            dgvTracking.TabIndex = 0;
            // 
            // panelTrackingButtons
            // 
            panelTrackingButtons.Controls.Add(chkMostrarProcesados);
            panelTrackingButtons.Controls.Add(btnVerificarEstado);
            panelTrackingButtons.Controls.Add(btnProcesarResultados);
            panelTrackingButtons.Controls.Add(btnActualizarTracking);
            panelTrackingButtons.BackColor = Color.FromArgb(45, 45, 48);
            panelTrackingButtons.Dock = DockStyle.Top;
            panelTrackingButtons.Location = new Point(9, 24);
            panelTrackingButtons.Margin = new Padding(3, 2, 3, 2);
            panelTrackingButtons.Name = "panelTrackingButtons";
            panelTrackingButtons.Size = new Size(839, 30);
            panelTrackingButtons.TabIndex = 1;
            // 
            // chkMostrarProcesados
            // 
            chkMostrarProcesados.AutoSize = true;
            chkMostrarProcesados.ForeColor = Color.White;
            chkMostrarProcesados.Location = new Point(445, 6);
            chkMostrarProcesados.Name = "chkMostrarProcesados";
            chkMostrarProcesados.Size = new Size(187, 19);
            chkMostrarProcesados.TabIndex = 3;
            chkMostrarProcesados.Text = "Mostrar resultados procesados";
            chkMostrarProcesados.UseVisualStyleBackColor = true;
            chkMostrarProcesados.CheckedChanged += chkMostrarProcesados_CheckedChanged;
            // 
            // btnVerificarEstado
            // 
            btnVerificarEstado.BackColor = Color.FromArgb(0, 122, 204);
            btnVerificarEstado.ForeColor = Color.White;
            btnVerificarEstado.FlatStyle = FlatStyle.Flat;
            btnVerificarEstado.Location = new Point(144, 4);
            btnVerificarEstado.Margin = new Padding(3, 2, 3, 2);
            btnVerificarEstado.Name = "btnVerificarEstado";
            btnVerificarEstado.Size = new Size(131, 22);
            btnVerificarEstado.TabIndex = 1;
            btnVerificarEstado.Text = "Verificar Estado";
            btnVerificarEstado.UseVisualStyleBackColor = false;
            btnVerificarEstado.Click += btnVerificarEstado_Click;
            // 
            // btnProcesarResultados
            // 
            btnProcesarResultados.BackColor = Color.FromArgb(0, 122, 204);
            btnProcesarResultados.ForeColor = Color.White;
            btnProcesarResultados.FlatStyle = FlatStyle.Flat;
            btnProcesarResultados.Location = new Point(284, 4);
            btnProcesarResultados.Margin = new Padding(3, 2, 3, 2);
            btnProcesarResultados.Name = "btnProcesarResultados";
            btnProcesarResultados.Size = new Size(149, 22);
            btnProcesarResultados.TabIndex = 2;
            btnProcesarResultados.Text = "Procesar Resultados";
            btnProcesarResultados.UseVisualStyleBackColor = false;
            btnProcesarResultados.Click += btnProcesarResultados_Click;
            // 
            // btnActualizarTracking
            // 
            btnActualizarTracking.BackColor = Color.FromArgb(0, 122, 204);
            btnActualizarTracking.ForeColor = Color.White;
            btnActualizarTracking.FlatStyle = FlatStyle.Flat;
            btnActualizarTracking.Location = new Point(4, 4);
            btnActualizarTracking.Margin = new Padding(3, 2, 3, 2);
            btnActualizarTracking.Name = "btnActualizarTracking";
            btnActualizarTracking.Size = new Size(131, 22);
            btnActualizarTracking.TabIndex = 0;
            btnActualizarTracking.Text = "Actualizar Lista";
            btnActualizarTracking.UseVisualStyleBackColor = false;
            btnActualizarTracking.Click += btnActualizarTracking_Click;
            // 
            // FrmProcesamientoIA
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ForeColor = Color.White;
            ClientSize = new Size(875, 525);
            Controls.Add(groupBoxTracking);
            Controls.Add(panelSuperior);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmProcesamientoIA";
            Padding = new Padding(9, 8, 9, 8);
            Text = "Procesamiento por OpenIA";
            Load += FrmProcesamientoIA_Load;
            panelSuperior.ResumeLayout(false);
            groupBoxAcciones.ResumeLayout(false);
            groupBoxAcciones.PerformLayout();
            groupBoxLotes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLotes).EndInit();
            panelSeleccion.ResumeLayout(false);
            panelSeleccion.PerformLayout();
            groupBoxFiltros.ResumeLayout(false);
            groupBoxFiltros.PerformLayout();
            groupBoxTracking.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTracking).EndInit();
            panelTrackingButtons.ResumeLayout(false);
            panelTrackingButtons.PerformLayout();
            ResumeLayout(false);
        }

        private Panel panelSuperior;
        private GroupBox groupBoxFiltros;
        private Label lblProyecto;
        private ComboBox cboProyecto;
        private Button btnCargarLotes;
        private Button btnVerPrompt;
        private GroupBox groupBoxLotes;
        private DataGridView dgvLotes;
        private Panel panelSeleccion;
        private Label lblTotalizadorLotes;
        private Button btnSeleccionarTodo;
        private Button btnDeseleccionarTodo;
        private Button btnSeleccionar50;
        private GroupBox groupBoxTracking;
        private DataGridView dgvTracking;
        private Panel panelTrackingButtons;
        private CheckBox chkMostrarProcesados;
        private Button btnActualizarTracking;
        private Button btnVerificarEstado;
        private Button btnProcesarResultados;
        private GroupBox groupBoxAcciones;
        private Label lblEstado;
        private ProgressBar progressBar;
        private Button btnProcesar;
        private Button btnCerrar;
    }
}
