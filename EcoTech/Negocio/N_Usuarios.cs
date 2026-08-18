using System;
using System.Collections.Generic;
using EcoTech.Datos;
using EcoTech.Entidades;
using EcoTech.Utilidades;

namespace EcoTech.Negocio
{
    public class N_Usuarios
    {
        private readonly D_Usuarios datos = new D_Usuarios();

        public List<Usuario> Listar()
        {
            return datos.Listar();
        }

        public Usuario ObtenerPorUsuario(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return null;

            return datos.ObtenerPorUsuario(nombreUsuario.Trim());
        }

        public void Insertar(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                throw new Exception("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
                throw new Exception("La contraseña es obligatoria.");

            if (usuario.RolId <= 0)
                throw new Exception("Debe seleccionar un rol.");

            datos.Insertar(usuario);
        }

        public void Editar(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if (usuario.Id <= 0)
                throw new Exception("El usuario seleccionado no es válido.");

            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                throw new Exception("El nombre de usuario es obligatorio.");

            if (usuario.RolId <= 0)
                throw new Exception("Debe seleccionar un rol.");

            datos.Editar(usuario);
        }

        public void Desactivar(int id)
        {
            if (id <= 0)
                throw new Exception("El usuario seleccionado no es válido.");

            datos.Desactivar(id);
        }

        public Usuario Autenticar(string nombreUsuario, string password)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return null;

            if (string.IsNullOrWhiteSpace(password))
                return null;

            Usuario usuario = datos.ObtenerPorUsuario(nombreUsuario.Trim());

            if (usuario == null)
                return null;

            if (!usuario.Activo)
                return null;

            if (!PasswordHelper.VerifyPassword(
                password,
                usuario.PasswordHash))
            {
                return null;
            }

            return usuario;
        }
    }
}