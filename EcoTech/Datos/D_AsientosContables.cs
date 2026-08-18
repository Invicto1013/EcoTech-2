using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EcoTech.Entidades;
using EcoTech.Utilidades;

namespace EcoTech.Datos
{
    public class D_AsientosContables
    {
        private readonly Conexion conexion =
            new Conexion();

        public List<AsientoContable> Listar()
        {
            List<AsientoContable> lista =
                new List<AsientoContable>();

            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        a.id,
                        a.fecha,
                        a.concepto,
                        a.cuenta_id,
                        c.codigo AS cuenta_codigo,
                        c.nombre AS cuenta_nombre,
                        a.debe,
                        a.haber,
                        a.usuario_id,
                        u.usuario AS usuario_nombre
                    FROM dbo.asientos_contables a
                    INNER JOIN dbo.cuentas_contables c
                        ON a.cuenta_id = c.id
                    INNER JOIN dbo.usuarios u
                        ON a.usuario_id = u.id
                    ORDER BY
                        a.fecha DESC,
                        a.id DESC;
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                using (SqlDataReader reader =
                       cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new AsientoContable
                        {
                            Id =
                                Convert.ToInt32(
                                    reader["id"]),

                            Fecha =
                                Convert.ToDateTime(
                                    reader["fecha"]),

                            Concepto =
                                reader["concepto"]
                                .ToString(),

                            CuentaId =
                                Convert.ToInt32(
                                    reader["cuenta_id"]),

                            CuentaCodigo =
                                reader["cuenta_codigo"]
                                .ToString(),

                            CuentaNombre =
                                reader["cuenta_nombre"]
                                .ToString(),

                            Debe =
                                Convert.ToDecimal(
                                    reader["debe"]),

                            Haber =
                                Convert.ToDecimal(
                                    reader["haber"]),

                            UsuarioId =
                                Convert.ToInt32(
                                    reader["usuario_id"]),

                            UsuarioNombre =
                                reader["usuario_nombre"]
                                .ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public void Insertar(AsientoContable asiento)
        {
            using (SqlConnection cn =
                   conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    INSERT INTO dbo.asientos_contables
                    (
                        fecha,
                        concepto,
                        cuenta_id,
                        debe,
                        haber,
                        usuario_id
                    )
                    VALUES
                    (
                        @fecha,
                        @concepto,
                        @cuenta_id,
                        @debe,
                        @haber,
                        @usuario_id
                    );
                ";

                using (SqlCommand cmd =
                       new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@fecha",
                        asiento.Fecha);

                    cmd.Parameters.AddWithValue(
                        "@concepto",
                        asiento.Concepto);

                    cmd.Parameters.AddWithValue(
                        "@cuenta_id",
                        asiento.CuentaId);

                    cmd.Parameters.AddWithValue(
                        "@debe",
                        asiento.Debe);

                    cmd.Parameters.AddWithValue(
                        "@haber",
                        asiento.Haber);

                    cmd.Parameters.AddWithValue(
                        "@usuario_id",
                        asiento.UsuarioId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}