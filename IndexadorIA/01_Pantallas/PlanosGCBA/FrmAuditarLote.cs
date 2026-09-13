using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    /// <summary>
    /// Pantalla de Auditoría de un lote asignado al usuario auditor (cdEstadoLote=8).
    /// Permite visualizar los registros del lote, analizarlos (validaciones automáticas
    /// que marcan en rojo los registros sospechosos) y finalmente marcar el lote como auditado.
    /// </summary>
    public partial class FrmAuditarLote : Form
    {
        private const int ANIO_EXPEDIENTE_MINIMO = 1850;
        private const int ANIO_EXPEDIENTE_MAXIMO = 2030;

        private readonly int _cdLote;
        private readonly string _dsNombreLote;
        private List<RegistroAuditoria> _registrosActuales = new();
        private readonly HashSet<int> _cdResultadosRevisados = new();

        public FrmAuditarLote(int cdLote, string dsNombreLote)
        {
            InitializeComponent();
            _cdLote = cdLote;
            _dsNombreLote = dsNombreLote;
        }

        private void FrmAuditarLote_Load(object sender, EventArgs e)
        {
            try
            {
                Text = $"Auditar Lote - {_dsNombreLote}";

                ConfigurarDataGridView();
                CargarEstadosControl();
                CargarRegistros();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvRegistros.AutoGenerateColumns = false;
            dgvRegistros.Columns.Clear();

            dgvRegistros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "cdLote",
                DataPropertyName = "CdLote",
                HeaderText = "ID Lote",
                Width = 70
            });

            dgvRegistros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsNombreLote",
                DataPropertyName = "DsNombreLote",
                HeaderText = "Nombre Lote"
            });

            dgvRegistros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsCategoriaPlano",
                DataPropertyName = "DsCategoriaPlano",
                HeaderText = "Categoría"
            });

            dgvRegistros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsTipoPlano",
                DataPropertyName = "DsTipoPlano",
                HeaderText = "Tipo Plano"
            });

            dgvRegistros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsExpediente",
                DataPropertyName = "DsExpediente",
                HeaderText = "Expediente"
            });

            dgvRegistros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsSeccion",
                DataPropertyName = "DsSeccion",
                HeaderText = "Sección"
            });

            dgvRegistros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsManzana",
                DataPropertyName = "DsManzana",
                HeaderText = "Manzana"
            });

            dgvRegistros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsParcela",
                DataPropertyName = "DsParcela",
                HeaderText = "Parcela"
            });

            dgvRegistros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsDireccion",
                DataPropertyName = "DsDireccion",
                HeaderText = "Dirección"
            });

            dgvRegistros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsNumeroPlano",
                DataPropertyName = "DsNumeroPlano",
                HeaderText = "Número Plano"
            });

            dgvRegistros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsObservaciones",
                DataPropertyName = "DsObservaciones",
                HeaderText = "Observaciones"
            });

            dgvRegistros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsEstadoControl",
                DataPropertyName = "DsEstadoControl",
                HeaderText = "Estado de Control"
            });

            foreach (DataGridViewColumn columna in dgvRegistros.Columns)
            {
                columna.SortMode = DataGridViewColumnSortMode.Automatic;
                columna.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void CargarEstadosControl()
        {
            var resultadoDAL = new ResultadoIADAL();
            var estados = resultadoDAL.ObtenerEstadosControlParaFiltro();

            cboEstadoControl.DisplayMember = "Value";
            cboEstadoControl.ValueMember = "Key";
            cboEstadoControl.DataSource = estados;
            cboEstadoControl.SelectedIndex = -1;
        }

        private void CargarRegistros()
        {
            try
            {
                var resultadoDAL = new ResultadoIADAL();

                List<int>? cdEstadosControl = null;
                if (cboEstadoControl.SelectedValue is int cdEstadoSeleccionado)
                {
                    cdEstadosControl = new List<int> { cdEstadoSeleccionado };
                }

                _registrosActuales = resultadoDAL.ObtenerParaAuditoria(_cdLote, cdEstadosControl);

                foreach (var registro in _registrosActuales)
                {
                    registro.EsRevisado = _cdResultadosRevisados.Contains(registro.CdResultado);
                }

                dgvRegistros.DataSource = null;
                dgvRegistros.DataSource = _registrosActuales;

                ActualizarTotalizador();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los registros del lote: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarTotalizador()
        {
            lblTotalizadorRegistros.Text = $"Total: {_registrosActuales.Count}";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarRegistros();
        }

        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            cboEstadoControl.SelectedIndex = -1;
            CargarRegistros();
        }

        private void dgvRegistros_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _registrosActuales.Count)
            {
                return;
            }

            var registro = _registrosActuales[e.RowIndex];
            if (registro.EsInvalido)
            {
                dgvRegistros.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.IndianRed;
                dgvRegistros.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
            }
            else if (registro.EsRevisado)
            {
                dgvRegistros.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                dgvRegistros.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void btnAnalizar_Click(object sender, EventArgs e)
        {
            try
            {
                int cantidadInvalidos = 0;

                foreach (var registro in _registrosActuales)
                {
                    registro.EsInvalido = !ValidarRegistro(registro);
                    if (registro.EsInvalido)
                    {
                        cantidadInvalidos++;
                    }
                }

                dgvRegistros.Refresh();

                MessageBox.Show(
                    cantidadInvalidos > 0
                        ? $"Se encontraron {cantidadInvalidos} registro(s) marcado(s) en rojo para revisión."
                        : "No se encontraron inconsistencias en los registros analizados.",
                    "Análisis Finalizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al analizar los registros: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Valida un registro según las reglas de auditoría. Devuelve true si el registro es válido.
        /// </summary>
        private bool ValidarRegistro(RegistroAuditoria registro)
        {
            if (registro.CdEstadoControl == ResultadoIA.EstadosControl.Controlado
                && string.IsNullOrWhiteSpace(registro.DsExpediente))
            {
                return false;
            }

            if ((registro.CdEstadoControl == ResultadoIA.EstadosControl.Controlado
                    || registro.CdEstadoControl == ResultadoIA.EstadosControl.PaginaIlegible
                    || registro.CdEstadoControl == ResultadoIA.EstadosControl.DatosIlegibles)
                && !string.IsNullOrWhiteSpace(registro.DsExpediente))
            {
                var partes = registro.DsExpediente.Split('-');
                if (partes.Length < 2 || !int.TryParse(partes[1], out int anio)
                    || anio < ANIO_EXPEDIENTE_MINIMO || anio > ANIO_EXPEDIENTE_MAXIMO)
                {
                    return false;
                }
            }

            return true;
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            AbrirVentanaVerRegistro();
        }

        private void dgvRegistros_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                AbrirVentanaVerRegistro();
            }
        }

        private void AbrirVentanaVerRegistro()
        {
            if (dgvRegistros.CurrentRow?.DataBoundItem is not RegistroAuditoria registro)
            {
                MessageBox.Show("Seleccione un registro para revisar.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var frmVerRegistro = new FrmVerRegistroAuditoria(registro);
            var resultado = frmVerRegistro.ShowDialog(this);

            if (resultado == DialogResult.OK)
            {
                _cdResultadosRevisados.Add(registro.CdResultado);
            }

            CargarRegistros();
        }

        private void btnMarcarLoteAuditado_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmacion = MessageBox.Show(
                    "¿Esta seguro que desea marcar como auditado el lote?",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes)
                {
                    return;
                }

                int? cdUsuarioAuditado = SesionActual.UsuarioActual?.CdUsuario;
                if (cdUsuarioAuditado is null)
                {
                    MessageBox.Show("No se pudo determinar el usuario actual.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var loteDAL = new LoteDAL();
                loteDAL.MarcarLoteAuditado(_cdLote, cdUsuarioAuditado.Value);

                MessageBox.Show("El lote fue marcado como auditado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al marcar el lote como auditado: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
