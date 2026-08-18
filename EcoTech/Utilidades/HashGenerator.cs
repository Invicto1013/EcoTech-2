using System;

namespace EcoTech.Utilidades
{
    public static class HashGenerator
    {
        public static string GenerarHash(string password)
        {
            return PasswordHelper.HashPassword(password);
        }
    }
}