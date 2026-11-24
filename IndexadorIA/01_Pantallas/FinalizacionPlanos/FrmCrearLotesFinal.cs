using System.ComponentModel;
using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.FinalizacionPlanos
{
    /// <summary>Crear Lotes Final: agrupa archivos originales elegibles y los asigna a un usuario (solo ADMIN).</summary>
    public class FrmCrearLotesFinal : Form
    {
        private const int BloqueSeleccion = 1000;

        private readonly DataGridView dgv = new()
        {
            Dock = DockStyle.Fill,
            AutoGenerateColumns = false,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            BackgroundColor = Color.White
        };
        private readonly ComboBox cboUsuarios = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 260 };
        private readonly Label lblTotalizador = new() { AutoSize = true, Padding = new Padding(10, 8, 0, 0) };
        private readonly Button btnRefrescar = new() { Text = "Refrescar", AutoSize = true };
        private readonly Button btnSelTodo = new() { Text = "Seleccionar Todo", AutoSize = true };
        private readonly Button btnDeselTodo = new() { Text = "Deseleccionar Todo", AutoSize = true };
        private readonly Button btnSel1000 = new() { Text = "Seleccionar 1000", AutoSize = true };
        private readonly Button btnAsignar = new() { Text = "Asignar", AutoSize = true };

        private BindingList<ArchivoOriginalElegibleDto> _datos = new();

        public FrmCrearLotesFinal()
        {
            Text = "Crear Lotes Final";
            Font = new Font("Segoe UI", 10F);
            BackColor = Color.White;

            var barra = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(8) };
            barra.Controls.AddRange(new Control[]
            {
                btnRefrescar, btnSelTodo, btnDeselTodo, btnSel1000,
                new Label { Text = "Usuario:", AutoSize = true, Padding = new Padding(15, 8, 0, 0) },
                cboUsuarios, btnAsignar, lblTotalizador
            });

            dgv.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Seleccionado", DataPropertyName = "Seleccionado", HeaderText = "", Width = 40 });
            dgv.Columns.Add(Texto("CdArchivo", "ID", 80));
            dgv.Columns.Add(Texto("DsNombreArchivo", "Archivo original", 400));
            dgv.Columns.Add(Texto("DsRutaCompleta", "Ruta", 500));
            dgv.Columns.Add(Texto("NuPaginas", "Paginas", 70));
            dgv.Columns.Add(Texto("NuCorrectos", "Correctos", 80));
            dgv.Columns.Add(Texto("NuIlegibles", "Ilegibles", 80));

            Controls.Add(dgv);
            Controls.Add(barra);

            dgv.CurrentCellDirtyStateChanged += (_, _) =>
            {
                if (dgv.IsCurrentCellDirty) dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            dgv.CellValueChanged += (_, _) => ActualizarTotalizador();

            btnRefrescar.Click += (_, _) => Cargar();
            btnSelTodo.Click += (_, _) => Marcar(_ => true);
            btnDeselTodo.Click += (_, _) => Marcar(_ => false);
            btnSel1000.Click += (_, _) => SeleccionarSiguientes();
            btnAsignar.Click += (_, _) => Asignar();

            Load += (_, _) => { CargarUsuarios(); Cargar(); };
        }

        private static DataGridViewTextBoxColumn Texto(string prop, string titulo, int ancho) =>
            new() { DataPropertyName = prop, HeaderText = titulo, Width = ancho, ReadOnly = true };

        private void CargarUsuarios()
        {
            var usuarios = new UsuarioDAL().ObtenerTodos();
            cboUsuarios.DisplayMember = "DsUsuario";
            cboUsuarios.ValueMember = "CdUsuario";
            cboUsuarios.DataSource = usuarios;
            cboUsuarios.SelectedIndex = -1;
        }

        private void Cargar()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _datos = new BindingList<ArchivoOriginalElegibleDto>(new LoteFinalDAL().ObtenerElegibles());
                dgv.DataSource = _datos;
                ActualizarTotalizador();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar archivos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void Marcar(Func<ArchivoOriginalElegibleDto, bool> valor)
        {
            dgv.EndEdit();
            foreach (var d in _datos) d.Seleccionado = valor(d);
            dgv.Refresh();
            ActualizarTotalizador();
        }

        private void SeleccionarSiguientes()
        {
            dgv.EndEdit();
            foreach (var d in _datos.Where(x => !x.Seleccionado).Take(BloqueSeleccion))
            {
                d.Seleccionado = true;
            }
            dgv.Refresh();
            ActualizarTotalizador();
        }

        private void ActualizarTotalizador()
        {
            lblTotalizador.Text = $"Seleccionados: {_datos.Count(d => d.Seleccionado):N0} de {_datos.Count:N0}  |  Paginas/planos: {_datos.Where(d => d.Seleccionado).Sum(d => d.NuPaginas):N0}";
        }

        private void Asignar()
        {
            dgv.EndEdit();
            var seleccionados = _datos.Where(d => d.Seleccionado).Select(d => d.CdArchivo).ToList();
            if (seleccionados.Count == 0)
            {
                MessageBox.Show("Seleccione al menos un archivo.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboUsuarios.SelectedValue is not int cdUsuario)
            {
                MessageBox.Show("Seleccione un usuario.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var r = MessageBox.Show(
                $"Se creara un lote con {seleccionados.Count:N0} archivos para {cboUsuarios.Text}. ¿Continuar?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                int cdLote = new LoteFinalDAL().CrearYAsignar(seleccionados, cdUsuario, SesionActual.UsuarioActual!.CdUsuario);
                MessageBox.Show($"Lote final {cdLote} creado y asignado.", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al asignar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
