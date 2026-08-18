using System;

namespace EcoTech.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }

        public string NombreUsuario { get; set; }

        public string PasswordHash { get; set; }

        public int RolId { get; set; }

        public string RolNombre { get; set; }

        public bool Activo { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}