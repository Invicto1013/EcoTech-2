using System;

namespace EcoTech.Entidades
{
    public class Caja
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public DateTime Fecha { get; set; }

        public string Concepto { get; set; }

        public decimal Ingreso { get; set; }

        public decimal Egreso { get; set; }

        public decimal Saldo { get; set; }

        public string Tipo { get; set; }

        public string UsuarioNombre { get; set; }
    }
}