using System;

namespace EcoTech.Entidades
{
    public class DetalleOrden
    {
        public int Id { get; set; }

        public int OrdenId { get; set; }

        public int ProductoId { get; set; }

        public decimal Cantidad { get; set; }

        public decimal Precio { get; set; }

        public decimal Subtotal { get; set; }

        // Datos adicionales para mostrar en pantalla
        public string ProductoCodigo { get; set; }

        public string ProductoNombre { get; set; }
    }
}