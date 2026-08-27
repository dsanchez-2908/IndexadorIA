using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    /// <summary>
    /// Pantalla de Asignación de Lotes: permite asignar lotes procesados por IA (cdEstado=4)
    /// a un usuario para su control, cambiando su estado a "Controlando" (cdEstado=6)
    /// </summary>
    public partial class FrmAsignacionLote : Form
    {
        private const int CD_ESTADO_PROCESADO_IA = 4;

        private List<LoteSeleccionable> _lotesActuales = new();
        private int _indiceSeleccion10 = 0;

        /// <summary>
        /// Envoltorio de Lote con una propiedad de selección para el checkbox de la grilla
        /// </summary>
        private class LoteSeleccionable
        {
            public bool Seleccionado { get; set; }
            public Lote Lote { get; set; } = null!;

            public int CdLote => Lote.CdLote;
            public string DsNombreLote => Lote.DsNombreLote;
            public int NuCantidadArchivos => Lote.NuCantidadArchivos;
            public string? DsEstado => Lote.DsEstado;
            public DateTime FeAltaLote => Lote.FeAltaLote;
            public int NuCorrectos => Lote.NuCorrectos;
            public int NuIncorrectos => Lote.NuIncorrectos;
        }

        public FrmAsignacionLote()
        {
            InitializeComponent();
        }

        private void FrmAsignacionLote_Load(object sender, EventArgs e)
        {
            try
            {
                dtpFechaDesde.Value = DateTime.Now.AddDays(-30);
                dtpFechaHasta.Value = DateTime.Now;
                chkFiltrarFecha.Checked = false;
                dtpFechaDesde.Enabled = false;
                dtpFechaHasta.Enabled = false;

                ConfigurarDataGridView();
                CargarUsuarios();
                CargarLotes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvLotes.AutoGenerateColumns = false;
            dgvLotes.Columns.Clear();

            dgvLotes.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "chkSeleccionado",
                DataPropertyName = "Seleccionado",
                HeaderText = string.Empty,
                Width = 40
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "cdLote",
                DataPropertyName = "CdLote",
                HeaderText = "ID Lote",
                Width = 70
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsNombreLote",
                DataPropertyName = "DsNombreLote",
                HeaderText = "Nombre Lote"
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadArchivos",
                DataPropertyName = "NuCantidadArchivos",
                HeaderText = "Cant. Archivos"
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsEstado",
                DataPropertyName = "DsEstado",
                HeaderText = "Estado"
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "feAltaLote",
                DataPropertyName = "FeAltaLote",
                HeaderText = "Fecha Alta",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCorrectos",
                DataPropertyName = "NuCorrectos",
                HeaderText = "Correctos"
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuIncorrectos",
                DataPropertyName = "NuIncorrectos",
                HeaderText = "Incorrectos"
            });
        }

        private void CargarUsuarios()
        {
            var usuarioDAL = new UsuarioDAL();
            var usuarios = usuarioDAL.ObtenerTodos();

            cboUsuarios.DisplayMember = "DsUsuario";
            cboUsuarios.ValueMember = "CdUsuario";
            cboUsuarios.DataSource = usuarios;
            cboUsuarios.SelectedIndex = -1;
        }

        private void CargarLotes()
        {
            try
            {
                var loteDAL = new LoteDAL();

                string? dsNombreLote = string.IsNullOrWhiteSpace(txtNombreLote.Text) ? null : txtNombreLote.Text.Trim();
                DateTime? feAltaDesde = chkFiltrarFecha.Checked ? dtpFechaDesde.Value : null;
                DateTime? feAltaHasta = chkFiltrarFecha.Checked ? dtpFechaHasta.Value : null;

                var lotes = loteDAL.ObtenerLotesPorEstadoFiltrado(
                    CD_ESTADO_PROCESADO_IA, dsNombreLote, feAltaDesde, feAltaHasta);

                _lotesActuales = lotes.Select(l => new LoteSeleccionable { Lote = l }).ToList();
                _indiceSeleccion10 = 0;

                dgvLotes.DataSource = null;
                dgvLotes.DataSource = _lotesActuales;

                ActualizarTotalizador();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar lotes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarTotalizador()
        {
            lblTotalizadorLotes.Text = $"Total: {_lotesActuales.Count}";
        }

        private void chkFiltrarFecha_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaDesde.Enabled = chkFiltrarFecha.Checked;
            dtpFechaHasta.Enabled = chkFiltrarFecha.Checked;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLotes();
        }

        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtNombreLote.Text = string.Empty;
            chkFiltrarFecha.Checked = false;
            dtpFechaDesde.Value = DateTime.Now.AddDays(-30);
            dtpFechaHasta.Value = DateTime.Now;
            CargarLotes();
        }

        private void dgvLotes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLotes.Columns[e.ColumnIndex].Name == "chkSeleccionado")
            {
                dgvLotes.EndEdit();
            }
        }

        private void dgvLotes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                AbrirVerLote();
            }
        }

        private void btnVerLote_Click(object sender, EventArgs e)
        {
            AbrirVerLote();
        }

        private void AbrirVerLote()
        {
            if (dgvLotes.CurrentRow?.DataBoundItem is not LoteSeleccionable loteSeleccionado)
            {
                MessageBox.Show("Seleccione un lote para ver su detalle.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var frm = new FrmVerLoteDetalle(loteSeleccionado.CdLote, loteSeleccionado.DsNombreLote);
            frm.ShowDialog(this);
        }

        private void btnAsignarLotes_Click(object sender, EventArgs e)
        {
            try
            {
                var lotesSeleccionados = _lotesActuales.Where(l => l.Seleccionado).Select(l => l.CdLote).ToList();

                if (lotesSeleccionados.Count == 0)
                {
                    MessageBox.Show("Seleccione al menos un lote para asignar.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (cboUsuarios.SelectedValue is not int cdUsuarioAsignado)
                {
                    MessageBox.Show("Seleccione un usuario para asignar los lotes.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var confirmacion = MessageBox.Show(
                    $"¿Confirma asignar {lotesSeleccionados.Count} lote(s) al usuario seleccionado?",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes)
                {
                    return;
                }

                var loteDAL = new LoteDAL();
                loteDAL.AsignarLotes(lotesSeleccionados, cdUsuarioAsignado);

                MessageBox.Show("Lotes asignados correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarLotes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al asignar lotes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSeleccionarTodos_Click(object sender, EventArgs e)
        {
            foreach (var lote in _lotesActuales)
            {
                lote.Seleccionado = true;
            }

            _indiceSeleccion10 = 0;
            dgvLotes.Refresh();
        }

        private void btnDeseleccionarTodo_Click(object sender, EventArgs e)
        {
            foreach (var lote in _lotesActuales)
            {
                lote.Seleccionado = false;
            }

            _indiceSeleccion10 = 0;
            dgvLotes.Refresh();
        }

        private void btnSeleccionar10_Click(object sender, EventArgs e)
        {
            if (_lotesActuales.Count == 0)
            {
                return;
            }

            if (_indiceSeleccion10 >= _lotesActuales.Count)
            {
                _indiceSeleccion10 = 0;
            }

            int cantidad = Math.Min(10, _lotesActuales.Count - _indiceSeleccion10);
            for (int i = 0; i < cantidad; i++)
            {
                _lotesActuales[_indiceSeleccion10 + i].Seleccionado = true;
            }

            _indiceSeleccion10 += cantidad;
            dgvLotes.Refresh();
        }
    }
}
