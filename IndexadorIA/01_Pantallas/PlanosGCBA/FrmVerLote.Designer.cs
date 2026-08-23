namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmVerLote
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
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            panelEncabezado = new Panel();
            lblFechaProcesamientoIATitulo = new Label();
            lblFechaProcesamientoIA = new Label();
            lblFechaCreacionTitulo = new Label();
            lblFechaCreacion = new Label();
            lblCantidadArchivosTitulo = new Label();
            lblCantidadArchivos = new Label();
            lblNombreLoteTitulo = new Label();
            lblNombreLote = new Label();
            lblDireccionLote = new Label();
            splitContainerPrincipal = new SplitContainer();
            panelIzquierdo = new Panel();
            dgvArchivos = new DataGridView();
            groupBoxFiltros = new GroupBox();
            btnLimpiarFiltrosPagina = new Button();
            btnAplicarFiltros = new Button();
            nudExactitudMinima = new NumericUpDown();
            lblExactitudMinima = new Label();
            chkFaltaDireccion = new CheckBox();
            chkFaltaParcela = new CheckBox();
            chkFaltaManzana = new CheckBox();
            chkFaltaSeccion = new CheckBox();
            chkFaltaExpediente = new CheckBox();
            chkFaltaTipoPlano = new CheckBox();
            chkFaltaCategoriaPlano = new CheckBox();
            chkFaltaNumeroPlano = new CheckBox();
            cboTipoPlanoFiltro = new ComboBox();
            lblTipoPlanoFiltro = new Label();
            cboCategoriaPlanoFiltro = new ComboBox();
            lblCategoriaPlanoFiltro = new Label();
            chkMostrarTodos = new CheckBox();
            splitContainerDetalle = new SplitContainer();
            panelCentro = new Panel();
            groupBoxDetalle = new GroupBox();
            txtDireccion = new TextBox();
            lblConfianzaDireccion = new Label();
            lblDireccionCampo = new Label();
            txtParcela = new TextBox();
            lblConfianzaParcela = new Label();
            lblParcelaCampo = new Label();
            txtManzana = new TextBox();
            lblConfianzaManzana = new Label();
            lblManzanaCampo = new Label();
            txtSeccion = new TextBox();
            lblConfianzaSeccion = new Label();
            lblSeccionCampo = new Label();
            txtExpedienteEx = new TextBox();
            lblExpedienteGuion1 = new Label();
            txtExpedienteAnio = new TextBox();
            lblExpedienteGuion2 = new Label();
            txtExpedienteNumero = new TextBox();
            lblExpedienteGuion3 = new Label();
            txtExpedienteGcaba = new TextBox();
            lblExpedienteGuion4 = new Label();
            txtExpedienteReparticion = new TextBox();
            lblConfianzaExpediente = new Label();
            btnParsearExpedienteDeArchivo = new Button();
            lblExpedienteCampo = new Label();
            cboTipoPlanoDetalle = new ComboBox();
            lblConfianzaTipoPlano = new Label();
            lblTipoPlanoCampo = new Label();
            cboCategoriaPlanoDetalle = new ComboBox();
            lblConfianzaCategoriaPlano = new Label();
            lblCategoriaPlanoCampo = new Label();
            txtNumeroPlano = new TextBox();
            lblConfianzaNumeroPlano = new Label();
            lblNumeroPlanoCampo = new Label();
            lblSnPosibleBlanca = new Label();
            lblSnGirada = new Label();
            lblNuPagina = new Label();
            lblNombreArchivoOriginal = new Label();
            lblCabeceraArchivo = new Label();
            txtObservaciones = new TextBox();
            lblObservacionesCampo = new Label();
            panelDerecho = new Panel();
            panelImagenScroll = new Panel();
            pictureBoxImagen = new PictureBox();
            pictureBoxPdf = new PictureBox();
            panelZoom = new Panel();
            lblZoom = new Label();
            btnZoomAjustar = new Button();
            btnZoomMenos = new Button();
            btnZoomMas = new Button();
            btnVerImagenPDF = new Button();
            btnGirarIzquierda = new Button();
            btnGirarDerecha = new Button();
            panelBotones = new Panel();
            btnMarcarPaginaIlegible = new Button();
            btnMarcarDatosIlegible = new Button();
            btnMarcarLoteCompletado = new Button();
            btnCerrar = new Button();
            btnGuardarControlada = new Button();
            panelEncabezado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerPrincipal).BeginInit();
            splitContainerPrincipal.Panel1.SuspendLayout();
            splitContainerPrincipal.Panel2.SuspendLayout();
            splitContainerPrincipal.SuspendLayout();
            panelIzquierdo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArchivos).BeginInit();
            groupBoxFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudExactitudMinima).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerDetalle).BeginInit();
            splitContainerDetalle.Panel1.SuspendLayout();
            splitContainerDetalle.Panel2.SuspendLayout();
            splitContainerDetalle.SuspendLayout();
            panelCentro.SuspendLayout();
            groupBoxDetalle.SuspendLayout();
            panelDerecho.SuspendLayout();
            panelImagenScroll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImagen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPdf).BeginInit();
            panelZoom.SuspendLayout();
            panelBotones.SuspendLayout();
            SuspendLayout();
            // 
            // panelEncabezado
            // 
            panelEncabezado.BackColor = Color.FromArgb(45, 45, 48);
            panelEncabezado.Controls.Add(lblFechaProcesamientoIATitulo);
            panelEncabezado.Controls.Add(lblFechaProcesamientoIA);
            panelEncabezado.Controls.Add(lblFechaCreacionTitulo);
            panelEncabezado.Controls.Add(lblFechaCreacion);
            panelEncabezado.Controls.Add(lblCantidadArchivosTitulo);
            panelEncabezado.Controls.Add(lblCantidadArchivos);
            panelEncabezado.Controls.Add(lblNombreLoteTitulo);
            panelEncabezado.Controls.Add(lblNombreLote);
            panelEncabezado.Dock = DockStyle.Top;
            panelEncabezado.Location = new Point(0, 0);
            panelEncabezado.Name = "panelEncabezado";
            panelEncabezado.Size = new Size(1400, 60);
            panelEncabezado.TabIndex = 0;
            // 
            // lblFechaProcesamientoIATitulo
            // 
            lblFechaProcesamientoIATitulo.AutoSize = true;
            lblFechaProcesamientoIATitulo.ForeColor = Color.Gainsboro;
            lblFechaProcesamientoIATitulo.Location = new Point(800, 10);
            lblFechaProcesamientoIATitulo.Name = "lblFechaProcesamientoIATitulo";
            lblFechaProcesamientoIATitulo.Size = new Size(137, 15);
            lblFechaProcesamientoIATitulo.TabIndex = 6;
            lblFechaProcesamientoIATitulo.Text = "Fecha Procesamiento IA:";
            // 
            // lblFechaProcesamientoIA
            // 
            lblFechaProcesamientoIA.AutoSize = true;
            lblFechaProcesamientoIA.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFechaProcesamientoIA.ForeColor = Color.White;
            lblFechaProcesamientoIA.Location = new Point(800, 28);
            lblFechaProcesamientoIA.Name = "lblFechaProcesamientoIA";
            lblFechaProcesamientoIA.Size = new Size(12, 15);
            lblFechaProcesamientoIA.TabIndex = 7;
            lblFechaProcesamientoIA.Text = "-";
            // 
            // lblFechaCreacionTitulo
            // 
            lblFechaCreacionTitulo.AutoSize = true;
            lblFechaCreacionTitulo.ForeColor = Color.Gainsboro;
            lblFechaCreacionTitulo.Location = new Point(540, 10);
            lblFechaCreacionTitulo.Name = "lblFechaCreacionTitulo";
            lblFechaCreacionTitulo.Size = new Size(91, 15);
            lblFechaCreacionTitulo.TabIndex = 4;
            lblFechaCreacionTitulo.Text = "Fecha Creación:";
            // 
            // lblFechaCreacion
            // 
            lblFechaCreacion.AutoSize = true;
            lblFechaCreacion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFechaCreacion.ForeColor = Color.White;
            lblFechaCreacion.Location = new Point(540, 28);
            lblFechaCreacion.Name = "lblFechaCreacion";
            lblFechaCreacion.Size = new Size(12, 15);
            lblFechaCreacion.TabIndex = 5;
            lblFechaCreacion.Text = "-";
            // 
            // lblCantidadArchivosTitulo
            // 
            lblCantidadArchivosTitulo.AutoSize = true;
            lblCantidadArchivosTitulo.ForeColor = Color.Gainsboro;
            lblCantidadArchivosTitulo.Location = new Point(320, 10);
            lblCantidadArchivosTitulo.Name = "lblCantidadArchivosTitulo";
            lblCantidadArchivosTitulo.Size = new Size(107, 15);
            lblCantidadArchivosTitulo.TabIndex = 2;
            lblCantidadArchivosTitulo.Text = "Cantidad Archivos:";
            // 
            // lblCantidadArchivos
            // 
            lblCantidadArchivos.AutoSize = true;
            lblCantidadArchivos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCantidadArchivos.ForeColor = Color.White;
            lblCantidadArchivos.Location = new Point(320, 28);
            lblCantidadArchivos.Name = "lblCantidadArchivos";
            lblCantidadArchivos.Size = new Size(12, 15);
            lblCantidadArchivos.TabIndex = 3;
            lblCantidadArchivos.Text = "-";
            // 
            // lblNombreLoteTitulo
            // 
            lblNombreLoteTitulo.AutoSize = true;
            lblNombreLoteTitulo.ForeColor = Color.Gainsboro;
            lblNombreLoteTitulo.Location = new Point(18, 10);
            lblNombreLoteTitulo.Name = "lblNombreLoteTitulo";
            lblNombreLoteTitulo.Size = new Size(80, 15);
            lblNombreLoteTitulo.TabIndex = 0;
            lblNombreLoteTitulo.Text = "Nombre Lote:";
            // 
            // lblNombreLote
            // 
            lblNombreLote.AutoSize = true;
            lblNombreLote.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNombreLote.ForeColor = Color.White;
            lblNombreLote.Location = new Point(18, 28);
            lblNombreLote.Name = "lblNombreLote";
            lblNombreLote.Size = new Size(12, 15);
            lblNombreLote.TabIndex = 1;
            lblNombreLote.Text = "-";
            // 
            // lblDireccionLote
            // 
            lblDireccionLote.Location = new Point(0, 0);
            lblDireccionLote.Name = "lblDireccionLote";
            lblDireccionLote.Size = new Size(100, 23);
            lblDireccionLote.TabIndex = 0;
            // 
            // splitContainerPrincipal
            // 
            splitContainerPrincipal.BackColor = Color.FromArgb(45, 45, 48);
            splitContainerPrincipal.Dock = DockStyle.Fill;
            splitContainerPrincipal.Location = new Point(0, 60);
            splitContainerPrincipal.Name = "splitContainerPrincipal";
            // 
            // splitContainerPrincipal.Panel1
            // 
            splitContainerPrincipal.Panel1.Controls.Add(panelIzquierdo);
            // 
            // splitContainerPrincipal.Panel2
            // 
            splitContainerPrincipal.Panel2.Controls.Add(splitContainerDetalle);
            splitContainerPrincipal.Size = new Size(1400, 690);
            splitContainerPrincipal.SplitterDistance = 459;
            splitContainerPrincipal.TabIndex = 1;
            // 
            // panelIzquierdo
            // 
            panelIzquierdo.Controls.Add(dgvArchivos);
            panelIzquierdo.Controls.Add(groupBoxFiltros);
            panelIzquierdo.Dock = DockStyle.Fill;
            panelIzquierdo.Location = new Point(0, 0);
            panelIzquierdo.Name = "panelIzquierdo";
            panelIzquierdo.Size = new Size(459, 690);
            panelIzquierdo.TabIndex = 0;
            // 
            // dgvArchivos
            // 
            dgvArchivos.AllowUserToAddRows = false;
            dgvArchivos.AllowUserToDeleteRows = false;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(40, 40, 40);
            dgvArchivos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            dgvArchivos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvArchivos.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvArchivos.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = Color.FromArgb(60, 60, 60);
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle8.ForeColor = Color.White;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            dgvArchivos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            dgvArchivos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = Color.FromArgb(30, 30, 30);
            dataGridViewCellStyle9.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle9.ForeColor = Color.White;
            dataGridViewCellStyle9.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dataGridViewCellStyle9.SelectionForeColor = Color.White;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.False;
            dgvArchivos.DefaultCellStyle = dataGridViewCellStyle9;
            dgvArchivos.Dock = DockStyle.Fill;
            dgvArchivos.EnableHeadersVisualStyles = false;
            dgvArchivos.GridColor = Color.FromArgb(63, 63, 70);
            dgvArchivos.Location = new Point(0, 300);
            dgvArchivos.MultiSelect = false;
            dgvArchivos.Name = "dgvArchivos";
            dgvArchivos.ReadOnly = true;
            dgvArchivos.RowHeadersWidth = 51;
            dgvArchivos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArchivos.Size = new Size(459, 390);
            dgvArchivos.TabIndex = 1;
            dgvArchivos.SelectionChanged += dgvArchivos_SelectionChanged;
            // 
            // groupBoxFiltros
            // 
            groupBoxFiltros.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxFiltros.Controls.Add(btnLimpiarFiltrosPagina);
            groupBoxFiltros.Controls.Add(btnAplicarFiltros);
            groupBoxFiltros.Controls.Add(nudExactitudMinima);
            groupBoxFiltros.Controls.Add(lblExactitudMinima);
            groupBoxFiltros.Controls.Add(chkFaltaDireccion);
            groupBoxFiltros.Controls.Add(chkFaltaParcela);
            groupBoxFiltros.Controls.Add(chkFaltaManzana);
            groupBoxFiltros.Controls.Add(chkFaltaSeccion);
            groupBoxFiltros.Controls.Add(chkFaltaExpediente);
            groupBoxFiltros.Controls.Add(chkFaltaTipoPlano);
            groupBoxFiltros.Controls.Add(chkFaltaCategoriaPlano);
            groupBoxFiltros.Controls.Add(chkFaltaNumeroPlano);
            groupBoxFiltros.Controls.Add(cboTipoPlanoFiltro);
            groupBoxFiltros.Controls.Add(lblTipoPlanoFiltro);
            groupBoxFiltros.Controls.Add(cboCategoriaPlanoFiltro);
            groupBoxFiltros.Controls.Add(lblCategoriaPlanoFiltro);
            groupBoxFiltros.Controls.Add(chkMostrarTodos);
            groupBoxFiltros.Dock = DockStyle.Top;
            groupBoxFiltros.ForeColor = Color.White;
            groupBoxFiltros.Location = new Point(0, 0);
            groupBoxFiltros.Name = "groupBoxFiltros";
            groupBoxFiltros.Size = new Size(459, 300);
            groupBoxFiltros.TabIndex = 0;
            groupBoxFiltros.TabStop = false;
            groupBoxFiltros.Text = "Filtros";
            // 
            // btnLimpiarFiltrosPagina
            // 
            btnLimpiarFiltrosPagina.BackColor = Color.FromArgb(70, 70, 70);
            btnLimpiarFiltrosPagina.FlatStyle = FlatStyle.Flat;
            btnLimpiarFiltrosPagina.ForeColor = Color.White;
            btnLimpiarFiltrosPagina.Location = new Point(130, 222);
            btnLimpiarFiltrosPagina.Name = "btnLimpiarFiltrosPagina";
            btnLimpiarFiltrosPagina.Size = new Size(110, 30);
            btnLimpiarFiltrosPagina.TabIndex = 11;
            btnLimpiarFiltrosPagina.Text = "Limpiar";
            btnLimpiarFiltrosPagina.UseVisualStyleBackColor = false;
            btnLimpiarFiltrosPagina.Click += btnLimpiarFiltrosPagina_Click;
            // 
            // btnAplicarFiltros
            // 
            btnAplicarFiltros.BackColor = Color.FromArgb(0, 122, 204);
            btnAplicarFiltros.FlatStyle = FlatStyle.Flat;
            btnAplicarFiltros.ForeColor = Color.White;
            btnAplicarFiltros.Location = new Point(12, 222);
            btnAplicarFiltros.Name = "btnAplicarFiltros";
            btnAplicarFiltros.Size = new Size(110, 30);
            btnAplicarFiltros.TabIndex = 10;
            btnAplicarFiltros.Text = "Aplicar Filtros";
            btnAplicarFiltros.UseVisualStyleBackColor = false;
            btnAplicarFiltros.Click += btnAplicarFiltros_Click;
            // 
            // nudExactitudMinima
            // 
            nudExactitudMinima.BackColor = Color.FromArgb(30, 30, 30);
            nudExactitudMinima.ForeColor = Color.White;
            nudExactitudMinima.Location = new Point(160, 187);
            nudExactitudMinima.Name = "nudExactitudMinima";
            nudExactitudMinima.Size = new Size(80, 23);
            nudExactitudMinima.TabIndex = 9;
            // 
            // lblExactitudMinima
            // 
            lblExactitudMinima.AutoSize = true;
            lblExactitudMinima.ForeColor = Color.White;
            lblExactitudMinima.Location = new Point(12, 190);
            lblExactitudMinima.Name = "lblExactitudMinima";
            lblExactitudMinima.Size = new Size(115, 15);
            lblExactitudMinima.TabIndex = 8;
            lblExactitudMinima.Text = "% Exactitud mínima:";
            // 
            // chkFaltaDireccion
            // 
            chkFaltaDireccion.AutoSize = true;
            chkFaltaDireccion.ForeColor = Color.White;
            chkFaltaDireccion.Location = new Point(180, 160);
            chkFaltaDireccion.Name = "chkFaltaDireccion";
            chkFaltaDireccion.Size = new Size(104, 19);
            chkFaltaDireccion.TabIndex = 7;
            chkFaltaDireccion.Text = "Falta Dirección";
            // 
            // chkFaltaParcela
            // 
            chkFaltaParcela.AutoSize = true;
            chkFaltaParcela.ForeColor = Color.White;
            chkFaltaParcela.Location = new Point(12, 160);
            chkFaltaParcela.Name = "chkFaltaParcela";
            chkFaltaParcela.Size = new Size(92, 19);
            chkFaltaParcela.TabIndex = 6;
            chkFaltaParcela.Text = "Falta Parcela";
            // 
            // chkFaltaManzana
            // 
            chkFaltaManzana.AutoSize = true;
            chkFaltaManzana.ForeColor = Color.White;
            chkFaltaManzana.Location = new Point(180, 135);
            chkFaltaManzana.Name = "chkFaltaManzana";
            chkFaltaManzana.Size = new Size(102, 19);
            chkFaltaManzana.TabIndex = 5;
            chkFaltaManzana.Text = "Falta Manzana";
            // 
            // chkFaltaSeccion
            // 
            chkFaltaSeccion.AutoSize = true;
            chkFaltaSeccion.ForeColor = Color.White;
            chkFaltaSeccion.Location = new Point(12, 135);
            chkFaltaSeccion.Name = "chkFaltaSeccion";
            chkFaltaSeccion.Size = new Size(95, 19);
            chkFaltaSeccion.TabIndex = 4;
            chkFaltaSeccion.Text = "Falta Sección";
            // 
            // chkFaltaExpediente
            // 
            chkFaltaExpediente.AutoSize = true;
            chkFaltaExpediente.ForeColor = Color.White;
            chkFaltaExpediente.Location = new Point(12, 110);
            chkFaltaExpediente.Name = "chkFaltaExpediente";
            chkFaltaExpediente.Size = new Size(111, 19);
            chkFaltaExpediente.TabIndex = 3;
            chkFaltaExpediente.Text = "Falta Expediente";
            // 
            // chkFaltaTipoPlano
            // 
            chkFaltaTipoPlano.AutoSize = true;
            chkFaltaTipoPlano.ForeColor = Color.White;
            chkFaltaTipoPlano.Location = new Point(180, 85);
            chkFaltaTipoPlano.Name = "chkFaltaTipoPlano";
            chkFaltaTipoPlano.Size = new Size(127, 19);
            chkFaltaTipoPlano.TabIndex = 2;
            chkFaltaTipoPlano.Text = "Falta Tipo de Plano";
            // 
            // chkFaltaCategoriaPlano
            // 
            chkFaltaCategoriaPlano.AutoSize = true;
            chkFaltaCategoriaPlano.ForeColor = Color.White;
            chkFaltaCategoriaPlano.Location = new Point(12, 85);
            chkFaltaCategoriaPlano.Name = "chkFaltaCategoriaPlano";
            chkFaltaCategoriaPlano.Size = new Size(154, 19);
            chkFaltaCategoriaPlano.TabIndex = 1;
            chkFaltaCategoriaPlano.Text = "Falta Categoría de Plano";
            // 
            // chkFaltaNumeroPlano
            // 
            chkFaltaNumeroPlano.AutoSize = true;
            chkFaltaNumeroPlano.ForeColor = Color.White;
            chkFaltaNumeroPlano.Location = new Point(180, 110);
            chkFaltaNumeroPlano.Name = "chkFaltaNumeroPlano";
            chkFaltaNumeroPlano.Size = new Size(147, 19);
            chkFaltaNumeroPlano.TabIndex = 4;
            chkFaltaNumeroPlano.Text = "Falta Número de Plano";
            // 
            // cboTipoPlanoFiltro
            // 
            cboTipoPlanoFiltro.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboTipoPlanoFiltro.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboTipoPlanoFiltro.BackColor = Color.FromArgb(30, 30, 30);
            cboTipoPlanoFiltro.DropDownWidth = 500;
            cboTipoPlanoFiltro.ForeColor = Color.White;
            cboTipoPlanoFiltro.FormattingEnabled = true;
            cboTipoPlanoFiltro.Location = new Point(120, 55);
            cboTipoPlanoFiltro.Name = "cboTipoPlanoFiltro";
            cboTipoPlanoFiltro.Size = new Size(320, 23);
            cboTipoPlanoFiltro.TabIndex = 6;
            // 
            // lblTipoPlanoFiltro
            // 
            lblTipoPlanoFiltro.AutoSize = true;
            lblTipoPlanoFiltro.ForeColor = Color.White;
            lblTipoPlanoFiltro.Location = new Point(12, 58);
            lblTipoPlanoFiltro.Name = "lblTipoPlanoFiltro";
            lblTipoPlanoFiltro.Size = new Size(83, 15);
            lblTipoPlanoFiltro.TabIndex = 5;
            lblTipoPlanoFiltro.Text = "Tipo de Plano:";
            // 
            // cboCategoriaPlanoFiltro
            // 
            cboCategoriaPlanoFiltro.BackColor = Color.FromArgb(30, 30, 30);
            cboCategoriaPlanoFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoriaPlanoFiltro.ForeColor = Color.White;
            cboCategoriaPlanoFiltro.FormattingEnabled = true;
            cboCategoriaPlanoFiltro.Location = new Point(120, 25);
            cboCategoriaPlanoFiltro.Name = "cboCategoriaPlanoFiltro";
            cboCategoriaPlanoFiltro.Size = new Size(320, 23);
            cboCategoriaPlanoFiltro.TabIndex = 0;
            // 
            // lblCategoriaPlanoFiltro
            // 
            lblCategoriaPlanoFiltro.AutoSize = true;
            lblCategoriaPlanoFiltro.ForeColor = Color.White;
            lblCategoriaPlanoFiltro.Location = new Point(12, 28);
            lblCategoriaPlanoFiltro.Name = "lblCategoriaPlanoFiltro";
            lblCategoriaPlanoFiltro.Size = new Size(110, 15);
            lblCategoriaPlanoFiltro.TabIndex = 12;
            lblCategoriaPlanoFiltro.Text = "Categoría de Plano:";
            // 
            // chkMostrarTodos
            // 
            chkMostrarTodos.AutoSize = true;
            chkMostrarTodos.ForeColor = Color.White;
            chkMostrarTodos.Location = new Point(12, 257);
            chkMostrarTodos.Name = "chkMostrarTodos";
            chkMostrarTodos.Size = new Size(102, 19);
            chkMostrarTodos.TabIndex = 12;
            chkMostrarTodos.Text = "Mostrar Todos";
            chkMostrarTodos.CheckedChanged += chkMostrarTodos_CheckedChanged;
            // 
            // splitContainerDetalle
            // 
            splitContainerDetalle.BackColor = Color.FromArgb(45, 45, 48);
            splitContainerDetalle.Dock = DockStyle.Fill;
            splitContainerDetalle.Location = new Point(0, 0);
            splitContainerDetalle.Name = "splitContainerDetalle";
            // 
            // splitContainerDetalle.Panel1
            // 
            splitContainerDetalle.Panel1.Controls.Add(panelCentro);
            // 
            // splitContainerDetalle.Panel2
            // 
            splitContainerDetalle.Panel2.Controls.Add(panelDerecho);
            splitContainerDetalle.Size = new Size(937, 690);
            splitContainerDetalle.SplitterDistance = 460;
            splitContainerDetalle.TabIndex = 0;
            // 
            // panelCentro
            // 
            panelCentro.Controls.Add(groupBoxDetalle);
            panelCentro.Dock = DockStyle.Fill;
            panelCentro.Location = new Point(0, 0);
            panelCentro.Name = "panelCentro";
            panelCentro.Size = new Size(460, 690);
            panelCentro.TabIndex = 0;
            // 
            // groupBoxDetalle
            // 
            groupBoxDetalle.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxDetalle.Controls.Add(txtDireccion);
            groupBoxDetalle.Controls.Add(lblConfianzaDireccion);
            groupBoxDetalle.Controls.Add(lblDireccionCampo);
            groupBoxDetalle.Controls.Add(txtParcela);
            groupBoxDetalle.Controls.Add(lblConfianzaParcela);
            groupBoxDetalle.Controls.Add(lblParcelaCampo);
            groupBoxDetalle.Controls.Add(txtManzana);
            groupBoxDetalle.Controls.Add(lblConfianzaManzana);
            groupBoxDetalle.Controls.Add(lblManzanaCampo);
            groupBoxDetalle.Controls.Add(txtSeccion);
            groupBoxDetalle.Controls.Add(lblConfianzaSeccion);
            groupBoxDetalle.Controls.Add(lblSeccionCampo);
            groupBoxDetalle.Controls.Add(txtExpedienteEx);
            groupBoxDetalle.Controls.Add(lblExpedienteGuion1);
            groupBoxDetalle.Controls.Add(txtExpedienteAnio);
            groupBoxDetalle.Controls.Add(lblExpedienteGuion2);
            groupBoxDetalle.Controls.Add(txtExpedienteNumero);
            groupBoxDetalle.Controls.Add(lblExpedienteGuion3);
            groupBoxDetalle.Controls.Add(txtExpedienteGcaba);
            groupBoxDetalle.Controls.Add(lblExpedienteGuion4);
            groupBoxDetalle.Controls.Add(txtExpedienteReparticion);
            groupBoxDetalle.Controls.Add(lblConfianzaExpediente);
            groupBoxDetalle.Controls.Add(btnParsearExpedienteDeArchivo);
            groupBoxDetalle.Controls.Add(lblExpedienteCampo);
            groupBoxDetalle.Controls.Add(cboTipoPlanoDetalle);
            groupBoxDetalle.Controls.Add(lblConfianzaTipoPlano);
            groupBoxDetalle.Controls.Add(lblTipoPlanoCampo);
            groupBoxDetalle.Controls.Add(cboCategoriaPlanoDetalle);
            groupBoxDetalle.Controls.Add(lblConfianzaCategoriaPlano);
            groupBoxDetalle.Controls.Add(lblCategoriaPlanoCampo);
            groupBoxDetalle.Controls.Add(txtNumeroPlano);
            groupBoxDetalle.Controls.Add(lblConfianzaNumeroPlano);
            groupBoxDetalle.Controls.Add(lblNumeroPlanoCampo);
            groupBoxDetalle.Controls.Add(lblSnPosibleBlanca);
            groupBoxDetalle.Controls.Add(lblSnGirada);
            groupBoxDetalle.Controls.Add(lblNuPagina);
            groupBoxDetalle.Controls.Add(lblNombreArchivoOriginal);
            groupBoxDetalle.Controls.Add(lblCabeceraArchivo);
            groupBoxDetalle.Controls.Add(txtObservaciones);
            groupBoxDetalle.Controls.Add(lblObservacionesCampo);
            groupBoxDetalle.Dock = DockStyle.Fill;
            groupBoxDetalle.ForeColor = Color.White;
            groupBoxDetalle.Location = new Point(0, 0);
            groupBoxDetalle.Name = "groupBoxDetalle";
            groupBoxDetalle.Size = new Size(460, 690);
            groupBoxDetalle.TabIndex = 0;
            groupBoxDetalle.TabStop = false;
            groupBoxDetalle.Text = "Detalle de la Página";
            // 
            // txtDireccion
            // 
            txtDireccion.BackColor = Color.FromArgb(30, 30, 30);
            txtDireccion.BorderStyle = BorderStyle.FixedSingle;
            txtDireccion.CharacterCasing = CharacterCasing.Upper;
            txtDireccion.Font = new Font("Segoe UI", 12F);
            txtDireccion.ForeColor = Color.White;
            txtDireccion.Location = new Point(15, 449);
            txtDireccion.Multiline = true;
            txtDireccion.Name = "txtDireccion";
            txtDireccion.ScrollBars = ScrollBars.Vertical;
            txtDireccion.Size = new Size(401, 94);
            txtDireccion.TabIndex = 17;
            // 
            // lblConfianzaDireccion
            // 
            lblConfianzaDireccion.AutoSize = true;
            lblConfianzaDireccion.ForeColor = Color.Yellow;
            lblConfianzaDireccion.Location = new Point(422, 449);
            lblConfianzaDireccion.Name = "lblConfianzaDireccion";
            lblConfianzaDireccion.Size = new Size(30, 15);
            lblConfianzaDireccion.TabIndex = 22;
            lblConfianzaDireccion.Text = "-- %";
            // 
            // lblDireccionCampo
            // 
            lblDireccionCampo.AutoSize = true;
            lblDireccionCampo.ForeColor = Color.White;
            lblDireccionCampo.Location = new Point(15, 431);
            lblDireccionCampo.Name = "lblDireccionCampo";
            lblDireccionCampo.Size = new Size(76, 15);
            lblDireccionCampo.TabIndex = 20;
            lblDireccionCampo.Text = "Dirección (*):";
            // 
            // txtParcela
            // 
            txtParcela.BackColor = Color.FromArgb(30, 30, 30);
            txtParcela.BorderStyle = BorderStyle.FixedSingle;
            txtParcela.Font = new Font("Segoe UI", 12F);
            txtParcela.ForeColor = Color.White;
            txtParcela.Location = new Point(15, 393);
            txtParcela.Name = "txtParcela";
            txtParcela.Size = new Size(300, 29);
            txtParcela.TabIndex = 16;
            // 
            // lblConfianzaParcela
            // 
            lblConfianzaParcela.AutoSize = true;
            lblConfianzaParcela.ForeColor = Color.Yellow;
            lblConfianzaParcela.Location = new Point(325, 396);
            lblConfianzaParcela.Name = "lblConfianzaParcela";
            lblConfianzaParcela.Size = new Size(30, 15);
            lblConfianzaParcela.TabIndex = 19;
            lblConfianzaParcela.Text = "-- %";
            // 
            // lblParcelaCampo
            // 
            lblParcelaCampo.AutoSize = true;
            lblParcelaCampo.ForeColor = Color.White;
            lblParcelaCampo.Location = new Point(15, 375);
            lblParcelaCampo.Name = "lblParcelaCampo";
            lblParcelaCampo.Size = new Size(64, 15);
            lblParcelaCampo.TabIndex = 17;
            lblParcelaCampo.Text = "Parcela (*):";
            // 
            // txtManzana
            // 
            txtManzana.BackColor = Color.FromArgb(30, 30, 30);
            txtManzana.BorderStyle = BorderStyle.FixedSingle;
            txtManzana.CharacterCasing = CharacterCasing.Upper;
            txtManzana.Font = new Font("Segoe UI", 12F);
            txtManzana.ForeColor = Color.White;
            txtManzana.Location = new Point(15, 340);
            txtManzana.Name = "txtManzana";
            txtManzana.Size = new Size(300, 29);
            txtManzana.TabIndex = 15;
            // 
            // lblConfianzaManzana
            // 
            lblConfianzaManzana.AutoSize = true;
            lblConfianzaManzana.ForeColor = Color.Yellow;
            lblConfianzaManzana.Location = new Point(325, 343);
            lblConfianzaManzana.Name = "lblConfianzaManzana";
            lblConfianzaManzana.Size = new Size(30, 15);
            lblConfianzaManzana.TabIndex = 16;
            lblConfianzaManzana.Text = "-- %";
            // 
            // lblManzanaCampo
            // 
            lblManzanaCampo.AutoSize = true;
            lblManzanaCampo.ForeColor = Color.White;
            lblManzanaCampo.Location = new Point(15, 322);
            lblManzanaCampo.Name = "lblManzanaCampo";
            lblManzanaCampo.Size = new Size(74, 15);
            lblManzanaCampo.TabIndex = 14;
            lblManzanaCampo.Text = "Manzana (*):";
            // 
            // txtSeccion
            // 
            txtSeccion.BackColor = Color.FromArgb(30, 30, 30);
            txtSeccion.BorderStyle = BorderStyle.FixedSingle;
            txtSeccion.CharacterCasing = CharacterCasing.Upper;
            txtSeccion.Font = new Font("Segoe UI", 12F);
            txtSeccion.ForeColor = Color.White;
            txtSeccion.Location = new Point(15, 287);
            txtSeccion.Name = "txtSeccion";
            txtSeccion.Size = new Size(300, 29);
            txtSeccion.TabIndex = 14;
            // 
            // lblConfianzaSeccion
            // 
            lblConfianzaSeccion.AutoSize = true;
            lblConfianzaSeccion.ForeColor = Color.Yellow;
            lblConfianzaSeccion.Location = new Point(325, 290);
            lblConfianzaSeccion.Name = "lblConfianzaSeccion";
            lblConfianzaSeccion.Size = new Size(30, 15);
            lblConfianzaSeccion.TabIndex = 13;
            lblConfianzaSeccion.Text = "-- %";
            // 
            // lblSeccionCampo
            // 
            lblSeccionCampo.AutoSize = true;
            lblSeccionCampo.ForeColor = Color.White;
            lblSeccionCampo.Location = new Point(15, 269);
            lblSeccionCampo.Name = "lblSeccionCampo";
            lblSeccionCampo.Size = new Size(67, 15);
            lblSeccionCampo.TabIndex = 11;
            lblSeccionCampo.Text = "Sección (*):";
            // 
            // txtExpedienteEx
            // 
            txtExpedienteEx.BackColor = Color.FromArgb(30, 30, 30);
            txtExpedienteEx.BorderStyle = BorderStyle.FixedSingle;
            txtExpedienteEx.CharacterCasing = CharacterCasing.Upper;
            txtExpedienteEx.Enabled = false;
            txtExpedienteEx.Font = new Font("Segoe UI", 12F);
            txtExpedienteEx.ForeColor = Color.White;
            txtExpedienteEx.Location = new Point(15, 236);
            txtExpedienteEx.Name = "txtExpedienteEx";
            txtExpedienteEx.Size = new Size(35, 29);
            txtExpedienteEx.TabIndex = 9;
            txtExpedienteEx.Text = "EX";
            // 
            // lblExpedienteGuion1
            // 
            lblExpedienteGuion1.AutoSize = true;
            lblExpedienteGuion1.ForeColor = Color.White;
            lblExpedienteGuion1.Location = new Point(52, 243);
            lblExpedienteGuion1.Name = "lblExpedienteGuion1";
            lblExpedienteGuion1.Size = new Size(12, 15);
            lblExpedienteGuion1.TabIndex = 10;
            lblExpedienteGuion1.Text = "-";
            // 
            // txtExpedienteAnio
            // 
            txtExpedienteAnio.BackColor = Color.FromArgb(30, 30, 30);
            txtExpedienteAnio.BorderStyle = BorderStyle.FixedSingle;
            txtExpedienteAnio.CharacterCasing = CharacterCasing.Upper;
            txtExpedienteAnio.Font = new Font("Segoe UI", 12F);
            txtExpedienteAnio.ForeColor = Color.White;
            txtExpedienteAnio.Location = new Point(64, 236);
            txtExpedienteAnio.MaxLength = 4;
            txtExpedienteAnio.Name = "txtExpedienteAnio";
            txtExpedienteAnio.Size = new Size(48, 29);
            txtExpedienteAnio.TabIndex = 10;
            // 
            // lblExpedienteGuion2
            // 
            lblExpedienteGuion2.AutoSize = true;
            lblExpedienteGuion2.ForeColor = Color.White;
            lblExpedienteGuion2.Location = new Point(114, 243);
            lblExpedienteGuion2.Name = "lblExpedienteGuion2";
            lblExpedienteGuion2.Size = new Size(12, 15);
            lblExpedienteGuion2.TabIndex = 12;
            lblExpedienteGuion2.Text = "-";
            // 
            // txtExpedienteNumero
            // 
            txtExpedienteNumero.BackColor = Color.FromArgb(30, 30, 30);
            txtExpedienteNumero.BorderStyle = BorderStyle.FixedSingle;
            txtExpedienteNumero.CharacterCasing = CharacterCasing.Upper;
            txtExpedienteNumero.Font = new Font("Segoe UI", 12F);
            txtExpedienteNumero.ForeColor = Color.White;
            txtExpedienteNumero.Location = new Point(126, 236);
            txtExpedienteNumero.MaxLength = 8;
            txtExpedienteNumero.Name = "txtExpedienteNumero";
            txtExpedienteNumero.Size = new Size(85, 29);
            txtExpedienteNumero.TabIndex = 11;
            // 
            // lblExpedienteGuion3
            // 
            lblExpedienteGuion3.AutoSize = true;
            lblExpedienteGuion3.ForeColor = Color.White;
            lblExpedienteGuion3.Location = new Point(215, 243);
            lblExpedienteGuion3.Name = "lblExpedienteGuion3";
            lblExpedienteGuion3.Size = new Size(12, 15);
            lblExpedienteGuion3.TabIndex = 14;
            lblExpedienteGuion3.Text = "-";
            // 
            // txtExpedienteGcaba
            // 
            txtExpedienteGcaba.BackColor = Color.FromArgb(30, 30, 30);
            txtExpedienteGcaba.BorderStyle = BorderStyle.FixedSingle;
            txtExpedienteGcaba.CharacterCasing = CharacterCasing.Upper;
            txtExpedienteGcaba.Enabled = false;
            txtExpedienteGcaba.Font = new Font("Segoe UI", 12F);
            txtExpedienteGcaba.ForeColor = Color.White;
            txtExpedienteGcaba.Location = new Point(227, 236);
            txtExpedienteGcaba.Name = "txtExpedienteGcaba";
            txtExpedienteGcaba.Size = new Size(55, 29);
            txtExpedienteGcaba.TabIndex = 12;
            txtExpedienteGcaba.Text = "GCABA";
            // 
            // lblExpedienteGuion4
            // 
            lblExpedienteGuion4.AutoSize = true;
            lblExpedienteGuion4.ForeColor = Color.White;
            lblExpedienteGuion4.Location = new Point(285, 243);
            lblExpedienteGuion4.Name = "lblExpedienteGuion4";
            lblExpedienteGuion4.Size = new Size(12, 15);
            lblExpedienteGuion4.TabIndex = 16;
            lblExpedienteGuion4.Text = "-";
            // 
            // txtExpedienteReparticion
            // 
            txtExpedienteReparticion.BackColor = Color.FromArgb(30, 30, 30);
            txtExpedienteReparticion.BorderStyle = BorderStyle.FixedSingle;
            txtExpedienteReparticion.CharacterCasing = CharacterCasing.Upper;
            txtExpedienteReparticion.Font = new Font("Segoe UI", 12F);
            txtExpedienteReparticion.ForeColor = Color.White;
            txtExpedienteReparticion.Location = new Point(297, 236);
            txtExpedienteReparticion.Name = "txtExpedienteReparticion";
            txtExpedienteReparticion.Size = new Size(120, 29);
            txtExpedienteReparticion.TabIndex = 13;
            // 
            // lblConfianzaExpediente
            // 
            lblConfianzaExpediente.AutoSize = true;
            lblConfianzaExpediente.ForeColor = Color.Yellow;
            lblConfianzaExpediente.Location = new Point(422, 239);
            lblConfianzaExpediente.Name = "lblConfianzaExpediente";
            lblConfianzaExpediente.Size = new Size(30, 15);
            lblConfianzaExpediente.TabIndex = 18;
            lblConfianzaExpediente.Text = "-- %";
            // 
            // btnParsearExpedienteDeArchivo
            // 
            btnParsearExpedienteDeArchivo.BackColor = Color.FromArgb(60, 60, 60);
            btnParsearExpedienteDeArchivo.FlatStyle = FlatStyle.Flat;
            btnParsearExpedienteDeArchivo.ForeColor = Color.White;
            btnParsearExpedienteDeArchivo.Location = new Point(458, 237);
            btnParsearExpedienteDeArchivo.Name = "btnParsearExpedienteDeArchivo";
            btnParsearExpedienteDeArchivo.Size = new Size(30, 30);
            btnParsearExpedienteDeArchivo.TabIndex = 19;
            btnParsearExpedienteDeArchivo.Text = "E";
            btnParsearExpedienteDeArchivo.UseVisualStyleBackColor = false;
            btnParsearExpedienteDeArchivo.Click += btnParsearExpedienteDeArchivo_Click;
            // 
            // lblExpedienteCampo
            // 
            lblExpedienteCampo.AutoSize = true;
            lblExpedienteCampo.ForeColor = Color.White;
            lblExpedienteCampo.Location = new Point(15, 218);
            lblExpedienteCampo.Name = "lblExpedienteCampo";
            lblExpedienteCampo.Size = new Size(67, 15);
            lblExpedienteCampo.TabIndex = 8;
            lblExpedienteCampo.Text = "Expediente:";
            // 
            // cboTipoPlanoDetalle
            // 
            cboTipoPlanoDetalle.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboTipoPlanoDetalle.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboTipoPlanoDetalle.BackColor = Color.FromArgb(30, 30, 30);
            cboTipoPlanoDetalle.DropDownWidth = 500;
            cboTipoPlanoDetalle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboTipoPlanoDetalle.ForeColor = Color.White;
            cboTipoPlanoDetalle.FormattingEnabled = true;
            cboTipoPlanoDetalle.Location = new Point(15, 182);
            cboTipoPlanoDetalle.Name = "cboTipoPlanoDetalle";
            cboTipoPlanoDetalle.Size = new Size(401, 29);
            cboTipoPlanoDetalle.TabIndex = 20;
            // 
            // lblConfianzaTipoPlano
            // 
            lblConfianzaTipoPlano.AutoSize = true;
            lblConfianzaTipoPlano.ForeColor = Color.Yellow;
            lblConfianzaTipoPlano.Location = new Point(422, 187);
            lblConfianzaTipoPlano.Name = "lblConfianzaTipoPlano";
            lblConfianzaTipoPlano.Size = new Size(30, 15);
            lblConfianzaTipoPlano.TabIndex = 21;
            lblConfianzaTipoPlano.Text = "-- %";
            // 
            // lblTipoPlanoCampo
            // 
            lblTipoPlanoCampo.AutoSize = true;
            lblTipoPlanoCampo.ForeColor = Color.White;
            lblTipoPlanoCampo.Location = new Point(15, 164);
            lblTipoPlanoCampo.Name = "lblTipoPlanoCampo";
            lblTipoPlanoCampo.Size = new Size(99, 15);
            lblTipoPlanoCampo.TabIndex = 22;
            lblTipoPlanoCampo.Text = "Tipo de Plano (*):";
            // 
            // cboCategoriaPlanoDetalle
            // 
            cboCategoriaPlanoDetalle.BackColor = Color.FromArgb(30, 30, 30);
            cboCategoriaPlanoDetalle.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoriaPlanoDetalle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboCategoriaPlanoDetalle.ForeColor = Color.White;
            cboCategoriaPlanoDetalle.FormattingEnabled = true;
            cboCategoriaPlanoDetalle.Location = new Point(15, 125);
            cboCategoriaPlanoDetalle.Name = "cboCategoriaPlanoDetalle";
            cboCategoriaPlanoDetalle.Size = new Size(401, 29);
            cboCategoriaPlanoDetalle.TabIndex = 17;
            cboCategoriaPlanoDetalle.SelectedIndexChanged += cboCategoriaPlanoDetalle_SelectedIndexChanged;
            // 
            // lblConfianzaCategoriaPlano
            // 
            lblConfianzaCategoriaPlano.AutoSize = true;
            lblConfianzaCategoriaPlano.ForeColor = Color.Yellow;
            lblConfianzaCategoriaPlano.Location = new Point(422, 130);
            lblConfianzaCategoriaPlano.Name = "lblConfianzaCategoriaPlano";
            lblConfianzaCategoriaPlano.Size = new Size(30, 15);
            lblConfianzaCategoriaPlano.TabIndex = 18;
            lblConfianzaCategoriaPlano.Text = "-- %";
            // 
            // lblCategoriaPlanoCampo
            // 
            lblCategoriaPlanoCampo.AutoSize = true;
            lblCategoriaPlanoCampo.ForeColor = Color.White;
            lblCategoriaPlanoCampo.Location = new Point(15, 107);
            lblCategoriaPlanoCampo.Name = "lblCategoriaPlanoCampo";
            lblCategoriaPlanoCampo.Size = new Size(126, 15);
            lblCategoriaPlanoCampo.TabIndex = 19;
            lblCategoriaPlanoCampo.Text = "Categoría de Plano (*):";
            // 
            // txtNumeroPlano
            // 
            txtNumeroPlano.BackColor = Color.FromArgb(30, 30, 30);
            txtNumeroPlano.BorderStyle = BorderStyle.FixedSingle;
            txtNumeroPlano.CharacterCasing = CharacterCasing.Upper;
            txtNumeroPlano.Font = new Font("Segoe UI", 12F);
            txtNumeroPlano.ForeColor = Color.White;
            txtNumeroPlano.Location = new Point(15, 564);
            txtNumeroPlano.Name = "txtNumeroPlano";
            txtNumeroPlano.Size = new Size(300, 29);
            txtNumeroPlano.TabIndex = 18;
            // 
            // lblConfianzaNumeroPlano
            // 
            lblConfianzaNumeroPlano.AutoSize = true;
            lblConfianzaNumeroPlano.ForeColor = Color.Yellow;
            lblConfianzaNumeroPlano.Location = new Point(325, 567);
            lblConfianzaNumeroPlano.Name = "lblConfianzaNumeroPlano";
            lblConfianzaNumeroPlano.Size = new Size(30, 15);
            lblConfianzaNumeroPlano.TabIndex = 24;
            lblConfianzaNumeroPlano.Text = "-- %";
            // 
            // lblNumeroPlanoCampo
            // 
            lblNumeroPlanoCampo.AutoSize = true;
            lblNumeroPlanoCampo.ForeColor = Color.White;
            lblNumeroPlanoCampo.Location = new Point(15, 546);
            lblNumeroPlanoCampo.Name = "lblNumeroPlanoCampo";
            lblNumeroPlanoCampo.Size = new Size(103, 15);
            lblNumeroPlanoCampo.TabIndex = 25;
            lblNumeroPlanoCampo.Text = "Número de Plano:";
            // 
            // lblSnPosibleBlanca
            // 
            lblSnPosibleBlanca.AutoSize = true;
            lblSnPosibleBlanca.ForeColor = Color.Gainsboro;
            lblSnPosibleBlanca.Location = new Point(203, 70);
            lblSnPosibleBlanca.Name = "lblSnPosibleBlanca";
            lblSnPosibleBlanca.Size = new Size(94, 15);
            lblSnPosibleBlanca.TabIndex = 4;
            lblSnPosibleBlanca.Text = "Posible Blanca: -";
            // 
            // lblSnGirada
            // 
            lblSnGirada.AutoSize = true;
            lblSnGirada.ForeColor = Color.Gainsboro;
            lblSnGirada.Location = new Point(114, 70);
            lblSnGirada.Name = "lblSnGirada";
            lblSnGirada.Size = new Size(52, 15);
            lblSnGirada.TabIndex = 3;
            lblSnGirada.Text = "Girada: -";
            // 
            // lblNuPagina
            // 
            lblNuPagina.AutoSize = true;
            lblNuPagina.ForeColor = Color.Gainsboro;
            lblNuPagina.Location = new Point(15, 70);
            lblNuPagina.Name = "lblNuPagina";
            lblNuPagina.Size = new Size(54, 15);
            lblNuPagina.TabIndex = 2;
            lblNuPagina.Text = "Página: -";
            // 
            // lblNombreArchivoOriginal
            // 
            lblNombreArchivoOriginal.AutoSize = true;
            lblNombreArchivoOriginal.ForeColor = Color.Gainsboro;
            lblNombreArchivoOriginal.Location = new Point(15, 52);
            lblNombreArchivoOriginal.Name = "lblNombreArchivoOriginal";
            lblNombreArchivoOriginal.Size = new Size(102, 15);
            lblNombreArchivoOriginal.TabIndex = 1;
            lblNombreArchivoOriginal.Text = "Archivo original: -";
            // 
            // lblCabeceraArchivo
            // 
            lblCabeceraArchivo.AutoSize = true;
            lblCabeceraArchivo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblCabeceraArchivo.ForeColor = Color.White;
            lblCabeceraArchivo.Location = new Point(15, 28);
            lblCabeceraArchivo.Name = "lblCabeceraArchivo";
            lblCabeceraArchivo.Size = new Size(154, 17);
            lblCabeceraArchivo.TabIndex = 0;
            lblCabeceraArchivo.Text = "(seleccione una página)";
            // 
            // txtObservaciones
            // 
            txtObservaciones.BackColor = Color.FromArgb(30, 30, 30);
            txtObservaciones.BorderStyle = BorderStyle.FixedSingle;
            txtObservaciones.Font = new Font("Segoe UI", 10F);
            txtObservaciones.ForeColor = Color.White;
            txtObservaciones.Location = new Point(15, 619);
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Size = new Size(430, 67);
            txtObservaciones.TabIndex = 26;
            // 
            // lblObservacionesCampo
            // 
            lblObservacionesCampo.AutoSize = true;
            lblObservacionesCampo.ForeColor = Color.White;
            lblObservacionesCampo.Location = new Point(15, 601);
            lblObservacionesCampo.Name = "lblObservacionesCampo";
            lblObservacionesCampo.Size = new Size(87, 15);
            lblObservacionesCampo.TabIndex = 27;
            lblObservacionesCampo.Text = "Observaciones:";
            // 
            // panelDerecho
            // 
            panelDerecho.BackColor = Color.FromArgb(30, 30, 30);
            panelDerecho.Controls.Add(panelImagenScroll);
            panelDerecho.Controls.Add(panelZoom);
            panelDerecho.Dock = DockStyle.Fill;
            panelDerecho.Location = new Point(0, 0);
            panelDerecho.Name = "panelDerecho";
            panelDerecho.Size = new Size(473, 690);
            panelDerecho.TabIndex = 0;
            // 
            // panelImagenScroll
            // 
            panelImagenScroll.AutoScroll = true;
            panelImagenScroll.BackColor = Color.FromArgb(20, 20, 20);
            panelImagenScroll.Controls.Add(pictureBoxImagen);
            panelImagenScroll.Controls.Add(pictureBoxPdf);
            panelImagenScroll.Dock = DockStyle.Fill;
            panelImagenScroll.Location = new Point(0, 40);
            panelImagenScroll.Name = "panelImagenScroll";
            panelImagenScroll.Size = new Size(473, 650);
            panelImagenScroll.TabIndex = 1;
            // 
            // pictureBoxImagen
            // 
            pictureBoxImagen.BackColor = Color.FromArgb(20, 20, 20);
            pictureBoxImagen.Location = new Point(0, 0);
            pictureBoxImagen.Name = "pictureBoxImagen";
            pictureBoxImagen.Size = new Size(468, 646);
            pictureBoxImagen.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImagen.TabIndex = 0;
            pictureBoxImagen.TabStop = false;
            // 
            // pictureBoxPdf
            // 
            pictureBoxPdf.BackColor = Color.FromArgb(20, 20, 20);
            pictureBoxPdf.Location = new Point(0, 0);
            pictureBoxPdf.Name = "pictureBoxPdf";
            pictureBoxPdf.Size = new Size(468, 646);
            pictureBoxPdf.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxPdf.TabIndex = 1;
            pictureBoxPdf.TabStop = false;
            pictureBoxPdf.Visible = false;
            // 
            // panelZoom
            // 
            panelZoom.BackColor = Color.FromArgb(45, 45, 48);
            panelZoom.Controls.Add(lblZoom);
            panelZoom.Controls.Add(btnZoomAjustar);
            panelZoom.Controls.Add(btnZoomMenos);
            panelZoom.Controls.Add(btnZoomMas);
            panelZoom.Controls.Add(btnVerImagenPDF);
            panelZoom.Controls.Add(btnGirarIzquierda);
            panelZoom.Controls.Add(btnGirarDerecha);
            panelZoom.Dock = DockStyle.Top;
            panelZoom.Location = new Point(0, 0);
            panelZoom.Name = "panelZoom";
            panelZoom.Size = new Size(473, 40);
            panelZoom.TabIndex = 0;
            // 
            // lblZoom
            // 
            lblZoom.AutoSize = true;
            lblZoom.ForeColor = Color.White;
            lblZoom.Location = new Point(196, 12);
            lblZoom.Name = "lblZoom";
            lblZoom.Size = new Size(38, 15);
            lblZoom.TabIndex = 3;
            lblZoom.Text = "100 %";
            // 
            // btnZoomAjustar
            // 
            btnZoomAjustar.BackColor = Color.FromArgb(70, 70, 70);
            btnZoomAjustar.FlatStyle = FlatStyle.Flat;
            btnZoomAjustar.ForeColor = Color.White;
            btnZoomAjustar.Location = new Point(96, 5);
            btnZoomAjustar.Name = "btnZoomAjustar";
            btnZoomAjustar.Size = new Size(90, 30);
            btnZoomAjustar.TabIndex = 2;
            btnZoomAjustar.Text = "Ajustar";
            btnZoomAjustar.UseVisualStyleBackColor = false;
            btnZoomAjustar.Click += btnZoomAjustar_Click;
            // 
            // btnZoomMenos
            // 
            btnZoomMenos.BackColor = Color.FromArgb(70, 70, 70);
            btnZoomMenos.FlatStyle = FlatStyle.Flat;
            btnZoomMenos.ForeColor = Color.White;
            btnZoomMenos.Location = new Point(54, 5);
            btnZoomMenos.Name = "btnZoomMenos";
            btnZoomMenos.Size = new Size(36, 30);
            btnZoomMenos.TabIndex = 1;
            btnZoomMenos.Text = "-";
            btnZoomMenos.UseVisualStyleBackColor = false;
            btnZoomMenos.Click += btnZoomMenos_Click;
            // 
            // btnZoomMas
            // 
            btnZoomMas.BackColor = Color.FromArgb(70, 70, 70);
            btnZoomMas.FlatStyle = FlatStyle.Flat;
            btnZoomMas.ForeColor = Color.White;
            btnZoomMas.Location = new Point(12, 5);
            btnZoomMas.Name = "btnZoomMas";
            btnZoomMas.Size = new Size(36, 30);
            btnZoomMas.TabIndex = 0;
            btnZoomMas.Text = "+";
            btnZoomMas.UseVisualStyleBackColor = false;
            btnZoomMas.Click += btnZoomMas_Click;
            // 
            // btnVerImagenPDF
            // 
            btnVerImagenPDF.BackColor = Color.FromArgb(70, 70, 70);
            btnVerImagenPDF.FlatStyle = FlatStyle.Flat;
            btnVerImagenPDF.ForeColor = Color.White;
            btnVerImagenPDF.Location = new Point(252, 5);
            btnVerImagenPDF.Margin = new Padding(3, 3, 8, 3);
            btnVerImagenPDF.Name = "btnVerImagenPDF";
            btnVerImagenPDF.Size = new Size(120, 30);
            btnVerImagenPDF.TabIndex = 4;
            btnVerImagenPDF.Text = "Ver imagen PDF";
            btnVerImagenPDF.UseVisualStyleBackColor = false;
            btnVerImagenPDF.Click += btnVerImagenPDF_Click;
            // 
            // btnGirarIzquierda
            // 
            btnGirarIzquierda.BackColor = Color.FromArgb(70, 70, 70);
            btnGirarIzquierda.FlatStyle = FlatStyle.Flat;
            btnGirarIzquierda.ForeColor = Color.White;
            btnGirarIzquierda.Location = new Point(378, 5);
            btnGirarIzquierda.Margin = new Padding(3, 3, 8, 3);
            btnGirarIzquierda.Name = "btnGirarIzquierda";
            btnGirarIzquierda.Size = new Size(36, 30);
            btnGirarIzquierda.TabIndex = 5;
            btnGirarIzquierda.Text = "⟲";
            btnGirarIzquierda.UseVisualStyleBackColor = false;
            btnGirarIzquierda.Click += btnGirarIzquierda_Click;
            // 
            // btnGirarDerecha
            // 
            btnGirarDerecha.BackColor = Color.FromArgb(70, 70, 70);
            btnGirarDerecha.FlatStyle = FlatStyle.Flat;
            btnGirarDerecha.ForeColor = Color.White;
            btnGirarDerecha.Location = new Point(420, 5);
            btnGirarDerecha.Margin = new Padding(3, 3, 8, 3);
            btnGirarDerecha.Name = "btnGirarDerecha";
            btnGirarDerecha.Size = new Size(36, 30);
            btnGirarDerecha.TabIndex = 6;
            btnGirarDerecha.Text = "⟳";
            btnGirarDerecha.UseVisualStyleBackColor = false;
            btnGirarDerecha.Click += btnGirarDerecha_Click;
            // 
            // panelBotones
            // 
            panelBotones.BackColor = Color.FromArgb(45, 45, 48);
            panelBotones.Controls.Add(btnMarcarPaginaIlegible);
            panelBotones.Controls.Add(btnMarcarDatosIlegible);
            panelBotones.Controls.Add(btnMarcarLoteCompletado);
            panelBotones.Controls.Add(btnCerrar);
            panelBotones.Controls.Add(btnGuardarControlada);
            panelBotones.Dock = DockStyle.Bottom;
            panelBotones.Location = new Point(0, 750);
            panelBotones.Name = "panelBotones";
            panelBotones.Padding = new Padding(10);
            panelBotones.Size = new Size(1400, 56);
            panelBotones.TabIndex = 2;
            // 
            // btnMarcarPaginaIlegible
            // 
            btnMarcarPaginaIlegible.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMarcarPaginaIlegible.BackColor = Color.FromArgb(180, 90, 0);
            btnMarcarPaginaIlegible.FlatStyle = FlatStyle.Flat;
            btnMarcarPaginaIlegible.ForeColor = Color.White;
            btnMarcarPaginaIlegible.Location = new Point(885, 13);
            btnMarcarPaginaIlegible.Margin = new Padding(3, 3, 8, 3);
            btnMarcarPaginaIlegible.Name = "btnMarcarPaginaIlegible";
            btnMarcarPaginaIlegible.Size = new Size(170, 34);
            btnMarcarPaginaIlegible.TabIndex = 4;
            btnMarcarPaginaIlegible.Text = "Marcar Página ILEGIBLE";
            btnMarcarPaginaIlegible.UseVisualStyleBackColor = false;
            btnMarcarPaginaIlegible.Click += btnMarcarPaginaIlegible_Click;
            // 
            // btnMarcarDatosIlegible
            // 
            btnMarcarDatosIlegible.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMarcarDatosIlegible.BackColor = Color.FromArgb(180, 90, 0);
            btnMarcarDatosIlegible.FlatStyle = FlatStyle.Flat;
            btnMarcarDatosIlegible.ForeColor = Color.White;
            btnMarcarDatosIlegible.Location = new Point(678, 13);
            btnMarcarDatosIlegible.Margin = new Padding(3, 3, 8, 3);
            btnMarcarDatosIlegible.Name = "btnMarcarDatosIlegible";
            btnMarcarDatosIlegible.Size = new Size(170, 34);
            btnMarcarDatosIlegible.TabIndex = 5;
            btnMarcarDatosIlegible.Text = "Marcar Datos ILEGIBLE";
            btnMarcarDatosIlegible.UseVisualStyleBackColor = false;
            btnMarcarDatosIlegible.Click += btnMarcarDatosIlegible_Click;
            // 
            // btnMarcarLoteCompletado
            // 
            btnMarcarLoteCompletado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMarcarLoteCompletado.BackColor = Color.FromArgb(0, 122, 204);
            btnMarcarLoteCompletado.FlatStyle = FlatStyle.Flat;
            btnMarcarLoteCompletado.ForeColor = Color.White;
            btnMarcarLoteCompletado.Location = new Point(1087, 13);
            btnMarcarLoteCompletado.Margin = new Padding(3, 3, 8, 3);
            btnMarcarLoteCompletado.Name = "btnMarcarLoteCompletado";
            btnMarcarLoteCompletado.Size = new Size(170, 34);
            btnMarcarLoteCompletado.TabIndex = 6;
            btnMarcarLoteCompletado.Text = "Marcar Lote Completado";
            btnMarcarLoteCompletado.UseVisualStyleBackColor = false;
            btnMarcarLoteCompletado.Click += btnMarcarLoteCompletado_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.BackColor = Color.FromArgb(150, 30, 30);
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(1267, 13);
            btnCerrar.Margin = new Padding(3, 3, 8, 3);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(120, 34);
            btnCerrar.TabIndex = 7;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // btnGuardarControlada
            // 
            btnGuardarControlada.BackColor = Color.FromArgb(0, 150, 80);
            btnGuardarControlada.FlatStyle = FlatStyle.Flat;
            btnGuardarControlada.ForeColor = Color.White;
            btnGuardarControlada.Location = new Point(13, 13);
            btnGuardarControlada.Margin = new Padding(3, 3, 8, 3);
            btnGuardarControlada.Name = "btnGuardarControlada";
            btnGuardarControlada.Size = new Size(190, 34);
            btnGuardarControlada.TabIndex = 1;
            btnGuardarControlada.Text = "Guardar y Marcar Controlada";
            btnGuardarControlada.UseVisualStyleBackColor = false;
            btnGuardarControlada.Click += btnGuardarControlada_Click;
            // 
            // FrmVerLote
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(1400, 806);
            Controls.Add(splitContainerPrincipal);
            Controls.Add(panelBotones);
            Controls.Add(panelEncabezado);
            ForeColor = Color.White;
            MinimumSize = new Size(1100, 650);
            Name = "FrmVerLote";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Ver Lote";
            WindowState = FormWindowState.Maximized;
            Load += FrmVerLote_Load;
            panelEncabezado.ResumeLayout(false);
            panelEncabezado.PerformLayout();
            splitContainerPrincipal.Panel1.ResumeLayout(false);
            splitContainerPrincipal.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerPrincipal).EndInit();
            splitContainerPrincipal.ResumeLayout(false);
            panelIzquierdo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvArchivos).EndInit();
            groupBoxFiltros.ResumeLayout(false);
            groupBoxFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudExactitudMinima).EndInit();
            splitContainerDetalle.Panel1.ResumeLayout(false);
            splitContainerDetalle.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerDetalle).EndInit();
            splitContainerDetalle.ResumeLayout(false);
            panelCentro.ResumeLayout(false);
            groupBoxDetalle.ResumeLayout(false);
            groupBoxDetalle.PerformLayout();
            panelDerecho.ResumeLayout(false);
            panelImagenScroll.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxImagen).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPdf).EndInit();
            panelZoom.ResumeLayout(false);
            panelZoom.PerformLayout();
            panelBotones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelEncabezado;
        private Label lblDireccionLote;
        private Label lblFechaProcesamientoIATitulo;
        private Label lblFechaProcesamientoIA;
        private Label lblFechaCreacionTitulo;
        private Label lblFechaCreacion;
        private Label lblCantidadArchivosTitulo;
        private Label lblCantidadArchivos;
        private Label lblNombreLoteTitulo;
        private Label lblNombreLote;
        private SplitContainer splitContainerPrincipal;
        private Panel panelIzquierdo;
        private DataGridView dgvArchivos;
        private GroupBox groupBoxFiltros;
        private Button btnLimpiarFiltrosPagina;
        private Button btnAplicarFiltros;
        private NumericUpDown nudExactitudMinima;
        private Label lblExactitudMinima;
        private CheckBox chkFaltaDireccion;
        private CheckBox chkFaltaParcela;
        private CheckBox chkFaltaManzana;
        private CheckBox chkFaltaSeccion;
        private CheckBox chkFaltaExpediente;
        private CheckBox chkFaltaTipoPlano;
        private CheckBox chkFaltaCategoriaPlano;
        private CheckBox chkFaltaNumeroPlano;
        private ComboBox cboTipoPlanoFiltro;
        private Label lblTipoPlanoFiltro;
        private ComboBox cboCategoriaPlanoFiltro;
        private Label lblCategoriaPlanoFiltro;
        private CheckBox chkMostrarTodos;
        private SplitContainer splitContainerDetalle;
        private Panel panelCentro;
        private GroupBox groupBoxDetalle;
        private TextBox txtDireccion;
        private Label lblConfianzaDireccion;
        private Label lblDireccionCampo;
        private TextBox txtParcela;
        private Label lblConfianzaParcela;
        private Label lblParcelaCampo;
        private TextBox txtManzana;
        private Label lblConfianzaManzana;
        private Label lblManzanaCampo;
        private TextBox txtSeccion;
        private Label lblConfianzaSeccion;
        private Label lblSeccionCampo;
        private TextBox txtExpedienteEx;
        private Label lblExpedienteGuion1;
        private TextBox txtExpedienteAnio;
        private Label lblExpedienteGuion2;
        private TextBox txtExpedienteNumero;
        private Label lblExpedienteGuion3;
        private TextBox txtExpedienteGcaba;
        private Label lblExpedienteGuion4;
        private TextBox txtExpedienteReparticion;
        private Label lblConfianzaExpediente;
        private Button btnParsearExpedienteDeArchivo;
        private Label lblExpedienteCampo;
        private ComboBox cboTipoPlanoDetalle;
        private Label lblConfianzaTipoPlano;
        private Label lblTipoPlanoCampo;
        private ComboBox cboCategoriaPlanoDetalle;
        private Label lblConfianzaCategoriaPlano;
        private Label lblCategoriaPlanoCampo;
        private TextBox txtNumeroPlano;
        private Label lblConfianzaNumeroPlano;
        private Label lblNumeroPlanoCampo;
        private Label lblSnPosibleBlanca;
        private Label lblSnGirada;
        private Label lblNuPagina;
        private Label lblNombreArchivoOriginal;
        private Label lblCabeceraArchivo;
        private TextBox txtObservaciones;
        private Label lblObservacionesCampo;
        private Panel panelDerecho;
        private Panel panelImagenScroll;
        private PictureBox pictureBoxImagen;
        private Panel panelZoom;
        private Button btnZoomMas;
        private Button btnZoomMenos;
        private Button btnZoomAjustar;
        private Label lblZoom;
        private PictureBox pictureBoxPdf;
        private Panel panelBotones;
        private Button btnVerImagenPDF;
        private Button btnGuardarControlada;
        private Button btnGirarIzquierda;
        private Button btnGirarDerecha;
        private Button btnMarcarPaginaIlegible;
        private Button btnMarcarDatosIlegible;
        private Button btnCerrar;
        private Button btnMarcarLoteCompletado;
    }
}
