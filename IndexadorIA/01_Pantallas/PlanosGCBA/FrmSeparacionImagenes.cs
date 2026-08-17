using IndexadorIA.Entidades;
using IndexadorIA.Negocio;
using IndexadorIA.Utilidades;
using System.ComponentModel;

namespace IndexadorIA.Pantallas.PlanosGCBA
{
    public partial class FrmSeparacionImagenes : Form
    {
        /// <summary>
        /// Referencia liviana a una página girada automáticamente con baja confianza,
        /// usada para poblar la pantalla de revisión manual de rotaciones al finalizar
        /// el procesamiento del lote.
        /// </summary>
        private class PaginaParaRevisionGiro
        {
            public ArchivoPagina ArchivoPaginaRef { get; set; } = null!;
            public string? RutaImagenTemporal { get; set; }
        }

        private readonly ArchivoOriginalBL _archivoOriginalBL;
        private readonly ArchivoPaginaBL _archivoPaginaBL;
        private List<ArchivoOriginal> _archivos;
        private int _ultimoIndiceSeleccionado = 0;

        /// <summary>
        /// Páginas giradas automáticamente con baja confianza en la detección de
        /// orientación, acumuladas durante el procesamiento del lote actual, para
        /// mostrarlas al usuario en la pantalla de revisión al finalizar.
        /// </summary>
        private readonly List<PaginaParaRevisionGiro> _paginasParaRevisionGiro = new();

        public FrmSeparacionImagenes()
        {
            InitializeComponent();
            _archivoOriginalBL = new ArchivoOriginalBL();
            _archivoPaginaBL = new ArchivoPaginaBL();
            _archivos = new List<ArchivoOriginal>();

            // Configurar estilos de la grilla
            ConfigurarGrilla();
        }

        private void ConfigurarGrilla()
        {
            dgvArchivos.BackgroundColor = Color.FromArgb(37, 37, 38);
            dgvArchivos.ForeColor = Color.White;
            dgvArchivos.GridColor = Color.FromArgb(63, 63, 70);
            dgvArchivos.DefaultCellStyle.BackColor = Color.FromArgb(37, 37, 38);
            dgvArchivos.DefaultCellStyle.ForeColor = Color.White;
            dgvArchivos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvArchivos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvArchivos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvArchivos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvArchivos.EnableHeadersVisualStyles = false;
        }

        private void FrmSeparacionImagenes_Load(object sender, EventArgs e)
        {
            CargarArchivos();
        }

        private void chkFiltroDesde_CheckedChanged(object sender, EventArgs e)
        {
            dtpFiltroDesde.Enabled = chkFiltroDesde.Checked;
        }

