using System;
using System.Collections.Generic;
using EcoTech.Datos;
using EcoTech.Entidades;

namespace EcoTech.Negocio
{
    public class N_Productos
    {
        private readonly D_Productos datos =
            new D_Productos();

        public List<Producto> Listar()
        {
            return datos.Listar();
        }

        public void Insertar(Producto producto)
        {
            if (producto == null)
                throw new ArgumentNullException(
                    nameof(producto));

            if (string.IsNullOrWhiteSpace(producto.Codigo))
                throw new Exception(
                    "El código del producto es obligatorio.");

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new Exception(
                    "El nombre del producto es obligatorio.");

            if (producto.CategoriaId <= 0)
                throw new Exception(
                    "Debe seleccionar una categoría.");

            if (producto.PrecioCosto < 0)
                throw new Exception(
                    "El precio de costo no puede ser negativo.");

            if (producto.PrecioVenta < 0)
                throw new Exception(
                    "El precio de venta no puede ser negativo.");

            if (producto.Stock < 0)
                throw new Exception(
                    "El stock no puede ser negativo.");

            if (producto.StockMinimo < 0)
                throw new Exception(
                    "El stock mínimo no puede ser negativo.");

            datos.Insertar(producto);
        }

        public void Editar(Producto producto)
        {
            if (producto == null)
                throw new ArgumentNullException(
                    nameof(producto));

            if (producto.Id <= 0)
                throw new Exception(
                    "El producto seleccionado no es válido.");

            if (string.IsNullOrWhiteSpace(producto.Codigo))
                throw new Exception(
                    "El código del producto es obligatorio.");

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new Exception(
                    "El nombre del producto es obligatorio.");

            if (producto.CategoriaId <= 0)
                throw new Exception(
                    "Debe seleccionar una categoría.");

            if (producto.PrecioCosto < 0)
                throw new Exception(
                    "El precio de costo no puede ser negativo.");

            if (producto.PrecioVenta < 0)
                throw new Exception(
                    "El precio de venta no puede ser negativo.");

            if (producto.StockMinimo < 0)
                throw new Exception(
                    "El stock mínimo no puede ser negativo.");

            datos.Editar(producto);
        }

        public void Desactivar(int id)
        {
            if (id <= 0)
                throw new Exception(
                    "El producto seleccionado no es válido.");

            datos.Desactivar(id);
        }
    }
}