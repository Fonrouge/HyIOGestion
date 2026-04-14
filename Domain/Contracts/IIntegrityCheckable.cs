using Domain.Entities;

namespace Domain.Contracts
{
    public interface IIntegrityCheckable
    {
        DvhVo DVH { get; }
        string GetDvhSerialization();
        void UpdateDVH(string dvh);
    }
}
