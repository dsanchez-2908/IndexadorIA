namespace IndexadorIA.Pantallas.Consultas
{
    /// <summary>
    /// Pantalla modal utilizada desde FrmConsultaPlanos para reubicar planos de m\u00faltiples
    /// lotes de origen hacia nuevos lotes (uno por cada lote de origen presente en la grilla),
    /// asignando una descripci\u00f3n com\u00fan a todos los lotes nuevos creados.
    /// </summary>
    public partial class FrmReubicarPlanosMultiple : Form
    {
        public int TotalLotes { get; }
        public int TotalPlanos { get; }
        public string DescripcionLote { get; private set; } = string.Empty;

        public FrmReubicarPlanosMultiple(int totalLotes, int totalPlanos)
        {
            InitializeComponent();
            TotalLotes = totalLotes;
            TotalPlanos = totalPlanos;
        }

        private void FrmReubicarPlanosMultiple_Load(object sender, EventArgs e)
        {
            lblTotalLotesValor.Text = TotalLotes.ToString();
            lblTotalPlanosValor.Text = TotalPlanos.ToString();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcionLote.Text))
            {
                MessageBox.Show("Debe ingresar una descripci\u00f3n de lote.",
                    "Validaci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DescripcionLote = txtDescripcionLote.Text.Trim();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
