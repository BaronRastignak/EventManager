using IdentityService.Data;
using IdentityService.Models;
using IdentityService.Models.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
}
