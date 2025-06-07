namespace Core.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}
