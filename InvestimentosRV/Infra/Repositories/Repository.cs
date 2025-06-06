using Core.Domain;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : Entity
{
    protected readonly AppDbContext _context = context;

    public async Task CreateAsync(T entity, CancellationToken cancellationToken) =>
        await _context.Set<T>().AddAsync(entity, cancellationToken);

    public void Delete(T entity)
    {
        _context.Remove(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<T?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Set<T>()
            .FindAsync([id], cancellationToken);
    }

    public void Update(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Update(entity);
    }
}
