using IndexadorIA.Datos;
using IndexadorIA.Negocio.Api;
using IndexadorIA.Entidades;

namespace IndexadorIA.Pantallas.FinalizacionPlanos
{
    /// <summary>Detalle de un lote final: archivos originales a controlar.</summary>
    public class FrmControlFinalArchivos : Form
    {
        private readonly LoteFinalDto _lote;
        private readonly LoteFinalControlDAL _dal = new();
        private readonly DataGridView dgv = new() { Dock = DockStyle.Fill };
        private readonly Button btnVer = new() { Text = "Ver" };
        private readonly Button btnCerrar = new() { Text = "Cerrar" };
        private readonly CheckBox chkSoloPendientes = new() { Text = "Solo pendientes", Checked = true, AutoSize = true, Padding = new Padding(10, 6, 0, 0) };
        private List<ArchivoLoteFinalDto> _todos = new();

        public FrmControlFinalArchivos(LoteFinalDto lote)
        {
            _lote = lote;
            Text = $"Control Final - {lote.DsNombreLote}";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1100, 650);
            EstiloOscuro.Formulario(this);

            EstiloOscuro.Grilla(dgv);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.Columns.Add(Col("DsNombreArchivo", "Archivo original", 30));
            dgv.Columns.Add(Col("DsRutaCompleta", "Ruta", 45));
            dgv.Columns.Add(Col("DsEstado", "Estado", 10));
            dgv.Columns.Add(Col("NuPaginas", "Paginas", 7));
            dgv.Columns.Add(Col("NuPendientes", "Pendientes", 8));

            EstiloOscuro.Boton(btnVer);
            EstiloOscuro.Boton(btnCerrar, EstiloOscuro.Rojo);
            chkSoloPendientes.ForeColor = Color.White;
            var barra = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(8), BackColor = EstiloOscuro.Fondo };
            barra.Controls.AddRange(new Control[] { btnVer, chkSoloPendientes, btnCerrar });

            Controls.Add(dgv);
            Controls.Add(barra);

            btnVer.Click += (_, _) => Abrir();
            btnCerrar.Click += (_, _) => Close();
            chkSoloPendientes.CheckedChanged += (_, _) => Filtrar();
            dgv.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) Abrir(); };
            Load += (_, _) => Cargar();
        }

        private static DataGridViewTextBoxColumn Col(string prop, string titulo, float peso) =>
            new() { DataPropertyName = prop, HeaderText = titulo, ReadOnly = true, FillWeight = peso };

        private void Cargar()
        {
            try
            {
                _todos = SesionApi.ModoRemoto
                    ? Task.Run(() => new ApiClienteServicio().ObtenerArchivosLoteFinalAsync(_lote.CdLoteFinal)).GetAwaiter().GetResult()
                    : _dal.ObtenerArchivosDeLote(_lote.CdLoteFinal);
                Filtrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar archivos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Filtrar()
        {
            dgv.DataSource = chkSoloPendientes.Checked ? _todos.Where(a => a.CdEstado == 1).ToList() : _todos;
        }

        private void Abrir()
        {
            if (dgv.CurrentRow?.DataBoundItem is not ArchivoLoteFinalDto archivo)
            {
                MessageBox.Show("Seleccione un archivo.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (archivo.CdEstado != 1)
            {
                MessageBox.Show("El archivo no esta pendiente de control.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using var frm = new FrmControlFinalPlanos(_lote, archivo);
            frm.ShowDialog(this);
            Cargar();
        }
    }
}
