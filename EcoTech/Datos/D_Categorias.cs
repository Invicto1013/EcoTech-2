using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EcoTech.Entidades;
using EcoTech.Utilidades;

namespace EcoTech.Datos
{
    public class D_Categorias
    {
        private readonly Conexion conexion =
            new Conexion();

        public List<Categoria> Listar()
        {
            List<Categoria> lista =
                new List<Categoria>();

            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        id,
                        nombre,
                        descripcion,
                        activo
                    FROM dbo.categorias
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
                        lista.Add(new Categoria
                        {
                            Id =
                                Convert.ToInt32(
                                    reader["id"]),

                            Nombre =
                                reader["nombre"]
                                .ToString(),

                            Descripcion =
                                reader["descripcion"] == DBNull.Value
                                    ? string.Empty
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

        public void Insertar(Categoria categoria)
        {
            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    INSERT INTO dbo.categorias
                    (
                        nombre,
                        descripcion,
                        activo
                    )
                    VALUES
                    (
                        @nombre,
                        @descripcion,
                        @activo
                    );
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@nombre",
                        categoria.Nombre);

                    cmd.Parameters.AddWithValue(
                        "@descripcion",
                        string.IsNullOrWhiteSpace(
                            categoria.Descripcion)
                            ? (object)DBNull.Value
                            : categoria.Descripcion);

                    cmd.Parameters.AddWithValue(
                        "@activo",
                        categoria.Activo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Categoria categoria)
        {
            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    UPDATE dbo.categorias
                    SET
                        nombre = @nombre,
                        descripcion = @descripcion,
                        activo = @activo
                    WHERE id = @id;
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@id",
                        categoria.Id);

                    cmd.Parameters.AddWithValue(
                        "@nombre",
                        categoria.Nombre);

                    cmd.Parameters.AddWithValue(
                        "@descripcion",
                        string.IsNullOrWhiteSpace(
                            categoria.Descripcion)
                            ? (object)DBNull.Value
                            : categoria.Descripcion);

                    cmd.Parameters.AddWithValue(
                        "@activo",
                        categoria.Activo);

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
                    UPDATE dbo.categorias
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