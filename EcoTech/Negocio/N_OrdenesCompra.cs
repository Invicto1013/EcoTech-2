using System;
using System.Collections.Generic;
using EcoTech.Datos;
using EcoTech.Entidades;

namespace EcoTech.Negocio
{
    public class N_OrdenesCompra
    {
        private readonly D_OrdenesCompra datos =
            new D_OrdenesCompra();

        public List<OrdenCompra> Listar()
        {
            return datos.Listar();
        }

        public List<DetalleOrden> ListarDetalles(int ordenId)
        {
            if (ordenId <= 0)
                throw new Exception(
                    "La orden seleccionada no es válida.");

            return datos.ListarDetalles(ordenId);
        }

        public int Insertar(
            OrdenCompra orden,
            List<DetalleOrden> detalles)
        {
            if (orden == null)
                throw new ArgumentNullException(nameof(orden));

            if (detalles == null || detalles.Count == 0)
                throw new Exception(
                    "La orden debe tener al menos un producto.");

            if (orden.ProveedorId <= 0)
                throw new Exception(
                    "Debe seleccionar un proveedor.");

            if (orden.UsuarioId <= 0)
                throw new Exception(
                    "El usuario de la orden no es válido.");

            if (orden.Subtotal < 0)
                throw new Exception(
                    "El subtotal no puede ser negativo.");

            if (orden.Impuesto < 0)
                throw new Exception(
                    "El impuesto no puede ser negativo.");

            if (orden.Total < 0)
                throw new Exception(
                    "El total no puede ser negativo.");

            foreach (DetalleOrden detalle in detalles)
            {
                if (detalle.ProductoId <= 0)
                    throw new Exception(
                        "Uno de los productos seleccionados no es válido.");

                if (detalle.Cantidad <= 0)
                    throw new Exception(
                        "La cantidad debe ser mayor que cero.");

                if (detalle.Precio < 0)
                    throw new Exception(
                        "El precio no puede ser negativo.");

                if (detalle.Subtotal < 0)
                    throw new Exception(
                        "El subtotal del detalle no puede ser negativo.");
            }

            if (string.IsNullOrWhiteSpace(orden.Estado))
                orden.Estado = "Pendiente";

            return datos.Insertar(orden, detalles);
        }
    }
}