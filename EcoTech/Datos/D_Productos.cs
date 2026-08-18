using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EcoTech.Entidades;
using EcoTech.Utilidades;

namespace EcoTech.Datos
{
    public class D_Productos
    {
        private readonly Conexion conexion = new Conexion();

        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        p.id,
                        p.codigo,
                        p.nombre,
                        p.categoria_id,
                        c.nombre AS categoria_nombre,
                        p.precio_costo,
                        p.precio_venta,
                        p.stock,
                        p.stock_minimo,
                        p.activo,
                        p.fecha_registro
                    FROM dbo.productos p
                    INNER JOIN dbo.categorias c
                        ON p.categoria_id = c.id
                    ORDER BY p.id;
                ";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Producto
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Codigo = reader["codigo"].ToString(),
                            Nombre = reader["nombre"].ToString(),
                            CategoriaId =
                                Convert.ToInt32(reader["categoria_id"]),
                            CategoriaNombre =
                                reader["categoria_nombre"].ToString(),
                            PrecioCosto =
                                Convert.ToDecimal(reader["precio_costo"]),
                            PrecioVenta =
                                Convert.ToDecimal(reader["precio_venta"]),
                            Stock =
                                Convert.ToDecimal(reader["stock"]),
                            StockMinimo =
                                Convert.ToDecimal(reader["stock_minimo"]),
                            Activo =
                                Convert.ToBoolean(reader["activo"]),
                            FechaRegistro =
                                Convert.ToDateTime(reader["fecha_registro"])
                        });
                    }
                }
            }

            return lista;
        }

        public void Insertar(Producto producto)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    INSERT INTO dbo.productos
                    (
                        codigo,
                        nombre,
                        categoria_id,
                        precio_costo,
                        precio_venta,
                        stock,
                        stock_minimo,
                        activo,
                        fecha_registro
                    )
                    VALUES
                    (
                        @codigo,
                        @nombre,
                        @categoria_id,
                        @precio_costo,
                        @precio_venta,
                        @stock,
                        @stock_minimo,
                        @activo,
                        SYSDATETIME()
                    );
                ";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@codigo",
                        producto.Codigo);

                    cmd.Parameters.AddWithValue(
                        "@nombre",
                        producto.Nombre);

                    cmd.Parameters.AddWithValue(
                        "@categoria_id",
                        producto.CategoriaId);

                    cmd.Parameters.AddWithValue(
                        "@precio_costo",
                        producto.PrecioCosto);

                    cmd.Parameters.AddWithValue(
                        "@precio_venta",
                        producto.PrecioVenta);

                    cmd.Parameters.AddWithValue(
                        "@stock",
                        producto.Stock);

                    cmd.Parameters.AddWithValue(
                        "@stock_minimo",
                        producto.StockMinimo);

                    cmd.Parameters.AddWithValue(
                        "@activo",
                        producto.Activo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Editar(Producto producto)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    UPDATE dbo.productos
                    SET
                        codigo = @codigo,
                        nombre = @nombre,
                        categoria_id = @categoria_id,
                        precio_costo = @precio_costo,
                        precio_venta = @precio_venta,
                        stock_minimo = @stock_minimo,
                        activo = @activo
                    WHERE id = @id;
                ";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@id",
                        producto.Id);

                    cmd.Parameters.AddWithValue(
                        "@codigo",
                        producto.Codigo);

                    cmd.Parameters.AddWithValue(
                        "@nombre",
                        producto.Nombre);

                    cmd.Parameters.AddWithValue(
                        "@categoria_id",
                        producto.CategoriaId);

                    cmd.Parameters.AddWithValue(
                        "@precio_costo",
                        producto.PrecioCosto);

                    cmd.Parameters.AddWithValue(
                        "@precio_venta",
                        producto.PrecioVenta);

                    // El stock NO se modifica desde la edición
                    // del producto. Se manejará mediante
                    // movimientos de inventario.
                    cmd.Parameters.AddWithValue(
                        "@stock_minimo",
                        producto.StockMinimo);

                    cmd.Parameters.AddWithValue(
                        "@activo",
                        producto.Activo);

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
                    UPDATE dbo.productos
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