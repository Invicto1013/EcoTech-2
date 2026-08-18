namespace EcoTech.Entidades
{
    public class Dashboard
    {
        public int TotalUsuarios { get; set; }

        public int TotalClientes { get; set; }

        public int TotalProductos { get; set; }

        public int TotalCategorias { get; set; }

        public int TotalProveedores { get; set; }

        public int TotalOrdenes { get; set; }

        public int TotalRecepciones { get; set; }

        public decimal ComprasTotales { get; set; }

        public decimal SaldoCaja { get; set; }
    }
}