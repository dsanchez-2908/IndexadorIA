using ClosedXML.Excel;
using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;
using IndexadorIA.Pantallas.PlanosGCBA;

namespace IndexadorIA.Pantallas.Consultas
{
    /// <summary>
    /// Pantalla de "Consulta de Planos": permite filtrar registros de la vista
    /// VW_PLANOS_CONSULTAS por múltiples criterios y exportar el resultado a Excel.
    /// </summary>
    public partial class FrmConsultaPlanos : Form
    {
        /// <summary>
        /// Valor de respaldo para la cantidad máxima de registros a mostrar, usado
        /// únicamente si el parámetro MAX_CONSULTA no está configurado en
        /// TD_PARAMETROS o su valor no es un número válido.
        /// </summary>
        private const int MaxRegistrosResultadoPorDefecto = 50000;

        /// <summary>
        /// Clave del parámetro en TD_PARAMETROS que define la cantidad máxima de
        /// registros a mostrar en la grilla de Consulta de Planos.
        /// </summary>
        private const string ClaveParametroMaxConsulta = "MAX_CONSULTA";

        private int _maxRegistrosResultado = MaxRegistrosResultadoPorDefecto;

        private class ComboItem
        {
            public int? Codigo { get; set; }
            public string Texto { get; set; } = string.Empty;
        }

        public FrmConsultaPlanos()
        {
            InitializeComponent();
        }

        private void FrmConsultaPlanos_Load(object sender, EventArgs e)
        {
            try
            {
                CargarMaxRegistrosResultado();
                ConfigurarDataGridView();
                CargarCombos();
                ActualizarContadorRegistros();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Lee de TD_PARAMETROS la clave MAX_CONSULTA y la usa como cantidad máxima
        /// de registros a traer/mostrar en la grilla. Si el parámetro no existe o su
        /// valor no es un entero válido, se usa MaxRegistrosResultadoPorDefecto.
        /// </summary>
        private void CargarMaxRegistrosResultado()
        {
            var parametrosDAL = new ParametrosDAL();
            string? valor = parametrosDAL.ObtenerValor(ClaveParametroMaxConsulta);

            _maxRegistrosResultado = int.TryParse(valor, out int maxRegistros) && maxRegistros > 0
                ? maxRegistros
                : MaxRegistrosResultadoPorDefecto;
        }

        private void CargarCombos()
        {
            var dal = new PlanoConsultaDAL();

            var estadosLote = dal.ObtenerEstadosLoteParaFiltro()
                .Select(kv => new ComboItem { Codigo = kv.Key, Texto = kv.Value })
                .ToList();
            estadosLote.Insert(0, new ComboItem { Codigo = null, Texto = "Todos" });
            cboEstadoLote.DisplayMember = "Texto";
            cboEstadoLote.ValueMember = "Codigo";
            cboEstadoLote.DataSource = estadosLote;
            cboEstadoLote.SelectedIndex = 0;

            var estadosControl = dal.ObtenerEstadosControlParaFiltro()
                .Select(kv => new ComboItem { Codigo = kv.Key, Texto = kv.Value })
                .ToList();
            estadosControl.Insert(0, new ComboItem { Codigo = null, Texto = "Todos" });
            cboEstadoControl.DisplayMember = "Texto";
            cboEstadoControl.ValueMember = "Codigo";
            cboEstadoControl.DataSource = estadosControl;
            cboEstadoControl.SelectedIndex = 0;

            var categoriaDAL = new CategoriaPlanoDAL();
            var categorias = categoriaDAL.ObtenerTodos()
                .Select(c => new ComboItem { Codigo = c.CdCategoriaPlano, Texto = c.DsCategoriaPlano })
                .ToList();
            categorias.Insert(0, new ComboItem { Codigo = null, Texto = "Todos" });
            cboCategoriaPlano.DisplayMember = "Texto";
            cboCategoriaPlano.ValueMember = "Codigo";
            cboCategoriaPlano.DataSource = categorias;
            cboCategoriaPlano.SelectedIndex = 0;

            var tipoDAL = new TipoPlanoDAL();
            var tipos = tipoDAL.ObtenerTodos()
                .Select(t => new ComboItem { Codigo = t.CdTipoPlano, Texto = t.DsTipoPlano })
                .ToList();
            tipos.Insert(0, new ComboItem { Codigo = null, Texto = "Todos" });
            cboTipoPlano.DisplayMember = "Texto";
            cboTipoPlano.ValueMember = "Codigo";
            cboTipoPlano.DataSource = tipos;
            cboTipoPlano.SelectedIndex = 0;
        }

        private void ConfigurarDataGridView()
        {
            dgvPlanos.AutoGenerateColumns = false;
            dgvPlanos.Columns.Clear();

            AgregarColumna("cdLote", "CdLote", "Lote");
            AgregarColumna("dsEstadoLote", "DsEstadoLote", "Estado de Lote");
            AgregarColumna("dsDescripcionLote", "DsDescripcionLote", "Descripción Lote");
            AgregarColumna("dsEntrega", "DsEntrega", "Entrega");
            AgregarColumna("dsCarpeta", "DsCarpeta", "Carpeta");
            AgregarColumna("dsEstadoControl", "DsEstadoControl", "Estado de Control");
            AgregarColumna("dsCategoriaPlano", "DsCategoriaPlano", "Categoría de Plano");
            AgregarColumna("dsTipoPlano", "DsTipoPlano", "Tipo de Plano");
            AgregarColumna("dsExpediente", "DsExpediente", "Expediente");
            AgregarColumna("dsSeccion", "DsSeccion", "Sección");
            AgregarColumna("dsManzana", "DsManzana", "Manzana");
            AgregarColumna("dsParcela", "DsParcela", "Parcela");
            AgregarColumna("dsDireccion", "DsDireccion", "Dirección");
            AgregarColumna("dsNumeroPlano", "DsNumeroPlano", "Número de Plano");
            AgregarColumna("dsObservaciones", "DsObservaciones", "Observaciones");
            AgregarColumna("dsNombreArchivoOriginal", "DsNombreArchivoOriginal", "Archivo Original");
            AgregarColumna("dsNombreArchivoPagina", "DsNombreArchivoPagina", "Archivo Página");
            AgregarColumna("nuPagina", "NuPagina", "Página");
        }

        private void AgregarColumna(string nombre, string propiedad, string encabezado)
        {
            dgvPlanos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nombre,
                DataPropertyName = propiedad,
                HeaderText = encabezado,
                ReadOnly = true
            });
        }

