using Domain.Contracts;
using Shared.Services;

namespace BLL.LogicLayers
{
    internal static class IntegrityFacade
    {
        public static void RecalculateEntityDVH<T>(T entity) where T : IIntegrityCheckable
        {
            entity.UpdateDVH(IntegrityService.GetIntegrityHash(entity.GetDvhSerialization()));
        }

    }
}
