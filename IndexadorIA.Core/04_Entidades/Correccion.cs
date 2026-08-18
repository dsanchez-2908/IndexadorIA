namespace IndexadorIA.Entidades
{
    /// <summary>
    /// Entidad que representa una corrección manual realizada sobre un campo
    /// de un resultado de IA, con fines estadísticos (TD_CORRECIONES).
    /// </summary>
    public class Correccion
    {
        public int CdCorreccion { get; set; }
        public int CdResultado { get; set; }
        public string DsCampo { get; set; } = string.Empty;
        public string? DsValorAnterior { get; set; }
        public string? DsValorNuevo { get; set; }
        public DateTime FeControl { get; set; }
        public int CdUsuarioControl { get; set; }

        /// <summary>
        /// Nombres estandarizados de campo usados en dsCampo.
        /// </summary>
        public static class Campos
        {
            public const string Categoria = "Categoria";
            public const string TipoPlano = "TipoPlano";
            public const string Direccion = "Direccion";
            public const string Seccion = "Seccion";
            public const string Manzana = "Manzana";
            public const string Parcela = "Parcela";
            public const string Expediente = "Expediente";
            public const string NumeroPlano = "NumeroPlano";
        }
    }
}
