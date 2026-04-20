using Domain.Entities;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICrudl<T> where T : EntityBase
    {
        Task CreateAsync(T entity);        
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid entityId);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllDeletedAsync();
        void SetTransaction(object transaction);
    }
}