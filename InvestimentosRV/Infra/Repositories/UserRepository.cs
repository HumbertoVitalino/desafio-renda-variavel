using Core.Domain;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class UserRepository(AppDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Equals(email), cancellationToken);
    }
}
