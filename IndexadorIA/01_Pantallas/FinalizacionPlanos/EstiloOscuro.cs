namespace IndexadorIA.Pantallas.FinalizacionPlanos
{
    /// <summary>Estilo modo oscuro compartido por las pantallas de Finalizacion (misma paleta que el resto de la aplicacion).</summary>
    internal static class EstiloOscuro
    {
        public static readonly Color Fondo = Color.FromArgb(45, 45, 48);
        public static readonly Color FondoControl = Color.FromArgb(30, 30, 30);
        public static readonly Color Azul = Color.FromArgb(0, 122, 204);
        public static readonly Color Rojo = Color.FromArgb(150, 40, 40);
        public static readonly Color Verde = Color.FromArgb(40, 130, 70);

        public static void Formulario(Form f)
        {
            f.BackColor = Fondo;
            f.ForeColor = Color.White;
            f.Font = new Font("Segoe UI", 10F);
        }

        public static void Boton(Button b, Color? color = null)
        {
            b.BackColor = color ?? Azul;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.UseVisualStyleBackColor = false;
            b.AutoSize = true;
            b.Padding = new Padding(6, 2, 6, 2);
            b.Margin = new Padding(3, 2, 3, 2);
        }

        public static void Grilla(DataGridView g)
        {
            g.BackgroundColor = FondoControl;
            g.BorderStyle = BorderStyle.None;
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 60);
            g.DefaultCellStyle.BackColor = FondoControl;
            g.DefaultCellStyle.ForeColor = Color.White;
            g.DefaultCellStyle.SelectionBackColor = Azul;
            g.DefaultCellStyle.SelectionForeColor = Color.White;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            g.GridColor = Color.FromArgb(63, 63, 70);
            g.RowHeadersVisible = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.AllowUserToResizeRows = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.AutoGenerateColumns = false;
        }

        public static void Campo(Control c)
        {
            c.BackColor = FondoControl;
            c.ForeColor = Color.White;
            if (c is ComboBox cb) cb.FlatStyle = FlatStyle.Flat;
            if (c is TextBox tb) tb.BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
