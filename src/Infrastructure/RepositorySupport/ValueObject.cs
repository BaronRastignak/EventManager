namespace RepositorySupport;

/// <summary>
/// Base class for parts of persistent entities that are convenient to be grouped into a separate object
/// </summary>
public abstract class ValueObject
{
    public static bool operator==(ValueObject left, ValueObject right)
    {
        if (left is null)
            return right is null;

        return left.Equals(right);
    }

    public static bool operator!=(ValueObject left, ValueObject right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Get properties that are used to determine equality
    /// </summary>
    /// <returns>Collection of property values</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not ValueObject other || GetType() != other.GetType())
            return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(comp => comp?.GetHashCode() ?? 0)
            .Aggregate((accumulator, next) => accumulator ^ next);
    }

    /// <summary>
    /// Get copy of the object with the same values for properties
    /// </summary>
    /// <returns>Shallow copy of the object</returns>
    public ValueObject GetCopy()
    {
        return (ValueObject) MemberwiseClone();
    }
}
