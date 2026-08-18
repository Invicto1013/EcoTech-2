using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EcoTech.Entidades;
using EcoTech.Utilidades;

namespace EcoTech.Datos
{
    public class D_RecepcionesCompra
    {
        private readonly Conexion conexion =
            new Conexion();

        public List<RecepcionCompra> Listar()
        {
            List<RecepcionCompra> lista =
                new List<RecepcionCompra>();

            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        r.id,
                        r.orden_id,
                        r.usuario_id,
                        r.fecha,
                        r.observacion,

                        p.nombre AS proveedor_nombre,
                        u.usuario AS usuario_nombre,

                        oc.total AS orden_total,
                        oc.estado AS orden_estado

                    FROM dbo.recepciones_compra r

                    INNER JOIN dbo.ordenes_compra oc
                        ON r.orden_id = oc.id

                    INNER JOIN dbo.proveedores p
                        ON oc.proveedor_id = p.id

                    INNER JOIN dbo.usuarios u
                        ON r.usuario_id = u.id

                    ORDER BY
                        r.fecha DESC,
                        r.id DESC;
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                using (SqlDataReader reader =
                       cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new RecepcionCompra
                        {
                            Id =
                                Convert.ToInt32(
                                    reader["id"]),

                            OrdenId =
                                Convert.ToInt32(
                                    reader["orden_id"]),

                            UsuarioId =
                                Convert.ToInt32(
                                    reader["usuario_id"]),

                            Fecha =
                                Convert.ToDateTime(
                                    reader["fecha"]),

                            Observacion =
                                reader["observacion"] == DBNull.Value
                                    ? string.Empty
                                    : reader["observacion"].ToString(),

                            ProveedorNombre =
                                reader["proveedor_nombre"]
                                    .ToString(),

                            UsuarioNombre =
                                reader["usuario_nombre"]
                                    .ToString(),

                            OrdenTotal =
                                Convert.ToDecimal(
                                    reader["orden_total"]),

                            OrdenEstado =
                                reader["orden_estado"]
                                    .ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public List<RecepcionCompra> ListarOrdenesPendientes()
        {
            List<RecepcionCompra> lista =
                new List<RecepcionCompra>();

            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        oc.id AS orden_id,
                        oc.total AS orden_total,
                        oc.estado AS orden_estado,
                        p.nombre AS proveedor_nombre

                    FROM dbo.ordenes_compra oc

                    INNER JOIN dbo.proveedores p
                        ON oc.proveedor_id = p.id

                    WHERE
                        UPPER(LTRIM(RTRIM(oc.estado)))
                            NOT IN ('RECIBIDA')

                        AND NOT EXISTS
                        (
                            SELECT 1
                            FROM dbo.recepciones_compra r
                            WHERE r.orden_id = oc.id
                        )

                    ORDER BY
                        oc.id DESC;
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                using (SqlDataReader reader =
                       cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new RecepcionCompra
                        {
                            OrdenId =
                                Convert.ToInt32(
                                    reader["orden_id"]),

                            OrdenTotal =
                                Convert.ToDecimal(
                                    reader["orden_total"]),

                            OrdenEstado =
                                reader["orden_estado"]
                                    .ToString(),

                            ProveedorNombre =
                                reader["proveedor_nombre"]
                                    .ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public void Registrar(RecepcionCompra recepcion)
        {
            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                using (SqlTransaction transaccion =
                       cn.BeginTransaction())
                {
                    try
                    {
                        string sqlValidar = @"
                            SELECT
                                oc.estado,

                                CASE
                                    WHEN EXISTS
                                    (
                                        SELECT 1
                                        FROM dbo.recepciones_compra r
                                        WHERE r.orden_id = oc.id
                                    )
                                    THEN 1
                                    ELSE 0
                                END AS ya_recibida

                            FROM dbo.ordenes_compra oc

                            WHERE oc.id = @orden_id;
                        ";

                        string estado = null;
                        bool yaRecibida = false;

                        using (SqlCommand cmdValidar =
                               new SqlCommand(
                                   sqlValidar,
                                   cn,
                                   transaccion))
                        {
                            cmdValidar.Parameters.AddWithValue(
                                "@orden_id",
                                recepcion.OrdenId);

                            using (SqlDataReader reader =
                                   cmdValidar.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    throw new Exception(
                                        "La orden de compra seleccionada no existe.");
                                }

                                estado =
                                    reader["estado"]
                                        .ToString();

                                yaRecibida =
                                    Convert.ToInt32(
                                        reader["ya_recibida"]) == 1;
                            }
                        }

                        if (yaRecibida)
                        {
                            throw new Exception(
                                "La orden de compra ya tiene una recepción registrada.");
                        }

                        if (estado != null &&
                            estado.Equals(
                                "RECIBIDA",
                                StringComparison.OrdinalIgnoreCase))
                        {
                            throw new Exception(
                                "La orden de compra ya está marcada como RECIBIDA.");
                        }

                        string sqlInsertar = @"
                            INSERT INTO dbo.recepciones_compra
                            (
                                orden_id,
                                usuario_id,
                                fecha,
                                observacion
                            )
                            VALUES
                            (
                                @orden_id,
                                @usuario_id,
                                @fecha,
                                @observacion
                            );
                        ";

                        using (SqlCommand cmdInsertar =
                               new SqlCommand(
                                   sqlInsertar,
                                   cn,
                                   transaccion))
                        {
                            cmdInsertar.Parameters.AddWithValue(
                                "@orden_id",
                                recepcion.OrdenId);

                            cmdInsertar.Parameters.AddWithValue(
                                "@usuario_id",
                                recepcion.UsuarioId);

                            cmdInsertar.Parameters.AddWithValue(
                                "@fecha",
                                recepcion.Fecha);

                            cmdInsertar.Parameters.AddWithValue(
                                "@observacion",
                                string.IsNullOrWhiteSpace(
                                    recepcion.Observacion)
                                    ? (object)DBNull.Value
                                    : recepcion.Observacion);

                            cmdInsertar.ExecuteNonQuery();
                        }

                        string sqlActualizarOrden = @"
                            UPDATE dbo.ordenes_compra
                            SET estado = 'RECIBIDA'
                            WHERE id = @orden_id;
                        ";

                        using (SqlCommand cmdActualizar =
                               new SqlCommand(
                                   sqlActualizarOrden,
                                   cn,
                                   transaccion))
                        {
                            cmdActualizar.Parameters.AddWithValue(
                                "@orden_id",
                                recepcion.OrdenId);

                            cmdActualizar.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}