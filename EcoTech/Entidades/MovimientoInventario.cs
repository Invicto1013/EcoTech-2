using System;

namespace EcoTech.Entidades
{
    public class MovimientoInventario
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public int UsuarioId { get; set; }

        public string Tipo { get; set; }

        public decimal Cantidad { get; set; }

        public decimal StockAnterior { get; set; }

        public decimal StockNuevo { get; set; }

        public DateTime Fecha { get; set; }

        public string Concepto { get; set; }

        public string Referencia { get; set; }

        // Datos auxiliares para mostrar en pantalla
        public string ProductoCodigo { get; set; }

        public string ProductoNombre { get; set; }

        public string UsuarioNombre { get; set; }
    }
}