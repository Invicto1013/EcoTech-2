using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EcoTech.Entidades;
using EcoTech.Utilidades;

namespace EcoTech.Datos
{
    public class D_Clientes
    {
        private readonly Conexion conexion = new Conexion();

        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();

            using (SqlConnection cn = conexion.ObtenerConexion())
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
                        limite_credito,
                        activo,
                        fecha_registro
                    FROM dbo.clientes
                    ORDER BY id;
                ";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Cliente
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Nombre = reader["nombre"].ToString(),
                            Nit = reader["nit"] == DBNull.Value
                                ? string.Empty
                                : reader["nit"].ToString(),
                            Email = reader["email"] == DBNull.Value
                                ? string.Empty
                                : reader["email"].ToString(),
                            Telefono = reader["telefono"] == DBNull.Value
                                ? string.Empty
                                : reader["telefono"].ToString(),
                            Direccion = reader["direccion"] == DBNull.Value
                                ? string.Empty
                                : reader["direccion"].ToString(),
                            LimiteCredito = Convert.ToDecimal(
                                reader["limite_credito"]),
                            Activo = Convert.ToBoolean(
                                reader["activo"]),
                            FechaRegistro = Convert.ToDateTime(
                                reader["fecha_registro"])
                        });
                    }
                }
            }

            return lista;
        }

        public void Insertar(Cliente cliente)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    INSERT INTO dbo.clientes
                    (
                        nombre,
                        nit,
                        email,
                        telefono,
                        direccion,
                        limite_credito,
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
                        @limite_credito,
                        @activo,
                        SYSDATETIME()
                    );
                ";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@nombre",
                        cliente.Nombre);

                    cmd.Parameters.AddWithValue(
                        "@nit",
                        string.IsNullOrWhiteSpace(cliente.Nit)
                            ? (object)DBNull.Value
                            : cliente.Nit);

                    cmd.Parameters.AddWithValue(
                        "@email",
                        string.IsNullOrWhiteSpace(cliente.Email)
                            ? (object)DBNull.Value
                            : cliente.Email);

                    cmd.Parameters.AddWithValue(
                        "@telefono",
                        string.IsNullOrWhiteSpace(cliente.Telefono)
                            ? (object)DBNull.Value
                            : cliente.Telefono);

                    cmd.Parameters.AddWithValue(
                        "@direccion",
                        string.IsNullOrWhiteSpace(cliente.Direccion)
                            ? (object)DBNull.Value
                            : cliente.Direccion);

                    cmd.Parameters.AddWithValue(
                        "@limite_credito",
                        cliente.LimiteCredito);

                    cmd.Parameters.AddWithValue(
                        "@activo",
                        cliente.Activo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Editar(Cliente cliente)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    UPDATE dbo.clientes
                    SET
                        nombre = @nombre,
                        nit = @nit,
                        email = @email,
                        telefono = @telefono,
                        direccion = @direccion,
                        limite_credito = @limite_credito,
                        activo = @activo
                    WHERE id = @id;
                ";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@id",
                        cliente.Id);

                    cmd.Parameters.AddWithValue(
                        "@nombre",
                        cliente.Nombre);

                    cmd.Parameters.AddWithValue(
                        "@nit",
                        string.IsNullOrWhiteSpace(cliente.Nit)
                            ? (object)DBNull.Value
                            : cliente.Nit);

                    cmd.Parameters.AddWithValue(
                        "@email",
                        string.IsNullOrWhiteSpace(cliente.Email)
                            ? (object)DBNull.Value
                            : cliente.Email);

                    cmd.Parameters.AddWithValue(
                        "@telefono",
                        string.IsNullOrWhiteSpace(cliente.Telefono)
                            ? (object)DBNull.Value
                            : cliente.Telefono);

                    cmd.Parameters.AddWithValue(
                        "@direccion",
                        string.IsNullOrWhiteSpace(cliente.Direccion)
                            ? (object)DBNull.Value
                            : cliente.Direccion);

                    cmd.Parameters.AddWithValue(
                        "@limite_credito",
                        cliente.LimiteCredito);

                    cmd.Parameters.AddWithValue(
                        "@activo",
                        cliente.Activo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Desactivar(int id)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    UPDATE dbo.clientes
                    SET activo = 0
                    WHERE id = @id;
                ";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}