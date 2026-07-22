namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmEditorPrompt
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

        private void InitializeComponent()
        {
            groupBoxPrompt = new GroupBox();
            txtPrompt = new TextBox();
            lblInfo = new Label();
            groupBoxAcciones = new GroupBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            lblUltimaModificacion = new Label();
            groupBoxPrompt.SuspendLayout();
            groupBoxAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxPrompt
            // 
            groupBoxPrompt.Controls.Add(txtPrompt);
            groupBoxPrompt.Controls.Add(lblInfo);
            groupBoxPrompt.Dock = DockStyle.Fill;
            groupBoxPrompt.Location = new Point(13, 11);
            groupBoxPrompt.Margin = new Padding(3, 2, 3, 2);
            groupBoxPrompt.Name = "groupBoxPrompt";
            groupBoxPrompt.Padding = new Padding(9, 8, 9, 8);
            groupBoxPrompt.Size = new Size(674, 338);
            groupBoxPrompt.TabIndex = 0;
            groupBoxPrompt.TabStop = false;
            groupBoxPrompt.Text = "Prompt para Procesamiento IA";
            // 
            // txtPrompt
            // 
            txtPrompt.AcceptsReturn = true;
            txtPrompt.Dock = DockStyle.Fill;
            txtPrompt.Font = new Font("Consolas", 10F);
            txtPrompt.Location = new Point(9, 69);
            txtPrompt.Margin = new Padding(3, 2, 3, 2);
            txtPrompt.Multiline = true;
            txtPrompt.Name = "txtPrompt";
            txtPrompt.ScrollBars = ScrollBars.Both;
            txtPrompt.Size = new Size(656, 261);
            txtPrompt.TabIndex = 1;
            // 
            // lblInfo
            // 
            lblInfo.Dock = DockStyle.Top;
            lblInfo.Location = new Point(9, 24);
            lblInfo.Name = "lblInfo";
            lblInfo.Padding = new Padding(4, 4, 4, 4);
            lblInfo.Size = new Size(656, 45);
            lblInfo.TabIndex = 0;
            lblInfo.Text = "Edite el prompt que se utilizará para enviar las imágenes a OpenAI.\r\n Variables disponibles: {tipoPlano} - Tipo de plano según el código detectado";
            // 
            // groupBoxAcciones
            // 
            groupBoxAcciones.Controls.Add(btnGuardar);
            groupBoxAcciones.Controls.Add(btnCancelar);
            groupBoxAcciones.Controls.Add(lblUltimaModificacion);
            groupBoxAcciones.Dock = DockStyle.Bottom;
            groupBoxAcciones.Location = new Point(13, 349);
            groupBoxAcciones.Margin = new Padding(3, 2, 3, 2);
            groupBoxAcciones.Name = "groupBoxAcciones";
            groupBoxAcciones.Padding = new Padding(3, 2, 3, 2);
            groupBoxAcciones.Size = new Size(674, 68);
            groupBoxAcciones.TabIndex = 1;
            groupBoxAcciones.TabStop = false;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(455, 22);
            btnGuardar.Margin = new Padding(3, 2, 3, 2);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(105, 30);
            btnGuardar.TabIndex = 1;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(569, 22);
            btnCancelar.Margin = new Padding(3, 2, 3, 2);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(105, 30);
            btnCancelar.TabIndex = 2;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // lblUltimaModificacion
            // 
            lblUltimaModificacion.AutoSize = true;
            lblUltimaModificacion.Location = new Point(18, 30);
            lblUltimaModificacion.Name = "lblUltimaModificacion";
            lblUltimaModificacion.Size = new Size(143, 15);
            lblUltimaModificacion.TabIndex = 0;
            lblUltimaModificacion.Text = "Última modificación: N/A";
            // 
            // FrmEditorPrompt
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 428);
            Controls.Add(groupBoxPrompt);
            Controls.Add(groupBoxAcciones);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEditorPrompt";
            Padding = new Padding(13, 11, 13, 11);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Editor de Prompt - OpenAI";
            Load += FrmEditorPrompt_Load;
            groupBoxPrompt.ResumeLayout(false);
            groupBoxPrompt.PerformLayout();
            groupBoxAcciones.ResumeLayout(false);
            groupBoxAcciones.PerformLayout();
            ResumeLayout(false);
        }

        private GroupBox groupBoxPrompt;
        private TextBox txtPrompt;
        private Label lblInfo;
        private GroupBox groupBoxAcciones;
        private Button btnGuardar;
        private Button btnCancelar;
        private Label lblUltimaModificacion;
    }
}
