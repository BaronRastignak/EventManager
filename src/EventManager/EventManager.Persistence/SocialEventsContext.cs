using EventManager.Domain.Events;
using EventManager.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using RepositorySupport;

namespace EventManager.Persistence;

public class SocialEventsContext : DbContext, IUnitOfWork
{
    public const string DefaultSchema = "event_manager";

    public DbSet<SocialEvent> SocialEvents { get; set; }

    public SocialEventsContext(DbContextOptions<SocialEventsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        new SocialEventTypeConfiguration().Configure(modelBuilder.Entity<SocialEvent>());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        //TODO: raise necessary domain events
        
        await SaveChangesAsync(cancellationToken);
        return true;
    }
}
