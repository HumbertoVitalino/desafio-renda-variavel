using Core.Domain;
using Core.Interfaces;

namespace Infra.Repositories;

public class QuoteRepository(AppDbContext context) : Repository<Quote>(context), IQuoteRepository
{
}
