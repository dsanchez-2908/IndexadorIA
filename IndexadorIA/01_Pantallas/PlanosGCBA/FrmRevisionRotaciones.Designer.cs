namespace IndexadorIA.Pantallas.PlanosGCBA
{
    partial class FrmRevisionRotaciones
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.pnlPaginas = new System.Windows.Forms.TableLayoutPanel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnAnterior = new System.Windows.Forms.Button();
            this.btnSiguiente = new System.Windows.Forms.Button();
            this.lblPagina = new System.Windows.Forms.Label();
            this.btnFinalizar = new System.Windows.Forms.Button();
            this.progressBarFinalizar = new System.Windows.Forms.ProgressBar();
            this.lblProgresoFinalizar = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.pnlTop.Controls.Add(this.lblInfo);
            this.pnlTop.Controls.Add(this.lblTitulo);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1000, 70);
            this.pnlTop.TabIndex = 0;
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(15, 10);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(400, 21);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Revisión de rotaciones automáticas (baja confianza)";
            //
            // lblInfo
            //
            this.lblInfo.AutoSize = true;
            this.lblInfo.ForeColor = System.Drawing.Color.LightGray;
            this.lblInfo.Location = new System.Drawing.Point(15, 38);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(400, 15);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "Haga clic sobre una página para rotarla 90° a la derecha.";
            //
            // pnlPaginas
            //
            this.pnlPaginas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.pnlPaginas.ColumnCount = 5;
            this.pnlPaginas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pnlPaginas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pnlPaginas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pnlPaginas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pnlPaginas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pnlPaginas.RowCount = 2;
            this.pnlPaginas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlPaginas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlPaginas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPaginas.Location = new System.Drawing.Point(0, 70);
            this.pnlPaginas.Name = "pnlPaginas";
            this.pnlPaginas.Padding = new System.Windows.Forms.Padding(10);
            this.pnlPaginas.Size = new System.Drawing.Size(1000, 480);
            this.pnlPaginas.TabIndex = 1;
            //
            // pnlBottom
            //
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.pnlBottom.Controls.Add(this.lblProgresoFinalizar);
            this.pnlBottom.Controls.Add(this.progressBarFinalizar);
            this.pnlBottom.Controls.Add(this.btnFinalizar);
            this.pnlBottom.Controls.Add(this.lblPagina);
            this.pnlBottom.Controls.Add(this.btnSiguiente);
            this.pnlBottom.Controls.Add(this.btnAnterior);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 550);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1000, 60);
            this.pnlBottom.TabIndex = 2;
            //
            // btnAnterior
            //
            this.btnAnterior.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnterior.ForeColor = System.Drawing.Color.White;
            this.btnAnterior.Location = new System.Drawing.Point(15, 13);
            this.btnAnterior.Name = "btnAnterior";
            this.btnAnterior.Size = new System.Drawing.Size(110, 34);
            this.btnAnterior.TabIndex = 0;
            this.btnAnterior.Text = "◄ Anterior";
            this.btnAnterior.UseVisualStyleBackColor = false;
            this.btnAnterior.Click += new System.EventHandler(this.btnAnterior_Click);
            //
            // btnSiguiente
            //
            this.btnSiguiente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.btnSiguiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSiguiente.ForeColor = System.Drawing.Color.White;
            this.btnSiguiente.Location = new System.Drawing.Point(135, 13);
            this.btnSiguiente.Name = "btnSiguiente";
            this.btnSiguiente.Size = new System.Drawing.Size(110, 34);
            this.btnSiguiente.TabIndex = 1;
            this.btnSiguiente.Text = "Siguiente ►";
            this.btnSiguiente.UseVisualStyleBackColor = false;
            this.btnSiguiente.Click += new System.EventHandler(this.btnSiguiente_Click);
            //
            // lblPagina
            //
            this.lblPagina.AutoSize = true;
            this.lblPagina.ForeColor = System.Drawing.Color.White;
            this.lblPagina.Location = new System.Drawing.Point(260, 22);
            this.lblPagina.Name = "lblPagina";
            this.lblPagina.Size = new System.Drawing.Size(120, 15);
            this.lblPagina.TabIndex = 2;
            this.lblPagina.Text = "Página 1 de 1";
            //
            // btnFinalizar
            //
            this.btnFinalizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFinalizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnFinalizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFinalizar.ForeColor = System.Drawing.Color.White;
            this.btnFinalizar.Location = new System.Drawing.Point(860, 13);
            this.btnFinalizar.Name = "btnFinalizar";
            this.btnFinalizar.Size = new System.Drawing.Size(120, 34);
            this.btnFinalizar.TabIndex = 3;
            this.btnFinalizar.Text = "Finalizar";
            this.btnFinalizar.UseVisualStyleBackColor = false;
            this.btnFinalizar.Click += new System.EventHandler(this.btnFinalizar_Click);
            //
            // progressBarFinalizar
            //
            this.progressBarFinalizar.Location = new System.Drawing.Point(15, 13);
            this.progressBarFinalizar.Name = "progressBarFinalizar";
            this.progressBarFinalizar.Size = new System.Drawing.Size(700, 20);
            this.progressBarFinalizar.TabIndex = 4;
            this.progressBarFinalizar.Visible = false;
            //
            // lblProgresoFinalizar
            //
            this.lblProgresoFinalizar.AutoSize = true;
            this.lblProgresoFinalizar.ForeColor = System.Drawing.Color.White;
            this.lblProgresoFinalizar.Location = new System.Drawing.Point(15, 36);
            this.lblProgresoFinalizar.Name = "lblProgresoFinalizar";
            this.lblProgresoFinalizar.Size = new System.Drawing.Size(200, 15);
            this.lblProgresoFinalizar.TabIndex = 5;
            this.lblProgresoFinalizar.Text = "";
            this.lblProgresoFinalizar.Visible = false;
            //
            // FrmRevisionRotaciones
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(1000, 610);
            this.Controls.Add(this.pnlPaginas);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "FrmRevisionRotaciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Revisión de rotaciones";
            this.Load += new System.EventHandler(this.FrmRevisionRotaciones_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TableLayoutPanel pnlPaginas;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnAnterior;
        private System.Windows.Forms.Button btnSiguiente;
        private System.Windows.Forms.Label lblPagina;
        private System.Windows.Forms.Button btnFinalizar;
        private System.Windows.Forms.ProgressBar progressBarFinalizar;
        private System.Windows.Forms.Label lblProgresoFinalizar;
    }
}
