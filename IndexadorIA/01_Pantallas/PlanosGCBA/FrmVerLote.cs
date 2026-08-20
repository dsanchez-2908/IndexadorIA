using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;
using IndexadorIA.Negocio.Api;
using IndexadorIA.Utilidades;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    /// <summary>
    /// Pantalla de detalle de un lote: permite revisar/corregir los datos extraídos por IA,
    /// controlar rotación/legibilidad de páginas y visualizar la imagen recortada con zoom.
    /// Parte 2: layout y binding de datos. La funcionalidad de los botones se implementa en la Parte 3.
    /// </summary>
    public partial class FrmVerLote : Form
    {
        private readonly int _cdLote;
        private List<FilaArchivoPagina> _filasCompletas = new();
        private List<CategoriaPlano> _categoriasPlano = new();
        private List<TipoPlano> _tiposPlano = new();
        private List<Reparticion> _reparticiones = new();
        private FilaArchivoPagina? _filaSeleccionada;
        private ResultadoIA? _valoresOriginalesResultado;
        private float _factorZoom = 1.0f;
        private bool _arrastrandoImagen = false;
        private Point _puntoInicialArrastre;
        private readonly ApiClienteServicio? _apiCliente;
        private LoteDetalleApiDto? _detalleRemoto;

        /// <summary>
        /// Combina la información de ArchivoPagina + ResultadoIA para mostrar en la grilla
        /// y en el panel de detalle.
        /// </summary>
        private class FilaArchivoPagina
        {
            public ArchivoPagina Archivo { get; set; } = null!;
            public ResultadoIA? Resultado { get; set; }

            public string DsNombreArchivoPagina => Archivo.DsNombreArchivoPagina;
            public string DsCategoriaPlano => Resultado?.DsCategoriaPlano ?? string.Empty;
            public string DsTipoPlano => Resultado?.DsTipoPlano ?? string.Empty;
            public string DsNumeroPlano => Resultado?.DsNumeroPlano ?? string.Empty;
            public string DsExpediente => Resultado?.DsExpediente ?? string.Empty;
            public string DsSeccion => Resultado?.DsSeccion ?? string.Empty;
            public string DsManzana => Resultado?.DsManzana ?? string.Empty;
            public string DsParcela => Resultado?.DsParcela ?? string.Empty;
            public string DsDireccion => Resultado?.DsDireccion ?? string.Empty;

            public decimal ExactitudPromedio
            {
                get
                {
                    if (Resultado == null) return 0;
                    var valores = new[]
                    {
                        Resultado.NuConfianzaCategoriaPlano,
                        Resultado.NuConfianzaTipoPlano,
                        Resultado.NuConfianzaNumeroPlano,
                        Resultado.NuConfianzaExpediente,
                        Resultado.NuConfianzaSeccion,
                        Resultado.NuConfianzaManzana,
                        Resultado.NuConfianzaParcela,
                        Resultado.NuConfianzaDireccion
                    }.Where(v => v.HasValue).Select(v => v!.Value).ToList();

                    return valores.Count == 0 ? 0 : Math.Round(valores.Average() * 100m, 1);
                }
            }
        }

        public FrmVerLote(int cdLote)
        {
            InitializeComponent();
            _cdLote = cdLote;
            _apiCliente = SesionApi.ModoRemoto ? new ApiClienteServicio() : null;

            pictureBoxImagen.MouseDown += pictureBoxImagen_MouseDown;
            pictureBoxImagen.MouseMove += pictureBoxImagen_MouseMove;
            pictureBoxImagen.MouseUp += pictureBoxImagen_MouseUp;
            panelImagenScroll.MouseDown += pictureBoxImagen_MouseDown;
            panelImagenScroll.MouseMove += pictureBoxImagen_MouseMove;
            panelImagenScroll.MouseUp += pictureBoxImagen_MouseUp;
            // Ajuste 2 (posicionar imagen en esquina inferior derecha) comentado temporalmente.
            // panelImagenScroll.Resize += (s, e) => PosicionarImagenEnEsquinaInferiorDerecha();

            KeyPreview = true;
            KeyDown += FrmVerLote_KeyDown;
        }

        private void FrmVerLote_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.G && e.Alt)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                btnGuardarControlada_Click(this, EventArgs.Empty);
            }
        }

        private void FrmVerLote_Load(object sender, EventArgs e)
        {
            try
            {
                if (SesionApi.ModoRemoto)
                {
                    _detalleRemoto = _apiCliente!.ObtenerDetalleLoteAsync(_cdLote, chkMostrarTodos.Checked)
                        .GetAwaiter().GetResult();
                }

                CargarEncabezadoLote();
                CargarCategoriasYTiposPlano();
                LimpiarPanelDetalle();
                CargarDatosGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar la pantalla de Ver Lote: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Carga de datos

        private void CargarEncabezadoLote()
        {
            if (SesionApi.ModoRemoto)
            {
                var loteRemoto = _detalleRemoto?.Lote;
                if (loteRemoto == null)
                {
                    MessageBox.Show("No se encontró el lote solicitado.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblNombreLote.Text = loteRemoto.DsNombreLote;
                lblCantidadArchivos.Text = loteRemoto.NuCantidadArchivos.ToString();
                lblFechaCreacion.Text = loteRemoto.FeAltaLote.ToString("dd/MM/yyyy HH:mm");
                lblFechaProcesamientoIA.Text = loteRemoto.FeProcesamientoIA.HasValue
                    ? loteRemoto.FeProcesamientoIA.Value.ToString("dd/MM/yyyy HH:mm")
                    : "N/D";
                return;
            }

            var loteDAL = new LoteDAL();
            var lote = loteDAL.ObtenerPorId(_cdLote);

            if (lote == null)
            {
                MessageBox.Show("No se encontró el lote solicitado.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblNombreLote.Text = lote.DsNombreLote;
            lblCantidadArchivos.Text = lote.NuCantidadArchivos.ToString();
            lblFechaCreacion.Text = lote.FeAltaLote.ToString("dd/MM/yyyy HH:mm");

            var feProcesamientoIA = loteDAL.ObtenerFechaProcesamientoIA(_cdLote);
            lblFechaProcesamientoIA.Text = feProcesamientoIA.HasValue
                ? feProcesamientoIA.Value.ToString("dd/MM/yyyy HH:mm")
                : "N/D";
        }

        private void CargarCategoriasYTiposPlano()
        {
            if (SesionApi.ModoRemoto)
            {
                _categoriasPlano = _apiCliente!.ObtenerCategoriasPlanoAsync().GetAwaiter().GetResult()
                    .Select(c => new CategoriaPlano { CdCategoriaPlano = c.CdCategoriaPlano, DsCategoriaPlano = c.DsCategoriaPlano })
                    .ToList();
                _tiposPlano = _apiCliente!.ObtenerTiposPlanoAsync().GetAwaiter().GetResult()
                    .Select(t => new TipoPlano { CdTipoPlano = t.CdTipoPlano, CdCategoriaPlano = t.CdCategoriaPlano, DsTipoPlano = t.DsTipoPlano })
                    .ToList();
                _reparticiones = _apiCliente!.ObtenerReparticionesAsync().GetAwaiter().GetResult()
                    .Select(r => new Reparticion { CdReparticion = r.CdReparticion, DsReparticion = r.DsReparticion })
                    .ToList();
            }
            else
            {
                var categoriaPlanoDAL = new CategoriaPlanoDAL();
                var tipoPlanoDAL = new TipoPlanoDAL();
                var reparticionDAL = new ReparticionDAL();
                _categoriasPlano = categoriaPlanoDAL.ObtenerTodos();
                _tiposPlano = tipoPlanoDAL.ObtenerTodos();
                _reparticiones = reparticionDAL.ObtenerTodos();
            }

            cboCategoriaPlanoFiltro.DisplayMember = "DsCategoriaPlano";
            cboCategoriaPlanoFiltro.ValueMember = "CdCategoriaPlano";
            cboCategoriaPlanoFiltro.DataSource = new List<CategoriaPlano>
            {
                new CategoriaPlano { CdCategoriaPlano = 0, DsCategoriaPlano = "(Todos)" }
            }.Concat(_categoriasPlano).ToList();

            cboTipoPlanoFiltro.DisplayMember = "DsTipoPlano";
            cboTipoPlanoFiltro.ValueMember = "CdTipoPlano";
            cboTipoPlanoFiltro.DataSource = new List<TipoPlano>
            {
                new TipoPlano { CdTipoPlano = 0, DsTipoPlano = "(Todos)" }
            }.Concat(_tiposPlano).ToList();

            var autoCompleteTiposPlano = new AutoCompleteStringCollection();
            autoCompleteTiposPlano.AddRange(_tiposPlano.Select(t => t.DsTipoPlano).ToArray());
            cboTipoPlanoFiltro.AutoCompleteCustomSource = autoCompleteTiposPlano;
            cboTipoPlanoDetalle.AutoCompleteCustomSource = autoCompleteTiposPlano;

            cboCategoriaPlanoDetalle.DisplayMember = "DsCategoriaPlano";
            cboCategoriaPlanoDetalle.ValueMember = "CdCategoriaPlano";
            cboCategoriaPlanoDetalle.DataSource = _categoriasPlano.ToList();

            ActualizarTiposPlanoDetalle(null);
        }

        /// <summary>
        /// Filtra el combo de Tipo de Plano del detalle según la categoría seleccionada.
        /// Si no se indica categoría, muestra todos los tipos.
        /// </summary>
        private void ActualizarTiposPlanoDetalle(int? cdCategoriaPlano)
        {
            var tiposFiltrados = cdCategoriaPlano.HasValue
                ? _tiposPlano.Where(t => t.CdCategoriaPlano == cdCategoriaPlano.Value).ToList()
                : _tiposPlano.ToList();

            cboTipoPlanoDetalle.DisplayMember = "DsTipoPlano";
            cboTipoPlanoDetalle.ValueMember = "CdTipoPlano";
            cboTipoPlanoDetalle.DataSource = tiposFiltrados;
        }

        private void cboCategoriaPlanoDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
            int? cdCategoriaPlano = cboCategoriaPlanoDetalle.SelectedValue is int cdCategoria && cdCategoria != 0
                ? cdCategoria
                : null;

            ActualizarTiposPlanoDetalle(cdCategoriaPlano);
        }

        private void CargarDatosGrilla()
        {
            if (SesionApi.ModoRemoto)
            {
                _detalleRemoto = _apiCliente!.ObtenerDetalleLoteAsync(_cdLote, chkMostrarTodos.Checked)
                    .GetAwaiter().GetResult();

                _filasCompletas = _detalleRemoto.Filas
                    .Where(f => f.Resultado != null)
                    .Select(f => new FilaArchivoPagina
                    {
                        Archivo = new ArchivoPagina
                        {
                            CdArchivoPagina = f.Archivo.CdArchivoPagina,
                            NuPagina = f.Archivo.NuPagina,
                            DsNombreArchivoPagina = f.Archivo.DsNombreArchivoPagina,
                            NombreArchivo = f.Archivo.NombreArchivo,
                            SnGirada = f.Archivo.SnGirada,
                            SnPosibleBlanca = f.Archivo.SnPosibleBlanca
                        },
                        Resultado = new ResultadoIA
                        {
                            CdResultado = f.Resultado!.CdResultado,
                            CdLote = f.Resultado.CdLote,
                            CdArchivoPagina = f.Resultado.CdArchivoPagina,
                            CdCategoriaPlano = f.Resultado.CdCategoriaPlano,
                            CdTipoPlano = f.Resultado.CdTipoPlano,
                            DsNumeroPlano = f.Resultado.DsNumeroPlano,
                            DsExpediente = f.Resultado.DsExpediente,
                            DsSeccion = f.Resultado.DsSeccion,
                            DsManzana = f.Resultado.DsManzana,
                            DsParcela = f.Resultado.DsParcela,
                            DsDireccion = f.Resultado.DsDireccion,
                            CdEstadoControl = f.Resultado.CdEstadoControl,
                            NuConfianzaCategoriaPlano = f.Resultado.NuConfianzaCategoriaPlano,
                            NuConfianzaTipoPlano = f.Resultado.NuConfianzaTipoPlano,
                            NuConfianzaNumeroPlano = f.Resultado.NuConfianzaNumeroPlano,
                            NuConfianzaExpediente = f.Resultado.NuConfianzaExpediente,
                            NuConfianzaSeccion = f.Resultado.NuConfianzaSeccion,
                            NuConfianzaManzana = f.Resultado.NuConfianzaManzana,
                            NuConfianzaParcela = f.Resultado.NuConfianzaParcela,
                            NuConfianzaDireccion = f.Resultado.NuConfianzaDireccion,
                            DsCategoriaPlano = _categoriasPlano.FirstOrDefault(c => c.CdCategoriaPlano == f.Resultado.CdCategoriaPlano)?.DsCategoriaPlano,
                            DsTipoPlano = _tiposPlano.FirstOrDefault(t => t.CdTipoPlano == f.Resultado.CdTipoPlano)?.DsTipoPlano
                        }
                    }).ToList();
            }
            else
            {
                var loteDAL = new LoteDAL();
                var resultadoIADAL = new ResultadoIADAL();

                var archivos = loteDAL.ObtenerArchivosPaginasPorLote(_cdLote);
                var resultados = resultadoIADAL.ObtenerPorLote(_cdLote, chkMostrarTodos.Checked);
                var resultadosPorPagina = resultados.ToDictionary(r => r.CdArchivoPagina, r => r);

                _filasCompletas = archivos
                    .Where(a => resultadosPorPagina.ContainsKey(a.CdArchivoPagina))
                    .Select(a => new FilaArchivoPagina
                    {
                        Archivo = a,
                        Resultado = resultadosPorPagina[a.CdArchivoPagina]
                    }).ToList();
            }

            ConfigurarDataGridView();
            AplicarFiltrosYRefrescar();
        }

        private void chkMostrarTodos_CheckedChanged(object sender, EventArgs e)
        {
            CargarDatosGrilla();
        }

        private void ConfigurarDataGridView()
        {
            if (dgvArchivos.Columns.Count > 0)
                return;

            dgvArchivos.AutoGenerateColumns = false;
            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsNombreArchivoPagina",
                DataPropertyName = "DsNombreArchivoPagina",
                HeaderText = "Archivo/Página"
            });
            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsCategoriaPlano",
                DataPropertyName = "DsCategoriaPlano",
                HeaderText = "Categoría de Plano"
            });
            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsTipoPlano",
                DataPropertyName = "DsTipoPlano",
                HeaderText = "Tipo de Plano"
            });
            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsNumeroPlano",
                DataPropertyName = "DsNumeroPlano",
                HeaderText = "Número de Plano"
            });
            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsExpediente",
                DataPropertyName = "DsExpediente",
                HeaderText = "Expediente"
            });
            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsSeccion",
                DataPropertyName = "DsSeccion",
                HeaderText = "Sección"
            });
            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsManzana",
                DataPropertyName = "DsManzana",
                HeaderText = "Manzana"
            });
            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsParcela",
                DataPropertyName = "DsParcela",
                HeaderText = "Parcela"
            });
            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsDireccion",
                DataPropertyName = "DsDireccion",
                HeaderText = "Dirección"
            });
        }

        #endregion

        #region Filtros

        private void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            AplicarFiltrosYRefrescar();
        }

        private void btnLimpiarFiltrosPagina_Click(object sender, EventArgs e)
        {
            cboCategoriaPlanoFiltro.SelectedIndex = 0;
            cboTipoPlanoFiltro.SelectedIndex = 0;
            chkFaltaCategoriaPlano.Checked = false;
            chkFaltaTipoPlano.Checked = false;
            chkFaltaNumeroPlano.Checked = false;
            chkFaltaExpediente.Checked = false;
            chkFaltaSeccion.Checked = false;
            chkFaltaManzana.Checked = false;
            chkFaltaParcela.Checked = false;
            chkFaltaDireccion.Checked = false;
            nudExactitudMinima.Value = 0;

            AplicarFiltrosYRefrescar();
        }

        private void AplicarFiltrosYRefrescar()
        {
            IEnumerable<FilaArchivoPagina> filas = _filasCompletas;

            if (cboCategoriaPlanoFiltro.SelectedItem is CategoriaPlano categoriaSeleccionada && categoriaSeleccionada.CdCategoriaPlano != 0)
            {
                filas = filas.Where(f => f.Resultado?.CdCategoriaPlano == categoriaSeleccionada.CdCategoriaPlano);
            }

            if (cboTipoPlanoFiltro.SelectedItem is TipoPlano tipoSeleccionado && tipoSeleccionado.CdTipoPlano != 0)
            {
                filas = filas.Where(f => f.Resultado?.CdTipoPlano == tipoSeleccionado.CdTipoPlano);
            }

            if (chkFaltaCategoriaPlano.Checked)
                filas = filas.Where(f => f.Resultado?.CdCategoriaPlano == null);

            if (chkFaltaTipoPlano.Checked)
                filas = filas.Where(f => f.Resultado?.CdTipoPlano == null);

            if (chkFaltaNumeroPlano.Checked)
                filas = filas.Where(f => string.IsNullOrWhiteSpace(f.DsNumeroPlano));

            if (chkFaltaExpediente.Checked)
                filas = filas.Where(f => !EsExpedienteValido(f.DsExpediente));

            if (chkFaltaSeccion.Checked)
                filas = filas.Where(f => string.IsNullOrWhiteSpace(f.DsSeccion));

            if (chkFaltaManzana.Checked)
                filas = filas.Where(f => string.IsNullOrWhiteSpace(f.DsManzana));

            if (chkFaltaParcela.Checked)
                filas = filas.Where(f => string.IsNullOrWhiteSpace(f.DsParcela));

            if (chkFaltaDireccion.Checked)
                filas = filas.Where(f => string.IsNullOrWhiteSpace(f.DsDireccion));

            if (nudExactitudMinima.Value > 0)
                filas = filas.Where(f => f.ExactitudPromedio >= nudExactitudMinima.Value);

            dgvArchivos.DataSource = filas.ToList();
        }

        #endregion

        #region Selección de fila / panel de detalle

        private void dgvArchivos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArchivos.CurrentRow?.DataBoundItem is FilaArchivoPagina fila)
            {
                _filaSeleccionada = fila;
                MostrarDetalle(fila);
            }
            else
            {
                _filaSeleccionada = null;
                LimpiarPanelDetalle();
            }
        }

        private void MostrarDetalle(FilaArchivoPagina fila)
        {
            lblCabeceraArchivo.Text = fila.Archivo.DsNombreArchivoPagina;
            lblNombreArchivoOriginal.Text = $"Archivo original: {fila.Archivo.NombreArchivo ?? fila.Archivo.DsArchivoOriginal}";
            lblNuPagina.Text = $"Página: {fila.Archivo.NuPagina}";
            lblSnGirada.Text = $"Girada: {fila.Archivo.SnGirada}";
            lblSnPosibleBlanca.Text = $"Posible Blanca: {fila.Archivo.SnPosibleBlanca}";

            var resultado = fila.Resultado;

            _valoresOriginalesResultado = resultado == null ? null : new ResultadoIA
            {
                CdResultado = resultado.CdResultado,
                CdCategoriaPlano = resultado.CdCategoriaPlano,
                CdTipoPlano = resultado.CdTipoPlano,
                DsNumeroPlano = resultado.DsNumeroPlano,
                DsExpediente = resultado.DsExpediente,
                DsSeccion = resultado.DsSeccion,
                DsManzana = resultado.DsManzana,
                DsParcela = resultado.DsParcela,
                DsDireccion = resultado.DsDireccion
            };

            cboCategoriaPlanoDetalle.SelectedValue = resultado?.CdCategoriaPlano ?? 0;
            ActualizarTiposPlanoDetalle(resultado?.CdCategoriaPlano);
            cboTipoPlanoDetalle.SelectedValue = resultado?.CdTipoPlano ?? 0;
            txtNumeroPlano.Text = resultado?.DsNumeroPlano ?? string.Empty;
            CargarExpedienteEnCampos(resultado?.DsExpediente);
            txtSeccion.Text = resultado?.DsSeccion ?? string.Empty;
            txtManzana.Text = resultado?.DsManzana ?? string.Empty;
            txtParcela.Text = resultado?.DsParcela ?? string.Empty;
            txtDireccion.Text = resultado?.DsDireccion ?? string.Empty;

            lblConfianzaCategoriaPlano.Text = FormatearConfianza(resultado?.NuConfianzaCategoriaPlano);
            lblConfianzaTipoPlano.Text = FormatearConfianza(resultado?.NuConfianzaTipoPlano);
            lblConfianzaNumeroPlano.Text = FormatearConfianza(resultado?.NuConfianzaNumeroPlano);
            lblConfianzaExpediente.Text = FormatearConfianza(resultado?.NuConfianzaExpediente);
            lblConfianzaSeccion.Text = FormatearConfianza(resultado?.NuConfianzaSeccion);
            lblConfianzaManzana.Text = FormatearConfianza(resultado?.NuConfianzaManzana);
            lblConfianzaParcela.Text = FormatearConfianza(resultado?.NuConfianzaParcela);
            lblConfianzaDireccion.Text = FormatearConfianza(resultado?.NuConfianzaDireccion);

            MostrarVisorJPG();
            CargarImagenDeFila(fila);
        }

        private void LimpiarPanelDetalle()
        {
            _valoresOriginalesResultado = null;
            lblCabeceraArchivo.Text = "(seleccione una página)";
            lblNombreArchivoOriginal.Text = "Archivo original: -";
            lblNuPagina.Text = "Página: -";
            lblSnGirada.Text = "Girada: -";
            lblSnPosibleBlanca.Text = "Posible Blanca: -";

            cboCategoriaPlanoDetalle.SelectedIndex = -1;
            ActualizarTiposPlanoDetalle(null);
            cboTipoPlanoDetalle.SelectedIndex = -1;
            txtNumeroPlano.Text = string.Empty;
            CargarExpedienteEnCampos(null);
            txtSeccion.Text = string.Empty;
            txtManzana.Text = string.Empty;
            txtParcela.Text = string.Empty;
            txtDireccion.Text = string.Empty;

            lblConfianzaCategoriaPlano.Text = "-- %";
            lblConfianzaTipoPlano.Text = "-- %";
            lblConfianzaNumeroPlano.Text = "-- %";
            lblConfianzaExpediente.Text = "-- %";
            lblConfianzaSeccion.Text = "-- %";
            lblConfianzaManzana.Text = "-- %";
            lblConfianzaParcela.Text = "-- %";
            lblConfianzaDireccion.Text = "-- %";

            MostrarVisorJPG();
            LimpiarImagen();
        }

        private static string FormatearConfianza(decimal? confianza)
        {
            return confianza.HasValue ? $"{confianza.Value * 100m:0.#} %" : "-- %";
        }

        /// <summary>
        /// Valida que el expediente respete el formato EX-YYYY-NNNNNNNN-GCABA-REPARTICION
        /// (EX literal, anio de 4 digitos, numero de 8 digitos, GCABA literal y reparticion no vacia).
        /// Si no respeta este formato, se considera como si faltara el expediente.
        /// </summary>
        private static readonly System.Text.RegularExpressions.Regex _regexExpedienteValido =
            new(@"^EX-\d{4}-\d{8}-GCABA-.+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        private static bool EsExpedienteValido(string? dsExpediente)
        {
            if (string.IsNullOrWhiteSpace(dsExpediente))
                return false;

            return _regexExpedienteValido.IsMatch(dsExpediente.Trim());
        }

        /// <summary>
        /// Parsea el nombre del archivo original (formato EX-ANIO-NUMERO-XXX-REPARTICION-...)
        /// y vuelca el anio, numero y reparticion en los campos de expediente, siempre
        /// que las validaciones (anio entre 1900-2050, numero de 8 digitos) se cumplan.
        /// Si el parseo o alguna validacion falla, no realiza ningun cambio.
        /// </summary>
        private void btnParsearExpedienteDeArchivo_Click(object sender, EventArgs e)
        {
            if (_filaSeleccionada == null)
                return;

            string? nombreArchivoOriginal = _filaSeleccionada.Archivo.NombreArchivo ?? _filaSeleccionada.Archivo.DsArchivoOriginal;
            if (string.IsNullOrWhiteSpace(nombreArchivoOriginal))
                return;

            string nombreSinExtension = Path.GetFileNameWithoutExtension(nombreArchivoOriginal);
            string[] partes = nombreSinExtension.Split('-', StringSplitOptions.None);

            if (partes.Length < 6)
                return;

            string anio = partes[1].Trim();
            string numero = partes[2].Trim();
            string reparticion = partes[5].Trim();

            if (!int.TryParse(anio, out int anioValor) || anioValor < 1900 || anioValor > 2050)
                return;

            if (numero.Length != 8 || !numero.All(char.IsDigit))
                return;

            if (string.IsNullOrWhiteSpace(reparticion))
                return;

            txtExpedienteAnio.Text = anio;
            txtExpedienteNumero.Text = numero;
            txtExpedienteReparticion.Text = reparticion;
        }

        /// <summary>
        /// Distribuye el expediente (formato EX-ANIO-NUMERO-GCABA-REPARTICION) en los
        /// 5 textbox del panel de detalle. Si no tiene exactamente 5 partes, se vuelca
        /// el valor completo en el 5to campo (Reparticion) para no perder informacion.
        /// </summary>
        private void CargarExpedienteEnCampos(string? dsExpediente)
        {
            txtExpedienteEx.Text = "EX";
            txtExpedienteGcaba.Text = "GCABA";

            if (string.IsNullOrWhiteSpace(dsExpediente))
            {
                txtExpedienteAnio.Text = string.Empty;
                txtExpedienteNumero.Text = string.Empty;
                txtExpedienteReparticion.Text = string.Empty;
                return;
            }

            string[] partes = dsExpediente.Split('-', StringSplitOptions.None);

            if (partes.Length == 5)
            {
                txtExpedienteAnio.Text = partes[1].Trim();
                txtExpedienteNumero.Text = partes[2].Trim();
                txtExpedienteReparticion.Text = partes[4].Trim();
            }
            else
            {
                txtExpedienteAnio.Text = string.Empty;
                txtExpedienteNumero.Text = string.Empty;
                txtExpedienteReparticion.Text = dsExpediente.Trim();
            }
        }

        /// <summary>
        /// Reconstruye el expediente concatenando los 5 campos con guiones, completando
        /// el numero de expediente con ceros a la izquierda hasta 8 digitos y validando
        /// que la reparticion exista en TD_REPARTICIONES.
        /// </summary>
        private string? ArmarExpedienteDesdeCampos(out bool reparticionValida)
        {
            reparticionValida = true;

            const string ex = "EX";
            string anio = txtExpedienteAnio.Text.Trim();
            string numero = txtExpedienteNumero.Text.Trim();
            const string gcaba = "GCABA";
            string reparticion = txtExpedienteReparticion.Text.Trim();

            if (string.IsNullOrEmpty(anio) && string.IsNullOrEmpty(numero) && string.IsNullOrEmpty(reparticion))
            {
                return null;
            }

            if (int.TryParse(numero, out int numeroExpediente))
            {
                numero = numeroExpediente.ToString("D8");
            }

            if (!string.IsNullOrEmpty(reparticion))
            {
                reparticionValida = _reparticiones.Any(r =>
                    string.Equals(r.DsReparticion, reparticion, StringComparison.OrdinalIgnoreCase));
            }

            return $"{ex}-{anio}-{numero}-{gcaba}-{reparticion}";
        }

        /// <summary>
        /// La ruta almacenada en DsRutaCompleta corresponde al archivo PDF original de la página;
        /// el recorte JPG usado por el visor se guarda con el mismo nombre y extensión .jpg.
        /// </summary>
        private static string ObtenerRutaImagenJPG(string rutaCompleta)
        {
            return string.IsNullOrWhiteSpace(rutaCompleta)
                ? string.Empty
                : Path.ChangeExtension(rutaCompleta, ".jpg");
        }

        #endregion

        #region Visor de imagen / zoom

        /// <summary>
        /// Carga la imagen JPG del visor para la fila indicada, obteniendo los bytes
        /// desde la API (modo remoto, ya que el cliente no tiene acceso al storage)
        /// o desde el archivo local (modo local).
        /// </summary>
        private void CargarImagenDeFila(FilaArchivoPagina fila)
        {
            if (SesionApi.ModoRemoto)
            {
                try
                {
                    byte[] bytes = _apiCliente!.ObtenerJpgAsync(_cdLote, fila.Archivo.CdArchivoPagina)
                        .GetAwaiter().GetResult();
                    CargarImagenDesdeBytes(bytes);
                }
                catch (Exception ex)
                {
                    LimpiarImagen();
                    MessageBox.Show($"No se pudo cargar la imagen desde la API: {ex.Message}", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                CargarImagen(ObtenerRutaImagenJPG(fila.Archivo.DsRutaCompleta));
            }
        }

        private void CargarImagen(string rutaImagen)
        {
            LimpiarImagen();

            if (string.IsNullOrWhiteSpace(rutaImagen) || !File.Exists(rutaImagen))
            {
                return;
            }

            try
            {
                CargarImagenDesdeBytes(File.ReadAllBytes(rutaImagen));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo cargar la imagen: {ex.Message}", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CargarImagenDesdeBytes(byte[] bytes)
        {
            LimpiarImagen();

            if (bytes == null || bytes.Length == 0)
                return;

            try
            {
                using var stream = new MemoryStream(bytes);
                pictureBoxImagen.Image = Image.FromStream(stream);
                _factorZoom = 1.0f;
                AplicarZoom();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo cargar la imagen: {ex.Message}", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LimpiarImagen()
        {
            pictureBoxImagen.Image?.Dispose();
            pictureBoxImagen.Image = null;
            _factorZoom = 1.0f;
            lblZoom.Text = "100 %";
        }

        /// <summary>
        /// Muestra el visor de imagen JPG (recorte). El PDF original se abre con la
        /// aplicación predeterminada de Windows en lugar de mostrarse embebido en la pantalla.
        /// </summary>
        private void MostrarVisorJPG()
        {
            pictureBoxImagen.Visible = true;
        }

        /// <summary>
        /// Abre el PDF original de la página con la aplicación predeterminada de Windows
        /// (por ejemplo, Edge o el lector de PDF configurado por el usuario).
        /// </summary>
        private static void AbrirPdfConAplicacionPredeterminada(string rutaPdf)
        {
            if (string.IsNullOrWhiteSpace(rutaPdf) || !File.Exists(rutaPdf))
            {
                MessageBox.Show("No se encontró el archivo PDF original.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo(rutaPdf)
                {
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo abrir el PDF: {ex.Message}", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AplicarZoom()
        {
            if (pictureBoxImagen.Image == null)
                return;

            int nuevoAncho = (int)(pictureBoxImagen.Image.Width * _factorZoom);
            int nuevoAlto = (int)(pictureBoxImagen.Image.Height * _factorZoom);

            pictureBoxImagen.Size = new Size(nuevoAncho, nuevoAlto);
            lblZoom.Text = $"{(int)(_factorZoom * 100)} %";
        }

        // Ajuste 2 (posicionar la imagen en la esquina inferior derecha) comentado temporalmente:
        // aun no se comporta como se espera en todos los casos (paginas con distinta resolucion),
        // se deja el codigo para retomarlo mas adelante.
        // /// <summary>
        // /// Ubica la imagen del visor en la esquina inferior derecha del area de scroll.
        // /// Si la imagen es mas chica que el area visible, se alinea el control ahi.
        // /// Si la imagen es mas grande (caso mas comun, con scrollbars), se desplaza el
        // /// scroll para que la vista inicial muestre la esquina inferior derecha de la
        // /// imagen, que es donde generalmente estan los datos a controlar.
        // /// </summary>
        // private void PosicionarImagenEnEsquinaInferiorDerecha()
        // {
        //     if (pictureBoxImagen.Image == null)
        //         return;
        //
        //     int x = Math.Max(0, panelImagenScroll.ClientSize.Width - pictureBoxImagen.Width);
        //     int y = Math.Max(0, panelImagenScroll.ClientSize.Height - pictureBoxImagen.Height);
        //
        //     pictureBoxImagen.Location = new Point(x, y);
        //
        //     int desplazamientoX = Math.Max(0, pictureBoxImagen.Width - panelImagenScroll.ClientSize.Width);
        //     int desplazamientoY = Math.Max(0, pictureBoxImagen.Height - panelImagenScroll.ClientSize.Height);
        //
        //     panelImagenScroll.AutoScrollPosition = new Point(desplazamientoX, desplazamientoY);
        // }

        private void btnZoomMas_Click(object sender, EventArgs e)
        {
            _factorZoom = Math.Min(_factorZoom + 0.25f, 5.0f);
            AplicarZoom();
        }

        private void btnZoomMenos_Click(object sender, EventArgs e)
        {
            _factorZoom = Math.Max(_factorZoom - 0.25f, 0.25f);
            AplicarZoom();
        }

        private void btnZoomAjustar_Click(object sender, EventArgs e)
        {
            if (pictureBoxImagen.Image == null)
                return;

            float factorAncho = (float)panelImagenScroll.ClientSize.Width / pictureBoxImagen.Image.Width;
            float factorAlto = (float)panelImagenScroll.ClientSize.Height / pictureBoxImagen.Image.Height;
            _factorZoom = Math.Min(factorAncho, factorAlto);
            AplicarZoom();
        }

        /// <summary>
        /// Permite arrastrar la imagen con el mouse para navegar cuando el zoom
        /// hace que sea más grande que el área visible de panelImagenScroll.
        /// Se engancha tanto en pictureBoxImagen como en panelImagenScroll (para que
        /// también funcione al hacer click en el fondo/borde negro alrededor de la imagen).
        /// Las coordenadas se convierten a pantalla para que el arrastre sea consistente
        /// sin importar cuál de los dos controles disparó el evento.
        /// </summary>
        private void pictureBoxImagen_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || pictureBoxImagen.Image == null || sender is not Control control)
                return;

            _arrastrandoImagen = true;
            _puntoInicialArrastre = control.PointToScreen(e.Location);
            pictureBoxImagen.Cursor = Cursors.SizeAll;
        }

        private void pictureBoxImagen_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!_arrastrandoImagen || sender is not Control control)
                return;

            Point puntoActual = control.PointToScreen(e.Location);
            int deltaX = puntoActual.X - _puntoInicialArrastre.X;
            int deltaY = puntoActual.Y - _puntoInicialArrastre.Y;
            _puntoInicialArrastre = puntoActual;

            var posicionActual = panelImagenScroll.AutoScrollPosition;
            int nuevoX = -posicionActual.X - deltaX;
            int nuevoY = -posicionActual.Y - deltaY;

            panelImagenScroll.AutoScrollPosition = new Point(nuevoX, nuevoY);
        }

        private void pictureBoxImagen_MouseUp(object? sender, MouseEventArgs e)
        {
            if (!_arrastrandoImagen)
                return;

            _arrastrandoImagen = false;
            pictureBoxImagen.Cursor = Cursors.Default;
        }

        #endregion

        #region Botones de acción (funcionalidad completa en Parte 3)

        private void btnVerImagenPDF_Click(object sender, EventArgs e)
        {
            if (_filaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una página de la grilla.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (SesionApi.ModoRemoto)
            {
                AbrirPdfRemoto(_filaSeleccionada.Archivo);
            }
            else
            {
                AbrirPdfConAplicacionPredeterminada(_filaSeleccionada.Archivo.DsRutaCompleta);
            }
        }

        /// <summary>
        /// Descarga el PDF de la página seleccionada desde la API y lo abre con la
        /// aplicación predeterminada de Windows, ya que el cliente no tiene acceso
        /// directo al storage del servidor en modo remoto.
        /// </summary>
        private void AbrirPdfRemoto(ArchivoPagina archivo)
        {
            try
            {
                byte[] bytes = _apiCliente!.ObtenerPdfAsync(_cdLote, archivo.CdArchivoPagina)
                    .GetAwaiter().GetResult();

                string rutaTemporal = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.pdf");
                File.WriteAllBytes(rutaTemporal, bytes);

                var psi = new System.Diagnostics.ProcessStartInfo(rutaTemporal)
                {
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo obtener el PDF desde la API: {ex.Message}", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGuardarControlada_Click(object sender, EventArgs e)
        {
            if (_filaSeleccionada?.Resultado == null)
            {
                MessageBox.Show("Seleccione una página de la grilla.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool categoriaCompleta = cboCategoriaPlanoDetalle.SelectedValue is int cdCategoriaPlanoValidar && cdCategoriaPlanoValidar != 0;
            bool tipoPlanoCompleto = cboTipoPlanoDetalle.SelectedValue is int cdTipoPlanoValidar && cdTipoPlanoValidar != 0;

            if (!categoriaCompleta
                || !tipoPlanoCompleto
                || string.IsNullOrWhiteSpace(txtSeccion.Text)
                || string.IsNullOrWhiteSpace(txtManzana.Text)
                || string.IsNullOrWhiteSpace(txtParcela.Text)
                || string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                MessageBox.Show(
                    "Para marcar el registro como Controlado debe completar los campos: Categoría de Plano, Tipo de Plano, Sección, Manzana, Parcela y Dirección.\n\nLos campos Expediente y Número de Plano pueden quedar incompletos.",
                    "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string? dsExpedienteArmado = ArmarExpedienteDesdeCampos(out bool reparticionValida);

            if (!reparticionValida)
            {
                MessageBox.Show(
                    $"La repartición \"{txtExpedienteReparticion.Text.Trim()}\" no existe en TD_REPARTICIONES. Corríjala antes de guardar.",
                    "Repartición inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var resultado = _filaSeleccionada.Resultado;
                int cdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0;

                resultado.CdCategoriaPlano = cboCategoriaPlanoDetalle.SelectedValue is int cdCategoriaPlano && cdCategoriaPlano != 0 ? cdCategoriaPlano : null;
                resultado.CdTipoPlano = cboTipoPlanoDetalle.SelectedValue is int cdTipoPlano && cdTipoPlano != 0 ? cdTipoPlano : null;
                resultado.DsNumeroPlano = txtNumeroPlano.Text;
                resultado.DsExpediente = dsExpedienteArmado;
                resultado.DsSeccion = txtSeccion.Text;
                resultado.DsManzana = txtManzana.Text;
                resultado.DsParcela = txtParcela.Text;
                resultado.DsDireccion = txtDireccion.Text;

                bool modificoDatos = DatosFueronModificados(resultado);

                if (SesionApi.ModoRemoto)
                {
                    var requestDatos = new ActualizarResultadoApiRequestDto
                    {
                        CdCategoriaPlano = resultado.CdCategoriaPlano,
                        CdTipoPlano = resultado.CdTipoPlano,
                        DsNumeroPlano = resultado.DsNumeroPlano,
                        DsExpediente = resultado.DsExpediente,
                        DsSeccion = resultado.DsSeccion,
                        DsManzana = resultado.DsManzana,
                        DsParcela = resultado.DsParcela,
                        DsDireccion = resultado.DsDireccion
                    };

                    _apiCliente!.ActualizarDatosResultadoAsync(resultado.CdResultado, requestDatos)
                        .GetAwaiter().GetResult();

                    _apiCliente!.ActualizarEstadoControlResultadoAsync(resultado.CdResultado, new ActualizarEstadoControlApiRequestDto
                    {
                        CdEstadoControl = ResultadoIA.EstadosControl.Controlado,
                        SnModificaDatos = modificoDatos ? "SI" : "NO"
                    }).GetAwaiter().GetResult();
                }
                else
                {
                    RegistrarCorreccionesSiCorresponde(resultado, cdUsuario);

                    var resultadoIADAL = new ResultadoIADAL();
                    resultadoIADAL.ActualizarDatos(resultado, cdUsuario);
                    resultadoIADAL.ActualizarEstadoControl(
                        resultado.CdResultado,
                        ResultadoIA.EstadosControl.Controlado,
                        modificoDatos ? "SI" : "NO",
                        cdUsuario);
                }

                CargarDatosGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar y marcar como controlada: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Determina si los datos del panel de detalle difieren de los valores originales cargados
        /// desde la base de datos para el resultado seleccionado.
        /// </summary>
        private bool DatosFueronModificados(ResultadoIA resultadoActualizado)
        {
            if (_valoresOriginalesResultado == null)
                return false;

            var original = _valoresOriginalesResultado;
            return original.CdCategoriaPlano != resultadoActualizado.CdCategoriaPlano
                || original.CdTipoPlano != resultadoActualizado.CdTipoPlano
                || original.DsNumeroPlano != resultadoActualizado.DsNumeroPlano
                || original.DsExpediente != resultadoActualizado.DsExpediente
                || original.DsSeccion != resultadoActualizado.DsSeccion
                || original.DsManzana != resultadoActualizado.DsManzana
                || original.DsParcela != resultadoActualizado.DsParcela
                || original.DsDireccion != resultadoActualizado.DsDireccion;
        }

        /// <summary>
        /// Compara los valores originales contra los actualizados y registra en TD_CORRECIONES
        /// cada campo que haya sido modificado manualmente, con fines estadísticos.
        /// </summary>
        private void RegistrarCorreccionesSiCorresponde(ResultadoIA resultadoActualizado, int cdUsuario)
        {
            if (_valoresOriginalesResultado == null)
                return;

            var original = _valoresOriginalesResultado;
            var correccionDAL = new CorreccionDAL();

            void RegistrarSiDistinto(string dsCampo, string? valorAnterior, string? valorNuevo)
            {
                if (valorAnterior == valorNuevo)
                    return;

                correccionDAL.Insertar(new Correccion
                {
                    CdResultado = resultadoActualizado.CdResultado,
                    DsCampo = dsCampo,
                    DsValorAnterior = valorAnterior,
                    DsValorNuevo = valorNuevo,
                    CdUsuarioControl = cdUsuario
                });
            }

            RegistrarSiDistinto(Correccion.Campos.Categoria,
                ObtenerDescripcionCategoria(original.CdCategoriaPlano),
                ObtenerDescripcionCategoria(resultadoActualizado.CdCategoriaPlano));

            RegistrarSiDistinto(Correccion.Campos.TipoPlano,
                ObtenerDescripcionTipoPlano(original.CdTipoPlano),
                ObtenerDescripcionTipoPlano(resultadoActualizado.CdTipoPlano));

            RegistrarSiDistinto(Correccion.Campos.Direccion, original.DsDireccion, resultadoActualizado.DsDireccion);
            RegistrarSiDistinto(Correccion.Campos.Seccion, original.DsSeccion, resultadoActualizado.DsSeccion);
            RegistrarSiDistinto(Correccion.Campos.Manzana, original.DsManzana, resultadoActualizado.DsManzana);
            RegistrarSiDistinto(Correccion.Campos.Parcela, original.DsParcela, resultadoActualizado.DsParcela);
            RegistrarSiDistinto(Correccion.Campos.Expediente, original.DsExpediente, resultadoActualizado.DsExpediente);
            RegistrarSiDistinto(Correccion.Campos.NumeroPlano, original.DsNumeroPlano, resultadoActualizado.DsNumeroPlano);
        }

        private string? ObtenerDescripcionCategoria(int? cdCategoriaPlano)
        {
            if (!cdCategoriaPlano.HasValue)
                return null;

            return _categoriasPlano.FirstOrDefault(c => c.CdCategoriaPlano == cdCategoriaPlano.Value)?.DsCategoriaPlano
                ?? cdCategoriaPlano.Value.ToString();
        }

        private string? ObtenerDescripcionTipoPlano(int? cdTipoPlano)
        {
            if (!cdTipoPlano.HasValue)
                return null;

            return _tiposPlano.FirstOrDefault(t => t.CdTipoPlano == cdTipoPlano.Value)?.DsTipoPlano
                ?? cdTipoPlano.Value.ToString();
        }

        private void btnGirarIzquierda_Click(object sender, EventArgs e)
        {
            RotarPaginaActual(-90);
        }

        private void btnGirarDerecha_Click(object sender, EventArgs e)
        {
            RotarPaginaActual(90);
        }

        /// <summary>
        /// Rota tanto el recorte JPG como el PDF original de la página seleccionada,
        /// actualiza la vista y deja registro de la rotación manual en la base de datos.
        /// </summary>
        private void RotarPaginaActual(int grados)
        {
            if (SesionApi.ModoRemoto)
            {
                MessageBox.Show(
                    "Esta función no está disponible en modo remoto (requiere acceso directo al almacenamiento de archivos).",
                    "Función no disponible", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_filaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una página de la grilla.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var archivo = _filaSeleccionada.Archivo;
            string rutaPdf = archivo.DsRutaCompleta;
            string rutaJpg = ObtenerRutaImagenJPG(rutaPdf);

            try
            {
                RotarArchivoPDF(rutaPdf, grados);
                RotarArchivoJPG(rutaJpg, grados);

                int gradosNormalizados = ((grados % 360) + 360) % 360;

                var rotacionDAL = new RotacionDAL();
                rotacionDAL.Insertar(new Rotacion
                {
                    CdArchivoPagina = archivo.CdArchivoPagina,
                    SnRotacionAutomatica = "NO",
                    NuRotacionAplicada = gradosNormalizados,
                    SnRotacionManual = "SI",
                    CdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0
                });

                var archivoPaginaDAL = new ArchivoPaginaDAL();
                archivoPaginaDAL.ActualizarSnGirada(archivo.CdArchivoPagina, "SI");
                archivo.SnGirada = "SI";
                lblSnGirada.Text = $"Girada: {archivo.SnGirada}";

                MostrarVisorJPG();
                CargarImagen(rutaJpg);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo rotar la página: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void RotarArchivoPDF(string rutaPdf, int grados)
        {
            if (string.IsNullOrWhiteSpace(rutaPdf) || !File.Exists(rutaPdf))
                return;

            string rutaTemporal = Path.Combine(
                Path.GetDirectoryName(rutaPdf) ?? string.Empty,
                $"{Path.GetFileNameWithoutExtension(rutaPdf)}_tmp_{Guid.NewGuid():N}.pdf");

            try
            {
                ProcesamientoPaginas.ExtraerYRotarPaginaPorCopiaYEliminacion(rutaPdf, 1, rutaTemporal, grados);

                File.Delete(rutaPdf);
                File.Move(rutaTemporal, rutaPdf);
            }
            finally
            {
                if (File.Exists(rutaTemporal))
                {
                    try { File.Delete(rutaTemporal); } catch { }
                }
            }
        }

        private static void RotarArchivoJPG(string rutaJpg, int grados)
        {
            if (string.IsNullOrWhiteSpace(rutaJpg) || !File.Exists(rutaJpg))
                return;

            int gradosNormalizados = ((grados % 360) + 360) % 360;
            var rotateFlip = gradosNormalizados switch
            {
                90 => RotateFlipType.Rotate90FlipNone,
                180 => RotateFlipType.Rotate180FlipNone,
                270 => RotateFlipType.Rotate270FlipNone,
                _ => RotateFlipType.RotateNoneFlipNone
            };

            using (var imagenOriginal = Image.FromFile(rutaJpg))
            using (var imagenRotada = new Bitmap(imagenOriginal))
            {
                imagenRotada.RotateFlip(rotateFlip);
                imagenOriginal.Dispose();
                imagenRotada.Save(rutaJpg, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void btnMarcarPaginaIlegible_Click(object sender, EventArgs e)
        {
            var confirmacion = MessageBox.Show(
                "¿Confirma que desea marcar la página como ILEGIBLE?",
                "Marcar Página como ILEGIBLE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            MarcarEstadoControl(ResultadoIA.EstadosControl.PaginaIlegible, "Marcar Página como ILEGIBLE");
        }

        private void btnMarcarDatosIlegible_Click(object sender, EventArgs e)
        {
            var confirmacion = MessageBox.Show(
                "¿Confirma que desea marcar los datos como ILEGIBLES?",
                "Marcar Datos ILEGIBLE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            MarcarEstadoControl(ResultadoIA.EstadosControl.DatosIlegibles, "Marcar Datos ILEGIBLE");
        }

        /// <summary>
        /// Marca el registro seleccionado con el estado de control indicado (Página Ilegible o Datos Ilegibles),
        /// registrando fecha y usuario de control.
        /// </summary>
        private void MarcarEstadoControl(int cdEstadoControl, string tituloAccion)
        {
            if (_filaSeleccionada?.Resultado == null)
            {
                MessageBox.Show("Seleccione una página de la grilla.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (SesionApi.ModoRemoto)
                {
                    _apiCliente!.ActualizarEstadoControlResultadoAsync(_filaSeleccionada.Resultado.CdResultado, new ActualizarEstadoControlApiRequestDto
                    {
                        CdEstadoControl = cdEstadoControl,
                        SnModificaDatos = "NO"
                    }).GetAwaiter().GetResult();
                }
                else
                {
                    int cdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0;
                    var resultadoIADAL = new ResultadoIADAL();
                    resultadoIADAL.ActualizarEstadoControl(_filaSeleccionada.Resultado.CdResultado, cdEstadoControl, "NO", cdUsuario);
                }

                MessageBox.Show("Registro actualizado correctamente.", tituloAccion,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarDatosGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo actualizar el estado: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMarcarLoteCompletado_Click(object sender, EventArgs e)
        {
            try
            {
                var loteFinalizacionBL = new LoteFinalizacionBL();

                List<LoteFinalizacionBL.FilaFinalizacion> filas;

                if (SesionApi.ModoRemoto)
                {
                    // Se vuelve a pedir el detalle con mostrarTodos=true para no depender
                    // del estado del checkbox "Mostrar Todos" (si está destildado, la
                    // grilla puede no incluir los registros ya controlados).
                    var detalleCompleto = _apiCliente!.ObtenerDetalleLoteAsync(_cdLote, mostrarTodos: true)
                        .GetAwaiter().GetResult();

                    filas = detalleCompleto.Filas
                        .Where(f => f.Resultado != null)
                        .Select(f => new LoteFinalizacionBL.FilaFinalizacion
                        {
                            Archivo = new ArchivoPagina
                            {
                                CdArchivoPagina = f.Archivo.CdArchivoPagina,
                                NuPagina = f.Archivo.NuPagina,
                                DsNombreArchivoPagina = f.Archivo.DsNombreArchivoPagina,
                                NombreArchivo = f.Archivo.NombreArchivo,
                                SnGirada = f.Archivo.SnGirada,
                                SnPosibleBlanca = f.Archivo.SnPosibleBlanca
                            },
                            Resultado = new ResultadoIA
                            {
                                CdResultado = f.Resultado!.CdResultado,
                                CdLote = f.Resultado.CdLote,
                                CdArchivoPagina = f.Resultado.CdArchivoPagina,
                                CdCategoriaPlano = f.Resultado.CdCategoriaPlano,
                                CdTipoPlano = f.Resultado.CdTipoPlano,
                                DsNumeroPlano = f.Resultado.DsNumeroPlano,
                                DsExpediente = f.Resultado.DsExpediente,
                                DsSeccion = f.Resultado.DsSeccion,
                                DsManzana = f.Resultado.DsManzana,
                                DsParcela = f.Resultado.DsParcela,
                                DsDireccion = f.Resultado.DsDireccion,
                                CdEstadoControl = f.Resultado.CdEstadoControl
                            }
                        }).ToList();
                }
                else
                {
                    var loteDAL = new LoteDAL();
                    var resultadoIADAL = new ResultadoIADAL();

                    var resultados = resultadoIADAL.ObtenerPorLote(_cdLote, mostrarTodos: true);
                    var archivosPorPagina = loteDAL.ObtenerArchivosPaginasPorLote(_cdLote)
                        .ToDictionary(a => a.CdArchivoPagina, a => a);

                    filas = resultados
                        .Where(r => archivosPorPagina.ContainsKey(r.CdArchivoPagina))
                        .Select(r => new LoteFinalizacionBL.FilaFinalizacion
                        {
                            Archivo = archivosPorPagina[r.CdArchivoPagina],
                            Resultado = r
                        }).ToList();
                }

                if (filas.Count == 0)
                {
                    MessageBox.Show("No hay registros en el lote para procesar.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Control 1: campos obligatorios completos para estados Pendiente de Control y Controlado
                var faltantes = loteFinalizacionBL.ValidarCamposObligatorios(filas);
                if (faltantes.Count > 0)
                {
                    MessageBox.Show(
                        $"Existen {faltantes.Count} registro(s) sin completar los campos obligatorios " +
                        "(Tipo de Plano, Sección, Manzana, Parcela, Dirección).\n\n" +
                        "Complete los datos faltantes antes de marcar el lote como completado.",
                        "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Control 2: advertir si quedan registros sin controlar (Pendiente de Control)
                int cantidadPendientes = loteFinalizacionBL.ContarPendientesControl(filas);
                if (cantidadPendientes > 0)
                {
                    var respuesta = MessageBox.Show(
                        $"Quedan {cantidadPendientes} registro(s) sin controlar (Pendiente de Control).\n\n" +
                        "¿Desea continuar de todas formas?",
                        "Registros sin controlar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (respuesta != DialogResult.Yes)
                        return;
                }

                var confirmacion = MessageBox.Show(
                    $"El lote tiene {filas.Count} registro(s) listo(s). " +
                    "El lote quedará marcado como \"Pendiente de Finalizar\" y su finalización " +
                    "(movimiento de archivos y generación del CSV) se realizará desde la pantalla de finalización.\n\n" +
                    "¿Desea continuar?",
                    "Marcar Lote Pendiente de Finalizar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes)
                    return;

                if (SesionApi.ModoRemoto)
                {
                    _apiCliente!.MarcarLotePendienteFinalizarAsync(_cdLote).GetAwaiter().GetResult();
                }
                else
                {
                    int cdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0;
                    loteFinalizacionBL.MarcarLotePendienteFinalizar(_cdLote, cdUsuario);
                }

                MessageBox.Show("El lote fue marcado como Pendiente de Finalizar.", "Marcar Lote Pendiente de Finalizar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo completar el lote: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
