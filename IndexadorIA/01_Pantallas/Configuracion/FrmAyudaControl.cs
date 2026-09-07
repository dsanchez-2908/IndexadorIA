using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio.Api;

namespace IndexadorIA.Pantallas.Configuracion
{
    /// <summary>
    /// Pantalla de administración (solo rol Administrador) para configurar el texto de
    /// ayuda que se muestra al usuario en FrmVerLote al presionar el botón "?" de cada
    /// uno de los 9 campos del panel de detalle. Funciona tanto en modo local (base de
    /// datos) como en modo remoto (API).
    /// </summary>
    public partial class FrmAyudaControl : Form
    {
        private readonly ApiClienteServicio? _apiCliente;

        public FrmAyudaControl()
        {
            InitializeComponent();
            _apiCliente = SesionApi.ModoRemoto ? new ApiClienteServicio() : null;
        }

        private void FrmAyudaControl_Load(object sender, EventArgs e)
        {
            try
            {
                CargarAyuda();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la ayuda de control: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarAyuda()
        {
            if (SesionApi.ModoRemoto)
            {
                var ayuda = _apiCliente!.ObtenerAyudaControlAsync().GetAwaiter().GetResult();
                txtCategoriaPlano.Text = ayuda.DsCategoriaPlano;
                txtTipoPlano.Text = ayuda.DsTipoPlano;
                txtExpediente.Text = ayuda.DsExpediente;
                txtSeccion.Text = ayuda.DsSeccion;
                txtManzana.Text = ayuda.DsManzana;
                txtParcela.Text = ayuda.DsParcela;
                txtDireccion.Text = ayuda.DsDireccion;
                txtNumeroPlano.Text = ayuda.DsNumeroPlano;
                txtObservaciones.Text = ayuda.DsObservaciones;
            }
            else
            {
                var ayudaControlDAL = new AyudaControlDAL();
                var ayuda = ayudaControlDAL.Obtener();
                txtCategoriaPlano.Text = ayuda?.DsCategoriaPlano;
                txtTipoPlano.Text = ayuda?.DsTipoPlano;
                txtExpediente.Text = ayuda?.DsExpediente;
                txtSeccion.Text = ayuda?.DsSeccion;
                txtManzana.Text = ayuda?.DsManzana;
                txtParcela.Text = ayuda?.DsParcela;
                txtDireccion.Text = ayuda?.DsDireccion;
                txtNumeroPlano.Text = ayuda?.DsNumeroPlano;
                txtObservaciones.Text = ayuda?.DsObservaciones;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (SesionApi.ModoRemoto)
                {
                    var ayudaApi = new AyudaControlApiDto
                    {
                        DsCategoriaPlano = txtCategoriaPlano.Text,
                        DsTipoPlano = txtTipoPlano.Text,
                        DsExpediente = txtExpediente.Text,
                        DsSeccion = txtSeccion.Text,
                        DsManzana = txtManzana.Text,
                        DsParcela = txtParcela.Text,
                        DsDireccion = txtDireccion.Text,
                        DsNumeroPlano = txtNumeroPlano.Text,
                        DsObservaciones = txtObservaciones.Text
                    };

                    _apiCliente!.GuardarAyudaControlAsync(ayudaApi).GetAwaiter().GetResult();
                }
                else
                {
                    var ayudaControlDAL = new AyudaControlDAL();
                    var ayuda = new AyudaControl
                    {
                        DsCategoriaPlano = txtCategoriaPlano.Text,
                        DsTipoPlano = txtTipoPlano.Text,
                        DsExpediente = txtExpediente.Text,
                        DsSeccion = txtSeccion.Text,
                        DsManzana = txtManzana.Text,
                        DsParcela = txtParcela.Text,
                        DsDireccion = txtDireccion.Text,
                        DsNumeroPlano = txtNumeroPlano.Text,
                        DsObservaciones = txtObservaciones.Text
                    };

                    ayudaControlDAL.Guardar(ayuda);
                }

                MessageBox.Show("Ayuda de control guardada correctamente.",
                    "Ayuda Control", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la ayuda de control: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
