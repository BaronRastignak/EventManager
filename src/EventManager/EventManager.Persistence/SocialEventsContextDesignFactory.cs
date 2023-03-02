using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EventManager.Persistence;

public class SocialEventsContextDesignFactory : IDesignTimeDbContextFactory<SocialEventsContext>
{
    public SocialEventsContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SocialEventsContext>()
            .UseSqlServer("Server=.;Initial Catalog=EventManager.Main;Integrated Security=true");
        return new SocialEventsContext(optionsBuilder.Options);
    }
}
