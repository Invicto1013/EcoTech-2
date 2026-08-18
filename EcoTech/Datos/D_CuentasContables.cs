using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EcoTech.Entidades;
using EcoTech.Utilidades;

namespace EcoTech.Datos
{
    public class D_CuentasContables
    {
        private readonly Conexion conexion =
            new Conexion();

        public List<CuentaContable> Listar()
        {
            List<CuentaContable> lista =
                new List<CuentaContable>();

            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        id,
                        codigo,
                        nombre,
                        tipo,
                        descripcion,
                        activo
                    FROM dbo.cuentas_contables
                    ORDER BY codigo;
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                using (SqlDataReader reader =
                       cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new CuentaContable
                        {
                            Id =
                                Convert.ToInt32(
                                    reader["id"]),

                            Codigo =
                                reader["codigo"]
                                .ToString(),

                            Nombre =
                                reader["nombre"]
                                .ToString(),

                            Tipo =
                                reader["tipo"]
                                .ToString(),

                            Descripcion =
                                reader["descripcion"] ==
                                DBNull.Value
                                    ? null
                                    : reader["descripcion"]
                                        .ToString(),

                            Activo =
                                Convert.ToBoolean(
                                    reader["activo"])
                        });
                    }
                }
            }

            return lista;
        }

        public void Insertar(
            CuentaContable cuenta)
        {
            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    INSERT INTO dbo.cuentas_contables
                    (
                        codigo,
                        nombre,
                        tipo,
                        descripcion,
                        activo
                    )
                    VALUES
                    (
                        @codigo,
                        @nombre,
                        @tipo,
                        @descripcion,
                        @activo
                    );
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@codigo",
                        cuenta.Codigo);

                    cmd.Parameters.AddWithValue(
                        "@nombre",
                        cuenta.Nombre);

                    cmd.Parameters.AddWithValue(
                        "@tipo",
                        cuenta.Tipo);

                    cmd.Parameters.AddWithValue(
                        "@descripcion",
                        string.IsNullOrWhiteSpace(
                            cuenta.Descripcion)
                            ? (object)DBNull.Value
                            : cuenta.Descripcion);

                    cmd.Parameters.AddWithValue(
                        "@activo",
                        cuenta.Activo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Editar(
            CuentaContable cuenta)
        {
            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    UPDATE dbo.cuentas_contables
                    SET
                        codigo = @codigo,
                        nombre = @nombre,
                        tipo = @tipo,
                        descripcion = @descripcion,
                        activo = @activo
                    WHERE id = @id;
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@id",
                        cuenta.Id);

                    cmd.Parameters.AddWithValue(
                        "@codigo",
                        cuenta.Codigo);

                    cmd.Parameters.AddWithValue(
                        "@nombre",
                        cuenta.Nombre);

                    cmd.Parameters.AddWithValue(
                        "@tipo",
                        cuenta.Tipo);

                    cmd.Parameters.AddWithValue(
                        "@descripcion",
                        string.IsNullOrWhiteSpace(
                            cuenta.Descripcion)
                            ? (object)DBNull.Value
                            : cuenta.Descripcion);

                    cmd.Parameters.AddWithValue(
                        "@activo",
                        cuenta.Activo);

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
                    UPDATE dbo.cuentas_contables
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