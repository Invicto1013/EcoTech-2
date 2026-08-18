using System;
using System.Collections.Generic;
using EcoTech.Datos;
using EcoTech.Entidades;

namespace EcoTech.Negocio
{
    public class N_Inventario
    {
        private readonly D_Inventario datos =
            new D_Inventario();

        public List<MovimientoInventario> ListarMovimientos()
        {
            return datos.ListarMovimientos();
        }

        public List<ProductoInventario> ListarProductos()
        {
            return datos.ListarProductos();
        }

        public void RegistrarMovimiento(
            MovimientoInventario movimiento)
        {
            if (movimiento == null)
                throw new ArgumentNullException(
                    nameof(movimiento));

            if (movimiento.ProductoId <= 0)
                throw new Exception(
                    "Debe seleccionar un producto.");

            if (movimiento.UsuarioId <= 0)
                throw new Exception(
                    "El usuario seleccionado no es válido.");

            if (string.IsNullOrWhiteSpace(movimiento.Tipo))
                throw new Exception(
                    "Debe indicar el tipo de movimiento.");

            if (movimiento.Cantidad <= 0)
                throw new Exception(
                    "La cantidad debe ser mayor que cero.");

            if (movimiento.StockAnterior < 0)
                throw new Exception(
                    "El stock anterior no puede ser negativo.");

            if (movimiento.StockNuevo < 0)
                throw new Exception(
                    "El stock nuevo no puede ser negativo.");

            if (string.IsNullOrWhiteSpace(movimiento.Concepto))
                throw new Exception(
                    "Debe indicar el concepto del movimiento.");

            datos.RegistrarMovimiento(movimiento);
        }
    }
}