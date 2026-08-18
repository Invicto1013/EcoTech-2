using System;
using System.Data.SqlClient;
using EcoTech.Entidades;
using EcoTech.Utilidades;

namespace EcoTech.Datos
{
    public class D_Dashboard
    {
        private readonly Conexion conexion =
            new Conexion();

        public Dashboard ObtenerResumen()
        {
            Dashboard dashboard =
                new Dashboard();

            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                dashboard.TotalUsuarios =
                    ObtenerEntero(
                        cn,
                        @"
                        SELECT COUNT(*)
                        FROM dbo.usuarios;
                        ");

                dashboard.TotalClientes =
                    ObtenerEntero(
                        cn,
                        @"
                        SELECT COUNT(*)
                        FROM dbo.clientes;
                        ");

                dashboard.TotalProductos =
                    ObtenerEntero(
                        cn,
                        @"
                        SELECT COUNT(*)
                        FROM dbo.productos;
                        ");

                dashboard.TotalCategorias =
                    ObtenerEntero(
                        cn,
                        @"
                        SELECT COUNT(*)
                        FROM dbo.categorias
                        WHERE activo = 1;
                        ");

                dashboard.TotalProveedores =
                    ObtenerEntero(
                        cn,
                        @"
                        SELECT COUNT(*)
                        FROM dbo.proveedores
                        WHERE activo = 1;
                        ");

                dashboard.TotalOrdenes =
                    ObtenerEntero(
                        cn,
                        @"
                        SELECT COUNT(*)
                        FROM dbo.ordenes_compra;
                        ");

                dashboard.TotalRecepciones =
                    ObtenerEntero(
                        cn,
                        @"
                        SELECT COUNT(*)
                        FROM dbo.recepciones_compra;
                        ");

                dashboard.ComprasTotales =
                    ObtenerDecimal(
                        cn,
                        @"
                        SELECT ISNULL(
                            SUM(total),
                            0
                        )
                        FROM dbo.ordenes_compra;
                        ");

                dashboard.SaldoCaja =
                    ObtenerDecimal(
                        cn,
                        @"
                        SELECT ISNULL(
                            MAX(saldo),
                            0
                        )
                        FROM dbo.caja;
                        ");
            }

            return dashboard;
        }

        private int ObtenerEntero(
            SqlConnection cn,
            string sql)
        {
            using (SqlCommand cmd =
                   new SqlCommand(sql, cn))
            {
                object resultado =
                    cmd.ExecuteScalar();

                return Convert.ToInt32(
                    resultado);
            }
        }

        private decimal ObtenerDecimal(
            SqlConnection cn,
            string sql)
        {
            using (SqlCommand cmd =
                   new SqlCommand(sql, cn))
            {
                object resultado =
                    cmd.ExecuteScalar();

                return Convert.ToDecimal(
                    resultado);
            }
        }
    }
}