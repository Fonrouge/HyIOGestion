using Infrastructure.Persistence.MicrosoftSQL;
using SharedAbstractions.Services.Hashing;

namespace BLL.LogicLayer
{
    public class IntegrityBLL : IIntegrityBLL
    {
        private readonly IIntegrityRepository _intRepo;


        public IntegrityBLL(IIntegrityRepository intRepo)
        {
            _intRepo = intRepo;
        }

        public bool ValidateGlobalIntegrity(string tabla, string connectionString)
        {
            // 1. Obtener el DVV que tenemos guardado en la tabla de control
            string storedDvv = _intRepo.GetStoredDVV(tabla, connectionString);

            // 2. Recalcular el DVV actual recorriendo todos los registros de la tabla
            var allHashes = _intRepo.GetVerticalHashes(tabla, connectionString);
            string recalculatedDvv = IntegrityService.CalculateDVV(allHashes);

            // 3. Comparar. Si no coinciden, la tabla fue violada (borrado o inserción externa)
            return storedDvv == recalculatedDvv;
        }
    }
}
