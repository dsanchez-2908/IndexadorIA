namespace IndexadorIA.Pantallas.Consultas
{
    partial class FrmReubicarPlanosMultiple
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
            lblTotalLotes = new Label();
            lblTotalLotesValor = new Label();
            lblTotalPlanos = new Label();
            lblTotalPlanosValor = new Label();
            lblDescripcionLote = new Label();
            txtDescripcionLote = new TextBox();
            btnCancelar = new Button();
            btnAceptar = new Button();
            SuspendLayout();
            //
            // lblTotalLotes
            //
            lblTotalLotes.AutoSize = true;
            lblTotalLotes.ForeColor = Color.Silver;
            lblTotalLotes.Location = new Point(18, 18);
            lblTotalLotes.Name = "lblTotalLotes";
            lblTotalLotes.Size = new Size(76, 15);
            lblTotalLotes.TabIndex = 0;
            lblTotalLotes.Text = "Total de Lotes:";
            //
            // lblTotalLotesValor
            //
            lblTotalLotesValor.AutoSize = true;
            lblTotalLotesValor.ForeColor = Color.White;
            lblTotalLotesValor.Location = new Point(160, 18);
            lblTotalLotesValor.Name = "lblTotalLotesValor";
            lblTotalLotesValor.Size = new Size(14, 15);
            lblTotalLotesValor.TabIndex = 1;
            lblTotalLotesValor.Text = "0";
            //
            // lblTotalPlanos
            //
            lblTotalPlanos.AutoSize = true;
            lblTotalPlanos.ForeColor = Color.Silver;
            lblTotalPlanos.Location = new Point(18, 46);
            lblTotalPlanos.Name = "lblTotalPlanos";
            lblTotalPlanos.Size = new Size(85, 15);
            lblTotalPlanos.TabIndex = 2;
            lblTotalPlanos.Text = "Total de Planos:";
            //
            // lblTotalPlanosValor
            //
            lblTotalPlanosValor.AutoSize = true;
            lblTotalPlanosValor.ForeColor = Color.White;
            lblTotalPlanosValor.Location = new Point(160, 46);
            lblTotalPlanosValor.Name = "lblTotalPlanosValor";
            lblTotalPlanosValor.Size = new Size(14, 15);
            lblTotalPlanosValor.TabIndex = 3;
            lblTotalPlanosValor.Text = "0";
            //
            // lblDescripcionLote
            //
            lblDescripcionLote.AutoSize = true;
            lblDescripcionLote.ForeColor = Color.Silver;
            lblDescripcionLote.Location = new Point(18, 80);
            lblDescripcionLote.Name = "lblDescripcionLote";
            lblDescripcionLote.Size = new Size(104, 15);
            lblDescripcionLote.TabIndex = 4;
            lblDescripcionLote.Text = "Descripci\u00f3n Lote:";
            //
            // txtDescripcionLote
            //
            txtDescripcionLote.Location = new Point(18, 98);
            txtDescripcionLote.Name = "txtDescripcionLote";
            txtDescripcionLote.Size = new Size(344, 23);
            txtDescripcionLote.TabIndex = 5;
            //
            // btnCancelar
            //
            btnCancelar.BackColor = Color.FromArgb(96, 96, 96);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(162, 140);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(95, 30);
            btnCancelar.TabIndex = 6;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            //
            // btnAceptar
            //
            btnAceptar.BackColor = Color.FromArgb(0, 122, 204);
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.ForeColor = Color.White;
            btnAceptar.Location = new Point(267, 140);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(95, 30);
            btnAceptar.TabIndex = 7;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = false;
            btnAceptar.Click += btnAceptar_Click;
            //
            // FrmReubicarPlanosMultiple
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(380, 188);
            Controls.Add(lblTotalLotes);
            Controls.Add(lblTotalLotesValor);
            Controls.Add(lblTotalPlanos);
            Controls.Add(lblTotalPlanosValor);
            Controls.Add(lblDescripcionLote);
            Controls.Add(txtDescripcionLote);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmReubicarPlanosMultiple";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reubicar Planos en Nuevos Lotes";
            Load += FrmReubicarPlanosMultiple_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTotalLotes;
        private Label lblTotalLotesValor;
        private Label lblTotalPlanos;
        private Label lblTotalPlanosValor;
        private Label lblDescripcionLote;
        private TextBox txtDescripcionLote;
        private Button btnCancelar;
        private Button btnAceptar;
    }
}
