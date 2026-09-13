using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using System.Windows.Forms.DataVisualization.Charting;

namespace IndexadorIA.Pantallas.Reportes
{
    /// <summary>
    /// Pantalla de reporte "Estado Proyecto Planos": muestra el avance general de
    /// digitalización y control de planos del proyecto de GCBA (cdProyecto = 1),
    /// incluyendo totales, estado de control, pendientes, asignaciones y gráficos.
    /// </summary>
    public partial class FrmEstadoProyecto : Form
    {
        private const int CdProyectoGCBA = 1;

        public FrmEstadoProyecto()
        {
            InitializeComponent();
        }

        private void FrmEstadoProyecto_Load(object sender, EventArgs e)
        {
            CargarEstadoProyecto();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarEstadoProyecto();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CargarEstadoProyecto()
        {
            try
            {
                var estadoProyectoDAL = new EstadoProyectoDAL();
                EstadoProyectoDto estado = estadoProyectoDAL.ObtenerEstadoProyecto(CdProyectoGCBA);

                lblTotalPlanosDelProyectoValor.Text = estado.TotalPlanosDelProyecto.ToString("N0");
                lblTotalPlanosPendientesDelProyectoValor.Text = estado.TotalPlanosPendientesDelProyecto.ToString("N0");

                lblTotalArchivosIngresadosValor.Text = estado.TotalArchivosIngresados.ToString("N0");
                lblTotalPaginasIngresadasValor.Text = estado.TotalPaginasIngresadas.ToString("N0");

                lblTotalPlanosRevisadosValor.Text = estado.TotalPlanosRevisados.ToString("N0");
                lblTotalPlanosControladosValor.Text = estado.TotalPlanosControlados.ToString("N0");
                lblTotalPlanosFaltaDatosValor.Text = estado.TotalPlanosFaltaDatos.ToString("N0");
                lblTotalPlanosIlegiblesValor.Text = estado.TotalPlanosIlegibles.ToString("N0");

                lblTotalPlanosPendientesIngresadosValor.Text = estado.TotalPlanosPendientesIngresados.ToString("N0");
                lblTotalPlanosPendientesProyectoValor.Text = estado.TotalPlanosPendientesProyecto.ToString("N0");

                lblTotalPlanosAsignadosPendienteControlValor.Text = estado.TotalPlanosAsignadosPendienteControl.ToString("N0");
                lblTotalPlanosSinAsignarControlValor.Text = estado.TotalPlanosSinAsignarControl.ToString("N0");

                ActualizarGraficos(estado);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el estado del proyecto: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarGraficos(EstadoProyectoDto estado)
        {
            chartEstadoTotal.Series[0].Points.Clear();
            chartEstadoTotal.Series[0].Points.AddXY("Planos Revisados", estado.TotalPlanosRevisados);
            chartEstadoTotal.Series[0].Points.AddXY("Planos Pendientes (Proyecto)", Math.Max(estado.TotalPlanosPendientesProyecto, 0));

            chartEstadoIngresado.Series[0].Points.Clear();
            chartEstadoIngresado.Series[0].Points.AddXY("Planos Pendientes (Ingresados)", Math.Max(estado.TotalPlanosPendientesIngresados, 0));
            chartEstadoIngresado.Series[0].Points.AddXY("Planos Revisados", estado.TotalPlanosRevisados);
        }
    }
}
