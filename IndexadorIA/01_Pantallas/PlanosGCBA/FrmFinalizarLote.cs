using IndexadorIA.Datos;
using IndexadorIA.Entidades;
using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    /// <summary>
    /// Pantalla de Finalización de lotes pendientes de finalizar (cdEstado=9, tras
    /// completarse la auditoría): permite renombrar/mover los PDF de página, generar
    /// los CSV de metadatos y marcar el lote como Finalizado / Pendiente de Enviar (cdEstado=10).
    /// </summary>
    public partial class FrmFinalizarLote : Form
    {
        private const int CD_ESTADO_PENDIENTE_FINALIZAR = 9;
        private List<LoteFinalizacionGridDto> _lotesActuales = new();
        private int _indiceSeleccion50 = 0;

        public FrmFinalizarLote()
        {
            InitializeComponent();
        }

        private void FrmFinalizarLote_Load(object sender, EventArgs e)
        {
            try
            {
                dtpFechaDesde.Value = DateTime.Now.AddDays(-30);
                dtpFechaHasta.Value = DateTime.Now;
                chkFiltrarFecha.Checked = false;
                dtpFechaDesde.Enabled = false;
                dtpFechaHasta.Enabled = false;

                progressBar.Visible = false;
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Value = 0;

                ConfigurarDataGridView();
                CargarLotes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvLotes.AutoGenerateColumns = false;
            dgvLotes.Columns.Clear();

            dgvLotes.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "snSeleccionado",
                DataPropertyName = "SnSeleccionado",
                HeaderText = string.Empty,
                Width = 40
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "cdLote",
                DataPropertyName = "CdLote",
                HeaderText = "ID Lote",
                Width = 70,
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsNombreLote",
                DataPropertyName = "DsNombreLote",
                HeaderText = "Lote",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsDescripcion",
                DataPropertyName = "DsDescripcion",
                HeaderText = "Descripción",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsEntrega",
                DataPropertyName = "DsEntrega",
                HeaderText = "Entrega",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsCarpeta",
                DataPropertyName = "DsCarpeta",
                HeaderText = "Carpeta",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadPlanos",
                DataPropertyName = "NuCantidadPlanos",
                HeaderText = "Cant. Planos",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadControlado",
                DataPropertyName = "NuCantidadControlado",
                HeaderText = "Controlado",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadDatosIlegibles",
                DataPropertyName = "NuCantidadDatosIlegibles",
                HeaderText = "Datos Ilegibles",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadPaginaIlegible",
                DataPropertyName = "NuCantidadPaginaIlegible",
                HeaderText = "Página Ilegible",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadCasosEspeciales",
                DataPropertyName = "NuCantidadCasosEspeciales",
                HeaderText = "Casos Especiales",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "nuCantidadParcelasMultiples",
                DataPropertyName = "NuCantidadParcelasMultiples",
                HeaderText = "Parcelas Multiples",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsUsuarioAsignado",
                DataPropertyName = "DsUsuarioAsignado",
                HeaderText = "Usuario Control",
                ReadOnly = true
            });

            dgvLotes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "dsUsuarioAuditor",
                DataPropertyName = "DsUsuarioAuditor",
                HeaderText = "Usuario Auditor",
                ReadOnly = true
            });
        }

        private void CargarLotes()
        {
            try
            {
                var loteDAL = new LoteDAL();

                string? dsNombreLote = string.IsNullOrWhiteSpace(txtNombreLote.Text) ? null : txtNombreLote.Text.Trim();
                DateTime? feControlDesde = chkFiltrarFecha.Checked ? dtpFechaDesde.Value : null;
                DateTime? feControlHasta = chkFiltrarFecha.Checked ? dtpFechaHasta.Value : null;

                _lotesActuales = loteDAL.ObtenerLotesPendientesFinalizarConEstadisticas(
                    dsNombreLote, feControlDesde, feControlHasta, CD_ESTADO_PENDIENTE_FINALIZAR);

                _indiceSeleccion50 = 0;

                dgvLotes.DataSource = null;
                dgvLotes.DataSource = _lotesActuales;

                ActualizarTotalizador();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar lotes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarTotalizador()
        {
            lblTotalizadorLotes.Text = $"Total: {_lotesActuales.Count}";
        }

        private void chkFiltrarFecha_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaDesde.Enabled = chkFiltrarFecha.Checked;
            dtpFechaHasta.Enabled = chkFiltrarFecha.Checked;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLotes();
        }

        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtNombreLote.Text = string.Empty;
            chkFiltrarFecha.Checked = false;
            dtpFechaDesde.Value = DateTime.Now.AddDays(-30);
            dtpFechaHasta.Value = DateTime.Now;
            CargarLotes();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSeleccionarTodo_Click(object sender, EventArgs e)
        {
            foreach (var lote in _lotesActuales)
            {
                lote.SnSeleccionado = true;
            }

            _indiceSeleccion50 = 0;
            dgvLotes.Refresh();
        }

        private void btnDeseleccionarTodo_Click(object sender, EventArgs e)
        {
            foreach (var lote in _lotesActuales)
            {
                lote.SnSeleccionado = false;
            }

            _indiceSeleccion50 = 0;
            dgvLotes.Refresh();
        }

        private void btnSeleccionar50_Click(object sender, EventArgs e)
        {
            if (_lotesActuales.Count == 0)
            {
                return;
            }

            if (_indiceSeleccion50 >= _lotesActuales.Count)
            {
                _indiceSeleccion50 = 0;
            }

            int cantidad = Math.Min(50, _lotesActuales.Count - _indiceSeleccion50);
            for (int i = 0; i < cantidad; i++)
            {
                _lotesActuales[_indiceSeleccion50 + i].SnSeleccionado = true;
            }

            _indiceSeleccion50 += cantidad;
            dgvLotes.Refresh();
        }

        private async void btnFinalizarLote_Click(object sender, EventArgs e)
        {
            var seleccionados = _lotesActuales.Where(l => l.SnSeleccionado).ToList();

            if (seleccionados.Count == 0)
            {
                MessageBox.Show("Seleccione al menos un lote para finalizar.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"Se procesarán {seleccionados.Count} lote(s): se renombrarán/moverán los archivos PDF, " +
                "se generarán los CSV de metadatos y los lotes quedarán marcados como Finalizados.\n\n" +
                "¿Desea continuar?",
                "Finalizar Lote", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            btnFinalizarLote.Enabled = false;
            btnBuscar.Enabled = false;
            progressBar.Visible = true;
            progressBar.Value = 0;
            progressBar.Maximum = seleccionados.Count;

            var loteDAL = new LoteDAL();
            var resultadoIADAL = new ResultadoIADAL();
            var loteFinalizacionBL = new LoteFinalizacionBL();
            int cdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 0;

            var lotesConError = new List<string>();
            var lotesConAdvertencia = new List<string>();

            try
            {
                foreach (var lote in seleccionados)
                {
                    try
                    {
                        var filas = await Task.Run(() => ObtenerFilasDelLote(loteDAL, resultadoIADAL, lote.CdLote));

                        if (filas.Count == 0)
                        {
                            lotesConError.Add($"Lote {lote.CdLote}: no tiene registros para procesar.");
                            continue;
                        }

                        // Control 1: campos obligatorios (solo aplica a registros Controlados)
                        var faltantes = loteFinalizacionBL.ValidarCamposObligatorios(filas);
                        if (faltantes.Count > 0)
                        {
                            lotesConError.Add($"Lote {lote.CdLote}: {faltantes.Count} registro(s) sin completar " +
                                "campos obligatorios (Categoría, Tipo de Plano, Sección, Manzana, Parcela, Dirección).");
                            continue;
                        }

                        // Control 2: advertir si quedan registros sin controlar (Pendiente de Control)
                        int cantidadPendientes = loteFinalizacionBL.ContarPendientesControl(filas);
                        if (cantidadPendientes > 0)
                        {
                            lotesConAdvertencia.Add($"Lote {lote.CdLote}: {cantidadPendientes} registro(s) " +
                                "sin controlar (Pendiente de Control); se finalizó de todas formas.");
                        }

                        // Control 3: archivos abiertos/bloqueados
                        var bloqueados = await Task.Run(() => loteFinalizacionBL.ValidarArchivosNoAbiertos(filas));
                        if (bloqueados.Count > 0)
                        {
                            lotesConError.Add($"Lote {lote.CdLote}: {bloqueados.Count} archivo(s) están abiertos " +
                                "o bloqueados y deben cerrarse antes de finalizar.");
                            continue;
                        }

                        await Task.Run(() => loteFinalizacionBL.FinalizarLote(lote.CdLote, filas, cdUsuario));
                    }
                    catch (Exception ex)
                    {
                        loteFinalizacionBL.RegistrarError("FrmFinalizarLote.btnFinalizarLote_Click", ex, cdUsuario);
                        lotesConError.Add($"Lote {lote.CdLote}: {ex.Message}");
                    }
                    finally
                    {
                        progressBar.Value = Math.Min(progressBar.Value + 1, progressBar.Maximum);
                    }
                }

                if (lotesConError.Count > 0 || lotesConAdvertencia.Count > 0)
                {
                    var mensajes = new List<string>();
                    if (lotesConError.Count > 0)
                        mensajes.AddRange(lotesConError);
                    if (lotesConAdvertencia.Count > 0)
                        mensajes.AddRange(lotesConAdvertencia);

                    MessageBox.Show(
                        "Se procesaron los lotes con las siguientes advertencias/errores:\n\n" +
                        string.Join("\n", mensajes),
                        "Finalización con observaciones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Los lotes seleccionados fueron finalizados correctamente.",
                        "Finalizar Lote", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarLotes();
            }
            finally
            {
                progressBar.Visible = false;
                btnFinalizarLote.Enabled = true;
                btnBuscar.Enabled = true;
            }
        }

        private static List<LoteFinalizacionBL.FilaFinalizacion> ObtenerFilasDelLote(
            LoteDAL loteDAL, ResultadoIADAL resultadoIADAL, int cdLote)
        {
            var resultados = resultadoIADAL.ObtenerPorLote(cdLote, mostrarTodos: true);
            var archivosPorPagina = loteDAL.ObtenerArchivosPaginasPorLote(cdLote)
                .ToDictionary(a => a.CdArchivoPagina, a => a);

            // Los registros marcados como Casos Especiales (cdEstadoControl = 5) se ignoran
            // en la finalización del lote; se tratarán mas adelante en una pantalla de
            // "Reubicar" a un nuevo lote para su tratamiento especial.
            return resultados
                .Where(r => r.CdEstadoControl != ResultadoIA.EstadosControl.CasosEspeciales)
                .Where(r => archivosPorPagina.ContainsKey(r.CdArchivoPagina))
                .Select(r => new LoteFinalizacionBL.FilaFinalizacion
                {
                    Archivo = archivosPorPagina[r.CdArchivoPagina],
                    Resultado = r
                }).ToList();
        }
    }
}