        private int? ObtenerCodigoSeleccionado(ComboBox combo)
        {
            return combo.SelectedItem is ComboItem item ? item.Codigo : null;
        }

        private void Buscar()
        {
            try
            {
                var filtro = new PlanoConsultaFiltro
                {
                    CdLote = string.IsNullOrWhiteSpace(txtCdLote.Text) ? null : (int.TryParse(txtCdLote.Text, out int cdLote) ? cdLote : null),
                    CdEstadoLote = ObtenerCodigoSeleccionado(cboEstadoLote),
                    DsDescripcionLote = txtDescripcionLote.Text,
                    DsEntrega = txtEntrega.Text,
                    DsCarpeta = txtCarpeta.Text,
                    CdEstadoControl = ObtenerCodigoSeleccionado(cboEstadoControl),
                    CdCategoriaPlano = ObtenerCodigoSeleccionado(cboCategoriaPlano),
                    CdTipoPlano = ObtenerCodigoSeleccionado(cboTipoPlano),
                    DsExpediente = txtExpediente.Text,
                    DsSeccion = txtSeccion.Text,
                    DsManzana = txtManzana.Text,
                    DsParcela = txtParcela.Text,
                    DsDireccion = txtDireccion.Text,
                    DsNumeroPlano = txtNumeroPlano.Text,
                    DsObservaciones = txtObservaciones.Text,
                    MaxRegistros = _maxRegistrosResultado
                };

                var dal = new PlanoConsultaDAL();
                var resultado = dal.Consultar(filtro);

                dgvPlanos.DataSource = null;
                dgvPlanos.DataSource = resultado;

                ActualizarContadorRegistros();

                if (resultado.Count >= _maxRegistrosResultado)
                {
                    MessageBox.Show(
                        $"Se alcanzó el límite máximo de {_maxRegistrosResultado:N0} registros. Refine los filtros para obtener resultados más precisos.",
                        "Límite de resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar planos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarContadorRegistros()
        {
            lblCantidadRegistros.Text = $"Registros: {dgvPlanos.Rows.Count:N0}";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCdLote.Clear();
            txtDescripcionLote.Clear();
            txtEntrega.Clear();
            txtCarpeta.Clear();
            txtExpediente.Clear();
            txtSeccion.Clear();
            txtManzana.Clear();
            txtParcela.Clear();
            txtDireccion.Clear();
            txtNumeroPlano.Clear();
            txtObservaciones.Clear();
            cboEstadoLote.SelectedIndex = 0;
            cboEstadoControl.SelectedIndex = 0;
            cboCategoriaPlano.SelectedIndex = 0;
            cboTipoPlano.SelectedIndex = 0;

            dgvPlanos.DataSource = null;
            ActualizarContadorRegistros();
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (dgvPlanos.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.",
                    "Exportar a Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo de Excel (*.xlsx)|*.xlsx",
                FileName = $"ConsultaPlanos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                using var libro = new XLWorkbook();
                var hoja = libro.Worksheets.Add("Consulta de Planos");

                for (int i = 0; i < dgvPlanos.Columns.Count; i++)
                {
                    hoja.Cell(1, i + 1).Value = dgvPlanos.Columns[i].HeaderText;
                    hoja.Cell(1, i + 1).Style.Font.Bold = true;
                }

                for (int fila = 0; fila < dgvPlanos.Rows.Count; fila++)
                {
                    for (int columna = 0; columna < dgvPlanos.Columns.Count; columna++)
                    {
                        var valor = dgvPlanos.Rows[fila].Cells[columna].Value;
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

        private void btnAuditarPlano_Click(object sender, EventArgs e)
        {
            if (dgvPlanos.CurrentRow?.DataBoundItem is not PlanoConsulta plano)
            {
                MessageBox.Show("Seleccione un registro para auditar.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var registro = new RegistroAuditoria
            {
                CdResultado = plano.CdResultado,
                CdLote = plano.CdLote,
                DsNombreLote = plano.DsCarpeta ?? string.Empty,
                CdEstadoLote = plano.CdEstadoLote ?? 0,
                DsEstadoLote = plano.DsEstadoLote,
                CdArchivoPagina = plano.CdArchivoPagina ?? 0,
                DsNombreArchivoOriginal = plano.DsNombreArchivoOriginal,
                CdCategoriaPlano = plano.CdCategoriaPlano,
                DsCategoriaPlano = plano.DsCategoriaPlano,
                CdTipoPlano = plano.CdTipoPlano,
                DsTipoPlano = plano.DsTipoPlano,
                DsExpediente = plano.DsExpediente,
                DsSeccion = plano.DsSeccion,
                DsManzana = plano.DsManzana,
                DsParcela = plano.DsParcela,
                DsDireccion = plano.DsDireccion,
                DsNumeroPlano = plano.DsNumeroPlano,
                DsObservaciones = plano.DsObservaciones,
                CdEstadoControl = plano.CdEstadoControl ?? 0,
                DsEstadoControl = plano.DsEstadoControl
            };

            using var frmVerRegistro = new FrmVerRegistroAuditoria(registro);
            var resultado = frmVerRegistro.ShowDialog(this);

            if (resultado == DialogResult.OK)
            {
                Buscar();
            }
        }

        private void btnReubicarLote_Click(object sender, EventArgs e)
        {
            if (dgvPlanos.DataSource is not List<PlanoConsulta> planos || planos.Count == 0)
            {
                MessageBox.Show("No hay registros en la grilla para reubicar.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var archivosPorLoteOrigen = planos
                .Where(p => p.CdArchivoPagina.HasValue)
                .GroupBy(p => p.CdLote)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(p => p.CdArchivoPagina!.Value).Distinct().ToList());

            int totalPlanos = archivosPorLoteOrigen.Values.Sum(v => v.Count);

            if (totalPlanos == 0)
            {
                MessageBox.Show("No se encontraron archivos página válidos para reubicar.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frmReubicar = new FrmReubicarPlanosMultiple(archivosPorLoteOrigen.Count, totalPlanos);
            if (frmReubicar.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                var loteBL = new LoteBL();
                var (exito, mensaje, _) = loteBL.ReubicarEnNuevosLotesMultiples(archivosPorLoteOrigen, frmReubicar.DescripcionLote);

                if (exito)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Buscar();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al reubicar planos en nuevos lotes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
