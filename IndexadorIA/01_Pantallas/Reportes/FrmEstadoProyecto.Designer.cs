using System.Windows.Forms.DataVisualization.Charting;

namespace IndexadorIA.Pantallas.Reportes
{
    partial class FrmEstadoProyecto
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelContenido = new Panel();
            groupBoxTotalesProyecto = new GroupBox();
            lblTotalPlanosDelProyectoTitulo = new Label();
            lblTotalPlanosDelProyectoValor = new Label();
            lblTotalPlanosPendientesDelProyectoTitulo = new Label();
            lblTotalPlanosPendientesDelProyectoValor = new Label();
            groupBoxIngresados = new GroupBox();
            lblTotalArchivosIngresadosTitulo = new Label();
            lblTotalArchivosIngresadosValor = new Label();
            lblTotalPaginasIngresadasTitulo = new Label();
            lblTotalPaginasIngresadasValor = new Label();
            groupBoxEstadoControl = new GroupBox();
            lblTotalPlanosRevisadosTitulo = new Label();
            lblTotalPlanosRevisadosValor = new Label();
            lblTotalPlanosControladosTitulo = new Label();
            lblTotalPlanosControladosValor = new Label();
            lblTotalPlanosFaltaDatosTitulo = new Label();
            lblTotalPlanosFaltaDatosValor = new Label();
            lblTotalPlanosIlegiblesTitulo = new Label();
            lblTotalPlanosIlegiblesValor = new Label();
            groupBoxPendientes = new GroupBox();
            lblTotalPlanosPendientesIngresadosTitulo = new Label();
            lblTotalPlanosPendientesIngresadosValor = new Label();
            lblTotalPlanosPendientesProyectoTitulo = new Label();
            lblTotalPlanosPendientesProyectoValor = new Label();
            groupBoxAsignaciones = new GroupBox();
            lblTotalPlanosAsignadosPendienteControlTitulo = new Label();
            lblTotalPlanosAsignadosPendienteControlValor = new Label();
            lblTotalPlanosSinAsignarControlTitulo = new Label();
            lblTotalPlanosSinAsignarControlValor = new Label();
            groupBoxGraficos = new GroupBox();
            chartEstadoTotal = new Chart();
            chartEstadoIngresado = new Chart();
            panelAcciones = new Panel();
            btnActualizar = new Button();
            btnCerrar = new Button();
            groupBoxTotalesProyecto.SuspendLayout();
            groupBoxIngresados.SuspendLayout();
            groupBoxEstadoControl.SuspendLayout();
            groupBoxPendientes.SuspendLayout();
            groupBoxAsignaciones.SuspendLayout();
            groupBoxGraficos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartEstadoTotal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartEstadoIngresado).BeginInit();
            panelAcciones.SuspendLayout();
            panelContenido.SuspendLayout();
            SuspendLayout();
            //
            // groupBoxTotalesProyecto
            //
            groupBoxTotalesProyecto.Controls.Add(lblTotalPlanosDelProyectoTitulo);
            groupBoxTotalesProyecto.Controls.Add(lblTotalPlanosDelProyectoValor);
            groupBoxTotalesProyecto.Controls.Add(lblTotalPlanosPendientesDelProyectoTitulo);
            groupBoxTotalesProyecto.Controls.Add(lblTotalPlanosPendientesDelProyectoValor);
            groupBoxTotalesProyecto.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxTotalesProyecto.ForeColor = Color.White;
            groupBoxTotalesProyecto.Location = new Point(0, 0);
            groupBoxTotalesProyecto.Name = "groupBoxTotalesProyecto";
            groupBoxTotalesProyecto.Size = new Size(1010, 110);
            groupBoxTotalesProyecto.TabIndex = 0;
            groupBoxTotalesProyecto.TabStop = false;
            groupBoxTotalesProyecto.Text = "Total de Proyecto Planos de GCBA";
            //
            // lblTotalPlanosDelProyectoTitulo
            //
            lblTotalPlanosDelProyectoTitulo.ForeColor = Color.White;
            lblTotalPlanosDelProyectoTitulo.Location = new Point(20, 30);
            lblTotalPlanosDelProyectoTitulo.Name = "lblTotalPlanosDelProyectoTitulo";
            lblTotalPlanosDelProyectoTitulo.Size = new Size(460, 20);
            lblTotalPlanosDelProyectoTitulo.Text = "Total de Planos del Proyecto";
            //
            // lblTotalPlanosDelProyectoValor
            //
            lblTotalPlanosDelProyectoValor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotalPlanosDelProyectoValor.ForeColor = Color.FromArgb(0, 174, 239);
            lblTotalPlanosDelProyectoValor.Location = new Point(20, 55);
            lblTotalPlanosDelProyectoValor.Name = "lblTotalPlanosDelProyectoValor";
            lblTotalPlanosDelProyectoValor.Size = new Size(460, 35);
            lblTotalPlanosDelProyectoValor.Text = "0";
            //
            // lblTotalPlanosPendientesDelProyectoTitulo
            //
            lblTotalPlanosPendientesDelProyectoTitulo.ForeColor = Color.White;
            lblTotalPlanosPendientesDelProyectoTitulo.Location = new Point(520, 30);
            lblTotalPlanosPendientesDelProyectoTitulo.Name = "lblTotalPlanosPendientesDelProyectoTitulo";
            lblTotalPlanosPendientesDelProyectoTitulo.Size = new Size(460, 20);
            lblTotalPlanosPendientesDelProyectoTitulo.Text = "Total de Planos Pendientes del Proyecto";
            //
            // lblTotalPlanosPendientesDelProyectoValor
            //
            lblTotalPlanosPendientesDelProyectoValor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotalPlanosPendientesDelProyectoValor.ForeColor = Color.FromArgb(0, 174, 239);
            lblTotalPlanosPendientesDelProyectoValor.Location = new Point(520, 55);
            lblTotalPlanosPendientesDelProyectoValor.Name = "lblTotalPlanosPendientesDelProyectoValor";
            lblTotalPlanosPendientesDelProyectoValor.Size = new Size(460, 35);
            lblTotalPlanosPendientesDelProyectoValor.Text = "0";
            //
            // groupBoxIngresados
            //
            groupBoxIngresados.Controls.Add(lblTotalArchivosIngresadosTitulo);
            groupBoxIngresados.Controls.Add(lblTotalArchivosIngresadosValor);
            groupBoxIngresados.Controls.Add(lblTotalPaginasIngresadasTitulo);
            groupBoxIngresados.Controls.Add(lblTotalPaginasIngresadasValor);
            groupBoxIngresados.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxIngresados.ForeColor = Color.White;
            groupBoxIngresados.Location = new Point(0, 115);
            groupBoxIngresados.Name = "groupBoxIngresados";
            groupBoxIngresados.Size = new Size(1010, 110);
            groupBoxIngresados.TabIndex = 1;
            groupBoxIngresados.TabStop = false;
            groupBoxIngresados.Text = "Ingresados hasta el momento";
            //
            // lblTotalArchivosIngresadosTitulo
            //
            lblTotalArchivosIngresadosTitulo.ForeColor = Color.White;
            lblTotalArchivosIngresadosTitulo.Location = new Point(20, 30);
            lblTotalArchivosIngresadosTitulo.Name = "lblTotalArchivosIngresadosTitulo";
            lblTotalArchivosIngresadosTitulo.Size = new Size(460, 20);
            lblTotalArchivosIngresadosTitulo.Text = "Total de Archivos Ingresados";
            //
            // lblTotalArchivosIngresadosValor
            //
            lblTotalArchivosIngresadosValor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotalArchivosIngresadosValor.ForeColor = Color.FromArgb(0, 174, 239);
            lblTotalArchivosIngresadosValor.Location = new Point(20, 55);
            lblTotalArchivosIngresadosValor.Name = "lblTotalArchivosIngresadosValor";
            lblTotalArchivosIngresadosValor.Size = new Size(460, 35);
            lblTotalArchivosIngresadosValor.Text = "0";
            //
            // lblTotalPaginasIngresadasTitulo
            //
            lblTotalPaginasIngresadasTitulo.ForeColor = Color.White;
            lblTotalPaginasIngresadasTitulo.Location = new Point(520, 30);
            lblTotalPaginasIngresadasTitulo.Name = "lblTotalPaginasIngresadasTitulo";
            lblTotalPaginasIngresadasTitulo.Size = new Size(460, 20);
            lblTotalPaginasIngresadasTitulo.Text = "Total de Páginas/Planos Ingresados";
            //
            // lblTotalPaginasIngresadasValor
            //
            lblTotalPaginasIngresadasValor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotalPaginasIngresadasValor.ForeColor = Color.FromArgb(0, 174, 239);
            lblTotalPaginasIngresadasValor.Location = new Point(520, 55);
            lblTotalPaginasIngresadasValor.Name = "lblTotalPaginasIngresadasValor";
            lblTotalPaginasIngresadasValor.Size = new Size(460, 35);
            lblTotalPaginasIngresadasValor.Text = "0";
            //
            // groupBoxEstadoControl
            //
            groupBoxEstadoControl.Controls.Add(lblTotalPlanosRevisadosTitulo);
            groupBoxEstadoControl.Controls.Add(lblTotalPlanosRevisadosValor);
            groupBoxEstadoControl.Controls.Add(lblTotalPlanosControladosTitulo);
            groupBoxEstadoControl.Controls.Add(lblTotalPlanosControladosValor);
            groupBoxEstadoControl.Controls.Add(lblTotalPlanosFaltaDatosTitulo);
            groupBoxEstadoControl.Controls.Add(lblTotalPlanosFaltaDatosValor);
            groupBoxEstadoControl.Controls.Add(lblTotalPlanosIlegiblesTitulo);
            groupBoxEstadoControl.Controls.Add(lblTotalPlanosIlegiblesValor);
            groupBoxEstadoControl.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxEstadoControl.ForeColor = Color.White;
            groupBoxEstadoControl.Location = new Point(0, 230);
            groupBoxEstadoControl.Name = "groupBoxEstadoControl";
            groupBoxEstadoControl.Size = new Size(1010, 110);
            groupBoxEstadoControl.TabIndex = 2;
            groupBoxEstadoControl.TabStop = false;
            groupBoxEstadoControl.Text = "Estado de Control";
            //
            // lblTotalPlanosRevisadosTitulo
            //
            lblTotalPlanosRevisadosTitulo.ForeColor = Color.White;
            lblTotalPlanosRevisadosTitulo.Location = new Point(20, 30);
            lblTotalPlanosRevisadosTitulo.Name = "lblTotalPlanosRevisadosTitulo";
            lblTotalPlanosRevisadosTitulo.Size = new Size(230, 20);
            lblTotalPlanosRevisadosTitulo.Text = "Total de Planos Revisados";
            //
            // lblTotalPlanosRevisadosValor
            //
            lblTotalPlanosRevisadosValor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTotalPlanosRevisadosValor.ForeColor = Color.FromArgb(0, 174, 239);
            lblTotalPlanosRevisadosValor.Location = new Point(20, 55);
            lblTotalPlanosRevisadosValor.Name = "lblTotalPlanosRevisadosValor";
            lblTotalPlanosRevisadosValor.Size = new Size(230, 30);
            lblTotalPlanosRevisadosValor.Text = "0";
            //
            // lblTotalPlanosControladosTitulo
            //
            lblTotalPlanosControladosTitulo.ForeColor = Color.White;
            lblTotalPlanosControladosTitulo.Location = new Point(270, 30);
            lblTotalPlanosControladosTitulo.Name = "lblTotalPlanosControladosTitulo";
            lblTotalPlanosControladosTitulo.Size = new Size(230, 20);
            lblTotalPlanosControladosTitulo.Text = "Total de Planos Controlados";
            //
            // lblTotalPlanosControladosValor
            //
            lblTotalPlanosControladosValor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTotalPlanosControladosValor.ForeColor = Color.FromArgb(0, 174, 239);
            lblTotalPlanosControladosValor.Location = new Point(270, 55);
            lblTotalPlanosControladosValor.Name = "lblTotalPlanosControladosValor";
            lblTotalPlanosControladosValor.Size = new Size(230, 30);
            lblTotalPlanosControladosValor.Text = "0";
            //
            // lblTotalPlanosFaltaDatosTitulo
            //
            lblTotalPlanosFaltaDatosTitulo.ForeColor = Color.White;
            lblTotalPlanosFaltaDatosTitulo.Location = new Point(520, 30);
            lblTotalPlanosFaltaDatosTitulo.Name = "lblTotalPlanosFaltaDatosTitulo";
            lblTotalPlanosFaltaDatosTitulo.Size = new Size(230, 20);
            lblTotalPlanosFaltaDatosTitulo.Text = "Total de Planos Falta Datos";
            //
            // lblTotalPlanosFaltaDatosValor
            //
            lblTotalPlanosFaltaDatosValor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTotalPlanosFaltaDatosValor.ForeColor = Color.FromArgb(0, 174, 239);
            lblTotalPlanosFaltaDatosValor.Location = new Point(520, 55);
            lblTotalPlanosFaltaDatosValor.Name = "lblTotalPlanosFaltaDatosValor";
            lblTotalPlanosFaltaDatosValor.Size = new Size(230, 30);
            lblTotalPlanosFaltaDatosValor.Text = "0";
            //
            // lblTotalPlanosIlegiblesTitulo
            //
            lblTotalPlanosIlegiblesTitulo.ForeColor = Color.White;
            lblTotalPlanosIlegiblesTitulo.Location = new Point(770, 30);
            lblTotalPlanosIlegiblesTitulo.Name = "lblTotalPlanosIlegiblesTitulo";
            lblTotalPlanosIlegiblesTitulo.Size = new Size(220, 20);
            lblTotalPlanosIlegiblesTitulo.Text = "Total de Planos Ilegibles";
            //
            // lblTotalPlanosIlegiblesValor
            //
            lblTotalPlanosIlegiblesValor.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTotalPlanosIlegiblesValor.ForeColor = Color.FromArgb(0, 174, 239);
            lblTotalPlanosIlegiblesValor.Location = new Point(770, 55);
            lblTotalPlanosIlegiblesValor.Name = "lblTotalPlanosIlegiblesValor";
            lblTotalPlanosIlegiblesValor.Size = new Size(220, 30);
            lblTotalPlanosIlegiblesValor.Text = "0";
            //
            // groupBoxPendientes
            //
            groupBoxPendientes.Controls.Add(lblTotalPlanosPendientesIngresadosTitulo);
            groupBoxPendientes.Controls.Add(lblTotalPlanosPendientesIngresadosValor);
            groupBoxPendientes.Controls.Add(lblTotalPlanosPendientesProyectoTitulo);
            groupBoxPendientes.Controls.Add(lblTotalPlanosPendientesProyectoValor);
            groupBoxPendientes.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxPendientes.ForeColor = Color.White;
            groupBoxPendientes.Location = new Point(0, 345);
            groupBoxPendientes.Name = "groupBoxPendientes";
            groupBoxPendientes.Size = new Size(1010, 110);
            groupBoxPendientes.TabIndex = 3;
            groupBoxPendientes.TabStop = false;
            groupBoxPendientes.Text = "Pendientes de Control";
            //
            // lblTotalPlanosPendientesIngresadosTitulo
            //
            lblTotalPlanosPendientesIngresadosTitulo.ForeColor = Color.White;
            lblTotalPlanosPendientesIngresadosTitulo.Location = new Point(20, 30);
            lblTotalPlanosPendientesIngresadosTitulo.Name = "lblTotalPlanosPendientesIngresadosTitulo";
            lblTotalPlanosPendientesIngresadosTitulo.Size = new Size(460, 20);
            lblTotalPlanosPendientesIngresadosTitulo.Text = "Total de Planos Pendientes (Ingresados)";
            //
            // lblTotalPlanosPendientesIngresadosValor
            //
            lblTotalPlanosPendientesIngresadosValor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotalPlanosPendientesIngresadosValor.ForeColor = Color.FromArgb(0, 174, 239);
            lblTotalPlanosPendientesIngresadosValor.Location = new Point(20, 55);
            lblTotalPlanosPendientesIngresadosValor.Name = "lblTotalPlanosPendientesIngresadosValor";
            lblTotalPlanosPendientesIngresadosValor.Size = new Size(460, 35);
            lblTotalPlanosPendientesIngresadosValor.Text = "0";
            //
            // lblTotalPlanosPendientesProyectoTitulo
            //
            lblTotalPlanosPendientesProyectoTitulo.ForeColor = Color.White;
            lblTotalPlanosPendientesProyectoTitulo.Location = new Point(520, 30);
            lblTotalPlanosPendientesProyectoTitulo.Name = "lblTotalPlanosPendientesProyectoTitulo";
            lblTotalPlanosPendientesProyectoTitulo.Size = new Size(460, 20);
            lblTotalPlanosPendientesProyectoTitulo.Text = "Total de Planos Pendientes (Proyecto)";
            //
            // lblTotalPlanosPendientesProyectoValor
            //
            lblTotalPlanosPendientesProyectoValor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotalPlanosPendientesProyectoValor.ForeColor = Color.FromArgb(0, 174, 239);
            lblTotalPlanosPendientesProyectoValor.Location = new Point(520, 55);
            lblTotalPlanosPendientesProyectoValor.Name = "lblTotalPlanosPendientesProyectoValor";
            lblTotalPlanosPendientesProyectoValor.Size = new Size(460, 35);
            lblTotalPlanosPendientesProyectoValor.Text = "0";
            //
            // groupBoxAsignaciones
            //
            groupBoxAsignaciones.Controls.Add(lblTotalPlanosAsignadosPendienteControlTitulo);
            groupBoxAsignaciones.Controls.Add(lblTotalPlanosAsignadosPendienteControlValor);
            groupBoxAsignaciones.Controls.Add(lblTotalPlanosSinAsignarControlTitulo);
            groupBoxAsignaciones.Controls.Add(lblTotalPlanosSinAsignarControlValor);
            groupBoxAsignaciones.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxAsignaciones.ForeColor = Color.White;
            groupBoxAsignaciones.Location = new Point(0, 460);
            groupBoxAsignaciones.Name = "groupBoxAsignaciones";
            groupBoxAsignaciones.Size = new Size(1010, 110);
            groupBoxAsignaciones.TabIndex = 4;
            groupBoxAsignaciones.TabStop = false;
            groupBoxAsignaciones.Text = "Asignaciones";
            //
            // lblTotalPlanosAsignadosPendienteControlTitulo
            //
            lblTotalPlanosAsignadosPendienteControlTitulo.ForeColor = Color.White;
            lblTotalPlanosAsignadosPendienteControlTitulo.Location = new Point(20, 30);
            lblTotalPlanosAsignadosPendienteControlTitulo.Name = "lblTotalPlanosAsignadosPendienteControlTitulo";
            lblTotalPlanosAsignadosPendienteControlTitulo.Size = new Size(460, 20);
            lblTotalPlanosAsignadosPendienteControlTitulo.Text = "Total de Planos Asignados y Pendiente de Control";
            //
            // lblTotalPlanosAsignadosPendienteControlValor
            //
            lblTotalPlanosAsignadosPendienteControlValor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotalPlanosAsignadosPendienteControlValor.ForeColor = Color.FromArgb(0, 174, 239);
            lblTotalPlanosAsignadosPendienteControlValor.Location = new Point(20, 55);
            lblTotalPlanosAsignadosPendienteControlValor.Name = "lblTotalPlanosAsignadosPendienteControlValor";
            lblTotalPlanosAsignadosPendienteControlValor.Size = new Size(460, 35);
            lblTotalPlanosAsignadosPendienteControlValor.Text = "0";
            //
            // lblTotalPlanosSinAsignarControlTitulo
            //
            lblTotalPlanosSinAsignarControlTitulo.ForeColor = Color.White;
            lblTotalPlanosSinAsignarControlTitulo.Location = new Point(520, 30);
            lblTotalPlanosSinAsignarControlTitulo.Name = "lblTotalPlanosSinAsignarControlTitulo";
            lblTotalPlanosSinAsignarControlTitulo.Size = new Size(460, 20);
            lblTotalPlanosSinAsignarControlTitulo.Text = "Total de Planos sin Asignar Control";
            //
            // lblTotalPlanosSinAsignarControlValor
            //
            lblTotalPlanosSinAsignarControlValor.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotalPlanosSinAsignarControlValor.ForeColor = Color.FromArgb(0, 174, 239);
            lblTotalPlanosSinAsignarControlValor.Location = new Point(520, 55);
            lblTotalPlanosSinAsignarControlValor.Name = "lblTotalPlanosSinAsignarControlValor";
            lblTotalPlanosSinAsignarControlValor.Size = new Size(460, 35);
            lblTotalPlanosSinAsignarControlValor.Text = "0";
            //
            // groupBoxGraficos
            //
            groupBoxGraficos.Controls.Add(chartEstadoTotal);
            groupBoxGraficos.Controls.Add(chartEstadoIngresado);
            groupBoxGraficos.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxGraficos.ForeColor = Color.White;
            groupBoxGraficos.Location = new Point(0, 575);
            groupBoxGraficos.Name = "groupBoxGraficos";
            groupBoxGraficos.Size = new Size(1010, 340);
            groupBoxGraficos.TabIndex = 5;
            groupBoxGraficos.TabStop = false;
            groupBoxGraficos.Text = "Gráficos";
            //
            // chartEstadoTotal
            //
            {
                ChartArea chartAreaTotal = new ChartArea("ChartAreaEstadoTotal");
                chartEstadoTotal.ChartAreas.Add(chartAreaTotal);
                Legend legendTotal = new Legend("LegendEstadoTotal");
                chartEstadoTotal.Legends.Add(legendTotal);
                Title tituloTotal = new Title("Estado Total del Proyecto")
                {
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold)
                };
                chartEstadoTotal.Titles.Add(tituloTotal);
                Series serieTotal = new Series("SerieEstadoTotal")
                {
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = true
                };
                chartEstadoTotal.Series.Add(serieTotal);
            }
            chartEstadoTotal.BackColor = Color.FromArgb(45, 45, 48);
            chartEstadoTotal.ForeColor = Color.White;
            chartEstadoTotal.Location = new Point(10, 30);
            chartEstadoTotal.Name = "chartEstadoTotal";
            chartEstadoTotal.Size = new Size(480, 300);
            chartEstadoTotal.TabIndex = 0;
            //
            // chartEstadoIngresado
            //
            {
                ChartArea chartAreaIngresado = new ChartArea("ChartAreaEstadoIngresado");
                chartEstadoIngresado.ChartAreas.Add(chartAreaIngresado);
                Legend legendIngresado = new Legend("LegendEstadoIngresado");
                chartEstadoIngresado.Legends.Add(legendIngresado);
                Title tituloIngresado = new Title("Estado de Planos sobre lo ingresado")
                {
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold)
                };
                chartEstadoIngresado.Titles.Add(tituloIngresado);
                Series serieIngresado = new Series("SerieEstadoIngresado")
                {
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = true
                };
                chartEstadoIngresado.Series.Add(serieIngresado);
            }
            chartEstadoIngresado.BackColor = Color.FromArgb(45, 45, 48);
            chartEstadoIngresado.ForeColor = Color.White;
            chartEstadoIngresado.Location = new Point(510, 30);
            chartEstadoIngresado.Name = "chartEstadoIngresado";
            chartEstadoIngresado.Size = new Size(480, 300);
            chartEstadoIngresado.TabIndex = 1;
            //
            // panelAcciones
            //
            panelAcciones.Controls.Add(btnActualizar);
            panelAcciones.Controls.Add(btnCerrar);
            panelAcciones.BackColor = Color.FromArgb(45, 45, 48);
            panelAcciones.Dock = DockStyle.Bottom;
            panelAcciones.Location = new Point(0, 620);
            panelAcciones.Name = "panelAcciones";
            panelAcciones.Size = new Size(1010, 46);
            panelAcciones.TabIndex = 1;
            //
            // btnActualizar
            //
            btnActualizar.BackColor = Color.FromArgb(0, 122, 204);
            btnActualizar.ForeColor = Color.White;
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.Location = new Point(18, 8);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(150, 30);
            btnActualizar.TabIndex = 0;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            btnActualizar.Click += btnActualizar_Click;
            //
            // btnCerrar
            //
            btnCerrar.BackColor = Color.FromArgb(96, 96, 96);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Location = new Point(176, 8);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(150, 30);
            btnCerrar.TabIndex = 1;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            //
            // panelContenido
            //
            panelContenido.AutoScroll = true;
            panelContenido.BackColor = Color.FromArgb(32, 32, 32);
            panelContenido.Controls.Add(groupBoxGraficos);
            panelContenido.Controls.Add(groupBoxAsignaciones);
            panelContenido.Controls.Add(groupBoxPendientes);
            panelContenido.Controls.Add(groupBoxEstadoControl);
            panelContenido.Controls.Add(groupBoxIngresados);
            panelContenido.Controls.Add(groupBoxTotalesProyecto);
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.Location = new Point(0, 0);
            panelContenido.Name = "panelContenido";
            panelContenido.Size = new Size(1010, 620);
            panelContenido.TabIndex = 0;
            //
            // FrmEstadoProyecto
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(1010, 666);
            Controls.Add(panelContenido);
            Controls.Add(panelAcciones);
            Name = "FrmEstadoProyecto";
            Text = "Estado Proyecto Planos";
            Load += FrmEstadoProyecto_Load;
            groupBoxTotalesProyecto.ResumeLayout(false);
            groupBoxIngresados.ResumeLayout(false);
            groupBoxEstadoControl.ResumeLayout(false);
            groupBoxPendientes.ResumeLayout(false);
            groupBoxAsignaciones.ResumeLayout(false);
            groupBoxGraficos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartEstadoTotal).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartEstadoIngresado).EndInit();
            panelAcciones.ResumeLayout(false);
            panelContenido.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelContenido;
        private GroupBox groupBoxTotalesProyecto;
        private Label lblTotalPlanosDelProyectoTitulo;
        private Label lblTotalPlanosDelProyectoValor;
        private Label lblTotalPlanosPendientesDelProyectoTitulo;
        private Label lblTotalPlanosPendientesDelProyectoValor;
        private GroupBox groupBoxIngresados;
        private Label lblTotalArchivosIngresadosTitulo;
        private Label lblTotalArchivosIngresadosValor;
        private Label lblTotalPaginasIngresadasTitulo;
        private Label lblTotalPaginasIngresadasValor;
        private GroupBox groupBoxEstadoControl;
        private Label lblTotalPlanosRevisadosTitulo;
        private Label lblTotalPlanosRevisadosValor;
        private Label lblTotalPlanosControladosTitulo;
        private Label lblTotalPlanosControladosValor;
        private Label lblTotalPlanosFaltaDatosTitulo;
        private Label lblTotalPlanosFaltaDatosValor;
        private Label lblTotalPlanosIlegiblesTitulo;
        private Label lblTotalPlanosIlegiblesValor;
        private GroupBox groupBoxPendientes;
        private Label lblTotalPlanosPendientesIngresadosTitulo;
        private Label lblTotalPlanosPendientesIngresadosValor;
        private Label lblTotalPlanosPendientesProyectoTitulo;
        private Label lblTotalPlanosPendientesProyectoValor;
        private GroupBox groupBoxAsignaciones;
        private Label lblTotalPlanosAsignadosPendienteControlTitulo;
        private Label lblTotalPlanosAsignadosPendienteControlValor;
        private Label lblTotalPlanosSinAsignarControlTitulo;
        private Label lblTotalPlanosSinAsignarControlValor;
        private GroupBox groupBoxGraficos;
        private Chart chartEstadoTotal;
        private Chart chartEstadoIngresado;
        private Panel panelAcciones;
        private Button btnActualizar;
        private Button btnCerrar;
    }
}
