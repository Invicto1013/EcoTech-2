using System;

namespace EcoTech.Entidades
{
    public class RecepcionCompra
    {
        public int Id { get; set; }

        public int OrdenId { get; set; }

        public int UsuarioId { get; set; }

        public DateTime Fecha { get; set; }

        public string Observacion { get; set; }

        public string ProveedorNombre { get; set; }

        public string UsuarioNombre { get; set; }

        public decimal OrdenTotal { get; set; }

        public string OrdenEstado { get; set; }
    }
}