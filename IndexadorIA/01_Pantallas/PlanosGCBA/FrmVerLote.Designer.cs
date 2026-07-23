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
            panelEncabezado = new Panel();
            lblDireccionLote = new Label();
            lblFechaProcesamientoIATitulo = new Label();
            lblFechaProcesamientoIA = new Label();
            lblFechaCreacionTitulo = new Label();
            lblFechaCreacion = new Label();
            lblCantidadArchivosTitulo = new Label();
            lblCantidadArchivos = new Label();
            lblNombreLoteTitulo = new Label();
            lblNombreLote = new Label();
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
            cboTipoPlanoFiltro = new ComboBox();
            lblTipoPlanoFiltro = new Label();
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
            txtExpediente = new TextBox();
            lblConfianzaExpediente = new Label();
            lblExpedienteCampo = new Label();
            cboTipoPlanoDetalle = new ComboBox();
            lblConfianzaTipoPlano = new Label();
            lblTipoPlanoCampo = new Label();
            lblSnPosibleBlanca = new Label();
            lblSnGirada = new Label();
            lblNuPagina = new Label();
            lblNombreArchivoOriginal = new Label();
            lblCabeceraArchivo = new Label();
            panelDerecho = new Panel();
            panelImagenScroll = new Panel();
            pictureBoxImagen = new PictureBox();
            panelZoom = new Panel();
            btnZoomMas = new Button();
            btnZoomMenos = new Button();
            btnZoomAjustar = new Button();
            lblZoom = new Label();
            pictureBoxPdf = new PictureBox();
            panelBotones = new FlowLayoutPanel();
            btnVerImagenPDF = new Button();
            btnGuardarControlada = new Button();
            btnGirarIzquierda = new Button();
            btnGirarDerecha = new Button();
            btnMarcarPaginaIlegible = new Button();
            btnMarcarDatosIlegible = new Button();
            btnCerrar = new Button();
            btnMarcarLoteCompletado = new Button();
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
            lblNombreLote.Size = new Size(14, 17);
            lblNombreLote.TabIndex = 1;
            lblNombreLote.Text = "-";
            //
            // lblCantidadArchivosTitulo
            //
            lblCantidadArchivosTitulo.AutoSize = true;
            lblCantidadArchivosTitulo.ForeColor = Color.Gainsboro;
            lblCantidadArchivosTitulo.Location = new Point(320, 10);
            lblCantidadArchivosTitulo.Name = "lblCantidadArchivosTitulo";
            lblCantidadArchivosTitulo.Size = new Size(103, 15);
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
            lblCantidadArchivos.Size = new Size(14, 17);
            lblCantidadArchivos.TabIndex = 3;
            lblCantidadArchivos.Text = "-";
            //
            // lblFechaCreacionTitulo
            //
            lblFechaCreacionTitulo.AutoSize = true;
            lblFechaCreacionTitulo.ForeColor = Color.Gainsboro;
            lblFechaCreacionTitulo.Location = new Point(540, 10);
            lblFechaCreacionTitulo.Name = "lblFechaCreacionTitulo";
            lblFechaCreacionTitulo.Size = new Size(93, 15);
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
            lblFechaCreacion.Size = new Size(14, 17);
            lblFechaCreacion.TabIndex = 5;
            lblFechaCreacion.Text = "-";
            //
            // lblFechaProcesamientoIATitulo
            //
            lblFechaProcesamientoIATitulo.AutoSize = true;
            lblFechaProcesamientoIATitulo.ForeColor = Color.Gainsboro;
            lblFechaProcesamientoIATitulo.Location = new Point(800, 10);
            lblFechaProcesamientoIATitulo.Name = "lblFechaProcesamientoIATitulo";
            lblFechaProcesamientoIATitulo.Size = new Size(133, 15);
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
            lblFechaProcesamientoIA.Size = new Size(14, 17);
            lblFechaProcesamientoIA.TabIndex = 7;
            lblFechaProcesamientoIA.Text = "-";
            //
            // splitContainerPrincipal
            //
            splitContainerPrincipal.BackColor = Color.FromArgb(45, 45, 48);
            splitContainerPrincipal.Dock = DockStyle.Fill;
            splitContainerPrincipal.Location = new Point(0, 60);
            splitContainerPrincipal.Name = "splitContainerPrincipal";
            splitContainerPrincipal.Panel1.Controls.Add(panelIzquierdo);
            splitContainerPrincipal.Panel2.Controls.Add(splitContainerDetalle);
            splitContainerPrincipal.Size = new Size(1400, 690);
            splitContainerPrincipal.SplitterDistance = 460;
            splitContainerPrincipal.TabIndex = 1;
            //
            // panelIzquierdo
            //
            panelIzquierdo.Controls.Add(dgvArchivos);
            panelIzquierdo.Controls.Add(groupBoxFiltros);
            panelIzquierdo.Dock = DockStyle.Fill;
            panelIzquierdo.Location = new Point(0, 0);
            panelIzquierdo.Name = "panelIzquierdo";
            panelIzquierdo.Size = new Size(460, 690);
            panelIzquierdo.TabIndex = 0;
            //
            // dgvArchivos
            //
            dgvArchivos.AllowUserToAddRows = false;
            dgvArchivos.AllowUserToDeleteRows = false;
            dgvArchivos.AllowUserToOrderColumns = false;
            dgvArchivos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvArchivos.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvArchivos.BorderStyle = BorderStyle.None;
            dgvArchivos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvArchivos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvArchivos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArchivos.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvArchivos.DefaultCellStyle.ForeColor = Color.White;
            dgvArchivos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvArchivos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvArchivos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvArchivos.Dock = DockStyle.Fill;
            dgvArchivos.EnableHeadersVisualStyles = false;
            dgvArchivos.GridColor = Color.FromArgb(63, 63, 70);
            dgvArchivos.Location = new Point(0, 230);
            dgvArchivos.MultiSelect = false;
            dgvArchivos.Name = "dgvArchivos";
            dgvArchivos.ReadOnly = true;
            dgvArchivos.RowHeadersWidth = 51;
            dgvArchivos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArchivos.Size = new Size(460, 460);
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
            groupBoxFiltros.Controls.Add(cboTipoPlanoFiltro);
            groupBoxFiltros.Controls.Add(lblTipoPlanoFiltro);
            groupBoxFiltros.Controls.Add(chkMostrarTodos);
            groupBoxFiltros.Dock = DockStyle.Top;
            groupBoxFiltros.ForeColor = Color.White;
            groupBoxFiltros.Location = new Point(0, 0);
            groupBoxFiltros.Name = "groupBoxFiltros";
            groupBoxFiltros.Size = new Size(460, 260);
            groupBoxFiltros.TabIndex = 0;
            groupBoxFiltros.TabStop = false;
            groupBoxFiltros.Text = "Filtros";
            //
            // lblTipoPlanoFiltro
            //
            lblTipoPlanoFiltro.AutoSize = true;
            lblTipoPlanoFiltro.ForeColor = Color.White;
            lblTipoPlanoFiltro.Location = new Point(12, 28);
            lblTipoPlanoFiltro.Name = "lblTipoPlanoFiltro";
            lblTipoPlanoFiltro.Size = new Size(72, 15);
            lblTipoPlanoFiltro.TabIndex = 0;
            lblTipoPlanoFiltro.Text = "Tipo de Plano:";
            //
            // cboTipoPlanoFiltro
            //
            cboTipoPlanoFiltro.BackColor = Color.FromArgb(30, 30, 30);
            cboTipoPlanoFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoPlanoFiltro.ForeColor = Color.White;
            cboTipoPlanoFiltro.FormattingEnabled = true;
            cboTipoPlanoFiltro.Location = new Point(120, 25);
            cboTipoPlanoFiltro.Name = "cboTipoPlanoFiltro";
            cboTipoPlanoFiltro.Size = new Size(320, 23);
            cboTipoPlanoFiltro.TabIndex = 1;
            //
            // chkFaltaTipoPlano
            //
            chkFaltaTipoPlano.AutoSize = true;
            chkFaltaTipoPlano.ForeColor = Color.White;
            chkFaltaTipoPlano.Location = new Point(12, 55);
            chkFaltaTipoPlano.Name = "chkFaltaTipoPlano";
            chkFaltaTipoPlano.Size = new Size(122, 19);
            chkFaltaTipoPlano.TabIndex = 2;
            chkFaltaTipoPlano.Text = "Falta Tipo de Plano";
            //
            // chkFaltaExpediente
            //
            chkFaltaExpediente.AutoSize = true;
            chkFaltaExpediente.ForeColor = Color.White;
            chkFaltaExpediente.Location = new Point(12, 80);
            chkFaltaExpediente.Name = "chkFaltaExpediente";
            chkFaltaExpediente.Size = new Size(104, 19);
            chkFaltaExpediente.TabIndex = 3;
            chkFaltaExpediente.Text = "Falta Expediente";
            //
            // chkFaltaSeccion
            //
            chkFaltaSeccion.AutoSize = true;
            chkFaltaSeccion.ForeColor = Color.White;
            chkFaltaSeccion.Location = new Point(12, 105);
            chkFaltaSeccion.Name = "chkFaltaSeccion";
            chkFaltaSeccion.Size = new Size(90, 19);
            chkFaltaSeccion.TabIndex = 4;
            chkFaltaSeccion.Text = "Falta Sección";
            //
            // chkFaltaManzana
            //
            chkFaltaManzana.AutoSize = true;
            chkFaltaManzana.ForeColor = Color.White;
            chkFaltaManzana.Location = new Point(180, 55);
            chkFaltaManzana.Name = "chkFaltaManzana";
            chkFaltaManzana.Size = new Size(96, 19);
            chkFaltaManzana.TabIndex = 5;
            chkFaltaManzana.Text = "Falta Manzana";
            //
            // chkFaltaParcela
            //
            chkFaltaParcela.AutoSize = true;
            chkFaltaParcela.ForeColor = Color.White;
            chkFaltaParcela.Location = new Point(180, 80);
            chkFaltaParcela.Name = "chkFaltaParcela";
            chkFaltaParcela.Size = new Size(88, 19);
            chkFaltaParcela.TabIndex = 6;
            chkFaltaParcela.Text = "Falta Parcela";
            //
            // chkFaltaDireccion
            //
            chkFaltaDireccion.AutoSize = true;
            chkFaltaDireccion.ForeColor = Color.White;
            chkFaltaDireccion.Location = new Point(180, 105);
            chkFaltaDireccion.Name = "chkFaltaDireccion";
            chkFaltaDireccion.Size = new Size(99, 19);
            chkFaltaDireccion.TabIndex = 7;
            chkFaltaDireccion.Text = "Falta Dirección";
            //
            // lblExactitudMinima
            //
            lblExactitudMinima.AutoSize = true;
            lblExactitudMinima.ForeColor = Color.White;
            lblExactitudMinima.Location = new Point(12, 138);
            lblExactitudMinima.Name = "lblExactitudMinima";
            lblExactitudMinima.Size = new Size(140, 15);
            lblExactitudMinima.TabIndex = 8;
            lblExactitudMinima.Text = "% Exactitud mínima:";
            //
            // nudExactitudMinima
            //
            nudExactitudMinima.BackColor = Color.FromArgb(30, 30, 30);
            nudExactitudMinima.ForeColor = Color.White;
            nudExactitudMinima.Location = new Point(160, 135);
            nudExactitudMinima.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            nudExactitudMinima.Name = "nudExactitudMinima";
            nudExactitudMinima.Size = new Size(80, 23);
            nudExactitudMinima.TabIndex = 9;
            //
            // btnAplicarFiltros
            //
            btnAplicarFiltros.BackColor = Color.FromArgb(0, 122, 204);
            btnAplicarFiltros.FlatStyle = FlatStyle.Flat;
            btnAplicarFiltros.ForeColor = Color.White;
            btnAplicarFiltros.Location = new Point(12, 170);
            btnAplicarFiltros.Name = "btnAplicarFiltros";
            btnAplicarFiltros.Size = new Size(110, 30);
            btnAplicarFiltros.TabIndex = 10;
            btnAplicarFiltros.Text = "Aplicar Filtros";
            btnAplicarFiltros.UseVisualStyleBackColor = false;
            btnAplicarFiltros.Click += btnAplicarFiltros_Click;
            //
            // btnLimpiarFiltrosPagina
            //
            btnLimpiarFiltrosPagina.BackColor = Color.FromArgb(70, 70, 70);
            btnLimpiarFiltrosPagina.FlatStyle = FlatStyle.Flat;
            btnLimpiarFiltrosPagina.ForeColor = Color.White;
            btnLimpiarFiltrosPagina.Location = new Point(130, 170);
            btnLimpiarFiltrosPagina.Name = "btnLimpiarFiltrosPagina";
            btnLimpiarFiltrosPagina.Size = new Size(110, 30);
            btnLimpiarFiltrosPagina.TabIndex = 11;
            btnLimpiarFiltrosPagina.Text = "Limpiar";
            btnLimpiarFiltrosPagina.UseVisualStyleBackColor = false;
            btnLimpiarFiltrosPagina.Click += btnLimpiarFiltrosPagina_Click;
            //
            // chkMostrarTodos
            //
            chkMostrarTodos.AutoSize = true;
            chkMostrarTodos.ForeColor = Color.White;
            chkMostrarTodos.Location = new Point(12, 205);
            chkMostrarTodos.Name = "chkMostrarTodos";
            chkMostrarTodos.Size = new Size(95, 19);
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
            splitContainerDetalle.Panel1.Controls.Add(panelCentro);
            splitContainerDetalle.Panel2.Controls.Add(panelDerecho);
            splitContainerDetalle.Size = new Size(936, 690);
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
            groupBoxDetalle.Controls.Add(txtExpediente);
            groupBoxDetalle.Controls.Add(lblConfianzaExpediente);
            groupBoxDetalle.Controls.Add(lblExpedienteCampo);
            groupBoxDetalle.Controls.Add(cboTipoPlanoDetalle);
            groupBoxDetalle.Controls.Add(lblConfianzaTipoPlano);
            groupBoxDetalle.Controls.Add(lblTipoPlanoCampo);
            groupBoxDetalle.Controls.Add(lblSnPosibleBlanca);
            groupBoxDetalle.Controls.Add(lblSnGirada);
            groupBoxDetalle.Controls.Add(lblNuPagina);
            groupBoxDetalle.Controls.Add(lblNombreArchivoOriginal);
            groupBoxDetalle.Controls.Add(lblCabeceraArchivo);
            groupBoxDetalle.Dock = DockStyle.Fill;
            groupBoxDetalle.ForeColor = Color.White;
            groupBoxDetalle.Location = new Point(0, 0);
            groupBoxDetalle.Name = "groupBoxDetalle";
            groupBoxDetalle.Size = new Size(460, 690);
            groupBoxDetalle.TabIndex = 0;
            groupBoxDetalle.TabStop = false;
            groupBoxDetalle.Text = "Detalle de la Página";
            //
            // lblCabeceraArchivo
            //
            lblCabeceraArchivo.AutoSize = true;
            lblCabeceraArchivo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblCabeceraArchivo.ForeColor = Color.White;
            lblCabeceraArchivo.Location = new Point(15, 28);
            lblCabeceraArchivo.Name = "lblCabeceraArchivo";
            lblCabeceraArchivo.Size = new Size(150, 17);
            lblCabeceraArchivo.TabIndex = 0;
            lblCabeceraArchivo.Text = "(seleccione una página)";
            //
            // lblNombreArchivoOriginal
            //
            lblNombreArchivoOriginal.AutoSize = true;
            lblNombreArchivoOriginal.ForeColor = Color.Gainsboro;
            lblNombreArchivoOriginal.Location = new Point(15, 52);
            lblNombreArchivoOriginal.Name = "lblNombreArchivoOriginal";
            lblNombreArchivoOriginal.Size = new Size(160, 15);
            lblNombreArchivoOriginal.TabIndex = 1;
            lblNombreArchivoOriginal.Text = "Archivo original: -";
            //
            // lblNuPagina
            //
            lblNuPagina.AutoSize = true;
            lblNuPagina.ForeColor = Color.Gainsboro;
            lblNuPagina.Location = new Point(15, 70);
            lblNuPagina.Name = "lblNuPagina";
            lblNuPagina.Size = new Size(70, 15);
            lblNuPagina.TabIndex = 2;
            lblNuPagina.Text = "Página: -";
            //
            // lblSnGirada
            //
            lblSnGirada.AutoSize = true;
            lblSnGirada.ForeColor = Color.Gainsboro;
            lblSnGirada.Location = new Point(15, 88);
            lblSnGirada.Name = "lblSnGirada";
            lblSnGirada.Size = new Size(65, 15);
            lblSnGirada.TabIndex = 3;
            lblSnGirada.Text = "Girada: -";
            //
            // lblSnPosibleBlanca
            //
            lblSnPosibleBlanca.AutoSize = true;
            lblSnPosibleBlanca.ForeColor = Color.Gainsboro;
            lblSnPosibleBlanca.Location = new Point(15, 106);
            lblSnPosibleBlanca.Name = "lblSnPosibleBlanca";
            lblSnPosibleBlanca.Size = new Size(120, 15);
            lblSnPosibleBlanca.TabIndex = 4;
            lblSnPosibleBlanca.Text = "Posible Blanca: -";
            //
            // lblTipoPlanoCampo
            //
            lblTipoPlanoCampo.AutoSize = true;
            lblTipoPlanoCampo.ForeColor = Color.White;
            lblTipoPlanoCampo.Location = new Point(15, 145);
            lblTipoPlanoCampo.Name = "lblTipoPlanoCampo";
            lblTipoPlanoCampo.Size = new Size(125, 15);
            lblTipoPlanoCampo.TabIndex = 5;
            lblTipoPlanoCampo.Text = "Tipo de Plano (*):";
            //
            // cboTipoPlanoDetalle
            //
            cboTipoPlanoDetalle.BackColor = Color.FromArgb(30, 30, 30);
            cboTipoPlanoDetalle.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipoPlanoDetalle.ForeColor = Color.White;
            cboTipoPlanoDetalle.FormattingEnabled = true;
            cboTipoPlanoDetalle.Location = new Point(15, 163);
            cboTipoPlanoDetalle.Name = "cboTipoPlanoDetalle";
            cboTipoPlanoDetalle.Size = new Size(300, 23);
            cboTipoPlanoDetalle.TabIndex = 6;
            //
            // lblConfianzaTipoPlano
            //
            lblConfianzaTipoPlano.AutoSize = true;
            lblConfianzaTipoPlano.ForeColor = Color.Yellow;
            lblConfianzaTipoPlano.Location = new Point(325, 166);
            lblConfianzaTipoPlano.Name = "lblConfianzaTipoPlano";
            lblConfianzaTipoPlano.Size = new Size(45, 15);
            lblConfianzaTipoPlano.TabIndex = 7;
            lblConfianzaTipoPlano.Text = "-- %";
            //
            // lblExpedienteCampo
            //
            lblExpedienteCampo.AutoSize = true;
            lblExpedienteCampo.ForeColor = Color.White;
            lblExpedienteCampo.Location = new Point(15, 198);
            lblExpedienteCampo.Name = "lblExpedienteCampo";
            lblExpedienteCampo.Size = new Size(112, 15);
            lblExpedienteCampo.TabIndex = 8;
            lblExpedienteCampo.Text = "Expediente:";
            //
            // txtExpediente
            //
            txtExpediente.BackColor = Color.FromArgb(30, 30, 30);
            txtExpediente.BorderStyle = BorderStyle.FixedSingle;
            txtExpediente.ForeColor = Color.White;
            txtExpediente.Location = new Point(15, 216);
            txtExpediente.Name = "txtExpediente";
            txtExpediente.Size = new Size(300, 23);
            txtExpediente.TabIndex = 9;
            //
            // lblConfianzaExpediente
            //
            lblConfianzaExpediente.AutoSize = true;
            lblConfianzaExpediente.ForeColor = Color.Yellow;
            lblConfianzaExpediente.Location = new Point(325, 219);
            lblConfianzaExpediente.Name = "lblConfianzaExpediente";
            lblConfianzaExpediente.Size = new Size(45, 15);
            lblConfianzaExpediente.TabIndex = 10;
            lblConfianzaExpediente.Text = "-- %";
            //
            // lblSeccionCampo
            //
            lblSeccionCampo.AutoSize = true;
            lblSeccionCampo.ForeColor = Color.White;
            lblSeccionCampo.Location = new Point(15, 251);
            lblSeccionCampo.Name = "lblSeccionCampo";
            lblSeccionCampo.Size = new Size(90, 15);
            lblSeccionCampo.TabIndex = 11;
            lblSeccionCampo.Text = "Sección (*):";
            //
            // txtSeccion
            //
            txtSeccion.BackColor = Color.FromArgb(30, 30, 30);
            txtSeccion.BorderStyle = BorderStyle.FixedSingle;
            txtSeccion.ForeColor = Color.White;
            txtSeccion.Location = new Point(15, 269);
            txtSeccion.Name = "txtSeccion";
            txtSeccion.Size = new Size(300, 23);
            txtSeccion.TabIndex = 12;
            //
            // lblConfianzaSeccion
            //
            lblConfianzaSeccion.AutoSize = true;
            lblConfianzaSeccion.ForeColor = Color.Yellow;
            lblConfianzaSeccion.Location = new Point(325, 272);
            lblConfianzaSeccion.Name = "lblConfianzaSeccion";
            lblConfianzaSeccion.Size = new Size(45, 15);
            lblConfianzaSeccion.TabIndex = 13;
            lblConfianzaSeccion.Text = "-- %";
            //
            // lblManzanaCampo
            //
            lblManzanaCampo.AutoSize = true;
            lblManzanaCampo.ForeColor = Color.White;
            lblManzanaCampo.Location = new Point(15, 304);
            lblManzanaCampo.Name = "lblManzanaCampo";
            lblManzanaCampo.Size = new Size(96, 15);
            lblManzanaCampo.TabIndex = 14;
            lblManzanaCampo.Text = "Manzana (*):";
            //
            // txtManzana
            //
            txtManzana.BackColor = Color.FromArgb(30, 30, 30);
            txtManzana.BorderStyle = BorderStyle.FixedSingle;
            txtManzana.ForeColor = Color.White;
            txtManzana.Location = new Point(15, 322);
            txtManzana.Name = "txtManzana";
            txtManzana.Size = new Size(300, 23);
            txtManzana.TabIndex = 15;
            //
            // lblConfianzaManzana
            //
            lblConfianzaManzana.AutoSize = true;
            lblConfianzaManzana.ForeColor = Color.Yellow;
            lblConfianzaManzana.Location = new Point(325, 325);
            lblConfianzaManzana.Name = "lblConfianzaManzana";
            lblConfianzaManzana.Size = new Size(45, 15);
            lblConfianzaManzana.TabIndex = 16;
            lblConfianzaManzana.Text = "-- %";
            //
            // lblParcelaCampo
            //
            lblParcelaCampo.AutoSize = true;
            lblParcelaCampo.ForeColor = Color.White;
            lblParcelaCampo.Location = new Point(15, 357);
            lblParcelaCampo.Name = "lblParcelaCampo";
            lblParcelaCampo.Size = new Size(88, 15);
            lblParcelaCampo.TabIndex = 17;
            lblParcelaCampo.Text = "Parcela (*):";
            //
            // txtParcela
            //
            txtParcela.BackColor = Color.FromArgb(30, 30, 30);
            txtParcela.BorderStyle = BorderStyle.FixedSingle;
            txtParcela.ForeColor = Color.White;
            txtParcela.Location = new Point(15, 375);
            txtParcela.Name = "txtParcela";
            txtParcela.Size = new Size(300, 23);
            txtParcela.TabIndex = 18;
            //
            // lblConfianzaParcela
            //
            lblConfianzaParcela.AutoSize = true;
            lblConfianzaParcela.ForeColor = Color.Yellow;
            lblConfianzaParcela.Location = new Point(325, 378);
            lblConfianzaParcela.Name = "lblConfianzaParcela";
            lblConfianzaParcela.Size = new Size(45, 15);
            lblConfianzaParcela.TabIndex = 19;
            lblConfianzaParcela.Text = "-- %";
            //
            // lblDireccionCampo
            //
            lblDireccionCampo.AutoSize = true;
            lblDireccionCampo.ForeColor = Color.White;
            lblDireccionCampo.Location = new Point(15, 410);
            lblDireccionCampo.Name = "lblDireccionCampo";
            lblDireccionCampo.Size = new Size(99, 15);
            lblDireccionCampo.TabIndex = 20;
            lblDireccionCampo.Text = "Dirección (*):";
            //
            // txtDireccion
            //
            txtDireccion.BackColor = Color.FromArgb(30, 30, 30);
            txtDireccion.BorderStyle = BorderStyle.FixedSingle;
            txtDireccion.ForeColor = Color.White;
            txtDireccion.Location = new Point(15, 428);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(300, 23);
            txtDireccion.TabIndex = 21;
            //
            // lblConfianzaDireccion
            //
            lblConfianzaDireccion.AutoSize = true;
            lblConfianzaDireccion.ForeColor = Color.Yellow;
            lblConfianzaDireccion.Location = new Point(325, 431);
            lblConfianzaDireccion.Name = "lblConfianzaDireccion";
            lblConfianzaDireccion.Size = new Size(45, 15);
            lblConfianzaDireccion.TabIndex = 22;
            lblConfianzaDireccion.Text = "-- %";
            //
            // panelDerecho
            //
            panelDerecho.BackColor = Color.FromArgb(30, 30, 30);
            panelDerecho.Controls.Add(panelImagenScroll);
            panelDerecho.Controls.Add(panelZoom);
            panelDerecho.Dock = DockStyle.Fill;
            panelDerecho.Location = new Point(0, 0);
            panelDerecho.Name = "panelDerecho";
            panelDerecho.Size = new Size(472, 690);
            panelDerecho.TabIndex = 0;
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
            panelZoom.Size = new Size(472, 40);
            panelZoom.TabIndex = 0;
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
            // lblZoom
            //
            lblZoom.AutoSize = true;
            lblZoom.ForeColor = Color.White;
            lblZoom.Location = new Point(196, 12);
            lblZoom.Name = "lblZoom";
            lblZoom.Size = new Size(50, 15);
            lblZoom.TabIndex = 3;
            lblZoom.Text = "100 %";
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
            // panelImagenScroll
            //
            panelImagenScroll.AutoScroll = true;
            panelImagenScroll.BackColor = Color.FromArgb(20, 20, 20);
            panelImagenScroll.Controls.Add(pictureBoxImagen);
            panelImagenScroll.Controls.Add(pictureBoxPdf);
            panelImagenScroll.Dock = DockStyle.Fill;
            panelImagenScroll.Location = new Point(0, 40);
            panelImagenScroll.Name = "panelImagenScroll";
            panelImagenScroll.Size = new Size(472, 650);
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
            // panelBotones
            //
            panelBotones.BackColor = Color.FromArgb(45, 45, 48);
            panelBotones.Controls.Add(btnGuardarControlada);
            panelBotones.Controls.Add(btnMarcarPaginaIlegible);
            panelBotones.Controls.Add(btnMarcarDatosIlegible);
            panelBotones.Controls.Add(btnMarcarLoteCompletado);
            panelBotones.Controls.Add(btnCerrar);
            panelBotones.Dock = DockStyle.Bottom;
            panelBotones.Location = new Point(0, 750);
            panelBotones.Name = "panelBotones";
            panelBotones.Padding = new Padding(10);
            panelBotones.Size = new Size(1400, 56);
            panelBotones.TabIndex = 2;
            //
            // btnGuardarControlada
            //
            btnGuardarControlada.BackColor = Color.FromArgb(0, 150, 80);
            btnGuardarControlada.FlatStyle = FlatStyle.Flat;
            btnGuardarControlada.ForeColor = Color.White;
            btnGuardarControlada.Margin = new Padding(3, 3, 8, 3);
            btnGuardarControlada.Name = "btnGuardarControlada";
            btnGuardarControlada.Size = new Size(190, 34);
            btnGuardarControlada.TabIndex = 1;
            btnGuardarControlada.Text = "Guardar y Marcar Controlada";
            btnGuardarControlada.UseVisualStyleBackColor = false;
            btnGuardarControlada.Click += btnGuardarControlada_Click;
            //
            // btnMarcarPaginaIlegible
            //
            btnMarcarPaginaIlegible.BackColor = Color.FromArgb(180, 90, 0);
            btnMarcarPaginaIlegible.FlatStyle = FlatStyle.Flat;
            btnMarcarPaginaIlegible.ForeColor = Color.White;
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
            btnMarcarDatosIlegible.BackColor = Color.FromArgb(180, 90, 0);
            btnMarcarDatosIlegible.FlatStyle = FlatStyle.Flat;
            btnMarcarDatosIlegible.ForeColor = Color.White;
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
            btnMarcarLoteCompletado.BackColor = Color.FromArgb(0, 122, 204);
            btnMarcarLoteCompletado.FlatStyle = FlatStyle.Flat;
            btnMarcarLoteCompletado.ForeColor = Color.White;
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
            btnCerrar.BackColor = Color.FromArgb(150, 30, 30);
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Margin = new Padding(3, 3, 8, 3);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(120, 34);
            btnCerrar.TabIndex = 7;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            //
            // FrmVerLote
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ForeColor = Color.White;
            ClientSize = new Size(1400, 806);
            Controls.Add(splitContainerPrincipal);
            Controls.Add(panelBotones);
            Controls.Add(panelEncabezado);
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
        private ComboBox cboTipoPlanoFiltro;
        private Label lblTipoPlanoFiltro;
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
        private TextBox txtExpediente;
        private Label lblConfianzaExpediente;
        private Label lblExpedienteCampo;
        private ComboBox cboTipoPlanoDetalle;
        private Label lblConfianzaTipoPlano;
        private Label lblTipoPlanoCampo;
        private Label lblSnPosibleBlanca;
        private Label lblSnGirada;
        private Label lblNuPagina;
        private Label lblNombreArchivoOriginal;
        private Label lblCabeceraArchivo;
        private Panel panelDerecho;
        private Panel panelImagenScroll;
        private PictureBox pictureBoxImagen;
        private Panel panelZoom;
        private Button btnZoomMas;
        private Button btnZoomMenos;
        private Button btnZoomAjustar;
        private Label lblZoom;
        private PictureBox pictureBoxPdf;
        private FlowLayoutPanel panelBotones;
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
