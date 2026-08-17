using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;
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

                    return valores.Count == 0 ? 0 : Math.Round(valores.Average(), 1);
                }
            }
        }

        public FrmVerLote(int cdLote)
        {
            InitializeComponent();
            _cdLote = cdLote;

            pictureBoxImagen.MouseDown += pictureBoxImagen_MouseDown;
            pictureBoxImagen.MouseMove += pictureBoxImagen_MouseMove;
            pictureBoxImagen.MouseUp += pictureBoxImagen_MouseUp;
        }

        private void FrmVerLote_Load(object sender, EventArgs e)
        {
            try
            {
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
            var categoriaPlanoDAL = new CategoriaPlanoDAL();
            var tipoPlanoDAL = new TipoPlanoDAL();
            var reparticionDAL = new ReparticionDAL();
            _categoriasPlano = categoriaPlanoDAL.ObtenerTodos();
            _tiposPlano = tipoPlanoDAL.ObtenerTodos();
            _reparticiones = reparticionDAL.ObtenerTodos();

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
                filas = filas.Where(f => string.IsNullOrWhiteSpace(f.DsExpediente));

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
            CargarImagen(ObtenerRutaImagenJPG(fila.Archivo.DsRutaCompleta));
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
            return confianza.HasValue ? $"{confianza.Value:0.#} %" : "-- %";
        }

        /// <summary>
        /// Distribuye el expediente (formato EX-ANIO-NUMERO-GCABA-REPARTICION) en los
        /// 5 textbox del panel de detalle. Si no tiene exactamente 5 partes, se vuelca
        /// el valor completo en el 5to campo (Reparticion) para no perder informacion.
        /// </summary>
        private void CargarExpedienteEnCampos(string? dsExpediente)
        {
            if (string.IsNullOrWhiteSpace(dsExpediente))
            {
                txtExpedienteEx.Text = string.Empty;
                txtExpedienteAnio.Text = string.Empty;
                txtExpedienteNumero.Text = string.Empty;
                txtExpedienteGcaba.Text = string.Empty;
                txtExpedienteReparticion.Text = string.Empty;
                return;
            }

            string[] partes = dsExpediente.Split('-', StringSplitOptions.None);

            if (partes.Length == 5)
            {
                txtExpedienteEx.Text = partes[0].Trim();
                txtExpedienteAnio.Text = partes[1].Trim();
                txtExpedienteNumero.Text = partes[2].Trim();
                txtExpedienteGcaba.Text = partes[3].Trim();
                txtExpedienteReparticion.Text = partes[4].Trim();
            }
            else
            {
                txtExpedienteEx.Text = string.Empty;
                txtExpedienteAnio.Text = string.Empty;
                txtExpedienteNumero.Text = string.Empty;
                txtExpedienteGcaba.Text = string.Empty;
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

            string ex = txtExpedienteEx.Text.Trim();
            string anio = txtExpedienteAnio.Text.Trim();
            string numero = txtExpedienteNumero.Text.Trim();
            string gcaba = txtExpedienteGcaba.Text.Trim();
            string reparticion = txtExpedienteReparticion.Text.Trim();

            if (string.IsNullOrEmpty(ex) && string.IsNullOrEmpty(anio) && string.IsNullOrEmpty(numero)
                && string.IsNullOrEmpty(gcaba) && string.IsNullOrEmpty(reparticion))
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

        private void CargarImagen(string rutaImagen)
        {
            LimpiarImagen();

            if (string.IsNullOrWhiteSpace(rutaImagen) || !File.Exists(rutaImagen))
            {
                return;
            }

            try
            {
                using var stream = new MemoryStream(File.ReadAllBytes(rutaImagen));
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
        /// </summary>
        private void pictureBoxImagen_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || pictureBoxImagen.Image == null)
                return;

            _arrastrandoImagen = true;
            _puntoInicialArrastre = e.Location;
            pictureBoxImagen.Cursor = Cursors.SizeAll;
        }

        private void pictureBoxImagen_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!_arrastrandoImagen)
                return;

            int deltaX = e.Location.X - _puntoInicialArrastre.X;
            int deltaY = e.Location.Y - _puntoInicialArrastre.Y;

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

            AbrirPdfConAplicacionPredeterminada(_filaSeleccionada.Archivo.DsRutaCompleta);
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
                RegistrarCorreccionesSiCorresponde(resultado, cdUsuario);

                var resultadoIADAL = new ResultadoIADAL();
                resultadoIADAL.ActualizarDatos(resultado, cdUsuario);
                resultadoIADAL.ActualizarEstadoControl(
                    resultado.CdResultado,
                    ResultadoIA.EstadosControl.Controlado,
                    modificoDatos ? "SI" : "NO",
                    cdUsuario);

                MessageBox.Show("Registro guardado y marcado como Controlado.", "Guardar y Marcar como controlada",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            MarcarEstadoControl(ResultadoIA.EstadosControl.PaginaIlegible, "Marcar Página como ILEGIBLE");
        }

        private void btnMarcarDatosIlegible_Click(object sender, EventArgs e)
        {
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
                int cdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0;
                var resultadoIADAL = new ResultadoIADAL();
                resultadoIADAL.ActualizarEstadoControl(_filaSeleccionada.Resultado.CdResultado, cdEstadoControl, "NO", cdUsuario);

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
                var loteDAL = new LoteDAL();
                var resultadoIADAL = new ResultadoIADAL();
                var loteFinalizacionBL = new LoteFinalizacionBL();

                var resultados = resultadoIADAL.ObtenerPorLote(_cdLote, mostrarTodos: true);
                var archivosPorPagina = loteDAL.ObtenerArchivosPaginasPorLote(_cdLote)
                    .ToDictionary(a => a.CdArchivoPagina, a => a);

                var filas = resultados
                    .Where(r => archivosPorPagina.ContainsKey(r.CdArchivoPagina))
                    .Select(r => new LoteFinalizacionBL.FilaFinalizacion
                    {
                        Archivo = archivosPorPagina[r.CdArchivoPagina],
                        Resultado = r
                    }).ToList();

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

                int cdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0;
                loteFinalizacionBL.MarcarLotePendienteFinalizar(_cdLote, cdUsuario);

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
