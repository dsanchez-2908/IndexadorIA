using IndexadorIA.Negocio;

namespace IndexadorIA.Pantallas.FinalizacionPlanos
{
    /// <summary>
    /// Validador API SMP. Flujo en 3 pasos, cada uno con su VPN:
    /// 1) Descargar pendientes (OpenVPN / BD), 2) Validar contra la API (FortiClient),
    /// 3) Sincronizar resultados a la BD (OpenVPN).
    /// </summary>
    public class FrmValidadorApiSmp : Form
    {
        private readonly SmpValidacionBL _bl = new();
        private CancellationTokenSource? _cts;

        private readonly TextBox txtUrlBase = new() { Text = SmpApiClient.UrlBaseDefault, Width = 600 };
        private readonly Label lblTotal = new() { AutoSize = true };
        private readonly Label lblValidados = new() { AutoSize = true };
        private readonly Label lblPendientes = new() { AutoSize = true };
        private readonly Label lblOmitidos = new() { AutoSize = true };
        private readonly Label lblSinSincronizar = new() { AutoSize = true };
        private readonly Label lblEstado = new() { AutoSize = true, ForeColor = Color.DimGray };
        private readonly Button btnDescargar = new() { Text = "1. Descargar pendientes (VPN BD)", AutoSize = true };
        private readonly Button btnValidar = new() { Text = "2. Validar (VPN API)", AutoSize = true };
        private readonly Button btnSincronizar = new() { Text = "3. Sincronizar a BD (VPN BD)", AutoSize = true };
        private readonly Button btnCancelar = new() { Text = "Cancelar", AutoSize = true, Enabled = false };

        public FrmValidadorApiSmp()
        {
            Text = "Validador API SMP";
            Font = new Font("Segoe UI", 10F);
            BackColor = Color.White;

            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(20),
                AutoScroll = true
            };

            var botones = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
            botones.Controls.AddRange(new Control[] { btnDescargar, btnValidar, btnSincronizar, btnCancelar });

            panel.Controls.Add(new Label { Text = "URL BASE API:", AutoSize = true });
            panel.Controls.Add(txtUrlBase);
            panel.Controls.Add(new Label { Text = " ", AutoSize = true });
            panel.Controls.Add(lblTotal);
            panel.Controls.Add(lblValidados);
            panel.Controls.Add(lblPendientes);
            panel.Controls.Add(lblOmitidos);
            panel.Controls.Add(lblSinSincronizar);
            panel.Controls.Add(new Label { Text = " ", AutoSize = true });
            panel.Controls.Add(botones);
            panel.Controls.Add(new Label { Text = " ", AutoSize = true });
            panel.Controls.Add(lblEstado);
            Controls.Add(panel);

            btnDescargar.Click += async (_, _) => await EjecutarAsync("Descargando pendientes...",
                (p, ct) => Task.Run(() => $"Descarga finalizada: {_bl.Descargar(p, ct):N0} registros incorporados.", ct));
            btnValidar.Click += async (_, _) => await EjecutarAsync("Validando contra la API...",
                async (p, ct) => $"Validacion finalizada: {await _bl.ValidarAsync(txtUrlBase.Text.Trim(), p, ct):N0} registros consultados.");
            btnSincronizar.Click += async (_, _) => await EjecutarAsync("Sincronizando con la base de datos...",
                (p, ct) => Task.Run(() => $"Sincronizacion finalizada: {_bl.Sincronizar(p, ct):N0} registros subidos.", ct));
            btnCancelar.Click += (_, _) => _cts?.Cancel();

            Load += (_, _) => ActualizarContadores();
        }

        private void ActualizarContadores()
        {
            var c = _bl.ObtenerContadores();
            lblTotal.Text = $"Total de Planos: {c.Total:N0}";
            lblValidados.Text = $"Total de Planos Validados: {c.Validados:N0}";
            lblPendientes.Text = $"Total de Planos Pendientes: {c.Pendientes:N0}";
            lblOmitidos.Text = $"Omitidos (datos incompletos o invalidos): {c.Omitidos:N0}";
            lblSinSincronizar.Text = $"Pendientes de sincronizar a la BD: {c.SinSincronizar:N0}";
        }

        private async Task EjecutarAsync(string mensajeInicio, Func<IProgress<string>, CancellationToken, Task<string>> accion)
        {
            _cts = new CancellationTokenSource();
            HabilitarBotones(false);
            lblEstado.Text = mensajeInicio;

            var timer = new System.Windows.Forms.Timer { Interval = 2000 };
            timer.Tick += (_, _) => { try { ActualizarContadores(); } catch { } };
            timer.Start();

            var progreso = new Progress<string>(m => lblEstado.Text = m);
            try
            {
                lblEstado.Text = await accion(progreso, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                lblEstado.Text = "Proceso cancelado.";
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error.";
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                timer.Stop();
                timer.Dispose();
                _cts.Dispose();
                _cts = null;
                HabilitarBotones(true);
                ActualizarContadores();
            }
        }

        private void HabilitarBotones(bool habilitado)
        {
            btnDescargar.Enabled = habilitado;
            btnValidar.Enabled = habilitado;
            btnSincronizar.Enabled = habilitado;
            btnCancelar.Enabled = !habilitado;
        }
    }
}
