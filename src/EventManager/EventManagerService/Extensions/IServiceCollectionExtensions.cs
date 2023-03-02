using EventManager.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventManagerService.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDbContexts(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SocialEventsContext>(options =>
        {
            options.UseSqlServer(configuration["ConnectionString"], sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
            });
        });

        return services;
    }
}
