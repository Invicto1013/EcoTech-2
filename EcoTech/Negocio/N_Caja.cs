using System;
using System.Collections.Generic;
using EcoTech.Datos;
using EcoTech.Entidades;

namespace EcoTech.Negocio
{
    public class N_Caja
    {
        private readonly D_Caja datos =
            new D_Caja();

        public List<Caja> Listar()
        {
            return datos.Listar();
        }

        public decimal ObtenerSaldoActual()
        {
            return datos.ObtenerSaldoActual();
        }

        public decimal Registrar(Caja caja)
        {
            if (caja == null)
                throw new ArgumentNullException(
                    nameof(caja));

            if (caja.UsuarioId <= 0)
                throw new Exception(
                    "El usuario seleccionado no es válido.");

            if (string.IsNullOrWhiteSpace(caja.Concepto))
                throw new Exception(
                    "Debe indicar el concepto del movimiento.");

            if (string.IsNullOrWhiteSpace(caja.Tipo))
                throw new Exception(
                    "Debe indicar el tipo de movimiento.");

            if (caja.Ingreso < 0)
                throw new Exception(
                    "El ingreso no puede ser negativo.");

            if (caja.Egreso < 0)
                throw new Exception(
                    "El egreso no puede ser negativo.");

            if (caja.Ingreso == 0 &&
                caja.Egreso == 0)
            {
                throw new Exception(
                    "Debe registrar un ingreso o un egreso.");
            }

            if (caja.Ingreso > 0 &&
                caja.Egreso > 0)
            {
                throw new Exception(
                    "Un movimiento no puede tener ingreso y " +
                    "egreso al mismo tiempo.");
            }

            if (caja.Tipo.Equals(
                "INGRESO",
                StringComparison.OrdinalIgnoreCase))
            {
                if (caja.Ingreso <= 0 ||
                    caja.Egreso != 0)
                {
                    throw new Exception(
                        "Un INGRESO debe tener un ingreso " +
                        "mayor que cero y ningún egreso.");
                }
            }

            if (caja.Tipo.Equals(
                "EGRESO",
                StringComparison.OrdinalIgnoreCase))
            {
                if (caja.Egreso <= 0 ||
                    caja.Ingreso != 0)
                {
                    throw new Exception(
                        "Un EGRESO debe tener un egreso " +
                        "mayor que cero y ningún ingreso.");
                }

                decimal saldoActual =
                    datos.ObtenerSaldoActual();

                if (caja.Egreso > saldoActual)
                {
                    throw new Exception(
                        "No hay suficiente saldo en caja.\n\n" +
                        $"Saldo disponible: {saldoActual:N2}");
                }
            }

            return datos.Registrar(caja);
        }
    }
}