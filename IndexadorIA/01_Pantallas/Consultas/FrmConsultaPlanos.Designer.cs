namespace IndexadorIA.Pantallas.Consultas
{
    partial class FrmConsultaPlanos
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
            groupBoxFiltros = new GroupBox();
            lblCdLote = new Label();
            txtCdLote = new TextBox();
            lblEstadoLote = new Label();
            cboEstadoLote = new ComboBox();
            lblEntrega = new Label();
            txtEntrega = new TextBox();
            lblCarpeta = new Label();
            txtCarpeta = new TextBox();
            lblEstadoControl = new Label();
            cboEstadoControl = new ComboBox();
            lblCategoriaPlano = new Label();
            cboCategoriaPlano = new ComboBox();
            lblTipoPlano = new Label();
            cboTipoPlano = new ComboBox();
            lblExpediente = new Label();
            txtExpediente = new TextBox();
            lblSeccion = new Label();
            txtSeccion = new TextBox();
            lblManzana = new Label();
            txtManzana = new TextBox();
            lblParcela = new Label();
            txtParcela = new TextBox();
            lblDireccion = new Label();
            txtDireccion = new TextBox();
            lblNumeroPlano = new Label();
            txtNumeroPlano = new TextBox();
            lblObservaciones = new Label();
            txtObservaciones = new TextBox();
            lblDescripcionLote = new Label();
            txtDescripcionLote = new TextBox();
            btnBuscar = new Button();
            btnLimpiar = new Button();
            groupBoxGrilla = new GroupBox();
            dgvPlanos = new DataGridView();
            panelAcciones = new Panel();
            btnCerrar = new Button();
            btnExportarExcel = new Button();
            btnAuditarPlano = new Button();
            lblCantidadRegistros = new Label();
            btnReubicarLote = new Button();
            groupBoxFiltros.SuspendLayout();
            groupBoxGrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPlanos).BeginInit();
            panelAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxFiltros
            // 
            groupBoxFiltros.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxFiltros.Controls.Add(lblCdLote);
            groupBoxFiltros.Controls.Add(txtCdLote);
            groupBoxFiltros.Controls.Add(lblEstadoLote);
            groupBoxFiltros.Controls.Add(cboEstadoLote);
            groupBoxFiltros.Controls.Add(lblEntrega);
            groupBoxFiltros.Controls.Add(txtEntrega);
            groupBoxFiltros.Controls.Add(lblCarpeta);
            groupBoxFiltros.Controls.Add(txtCarpeta);
            groupBoxFiltros.Controls.Add(lblEstadoControl);
            groupBoxFiltros.Controls.Add(cboEstadoControl);
            groupBoxFiltros.Controls.Add(lblCategoriaPlano);
            groupBoxFiltros.Controls.Add(cboCategoriaPlano);
            groupBoxFiltros.Controls.Add(lblTipoPlano);
            groupBoxFiltros.Controls.Add(cboTipoPlano);
            groupBoxFiltros.Controls.Add(lblExpediente);
            groupBoxFiltros.Controls.Add(txtExpediente);
            groupBoxFiltros.Controls.Add(lblSeccion);
            groupBoxFiltros.Controls.Add(txtSeccion);
            groupBoxFiltros.Controls.Add(lblManzana);
            groupBoxFiltros.Controls.Add(txtManzana);
            groupBoxFiltros.Controls.Add(lblParcela);
            groupBoxFiltros.Controls.Add(txtParcela);
            groupBoxFiltros.Controls.Add(lblDireccion);
            groupBoxFiltros.Controls.Add(txtDireccion);
            groupBoxFiltros.Controls.Add(lblNumeroPlano);
            groupBoxFiltros.Controls.Add(txtNumeroPlano);
            groupBoxFiltros.Controls.Add(lblObservaciones);
            groupBoxFiltros.Controls.Add(txtObservaciones);
            groupBoxFiltros.Controls.Add(lblDescripcionLote);
            groupBoxFiltros.Controls.Add(txtDescripcionLote);
            groupBoxFiltros.Controls.Add(btnBuscar);
            groupBoxFiltros.Controls.Add(btnLimpiar);
            groupBoxFiltros.Dock = DockStyle.Top;
            groupBoxFiltros.ForeColor = Color.White;
            groupBoxFiltros.Location = new Point(0, 0);
            groupBoxFiltros.Margin = new Padding(3, 2, 3, 2);
            groupBoxFiltros.Name = "groupBoxFiltros";
            groupBoxFiltros.Padding = new Padding(9, 24, 9, 8);
            groupBoxFiltros.Size = new Size(1060, 264);
            groupBoxFiltros.TabIndex = 0;
            groupBoxFiltros.TabStop = false;
            groupBoxFiltros.Text = "Filtros";
            // 
            // lblCdLote
            // 
            lblCdLote.AutoSize = true;
            lblCdLote.ForeColor = Color.Silver;
            lblCdLote.Location = new Point(12, 30);
            lblCdLote.Name = "lblCdLote";
            lblCdLote.Size = new Size(33, 15);
            lblCdLote.TabIndex = 0;
            lblCdLote.Text = "Lote:";
            // 
            // txtCdLote
            // 
            txtCdLote.Location = new Point(12, 48);
            txtCdLote.Margin = new Padding(3, 2, 3, 2);
            txtCdLote.Name = "txtCdLote";
            txtCdLote.Size = new Size(245, 23);
            txtCdLote.TabIndex = 1;
            // 
            // lblEstadoLote
            // 
            lblEstadoLote.AutoSize = true;
            lblEstadoLote.ForeColor = Color.Silver;
            lblEstadoLote.Location = new Point(270, 30);
            lblEstadoLote.Name = "lblEstadoLote";
            lblEstadoLote.Size = new Size(87, 15);
            lblEstadoLote.TabIndex = 2;
            lblEstadoLote.Text = "Estado de Lote:";
            // 
            // cboEstadoLote
            // 
            cboEstadoLote.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstadoLote.FormattingEnabled = true;
            cboEstadoLote.Location = new Point(270, 48);
            cboEstadoLote.Margin = new Padding(3, 2, 3, 2);
            cboEstadoLote.Name = "cboEstadoLote";
            cboEstadoLote.Size = new Size(245, 23);
            cboEstadoLote.TabIndex = 3;
            // 
            // lblEntrega
            // 
            lblEntrega.AutoSize = true;
            lblEntrega.ForeColor = Color.Silver;
            lblEntrega.Location = new Point(528, 30);
            lblEntrega.Name = "lblEntrega";
            lblEntrega.Size = new Size(50, 15);
            lblEntrega.TabIndex = 4;
            lblEntrega.Text = "Entrega:";
            // 
            // txtEntrega
            // 
            txtEntrega.Location = new Point(528, 48);
            txtEntrega.Margin = new Padding(3, 2, 3, 2);
            txtEntrega.Name = "txtEntrega";
            txtEntrega.Size = new Size(245, 23);
            txtEntrega.TabIndex = 5;
            // 
            // lblCarpeta
            // 
            lblCarpeta.AutoSize = true;
            lblCarpeta.ForeColor = Color.Silver;
            lblCarpeta.Location = new Point(786, 30);
            lblCarpeta.Name = "lblCarpeta";
            lblCarpeta.Size = new Size(51, 15);
            lblCarpeta.TabIndex = 6;
            lblCarpeta.Text = "Carpeta:";
            // 
            // txtCarpeta
            // 
            txtCarpeta.Location = new Point(786, 48);
            txtCarpeta.Margin = new Padding(3, 2, 3, 2);
            txtCarpeta.Name = "txtCarpeta";
            txtCarpeta.Size = new Size(245, 23);
            txtCarpeta.TabIndex = 7;
            // 
            // lblEstadoControl
            // 
            lblEstadoControl.AutoSize = true;
            lblEstadoControl.ForeColor = Color.Silver;
            lblEstadoControl.Location = new Point(12, 78);
            lblEstadoControl.Name = "lblEstadoControl";
            lblEstadoControl.Size = new Size(104, 15);
            lblEstadoControl.TabIndex = 8;
            lblEstadoControl.Text = "Estado de Control:";
            // 
            // cboEstadoControl
            // 
            cboEstadoControl.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstadoControl.FormattingEnabled = true;
            cboEstadoControl.Location = new Point(12, 96);
            cboEstadoControl.Margin = new Padding(3, 2, 3, 2);
            cboEstadoControl.Name = "cboEstadoControl";
            cboEstadoControl.Size = new Size(245, 23);
            cboEstadoControl.TabIndex = 9;
            // 
            // lblCategoriaPlano
            // 
            lblCategoriaPlano.AutoSize = true;
            lblCategoriaPlano.ForeColor = Color.Silver;
            lblCategoriaPlano.Location = new Point(270, 78);
            lblCategoriaPlano.Name = "lblCategoriaPlano";
            lblCategoriaPlano.Size = new Size(110, 15);
            lblCategoriaPlano.TabIndex = 10;
            lblCategoriaPlano.Text = "Categoría de Plano:";
            // 
            // cboCategoriaPlano
            // 
            cboCategoriaPlano.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoriaPlano.FormattingEnabled = true;
            cboCategoriaPlano.Location = new Point(270, 96);
            cboCategoriaPlano.Margin = new Padding(3, 2, 3, 2);
            cboCategoriaPlano.Name = "cboCategoriaPlano";
            cboCategoriaPlano.Size = new Size(245, 23);
            cboCategoriaPlano.TabIndex = 11;
            // 
            // lblTipoPlano
            // 
            lblTipoPlano.AutoSize = true;
            lblTipoPlano.ForeColor = Color.Silver;
            lblTipoPlano.Location = new Point(528, 78);
            lblTipoPlano.Name = "lblTipoPlano";
            lblTipoPlano.Size = new Size(83, 15);
            lblTipoPlano.TabIndex = 12;
            lblTipoPlano.Text = "Tipo de Plano:";
            // 
            // cboTipoPlano
            // 
            cboTipoPlano.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoPlano.FormattingEnabled = true;
            cboTipoPlano.Location = new Point(528, 96);
            cboTipoPlano.Margin = new Padding(3, 2, 3, 2);
            cboTipoPlano.Name = "cboTipoPlano";
            cboTipoPlano.Size = new Size(245, 23);
            cboTipoPlano.TabIndex = 13;
            // 
            // lblExpediente
            // 
            lblExpediente.AutoSize = true;
            lblExpediente.ForeColor = Color.Silver;
            lblExpediente.Location = new Point(786, 78);
            lblExpediente.Name = "lblExpediente";
            lblExpediente.Size = new Size(67, 15);
            lblExpediente.TabIndex = 14;
            lblExpediente.Text = "Expediente:";
            // 
            // txtExpediente
            // 
            txtExpediente.Location = new Point(786, 96);
            txtExpediente.Margin = new Padding(3, 2, 3, 2);
            txtExpediente.Name = "txtExpediente";
            txtExpediente.Size = new Size(245, 23);
            txtExpediente.TabIndex = 15;
            // 
            // lblSeccion
            // 
            lblSeccion.AutoSize = true;
            lblSeccion.ForeColor = Color.Silver;
            lblSeccion.Location = new Point(12, 126);
            lblSeccion.Name = "lblSeccion";
            lblSeccion.Size = new Size(51, 15);
            lblSeccion.TabIndex = 16;
            lblSeccion.Text = "Sección:";
            // 
            // txtSeccion
            // 
            txtSeccion.Location = new Point(12, 144);
            txtSeccion.Margin = new Padding(3, 2, 3, 2);
            txtSeccion.Name = "txtSeccion";
            txtSeccion.Size = new Size(245, 23);
            txtSeccion.TabIndex = 17;
            // 
            // lblManzana
            // 
            lblManzana.AutoSize = true;
            lblManzana.ForeColor = Color.Silver;
            lblManzana.Location = new Point(270, 126);
            lblManzana.Name = "lblManzana";
            lblManzana.Size = new Size(58, 15);
            lblManzana.TabIndex = 18;
            lblManzana.Text = "Manzana:";
            // 
            // txtManzana
            // 
            txtManzana.Location = new Point(270, 144);
            txtManzana.Margin = new Padding(3, 2, 3, 2);
            txtManzana.Name = "txtManzana";
            txtManzana.Size = new Size(245, 23);
            txtManzana.TabIndex = 19;
            // 
            // lblParcela
            // 
            lblParcela.AutoSize = true;
            lblParcela.ForeColor = Color.Silver;
            lblParcela.Location = new Point(528, 126);
            lblParcela.Name = "lblParcela";
            lblParcela.Size = new Size(48, 15);
            lblParcela.TabIndex = 20;
            lblParcela.Text = "Parcela:";
            // 
            // txtParcela
            // 
            txtParcela.Location = new Point(528, 144);
            txtParcela.Margin = new Padding(3, 2, 3, 2);
            txtParcela.Name = "txtParcela";
            txtParcela.Size = new Size(245, 23);
            txtParcela.TabIndex = 21;
            // 
            // lblDireccion
            // 
            lblDireccion.AutoSize = true;
            lblDireccion.ForeColor = Color.Silver;
            lblDireccion.Location = new Point(786, 126);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new Size(60, 15);
            lblDireccion.TabIndex = 22;
            lblDireccion.Text = "Dirección:";
            // 
            // txtDireccion
            // 
            txtDireccion.Location = new Point(786, 144);
            txtDireccion.Margin = new Padding(3, 2, 3, 2);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(245, 23);
            txtDireccion.TabIndex = 23;
            // 
            // lblNumeroPlano
            // 
            lblNumeroPlano.AutoSize = true;
            lblNumeroPlano.ForeColor = Color.Silver;
            lblNumeroPlano.Location = new Point(12, 174);
            lblNumeroPlano.Name = "lblNumeroPlano";
            lblNumeroPlano.Size = new Size(103, 15);
            lblNumeroPlano.TabIndex = 24;
            lblNumeroPlano.Text = "Número de Plano:";
            // 
            // txtNumeroPlano
            // 
            txtNumeroPlano.Location = new Point(12, 192);
            txtNumeroPlano.Margin = new Padding(3, 2, 3, 2);
            txtNumeroPlano.Name = "txtNumeroPlano";
            txtNumeroPlano.Size = new Size(245, 23);
            txtNumeroPlano.TabIndex = 25;
            // 
            // lblObservaciones
            // 
            lblObservaciones.AutoSize = true;
            lblObservaciones.ForeColor = Color.Silver;
            lblObservaciones.Location = new Point(270, 174);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(87, 15);
            lblObservaciones.TabIndex = 26;
            lblObservaciones.Text = "Observaciones:";
            // 
            // txtObservaciones
            // 
            txtObservaciones.Location = new Point(270, 192);
            txtObservaciones.Margin = new Padding(3, 2, 3, 2);
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.Size = new Size(245, 23);
            txtObservaciones.TabIndex = 27;
            // 
            // lblDescripcionLote
            // 
            lblDescripcionLote.AutoSize = true;
            lblDescripcionLote.ForeColor = Color.Silver;
            lblDescripcionLote.Location = new Point(528, 174);
            lblDescripcionLote.Name = "lblDescripcionLote";
            lblDescripcionLote.Size = new Size(98, 15);
            lblDescripcionLote.TabIndex = 30;
            lblDescripcionLote.Text = "Descripción Lote:";
            // 
            // txtDescripcionLote
            // 
            txtDescripcionLote.Location = new Point(528, 192);
            txtDescripcionLote.Margin = new Padding(3, 2, 3, 2);
            txtDescripcionLote.Name = "txtDescripcionLote";
            txtDescripcionLote.Size = new Size(245, 23);
            txtDescripcionLote.TabIndex = 31;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = Color.FromArgb(0, 122, 204);
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(781, 191);
            btnBuscar.Margin = new Padding(3, 2, 3, 2);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(120, 26);
            btnBuscar.TabIndex = 28;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.FromArgb(96, 96, 96);
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.ForeColor = Color.White;
            btnLimpiar.Location = new Point(911, 191);
            btnLimpiar.Margin = new Padding(3, 2, 3, 2);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(120, 26);
            btnLimpiar.TabIndex = 29;
            btnLimpiar.Text = "Limpiar Filtros";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // groupBoxGrilla
            // 
            groupBoxGrilla.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxGrilla.Controls.Add(dgvPlanos);
            groupBoxGrilla.Dock = DockStyle.Fill;
            groupBoxGrilla.ForeColor = Color.White;
            groupBoxGrilla.Location = new Point(0, 264);
            groupBoxGrilla.Margin = new Padding(3, 2, 3, 2);
            groupBoxGrilla.Name = "groupBoxGrilla";
            groupBoxGrilla.Padding = new Padding(9, 24, 9, 8);
            groupBoxGrilla.Size = new Size(1060, 356);
            groupBoxGrilla.TabIndex = 1;
            groupBoxGrilla.TabStop = false;
            groupBoxGrilla.Text = "Resultado";
            // 
            // dgvPlanos
            // 
            dgvPlanos.AllowUserToAddRows = false;
            dgvPlanos.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(40, 40, 40);
            dgvPlanos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvPlanos.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvPlanos.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(60, 60, 60);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvPlanos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvPlanos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(30, 30, 30);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvPlanos.DefaultCellStyle = dataGridViewCellStyle3;
            dgvPlanos.Dock = DockStyle.Fill;
            dgvPlanos.EnableHeadersVisualStyles = false;
            dgvPlanos.GridColor = Color.FromArgb(63, 63, 70);
            dgvPlanos.Location = new Point(9, 40);
            dgvPlanos.Margin = new Padding(3, 2, 3, 2);
            dgvPlanos.MultiSelect = false;
            dgvPlanos.Name = "dgvPlanos";
            dgvPlanos.ReadOnly = true;
            dgvPlanos.RowHeadersWidth = 51;
            dgvPlanos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPlanos.Size = new Size(1042, 308);
            dgvPlanos.TabIndex = 0;
            // 
            // panelAcciones
            // 
            panelAcciones.BackColor = Color.FromArgb(45, 45, 48);
            panelAcciones.Controls.Add(btnCerrar);
            panelAcciones.Controls.Add(btnExportarExcel);
            panelAcciones.Controls.Add(btnAuditarPlano);
            panelAcciones.Controls.Add(lblCantidadRegistros);
            panelAcciones.Controls.Add(btnReubicarLote);
            panelAcciones.Dock = DockStyle.Bottom;
            panelAcciones.Location = new Point(0, 620);
            panelAcciones.Margin = new Padding(3, 2, 3, 2);
            panelAcciones.Name = "panelAcciones";
            panelAcciones.Size = new Size(1060, 46);
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
            // btnExportarExcel
            // 
            btnExportarExcel.BackColor = Color.FromArgb(16, 124, 16);
            btnExportarExcel.FlatStyle = FlatStyle.Flat;
            btnExportarExcel.ForeColor = Color.White;
            btnExportarExcel.Location = new Point(180, 8);
            btnExportarExcel.Margin = new Padding(3, 2, 3, 2);
            btnExportarExcel.Name = "btnExportarExcel";
            btnExportarExcel.Size = new Size(150, 30);
            btnExportarExcel.TabIndex = 1;
            btnExportarExcel.Text = "Exportar a Excel";
            btnExportarExcel.UseVisualStyleBackColor = false;
            btnExportarExcel.Click += btnExportarExcel_Click;
            // 
            // btnAuditarPlano
            // 
            btnAuditarPlano.BackColor = Color.FromArgb(0, 122, 204);
            btnAuditarPlano.FlatStyle = FlatStyle.Flat;
            btnAuditarPlano.ForeColor = Color.White;
            btnAuditarPlano.Location = new Point(342, 8);
            btnAuditarPlano.Margin = new Padding(3, 2, 3, 2);
            btnAuditarPlano.Name = "btnAuditarPlano";
            btnAuditarPlano.Size = new Size(150, 30);
            btnAuditarPlano.TabIndex = 2;
            btnAuditarPlano.Text = "Auditar Plano";
            btnAuditarPlano.UseVisualStyleBackColor = false;
            btnAuditarPlano.Click += btnAuditarPlano_Click;
            // 
            // lblCantidadRegistros
            // 
            lblCantidadRegistros.AutoSize = true;
            lblCantidadRegistros.ForeColor = Color.White;
            lblCantidadRegistros.Location = new Point(510, 15);
            lblCantidadRegistros.Name = "lblCantidadRegistros";
            lblCantidadRegistros.Size = new Size(67, 15);
            lblCantidadRegistros.TabIndex = 3;
            lblCantidadRegistros.Text = "Registros: 0";
            // 
            // btnReubicarLote
            // 
            btnReubicarLote.BackColor = Color.FromArgb(193, 106, 0);
            btnReubicarLote.FlatStyle = FlatStyle.Flat;
            btnReubicarLote.ForeColor = Color.White;
            btnReubicarLote.Location = new Point(704, 8);
            btnReubicarLote.Margin = new Padding(3, 2, 3, 2);
            btnReubicarLote.Name = "btnReubicarLote";
            btnReubicarLote.Size = new Size(180, 30);
            btnReubicarLote.TabIndex = 4;
            btnReubicarLote.Text = "Reubicar en nuevo Lote";
            btnReubicarLote.UseVisualStyleBackColor = false;
            btnReubicarLote.Click += btnReubicarLote_Click;
            // 
            // FrmConsultaPlanos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(1060, 666);
            Controls.Add(groupBoxGrilla);
            Controls.Add(groupBoxFiltros);
            Controls.Add(panelAcciones);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmConsultaPlanos";
            Text = "Consulta de Planos";
            Load += FrmConsultaPlanos_Load;
            groupBoxFiltros.ResumeLayout(false);
            groupBoxFiltros.PerformLayout();
            groupBoxGrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPlanos).EndInit();
            panelAcciones.ResumeLayout(false);
            panelAcciones.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxFiltros;
        private Label lblCdLote;
        private TextBox txtCdLote;
        private Label lblEstadoLote;
        private ComboBox cboEstadoLote;
        private Label lblEntrega;
        private TextBox txtEntrega;
        private Label lblCarpeta;
        private TextBox txtCarpeta;
        private Label lblEstadoControl;
        private ComboBox cboEstadoControl;
        private Label lblCategoriaPlano;
        private ComboBox cboCategoriaPlano;
        private Label lblTipoPlano;
        private ComboBox cboTipoPlano;
        private Label lblExpediente;
        private TextBox txtExpediente;
        private Label lblSeccion;
        private TextBox txtSeccion;
        private Label lblManzana;
        private TextBox txtManzana;
        private Label lblParcela;
        private TextBox txtParcela;
        private Label lblDireccion;
        private TextBox txtDireccion;
        private Label lblNumeroPlano;
        private TextBox txtNumeroPlano;
        private Label lblObservaciones;
        private TextBox txtObservaciones;
        private Label lblDescripcionLote;
        private TextBox txtDescripcionLote;
        private Button btnBuscar;
        private Button btnLimpiar;
        private GroupBox groupBoxGrilla;
        private DataGridView dgvPlanos;
        private Panel panelAcciones;
        private Button btnCerrar;
        private Button btnExportarExcel;
        private Button btnAuditarPlano;
        private Label lblCantidadRegistros;
        private Button btnReubicarLote;
    }
}
