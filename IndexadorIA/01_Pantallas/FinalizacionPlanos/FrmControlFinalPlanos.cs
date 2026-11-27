using IndexadorIA.Negocio.Api;
using System.ComponentModel;
using System.Diagnostics;
using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;
using IndexadorIA.Negocio.Api;
using IndexadorIA.Utilidades;

namespace IndexadorIA.Pantallas.FinalizacionPlanos
{
    /// <summary>
    /// Control Final de un archivo original: revision/correccion de metadatos por pagina, visor del PDF,
    /// resultado de la API SMP, unificacion de paginas y generacion de los PDF finales + CSV indice.
    /// </summary>
    public class FrmControlFinalPlanos : Form
    {
        private readonly LoteFinalDto _lote;
        private readonly ArchivoLoteFinalDto _archivo;
        private readonly LoteFinalControlDAL _dal = new();

        private BindingList<PaginaFinalDto> _paginas = new();
        private List<CategoriaPlano> _categorias = new();
        private List<TipoPlano> _tiposTodos = new();
        private bool _cargando;
        private Bitmap? _bitmap;

        private readonly DataGridView dgv = new() { Dock = DockStyle.Fill };
        private readonly Label lblInfo = new() { AutoSize = true, Padding = new Padding(12, 8, 0, 0), ForeColor = Color.Gainsboro };

        private readonly Button btnGuardar = new() { Text = "Guardar registro seleccionado" };
        private readonly Button btnExpedienteMadre = new() { Text = "E", Width = 30, Dock = DockStyle.Left };
        private readonly Button btnSmpDireccionApi = new() { Text = "SMP y DIRECCION DESDE API" };
        private readonly Button btnNoEsPlano = new() { Text = "NO es un plano" };
        private readonly Button btnCopiarHermanos = new() { Text = "Copiar de Hermanos", Visible = false };
        private readonly Button btnCopiarDirecciones = new() { Text = "Copiar Direcciones de la API a todas las paginas" };        private readonly Button btnUnificar = new() { Text = "Unificar Planos" };
        private readonly Button btnActualizarSmp = new() { Text = "Actualizar SMP" };
        private readonly Button btnFinalizar = new() { Text = "Finalizar Plano" };
        private readonly Button btnCerrar = new() { Text = "Cerrar" };
        private readonly Button btnZoomMas = new() { Text = "+" };
        private readonly Button btnZoomMenos = new() { Text = "-" };
        private readonly Button btnZoomAjustar = new() { Text = "Ajustar" };
        private readonly Button btnZoom100 = new() { Text = "100%" };
        private readonly Label lblZoom = new() { AutoSize = true, Padding = new Padding(8, 7, 0, 0), ForeColor = Color.Gainsboro };
        private readonly Panel panelGrilla = new() { Dock = DockStyle.Top, Height = 150 };
        private readonly TextBox txtApiMensaje = new() { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Height = 90 };
        private List<EstadoControlItem> _estados = new();

        private readonly ComboBox cboEstado = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox cboCategoria = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox cboTipo = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly TextBox txtExpediente = new();
        private readonly TextBox txtSeccion = new();
        private readonly TextBox txtManzana = new();
        private readonly TextBox txtParcela = new();
        private readonly TextBox txtDireccion = new() { Multiline = true, Height = 55, ScrollBars = ScrollBars.Vertical, AcceptsReturn = false };
        private readonly TextBox txtNumeroPlano = new();
        private readonly TextBox txtObservaciones = new() { Multiline = true, Height = 38, ScrollBars = ScrollBars.Vertical };

        private readonly VisorImagenZoom picVisor = new() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(30, 30, 30) };

        private readonly Label lblApiEstado = new() { Dock = DockStyle.Top, AutoSize = false, Height = 60, ForeColor = Color.Gainsboro };
        private readonly DataGridView dgvApi = new() { Dock = DockStyle.Fill };

        public FrmControlFinalPlanos(LoteFinalDto lote, ArchivoLoteFinalDto archivo)
        {
            _lote = lote;
            _archivo = archivo;

            string carpeta = Path.GetDirectoryName(archivo.DsRutaCompleta) ?? archivo.DsRutaCompleta;
            Text = $"Control Final â€“ {archivo.DsNombreArchivo} ({carpeta}{Path.DirectorySeparatorChar})";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterParent;
            EstiloOscuro.Formulario(this);

            ConstruirInterfaz();
            ConectarEventos();
            Load += (_, _) => Inicializar();
            FormClosed += (_, _) => { picVisor.Imagen = null; _bitmap?.Dispose(); };
        }

        #region Interfaz

