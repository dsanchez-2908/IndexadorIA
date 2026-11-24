using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.FinalizacionPlanos
{
    /// <summary>Control Final: lotes finales asignados al usuario logueado.</summary>
    public class FrmControlFinal : Form
    {
        private readonly DataGridView dgv = new() { Dock = DockStyle.Fill };
        private readonly Label lblTotal = new() { Dock = DockStyle.Top, Padding = new Padding(0, 4, 0, 4), AutoSize = false, Height = 26 };
        private readonly Button btnVer = new() { Text = "Ver" };
        private readonly Button btnActualizar = new() { Text = "Actualizar" };
        private readonly LoteFinalControlDAL _dal = new();

        public FrmControlFinal()
        {
            Text = "Control Final";
            EstiloOscuro.Formulario(this);

            EstiloOscuro.Grilla(dgv);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.Columns.Add(Col("DsNombreLote", "Lote"));
            dgv.Columns.Add(Col("NuArchivos", "Archivos"));
            dgv.Columns.Add(Col("NuPendientes", "Pendientes"));
            dgv.Columns.Add(Col("NuFinalizados", "Finalizados"));
            dgv.Columns.Add(Col("FeAlta", "Fecha alta"));

            EstiloOscuro.Boton(btnVer);
            EstiloOscuro.Boton(btnActualizar);
            var barra = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(8), BackColor = EstiloOscuro.Fondo };
            barra.Controls.AddRange(new Control[] { btnVer, btnActualizar });

            var grupo = new GroupBox { Text = "Mis lotes asignados", Dock = DockStyle.Fill, Padding = new Padding(9, 24, 9, 8), ForeColor = Color.White, BackColor = EstiloOscuro.Fondo };
            grupo.Controls.Add(dgv);
            grupo.Controls.Add(lblTotal);

            Controls.Add(grupo);
            Controls.Add(barra);

            btnVer.Click += (_, _) => Abrir();
            btnActualizar.Click += (_, _) => Cargar();
            dgv.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) Abrir(); };
            Load += (_, _) => Cargar();
        }

        private static DataGridViewTextBoxColumn Col(string prop, string titulo) =>
            new() { DataPropertyName = prop, HeaderText = titulo, ReadOnly = true };

        private void Cargar()
        {
            try
            {
                var lotes = _dal.ObtenerLotesDeUsuario(SesionActual.UsuarioActual!.CdUsuario);
                dgv.DataSource = lotes;
                lblTotal.Text = $"Lotes asignados: {lotes.Count:N0}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar lotes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Abrir()
        {
            if (dgv.CurrentRow?.DataBoundItem is not LoteFinalDto lote)
            {
                MessageBox.Show("Seleccione un lote.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using var frm = new FrmControlFinalArchivos(lote);
            frm.ShowDialog(this);
            Cargar();
        }
    }
}
