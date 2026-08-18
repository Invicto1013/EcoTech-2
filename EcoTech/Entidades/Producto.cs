using System;

namespace EcoTech.Entidades
{
    public class Producto
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public int CategoriaId { get; set; }

        public decimal PrecioCosto { get; set; }

        public decimal PrecioVenta { get; set; }

        public decimal Stock { get; set; }

        public decimal StockMinimo { get; set; }

        public bool Activo { get; set; }

        public DateTime FechaRegistro { get; set; }

        // Información adicional para mostrar en pantalla
        public string CategoriaNombre { get; set; }
    }
}