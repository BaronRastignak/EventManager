using RepositorySupport;

namespace EventManager.Domain.Events;

public interface ISocialEventRepository : IRepository<SocialEvent>
{
    Task<IEnumerable<SocialEvent>> GetEventsAsync();

    Task<SocialEvent?> GetAsync(int eventId);

    SocialEvent Add(SocialEvent socialEvent);

    void Update(SocialEvent socialEvent);

    void Delete(SocialEvent socialEvent);
}
