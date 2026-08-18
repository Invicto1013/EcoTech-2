using System;

namespace EcoTech.Entidades
{
    public class OrdenCompra
    {
        public int Id { get; set; }

        public int ProveedorId { get; set; }

        public int UsuarioId { get; set; }

        public DateTime Fecha { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Total { get; set; }

        public string Estado { get; set; }

        // Datos adicionales para mostrar en pantalla
        public string ProveedorNombre { get; set; }

        public string UsuarioNombre { get; set; }
    }
}