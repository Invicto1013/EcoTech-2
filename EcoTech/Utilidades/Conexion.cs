using System.Configuration;
using System.Data.SqlClient;

namespace EcoTech.Utilidades
{
    public class Conexion
    {
        private readonly string cadenaConexion;

        public Conexion()
        {
            cadenaConexion = ConfigurationManager
                .ConnectionStrings["EcoTechConnection"]
                .ConnectionString;
        }

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}