        private void chkFiltroHasta_CheckedChanged(object sender, EventArgs e)
        {
            dtpFiltroHasta.Enabled = chkFiltroHasta.Checked;
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime? feDesde = chkFiltroDesde.Checked ? dtpFiltroDesde.Value : (DateTime?)null;
                DateTime? feHasta = chkFiltroHasta.Checked ? dtpFiltroHasta.Value : (DateTime?)null;
                string dsCarpeta = txtFiltroCarpeta.Text;

                _archivos = ObtenerArchivosPendientes(feDesde, feHasta, dsCarpeta);
                MostrarArchivos();
                ActualizarContadores();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar archivos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarArchivos()
        {
            try
            {
                // Obtener todos los archivos con cdEstado = 1
                // Necesitamos crear un método en ArchivoOriginalDAL para esto
                _archivos = ObtenerArchivosPendientes();

                MostrarArchivos();
                ActualizarContadores();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar archivos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<ArchivoOriginal> ObtenerArchivosPendientes(DateTime? feDesde = null, DateTime? feHasta = null, string dsCarpeta = null)
        {
            // Consulta directa a la base de datos para obtener archivos con cdEstado = 1
            var archivos = new List<ArchivoOriginal>();

            using (var conexion = new Microsoft.Data.SqlClient.SqlConnection(Datos.Configuracion.CadenaConexion))
            {
                string filtroSql = "";
                if (feDesde.HasValue)
                {
                    filtroSql += " AND a.feAlta >= @feDesde";
                }
                if (feHasta.HasValue)
                {
                    filtroSql += " AND a.feAlta < @feHasta";
                }
                if (!string.IsNullOrWhiteSpace(dsCarpeta))
                {
                    filtroSql += " AND a.dsNombreUltimaCarpeta LIKE @dsCarpeta";
                }

                var comando = new Microsoft.Data.SqlClient.SqlCommand(
                    @"SELECT a.cdArchivo, a.cdProyecto, a.dsNombreArchivo, a.dsExtension, 
                             a.dsRutaCompleta, a.dsNombreUltimaCarpeta, a.nuCantidadPaginas,
                             a.dsProceso, a.cdEstadoArchivo, a.feAlta, a.cdUsuarioAlta,
                             a.nuTamanoBytes, a.feModificacionArchivo,
                             p.dsProyecto, e.dsEstado
                      FROM TD_ARCHIVOS_ORIGINAL a
                      INNER JOIN TD_PROYECTOS p ON a.cdProyecto = p.cdProyecto
                      INNER JOIN TD_ESTADOS e ON a.dsProceso = e.dsProceso AND a.cdEstadoArchivo = e.cdEstado
                      WHERE a.dsProceso = 'ARCHIVO_ORIGINAL' AND a.cdEstadoArchivo = 1" + filtroSql + @"
                      ORDER BY a.feAlta DESC", conexion);

                if (feDesde.HasValue)
                {
                    comando.Parameters.AddWithValue("@feDesde", feDesde.Value.Date);
                }
                if (feHasta.HasValue)
                {
                    comando.Parameters.AddWithValue("@feHasta", feHasta.Value.Date.AddDays(1));
                }
                if (!string.IsNullOrWhiteSpace(dsCarpeta))
                {
                    comando.Parameters.AddWithValue("@dsCarpeta", "%" + dsCarpeta + "%");
                }

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        archivos.Add(new ArchivoOriginal
                        {
                            CdArchivo = lector.GetInt32(0),
                            CdProyecto = lector.GetInt32(1),
                            DsNombreArchivo = lector.GetString(2),
                            DsExtension = lector.GetString(3),
                            DsRutaCompleta = lector.GetString(4),
                            DsNombreUltimaCarpeta = lector.IsDBNull(5) ? null : lector.GetString(5),
                            NuCantidadPaginas = lector.GetInt32(6),
                            DsProceso = lector.GetString(7),
                            CdEstadoArchivo = lector.GetInt32(8),
                            FeAlta = lector.GetDateTime(9),
                            CdUsuarioAlta = lector.GetInt32(10),
                            NuTamanoBytes = lector.GetInt64(11),
                            FeModificacionArchivo = lector.GetDateTime(12),
                            DsProyecto = lector.GetString(13),
                            DsEstado = lector.GetString(14)
                        });
                    }
                }
            }

            return archivos;
        }

        private void MostrarArchivos()
        {
            dgvArchivos.Columns.Clear();

            // Columna de selección
            var colSeleccion = new DataGridViewCheckBoxColumn
            {
                Name = "Seleccion",
                HeaderText = "Seleccionar",
                Width = 80,
                MinimumWidth = 80,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.False
            };
            dgvArchivos.Columns.Add(colSeleccion);

            // Columnas de datos
            dgvArchivos.Columns.Add("CdArchivo", "ID");
            dgvArchivos.Columns.Add("DsProyecto", "Proyecto");
            dgvArchivos.Columns.Add("DsNombreArchivo", "Nombre Archivo");
            dgvArchivos.Columns.Add("DsExtension", "Extensión");
            dgvArchivos.Columns.Add("NuCantidadPaginas", "Páginas");
            dgvArchivos.Columns.Add("DsEstado", "Estado");
            dgvArchivos.Columns.Add("DsNombreUltimaCarpeta", "Ultima Carpeta");
            dgvArchivos.Columns.Add("FeAlta", "Fecha Alta");
            dgvArchivos.Columns.Add("DsRutaCompleta", "Ruta Completa");

            // Ocultar columnas que no se muestran pero se necesitan
            dgvArchivos.Columns["CdArchivo"].Visible = false;
            dgvArchivos.Columns["DsRutaCompleta"].Visible = false;

            // Ajustar anchos
            dgvArchivos.Columns["DsProyecto"].Width = 150;
            dgvArchivos.Columns["DsNombreArchivo"].Width = 300;
            dgvArchivos.Columns["DsExtension"].Width = 80;
            dgvArchivos.Columns["NuCantidadPaginas"].Width = 80;
            dgvArchivos.Columns["DsEstado"].Width = 150;
            dgvArchivos.Columns["DsNombreUltimaCarpeta"].Width = 150;
            dgvArchivos.Columns["FeAlta"].Width = 130;

            // Llenar datos
            foreach (var archivo in _archivos)
            {
                dgvArchivos.Rows.Add(
                    false, // Checkbox desmarcado
                    archivo.CdArchivo,
                    archivo.DsProyecto,
                    archivo.DsNombreArchivo,
                    archivo.DsExtension,
                    archivo.NuCantidadPaginas,
                    archivo.DsEstado,
                    archivo.DsNombreUltimaCarpeta,
                    archivo.FeAlta,
                    archivo.DsRutaCompleta
                );
            }
        }

        private void btnSeleccionarTodo_Click(object sender, EventArgs e)
        {
            CambiarSeleccionTodos(true);
        }

        private void btnDeseleccionarTodo_Click(object sender, EventArgs e)
        {
            CambiarSeleccionTodos(false);
        }

        private void CambiarSeleccionTodos(bool seleccionar)
        {
            foreach (DataGridViewRow row in dgvArchivos.Rows)
            {
                row.Cells["Seleccion"].Value = seleccionar;
            }
            ActualizarContadores();
        }

        private void btnMarcar50_Click(object sender, EventArgs e)
        {
            int contador = 0;
            int indiceInicial = _ultimoIndiceSeleccionado;

            for (int i = indiceInicial; i < dgvArchivos.Rows.Count && contador < 50; i++)
            {
                var celda = dgvArchivos.Rows[i].Cells["Seleccion"];
                if (celda.Value == null || !(bool)celda.Value)
                {
                    celda.Value = true;
                    contador++;
                    _ultimoIndiceSeleccionado = i + 1;
                }
            }

            // Si llegamos al final, reiniciar
            if (_ultimoIndiceSeleccionado >= dgvArchivos.Rows.Count)
            {
                _ultimoIndiceSeleccionado = 0;
            }

            ActualizarContadores();
        }

        private void dgvArchivos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvArchivos.Columns["Seleccion"].Index && e.RowIndex >= 0)
            {
                dgvArchivos.CommitEdit(DataGridViewDataErrorContexts.Commit);
                ActualizarContadores();
            }
        }

