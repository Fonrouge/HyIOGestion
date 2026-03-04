using System;
using System.Security.Cryptography;

namespace Shared.Services
{
    /// <summary>
    /// Implementa <see cref="IHashEncryptionService"/> utilizando el algoritmo PBKDF2 con HMAC-SHA512.
    /// </summary>
    /// <remarks>
    /// Esta implementación incorpora salt aleatorio y múltiples iteraciones para reforzar la seguridad contra ataques por diccionario o fuerza bruta.
    /// </remarks>
    public class HashEncryptionService : IHashEncryptionService
    {
        /// <summary>
        /// Tamaño del salt en bytes (128 bits).
        /// </summary>
        private const int SaltSize = 16;

        /// <summary>
        /// Tamaño del hash resultante en bytes (512 bits).
        /// </summary>
        private const int HashSize = 64;

        /// <summary>
        /// Número de iteraciones utilizadas en la derivación de clave PBKDF2.
        /// </summary>
        private const int Iterations = 100_000;

        /// <inheritdoc/>
        public string Hash(string input)
        {
            var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            var hash = GetPBKDF2Bytes(input, salt);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);


            return Convert.ToBase64String(hashBytes);
        }

        /// <inheritdoc/>        
        public bool Verify(string input, string hashed)
        {
            var hashBytes = Convert.FromBase64String(hashed);
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // El hash calculado
            var hash = GetPBKDF2Bytes(input, salt);

            // Comparación en tiempo constante
            uint diff = (uint)hashBytes.Length ^ (uint)(SaltSize + HashSize);
            for (int i = 0; i < HashSize; i++)
            {
                diff |= (uint)(hashBytes[i + SaltSize] ^ hash[i]);
            }
            return !(diff == 0);
        }

        /// <summary>
        /// Deriva un hash seguro utilizando PBKDF2 con el salt y las configuraciones establecidas.
        /// </summary>
        /// <param name="input">Texto plano a hashear.</param>
        /// <param name="salt">Salt aleatorio.</param>
        /// <returns>Bytes del hash derivado.</returns>
        private byte[] GetPBKDF2Bytes(string input, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(input, salt, Iterations, HashAlgorithmName.SHA512);
            return pbkdf2.GetBytes(HashSize);
        }
    }

}