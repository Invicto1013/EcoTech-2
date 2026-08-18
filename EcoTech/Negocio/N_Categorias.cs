using System;
using System.Collections.Generic;
using EcoTech.Datos;
using EcoTech.Entidades;

namespace EcoTech.Negocio
{
    public class N_Categorias
    {
        private readonly D_Categorias datos =
            new D_Categorias();

        public List<Categoria> Listar()
        {
            return datos.Listar();
        }

        public void Insertar(Categoria categoria)
        {
            if (categoria == null)
                throw new ArgumentNullException(
                    nameof(categoria));

            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                throw new Exception(
                    "El nombre de la categoría es obligatorio.");

            if (categoria.Nombre.Trim().Length > 100)
                throw new Exception(
                    "El nombre de la categoría no puede superar " +
                    "los 100 caracteres.");

            if (!string.IsNullOrWhiteSpace(categoria.Descripcion) &&
                categoria.Descripcion.Trim().Length > 255)
            {
                throw new Exception(
                    "La descripción no puede superar " +
                    "los 255 caracteres.");
            }

            datos.Insertar(categoria);
        }

        public void Actualizar(Categoria categoria)
        {
            if (categoria == null)
                throw new ArgumentNullException(
                    nameof(categoria));

            if (categoria.Id <= 0)
                throw new Exception(
                    "La categoría seleccionada no es válida.");

            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                throw new Exception(
                    "El nombre de la categoría es obligatorio.");

            if (categoria.Nombre.Trim().Length > 100)
                throw new Exception(
                    "El nombre de la categoría no puede superar " +
                    "los 100 caracteres.");

            if (!string.IsNullOrWhiteSpace(categoria.Descripcion) &&
                categoria.Descripcion.Trim().Length > 255)
            {
                throw new Exception(
                    "La descripción no puede superar " +
                    "los 255 caracteres.");
            }

            datos.Actualizar(categoria);
        }

        public void Desactivar(int id)
        {
            if (id <= 0)
                throw new Exception(
                    "La categoría seleccionada no es válida.");

            datos.Desactivar(id);
        }
    }
}