using EventManager.Persistence;
using EventManagerService.Policy;
using Microsoft.EntityFrameworkCore;

namespace EventManagerService.Extensions;

internal static class IServiceCollectionExtensions
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

    public static IServiceCollection AddJWTBearerAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.Authority = configuration.GetValue<string>("IdentityUrl");
                options.Audience = "events";
                options.TokenValidationParameters.ValidateAudience = false;
            });

        return services;
    }

    public static IServiceCollection AddApiScopeAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            var policy = AuthorizationPolicy.ApiScope();
            options.AddPolicy(policy.Name, policyOptions =>
            {
                policyOptions.RequireAuthenticatedUser();
                policyOptions.RequireClaim("scope", policy.Scope);
            });
        });

        return services;
    }
}
