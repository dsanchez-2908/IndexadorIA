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
            groupBoxIngreso = new GroupBox();
            groupBoxControl = new GroupBox();
            groupBoxAuditoria = new GroupBox();
            groupBoxPendienteFinalizar = new GroupBox();
            groupBoxFinalizados = new GroupBox();
            groupBoxEnviados = new GroupBox();
            groupBoxGraficos = new GroupBox();
            chartEstadoTotal = new Chart();
            chartEstadoIngresado = new Chart();
            panelAcciones = new Panel();
            btnActualizar = new Button();
            btnCerrar = new Button();
            panelContenido.SuspendLayout();
            groupBoxGraficos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartEstadoTotal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartEstadoIngresado).BeginInit();
            panelAcciones.SuspendLayout();
            SuspendLayout();
            //
            // groupBoxTotalesProyecto
            //
            groupBoxTotalesProyecto.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxTotalesProyecto.ForeColor = Color.White;
            groupBoxTotalesProyecto.Location = new Point(0, 0);
            groupBoxTotalesProyecto.Name = "groupBoxTotalesProyecto";
            groupBoxTotalesProyecto.Size = new Size(1010, 100);
            groupBoxTotalesProyecto.TabStop = false;
            groupBoxTotalesProyecto.Text = "Total de Proyecto Planos de GCBA";
            //
            // groupBoxIngreso
            //
            groupBoxIngreso.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxIngreso.ForeColor = Color.White;
            groupBoxIngreso.Location = new Point(0, 108);
            groupBoxIngreso.Name = "groupBoxIngreso";
            groupBoxIngreso.Size = new Size(1010, 210);
            groupBoxIngreso.TabStop = false;
            groupBoxIngreso.Text = "Ingreso al sistema";
            //
            // groupBoxControl
            //
            groupBoxControl.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxControl.ForeColor = Color.White;
            groupBoxControl.Location = new Point(0, 326);
            groupBoxControl.Name = "groupBoxControl";
            groupBoxControl.Size = new Size(1010, 300);
            groupBoxControl.TabStop = false;
            groupBoxControl.Text = "Control de Planos";
            //
            // groupBoxAuditoria
            //
            groupBoxAuditoria.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxAuditoria.ForeColor = Color.White;
            groupBoxAuditoria.Location = new Point(0, 634);
            groupBoxAuditoria.Name = "groupBoxAuditoria";
            groupBoxAuditoria.Size = new Size(1010, 490);
            groupBoxAuditoria.TabStop = false;
            groupBoxAuditoria.Text = "Auditoria";
            //
            // groupBoxPendienteFinalizar
            //
            groupBoxPendienteFinalizar.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxPendienteFinalizar.ForeColor = Color.White;
            groupBoxPendienteFinalizar.Location = new Point(0, 1132);
            groupBoxPendienteFinalizar.Name = "groupBoxPendienteFinalizar";
            groupBoxPendienteFinalizar.Size = new Size(1010, 170);
            groupBoxPendienteFinalizar.TabStop = false;
            groupBoxPendienteFinalizar.Text = "Planos pendientes de Finalizar (Ya auditados)";
            //
            // groupBoxFinalizados
            //
            groupBoxFinalizados.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxFinalizados.ForeColor = Color.White;
            groupBoxFinalizados.Location = new Point(0, 1310);
            groupBoxFinalizados.Name = "groupBoxFinalizados";
            groupBoxFinalizados.Size = new Size(1010, 170);
            groupBoxFinalizados.TabStop = false;
            groupBoxFinalizados.Text = "Planos Finalizados (Listo para enviar)";
            //
            // groupBoxEnviados
            //
            groupBoxEnviados.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxEnviados.ForeColor = Color.White;
            groupBoxEnviados.Location = new Point(0, 1488);
            groupBoxEnviados.Name = "groupBoxEnviados";
            groupBoxEnviados.Size = new Size(1010, 170);
            groupBoxEnviados.TabStop = false;
            groupBoxEnviados.Text = "Planos Enviados";
            //
            // groupBoxGraficos
            //
            groupBoxGraficos.BackColor = Color.FromArgb(45, 45, 48);
            groupBoxGraficos.Controls.Add(chartEstadoTotal);
            groupBoxGraficos.Controls.Add(chartEstadoIngresado);
            groupBoxGraficos.ForeColor = Color.White;
            groupBoxGraficos.Location = new Point(0, 1666);
            groupBoxGraficos.Name = "groupBoxGraficos";
            groupBoxGraficos.Size = new Size(1010, 340);
            groupBoxGraficos.TabStop = false;
            groupBoxGraficos.Text = "Estado Global";
            //
            // chartEstadoTotal
            //
            ChartArea chartArea1 = new ChartArea();
            chartArea1.BackColor = Color.Transparent;
            chartArea1.Name = "AreaEstadoTotal";
            chartEstadoTotal.ChartAreas.Add(chartArea1);
            Legend legend1 = new Legend();
            legend1.ForeColor = Color.White;
            legend1.Name = "LeyendaEstadoTotal";
            chartEstadoTotal.Legends.Add(legend1);
            chartEstadoTotal.BackColor = Color.FromArgb(45, 45, 48);
            chartEstadoTotal.ForeColor = Color.White;
            chartEstadoTotal.Location = new Point(20, 30);
            chartEstadoTotal.Name = "chartEstadoTotal";
            Title title1 = new Title();
            title1.ForeColor = Color.White;
            title1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            title1.Name = "TituloEstadoTotal";
            title1.Text = "Estado Total del Proyecto";
            chartEstadoTotal.Titles.Add(title1);
            Series series1 = new Series();
            series1.ChartArea = "AreaEstadoTotal";
            series1.ChartType = SeriesChartType.Pie;
            series1.IsValueShownAsLabel = true;
            series1.Name = "SerieEstadoTotal";
            chartEstadoTotal.Series.Add(series1);
            chartEstadoTotal.Size = new Size(480, 300);
            chartEstadoTotal.TabIndex = 0;
            chartEstadoTotal.Text = "chartEstadoTotal";
            //
            // chartEstadoIngresado
            //
            ChartArea chartArea2 = new ChartArea();
            chartArea2.BackColor = Color.Transparent;
            chartArea2.Name = "AreaEstadoIngresado";
            chartEstadoIngresado.ChartAreas.Add(chartArea2);
            Legend legend2 = new Legend();
            legend2.ForeColor = Color.White;
            legend2.Name = "LeyendaEstadoIngresado";
            chartEstadoIngresado.Legends.Add(legend2);
            chartEstadoIngresado.BackColor = Color.FromArgb(45, 45, 48);
            chartEstadoIngresado.ForeColor = Color.White;
            chartEstadoIngresado.Location = new Point(510, 30);
            chartEstadoIngresado.Name = "chartEstadoIngresado";
            Title title2 = new Title();
            title2.ForeColor = Color.White;
            title2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            title2.Name = "TituloEstadoIngresado";
            title2.Text = "Estado de Planos (Sobre lo ingresado)";
            chartEstadoIngresado.Titles.Add(title2);
            Series series2 = new Series();
            series2.ChartArea = "AreaEstadoIngresado";
            series2.ChartType = SeriesChartType.Pie;
            series2.IsValueShownAsLabel = true;
            series2.Name = "SerieEstadoIngresado";
            chartEstadoIngresado.Series.Add(series2);
            chartEstadoIngresado.Size = new Size(480, 300);
            chartEstadoIngresado.TabIndex = 1;
            chartEstadoIngresado.Text = "chartEstadoIngresado";
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
            panelContenido.Controls.Add(groupBoxEnviados);
            panelContenido.Controls.Add(groupBoxFinalizados);
            panelContenido.Controls.Add(groupBoxPendienteFinalizar);
            panelContenido.Controls.Add(groupBoxAuditoria);
            panelContenido.Controls.Add(groupBoxControl);
            panelContenido.Controls.Add(groupBoxIngreso);
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
            panelContenido.ResumeLayout(false);
            groupBoxGraficos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartEstadoTotal).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartEstadoIngresado).EndInit();
            panelAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelContenido;
        private GroupBox groupBoxTotalesProyecto;
        private GroupBox groupBoxIngreso;
        private GroupBox groupBoxControl;
        private GroupBox groupBoxAuditoria;
        private GroupBox groupBoxPendienteFinalizar;
        private GroupBox groupBoxFinalizados;
        private GroupBox groupBoxEnviados;
        private GroupBox groupBoxGraficos;
        private Chart chartEstadoTotal;
        private Chart chartEstadoIngresado;
        private Panel panelAcciones;
        private Button btnActualizar;
        private Button btnCerrar;
    }
}
