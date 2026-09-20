using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Pantallas.Reportes
{
    /// <summary>
    /// Pantalla de reporte "Planos para enviar": muestra los lotes pendientes de
    /// finalizar, los lotes finalizados listos para enviar y el total de casos
    /// especiales registrados en toda la base. Acceso restringido al rol ADMIN.
    /// </summary>
    public partial class FrmPlanosParaEnviar : Form
    {
        public FrmPlanosParaEnviar()
        {
            InitializeComponent();
        }

        private void FrmPlanosParaEnviar_Load(object sender, EventArgs e)
        {
            CargarPlanosParaEnviar();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarPlanosParaEnviar();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CargarPlanosParaEnviar()
        {
            try
            {
                var planosParaEnviarDAL = new PlanosParaEnviarDAL();
                PlanosParaEnviarDto estado = planosParaEnviarDAL.ObtenerPlanosParaEnviar();

                lblTotalLotesPendienteFinalizarValor.Text = estado.TotalLotesPendienteFinalizar.ToString("N0");
                lblTotalPlanosCorrectosPendienteFinalizarValor.Text = estado.TotalPlanosCorrectosPendienteFinalizar.ToString("N0");
                lblTotalPlanosDatosIlegiblesPendienteFinalizarValor.Text = estado.TotalPlanosDatosIlegiblesPendienteFinalizar.ToString("N0");
                lblTotalPlanosIlegiblesPendienteFinalizarValor.Text = estado.TotalPlanosIlegiblesPendienteFinalizar.ToString("N0");
                lblTotalPlanosCasosEspecialesPendienteFinalizarValor.Text = estado.TotalPlanosCasosEspecialesPendienteFinalizar.ToString("N0");

                lblTotalLotesFinalizadosParaEnviarValor.Text = estado.TotalLotesFinalizadosParaEnviar.ToString("N0");
                lblTotalPlanosCorrectosFinalizadosValor.Text = estado.TotalPlanosCorrectosFinalizados.ToString("N0");
                lblTotalPlanosDatosIlegiblesFinalizadosValor.Text = estado.TotalPlanosDatosIlegiblesFinalizados.ToString("N0");
                lblTotalPlanosIlegiblesFinalizadosValor.Text = estado.TotalPlanosIlegiblesFinalizados.ToString("N0");

                lblTotalCasosEspecialesEnBaseValor.Text = estado.TotalCasosEspecialesEnBase.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el reporte de planos para enviar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