        private void ConstruirInterfaz()
        {
            foreach (var b in new[] { btnGuardar, btnExpedienteMadre, btnSmpDireccionApi, btnNoEsPlano, btnCopiarHermanos, btnCopiarDirecciones, btnUnificar, btnZoomMas, btnZoomMenos, btnZoomAjustar, btnZoom100 })
                EstiloOscuro.Boton(b);            EstiloOscuro.Boton(btnActualizarSmp, Color.FromArgb(170, 110, 20));
            EstiloOscuro.Boton(btnFinalizar, EstiloOscuro.Verde);
            EstiloOscuro.Boton(btnCerrar, EstiloOscuro.Rojo);

            var barra = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(6), BackColor = EstiloOscuro.Fondo, WrapContents = true };
            barra.Controls.AddRange(new Control[]
            {
                btnUnificar, btnActualizarSmp, btnFinalizar, btnCerrar, lblInfo
            });

            // Grilla de paginas (todo el ancho)
            EstiloOscuro.Grilla(dgv);
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = nameof(PaginaFinalDto.Seleccionado), HeaderText = "", FillWeight = 4 });
            dgv.Columns.Add(Col(nameof(PaginaFinalDto.NuPagina), "Pag.", 4));
            dgv.Columns.Add(Col(nameof(PaginaFinalDto.DsEstadoControl), "Estado de Control", 10));
            dgv.Columns.Add(Col(nameof(PaginaFinalDto.DsCategoria), "Categoria de Plano", 9));
            dgv.Columns.Add(Col(nameof(PaginaFinalDto.DsTipo), "Tipo de Plano", 14));
            dgv.Columns.Add(Col(nameof(PaginaFinalDto.DsExpediente), "Nro. Expediente", 13));
            dgv.Columns.Add(Col(nameof(PaginaFinalDto.DsSeccion), "Seccion", 5));
            dgv.Columns.Add(Col(nameof(PaginaFinalDto.DsManzana), "Manzana", 6));
            dgv.Columns.Add(Col(nameof(PaginaFinalDto.DsParcela), "Parcela", 6));
            dgv.Columns.Add(Col(nameof(PaginaFinalDto.DsDireccion), "Direccion", 16));
            dgv.Columns.Add(Col(nameof(PaginaFinalDto.DsNumeroPlano), "Numero de Plano", 9));
            dgv.Columns.Add(Col(nameof(PaginaFinalDto.DsObservaciones), "Observaciones", 12));

            // Panel de edicion
            var editor = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, AutoScroll = true, Padding = new Padding(6) };
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            void Fila(string titulo, Control c)
            {
                EstiloOscuro.Campo(c);
                c.Dock = DockStyle.Fill;
                var l = new Label { Text = titulo, AutoSize = true, Anchor = AnchorStyles.Left, Padding = new Padding(0, 6, 8, 0), ForeColor = Color.White };
                editor.RowCount++;
                editor.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                editor.Controls.Add(l, 0, editor.RowCount - 1);
                editor.Controls.Add(c, 1, editor.RowCount - 1);
            }
            Fila("Estado:", cboEstado);
            Fila("Categoria:", cboCategoria);
            Fila("Tipo:", cboTipo);
            Fila("Expediente:", txtExpediente);
            int filaExp = editor.RowCount - 1;
            var lblExp = editor.GetControlFromPosition(0, filaExp)!;
            editor.Controls.Remove(lblExp);
            btnExpedienteMadre.Dock = DockStyle.None;
            btnExpedienteMadre.Size = new Size(28, 26);
            btnExpedienteMadre.Margin = new Padding(0, 2, 4, 2);
            var panelEtiquetaExp = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Anchor = AnchorStyles.Left, Margin = new Padding(0) };
            panelEtiquetaExp.Controls.Add(btnExpedienteMadre);
            panelEtiquetaExp.Controls.Add(lblExp);
            editor.Controls.Add(panelEtiquetaExp, 0, filaExp);
            Fila("Seccion:", txtSeccion);
            Fila("Manzana:", txtManzana);
            Fila("Parcela:", txtParcela);
            Fila("Direccion:", txtDireccion);
            Fila("Nro. Plano:", txtNumeroPlano);
            Fila("Observaciones:", txtObservaciones);
            btnSmpDireccionApi.Dock = DockStyle.Fill;
            btnSmpDireccionApi.AutoSize = false;
            btnSmpDireccionApi.Height = 32;
            editor.RowCount++;
            editor.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            editor.Controls.Add(btnSmpDireccionApi, 0, editor.RowCount - 1);
            editor.SetColumnSpan(btnSmpDireccionApi, 2);
            btnNoEsPlano.Dock = DockStyle.Fill;
            btnNoEsPlano.AutoSize = false;
            btnNoEsPlano.Height = 32;
            editor.RowCount++;
            editor.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            editor.Controls.Add(btnNoEsPlano, 0, editor.RowCount - 1);
            editor.SetColumnSpan(btnNoEsPlano, 2);
            btnCopiarHermanos.Dock = DockStyle.Fill;
            btnCopiarHermanos.AutoSize = false;
            btnCopiarHermanos.Height = 32;
            editor.RowCount++;
            editor.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            editor.Controls.Add(btnCopiarHermanos, 0, editor.RowCount - 1);
            editor.SetColumnSpan(btnCopiarHermanos, 2);
            btnGuardar.Dock = DockStyle.Fill;
            btnGuardar.AutoSize = false;
            btnGuardar.Height = 32;
            editor.RowCount++;
            editor.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            editor.Controls.Add(btnGuardar, 0, editor.RowCount - 1);
            editor.SetColumnSpan(btnGuardar, 2);

            var gbEditor = Grupo("Datos de la pagina (editable)", editor);

            var barraZoom = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, BackColor = EstiloOscuro.Fondo };
            barraZoom.Controls.AddRange(new Control[] { btnZoomMenos, btnZoomMas, btnZoomAjustar, btnZoom100, lblZoom });
            var panelVisor = new Panel { Dock = DockStyle.Fill };
            panelVisor.Controls.Add(picVisor);
            panelVisor.Controls.Add(barraZoom);
            var gbVisor = Grupo("Visor de la pagina (rueda: zoom, arrastrar: mover, doble clic: abrir PDF)", panelVisor);

            // Resultado API
            EstiloOscuro.Grilla(dgvApi);
            dgvApi.ReadOnly = true;
            dgvApi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvApi.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvApi.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvApi.Columns.Add(Col("DsSmp", "SMP", 20));
            dgvApi.Columns.Add(Col("DsCaso", "Caso", 20));
            dgvApi.Columns.Add(Col("DsDirecciones", "Direcciones (API)", 60));
            lblApiEstado.BackColor = EstiloOscuro.Fondo;
            lblApiEstado.Height = 48;
            EstiloOscuro.Campo(txtApiMensaje);
            txtApiMensaje.Dock = DockStyle.Top;
            var lblMensaje = new Label { Text = "Mensaje de la API (dsMensaje):", Dock = DockStyle.Top, AutoSize = false, Height = 22, ForeColor = Color.White, Padding = new Padding(0, 4, 0, 0) };
            btnCopiarDirecciones.Dock = DockStyle.Top;
            btnCopiarDirecciones.AutoSize = false;
            btnCopiarDirecciones.Height = 34;
            var panelApi = new Panel { Dock = DockStyle.Fill };
            panelApi.Controls.Add(dgvApi);
            panelApi.Controls.Add(txtApiMensaje);
            panelApi.Controls.Add(lblMensaje);
            panelApi.Controls.Add(lblApiEstado);
            panelApi.Controls.Add(btnCopiarDirecciones);
            var gbApi = Grupo("Resultado API SMP", panelApi);

            var inferior = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1 };
            inferior.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360));
            inferior.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            inferior.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 420));
            inferior.Controls.Add(gbEditor, 0, 0);
            inferior.Controls.Add(gbVisor, 1, 0);
            inferior.Controls.Add(gbApi, 2, 0);

            var gbGrilla = Grupo("Paginas del archivo original", dgv);
            gbGrilla.Dock = DockStyle.Fill;
            panelGrilla.Controls.Add(gbGrilla);
            var contenedor = new Panel { Dock = DockStyle.Fill };
            contenedor.Controls.Add(inferior);
            var divisor = new Splitter { Dock = DockStyle.Top, Height = 6, BackColor = Color.FromArgb(70, 70, 70), MinExtra = 150, MinSize = 60 };
            divisor.SplitterMoved += (_, _) => _alturaGrillaManual = true;
            contenedor.Controls.Add(divisor);
            contenedor.Controls.Add(panelGrilla);

            Controls.Add(contenedor);
            Controls.Add(barra);
        }

        private static GroupBox Grupo(string titulo, Control contenido)
        {
            var g = new GroupBox { Text = titulo, Dock = DockStyle.Fill, ForeColor = Color.White, BackColor = EstiloOscuro.Fondo, Padding = new Padding(6, 20, 6, 6) };
            contenido.Dock = DockStyle.Fill;
            g.Controls.Add(contenido);
            return g;
        }

        private static DataGridViewTextBoxColumn Col(string prop, string titulo, float peso) =>
            new() { DataPropertyName = prop, HeaderText = titulo, ReadOnly = true, FillWeight = peso };

        #endregion

        #region Eventos

        private void ConectarEventos()
        {
            dgv.CurrentCellDirtyStateChanged += (_, _) =>
            {
                if (dgv.IsCurrentCellDirty) dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            dgv.SelectionChanged += (_, _) => AlCambiarPagina();
            dgv.DataError += (_, e) => e.ThrowException = false;
            dgv.CellValueChanged += (_, _) => ActualizarInfo();

            cboCategoria.SelectionChangeCommitted += (_, _) => AlElegirCategoria();
            cboTipo.SelectionChangeCommitted += (_, _) => AlElegirTipo();
            foreach (var t in new[] { txtExpediente, txtSeccion, txtManzana, txtParcela, txtDireccion, txtNumeroPlano, txtObservaciones })
                t.TextChanged += (_, _) => AlEditarTexto();

            cboEstado.SelectionChangeCommitted += (_, _) => AlElegirEstado();
            cboTipo.DropDown += (_, _) => AjustarAnchoLista(cboTipo);
            btnZoomMas.Click += (_, _) => picVisor.Acercar();
            btnZoomMenos.Click += (_, _) => picVisor.Alejar();
            btnZoomAjustar.Click += (_, _) => picVisor.AjustarAVentana();
            btnZoom100.Click += (_, _) => picVisor.Zoom100();
            picVisor.ZoomCambiado += (_, _) => lblZoom.Text = $"Zoom: {picVisor.ZoomPorcentaje}%";
            Resize += (_, _) => AjustarAlturaGrilla();
            btnGuardar.Click += (_, _) => GuardarActual();
            btnExpedienteMadre.Click += (_, _) => PonerExpedienteMadre();
            btnSmpDireccionApi.Click += (_, _) => AgregarObservacionApi();
            btnNoEsPlano.Click += (_, _) => AgregarObservacionNoEsPlano();
            btnCopiarHermanos.Click += (_, _) => CopiarDeHermanos();
            btnCopiarDirecciones.Click += (_, _) => CopiarDirecciones();
            btnUnificar.Click += (_, _) => Unificar();
            btnActualizarSmp.Click += (_, _) => ActualizarSmp();
            btnFinalizar.Click += (_, _) => FinalizarPlano();
            btnCerrar.Click += (_, _) => Close();
            picVisor.DoubleClick += (_, _) => AbrirPdfExterno();
        }

        private static T Remoto<T>(Func<IndexadorIA.Negocio.Api.ApiClienteServicio, Task<T>> f) =>
            Task.Run(() => f(new IndexadorIA.Negocio.Api.ApiClienteServicio())).GetAwaiter().GetResult();

        private static void Remoto(Func<IndexadorIA.Negocio.Api.ApiClienteServicio, Task> f) =>
            Task.Run(() => f(new IndexadorIA.Negocio.Api.ApiClienteServicio())).GetAwaiter().GetResult();

        private string ObtenerRutaPdfLocal(PaginaFinalDto p)
        {
            if (!SesionApi.ModoRemoto) return p.DsRutaPagina;
            byte[] bytes = Remoto(a => a.ObtenerPdfPaginaFinalAsync(_lote.CdLoteFinal, _archivo.CdArchivo, p.CdArchivoPagina));
            string ruta = Path.Combine(Path.GetTempPath(), $"IndexadorIA_final_{p.CdArchivoPagina}.pdf");
            File.WriteAllBytes(ruta, bytes);
            return ruta;
        }

        private void Inicializar()
        {
            try
            {
                if (SesionApi.ModoRemoto)
                {
                    _categorias = Remoto(a => a.ObtenerCategoriasPlanoAsync()).Select(c => new CategoriaPlano { CdCategoriaPlano = c.CdCategoriaPlano, DsCategoriaPlano = c.DsCategoriaPlano }).ToList();
                    _tiposTodos = Remoto(a => a.ObtenerTiposPlanoAsync()).Select(t => new TipoPlano { CdTipoPlano = t.CdTipoPlano, CdCategoriaPlano = t.CdCategoriaPlano, DsTipoPlano = t.DsTipoPlano }).ToList();
                    _estados = Remoto(a => a.ObtenerEstadosControlFinalAsync());
                }
                else
                {
                    _categorias = new CategoriaPlanoDAL().ObtenerTodos();
                    _tiposTodos = new TipoPlanoDAL().ObtenerTodos();
                    _estados = _dal.ObtenerEstadosControl();
                }
                _cargando = true;
                cboEstado.DisplayMember = nameof(EstadoControlItem.Descripcion);
                cboEstado.DataSource = _estados;
                cboEstado.SelectedIndex = -1;
                cboCategoria.DisplayMember = nameof(CategoriaPlano.DsCategoriaPlano);
                cboCategoria.DataSource = _categorias;
                cboCategoria.SelectedIndex = -1;
                _cargando = false;

                CargarPaginas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el control: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarPaginas()
        {
            _paginas = new BindingList<PaginaFinalDto>(SesionApi.ModoRemoto
                            ? Remoto(a => a.ObtenerPaginasFinalAsync(_lote.CdLoteFinal, _archivo.CdArchivo))
                            : _dal.ObtenerPaginas(_archivo.CdArchivo));
            dgv.DataSource = _paginas;
            ActualizarInfo();
            AjustarAlturaGrilla();
            if (_paginas.Count == 0)
            {
                LimpiarDetalle();
            }
            else
            {
                dgv.ClearSelection();
                dgv.CurrentCell = dgv.Rows[0].Cells[1];
                AlCambiarPagina();
            }
        }

        private PaginaFinalDto? Actual => dgv.CurrentRow?.DataBoundItem as PaginaFinalDto;

        private void ActualizarInfo()
        {
            lblInfo.Text = $"Lote {_lote.DsNombreLote}  |  Paginas pendientes: {_paginas.Count}  |  Marcadas: {_paginas.Count(p => p.Seleccionado)}";
        }

        /// <summary>La grilla ocupa solo lo necesario para sus filas; el resto del espacio es para el visor.</summary>
        private bool _alturaGrillaManual;

        /// <summary>Ajusta la altura al contenido hasta que el usuario mueve el divisor.</summary>
        private void AjustarAlturaGrilla()
        {
            if (_alturaGrillaManual) return;
            int filas = Math.Max(_paginas.Count, 1);
            int alturaFilas = dgv.ColumnHeadersHeight + filas * dgv.RowTemplate.Height + 4;
            int deseada = alturaFilas + 40;
            int maxima = Math.Max(120, (int)(ClientSize.Height * 0.5));
            panelGrilla.Height = Math.Clamp(deseada, 90, maxima);
        }

        private static void AjustarAnchoLista(ComboBox cbo)
        {
            int ancho = cbo.Width;
            foreach (var item in cbo.Items)
            {
                string texto = cbo.GetItemText(item);
                ancho = Math.Max(ancho, TextRenderer.MeasureText(texto, cbo.Font).Width + SystemInformation.VerticalScrollBarWidth + 8);
            }
            cbo.DropDownWidth = Math.Min(ancho, 700);
            cbo.MaxDropDownItems = 20;
        }

        #endregion

        #region Detalle de pagina (editor, visor, API)

        private void LimpiarDetalle()
        {
            _cargando = true;
            cboEstado.SelectedIndex = -1;
            cboCategoria.SelectedIndex = -1;
            cboTipo.DataSource = null;
            foreach (var t in new[] { txtExpediente, txtSeccion, txtManzana, txtParcela, txtDireccion, txtNumeroPlano, txtObservaciones })
                t.Text = string.Empty;
            _cargando = false;
            picVisor.Imagen = null;
            _bitmap?.Dispose();
            _bitmap = null;
            dgvApi.DataSource = null;
            lblApiEstado.Text = string.Empty;
            txtApiMensaje.Text = string.Empty;
        }

        private void AlCambiarPagina()
        {
            var p = Actual;
            if (p == null) return;

            _cargando = true;
            try
            {
                cboEstado.SelectedItem = _estados.FirstOrDefault(s => s.Codigo == p.CdEstadoControl);
                cboCategoria.SelectedItem = _categorias.FirstOrDefault(c => c.CdCategoriaPlano == p.CdCategoriaPlano);
                CargarTipos(p.CdCategoriaPlano);
                cboTipo.SelectedItem = (cboTipo.DataSource as List<TipoPlano>)?.FirstOrDefault(t => t.CdTipoPlano == p.CdTipoPlano);
                txtExpediente.Text = p.DsExpediente;
                txtSeccion.Text = p.DsSeccion;
                txtManzana.Text = p.DsManzana;
                txtParcela.Text = p.DsParcela;
                txtDireccion.Text = p.DsDireccion;
                txtNumeroPlano.Text = p.DsNumeroPlano;
                txtObservaciones.Text = p.DsObservaciones;
            }
            finally
            {
                _cargando = false;
            }

            ActualizarBotonHermanos();
            MostrarPdf(p);
            MostrarApi(p.CdResultado);
        }

        private void ActualizarBotonHermanos()
        {
            btnCopiarHermanos.Visible = Actual?.CdEstadoControl == 1;
        }

        private void CargarTipos(int? cdCategoria)
        {
            var tipos = cdCategoria.HasValue
                ? _tiposTodos.Where(t => t.CdCategoriaPlano == cdCategoria.Value).ToList()
                : new List<TipoPlano>();
            cboTipo.DataSource = null;
            cboTipo.DisplayMember = nameof(TipoPlano.DsTipoPlano);
            cboTipo.DataSource = tipos;
            cboTipo.SelectedIndex = -1;
        }

        private void AlElegirEstado()
        {
            var p = Actual;
            if (_cargando || p == null || cboEstado.SelectedItem is not EstadoControlItem est) return;
            p.CdEstadoControl = est.Codigo;
            p.DsEstadoControl = est.Descripcion;
            Modificar(p);
            ActualizarBotonHermanos();
        }

        private void AlElegirCategoria()
        {
            var p = Actual;
            if (_cargando || p == null) return;
            var cat = cboCategoria.SelectedItem as CategoriaPlano;
            p.CdCategoriaPlano = cat?.CdCategoriaPlano;
            p.DsCategoria = cat?.DsCategoriaPlano ?? string.Empty;
            p.CdTipoPlano = null;
            p.DsTipo = string.Empty;
            p.DsAcronimo = string.Empty;
            _cargando = true;
            CargarTipos(p.CdCategoriaPlano);
            _cargando = false;
            Modificar(p);
        }

        private void AlElegirTipo()
        {
            var p = Actual;
            if (_cargando || p == null) return;
            var tipo = cboTipo.SelectedItem as TipoPlano;
            p.CdTipoPlano = tipo?.CdTipoPlano;
            p.DsTipo = tipo?.DsTipoPlano ?? string.Empty;
            p.DsAcronimo = tipo?.DsAcronimo ?? string.Empty;
            Modificar(p);
        }

        private void AlEditarTexto()
        {
            var p = Actual;
            if (_cargando || p == null) return;
            p.DsExpediente = txtExpediente.Text.Trim();
            p.DsSeccion = txtSeccion.Text.Trim();
            p.DsManzana = txtManzana.Text.Trim();
            p.DsParcela = txtParcela.Text.Trim();
            p.DsDireccion = txtDireccion.Text.Trim();
            p.DsNumeroPlano = txtNumeroPlano.Text.Trim();
            p.DsObservaciones = txtObservaciones.Text.Trim();
            Modificar(p);
        }

        private void Modificar(PaginaFinalDto p)
        {
            p.Modificado = true;
            int idx = _paginas.IndexOf(p);
            if (idx >= 0) dgv.InvalidateRow(idx);
        }

        private void MostrarPdf(PaginaFinalDto pag)
        {
            picVisor.Imagen = null;
            _bitmap?.Dispose();
            _bitmap = null;
            try
            {
                string ruta = ObtenerRutaPdfLocal(pag);
                if (string.IsNullOrWhiteSpace(ruta) || !File.Exists(ruta)) return;
                _bitmap = ProcesamientoImagenes.ConvertirPDFaBitmap(ruta, 200);
                picVisor.Imagen = _bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo mostrar el PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AbrirPdfExterno()
        {
            var p = Actual;
            if (p == null) return;
            string ruta;
            try { ruta = ObtenerRutaPdfLocal(p); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!File.Exists(ruta)) return;
            Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
        }

        private void MostrarApi(int cdResultado)
        {
            try
            {
                var v = SesionApi.ModoRemoto
                    ? Remoto(a => a.ObtenerValidacionSmpFinalAsync(cdResultado))
                    : _dal.ObtenerValidacionSmp(cdResultado);
                if (v == null)
                {
                    dgvApi.DataSource = null;
                    lblApiEstado.Text = "Sin resultado de la API para este registro.";
                    txtApiMensaje.Text = string.Empty;
                    return;
                }
                lblApiEstado.Text = $"SMP: {v.DsSmp}  |  Estado: {v.DsEstado}  |  Consulta: {v.FeConsulta:dd/MM/yyyy HH:mm}\r\n" +
                                    $"Sec/Mza/Parc validados: {v.DsSeccion}-{v.DsManzana}-{v.DsParcela}";
                txtApiMensaje.Text = string.Join("\r\n", new[] { v.DsMensaje, v.DsMotivoOmision }.Where(s => !string.IsNullOrWhiteSpace(s)));
                dgvApi.DataSource = v.Parcelas;
            }
            catch (Exception ex)
            {
                lblApiEstado.Text = $"Error al leer resultado API: {ex.Message}";
            }
        }

        #endregion

        #region Acciones

        /// <summary>Guarda SOLO el registro seleccionado en la grilla.</summary>
        private void GuardarActual()
        {
            var p = Actual;
            if (p == null) return;
            if (p.CdEstadoControl == 1)
            {
                MessageBox.Show("El registro todavia esta en estado de control 1. Controle los datos contra la imagen y cambie manualmente el estado que corresponda antes de guardar.",
                    "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                dgv.EndEdit();
                if (SesionApi.ModoRemoto) Remoto(a => a.GuardarPaginasFinalAsync(_lote.CdLoteFinal, new[] { p }));
                else _dal.GuardarPagina(p, SesionActual.UsuarioActual!.CdUsuario);
                p.Modificado = false;
                MessageBox.Show($"Pagina {p.NuPagina} guardada.", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Guarda todas
        private bool Guardar()
        {
            try
            {
                dgv.EndEdit();
                int cdUsuario = SesionActual.UsuarioActual!.CdUsuario;
                var modificadas = _paginas.Where(x => x.Modificado).ToList();
                if (SesionApi.ModoRemoto && modificadas.Count > 0)
                    Remoto(a => a.GuardarPaginasFinalAsync(_lote.CdLoteFinal, modificadas));
                foreach (var p in modificadas)
                {
                    if (!SesionApi.ModoRemoto) _dal.GuardarPagina(p, cdUsuario);
                    p.Modificado = false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void CopiarDirecciones()
        {
            var p = Actual;
            if (p == null) return;

            string? direccion = null;
            if (dgvApi.CurrentRow?.DataBoundItem is SmpParcelaVigente sel && !string.IsNullOrWhiteSpace(sel.DsDirecciones))
                direccion = sel.DsDirecciones;
            else if (dgvApi.DataSource is List<SmpParcelaVigente> lista)
                direccion = lista.Select(x => x.DsDirecciones).FirstOrDefault(d => !string.IsNullOrWhiteSpace(d));

            if (string.IsNullOrWhiteSpace(direccion))
            {
                MessageBox.Show("El registro seleccionado no tiene direccion en el resultado de la API.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            direccion = direccion.Trim();

            var r = MessageBox.Show($"Se copiara la direccion de la API a TODAS las paginas ({_paginas.Count}):\r\n\r\n{direccion}",
                "Copiar Direcciones", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r != DialogResult.OK) return;

            foreach (var pag in _paginas)
            {
                pag.DsDireccion = direccion;
                pag.Modificado = true;
            }
            dgv.Refresh();
            _cargando = true;
            txtDireccion.Text = direccion;
            _cargando = false;
            Guardar();
        }

        private static string Clave(PaginaFinalDto p) =>
            string.Join("|", p.CdCategoriaPlano, p.CdTipoPlano, Norm(p.DsExpediente), Norm(p.DsSeccion), Norm(p.DsManzana),
                Norm(p.DsParcela), Norm(p.DsDireccion), Norm(p.DsNumeroPlano), p.EsIlegible);

        private static string Norm(string? s) => (s ?? string.Empty).Trim().ToUpperInvariant();

        private void Unificar()
        {
            dgv.EndEdit();
            var marcadas = _paginas.Where(p => p.Seleccionado).OrderBy(p => p.NuPagina).ToList();
            if (marcadas.Count < 2)
            {
                MessageBox.Show("Marque al menos dos paginas para unificar.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (AdvertirEstadoUno(marcadas)) return;
            if (marcadas.Select(Clave).Distinct().Count() > 1)
            {
                MessageBox.Show("No se pueden unificar: las paginas marcadas tienen distinta categoria, tipo, expediente, " +
                                "seccion, manzana, parcela, direccion, numero de plano o condicion de legibilidad.\r\nCorrija los datos y reintente.",
                    "Unificar Planos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Guardar()) return;
            if (!GenerarGrupo(marcadas, out string? error))
            {
                MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DespuesDeGenerar();
        }

        private void PonerExpedienteMadre()
        {
            string nombre = _archivo.DsNombreArchivo;
            if (string.IsNullOrWhiteSpace(nombre)) return;

            string[] partes = Path.GetFileNameWithoutExtension(nombre).Split('-', StringSplitOptions.None);
            if (!IndexadorIA.Pantallas.PlanosGCBA.FrmVerLote.TryParsearExpedienteDeArchivo(partes, out _, out string anio, out string numero, out string reparticion))
                return;

            if (int.TryParse(numero, out int n)) numero = n.ToString("D8");
            txtExpediente.Text = $"EX-{anio}-{numero}-GCABA-{reparticion}";
        }

        private void AgregarObservacionApi()
        {
            const string marca = "[SMP y DIRECCION CARGADO DESDE LA API]";
            string actual = txtObservaciones.Text.Trim();
            if (actual.Contains(marca)) return;
            txtObservaciones.Text = actual.Length == 0 ? marca : actual + " " + marca;
        }

        private static bool AdvertirEstadoUno(IEnumerable<PaginaFinalDto> paginas)
        {
            var enUno = paginas.Where(p => p.CdEstadoControl == 1).OrderBy(p => p.NuPagina).Select(p => p.NuPagina).ToList();
            if (enUno.Count == 0) return false;
            MessageBox.Show($"Hay paginas en estado de control 1 (pag. {string.Join(", ", enUno)}). Controle los datos contra la imagen y cambie manualmente el estado antes de continuar.",
                "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return true;
        }

        private void CopiarDeHermanos()
        {
            var p = Actual;
            if (p == null) return;
            var h = _paginas.Where(x => x != p && x.CdCategoriaPlano.HasValue && x.CdEstadoControl != 1).OrderBy(x => x.NuPagina).FirstOrDefault()
                    ?? _paginas.FirstOrDefault(x => x != p && x.CdCategoriaPlano.HasValue);
            if (h == null)
            {
                MessageBox.Show("No hay otra pagina con datos para copiar.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            p.CdCategoriaPlano = h.CdCategoriaPlano;
            p.DsCategoria = h.DsCategoria;
            p.CdTipoPlano = h.CdTipoPlano;
            p.DsTipo = h.DsTipo;
            p.DsAcronimo = h.DsAcronimo;
            p.DsExpediente = h.DsExpediente;
            p.DsSeccion = h.DsSeccion;
            p.DsManzana = h.DsManzana;
            p.DsParcela = h.DsParcela;
            p.DsDireccion = h.DsDireccion;
            p.DsNumeroPlano = h.DsNumeroPlano;
            Modificar(p);

            _cargando = true;
            try
            {
                cboCategoria.SelectedItem = _categorias.FirstOrDefault(c => c.CdCategoriaPlano == p.CdCategoriaPlano);
                CargarTipos(p.CdCategoriaPlano);
                cboTipo.SelectedItem = (cboTipo.DataSource as List<TipoPlano>)?.FirstOrDefault(t => t.CdTipoPlano == p.CdTipoPlano);
                txtExpediente.Text = p.DsExpediente;
                txtSeccion.Text = p.DsSeccion;
                txtManzana.Text = p.DsManzana;
                txtParcela.Text = p.DsParcela;
                txtDireccion.Text = p.DsDireccion;
                txtNumeroPlano.Text = p.DsNumeroPlano;
            }
            finally
            {
                _cargando = false;
            }
            MessageBox.Show($"Datos copiados de la pagina {h.NuPagina}. Controlelos contra la imagen y cambie el estado manualmente.",
                "Copiar de Hermanos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AgregarObservacionNoEsPlano()
        {
            const string marca = "[NO ES UN PLANO]";
            string actual = txtObservaciones.Text.Trim();
            if (actual.Contains(marca)) return;
            txtObservaciones.Text = actual.Length == 0 ? marca : actual + " " + marca;
        }

        private void FinalizarPlano()
        {
            dgv.EndEdit();
            if (_paginas.Count == 0) { Close(); return; }
            if (AdvertirEstadoUno(_paginas)) return;

            var r = MessageBox.Show(
                $"Se generaran los PDF finales para las {_paginas.Count} paginas restantes (un archivo por cada registro de la grilla).\r\nÂ¿Continuar?",
                "Finalizar Plano", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            if (!Guardar()) return;

            var grupos = _paginas
                .OrderBy(p => p.NuPagina)
                .Select(p => new List<PaginaFinalDto> { p })
                .ToList();

            var errores = new List<string>();
            foreach (var g in grupos)
            {
                if (!GenerarGrupo(g, out string? error)) errores.Add($"Pag. {string.Join(",", g.Select(x => x.NuPagina))}: {error}");
            }

            if (errores.Count > 0)
            {
                MessageBox.Show("Algunos planos no se pudieron finalizar:\r\n\r\n" + string.Join("\r\n", errores),
                    "Finalizar Plano", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            DespuesDeGenerar();
        }

        /// <summary>Genera PDF + CSV, registra en BD y quita las paginas de la grilla. Si falla la BD deshace los archivos.</summary>
        private bool GenerarGrupo(List<PaginaFinalDto> grupo, out string? error)
        {
            error = null;
            var m = grupo[0];
            if (m.CdEstadoControl != 3 && !m.CdCategoriaPlano.HasValue)
            {
                error = "Falta la categoria de plano.";
                return false;
            }
            if (SesionApi.ModoRemoto)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    var ids = grupo.Select(p => p.CdArchivoPagina).ToList();
                    Remoto(a => a.FinalizarPlanoFinalAsync(_lote.CdLoteFinal, _archivo.CdArchivo, ids));
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    return false;
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
                foreach (var p in grupo) _paginas.Remove(p);
                return true;
            }

            var sinPdf = grupo.FirstOrDefault(p => !File.Exists(p.DsRutaPagina));
            if (sinPdf != null)
            {
                error = $"No se encontro el PDF de la pagina {sinPdf.NuPagina}.";
                return false;
            }

            FinalizadorPlanos.ResultadoFinal? res = null;
            try
            {
                Cursor = Cursors.WaitCursor;
                res = FinalizadorPlanos.Generar(_archivo.DsRutaCompleta, _archivo.DsNombreArchivo, grupo);
                _dal.RegistrarFinalizacion(_lote.CdLoteFinal, _archivo.CdArchivo, grupo.Select(p => p.CdArchivoPagina),
                    res.NombreNuevo, res.Carpeta, m.EsIlegible, SesionActual.UsuarioActual!.CdUsuario);
            }
            catch (Exception ex)
            {
                if (res != null) FinalizadorPlanos.Deshacer(res);
                error = ex.Message;
                return false;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            foreach (var p in grupo) _paginas.Remove(p);
            return true;
        }

        private void DespuesDeGenerar()
        {
            ActualizarInfo();
            AjustarAlturaGrilla();
            if (_paginas.Count == 0)
            {
                MessageBox.Show("Archivo original finalizado.", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void ActualizarSmp()
        {
            if (_paginas.Count == 0) return;
            var r = MessageBox.Show(
                "El archivo original se devolvera para volver a consultar la API SMP (con los datos de seccion, manzana y parcela guardados) " +
                "y volvera a la pantalla de creacion de lotes.\r\nÂ¿Continuar?",
                "Actualizar SMP", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            if (!Guardar()) return;
            try
            {
                if (SesionApi.ModoRemoto) Remoto(a => a.DevolverParaSmpFinalAsync(_lote.CdLoteFinal, _archivo.CdArchivo));
                else _dal.DevolverParaActualizarSmp(_lote.CdLoteFinal, _archivo.CdArchivo);
                MessageBox.Show("Archivo devuelto para actualizar SMP. Ejecute el Validador API SMP y luego cree un nuevo lote.",
                    "Actualizar SMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al devolver para SMP: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!e.Cancel && DialogResult != DialogResult.OK && _paginas.Any(p => p.Modificado))
            {
                var r = MessageBox.Show("Hay cambios sin guardar. Â¿Guardarlos antes de cerrar?", "Control Final",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r == DialogResult.Cancel) { e.Cancel = true; return; }
                if (r == DialogResult.Yes && !Guardar()) { e.Cancel = true; return; }
            }
            base.OnFormClosing(e);
        }

        #endregion
    }
}
