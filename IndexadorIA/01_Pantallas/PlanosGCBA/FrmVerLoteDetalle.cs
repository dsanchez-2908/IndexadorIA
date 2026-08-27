using IndexadorIA.Datos;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    /// <summary>
    /// Pantalla de solo lectura para ver el detalle de los registros de un lote
    /// (VW_001_RESULTADO_IA), utilizada desde la pantalla de Asignación de Lotes
    /// para inspeccionar la calidad de los datos antes de asignar.
    /// </summary>
    public partial class FrmVerLoteDetalle : Form
    {
        private readonly int _cdLote;
        private readonly string _dsNombreLote;

        public FrmVerLoteDetalle(int cdLote, string dsNombreLote)
        {
            InitializeComponent();
            _cdLote = cdLote;
            _dsNombreLote = dsNombreLote;
        }

        private void FrmVerLoteDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                Text = $"Detalle del Lote - {_dsNombreLote}";
                ConfigurarDataGridView();
                CargarDetalle();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el detalle del lote: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvDetalle.AutoGenerateColumns = false;
            dgvDetalle.Columns.Clear();

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsCategoriaPlano",
                DataPropertyName = "DsCategoriaPlano",
                HeaderText = "Categoría Plano"
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsTipoPlano",
                DataPropertyName = "DsTipoPlano",
                HeaderText = "Tipo Plano"
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsAcronimo",
                DataPropertyName = "DsAcronimo",
                HeaderText = "Acrónimo"
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsDireccion",
                DataPropertyName = "DsDireccion",
                HeaderText = "Dirección"
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsSeccion",
                DataPropertyName = "DsSeccion",
                HeaderText = "Sección"
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsManzana",
                DataPropertyName = "DsManzana",
                HeaderText = "Manzana"
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsParcela",
                DataPropertyName = "DsParcela",
                HeaderText = "Parcela"
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsExpediente",
                DataPropertyName = "DsExpediente",
                HeaderText = "Expediente"
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsNumeroPlano",
                DataPropertyName = "DsNumeroPlano",
                HeaderText = "Número Plano"
            });
        }

        private void CargarDetalle()
        {
            var loteDAL = new LoteDAL();
            var detalle = loteDAL.ObtenerDetalleLoteParaVer(_cdLote);

            dgvDetalle.DataSource = null;
            dgvDetalle.DataSource = detalle;

            lblTotalizadorDetalle.Text = $"Total: {detalle.Count}";
        }

        private void btnCerrarDetalle_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
