using EventManager.Domain.Events;
using Microsoft.EntityFrameworkCore;
using RepositorySupport;

namespace EventManager.Persistence.Repositories;

public class SocialEventRepository : ISocialEventRepository
{
    private readonly SocialEventsContext _dbContext;

    public IUnitOfWork UnitOfWork => _dbContext;

    public SocialEventRepository(SocialEventsContext dbContext)
    {
        _dbContext = dbContext;
    }

    public SocialEvent Add(SocialEvent socialEvent)
    {
        return _dbContext.SocialEvents.Add(socialEvent).Entity;
    }

    public void Delete(SocialEvent socialEvent)
    {
        _dbContext.SocialEvents.Remove(socialEvent);
    }

    public async Task<SocialEvent?> GetAsync(int eventId)
    {
        var theEvent = await _dbContext.SocialEvents
            .FindAsync(eventId)
            ?? _dbContext.SocialEvents.Local.FirstOrDefault(e => e.Id == eventId);

        return theEvent;
    }

    public void Update(SocialEvent socialEvent)
    {
        _dbContext.SocialEvents.Update(socialEvent);
    }

    public async Task<IEnumerable<SocialEvent>> GetEventsAsync()
    {
        //TODO: temporary method for CRUD list
        return await _dbContext.SocialEvents.ToListAsync();
    }
}
