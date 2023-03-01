namespace RepositorySupport;

/// <summary>
/// Grouping of operations over data in the aggregate
/// </summary>
/// <typeparam name="T">Type of aggregate root processed in the repository</typeparam>
public interface IRepository<T> where T : IAggregateRoot
{
    /// <summary>
    /// Current business transaction
    /// </summary>
    IUnitOfWork UnitOfWork { get; }
}
