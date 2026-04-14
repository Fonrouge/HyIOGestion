using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Services
{
    public static class IntegrityService
    {
        /// <summary>
        /// Genera un Hash SHA256 de una cadena de texto.
        /// </summary>
        public static string Hash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }



        /// <summary>
        /// Recibe N parámetros, los concatena y devuelve el hash resultante.
        /// Ideal para generar el DVH de cualquier entidad.
        /// </summary>
        public static string GetIntegrityHash(params object[] parameters)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var p in parameters)
            {
                // Manejamos nulos para evitar excepciones y asegurar consistencia
                sb.Append(p?.ToString() ?? string.Empty);
            }

            return Hash(sb.ToString());
        }

        public static string CalculateDVV(List<string> allDvhs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var dvh in allDvhs)
            {
                sb.Append(dvh);
            }
            return Hash(sb.ToString()); // Reutiliza tu método SHA256
        }
    }
}