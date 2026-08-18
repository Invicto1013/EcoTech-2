using System;
using System.Collections.Generic;
using EcoTech.Datos;
using EcoTech.Entidades;

namespace EcoTech.Negocio
{
    public class N_Proveedores
    {
        private readonly D_Proveedores datos =
            new D_Proveedores();

        public List<Proveedor> Listar()
        {
            return datos.Listar();
        }

        public void Insertar(Proveedor proveedor)
        {
            if (proveedor == null)
                throw new ArgumentNullException(
                    nameof(proveedor));

            if (string.IsNullOrWhiteSpace(proveedor.Nombre))
                throw new Exception(
                    "El nombre del proveedor es obligatorio.");

            if (proveedor.Nombre.Trim().Length > 150)
                throw new Exception(
                    "El nombre no puede superar los 150 caracteres.");

            if (!string.IsNullOrWhiteSpace(proveedor.Nit) &&
                proveedor.Nit.Trim().Length > 30)
            {
                throw new Exception(
                    "El NIT no puede superar los 30 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(proveedor.Email) &&
                proveedor.Email.Trim().Length > 100)
            {
                throw new Exception(
                    "El email no puede superar los 100 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(proveedor.Telefono) &&
                proveedor.Telefono.Trim().Length > 30)
            {
                throw new Exception(
                    "El teléfono no puede superar los 30 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(proveedor.Direccion) &&
                proveedor.Direccion.Trim().Length > 255)
            {
                throw new Exception(
                    "La dirección no puede superar los 255 caracteres.");
            }

            if (proveedor.PlazoPago < 0)
                throw new Exception(
                    "El plazo de pago no puede ser negativo.");

            datos.Insertar(proveedor);
        }

        public void Actualizar(Proveedor proveedor)
        {
            if (proveedor == null)
                throw new ArgumentNullException(
                    nameof(proveedor));

            if (proveedor.Id <= 0)
                throw new Exception(
                    "El proveedor seleccionado no es válido.");

            if (string.IsNullOrWhiteSpace(proveedor.Nombre))
                throw new Exception(
                    "El nombre del proveedor es obligatorio.");

            if (proveedor.Nombre.Trim().Length > 150)
                throw new Exception(
                    "El nombre no puede superar los 150 caracteres.");

            if (!string.IsNullOrWhiteSpace(proveedor.Nit) &&
                proveedor.Nit.Trim().Length > 30)
            {
                throw new Exception(
                    "El NIT no puede superar los 30 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(proveedor.Email) &&
                proveedor.Email.Trim().Length > 100)
            {
                throw new Exception(
                    "El email no puede superar los 100 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(proveedor.Telefono) &&
                proveedor.Telefono.Trim().Length > 30)
            {
                throw new Exception(
                    "El teléfono no puede superar los 30 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(proveedor.Direccion) &&
                proveedor.Direccion.Trim().Length > 255)
            {
                throw new Exception(
                    "La dirección no puede superar los 255 caracteres.");
            }

            if (proveedor.PlazoPago < 0)
                throw new Exception(
                    "El plazo de pago no puede ser negativo.");

            datos.Actualizar(proveedor);
        }

        public void Desactivar(int id)
        {
            if (id <= 0)
                throw new Exception(
                    "El proveedor seleccionado no es válido.");

            datos.Desactivar(id);
        }
    }
}