using ClosedXML.Excel;
using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Pantallas.Reportes
{
    /// <summary>
    /// Pantalla de reporte "Producción x Usuarios": permite filtrar por usuario (o
    /// "Todos") y rango de fechas, mostrando la cantidad de resultados controlados,
    /// lotes finalizados de control y campos corregidos manualmente por cada usuario.
    /// Solo disponible en modo local (no tiene equivalente remoto vía API).
    /// </summary>
    public partial class FrmProduccionUsuarios : Form
    {
        /// <summary>
        /// Envoltorio simple para incluir la opción "Todos" (CdUsuario = 0) en el combo.
        /// </summary>
        private class UsuarioFiltro
        {
            public int CdUsuario { get; set; }
            public string DsUsuario { get; set; } = string.Empty;
        }

        public FrmProduccionUsuarios()
        {
            InitializeComponent();
        }

        private void FrmProduccionUsuarios_Load(object sender, EventArgs e)
        {
            try
            {
                dtpFechaDesde.Value = DateTime.Now.Date;
                dtpFechaHasta.Value = DateTime.Now.Date;

                ConfigurarDataGridView();
                CargarUsuarios();
                BuscarProduccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarUsuarios()
        {
            var usuarioDAL = new UsuarioDAL();
            var usuarios = usuarioDAL.ObtenerTodos()
                .Select(u => new UsuarioFiltro { CdUsuario = u.CdUsuario, DsUsuario = u.DsNombreCompleto })
                .ToList();

            usuarios.Insert(0, new UsuarioFiltro { CdUsuario = 0, DsUsuario = "Todos" });

            cboUsuarios.DisplayMember = "DsUsuario";
            cboUsuarios.ValueMember = "CdUsuario";
            cboUsuarios.DataSource = usuarios;
            cboUsuarios.SelectedIndex = 0;
        }

        private void ConfigurarDataGridView()
        {
            dgvProduccion.AutoGenerateColumns = false;
            dgvProduccion.Columns.Clear();

            if (chkDetallePorFecha.Checked)
            {
                dgvProduccion.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "fecha",
                    DataPropertyName = "Fecha",
                    HeaderText = "Fecha",
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
                });
            }

            dgvProduccion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsUsuario",
                DataPropertyName = "DsUsuario",
                HeaderText = "Usuario",
                ReadOnly = true
            });

            dgvProduccion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "cantidadTotal",
                DataPropertyName = "CantidadTotal",
                HeaderText = "Cantidad Total",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvProduccion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "cantidadControlada",
                DataPropertyName = "CantidadControlada",
                HeaderText = "Cantidad Controlada",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvProduccion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "cantidadDatosIlegibles",
                DataPropertyName = "CantidadDatosIlegibles",
                HeaderText = "Cantidad Datos Ilegible",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvProduccion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "cantidadPaginaIlegible",
                DataPropertyName = "CantidadPaginaIlegible",
                HeaderText = "Cantidad Pagina Ilegible",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvProduccion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "cantidadLotesCompletos",
                DataPropertyName = "CantidadLotesCompletos",
                HeaderText = "Cantidad de Lotes Completos",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvProduccion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "cantidadCamposCorregidos",
                DataPropertyName = "CantidadCamposCorregidos",
                HeaderText = "Cantidad Campos Corregidos",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
        }

        private void BuscarProduccion()
        {
            try
            {
                int? cdUsuario = cboUsuarios.SelectedValue is int cdUsuarioSeleccionado && cdUsuarioSeleccionado != 0
                    ? cdUsuarioSeleccionado
                    : null;

                DateTime feDesde = dtpFechaDesde.Value.Date;
                DateTime feHasta = dtpFechaHasta.Value.Date.AddDays(1).AddTicks(-1);

                if (feDesde > feHasta)
                {
                    MessageBox.Show("La fecha Desde no puede ser posterior a la fecha Hasta.",
                        "Filtro inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var produccionDAL = new ProduccionDAL();
                var produccion = produccionDAL.ObtenerProduccionPorUsuario(cdUsuario, feDesde, feHasta, chkDetallePorFecha.Checked);

                ConfigurarDataGridView();
                dgvProduccion.DataSource = null;
                dgvProduccion.DataSource = produccion;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar la producción por usuario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarProduccion();
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (dgvProduccion.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.",
                    "Exportar a Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo de Excel (*.xlsx)|*.xlsx",
                FileName = $"ProduccionUsuarios_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                using var libro = new XLWorkbook();
                var hoja = libro.Worksheets.Add("Producción x Usuarios");

                for (int i = 0; i < dgvProduccion.Columns.Count; i++)
                {
                    hoja.Cell(1, i + 1).Value = dgvProduccion.Columns[i].HeaderText;
                    hoja.Cell(1, i + 1).Style.Font.Bold = true;
                }

                for (int fila = 0; fila < dgvProduccion.Rows.Count; fila++)
                {
                    for (int columna = 0; columna < dgvProduccion.Columns.Count; columna++)
                    {
                        var valor = dgvProduccion.Rows[fila].Cells[columna].Value;
                        hoja.Cell(fila + 2, columna + 1).Value = valor switch
                        {
                            null => string.Empty,
                            DateTime fecha => fecha.ToString("dd/MM/yyyy"),
                            int numero => numero,
                            _ => valor.ToString()
                        };
                    }
                }

                hoja.Columns().AdjustToContents();
                libro.SaveAs(saveFileDialog.FileName);

                MessageBox.Show("El archivo se exportó correctamente.",
                    "Exportar a Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar a Excel: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
