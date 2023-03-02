using RepositorySupport;

namespace EventManager.Domain.Events;

/// <summary>
/// Social occasion happening on some date on which participants can be included
/// </summary>
public class SocialEvent : Entity, IAggregateRoot
{
    /// <summary>
    /// Name of the event
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Date of the event
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Create new social event under the name <paramref name="name"/> happening on <paramref name="date"/>
    /// </summary>
    /// <param name="name">name of the event</param>
    /// <param name="date">date of the event</param>
    public SocialEvent(string name, DateTime date)
    {
        Name = name;
        Date = date;
    }
}
