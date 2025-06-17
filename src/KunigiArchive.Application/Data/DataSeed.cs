using KunigiArchive.Application.Services;
using KunigiArchive.Application.Utilities;
using KunigiArchive.Contracts.User;
using KunigiArchive.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KunigiArchive.Application.Data;

public class DataSeed
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var roleManager = scopedServices.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = scopedServices.GetRequiredService<IConfiguration>();
        var logger = scopedServices.GetRequiredService<ILogger<DataSeed>>();
        var accountService = scopedServices.GetRequiredService<IAccountService>();
        var context = scopedServices.GetRequiredService<DataContext>();
        
        // seed app roles
        string[] roles = ["Admin", "Manager"];
        
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = role,
                    NormalizedName = role.ToUpper()
                });
            }
        }
        
        // seed main admins
        var adminEmailsString = configuration["INITIAL_ADMIN_USERS"];
        if (string.IsNullOrWhiteSpace(adminEmailsString))
        {
            logger.LogWarning("INITIAL_ADMIN_USERS not found in configuration/.env file. No admin users will be seeded.");
            return;
        }
        
        var adminEmails = adminEmailsString.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var email in adminEmails)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                logger.LogInformation("Attempting to seed new admin user: {Email}", email);
                
                var request = new UserCreateRequest { Email = email, Role =  "Admin" };
                var result = await accountService.CreateUserAsync(request, new ModelStateDictionary());

                if (result.IsSuccess)
                {
                    var newUser = await userManager.FindByEmailAsync(email);
                    if (newUser != null)
                    {
                        await userManager.AddToRoleAsync(newUser, "Admin");
                        logger.LogInformation("Successfully created and assigned Admin role to {Email}", email);
                    }
                }
                else
                {
                    logger.LogError("Failed to seed admin user {Email}.", email);
                }
            }
        }
        
        // seed game types
        if (await context.GameTypes.AnyAsync())
        {
            return;
        }

        var gameTypes = new List<GameType>
        {
            new() { Label = "Χωρός", Slug = SlugGenerator.GenerateSlug("Χωρός") },
            new() { Label = "Σάββατο", Slug = SlugGenerator.GenerateSlug("Σάββατο") },
            new() { Label = "Κυριακή", Slug = SlugGenerator.GenerateSlug("Κυριακή") },
            new() { Label = "Διαδικτυακό", Slug = SlugGenerator.GenerateSlug("Διαδικτυακό") },
            new() { Label = "Παιδικό", Slug = SlugGenerator.GenerateSlug("Παιδικό") },
            new() { Label = "Εφηβικό", Slug = SlugGenerator.GenerateSlug("Εφηβικό") }
        };

        foreach (var gameType in gameTypes)
        {
            context.GameTypes.Add(gameType);
        }

        await context.SaveChangesAsync();
    }
}