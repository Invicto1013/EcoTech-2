using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EcoTech.Entidades;
using EcoTech.Utilidades;

namespace EcoTech.Datos
{
    public class D_Proveedores
    {
        private readonly Conexion conexion =
            new Conexion();

        public List<Proveedor> Listar()
        {
            List<Proveedor> lista =
                new List<Proveedor>();

            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        id,
                        nombre,
                        nit,
                        email,
                        telefono,
                        direccion,
                        plazo_pago,
                        activo,
                        fecha_registro
                    FROM dbo.proveedores
                    ORDER BY
                        id DESC;
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                using (SqlDataReader reader =
                       cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Proveedor
                        {
                            Id =
                                Convert.ToInt32(
                                    reader["id"]),

                            Nombre =
                                reader["nombre"]
                                .ToString(),

                            Nit =
                                reader["nit"] == DBNull.Value
                                    ? string.Empty
                                    : reader["nit"].ToString(),

                            Email =
                                reader["email"] == DBNull.Value
                                    ? string.Empty
                                    : reader["email"].ToString(),

                            Telefono =
                                reader["telefono"] == DBNull.Value
                                    ? string.Empty
                                    : reader["telefono"].ToString(),

                            Direccion =
                                reader["direccion"] == DBNull.Value
                                    ? string.Empty
                                    : reader["direccion"].ToString(),

                            PlazoPago =
                                Convert.ToInt32(
                                    reader["plazo_pago"]),

                            Activo =
                                Convert.ToBoolean(
                                    reader["activo"]),

                            FechaRegistro =
                                Convert.ToDateTime(
                                    reader["fecha_registro"])
                        });
                    }
                }
            }

            return lista;
        }

        public void Insertar(Proveedor proveedor)
        {
            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    INSERT INTO dbo.proveedores
                    (
                        nombre,
                        nit,
                        email,
                        telefono,
                        direccion,
                        plazo_pago,
                        activo,
                        fecha_registro
                    )
                    VALUES
                    (
                        @nombre,
                        @nit,
                        @email,
                        @telefono,
                        @direccion,
                        @plazo_pago,
                        @activo,
                        @fecha_registro
                    );
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@nombre",
                        proveedor.Nombre);

                    cmd.Parameters.AddWithValue(
                        "@nit",
                        string.IsNullOrWhiteSpace(
                            proveedor.Nit)
                            ? (object)DBNull.Value
                            : proveedor.Nit);

                    cmd.Parameters.AddWithValue(
                        "@email",
                        string.IsNullOrWhiteSpace(
                            proveedor.Email)
                            ? (object)DBNull.Value
                            : proveedor.Email);

                    cmd.Parameters.AddWithValue(
                        "@telefono",
                        string.IsNullOrWhiteSpace(
                            proveedor.Telefono)
                            ? (object)DBNull.Value
                            : proveedor.Telefono);

                    cmd.Parameters.AddWithValue(
                        "@direccion",
                        string.IsNullOrWhiteSpace(
                            proveedor.Direccion)
                            ? (object)DBNull.Value
                            : proveedor.Direccion);

                    cmd.Parameters.AddWithValue(
                        "@plazo_pago",
                        proveedor.PlazoPago);

                    cmd.Parameters.AddWithValue(
                        "@activo",
                        proveedor.Activo);

                    cmd.Parameters.AddWithValue(
                        "@fecha_registro",
                        proveedor.FechaRegistro);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Proveedor proveedor)
        {
            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    UPDATE dbo.proveedores
                    SET
                        nombre = @nombre,
                        nit = @nit,
                        email = @email,
                        telefono = @telefono,
                        direccion = @direccion,
                        plazo_pago = @plazo_pago,
                        activo = @activo
                    WHERE id = @id;
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@id",
                        proveedor.Id);

                    cmd.Parameters.AddWithValue(
                        "@nombre",
                        proveedor.Nombre);

                    cmd.Parameters.AddWithValue(
                        "@nit",
                        string.IsNullOrWhiteSpace(
                            proveedor.Nit)
                            ? (object)DBNull.Value
                            : proveedor.Nit);

                    cmd.Parameters.AddWithValue(
                        "@email",
                        string.IsNullOrWhiteSpace(
                            proveedor.Email)
                            ? (object)DBNull.Value
                            : proveedor.Email);

                    cmd.Parameters.AddWithValue(
                        "@telefono",
                        string.IsNullOrWhiteSpace(
                            proveedor.Telefono)
                            ? (object)DBNull.Value
                            : proveedor.Telefono);

                    cmd.Parameters.AddWithValue(
                        "@direccion",
                        string.IsNullOrWhiteSpace(
                            proveedor.Direccion)
                            ? (object)DBNull.Value
                            : proveedor.Direccion);

                    cmd.Parameters.AddWithValue(
                        "@plazo_pago",
                        proveedor.PlazoPago);

                    cmd.Parameters.AddWithValue(
                        "@activo",
                        proveedor.Activo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Desactivar(int id)
        {
            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    UPDATE dbo.proveedores
                    SET activo = 0
                    WHERE id = @id;
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@id",
                        id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}