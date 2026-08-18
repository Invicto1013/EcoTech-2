using System;
using System.Collections.Generic;
using EcoTech.Datos;
using EcoTech.Entidades;

namespace EcoTech.Negocio
{
    public class N_CuentasContables
    {
        private readonly D_CuentasContables datos =
            new D_CuentasContables();

        public List<CuentaContable> Listar()
        {
            return datos.Listar();
        }

        public void Insertar(CuentaContable cuenta)
        {
            if (cuenta == null)
                throw new ArgumentNullException(
                    nameof(cuenta));

            if (string.IsNullOrWhiteSpace(cuenta.Codigo))
                throw new Exception(
                    "El código de la cuenta es obligatorio.");

            if (string.IsNullOrWhiteSpace(cuenta.Nombre))
                throw new Exception(
                    "El nombre de la cuenta es obligatorio.");

            if (string.IsNullOrWhiteSpace(cuenta.Tipo))
                throw new Exception(
                    "Debe seleccionar el tipo de cuenta.");

            datos.Insertar(cuenta);
        }

        public void Editar(CuentaContable cuenta)
        {
            if (cuenta == null)
                throw new ArgumentNullException(
                    nameof(cuenta));

            if (cuenta.Id <= 0)
                throw new Exception(
                    "La cuenta seleccionada no es válida.");

            if (string.IsNullOrWhiteSpace(cuenta.Codigo))
                throw new Exception(
                    "El código de la cuenta es obligatorio.");

            if (string.IsNullOrWhiteSpace(cuenta.Nombre))
                throw new Exception(
                    "El nombre de la cuenta es obligatorio.");

            if (string.IsNullOrWhiteSpace(cuenta.Tipo))
                throw new Exception(
                    "Debe seleccionar el tipo de cuenta.");

            datos.Editar(cuenta);
        }

        public void Desactivar(int id)
        {
            if (id <= 0)
                throw new Exception(
                    "La cuenta seleccionada no es válida.");

            datos.Desactivar(id);
        }
    }
}