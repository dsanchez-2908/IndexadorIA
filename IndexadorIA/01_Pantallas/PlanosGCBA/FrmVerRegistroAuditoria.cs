using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;
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
        private ArchivoPagina? _archivoPagina;
        private float _factorZoom = 1.0f;

        public FrmVerRegistroAuditoria(RegistroAuditoria registro)
        {
            InitializeComponent();
            _registro = registro;
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
            var categoriaPlanoDAL = new CategoriaPlanoDAL();
            var tipoPlanoDAL = new TipoPlanoDAL();
            _categoriasPlano = categoriaPlanoDAL.ObtenerTodos();
            _tiposPlano = tipoPlanoDAL.ObtenerTodos();

            cboCategoriaPlano.DisplayMember = "DsCategoriaPlano";
            cboCategoriaPlano.ValueMember = "CdCategoriaPlano";
            cboCategoriaPlano.DataSource = _categoriasPlano.ToList();

            var resultadoDAL = new ResultadoIADAL();
            cboEstado.DisplayMember = "Value";
            cboEstado.ValueMember = "Key";
            cboEstado.DataSource = resultadoDAL.ObtenerEstadosControlParaFiltro();
        }

        private void ActualizarTiposPlano(int? cdCategoriaPlano)
        {
            var tiposFiltrados = cdCategoriaPlano.HasValue
                ? _tiposPlano.Where(t => t.CdCategoriaPlano == cdCategoriaPlano.Value).ToList()
                : _tiposPlano.ToList();

            cboTipoPlano.DisplayMember = "DsTipoPlano";
            cboTipoPlano.ValueMember = "CdTipoPlano";
            cboTipoPlano.DataSource = tiposFiltrados;
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
            cboCategoriaPlano.SelectedValue = _registro.CdCategoriaPlano ?? 0;
            ActualizarTiposPlano(_registro.CdCategoriaPlano);
            cboTipoPlano.SelectedValue = _registro.CdTipoPlano ?? 0;
            txtExpediente.Text = _registro.DsExpediente ?? string.Empty;
            txtSeccion.Text = _registro.DsSeccion ?? string.Empty;
            txtManzana.Text = _registro.DsManzana ?? string.Empty;
            txtParcela.Text = _registro.DsParcela ?? string.Empty;
            txtDireccion.Text = _registro.DsDireccion ?? string.Empty;
            txtNumeroPlano.Text = _registro.DsNumeroPlano ?? string.Empty;
            txtObservaciones.Text = _registro.DsObservaciones ?? string.Empty;
            cboEstado.SelectedValue = _registro.CdEstadoControl;
        }

        private void CargarArchivoPagina()
        {
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
            if (_archivoPagina == null)
            {
                MessageBox.Show("No se pudo determinar el archivo/página del registro.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AbrirPdfConAplicacionPredeterminada(_archivoPagina.DsRutaCompleta);
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
            try
            {
                var resultado = new ResultadoIA
                {
                    CdResultado = _registro.CdResultado,
                    CdCategoriaPlano = cboCategoriaPlano.SelectedValue is int cdCategoriaPlano && cdCategoriaPlano != 0 ? cdCategoriaPlano : null,
                    CdTipoPlano = cboTipoPlano.SelectedValue is int cdTipoPlano && cdTipoPlano != 0 ? cdTipoPlano : null,
                    DsExpediente = string.IsNullOrWhiteSpace(txtExpediente.Text) ? null : txtExpediente.Text.Trim(),
                    DsSeccion = string.IsNullOrWhiteSpace(txtSeccion.Text) ? null : txtSeccion.Text.Trim(),
                    DsManzana = string.IsNullOrWhiteSpace(txtManzana.Text) ? null : txtManzana.Text.Trim(),
                    DsParcela = string.IsNullOrWhiteSpace(txtParcela.Text) ? null : txtParcela.Text.Trim(),
                    DsDireccion = string.IsNullOrWhiteSpace(txtDireccion.Text) ? null : txtDireccion.Text.Trim(),
                    DsNumeroPlano = string.IsNullOrWhiteSpace(txtNumeroPlano.Text) ? null : txtNumeroPlano.Text.Trim()
                };

                int cdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0;

                var resultadoDAL = new ResultadoIADAL();
                resultadoDAL.ActualizarDatos(resultado, cdUsuario);

                if (cboEstado.SelectedValue is int cdEstadoControl && cdEstadoControl != _registro.CdEstadoControl)
                {
                    string? dsObservaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim();
                    resultadoDAL.ActualizarEstadoControl(_registro.CdResultado, cdEstadoControl, null, cdUsuario, dsObservaciones);
                }
                else if (!string.Equals(txtObservaciones.Text.Trim(), _registro.DsObservaciones ?? string.Empty, StringComparison.Ordinal))
                {
                    string? dsObservaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim();
                    resultadoDAL.ActualizarEstadoControl(_registro.CdResultado, _registro.CdEstadoControl, null, cdUsuario, dsObservaciones);
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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
