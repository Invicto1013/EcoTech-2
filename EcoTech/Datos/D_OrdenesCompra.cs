using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EcoTech.Entidades;
using EcoTech.Utilidades;

namespace EcoTech.Datos
{
    public class D_OrdenesCompra
    {
        private readonly Conexion conexion = new Conexion();

        public List<OrdenCompra> Listar()
        {
            List<OrdenCompra> lista = new List<OrdenCompra>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        o.id,
                        o.proveedor_id,
                        o.usuario_id,
                        o.fecha,
                        o.fecha_entrega,
                        o.subtotal,
                        o.impuesto,
                        o.total,
                        o.estado,
                        p.nombre AS proveedor_nombre,
                        u.usuario AS usuario_nombre
                    FROM dbo.ordenes_compra o
                    INNER JOIN dbo.proveedores p
                        ON o.proveedor_id = p.id
                    INNER JOIN dbo.usuarios u
                        ON o.usuario_id = u.id
                    ORDER BY o.id DESC;
                ";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new OrdenCompra
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            ProveedorId = Convert.ToInt32(reader["proveedor_id"]),
                            UsuarioId = Convert.ToInt32(reader["usuario_id"]),
                            Fecha = Convert.ToDateTime(reader["fecha"]),

                            FechaEntrega = reader["fecha_entrega"] == DBNull.Value
                                ? (DateTime?)null
                                : Convert.ToDateTime(reader["fecha_entrega"]),

                            Subtotal = Convert.ToDecimal(reader["subtotal"]),
                            Impuesto = Convert.ToDecimal(reader["impuesto"]),
                            Total = Convert.ToDecimal(reader["total"]),
                            Estado = reader["estado"].ToString(),
                            ProveedorNombre = reader["proveedor_nombre"].ToString(),
                            UsuarioNombre = reader["usuario_nombre"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public List<DetalleOrden> ListarDetalles(int ordenId)
        {
            List<DetalleOrden> lista = new List<DetalleOrden>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        d.id,
                        d.orden_id,
                        d.producto_id,
                        d.cantidad,
                        d.precio,
                        d.subtotal,
                        p.codigo AS producto_codigo,
                        p.nombre AS producto_nombre
                    FROM dbo.detalles_orden d
                    INNER JOIN dbo.productos p
                        ON d.producto_id = p.id
                    WHERE d.orden_id = @orden_id
                    ORDER BY d.id;
                ";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@orden_id", ordenId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new DetalleOrden
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                OrdenId = Convert.ToInt32(reader["orden_id"]),
                                ProductoId = Convert.ToInt32(reader["producto_id"]),
                                Cantidad = Convert.ToDecimal(reader["cantidad"]),
                                Precio = Convert.ToDecimal(reader["precio"]),
                                Subtotal = Convert.ToDecimal(reader["subtotal"]),
                                ProductoCodigo = reader["producto_codigo"].ToString(),
                                ProductoNombre = reader["producto_nombre"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public int Insertar(
            OrdenCompra orden,
            List<DetalleOrden> detalles)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                using (SqlTransaction transaction =
                       cn.BeginTransaction())
                {
                    try
                    {
                        string sqlOrden = @"
                            INSERT INTO dbo.ordenes_compra
                            (
                                proveedor_id,
                                usuario_id,
                                fecha,
                                fecha_entrega,
                                subtotal,
                                impuesto,
                                total,
                                estado
                            )
                            VALUES
                            (
                                @proveedor_id,
                                @usuario_id,
                                @fecha,
                                @fecha_entrega,
                                @subtotal,
                                @impuesto,
                                @total,
                                @estado
                            );

                            SELECT CAST(SCOPE_IDENTITY() AS INT);
                        ";

                        int ordenId;

                        using (SqlCommand cmd =
                               new SqlCommand(
                                   sqlOrden,
                                   cn,
                                   transaction))
                        {
                            cmd.Parameters.AddWithValue(
                                "@proveedor_id",
                                orden.ProveedorId);

                            cmd.Parameters.AddWithValue(
                                "@usuario_id",
                                orden.UsuarioId);

                            cmd.Parameters.AddWithValue(
                                "@fecha",
                                orden.Fecha);

                            cmd.Parameters.AddWithValue(
                                "@fecha_entrega",
                                orden.FechaEntrega.HasValue
                                    ? (object)orden.FechaEntrega.Value
                                    : DBNull.Value);

                            cmd.Parameters.AddWithValue(
                                "@subtotal",
                                orden.Subtotal);

                            cmd.Parameters.AddWithValue(
                                "@impuesto",
                                orden.Impuesto);

                            cmd.Parameters.AddWithValue(
                                "@total",
                                orden.Total);

                            cmd.Parameters.AddWithValue(
                                "@estado",
                                orden.Estado);

                            ordenId =
                                Convert.ToInt32(
                                    cmd.ExecuteScalar());
                        }

                        foreach (DetalleOrden detalle in detalles)
                        {
                            string sqlDetalle = @"
                                INSERT INTO dbo.detalles_orden
                                (
                                    orden_id,
                                    producto_id,
                                    cantidad,
                                    precio,
                                    subtotal
                                )
                                VALUES
                                (
                                    @orden_id,
                                    @producto_id,
                                    @cantidad,
                                    @precio,
                                    @subtotal
                                );
                            ";

                            using (SqlCommand cmd =
                                   new SqlCommand(
                                       sqlDetalle,
                                       cn,
                                       transaction))
                            {
                                cmd.Parameters.AddWithValue(
                                    "@orden_id",
                                    ordenId);

                                cmd.Parameters.AddWithValue(
                                    "@producto_id",
                                    detalle.ProductoId);

                                cmd.Parameters.AddWithValue(
                                    "@cantidad",
                                    detalle.Cantidad);

                                cmd.Parameters.AddWithValue(
                                    "@precio",
                                    detalle.Precio);

                                cmd.Parameters.AddWithValue(
                                    "@subtotal",
                                    detalle.Subtotal);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();

                        return ordenId;
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
}