using Hackton.Domain.Base.Entity;

namespace Hackton.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        Task<T> GetByIdAsync(Guid id);

    }
}
