using System;

namespace EcoTech.Entidades
{
    public class AsientoContable
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Concepto { get; set; }

        public int CuentaId { get; set; }

        public decimal Debe { get; set; }

        public decimal Haber { get; set; }

        public int UsuarioId { get; set; }

        // Datos auxiliares para mostrar en pantalla
        public string CuentaCodigo { get; set; }

        public string CuentaNombre { get; set; }

        public string UsuarioNombre { get; set; }
    }
}