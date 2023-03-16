using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using IdentityService.Data;
using IdentityService.Models;
using IdentityService.Models.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace IdentityService;

public class SeedData
{
    private const string RegisteredUserRole = "RegisteredUser";
    private const string AdministratorRole = "Administrator";

    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            Task.Run(() => SeedDefaultRoles(scope.ServiceProvider)).Wait();
            Task.Run(() => SeedInitialUsers(scope.ServiceProvider, app.Configuration)).Wait();
            Task.Run(() => SeedIdentityResources(scope.ServiceProvider)).Wait();
            Task.Run(() => SeedApiScopes(scope.ServiceProvider, app.Configuration)).Wait();
            Task.Run(() => SeedClients(scope.ServiceProvider, app.Configuration)).Wait();
        }
    }

    private static async Task SeedDefaultRoles(IServiceProvider serviceProvider)
    {
        var defaultRoles = new[] { RegisteredUserRole, AdministratorRole };

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in defaultRoles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (!result.Succeeded)
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));

                Log.Debug($"Role {role} created");
            }
            else
            {
                Log.Debug($"Role {role} already exists");
            }
        }    
    }

    private static async Task SeedInitialUsers(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        foreach (var user in configuration.GetSection("InitialData:Users").Get<UserSettings[]>())
        {
            if (await userManager.FindByNameAsync(user.UserName) is null)
            {
                var appUser = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed
                };

                var result = await userManager.CreateAsync(appUser, user.Password);
                if (!result.Succeeded)
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));

                result = await userManager.AddToRolesAsync(appUser,
                    new[] { RegisteredUserRole, AdministratorRole });
                if (!result.Succeeded)
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));

                Log.Debug($"{appUser.UserName} created");
            }
            else
            {
                Log.Debug($"{user.UserName} already exists");
            }
        }
    }

    private static async Task SeedIdentityResources(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetService<ConfigurationDbContext>();
        
        var defaultResources = new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        var existingResources = context.IdentityResources.Select(res => res.Name).ToHashSet();
        var resourcesToAdd = defaultResources
            .Select(res => res.ToEntity())
            .Where(res => !existingResources.Contains(res.Name))
            .ToList();

        if (resourcesToAdd.Count == 0)
        {
            Log.Debug("Identity Resources already created, skipping step");
            return;
        }

        await context.AddRangeAsync(resourcesToAdd);
        await context.SaveChangesAsync();
        resourcesToAdd.ForEach(res => Log.Debug($"Identity Resource {res.Name} created"));
    }

    private static async Task SeedApiScopes(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var context = serviceProvider.GetService<ConfigurationDbContext>();

        var apiScopes = configuration.GetSection("InitialData:ApiScopes").Get<ApiScope[]>();

        var existingScopes = context.ApiScopes.Select(scope => scope.Name).ToHashSet();
        var scopesToAdd = apiScopes
            .Select(scope => scope.ToEntity())
            .Where(scope => !existingScopes.Contains(scope.Name))
            .ToList();

        if (scopesToAdd.Count == 0)
        {
            Log.Debug("API Scopes already created, skipping step");
            return;
        }

        await context.AddRangeAsync(scopesToAdd);
        await context.SaveChangesAsync();
        scopesToAdd.ForEach(scope => Log.Debug($"API Scope {scope.Name} created"));
    }

    private static async Task SeedClients(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var context = serviceProvider.GetService<ConfigurationDbContext>();

        var clients = configuration.GetSection("InitialData:Clients").Get<Client[]>();

        var existingClients = context.Clients.Select(client => client.ClientId).ToHashSet();
        var clientsToAdd = clients
            .Select(client => client.ToEntity())
            .Where(client => !existingClients.Contains(client.ClientId))
            .ToList();

        if (clientsToAdd.Count == 0)
        {
            Log.Debug("Clients already created, skipping step");
            return;
        }

        await context.AddRangeAsync(clientsToAdd);
        await context.SaveChangesAsync();
        clientsToAdd.ForEach(client => Log.Debug($"Client {client.ClientId} created"));
    }
}
