using IndexadorIA.Entidades;
using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.Configuracion
{
    public partial class FrmProyectoDetalle : Form
    {
        private readonly ProyectoBL _proyectoBL;
        private Proyecto? _proyectoActual;

        public bool EsNuevo { get; private set; }

        public FrmProyectoDetalle(Proyecto? proyecto = null)
        {
            InitializeComponent();
            _proyectoBL = new ProyectoBL();
            _proyectoActual = proyecto;
            EsNuevo = proyecto == null;
        }

        private void FrmProyectoDetalle_Load(object sender, EventArgs e)
        {
            if (EsNuevo)
            {
                Text = "Nuevo Proyecto";
                lblTitulo.Text = "Nuevo Proyecto";
                chkActivo.Checked = true;
            }
            else
            {
                Text = "Editar Proyecto";
                lblTitulo.Text = "Editar Proyecto";
                CargarProyecto();
            }
        }

        private void CargarProyecto()
        {
            if (_proyectoActual != null)
            {
                txtNombre.Text = _proyectoActual.DsProyecto;
                chkActivo.Checked = _proyectoActual.SnActivo;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            try
            {
                (bool exito, string mensaje) resultado;

                if (EsNuevo)
                {
                    resultado = _proyectoBL.Crear(
                        txtNombre.Text.Trim(),
                        chkActivo.Checked,
                        SesionActual.UsuarioActual.CdUsuario
                    );
                }
                else
                {
                    resultado = _proyectoBL.Actualizar(
                        _proyectoActual!.CdProyecto,
                        txtNombre.Text.Trim(),
                        chkActivo.Checked,
                        SesionActual.UsuarioActual.CdUsuario
                    );
                }

                if (resultado.exito)
                {
                    MessageBox.Show(resultado.mensaje, "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(resultado.mensaje, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del proyecto es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
