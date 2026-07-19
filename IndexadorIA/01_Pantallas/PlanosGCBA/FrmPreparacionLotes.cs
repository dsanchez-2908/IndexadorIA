using IndexadorIA.Entidades;
using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    public partial class FrmPreparacionLotes : Form
    {
        private readonly LoteBL _loteBL;
        private List<ArchivoPaginaGridDto> _archivos;

        public FrmPreparacionLotes()
        {
            InitializeComponent();
            _loteBL = new LoteBL();
            _archivos = new List<ArchivoPaginaGridDto>();
        }

        private void FrmPreparacionLotes_Load(object sender, EventArgs e)
        {
            ConfigurarDataGridView();
            CargarDatos();
        }

        private void ConfigurarDataGridView()
        {
            dgvArchivos.AutoGenerateColumns = false;
            dgvArchivos.AllowUserToAddRows = false;
            dgvArchivos.AllowUserToDeleteRows = false;
            dgvArchivos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArchivos.MultiSelect = false;
            dgvArchivos.ReadOnly = false;

            // Estilos
            dgvArchivos.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgvArchivos.ForeColor = Color.White;
            dgvArchivos.GridColor = Color.FromArgb(60, 60, 63);
            dgvArchivos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvArchivos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvArchivos.EnableHeadersVisualStyles = false;
            dgvArchivos.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvArchivos.DefaultCellStyle.ForeColor = Color.White;
            dgvArchivos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvArchivos.DefaultCellStyle.SelectionForeColor = Color.White;

            // Columnas
            dgvArchivos.Columns.Clear();

            // Columna de selección (checkbox)
            var colSeleccion = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Sel",
                Name = "Seleccionado",
                DataPropertyName = "Seleccionado",
                Width = 50,
                ReadOnly = false
            };
            dgvArchivos.Columns.Add(colSeleccion);

            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "CdArchivoPagina",
                Name = "CdArchivoPagina",
                Width = 80,
                ReadOnly = true
            });

            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Fecha Alta",
                DataPropertyName = "FeAlta",
                Name = "FeAlta",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" },
                ReadOnly = true
            });

            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Estado",
                DataPropertyName = "DsEstado",
                Name = "DsEstado",
                Width = 120,
                ReadOnly = true
            });

            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Proyecto",
                DataPropertyName = "DsProyecto",
                Name = "DsProyecto",
                Width = 150,
                ReadOnly = true
            });

            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Archivo Original",
                DataPropertyName = "DsNombreArchivo",
                Name = "DsNombreArchivo",
                Width = 250,
                ReadOnly = true
            });

            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Carpeta",
                DataPropertyName = "DsNombreUltimaCarpeta",
                Name = "DsNombreUltimaCarpeta",
                Width = 150,
                ReadOnly = true
            });

            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Pág",
                DataPropertyName = "NuPagina",
                Name = "NuPagina",
                Width = 60,
                ReadOnly = true
            });

            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Total Págs",
                DataPropertyName = "NuCantidadPaginas",
                Name = "NuCantidadPaginas",
                Width = 85,
                ReadOnly = true
            });

            dgvArchivos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Nombre Archivo Página",
                DataPropertyName = "DsNombreArchivoPagina",
                Name = "DsNombreArchivoPagina",
                Width = 250,
                ReadOnly = true
            });

            // Evento para actualizar contador cuando cambia el valor de un checkbox
            dgvArchivos.CellValueChanged += dgvArchivos_CellValueChanged;
            dgvArchivos.CurrentCellDirtyStateChanged += dgvArchivos_CurrentCellDirtyStateChanged;
        }

        private void dgvArchivos_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            // Si la celda actual es un checkbox y está "sucia" (modificada), hacer commit inmediato
            if (dgvArchivos.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvArchivos.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvArchivos_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            // Si cambió la columna de selección, actualizar info
            if (e.RowIndex >= 0 && e.ColumnIndex == 0) // Columna 0 es "Seleccionado"
            {
                ActualizarInfo();
            }
        }

        private void CargarDatos()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                string? filtroArchivo = string.IsNullOrWhiteSpace(txtFiltroArchivo.Text) 
                    ? null 
                    : txtFiltroArchivo.Text.Trim();

                DateTime? feDesde = null;
                DateTime? feHasta = null;

                // Solo aplicar filtros de fecha si están checked
                // (podríamos agregar CheckBox para habilitar/deshabilitar filtros de fecha)

                // Por ahora cargo todos los archivos del proyecto 1 en estado 1
                _archivos = _loteBL.ObtenerArchivosPaginaParaLote(1, filtroArchivo, feDesde, feHasta);

                dgvArchivos.DataSource = null;
                dgvArchivos.DataSource = _archivos;

                ActualizarInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ActualizarInfo()
        {
            int total = _archivos.Count;
            int seleccionados = _archivos.Count(a => a.Seleccionado);
            lblInfo.Text = $"Total: {total} | Seleccionados: {seleccionados}";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtFiltroArchivo.Clear();
            dtpFeAltaDesde.Value = DateTime.Now;
            dtpFeAltaHasta.Value = DateTime.Now;
            CargarDatos();
        }

        private void btnSeleccionarTodo_Click(object sender, EventArgs e)
        {
            foreach (var archivo in _archivos)
            {
                archivo.Seleccionado = true;
            }
            dgvArchivos.Refresh();
            ActualizarInfo();
        }

        private void btnDeseleccionarTodo_Click(object sender, EventArgs e)
        {
            foreach (var archivo in _archivos)
            {
                archivo.Seleccionado = false;
            }
            dgvArchivos.Refresh();
            ActualizarInfo();
        }

        private void btnSeleccionar50_Click(object sender, EventArgs e)
        {
            // Deseleccionar todo primero
            foreach (var archivo in _archivos)
            {
                archivo.Seleccionado = false;
            }

            // Seleccionar los primeros 50
            int contador = 0;
            foreach (var archivo in _archivos)
            {
                if (contador >= 50) break;
                archivo.Seleccionado = true;
                contador++;
            }

            dgvArchivos.Refresh();
            ActualizarInfo();
        }

        private void btnCrearLotes_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que haya archivos seleccionados
                var seleccionados = _archivos.Where(a => a.Seleccionado).ToList();
                if (seleccionados.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar al menos un archivo página", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar campo archivos por lote
                if (!int.TryParse(txtArchivosPorLote.Text, out int archivosPorLote) || archivosPorLote <= 0)
                {
                    MessageBox.Show("Debe ingresar una cantidad válida de archivos por lote", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtArchivosPorLote.Focus();
                    return;
                }

                // Confirmar acción
                var result = MessageBox.Show(
                    $"Se crearán lotes con los {seleccionados.Count} archivos seleccionados.\n" +
                    $"Cantidad de archivos por lote: {archivosPorLote}\n\n¿Desea continuar?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                Cursor = Cursors.WaitCursor;

                // Obtener IDs de archivos seleccionados
                var idsSeleccionados = seleccionados.Select(a => a.CdArchivoPagina).ToList();

                // Crear lotes
                var (exito, mensaje) = _loteBL.CrearLotes(idsSeleccionados, archivosPorLote);

                if (exito)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Recargar datos para mostrar solo los que quedaron en estado 1
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear lotes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
