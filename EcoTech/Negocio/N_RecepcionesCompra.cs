using System;
using System.Collections.Generic;
using EcoTech.Datos;
using EcoTech.Entidades;

namespace EcoTech.Negocio
{
    public class N_RecepcionesCompra
    {
        private readonly D_RecepcionesCompra datos =
            new D_RecepcionesCompra();

        public List<RecepcionCompra> Listar()
        {
            return datos.Listar();
        }

        public List<RecepcionCompra> ListarOrdenesPendientes()
        {
            return datos.ListarOrdenesPendientes();
        }

        public void Registrar(RecepcionCompra recepcion)
        {
            if (recepcion == null)
                throw new ArgumentNullException(
                    nameof(recepcion));

            if (recepcion.OrdenId <= 0)
                throw new Exception(
                    "Debe seleccionar una orden de compra.");

            if (recepcion.UsuarioId <= 0)
                throw new Exception(
                    "El usuario actual no es válido.");

            if (string.IsNullOrWhiteSpace(
                recepcion.Observacion))
            {
                throw new Exception(
                    "Debe indicar una observación.");
            }

            if (recepcion.Observacion.Trim().Length > 500)
            {
                throw new Exception(
                    "La observación no puede superar " +
                    "los 500 caracteres.");
            }

            datos.Registrar(recepcion);
        }
    }
}