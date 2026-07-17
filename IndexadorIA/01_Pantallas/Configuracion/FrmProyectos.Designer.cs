namespace IndexadorIA.Pantallas.Configuracion
{
    partial class FrmProyectos
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
            panelSuperior = new Panel();
            btnCerrar = new Button();
            btnNuevo = new Button();
            lblTitulo = new Label();
            panelGrid = new Panel();
            dgvProyectos = new DataGridView();
            colEditar = new DataGridViewButtonColumn();
            colEliminar = new DataGridViewButtonColumn();
            panelSuperior.SuspendLayout();
            panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProyectos).BeginInit();
            SuspendLayout();
            // 
            // panelSuperior
            // 
            panelSuperior.BackColor = Color.FromArgb(45, 45, 48);
            panelSuperior.Controls.Add(btnCerrar);
            panelSuperior.Controls.Add(btnNuevo);
            panelSuperior.Controls.Add(lblTitulo);
            panelSuperior.Dock = DockStyle.Top;
            panelSuperior.Location = new Point(0, 0);
            panelSuperior.Name = "panelSuperior";
            panelSuperior.Padding = new Padding(20);
            panelSuperior.Size = new Size(1000, 80);
            panelSuperior.TabIndex = 0;
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.BackColor = Color.FromArgb(62, 62, 66);
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(700, 20);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(130, 40);
            btnCerrar.TabIndex = 2;
            btnCerrar.Text = "✖ CERRAR";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // btnNuevo
            // 
            btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNuevo.BackColor = Color.FromArgb(0, 122, 204);
            btnNuevo.Cursor = Cursors.Hand;
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(850, 20);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(130, 40);
            btnNuevo.TabIndex = 1;
            btnNuevo.Text = "+ NUEVO";
            btnNuevo.UseVisualStyleBackColor = false;
            btnNuevo.Click += btnNuevo_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 25);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(291, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de Proyectos";
            // 
            // panelGrid
            // 
            panelGrid.BackColor = Color.FromArgb(32, 32, 32);
            panelGrid.Controls.Add(dgvProyectos);
            panelGrid.Dock = DockStyle.Fill;
            panelGrid.Location = new Point(0, 80);
            panelGrid.Name = "panelGrid";
            panelGrid.Padding = new Padding(20);
            panelGrid.Size = new Size(1000, 520);
            panelGrid.TabIndex = 1;
            // 
            // dgvProyectos
            // 
            dgvProyectos.AllowUserToAddRows = false;
            dgvProyectos.AllowUserToDeleteRows = false;
            dgvProyectos.AutoGenerateColumns = true;
            dgvProyectos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProyectos.BackgroundColor = Color.FromArgb(45, 45, 48);
            dgvProyectos.BorderStyle = BorderStyle.None;
            dgvProyectos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProyectos.Columns.AddRange(new DataGridViewColumn[] { colEditar, colEliminar });
            dgvProyectos.Dock = DockStyle.Fill;
            dgvProyectos.Location = new Point(20, 20);
            dgvProyectos.Name = "dgvProyectos";
            dgvProyectos.ReadOnly = true;
            dgvProyectos.RowHeadersWidth = 51;
            dgvProyectos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProyectos.Size = new Size(960, 480);
            dgvProyectos.TabIndex = 0;
            dgvProyectos.CellClick += dgvProyectos_CellClick;
            // 
            // colEditar
            // 
            colEditar.FillWeight = 50F;
            colEditar.HeaderText = "";
            colEditar.MinimumWidth = 6;
            colEditar.Name = "colEditar";
            colEditar.ReadOnly = true;
            colEditar.Text = "Editar";
            colEditar.UseColumnTextForButtonValue = true;
            // 
            // colEliminar
            // 
            colEliminar.FillWeight = 50F;
            colEliminar.HeaderText = "";
            colEliminar.MinimumWidth = 6;
            colEliminar.Name = "colEliminar";
            colEliminar.ReadOnly = true;
            colEliminar.Text = "Eliminar";
            colEliminar.UseColumnTextForButtonValue = true;
            // 
            // FrmProyectos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 600);
            Controls.Add(panelGrid);
            Controls.Add(panelSuperior);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmProyectos";
            Text = "Proyectos";
            Load += FrmProyectos_Load;
            panelSuperior.ResumeLayout(false);
            panelSuperior.PerformLayout();
            panelGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProyectos).EndInit();
            ResumeLayout(false);
        }

        private Panel panelSuperior;
        private Label lblTitulo;
        private Button btnNuevo;
        private Button btnCerrar;
        private Panel panelGrid;
        private DataGridView dgvProyectos;
        private DataGridViewButtonColumn colEditar;
        private DataGridViewButtonColumn colEliminar;
    }
}
