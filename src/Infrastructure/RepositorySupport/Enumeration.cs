using System.Reflection;

namespace RepositorySupport;

/// <summary>
/// Rich enumeration base class
/// </summary>
public abstract class Enumeration : IComparable
{
    /// <summary>
    /// Display name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Value of the enum
    /// </summary>
    public int Id { get; }

    protected Enumeration(string name, int id)
    {
        Name = name;
        Id = id;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    /// Get all declared values for the given enumeration type
    /// </summary>
    /// <typeparam name="T">Enumeration type</typeparam>
    /// <returns>Collection of declared enumeration values</returns>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        return typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration other)
            return false;

        return GetType() == other.GetType() && Id == other.Id;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Get difference between two enumeration values
    /// </summary>
    /// <typeparam name="T">Enumeration type</typeparam>
    /// <param name="left">left value</param>
    /// <param name="right">right value</param>
    /// <returns>Absolute difference between enum values</returns>
    public static int AbsoluteDifference<T>(T left, T right) where T : Enumeration
    {
        return Math.Abs(left.Id - right.Id);
    }

    /// <summary>
    /// Get enumeration value from its int representation
    /// </summary>
    /// <typeparam name="T">Enumeration type</typeparam>
    /// <param name="value">enumeration value</param>
    /// <returns>Enumeration value instance</returns>
    public static T FromValue<T>(int value) where T : Enumeration
    {
        return Parse<T, int>(value, "value", item => item.Id == value);
    }

    /// <summary>
    /// Get enumeration value from its display name
    /// </summary>
    /// <typeparam name="T">Enumeration type</typeparam>
    /// <param name="displayName">enumeration display name</param>
    /// <returns>Enumeration value instance</returns>
    public static T FromDisplayName<T>(string displayName) where T : Enumeration
    {
        return Parse<T, string>(displayName, "display name", item => item.Name == displayName);
    }

    private static TEnum Parse<TEnum, TKey>(TKey value, string description, Func<TEnum, bool> predicate)
        where TEnum : Enumeration
    {
        var matchingItem = GetAll<TEnum>().FirstOrDefault(predicate);
        return matchingItem is not null
            ? matchingItem
            : throw new InvalidOperationException($"'{value} is not a valid {description} in {typeof(TEnum)}");
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        return Id.CompareTo((obj as Enumeration)?.Id);
    }
}
