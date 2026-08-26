using Application.Interfaces.Repositories;
using Domain.Entities;
using Infra.Repositories.Mappers;
using Infra.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class UserRepository(AppDbContext context) : Repository<User, UserModel>(context), IUserRepository
{
    protected override UserModel ToModel(User entity) => entity.MapToModel();
    protected override User ToDomain(UserModel model) => model.MapToDomain();

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var model = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Equals(email), cancellationToken);

        return model?.MapToDomain();
    }
}
