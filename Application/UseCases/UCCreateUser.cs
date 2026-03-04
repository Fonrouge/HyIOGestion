using BLL.DTOs;
using BLL.DTOs.Mappers;
using Infrastructure.Persistence.MicrosoftSQL;
using Shared;
using Shared.Services;
using SharedAbstractions.Services.Hashing;
using System.Transactions;

namespace BLL.UseCases
{
    public class UCCreateUser
    {
        private readonly UserRepository _userRepo = new UserRepository();
        private readonly IntegrityRepository _integrityRepo = new IntegrityRepository();
        private readonly EmployeeRepository _employeeRepo = new EmployeeRepository();

        public void Create(UsuarioDTO userDto)
        {
            // 1. Mapeo único (No volver a mapear después)
            var userEntity = UsuarioMapper.ToEntity(userDto);
            var employeeEntity = EmployeeMapper.ToEntity(userDto.EmployeeDTO);

            // 2. Procesamiento de Seguridad
            // Importante: Vinculamos las entidades para que el ID coincida
            userEntity.Employee = employeeEntity;
            userEntity.Password = new HashEncryptionService().Hash(userEntity.Password);

            // 3. Cálculo de Integridad Horizontal (DVH)
            // Usamos el método genérico que armamos en la clase estática
            userEntity.DVH = IntegrityService.GetIntegrityHash
            (
                userEntity.Id,
                userEntity.Username,
                userEntity.Password,
                userEntity.Language,
                userEntity.Employee.Id
            );

            employeeEntity.DVH = IntegrityService.GetIntegrityHash
            (
                employeeEntity.Id,
                employeeEntity.FileNumber,
                employeeEntity.FirstName,
                employeeEntity.LastName,
                employeeEntity.NationalId,
                employeeEntity.Active
            );

            // 4. Persistencia en el orden correcto
            // Primero el empleado porque el usuario depende de su ID (lógicamente)
            _employeeRepo.Create(employeeEntity);
            _userRepo.Create(userEntity); // USAMOS userEntity, NO el Mapper de nuevo


            // 5. Integridad Vertical (DVV) - SIEMPRE AL FINAL
            ActualizarDVV("User", ApplicationSettings.SecurityConnection);
            ActualizarDVV("Employee", ApplicationSettings.EntitiesConnection);
        }

        private void ActualizarDVV(string nombreTabla, string connectionString)
        {
            var hashes = _integrityRepo.GetVerticalHashes(nombreTabla, connectionString);
            var dvvFinal = IntegrityService.CalculateDVV(hashes);
            _integrityRepo.UpdateDVV(nombreTabla, dvvFinal, connectionString);
        }
    }
}
