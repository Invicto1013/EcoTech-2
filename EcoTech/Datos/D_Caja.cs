using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EcoTech.Entidades;
using EcoTech.Utilidades;

namespace EcoTech.Datos
{
    public class D_Caja
    {
        private readonly Conexion conexion =
            new Conexion();

        public List<Caja> Listar()
        {
            List<Caja> lista =
                new List<Caja>();

            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        c.id,
                        c.usuario_id,
                        c.fecha,
                        c.concepto,
                        c.ingreso,
                        c.egreso,
                        c.saldo,
                        c.tipo,
                        u.usuario AS usuario_nombre
                    FROM dbo.caja c
                    INNER JOIN dbo.usuarios u
                        ON c.usuario_id = u.id
                    ORDER BY
                        c.fecha DESC,
                        c.id DESC;
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                using (SqlDataReader reader =
                       cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Caja
                        {
                            Id =
                                Convert.ToInt32(
                                    reader["id"]),

                            UsuarioId =
                                Convert.ToInt32(
                                    reader["usuario_id"]),

                            Fecha =
                                Convert.ToDateTime(
                                    reader["fecha"]),

                            Concepto =
                                reader["concepto"]
                                .ToString(),

                            Ingreso =
                                Convert.ToDecimal(
                                    reader["ingreso"]),

                            Egreso =
                                Convert.ToDecimal(
                                    reader["egreso"]),

                            Saldo =
                                Convert.ToDecimal(
                                    reader["saldo"]),

                            Tipo =
                                reader["tipo"]
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

        public decimal ObtenerSaldoActual()
        {
            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT ISNULL(
                        (
                            SELECT TOP 1 saldo
                            FROM dbo.caja
                            ORDER BY id DESC
                        ),
                        0
                    );
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                {
                    object resultado =
                        cmd.ExecuteScalar();

                    return Convert.ToDecimal(resultado);
                }
            }
        }

        public decimal Registrar(Caja caja)
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
                        string sqlSaldo = @"
                            SELECT ISNULL(
                                (
                                    SELECT TOP 1 saldo
                                    FROM dbo.caja WITH (UPDLOCK)
                                    ORDER BY id DESC
                                ),
                                0
                            );
                        ";

                        decimal saldoAnterior;

                        using (SqlCommand cmdSaldo =
                               new SqlCommand(
                                   sqlSaldo,
                                   cn,
                                   transaccion))
                        {
                            saldoAnterior =
                                Convert.ToDecimal(
                                    cmdSaldo.ExecuteScalar());
                        }

                        decimal saldoNuevo =
                            saldoAnterior
                            + caja.Ingreso
                            - caja.Egreso;

                        string sqlInsertar = @"
                            INSERT INTO dbo.caja
                            (
                                usuario_id,
                                fecha,
                                concepto,
                                ingreso,
                                egreso,
                                saldo,
                                tipo
                            )
                            VALUES
                            (
                                @usuario_id,
                                @fecha,
                                @concepto,
                                @ingreso,
                                @egreso,
                                @saldo,
                                @tipo
                            );
                        ";

                        using (SqlCommand cmd =
                               new SqlCommand(
                                   sqlInsertar,
                                   cn,
                                   transaccion))
                        {
                            cmd.Parameters.AddWithValue(
                                "@usuario_id",
                                caja.UsuarioId);

                            cmd.Parameters.AddWithValue(
                                "@fecha",
                                caja.Fecha);

                            cmd.Parameters.AddWithValue(
                                "@concepto",
                                caja.Concepto);

                            cmd.Parameters.AddWithValue(
                                "@ingreso",
                                caja.Ingreso);

                            cmd.Parameters.AddWithValue(
                                "@egreso",
                                caja.Egreso);

                            cmd.Parameters.AddWithValue(
                                "@saldo",
                                saldoNuevo);

                            cmd.Parameters.AddWithValue(
                                "@tipo",
                                caja.Tipo);

                            cmd.ExecuteNonQuery();
                        }

                        transaccion.Commit();

                        return saldoNuevo;
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