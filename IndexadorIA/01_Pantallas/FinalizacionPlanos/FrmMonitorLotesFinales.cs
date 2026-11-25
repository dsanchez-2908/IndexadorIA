using IndexadorIA.Negocio.Api;
using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Pantallas.FinalizacionPlanos
{
    /// <summary>Monitor de Lotes Finales (solo ADMIN): resumen por usuario con acceso al detalle y reasignacion.</summary>
    public class FrmMonitorLotesFinales : Form
    {
        private readonly DataGridView dgv = new() { Dock = DockStyle.Fill };
        private readonly Label lblTotal = new() { Dock = DockStyle.Top, Height = 26, Padding = new Padding(0, 4, 0, 4) };
        private readonly Button btnVer = new() { Text = "Ver Detalle" };
        private readonly Button btnActualizar = new() { Text = "Actualizar" };
        private readonly LoteFinalControlDAL _dal = new();

        public FrmMonitorLotesFinales()
        {
            Text = "Monitor de Lotes Finales";
            EstiloOscuro.Formulario(this);

            EstiloOscuro.Grilla(dgv);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.Columns.Add(Col("DsUsuario", "Usuario"));
            dgv.Columns.Add(Col("NuLotes", "Total Lotes"));
            dgv.Columns.Add(Col("NuLotesAsignados", "Lotes Asignados"));
            dgv.Columns.Add(Col("NuArchivos", "Total Archivos"));
            dgv.Columns.Add(Col("NuPendientes", "Pendientes"));
            dgv.Columns.Add(Col("NuFinalizados", "Finalizados"));
            dgv.Columns.Add(Col("NuDevueltosSmp", "Devueltos SMP"));

            EstiloOscuro.Boton(btnVer);
            EstiloOscuro.Boton(btnActualizar);
            var barra = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(8), BackColor = EstiloOscuro.Fondo };
            barra.Controls.AddRange(new Control[] { btnVer, btnActualizar });

            var grupo = new GroupBox { Text = "Lotes finales por usuario", Dock = DockStyle.Fill, Padding = new Padding(9, 24, 9, 8), ForeColor = Color.White, BackColor = EstiloOscuro.Fondo };
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
                var datos = SesionApi.ModoRemoto
                    ? Task.Run(() => new IndexadorIA.Negocio.Api.ApiClienteServicio().ObtenerResumenMonitorFinalAsync()).GetAwaiter().GetResult()
                    : _dal.ObtenerResumenPorUsuario();
                dgv.DataSource = datos;
                lblTotal.Text = $"Total: {datos.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el resumen: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Abrir()
        {
            if (dgv.CurrentRow?.DataBoundItem is not MonitorLoteFinalUsuarioDto u)
            {
                MessageBox.Show("Seleccione un usuario para ver el detalle de sus lotes.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using var frm = new FrmMonitorLotesFinalesDetalle(u.CdUsuario, u.DsUsuario);
            frm.ShowDialog(this);
            Cargar();
        }
    }
}
