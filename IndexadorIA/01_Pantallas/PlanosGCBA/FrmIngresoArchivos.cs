using IndexadorIA.Entidades;
using IndexadorIA.Negocio;
using IndexadorIA.Utilidades;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    public partial class FrmIngresoArchivos : Form
    {
        private readonly ProyectoBL _proyectoBL;
        private readonly ArchivoOriginalBL _archivoBL;
        private List<ProcesamientoArchivos.ResultadoArchivo> _archivosEncontrados;
        private int _idEstadoPendiente;

        public FrmIngresoArchivos()
        {
            InitializeComponent();
            _proyectoBL = new ProyectoBL();
            _archivoBL = new ArchivoOriginalBL();
            _archivosEncontrados = new List<ProcesamientoArchivos.ResultadoArchivo>();
        }

        private void FrmIngresoArchivos_Load(object sender, EventArgs e)
        {
            CargarProyectos();
            ObtenerEstadoPendiente();
            chkIncluirSubcarpetas.Checked = true;
        }

        private void CargarProyectos()
        {
            try
            {
                var proyectos = _proyectoBL.ObtenerActivos();

                cboProyecto.DataSource = proyectos;
                cboProyecto.DisplayMember = "DsProyecto";
                cboProyecto.ValueMember = "CdProyecto";
                cboProyecto.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar proyectos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ObtenerEstadoPendiente()
        {
            try
            {
                // El estado "Pendiente de Procesar" para ARCHIVO_ORIGINAL debe tener cdEstado = 1
                // Necesitamos obtenerlo de la base de datos
                using (var conexion = new Microsoft.Data.SqlClient.SqlConnection(Datos.Configuracion.CadenaConexion))
                {
                    var comando = new Microsoft.Data.SqlClient.SqlCommand(
                        "SELECT cdEstado FROM TD_ESTADOS WHERE dsProceso = 'ARCHIVO_ORIGINAL' AND cdEstado = 1", conexion);

                    conexion.Open();
                    var resultado = comando.ExecuteScalar();
                    _idEstadoPendiente = resultado != null ? Convert.ToInt32(resultado) : 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener estado: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSeleccionarCarpeta_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Seleccione la carpeta de origen";
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtRutaOrigen.Text = dialog.SelectedPath;
                }
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!ValidarBusqueda())
            {
                return;
            }

            try
            {
                btnBuscar.Enabled = false;
                btnIngresar.Enabled = false;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;
                lblProgreso.Visible = true;
                lblProgreso.Text = "Buscando archivos...";

                var progreso = new Progress<string>(mensaje =>
                {
                    lblProgreso.Text = mensaje;
                });

                _archivosEncontrados = await Task.Run(() =>
                    ProcesamientoArchivos.BuscarArchivos(
                        txtRutaOrigen.Text,
                        chkIncluirSubcarpetas.Checked,
                        progreso));

                MostrarResultados();

                lblProgreso.Text = $"Búsqueda completada. {_archivosEncontrados.Count} archivos encontrados.";
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar archivos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnBuscar.Enabled = true;
                btnIngresar.Enabled = _archivosEncontrados.Count > 0;
            }
        }

        private bool ValidarBusqueda()
        {
            if (cboProyecto.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un proyecto.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboProyecto.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtRutaOrigen.Text))
            {
                MessageBox.Show("Debe seleccionar una carpeta de origen.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnSeleccionarCarpeta.Focus();
                return false;
            }

            if (!Directory.Exists(txtRutaOrigen.Text))
            {
                MessageBox.Show("La carpeta seleccionada no existe.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void MostrarResultados()
        {
            dgvArchivos.DataSource = null;
            dgvArchivos.DataSource = _archivosEncontrados;

            // Ocultar columnas innecesarias
            if (dgvArchivos.Columns["Error"] != null)
                dgvArchivos.Columns["Error"].Visible = false;

            // Configurar columnas
            if (dgvArchivos.Columns["NombreArchivo"] != null)
            {
                dgvArchivos.Columns["NombreArchivo"].HeaderText = "Nombre de Archivo";
                dgvArchivos.Columns["NombreArchivo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvArchivos.Columns["Extension"] != null)
            {
                dgvArchivos.Columns["Extension"].HeaderText = "Extensión";
                dgvArchivos.Columns["Extension"].Width = 100;
            }

            if (dgvArchivos.Columns["RutaCompleta"] != null)
            {
                dgvArchivos.Columns["RutaCompleta"].HeaderText = "Ruta";
                dgvArchivos.Columns["RutaCompleta"].Width = 300;
            }

            if (dgvArchivos.Columns["NombreUltimaCarpeta"] != null)
            {
                dgvArchivos.Columns["NombreUltimaCarpeta"].HeaderText = "Carpeta";
                dgvArchivos.Columns["NombreUltimaCarpeta"].Width = 150;
            }

            if (dgvArchivos.Columns["CantidadPaginas"] != null)
            {
                dgvArchivos.Columns["CantidadPaginas"].HeaderText = "Páginas";
                dgvArchivos.Columns["CantidadPaginas"].Width = 80;
            }

            if (dgvArchivos.Columns["TamanoBytes"] != null)
            {
                dgvArchivos.Columns["TamanoBytes"].HeaderText = "Tamaño (bytes)";
                dgvArchivos.Columns["TamanoBytes"].Width = 120;
            }

            if (dgvArchivos.Columns["FeModificacion"] != null)
            {
                dgvArchivos.Columns["FeModificacion"].HeaderText = "Fecha Modificación";
                dgvArchivos.Columns["FeModificacion"].Width = 150;
                dgvArchivos.Columns["FeModificacion"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
        }

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            if (_archivosEncontrados.Count == 0)
            {
                MessageBox.Show("No hay archivos para ingresar.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Está seguro de ingresar {_archivosEncontrados.Count} archivos?",
                "Confirmar ingreso",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            try
            {
                btnBuscar.Enabled = false;
                btnIngresar.Enabled = false;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Maximum = _archivosEncontrados.Count;
                progressBar.Value = 0;
                lblProgreso.Visible = true;

                int cdProyecto = (int)cboProyecto.SelectedValue;
                int cdUsuario = SesionActual.UsuarioActual.CdUsuario;

                var archivosParaInsertar = new List<ArchivoOriginal>();

                for (int i = 0; i < _archivosEncontrados.Count; i++)
                {
                    var archivoResultado = _archivosEncontrados[i];

                    var archivo = ProcesamientoArchivos.ConvertirAArchivoOriginal(
                        archivoResultado,
                        cdProyecto,
                        cdUsuario,
                        _idEstadoPendiente);

                    archivosParaInsertar.Add(archivo);

                    progressBar.Value = i + 1;
                    lblProgreso.Text = $"Preparando archivo {i + 1}/{_archivosEncontrados.Count}...";
                    await Task.Delay(1); // Para que la UI se actualice
                }

                lblProgreso.Text = "Guardando en base de datos...";

                var resultadoGuardado = await Task.Run(() => _archivoBL.GuardarLote(archivosParaInsertar));

                if (resultadoGuardado.exito)
                {
                    MessageBox.Show(resultadoGuardado.mensaje, "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Limpiar formulario
                    _archivosEncontrados.Clear();
                    dgvArchivos.DataSource = null;
                    txtRutaOrigen.Clear();
                    progressBar.Value = 0;
                    lblProgreso.Text = "";
                    progressBar.Visible = false;
                    lblProgreso.Visible = false;
                }
                else
                {
                    MessageBox.Show(resultadoGuardado.mensaje, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al ingresar archivos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnBuscar.Enabled = true;
                btnIngresar.Enabled = false;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
