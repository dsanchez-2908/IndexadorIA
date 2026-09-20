using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;
using IndexadorIA.Negocio.Api;
using IndexadorIA.Utilidades;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    /// <summary>
    /// Pantalla de detalle de un registro de auditoría (TD_001_RESULTADO_IA), abierta desde
    /// FrmAuditarLote al presionar "Mostrar" o hacer doble clic sobre un registro.
    /// Permite revisar/corregir los datos del registro y visualizar la imagen (zoom, giro, PDF).
    /// </summary>
    public partial class FrmVerRegistroAuditoria : Form
    {
        private readonly RegistroAuditoria _registro;
        private List<CategoriaPlano> _categoriasPlano = new();
        private List<TipoPlano> _tiposPlano = new();
        private List<Reparticion> _reparticiones = new();
        private ArchivoPagina? _archivoPagina;
        private float _factorZoom = 1.0f;
        private ResultadoIA? _valoresOriginalesResultado;
        private readonly ApiClienteServicio? _apiCliente;

        public FrmVerRegistroAuditoria(RegistroAuditoria registro)
        {
            InitializeComponent();
            _registro = registro;
            _apiCliente = SesionApi.ModoRemoto ? new ApiClienteServicio() : null;
        }

        private void FrmVerRegistroAuditoria_Load(object sender, EventArgs e)
        {
            try
            {
                Text = $"Ver Registro - {_registro.DsNombreLote}";

                CargarCategoriasYTiposPlano();
                CargarDatosRegistro();
                CargarArchivoPagina();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            cboCategoriaPlano.DisplayMember = "DsCategoriaPlano";
            cboCategoriaPlano.ValueMember = "CdCategoriaPlano";
            cboCategoriaPlano.DataSource = new List<CategoriaPlano>
            {
                new CategoriaPlano { CdCategoriaPlano = 0, DsCategoriaPlano = "(Vacío)" }
            }.Concat(_categoriasPlano).ToList();

            List<KeyValuePair<int, string>> estadosControl;
            if (SesionApi.ModoRemoto)
            {
                estadosControl = _apiCliente!.ObtenerEstadosControlAuditoriaAsync().GetAwaiter().GetResult()
                    .Select(e => new KeyValuePair<int, string>(e.CdEstado, e.DsEstado))
                    .ToList();
            }
            else
            {
                var resultadoDAL = new ResultadoIADAL();
                estadosControl = resultadoDAL.ObtenerEstadosControlParaFiltro();
            }

            cboEstado.DisplayMember = "Value";
            cboEstado.ValueMember = "Key";
            cboEstado.DataSource = estadosControl;

            var listaReparticiones = _reparticiones.Select(r => r.DsReparticion).ToList();
            cboExpedienteReparticion.DataSource = listaReparticiones;

            var autoCompleteReparticiones = new AutoCompleteStringCollection();
            autoCompleteReparticiones.AddRange(listaReparticiones.ToArray());
            cboExpedienteReparticion.AutoCompleteCustomSource = autoCompleteReparticiones;
        }

        private void ActualizarTiposPlano(int? cdCategoriaPlano)
        {
            var tiposFiltrados = cdCategoriaPlano.HasValue
                ? _tiposPlano.Where(t => t.CdCategoriaPlano == cdCategoriaPlano.Value).ToList()
                : _tiposPlano.ToList();

            cboTipoPlano.DisplayMember = "DsTipoPlano";
            cboTipoPlano.ValueMember = "CdTipoPlano";
            cboTipoPlano.DataSource = new List<TipoPlano>
            {
                new TipoPlano { CdTipoPlano = 0, DsTipoPlano = "(Vacío)" }
            }.Concat(tiposFiltrados).ToList();
        }

        private void cboCategoriaPlano_SelectedIndexChanged(object sender, EventArgs e)
        {
            int? cdCategoriaPlano = cboCategoriaPlano.SelectedValue is int cdCategoria && cdCategoria != 0
                ? cdCategoria
                : null;

            ActualizarTiposPlano(cdCategoriaPlano);
        }

        private void CargarDatosRegistro()
        {
            _valoresOriginalesResultado = new ResultadoIA
            {
                CdResultado = _registro.CdResultado,
                CdCategoriaPlano = _registro.CdCategoriaPlano,
                CdTipoPlano = _registro.CdTipoPlano,
                DsNumeroPlano = _registro.DsNumeroPlano,
                DsExpediente = _registro.DsExpediente,
                DsSeccion = _registro.DsSeccion,
                DsManzana = _registro.DsManzana,
                DsParcela = _registro.DsParcela,
                DsDireccion = _registro.DsDireccion,
                DsObservaciones = _registro.DsObservaciones
            };

            cboCategoriaPlano.SelectedValue = _registro.CdCategoriaPlano ?? 0;
            ActualizarTiposPlano(_registro.CdCategoriaPlano);
            cboTipoPlano.SelectedValue = _registro.CdTipoPlano ?? 0;
            CargarExpedienteEnCampos(_registro.DsExpediente);
            txtSeccion.Text = _registro.DsSeccion ?? string.Empty;
            txtManzana.Text = _registro.DsManzana ?? string.Empty;
            txtParcela.Text = _registro.DsParcela ?? string.Empty;
            txtDireccion.Text = _registro.DsDireccion ?? string.Empty;
            CargarNumeroPlanoEnCampos(_registro.DsNumeroPlano);
            txtObservaciones.Text = _registro.DsObservaciones ?? string.Empty;
            cboEstado.SelectedValue = _registro.CdEstadoControl;

            lblNombreArchivoOriginal.Text = $"Archivo original: {_registro.DsNombreArchivoOriginal ?? "-"}";
        }

        /// <summary>
        /// Distribuye el expediente (formato EX-ANIO-NUMERO-GCABA-REPARTICION) en los
        /// campos segmentados. Si no tiene exactamente 5 partes, se deja todo vacío.
        /// </summary>
        private void CargarExpedienteEnCampos(string? dsExpediente)
        {
            txtExpedienteEx.Text = "EX";
            txtExpedienteGcaba.Text = "GCABA";

            if (string.IsNullOrWhiteSpace(dsExpediente))
            {
                txtExpedienteAnio.Text = string.Empty;
                txtExpedienteNumero.Text = string.Empty;
                cboExpedienteReparticion.Text = string.Empty;
                return;
            }

            string[] partes = dsExpediente.Split('-', StringSplitOptions.None);

            if (partes.Length == 5)
            {
                txtExpedienteAnio.Text = partes[1].Trim();
                txtExpedienteNumero.Text = partes[2].Trim();
                cboExpedienteReparticion.Text = partes[4].Trim();
            }
            else
            {
                txtExpedienteAnio.Text = string.Empty;
                txtExpedienteNumero.Text = string.Empty;
                cboExpedienteReparticion.Text = dsExpediente.Trim();
            }
        }

        /// <summary>
        /// Reconstruye el expediente concatenando los campos con guiones, completando
        /// el número con ceros a la izquierda hasta 8 dígitos. Si todos los campos
        /// están vacíos, devuelve null.
        /// </summary>
        private string? ArmarExpedienteDesdeCampos()
        {
            const string ex = "EX";
            string anio = txtExpedienteAnio.Text.Trim();
            string numero = txtExpedienteNumero.Text.Trim();
            const string gcaba = "GCABA";
            string reparticion = cboExpedienteReparticion.Text.Trim();

            if (string.IsNullOrEmpty(anio) && string.IsNullOrEmpty(numero) && string.IsNullOrEmpty(reparticion))
            {
                return null;
            }

            if (int.TryParse(numero, out int numeroExpediente))
            {
                numero = numeroExpediente.ToString("D8");
            }

            return $"{ex}-{anio}-{numero}-{gcaba}-{reparticion}";
        }

        /// <summary>
        /// Parsea el expediente desde el nombre del archivo original (mismo formato y
        /// lógica que FrmVerLote.btnParsearExpedienteDeArchivo_Click): EX-ANIO-NUMERO-...-REPARTICION.
        /// </summary>
        private void btnParsearExpedienteDeArchivo_Click(object sender, EventArgs e)
        {
            string? nombreArchivoOriginal = _registro.DsNombreArchivoOriginal;
            if (string.IsNullOrWhiteSpace(nombreArchivoOriginal))
                return;

            string nombreSinExtension = Path.GetFileNameWithoutExtension(nombreArchivoOriginal);
            string[] partes = nombreSinExtension.Split('-', StringSplitOptions.None);

            if (partes.Length < 6)
                return;

            string ex = partes[0].Trim();
            string anio = partes[1].Trim();
            string numero = partes[2].Trim();
            string reparticion = partes[5].Trim();

            if (string.IsNullOrWhiteSpace(ex))
                return;

            if (!int.TryParse(anio, out int anioValor) || anioValor < 1900 || anioValor > 2050)
                return;

            if (numero.Length != 8 || !numero.All(char.IsDigit))
                return;

            if (string.IsNullOrWhiteSpace(reparticion))
                return;

            txtExpedienteEx.Text = ex;
            txtExpedienteAnio.Text = anio;
            txtExpedienteNumero.Text = numero;
            cboExpedienteReparticion.Text = reparticion;
        }

        /// <summary>
        /// Distribuye el número de plano (formato PREFIJO-NUMERO-ANIO) en los
        /// campos segmentados. Si no tiene exactamente 3 partes, deja el prefijo vacío
        /// y vuelca el valor completo en número.
        /// </summary>
        private void CargarNumeroPlanoEnCampos(string? dsNumeroPlano)
        {
            if (string.IsNullOrWhiteSpace(dsNumeroPlano))
            {
                txtNumeroPlanoPrefijo.Text = string.Empty;
                txtNumeroPlanoNumero.Text = string.Empty;
                txtNumeroPlanoAnio.Text = string.Empty;
                return;
            }

            string[] partes = dsNumeroPlano.Split('-', StringSplitOptions.None);

            if (partes.Length == 3)
            {
                txtNumeroPlanoPrefijo.Text = partes[0].Trim();
                txtNumeroPlanoNumero.Text = partes[1].Trim();
                txtNumeroPlanoAnio.Text = partes[2].Trim();
            }
            else
            {
                txtNumeroPlanoPrefijo.Text = string.Empty;
                txtNumeroPlanoNumero.Text = dsNumeroPlano.Trim();
                txtNumeroPlanoAnio.Text = string.Empty;
            }
        }

        /// <summary>
        /// Reconstruye el número de plano concatenando los campos con guiones, completando
        /// el número con ceros a la izquierda hasta 4 dígitos. Si todos los campos
        /// están vacíos, devuelve null.
        /// </summary>
        private string? ArmarNumeroPlanoDesdeCampos()
        {
            string prefijo = txtNumeroPlanoPrefijo.Text.Trim();
            string numero = txtNumeroPlanoNumero.Text.Trim();
            string anio = txtNumeroPlanoAnio.Text.Trim();

            if (string.IsNullOrEmpty(prefijo) && string.IsNullOrEmpty(numero) && string.IsNullOrEmpty(anio))
            {
                return null;
            }

            if (int.TryParse(numero, out int numeroPlano))
            {
                numero = numeroPlano.ToString("D4");
            }

            return $"{prefijo}-{numero}-{anio}";
        }

        private void CargarArchivoPagina()
        {
            if (SesionApi.ModoRemoto)
            {
                try
                {
                    byte[] bytes = _apiCliente!.ObtenerJpgAsync(_registro.CdLote, _registro.CdArchivoPagina)
                        .GetAwaiter().GetResult();
                    lblSnGirada.Text = string.Empty;
                    CargarImagenDesdeBytes(bytes);
                }
                catch (Exception ex)
                {
                    LimpiarImagen();
                    MessageBox.Show($"No se pudo cargar la imagen desde la API: {ex.Message}", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            var archivoPaginaDAL = new ArchivoPaginaDAL();
            _archivoPagina = archivoPaginaDAL.ObtenerPorId(_registro.CdArchivoPagina);

            if (_archivoPagina == null)
            {
                return;
            }

            lblSnGirada.Text = $"Girada: {_archivoPagina.SnGirada}";
            CargarImagen(ObtenerRutaImagenJPG(_archivoPagina.DsRutaCompleta));
        }

        private static string ObtenerRutaImagenJPG(string rutaCompleta)
        {
            return string.IsNullOrWhiteSpace(rutaCompleta)
                ? string.Empty
                : Path.ChangeExtension(rutaCompleta, ".jpg");
        }

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

        private void btnGirarIzquierda_Click(object sender, EventArgs e)
        {
            RotarPaginaActual(-90);
        }

        private void btnGirarDerecha_Click(object sender, EventArgs e)
        {
            RotarPaginaActual(90);
        }

        /// <summary>
        /// Rota tanto el recorte JPG como el PDF original de la página, actualiza la vista
        /// y deja registro de la rotación manual en la base de datos.
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

            if (_archivoPagina == null)
            {
                MessageBox.Show("No se pudo determinar el archivo/página del registro.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rutaPdf = _archivoPagina.DsRutaCompleta;
            string rutaJpg = ObtenerRutaImagenJPG(rutaPdf);

            try
            {
                RotarArchivoPDF(rutaPdf, grados);
                RotarArchivoJPG(rutaJpg, grados);

                int gradosNormalizados = ((grados % 360) + 360) % 360;

                var rotacionDAL = new RotacionDAL();
                rotacionDAL.Insertar(new Rotacion
                {
                    CdArchivoPagina = _archivoPagina.CdArchivoPagina,
                    SnRotacionAutomatica = "NO",
                    NuRotacionAplicada = gradosNormalizados,
                    SnRotacionManual = "SI",
                    CdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0
                });

                var archivoPaginaDAL = new ArchivoPaginaDAL();
                archivoPaginaDAL.ActualizarSnGirada(_archivoPagina.CdArchivoPagina, "SI");
                _archivoPagina.SnGirada = "SI";
                lblSnGirada.Text = $"Girada: {_archivoPagina.SnGirada}";

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

        private void btnVerImagenPDF_Click(object sender, EventArgs e)
        {
            if (SesionApi.ModoRemoto)
            {
                AbrirPdfRemoto(_registro.CdLote, _registro.CdArchivoPagina);
                return;
            }

            if (_archivoPagina == null)
            {
                MessageBox.Show("No se pudo determinar el archivo/página del registro.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AbrirPdfConAplicacionPredeterminada(_archivoPagina.DsRutaCompleta);
        }

        /// <summary>
        /// Descarga el PDF de la página del registro desde la API y lo abre con la
        /// aplicación predeterminada de Windows, ya que el cliente no tiene acceso
        /// directo al storage del servidor en modo remoto.
        /// </summary>
        private void AbrirPdfRemoto(int cdLote, int cdArchivoPagina)
        {
            try
            {
                byte[] bytes = _apiCliente!.ObtenerPdfAsync(cdLote, cdArchivoPagina)
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

        #endregion

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtExpedienteAnio.Text)
                && string.IsNullOrWhiteSpace(txtExpedienteNumero.Text)
                && string.IsNullOrWhiteSpace(cboExpedienteReparticion.Text))
            {
                var confirmacionExpediente = MessageBox.Show(
                    "El numero de expediente no esta completo, desea continuar?",
                    "Expediente incompleto", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmacionExpediente != DialogResult.Yes)
                    return;
            }

            if (!string.IsNullOrWhiteSpace(txtSeccion.Text) && txtSeccion.Text.Trim().Length < 3)
            {
                MessageBox.Show(
                    "El campo Sección debe tener 3 o más caracteres.",
                    "Sección inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtManzana.Text) && txtManzana.Text.Trim().Length < 3)
            {
                MessageBox.Show(
                    "El campo Manzana debe tener 3 o más caracteres.",
                    "Manzana inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtParcela.Text) && txtParcela.Text.Trim().Length < 3)
            {
                MessageBox.Show(
                    "El campo Parcela debe tener 3 o más caracteres.",
                    "Parcela inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtNumeroPlanoNumero.Text) && txtNumeroPlanoNumero.Text.Trim().Length != 4)
            {
                MessageBox.Show(
                    "El campo Número de Plano (número) debe tener 4 caracteres.",
                    "Número de Plano inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtNumeroPlanoAnio.Text))
            {
                string anioPlano = txtNumeroPlanoAnio.Text.Trim();
                if (anioPlano.Length != 4 || !int.TryParse(anioPlano, out int anioPlanoValor) || anioPlanoValor < 1900 || anioPlanoValor > 2030)
                {
                    MessageBox.Show(
                        "El campo Número de Plano (año) debe tener 4 dígitos y ser un año entre 1900 y 2030.",
                        "Número de Plano inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            int? cdEstadoControlSeleccionado = cboEstado.SelectedValue is int cdEstadoControlVal ? cdEstadoControlVal : null;

            if (cdEstadoControlSeleccionado == ResultadoIA.EstadosControl.CasosEspeciales
                && !string.IsNullOrWhiteSpace(txtParcela.Text) && txtParcela.Text.Trim().Length <= 3)
            {
                MessageBox.Show(
                    "Si la parcela tiene 3 caracteres o menos, se debe guardar como normal (Controlado).",
                    "Parcela inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cdEstadoControlSeleccionado == ResultadoIA.EstadosControl.Controlado
                && !string.IsNullOrWhiteSpace(txtParcela.Text) && txtParcela.Text.Trim().Length != 3)
            {
                MessageBox.Show(
                    "Si la parcela tiene letra o mas de una parcela, se debe guardar como casos especiales.",
                    "Parcela inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtDireccion.Text) && ContieneNumeroDeMasDeCuatroDigitos(txtDireccion.Text))
            {
                var confirmacionDireccion = MessageBox.Show(
                    "Revise el campo Dirección. Esta seguro de continuar?",
                    "Verifique la Dirección", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmacionDireccion != DialogResult.Yes)
                    return;
            }

            try
            {
                var resultado = new ResultadoIA
                {
                    CdResultado = _registro.CdResultado,
                    CdCategoriaPlano = cboCategoriaPlano.SelectedValue is int cdCategoriaPlano && cdCategoriaPlano != 0 ? cdCategoriaPlano : null,
                    CdTipoPlano = cboTipoPlano.SelectedValue is int cdTipoPlano && cdTipoPlano != 0 ? cdTipoPlano : null,
                    DsExpediente = ArmarExpedienteDesdeCampos(),
                    DsSeccion = string.IsNullOrWhiteSpace(txtSeccion.Text) ? null : txtSeccion.Text.Trim(),
                    DsManzana = string.IsNullOrWhiteSpace(txtManzana.Text) ? null : txtManzana.Text.Trim(),
                    DsParcela = string.IsNullOrWhiteSpace(txtParcela.Text) ? null : txtParcela.Text.Trim(),
                    DsDireccion = string.IsNullOrWhiteSpace(txtDireccion.Text) ? null : txtDireccion.Text.Trim(),
                    DsNumeroPlano = ArmarNumeroPlanoDesdeCampos()
                };

                int cdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0;

                bool cambioEstado = cboEstado.SelectedValue is int cdEstadoControlSel && cdEstadoControlSel != _registro.CdEstadoControl;
                int cdEstadoControlFinal = cboEstado.SelectedValue is int cdEstadoControlSel2 ? cdEstadoControlSel2 : _registro.CdEstadoControl;
                string? dsObservaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim();
                bool cambioObservaciones = !string.Equals(txtObservaciones.Text.Trim(), _registro.DsObservaciones ?? string.Empty, StringComparison.Ordinal);
                bool cambioDireccion = !string.Equals(resultado.DsDireccion ?? string.Empty, _valoresOriginalesResultado?.DsDireccion ?? string.Empty, StringComparison.Ordinal);

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

                    _apiCliente!.ActualizarDatosResultadoAuditoriaAsync(resultado.CdResultado, requestDatos)
                        .GetAwaiter().GetResult();

                    if (cambioEstado || cambioObservaciones)
                    {
                        _apiCliente!.ActualizarEstadoControlResultadoAuditoriaAsync(resultado.CdResultado, new ActualizarEstadoControlApiRequestDto
                        {
                            CdEstadoControl = cdEstadoControlFinal,
                            SnModificaDatos = null,
                            DsObservaciones = dsObservaciones
                        }).GetAwaiter().GetResult();
                    }

                    var correccionesApi = ObtenerCorreccionesDeAuditoriaSiCorresponde(resultado);
                    if (correccionesApi.Count > 0)
                    {
                        _apiCliente!.RegistrarCorreccionesAuditoriaAsync(resultado.CdResultado, new RegistrarCorreccionesApiRequestDto
                        {
                            Correcciones = correccionesApi
                        }).GetAwaiter().GetResult();
                    }

                    if (cambioDireccion && !string.IsNullOrWhiteSpace(resultado.DsExpediente))
                    {
                        _apiCliente!.PropagarDireccionAuditoriaAsync(resultado.CdResultado, new PropagarDireccionApiRequestDto
                        {
                            CdLote = _registro.CdLote,
                            DsExpediente = resultado.DsExpediente,
                            DsDireccion = resultado.DsDireccion
                        }).GetAwaiter().GetResult();
                    }
                }
                else
                {
                    var resultadoDAL = new ResultadoIADAL();
                    resultadoDAL.ActualizarDatos(resultado, cdUsuario);

                    RegistrarCorreccionesDeAuditoriaSiCorresponde(resultado, cdUsuario);

                    if (cambioEstado || cambioObservaciones)
                    {
                        resultadoDAL.ActualizarEstadoControl(_registro.CdResultado, cdEstadoControlFinal, null, cdUsuario, dsObservaciones);
                    }

                    if (cambioDireccion && !string.IsNullOrWhiteSpace(resultado.DsExpediente))
                    {
                        resultadoDAL.ActualizarDireccionPorExpedienteEnLote(
                            _registro.CdLote, resultado.DsExpediente, resultado.CdResultado, resultado.DsDireccion, cdUsuario);
                    }
                }

                MessageBox.Show("Los cambios fueron guardados correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar los cambios: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Compara los valores originales del registro contra los actualizados por el auditor
        /// y, si hubo diferencias, deja constancia en TD_CORRECIONES con los datos de auditoría
        /// (feAuditoria/cdUsuarioAuditoria).
        /// </summary>
        private void RegistrarCorreccionesDeAuditoriaSiCorresponde(ResultadoIA resultadoActualizado, int cdUsuario)
        {
            if (_valoresOriginalesResultado == null)
                return;

            var correccionDAL = new CorreccionDAL();
            foreach (var c in ObtenerCorreccionesDeAuditoriaSiCorresponde(resultadoActualizado))
            {
                correccionDAL.Insertar(new Correccion
                {
                    CdResultado = resultadoActualizado.CdResultado,
                    DsCampo = c.DsCampo,
                    DsValorAnterior = c.DsValorAnterior,
                    DsValorNuevo = c.DsValorNuevo,
                    CdUsuarioAuditoria = cdUsuario
                });
            }
        }

        /// <summary>
        /// Compara los valores originales del registro contra los actualizados por el auditor
        /// y arma la lista de campos modificados, usada tanto en modo local (inserción directa
        /// en TD_CORRECIONES) como en modo remoto (envío al endpoint de la API).
        /// </summary>
        private List<CorreccionApiRequestDto> ObtenerCorreccionesDeAuditoriaSiCorresponde(ResultadoIA resultadoActualizado)
        {
            var correcciones = new List<CorreccionApiRequestDto>();

            if (_valoresOriginalesResultado == null)
                return correcciones;

            var original = _valoresOriginalesResultado;

            void AgregarSiDistinto(string dsCampo, string? valorAnterior, string? valorNuevo)
            {
                if (valorAnterior == valorNuevo)
                    return;

                correcciones.Add(new CorreccionApiRequestDto
                {
                    DsCampo = dsCampo,
                    DsValorAnterior = valorAnterior,
                    DsValorNuevo = valorNuevo
                });
            }

            AgregarSiDistinto(Correccion.Campos.Categoria,
                ObtenerDescripcionCategoria(original.CdCategoriaPlano),
                ObtenerDescripcionCategoria(resultadoActualizado.CdCategoriaPlano));

            AgregarSiDistinto(Correccion.Campos.TipoPlano,
                ObtenerDescripcionTipoPlano(original.CdTipoPlano),
                ObtenerDescripcionTipoPlano(resultadoActualizado.CdTipoPlano));

            AgregarSiDistinto(Correccion.Campos.Direccion, original.DsDireccion, resultadoActualizado.DsDireccion);
            AgregarSiDistinto(Correccion.Campos.Seccion, original.DsSeccion, resultadoActualizado.DsSeccion);
            AgregarSiDistinto(Correccion.Campos.Manzana, original.DsManzana, resultadoActualizado.DsManzana);
            AgregarSiDistinto(Correccion.Campos.Parcela, original.DsParcela, resultadoActualizado.DsParcela);
            AgregarSiDistinto(Correccion.Campos.Expediente, original.DsExpediente, resultadoActualizado.DsExpediente);
            AgregarSiDistinto(Correccion.Campos.NumeroPlano, original.DsNumeroPlano, resultadoActualizado.DsNumeroPlano);

            return correcciones;
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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Recorre el texto buscando secuencias de dígitos consecutivos (números) y determina
        /// si alguna de ellas tiene más de 4 dígitos. Se usa para advertir sobre posibles
        /// direcciones mal escaneadas/tipeadas (por ejemplo "12345" en vez de "123.45").
        /// </summary>
        private static bool ContieneNumeroDeMasDeCuatroDigitos(string valor)
        {
            int longitudActual = 0;

            foreach (char c in valor)
            {
                if (char.IsDigit(c))
                {
                    longitudActual++;
                    if (longitudActual > 4)
                        return true;
                }
                else
                {
                    longitudActual = 0;
                }
            }

            return false;
        }
    }
}
