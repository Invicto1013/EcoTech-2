using System;

namespace EcoTech.Entidades
{
    public class CuentaContable
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public string Tipo { get; set; }

        public string Descripcion { get; set; }

        public bool Activo { get; set; }
        public override string ToString()
        {
            return $"{Codigo} - {Nombre}";
        }
    }
}