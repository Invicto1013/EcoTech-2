using System;
using System.Collections.Generic;
using EcoTech.Datos;
using EcoTech.Entidades;

namespace EcoTech.Negocio
{
    public class N_Clientes
    {
        private readonly D_Clientes datos = new D_Clientes();

        public List<Cliente> Listar()
        {
            return datos.Listar();
        }

        public void Insertar(Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new Exception(
                    "El nombre del cliente es obligatorio.");

            if (cliente.LimiteCredito < 0)
                throw new Exception(
                    "El límite de crédito no puede ser negativo.");

            datos.Insertar(cliente);
        }

        public void Editar(Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            if (cliente.Id <= 0)
                throw new Exception(
                    "El cliente seleccionado no es válido.");

            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new Exception(
                    "El nombre del cliente es obligatorio.");

            if (cliente.LimiteCredito < 0)
                throw new Exception(
                    "El límite de crédito no puede ser negativo.");

            datos.Editar(cliente);
        }

        public void Desactivar(int id)
        {
            if (id <= 0)
                throw new Exception(
                    "El cliente seleccionado no es válido.");

            datos.Desactivar(id);
        }
    }
}