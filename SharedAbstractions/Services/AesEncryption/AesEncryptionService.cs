using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Services
{
    /// <summary>
    /// Servicio de cifrado basado en AES con clave de 256 bits en modo CBC, encapsulado como adapter técnico reutilizable.
    /// </summary>
    /// <remarks>
    /// Diseñado para proveer operaciones simples de cifrado y descifrado sin exponer detalles internos del algoritmo.
    /// La clave y el vector de inicialización (IV) se derivan automáticamente a partir de un secreto textual utilizando SHA-256:
    /// - El hash completo actúa como clave simétrica.
    /// - Los primeros 16 bytes (128 bits) se utilizan como IV en modo CBC.

    /// Ideal para escenarios que requieran almacenamiento técnico seguro, como buffers cifrados, auditoría resiliente o transporte de datos sensibles.

    /// Este adapter no gestiona el almacenamiento del dato ni la persistencia del secreto; solo encapsula la operación criptográfica.
    /// </remarks>
    public class AesEncryptionService: IAesEncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        /// <summary>
        /// Inicializa el servicio de cifrado con un secreto compartido, desde el cual se derivan la clave y el IV.
        /// </summary>
        /// <param name="secret">
        /// Texto base utilizado para generar la clave simétrica y el vector de inicialización.
        /// Debe ser suficientemente complejo para garantizar entropía adecuada.
        /// </param>
        public AesEncryptionService(string secret)
        {
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(secret));
                _key = hash;
                _iv = hash.Take(16).ToArray(); // IV de 128 bits
            }
        }

        /// <summary>
        /// Cifra los datos binarios usando el algoritmo AES-256-CBC.
        /// </summary>
        /// <param name="data">Contenido sin cifrar, representado como array de bytes.</param>
        /// <returns>Array de bytes cifrado mediante AES.</returns>
        public byte[] Encrypt(byte[] data)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                using (var encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }

        /// <summary>
        /// Descifra datos previamente cifrados con AES-256-CBC.
        /// </summary>
        /// <param name="data">Contenido cifrado, representado como array de bytes.</param>
        /// <returns>Array de bytes descifrado.</returns>
        public byte[] Decrypt(byte[] data)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                using (var decryptor = aes.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }
    }
}