using System;
using System.Collections.Generic;
using EcoTech.Datos;
using EcoTech.Entidades;

namespace EcoTech.Negocio
{
    public class N_AsientosContables
    {
        private readonly D_AsientosContables datos =
            new D_AsientosContables();

        public List<AsientoContable> Listar()
        {
            return datos.Listar();
        }

        public void Insertar(AsientoContable asiento)
        {
            if (asiento == null)
                throw new ArgumentNullException(
                    nameof(asiento));

            if (asiento.CuentaId <= 0)
                throw new Exception(
                    "Debe seleccionar una cuenta contable.");

            if (asiento.UsuarioId <= 0)
                throw new Exception(
                    "El usuario seleccionado no es válido.");

            if (string.IsNullOrWhiteSpace(asiento.Concepto))
                throw new Exception(
                    "El concepto es obligatorio.");

            if (asiento.Debe < 0)
                throw new Exception(
                    "El Debe no puede ser negativo.");

            if (asiento.Haber < 0)
                throw new Exception(
                    "El Haber no puede ser negativo.");

            if (asiento.Debe == 0 &&
                asiento.Haber == 0)
            {
                throw new Exception(
                    "Debe registrar un valor en Debe o Haber.");
            }

            if (asiento.Debe > 0 &&
                asiento.Haber > 0)
            {
                throw new Exception(
                    "Un asiento no puede tener Debe y Haber " +
                    "al mismo tiempo.");
            }

            datos.Insertar(asiento);
        }
    }
}