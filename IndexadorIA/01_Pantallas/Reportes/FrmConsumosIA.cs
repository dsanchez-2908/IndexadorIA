using IndexadorIA.Datos;
using IndexadorIA.Entidades;

namespace IndexadorIA.Pantallas.Reportes
{
    /// <summary>
    /// Pantalla de reporte "Consumos IA": muestra un resumen general del consumo de tokens
    /// de OpenAI (tokens de ingreso/salida/total y costo en USD) y un detalle agrupado
    /// por mes y modelo de datos utilizado.
    /// </summary>
    public partial class FrmConsumosIA : Form
    {
        public FrmConsumosIA()
        {
            InitializeComponent();
        }

        private void FrmConsumosIA_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarDataGridView();
                CargarResumen();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvResumenMensual.AutoGenerateColumns = false;
            dgvResumenMensual.Columns.Clear();

            dgvResumenMensual.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsPeriodo",
                DataPropertyName = "DsPeriodo",
                HeaderText = "Mes",
                ReadOnly = true
            });

            dgvResumenMensual.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsModelo",
                DataPropertyName = "DsModelo",
                HeaderText = "Modelo",
                ReadOnly = true
            });

            dgvResumenMensual.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuTotalTokensPrompt",
                DataPropertyName = "NuTotalTokensPrompt",
                HeaderText = "Token Ingreso",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvResumenMensual.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuTotalTokensCompletion",
                DataPropertyName = "NuTotalTokensCompletion",
                HeaderText = "Token Salida",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvResumenMensual.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuTotalTokensTotal",
                DataPropertyName = "NuTotalTokensTotal",
                HeaderText = "Total Token",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvResumenMensual.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCostoIngresoUsd",
                DataPropertyName = "NuCostoIngresoUsd",
                HeaderText = "Precio Ingreso USD",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C4" }
            });

            dgvResumenMensual.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCostoSalidaUsd",
                DataPropertyName = "NuCostoSalidaUsd",
                HeaderText = "Precio Salida USD",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C4" }
            });

            dgvResumenMensual.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCostoTotalUsd",
                DataPropertyName = "NuCostoTotalUsd",
                HeaderText = "Total Consumido USD",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C4" }
            });

            dgvResumenMensual.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuTotalArchivos",
                DataPropertyName = "NuTotalArchivos",
                HeaderText = "Total Archivos",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
        }

        private void CargarResumen()
        {
            try
            {
                var tokenDAL = new TokenDAL();

                var resumenGeneral = tokenDAL.ObtenerResumenGeneral();
                MostrarResumenGeneral(resumenGeneral);

                var resumenPorMes = tokenDAL.ObtenerResumenPorMesYModelo();
                dgvResumenMensual.DataSource = null;
                dgvResumenMensual.DataSource = resumenPorMes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el resumen de consumo de tokens: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarResumenGeneral(ResumenConsumoTokenDto resumen)
        {
            lblTokenIngresoValor.Text = resumen.NuTotalTokensPrompt.ToString("N0");
            lblTokenSalidaValor.Text = resumen.NuTotalTokensCompletion.ToString("N0");
            lblTotalTokenValor.Text = resumen.NuTotalTokensTotal.ToString("N0");
            lblPrecioIngresoValor.Text = resumen.NuCostoIngresoUsd.ToString("C4");
            lblPrecioSalidaValor.Text = resumen.NuCostoSalidaUsd.ToString("C4");
            lblTotalConsumidoValor.Text = resumen.NuCostoTotalUsd.ToString("C4");
            lblTotalArchivosValor.Text = resumen.NuTotalArchivos.ToString("N0");
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarResumen();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
