using System;

namespace EcoTech.Entidades
{
    public class Proveedor
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Nit { get; set; }

        public string Email { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public int PlazoPago { get; set; }

        public bool Activo { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}