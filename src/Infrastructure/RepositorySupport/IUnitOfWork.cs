namespace RepositorySupport;

/// <summary>
/// Marks single business transaction
/// </summary>
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