        private void ActualizarContadores()
        {
            int total = dgvArchivos.Rows.Count;
            int seleccionados = 0;

            foreach (DataGridViewRow row in dgvArchivos.Rows)
            {
                if (row.Cells["Seleccion"].Value != null && (bool)row.Cells["Seleccion"].Value)
                {
                    seleccionados++;
                }
            }

            lblTotalRegistros.Text = $"Total de registros: {total}";
            lblRegistrosSeleccionados.Text = $"Registros seleccionados: {seleccionados}";
        }

        private async void btnProcesar_Click(object sender, EventArgs e)
        {
            // Validar que haya registros seleccionados
            var seleccionados = ObtenerArchivosSeleccionados();
            if (seleccionados.Count == 0)
            {
                MessageBox.Show("Debe seleccionar al menos un archivo para procesar.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar que Tesseract esté disponible si se eligió giro automático,
            // para no procesar el lote sin girar y sin que el usuario se entere.
            if (ObtenerModoGiro() == Utilidades.ModoGiro.Automatico && !Utilidades.ProcesamientoPaginas.EstaTesseractInstalado())
            {
                Datos.LogDAL.RegistrarLog(
                    Entidades.LogRegistro.Niveles.ERROR,
                    "FrmSeparacionImagenes.btnProcesar_Click",
                    "No se puede procesar con giro automático: Tesseract OCR no está instalado en el sistema.",
                    null,
                    SesionActual.UsuarioActual?.CdUsuario,
                    SesionActual.UsuarioActual?.DsUsuario);

                MessageBox.Show(
                    "No se encontró Tesseract OCR instalado en el sistema.\n\n" +
                    "El giro automático de páginas requiere Tesseract OCR para detectar la orientación del texto.\n" +
                    "Instale Tesseract OCR (o seleccione otro modo de giro) e intente nuevamente.",
                    "Error - Tesseract no instalado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Confirmar procesamiento
            var resultado = MessageBox.Show(
                $"¿Está seguro de procesar {seleccionados.Count} archivo(s)?\n\n" +
                $"Opciones seleccionadas:\n" +
                $"- Eliminar archivo original: {(chkEliminarOriginal.Checked ? "SÍ" : "NO")}\n" +
                $"- Giro: {DescripcionModoGiro()}\n" +
                $"- Detectar páginas blancas: {(chkMarcaBlanco.Checked ? "SÍ" : "NO")}",
                "Confirmar Procesamiento",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado != DialogResult.Yes)
                return;

            // Deshabilitar controles durante procesamiento
            HabilitarControles(false);

            try
            {
                await ProcesarArchivos(seleccionados);
            }
            finally
            {
                HabilitarControles(true);
            }
        }

        private List<ArchivoOriginal> ObtenerArchivosSeleccionados()
        {
            var seleccionados = new List<ArchivoOriginal>();

            foreach (DataGridViewRow row in dgvArchivos.Rows)
            {
                if (row.Cells["Seleccion"].Value != null && (bool)row.Cells["Seleccion"].Value)
                {
                    int cdArchivo = Convert.ToInt32(row.Cells["CdArchivo"].Value);
                    var archivo = _archivos.FirstOrDefault(a => a.CdArchivo == cdArchivo);
                    if (archivo != null)
                    {
                        seleccionados.Add(archivo);
                    }
                }
            }

            return seleccionados;
        }

        private async Task ProcesarArchivos(List<ArchivoOriginal> archivos)
        {
            progressBar.Value = 0;
            progressBar.Maximum = archivos.Count;

            int totalProcesados = 0;
            int totalErrores = 0;
            _paginasParaRevisionGiro.Clear();

            // Registrar inicio del procesamiento
            Datos.LogDAL.RegistrarLog(
                Entidades.LogRegistro.Niveles.INFO,
                "FrmSeparacionImagenes.ProcesarArchivos",
                $"Iniciando procesamiento de {archivos.Count} archivo(s)",
                null,
                SesionActual.UsuarioActual?.CdUsuario,
                SesionActual.UsuarioActual?.DsUsuario);

            foreach (var archivo in archivos)
            {
                lblProgreso.Text = $"Procesando: {archivo.DsNombreArchivo}...";
                Application.DoEvents();

                var resultado = await Task.Run(() => ProcesarArchivoIndividual(archivo));

                if (resultado.exito)
                {
                    totalProcesados++;

                    // Registrar éxito
                    Datos.LogDAL.RegistrarLog(
                        Entidades.LogRegistro.Niveles.INFO,
                        "FrmSeparacionImagenes.ProcesarArchivos",
                        $"Archivo procesado exitosamente: {archivo.DsNombreArchivo}",
                        null,
                        SesionActual.UsuarioActual?.CdUsuario,
                        SesionActual.UsuarioActual?.DsUsuario);
                }
                else
                {
                    totalErrores++;
                    lblProgreso.Text = $"Error en {archivo.DsNombreArchivo}: {resultado.mensaje}";

                    // Registrar error (ya se hizo en ProcesarArchivoIndividual, pero lo dejamos por consistencia)
                    Datos.LogDAL.RegistrarLog(
                        Entidades.LogRegistro.Niveles.ERROR,
                        "FrmSeparacionImagenes.ProcesarArchivos",
                        $"Error al procesar archivo: {archivo.DsNombreArchivo} (ID: {archivo.CdArchivo}) - {resultado.mensaje}",
                        null,
                        SesionActual.UsuarioActual?.CdUsuario,
                        SesionActual.UsuarioActual?.DsUsuario);

                    await Task.Delay(2000); // Mostrar error brevemente
                }

                progressBar.Value++;
            }

            lblProgreso.Text = $"Procesamiento completado. Exitosos: {totalProcesados}, Errores: {totalErrores}";

            // Registrar resumen final
            Datos.LogDAL.RegistrarLog(
                totalErrores > 0 ? Entidades.LogRegistro.Niveles.WARNING : Entidades.LogRegistro.Niveles.INFO,
                "FrmSeparacionImagenes.ProcesarArchivos",
                $"Procesamiento finalizado. Exitosos: {totalProcesados}, Errores: {totalErrores}",
                null,
                SesionActual.UsuarioActual?.CdUsuario,
                SesionActual.UsuarioActual?.DsUsuario);

            MessageBox.Show(
                $"Procesamiento completado.\n\nArchivos procesados: {totalProcesados}\nErrores: {totalErrores}",
                "Resultado",
                MessageBoxButtons.OK,
                totalErrores > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

            // Recargar grilla
            CargarArchivos();

            // Si hubo páginas giradas automáticamente con baja confianza, mostrar
            // la pantalla de revisión manual para que el usuario las corrija.
            if (_paginasParaRevisionGiro.Count > 0)
            {
                var paginasRevision = _paginasParaRevisionGiro
                    .Where(p => !string.IsNullOrEmpty(p.RutaImagenTemporal) && File.Exists(p.RutaImagenTemporal))
                    .Select(p => new FrmRevisionRotaciones.PaginaRevision
                    {
                        RutaPdf = p.ArchivoPaginaRef.DsRutaCompleta,
                        RutaImagen = p.RutaImagenTemporal!,
                        NombreArchivo = p.ArchivoPaginaRef.DsNombreArchivoPagina
                    })
                    .ToList();

                _paginasParaRevisionGiro.Clear();

                if (paginasRevision.Count > 0)
                {
                    using (var frmRevision = new FrmRevisionRotaciones(paginasRevision))
                    {
                        frmRevision.ShowDialog(this);
                    }
                }
            }
        }

        private (bool exito, string mensaje) ProcesarArchivoIndividual(ArchivoOriginal archivo)
        {
            try
            {
                string carpetaDestino = Path.GetDirectoryName(archivo.DsRutaCompleta) ?? "";
                ModoGiro modoGiro = ObtenerModoGiro();
                bool detectarBlancas = chkMarcaBlanco.Checked;
                bool eliminarOriginal = chkEliminarOriginal.Checked;

                // Validar que el archivo exista
                if (!File.Exists(archivo.DsRutaCompleta))
                {
                    string mensaje = $"El archivo no existe: {archivo.DsRutaCompleta}";
                    Datos.LogDAL.RegistrarLog(
                        Entidades.LogRegistro.Niveles.ERROR,
                        "FrmSeparacionImagenes.ProcesarArchivoIndividual",
                        mensaje,
                        null,
                        SesionActual.UsuarioActual?.CdUsuario,
                        SesionActual.UsuarioActual?.DsUsuario);
                    return (false, mensaje);
                }

                // Obtener secuencia inicial para este archivo
                int totalPaginas = archivo.NuCantidadPaginas;
                int secuenciaInicial = _archivoPaginaBL.ObtenerSiguienteSecuencia(totalPaginas);

                List<ArchivoPagina> paginas = new List<ArchivoPagina>();
                List<Rotacion> rotaciones = new List<Rotacion>();

                // Procesar según el tipo de archivo
                bool esPDF = archivo.DsExtension.ToLower() == "pdf";

                if (esPDF)
                {
                    // Separar páginas del PDF
                    var resultados = ProcesamientoPaginas.SepararPaginasPDF(
                        archivo.DsRutaCompleta,
                        carpetaDestino,
                        secuenciaInicial,
                        modoGiro,
                        detectarBlancas);

                    // Si no hay resultados, el archivo está corrupto o sin páginas
                    if (resultados.Count == 0)
                    {
                        string mensaje = $"El archivo PDF está corrupto, no tiene páginas válidas o no se pudo procesar";
                        Datos.LogDAL.RegistrarLog(
                            Entidades.LogRegistro.Niveles.WARNING,
                            "FrmSeparacionImagenes.ProcesarArchivoIndividual",
                            $"No se pudieron obtener páginas de: {archivo.DsNombreArchivo}",
                            null,
                            SesionActual.UsuarioActual?.CdUsuario,
                            SesionActual.UsuarioActual?.DsUsuario);
                        return (false, mensaje);
                    }

                    // Crear entidades de páginas y rotaciones
                    foreach (var resultado in resultados)
                    {
                        if (resultado.Exito)
                        {
                            var pagina = new ArchivoPagina
                            {
                                CdArchivoOriginal = archivo.CdArchivo,
                                NuPagina = resultado.NumeroPagina,
                                DsNombreArchivoPagina = resultado.NombreArchivo,
                                DsRutaCompleta = resultado.RutaCompleta,
                                SnGirada = resultado.SeGiro ? "SI" : "NO",
                                SnPosibleBlanca = resultado.EsPosibleBlanca ? "SI" : "NO",
                                CdEstado = 1, // Pendiente asignar lote
                                CdUsuarioAlta = SesionActual.UsuarioActual?.CdUsuario ?? 1
                            };

                            paginas.Add(pagina);

                            // SIEMPRE crear registro de rotación (estadística)
                            var rotacion = new Rotacion
                            {
                                SnRotacionAutomatica = resultado.SeGiro ? "SI" : "NO",
                                NuRotacionAplicada = resultado.SeGiro ? resultado.GradosRotacion : 0,
                                CdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 1
                            };
                            rotaciones.Add(rotacion);

                            if (resultado.BajaConfianzaGiro)
                            {
                                _paginasParaRevisionGiro.Add(new PaginaParaRevisionGiro
                                {
                                    ArchivoPaginaRef = pagina,
                                    RutaImagenTemporal = resultado.RutaImagenTemporalRevision
                                });
                            }
                        }
                    }
                }
                else
                {
                    // Procesar archivo JPG
                    var resultado = ProcesamientoPaginas.ProcesarArchivoJPG(
                        archivo.DsRutaCompleta,
                        carpetaDestino,
                        secuenciaInicial,
                        modoGiro,
                        detectarBlancas);

                    if (resultado.Exito)
                    {
                        var pagina = new ArchivoPagina
                        {
                            CdArchivoOriginal = archivo.CdArchivo,
                            NuPagina = 1,
                            DsNombreArchivoPagina = resultado.NombreArchivo,
                            DsRutaCompleta = resultado.RutaCompleta,
                            SnGirada = resultado.SeGiro ? "SI" : "NO",
                            SnPosibleBlanca = resultado.EsPosibleBlanca ? "SI" : "NO",
                            CdEstado = 1,
                            CdUsuarioAlta = SesionActual.UsuarioActual?.CdUsuario ?? 1
                        };

                        paginas.Add(pagina);

                        // SIEMPRE crear registro de rotación (estadística)
                        var rotacion = new Rotacion
                        {
                            SnRotacionAutomatica = resultado.SeGiro ? "SI" : "NO",
                            NuRotacionAplicada = resultado.SeGiro ? resultado.GradosRotacion : 0,
                            CdUsuario = SesionActual.UsuarioActual?.CdUsuario ?? 1
                        };
                        rotaciones.Add(rotacion);

                        if (resultado.BajaConfianzaGiro)
                        {
                            _paginasParaRevisionGiro.Add(new PaginaParaRevisionGiro
                            {
                                ArchivoPaginaRef = pagina,
                                RutaImagenTemporal = resultado.RutaImagenTemporalRevision
                            });
                        }
                    }
                }

                // Verificar si hay páginas válidas para guardar
                if (paginas.Count == 0)
                {
                    string mensaje = "No se pudo extraer ninguna página válida del archivo";
                    Datos.LogDAL.RegistrarLog(
                        Entidades.LogRegistro.Niveles.WARNING,
                        "FrmSeparacionImagenes.ProcesarArchivoIndividual",
                        $"No se pudo extraer ninguna página válida de: {archivo.DsNombreArchivo}",
                        null,
                        SesionActual.UsuarioActual?.CdUsuario,
                        SesionActual.UsuarioActual?.DsUsuario);
                    return (false, mensaje);
                }

                // Guardar páginas en la base de datos
                var resultadoGuardado = _archivoPaginaBL.GuardarLote(paginas, rotaciones);

                if (!resultadoGuardado.exito)
                {
                    Datos.LogDAL.RegistrarLog(
                        Entidades.LogRegistro.Niveles.ERROR,
                        "FrmSeparacionImagenes.ProcesarArchivoIndividual",
                        $"Error al guardar páginas en BD: {archivo.DsNombreArchivo} - {resultadoGuardado.mensaje}",
                        null,
                        SesionActual.UsuarioActual?.CdUsuario,
                        SesionActual.UsuarioActual?.DsUsuario);
                    return (false, resultadoGuardado.mensaje);
                }

                // Actualizar estado del archivo original a procesado (cdEstado = 2)
                ActualizarEstadoArchivoOriginal(archivo.CdArchivo, 2);

                // Eliminar archivo original si se solicitó
                if (eliminarOriginal && File.Exists(archivo.DsRutaCompleta))
                {
                    File.Delete(archivo.DsRutaCompleta);

                    // Registrar eliminación
                    Datos.LogDAL.RegistrarLog(
                        Entidades.LogRegistro.Niveles.INFO,
                        "FrmSeparacionImagenes.ProcesarArchivoIndividual",
                        $"Archivo original eliminado: {archivo.DsNombreArchivo}",
                        null,
                        SesionActual.UsuarioActual?.CdUsuario,
                        SesionActual.UsuarioActual?.DsUsuario);
                }

                return (true, "Procesado exitosamente");
            }
            catch (Exception ex)
            {
                // Registrar error detallado
                Datos.LogDAL.RegistrarLog(
                    Entidades.LogRegistro.Niveles.ERROR,
                    "FrmSeparacionImagenes.ProcesarArchivoIndividual",
                    $"Error al procesar archivo individual: {archivo.DsNombreArchivo} (Ruta: {archivo.DsRutaCompleta})",
                    ex,
                    SesionActual.UsuarioActual?.CdUsuario,
                    SesionActual.UsuarioActual?.DsUsuario);

                // NO re-lanzar, devolver false
                return (false, ex.Message);
            }
        }

        private void ActualizarEstadoArchivoOriginal(int cdArchivo, int nuevoEstado)
        {
            using (var conexion = new Microsoft.Data.SqlClient.SqlConnection(Datos.Configuracion.CadenaConexion))
            {
                var comando = new Microsoft.Data.SqlClient.SqlCommand(
                    @"UPDATE TD_ARCHIVOS_ORIGINAL 
                      SET cdEstadoArchivo = @nuevoEstado,
                          feUltimaModificacion = GETDATE(),
                          cdUsuarioModificacion = @cdUsuario
                      WHERE cdArchivo = @cdArchivo", conexion);

                comando.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
                comando.Parameters.AddWithValue("@cdUsuario", SesionActual.UsuarioActual?.CdUsuario ?? 1);
                comando.Parameters.AddWithValue("@cdArchivo", cdArchivo);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        private void HabilitarControles(bool habilitar)
        {
            btnSeleccionarTodo.Enabled = habilitar;
            btnDeseleccionarTodo.Enabled = habilitar;
            btnMarcar50.Enabled = habilitar;
            btnProcesar.Enabled = habilitar;
            dgvArchivos.Enabled = habilitar;
            chkEliminarOriginal.Enabled = habilitar;
            chkHabilitarGiro.Enabled = habilitar;
            rbGirarAutomatico.Enabled = habilitar && chkHabilitarGiro.Checked;
            rbGirar90Derecha.Enabled = habilitar && chkHabilitarGiro.Checked;
            rbGirar90Izquierda.Enabled = habilitar && chkHabilitarGiro.Checked;
            chkMarcaBlanco.Enabled = habilitar;
        }

        private void chkHabilitarGiro_CheckedChanged(object sender, EventArgs e)
        {
            bool habilitar = chkHabilitarGiro.Checked;
            rbGirarAutomatico.Enabled = habilitar;
            rbGirar90Derecha.Enabled = habilitar;
            rbGirar90Izquierda.Enabled = habilitar;
        }

        private ModoGiro ObtenerModoGiro()
        {
            if (!chkHabilitarGiro.Checked)
            {
                return ModoGiro.Ninguno;
            }

            if (rbGirar90Derecha.Checked)
            {
                return ModoGiro.Derecha90;
            }

            if (rbGirar90Izquierda.Checked)
            {
                return ModoGiro.Izquierda90;
            }

            return ModoGiro.Automatico;
        }

        private string DescripcionModoGiro()
        {
            switch (ObtenerModoGiro())
            {
                case ModoGiro.Automatico:
                    return "Autom\u00e1tico";
                case ModoGiro.Derecha90:
                    return "90\u00b0 a la derecha";
                case ModoGiro.Izquierda90:
                    return "90\u00b0 a la izquierda";
                default:
                    return "NO";
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
