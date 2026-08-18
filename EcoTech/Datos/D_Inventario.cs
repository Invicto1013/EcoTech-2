using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EcoTech.Entidades;
using EcoTech.Utilidades;

namespace EcoTech.Datos
{
    public class D_Inventario
    {
        private readonly Conexion conexion = new Conexion();

        public List<MovimientoInventario> ListarMovimientos()
        {
            List<MovimientoInventario> lista =
                new List<MovimientoInventario>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        m.id,
                        m.producto_id,
                        m.usuario_id,
                        m.tipo,
                        m.cantidad,
                        m.stock_anterior,
                        m.stock_nuevo,
                        m.fecha,
                        m.concepto,
                        m.referencia,
                        p.codigo AS producto_codigo,
                        p.nombre AS producto_nombre,
                        u.usuario AS usuario_nombre
                    FROM dbo.movimientos_inventario m
                    INNER JOIN dbo.productos p
                        ON m.producto_id = p.id
                    INNER JOIN dbo.usuarios u
                        ON m.usuario_id = u.id
                    ORDER BY m.fecha DESC, m.id DESC;
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                using (SqlDataReader reader =
                       cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new MovimientoInventario
                        {
                            Id = Convert.ToInt32(
                                reader["id"]),

                            ProductoId = Convert.ToInt32(
                                reader["producto_id"]),

                            UsuarioId = Convert.ToInt32(
                                reader["usuario_id"]),

                            Tipo = reader["tipo"].ToString(),

                            Cantidad = Convert.ToDecimal(
                                reader["cantidad"]),

                            StockAnterior = Convert.ToDecimal(
                                reader["stock_anterior"]),

                            StockNuevo = Convert.ToDecimal(
                                reader["stock_nuevo"]),

                            Fecha = Convert.ToDateTime(
                                reader["fecha"]),

                            Concepto = reader["concepto"]
                                .ToString(),

                            Referencia =
                                reader["referencia"] == DBNull.Value
                                    ? string.Empty
                                    : reader["referencia"].ToString(),

                            ProductoCodigo =
                                reader["producto_codigo"]
                                    .ToString(),

                            ProductoNombre =
                                reader["producto_nombre"]
                                    .ToString(),

                            UsuarioNombre =
                                reader["usuario_nombre"]
                                    .ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public List<ProductoInventario> ListarProductos()
        {
            List<ProductoInventario> lista =
                new List<ProductoInventario>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        p.id,
                        p.codigo,
                        p.nombre,
                        p.precio_costo,
                        p.precio_venta,
                        p.stock,
                        p.stock_minimo,
                        p.activo,
                        c.nombre AS categoria
                    FROM dbo.productos p
                    INNER JOIN dbo.categorias c
                        ON p.categoria_id = c.id
                    ORDER BY p.nombre;
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                using (SqlDataReader reader =
                       cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ProductoInventario
                        {
                            Id = Convert.ToInt32(
                                reader["id"]),

                            Codigo = reader["codigo"]
                                .ToString(),

                            Nombre = reader["nombre"]
                                .ToString(),

                            Categoria = reader["categoria"]
                                .ToString(),

                            PrecioCosto = Convert.ToDecimal(
                                reader["precio_costo"]),

                            PrecioVenta = Convert.ToDecimal(
                                reader["precio_venta"]),

                            Stock = Convert.ToDecimal(
                                reader["stock"]),

                            StockMinimo = Convert.ToDecimal(
                                reader["stock_minimo"]),

                            Activo = Convert.ToBoolean(
                                reader["activo"])
                        });
                    }
                }
            }

            return lista;
        }

        public void RegistrarMovimiento(
            MovimientoInventario movimiento)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                using (SqlTransaction transaction =
                       cn.BeginTransaction())
                {
                    try
                    {
                        string sqlProducto = @"
                            UPDATE dbo.productos
                            SET stock = @stock_nuevo
                            WHERE id = @producto_id;
                        ";

                        using (SqlCommand cmd =
                               new SqlCommand(
                                   sqlProducto,
                                   cn,
                                   transaction))
                        {
                            cmd.Parameters.AddWithValue(
                                "@stock_nuevo",
                                movimiento.StockNuevo);

                            cmd.Parameters.AddWithValue(
                                "@producto_id",
                                movimiento.ProductoId);

                            cmd.ExecuteNonQuery();
                        }

                        string sqlMovimiento = @"
                            INSERT INTO dbo.movimientos_inventario
                            (
                                producto_id,
                                usuario_id,
                                tipo,
                                cantidad,
                                stock_anterior,
                                stock_nuevo,
                                fecha,
                                concepto,
                                referencia
                            )
                            VALUES
                            (
                                @producto_id,
                                @usuario_id,
                                @tipo,
                                @cantidad,
                                @stock_anterior,
                                @stock_nuevo,
                                SYSDATETIME(),
                                @concepto,
                                @referencia
                            );
                        ";

                        using (SqlCommand cmd =
                               new SqlCommand(
                                   sqlMovimiento,
                                   cn,
                                   transaction))
                        {
                            cmd.Parameters.AddWithValue(
                                "@producto_id",
                                movimiento.ProductoId);

                            cmd.Parameters.AddWithValue(
                                "@usuario_id",
                                movimiento.UsuarioId);

                            cmd.Parameters.AddWithValue(
                                "@tipo",
                                movimiento.Tipo);

                            cmd.Parameters.AddWithValue(
                                "@cantidad",
                                movimiento.Cantidad);

                            cmd.Parameters.AddWithValue(
                                "@stock_anterior",
                                movimiento.StockAnterior);

                            cmd.Parameters.AddWithValue(
                                "@stock_nuevo",
                                movimiento.StockNuevo);

                            cmd.Parameters.AddWithValue(
                                "@concepto",
                                movimiento.Concepto);

                            cmd.Parameters.AddWithValue(
                                "@referencia",
                                string.IsNullOrWhiteSpace(
                                    movimiento.Referencia)
                                    ? (object)DBNull.Value
                                    : movimiento.Referencia);

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }

    public class ProductoInventario
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public string Categoria { get; set; }

        public decimal PrecioCosto { get; set; }

        public decimal PrecioVenta { get; set; }

        public decimal Stock { get; set; }

        public decimal StockMinimo { get; set; }

        public bool Activo { get; set; }

        public override string ToString()
        {
            return $"{Codigo} - {Nombre}";
        }
    }
}