using CadastralCase.Domain.Entities;

namespace CadastralCase.Domain.Interfaces;

public interface IRepository<T> where T : EntityBase
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetActiveAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task ActivateAsync(Guid id);
    Task DeactivateAsync(Guid id);
}
