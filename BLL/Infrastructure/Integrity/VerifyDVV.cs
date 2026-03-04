using Domain.Repositories;
using Shared.Services;
using System.Threading.Tasks;

namespace BLL.LogicLayer
{
    public class VerifyDVV : IVerifyDVV
    {
        private readonly IIntegrityVerifierRepository _intRepo;

        public VerifyDVV(IIntegrityVerifierRepository intRepo)
        {
            _intRepo = intRepo;
        }

        // 1. Agregamos 'async' y renombramos a ExecuteAsync
        public async Task<bool> ExecuteAsync(string tabla, string connectionString)
        {
            // 2. Esperamos asíncronamente el DVV guardado
            string storedDvv = await _intRepo.GetStoredDVVAsync(tabla, connectionString);

            // 3. Esperamos asíncronamente todos los hashes para recalcular
            var allHashes = await _intRepo.GetVerticalHashesAsync(tabla, connectionString);

            // El cálculo de DVV es puro CPU (criptografía o concatenación en memoria), así que se mantiene síncrono
            string recalculatedDvv = IntegrityService.CalculateDVV(allHashes);

            // 4. Al tener 'async' en la firma, devolver un bool automáticamente lo envuelve en Task<bool>
            return storedDvv == recalculatedDvv;
        }
    }
}