using MediatR;

namespace RepositorySupport;

/// <summary>
/// Base class for persistent entities
/// </summary>
public abstract class Entity
{
    int? _requestedHashCode;

    private List<INotification>? _domainEvents;

    /// <summary>
    /// Unique identifier of the entity
    /// </summary>
    public virtual int Id { get; protected set; }

    /// <summary>
    /// Entity is not yet persisted in storage
    /// </summary>
    public bool IsTransient => Id == default;

    /// <summary>
    /// Domain events to be raised on committing changes
    /// </summary>
    public IReadOnlyCollection<INotification>? DomainEvents => _domainEvents?.AsReadOnly();

    /// <summary>
    /// Add domain event for deferred raising
    /// </summary>
    /// <param name="domainEvent">event to raise on committing changes</param>
    public void AddDomainEvent(INotification domainEvent)
    {
        _domainEvents ??= new List<INotification>();
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Remove domain event to prevent its raise
    /// </summary>
    /// <param name="domainEvent">event to remove from collection</param>
    public void RemoveDomainEvent(INotification domainEvent)
    {
        _domainEvents?.Remove(domainEvent);
    }

    /// <summary>
    /// Clear all pending domain events
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not Entity entity)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        if (IsTransient || entity.IsTransient)
            return false;

        return Id == entity.Id;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        if (!IsTransient)
        {
            _requestedHashCode ??= Id.GetHashCode() ^ 31;
            return _requestedHashCode.Value;
        }

        return base.GetHashCode();
    }

    public static bool operator==(Entity left, Entity right)
    {
        if (left is null)
            return right is null;

        return left.Equals(right);
    }

    public static bool operator!=(Entity left, Entity right)
    {
        return !(left == right);
    }
}
