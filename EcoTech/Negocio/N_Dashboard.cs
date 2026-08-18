using System;
using EcoTech.Datos;
using EcoTech.Entidades;

namespace EcoTech.Negocio
{
    public class N_Dashboard
    {
        private readonly D_Dashboard datos =
            new D_Dashboard();

        public Dashboard ObtenerResumen()
        {
            try
            {
                return datos.ObtenerResumen();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "No se pudo obtener la información " +
                    "del Dashboard.\n\n" +
                    ex.Message);
            }
        }
    }
}