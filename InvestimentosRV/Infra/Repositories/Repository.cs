using Application.Interfaces.Repositories;
using Domain.Abstractions;
using Infra.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public abstract class Repository<TDomain, TModel>(AppDbContext context) : IRepository<TDomain>
    where TDomain : Entity
    where TModel : Model
{
    protected readonly AppDbContext _context = context;

    protected abstract TModel ToModel(TDomain entity);
    protected abstract TDomain ToDomain(TModel model);

    public async Task CreateAsync(TDomain entity, CancellationToken cancellationToken) =>
        await _context.Set<TModel>().AddAsync(ToModel(entity), cancellationToken);

    public void Update(TDomain entity) =>
        _context.Set<TModel>().Update(ToModel(entity));

    public void Delete(TDomain entity) =>
        _context.Set<TModel>().Remove(ToModel(entity));

    public async Task<TDomain?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var model = await _context.Set<TModel>()
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        return model is null ? null : ToDomain(model);
    }

    public async Task<IEnumerable<TDomain>> GetAllAsync(CancellationToken cancellationToken)
    {
        var models = await _context.Set<TModel>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return models.Select(ToDomain);
    }
}
