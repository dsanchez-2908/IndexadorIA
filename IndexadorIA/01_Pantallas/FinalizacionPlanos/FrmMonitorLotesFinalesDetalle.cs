using System.ComponentModel;
using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Pantallas.FinalizacionPlanos
{
    /// <summary>Detalle de lotes finales de un usuario, con reasignacion de los lotes aun asignados (no finalizados).</summary>
    public class FrmMonitorLotesFinalesDetalle : Form
    {
        private sealed class FilaLote
        {
            public bool Seleccionado { get; set; }
            public LoteFinalDto Lote { get; init; } = new();
            public string DsNombreLote => Lote.DsNombreLote;
            public string DsEstadoLote => Lote.DsEstadoLote;
            public int NuArchivos => Lote.NuArchivos;
            public int NuPendientes => Lote.NuPendientes;
            public int NuFinalizados => Lote.NuFinalizados;
            public DateTime FeAlta => Lote.FeAlta;
        }

        private readonly int _cdUsuario;
        private readonly string _dsUsuario;
        private readonly LoteFinalControlDAL _dal = new();
        private BindingList<FilaLote> _filas = new();

        private readonly DataGridView dgv = new() { Dock = DockStyle.Fill };
        private readonly ComboBox cboUsuarios = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 260 };
        private readonly Button btnSelTodo = new() { Text = "Seleccionar Todo" };
        private readonly Button btnDeselTodo = new() { Text = "Deseleccionar Todo" };
        private readonly Button btnReasignar = new() { Text = "Reasignar" };
        private readonly Button btnVerArchivos = new() { Text = "Ver Archivos" };
        private readonly Button btnCerrar = new() { Text = "Cerrar" };
        private readonly CheckBox chkSoloAsignados = new() { Text = "Solo asignados", Checked = true, AutoSize = true, Padding = new Padding(10, 6, 0, 0) };
        private readonly Label lblTotal = new() { AutoSize = true, Padding = new Padding(10, 8, 0, 0), ForeColor = Color.Gainsboro };

        public FrmMonitorLotesFinalesDetalle(int cdUsuario, string dsUsuario)
        {
            _cdUsuario = cdUsuario;
            _dsUsuario = dsUsuario;
            Text = $"Detalle de Lotes Finales - {dsUsuario}";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1000, 600);
            EstiloOscuro.Formulario(this);

            EstiloOscuro.Grilla(dgv);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = nameof(FilaLote.Seleccionado), HeaderText = "", FillWeight = 5 });
            dgv.Columns.Add(Col(nameof(FilaLote.DsNombreLote), "Lote"));
            dgv.Columns.Add(Col(nameof(FilaLote.DsEstadoLote), "Estado"));
            dgv.Columns.Add(Col(nameof(FilaLote.NuArchivos), "Archivos"));
            dgv.Columns.Add(Col(nameof(FilaLote.NuPendientes), "Pendientes"));
            dgv.Columns.Add(Col(nameof(FilaLote.NuFinalizados), "Finalizados"));
            dgv.Columns.Add(Col(nameof(FilaLote.FeAlta), "Fecha alta"));

            foreach (var b in new[] { btnSelTodo, btnDeselTodo, btnVerArchivos, btnReasignar }) EstiloOscuro.Boton(b);
            EstiloOscuro.Boton(btnCerrar, EstiloOscuro.Rojo);
            EstiloOscuro.Campo(cboUsuarios);
            chkSoloAsignados.ForeColor = Color.White;

            var barra = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(8), BackColor = EstiloOscuro.Fondo };
            barra.Controls.AddRange(new Control[]
            {
                btnSelTodo, btnDeselTodo, chkSoloAsignados, btnVerArchivos,
                new Label { Text = "Reasignar a:", AutoSize = true, Padding = new Padding(15, 8, 0, 0), ForeColor = Color.White },
                cboUsuarios, btnReasignar, btnCerrar, lblTotal
            });

            Controls.Add(dgv);
            Controls.Add(barra);

            dgv.CurrentCellDirtyStateChanged += (_, _) => { if (dgv.IsCurrentCellDirty) dgv.CommitEdit(DataGridViewDataErrorContexts.Commit); };
            dgv.CellValueChanged += (_, _) => ActualizarTotal();
            dgv.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) VerArchivos(); };
            btnSelTodo.Click += (_, _) => Marcar(true);
            btnDeselTodo.Click += (_, _) => Marcar(false);
            btnVerArchivos.Click += (_, _) => VerArchivos();
            btnReasignar.Click += (_, _) => Reasignar();
            btnCerrar.Click += (_, _) => Close();
            chkSoloAsignados.CheckedChanged += (_, _) => Cargar();
            Load += (_, _) => { CargarUsuarios(); Cargar(); };
        }

        private static DataGridViewTextBoxColumn Col(string prop, string titulo) =>
            new() { DataPropertyName = prop, HeaderText = titulo, ReadOnly = true };

        private void CargarUsuarios()
        {
            var usuarios = new UsuarioDAL().ObtenerTodos().Where(u => u.CdUsuario != _cdUsuario).ToList();
            cboUsuarios.DisplayMember = "DsUsuario";
            cboUsuarios.ValueMember = "CdUsuario";
            cboUsuarios.DataSource = usuarios;
            cboUsuarios.SelectedIndex = -1;
        }

        private void Cargar()
        {
            try
            {
                var lotes = _dal.ObtenerLotesPorUsuario(_cdUsuario);
                if (chkSoloAsignados.Checked) lotes = lotes.Where(l => l.CdEstadoLote == 1).ToList();
                _filas = new BindingList<FilaLote>(lotes.Select(l => new FilaLote { Lote = l }).ToList());
                dgv.DataSource = _filas;
                ActualizarTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar lotes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarTotal()
        {
            lblTotal.Text = $"Lotes: {_filas.Count}  |  Seleccionados: {_filas.Count(f => f.Seleccionado)}";
        }

        private void Marcar(bool valor)
        {
            dgv.EndEdit();
            foreach (var f in _filas) f.Seleccionado = valor;
            dgv.Refresh();
            ActualizarTotal();
        }

        private void VerArchivos()
        {
            if (dgv.CurrentRow?.DataBoundItem is not FilaLote fila)
            {
                MessageBox.Show("Seleccione un lote.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using var frm = new FrmMonitorLoteFinalArchivos(fila.Lote);
            frm.ShowDialog(this);
        }

        private void Reasignar()
        {
            dgv.EndEdit();
            var marcados = _filas.Where(f => f.Seleccionado).ToList();
            if (marcados.Count == 0)
            {
                MessageBox.Show("Seleccione al menos un lote.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboUsuarios.SelectedValue is not int cdNuevo)
            {
                MessageBox.Show("Seleccione el usuario destino.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var noAsignados = marcados.Where(f => f.Lote.CdEstadoLote != 1).ToList();
            if (noAsignados.Count > 0)
            {
                MessageBox.Show("Solo se pueden reasignar lotes en estado Asignado. Desmarque los lotes finalizados.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var r = MessageBox.Show($"Se reasignaran {marcados.Count} lote(s) de {_dsUsuario} a {cboUsuarios.Text}. ¿Continuar?",
                "Reasignar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            try
            {
                int n = _dal.Reasignar(marcados.Select(f => f.Lote.CdLoteFinal).ToList(), cdNuevo);
                MessageBox.Show($"Lotes reasignados: {n}.", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al reasignar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>Archivos originales de un lote final (solo lectura).</summary>
    internal class FrmMonitorLoteFinalArchivos : Form
    {
        public FrmMonitorLoteFinalArchivos(LoteFinalDto lote)
        {
            Text = $"Archivos del lote {lote.DsNombreLote}";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1100, 600);
            EstiloOscuro.Formulario(this);

            var dgv = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true };
            EstiloOscuro.Grilla(dgv);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DsNombreArchivo", HeaderText = "Archivo original", FillWeight = 30 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DsRutaCompleta", HeaderText = "Ruta", FillWeight = 45 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DsEstado", HeaderText = "Estado", FillWeight = 10 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NuPaginas", HeaderText = "Paginas", FillWeight = 7 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NuPendientes", HeaderText = "Pendientes", FillWeight = 8 });
            Controls.Add(dgv);

            Load += (_, _) =>
            {
                try { dgv.DataSource = new LoteFinalControlDAL().ObtenerArchivosDeLote(lote.CdLoteFinal); }
                catch (Exception ex) { MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };
        }
    }
}
