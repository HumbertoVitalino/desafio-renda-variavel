using Domain.Abstractions;

namespace Application.Interfaces.Repositories;

public interface IRepository<T> where T : Entity
{
    Task CreateAsync(T entity, CancellationToken cancellationToken);
    void Update(T entity);
    void Delete(T entity);
    Task<T?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
}
