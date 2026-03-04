using Application.UseCases;
using BLL.LogicLayer;
using Infrastructure.Persistence;
using Infrastructure.Persistence.MicrosoftSQL;
using Shared;
using SharedAbstractions.Services.Hashing;
using System.Linq;


namespace BLL.UseCases
{
    public class UCVerificateDBIntegrity : IUCVerificateDBIntegrity
    {

        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _uow;
        private readonly IIntegrityBLL _integrity;


        public UCVerificateDBIntegrity(IUserRepository userRepo, IUnitOfWork uow, IIntegrityBLL integrity)
        {
            _userRepo = userRepo;
            _uow = uow;
            _integrity = integrity;

        }

        public bool Execute(string user, string pass)
        {
            var allUsers = _uow.UserRepo.GetAll().Where(e => e.Username == user);

            foreach (var u in allUsers)
            {
                var storesDVH = u.DVH;
                var calculatedDVH = u.DVH = IntegrityService.GetIntegrityHash
                (
                    u.Id,
                    u.Username,
                    u.Password,
                    u.Language,
                    u.Employee.Id
                );

                if (storesDVH != calculatedDVH)
                    return false;
            }

           
            if (!_integrity.ValidateGlobalIntegrity("User", ApplicationSettings.SecurityConnection))
                return false;

            if (!_integrity.ValidateGlobalIntegrity("Employee", ApplicationSettings.EntitiesConnection))
                return false;


            return true;

        }
    }
}
