using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EcoTech.Entidades;
using EcoTech.Utilidades;

namespace EcoTech.Datos
{
    public class D_Usuarios
    {
        private readonly Conexion conexion = new Conexion();

        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        u.id,
                        u.usuario,
                        u.password_hash,
                        u.rol_id,
                        r.nombre AS rol_nombre,
                        u.activo,
                        u.fecha_creacion
                    FROM usuarios u
                    INNER JOIN roles r
                        ON u.rol_id = r.id
                    ORDER BY u.id;
                ";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Usuario
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            NombreUsuario = reader["usuario"].ToString(),
                            PasswordHash = reader["password_hash"].ToString(),
                            RolId = Convert.ToInt32(reader["rol_id"]),
                            RolNombre = reader["rol_nombre"].ToString(),
                            Activo = Convert.ToBoolean(reader["activo"]),
                            FechaCreacion = Convert.ToDateTime(reader["fecha_creacion"])
                        });
                    }
                }
            }

            return lista;
        }

        public Usuario ObtenerPorUsuario(string nombreUsuario)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    SELECT
                        u.id,
                        u.usuario,
                        u.password_hash,
                        u.rol_id,
                        r.nombre AS rol_nombre,
                        u.activo,
                        u.fecha_creacion
                    FROM usuarios u
                    INNER JOIN roles r
                        ON u.rol_id = r.id
                    WHERE u.usuario = @usuario;
                ";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@usuario", nombreUsuario);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                NombreUsuario = reader["usuario"].ToString(),
                                PasswordHash = reader["password_hash"].ToString(),
                                RolId = Convert.ToInt32(reader["rol_id"]),
                                RolNombre = reader["rol_nombre"].ToString(),
                                Activo = Convert.ToBoolean(reader["activo"]),
                                FechaCreacion = Convert.ToDateTime(reader["fecha_creacion"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Insertar(Usuario usuario)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"
                    INSERT INTO usuarios
                    (
                        usuario,
                        password_hash,
                        rol_id,
                        activo,
                        fecha_creacion
                    )
                    VALUES
                    (
                        @usuario,
                        @password_hash,
                        @rol_id,
                        @activo,
                        SYSDATETIME()
                    );
                ";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@password_hash", usuario.PasswordHash);
                    cmd.Parameters.AddWithValue("@rol_id", usuario.RolId);
                    cmd.Parameters.AddWithValue("@activo", usuario.Activo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Editar(Usuario usuario)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql;

                if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
                {
                    sql = @"
                        UPDATE usuarios
                        SET
                            usuario = @usuario,
                            rol_id = @rol_id,
                            activo = @activo
                        WHERE id = @id;
                    ";
                }
                else
                {
                    sql = @"
                        UPDATE usuarios
                        SET
                            usuario = @usuario,
                            password_hash = @password_hash,
                            rol_id = @rol_id,
                            activo = @activo
                        WHERE id = @id;
                    ";
                }

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", usuario.Id);
                    cmd.Parameters.AddWithValue("@usuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@rol_id", usuario.RolId);
                    cmd.Parameters.AddWithValue("@activo", usuario.Activo);

                    if (!string.IsNullOrWhiteSpace(usuario.PasswordHash))
                    {
                        cmd.Parameters.AddWithValue(
                            "@password_hash",
                            usuario.PasswordHash);
                    }

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
                    UPDATE usuarios
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