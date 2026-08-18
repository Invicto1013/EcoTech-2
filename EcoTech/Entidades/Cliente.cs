using System;

namespace EcoTech.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Nit { get; set; }

        public string Email { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public decimal LimiteCredito { get; set; }

        public bool Activo { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}