using Domain.BaseContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICrudl<T> where T : EntityBase
    {
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(Guid entityId);
        Task<T> GetById(Guid id);
        Task<IEnumerable<T>> GetAllAsync(); 
        void SetTransaction(object transaction);
    }
}