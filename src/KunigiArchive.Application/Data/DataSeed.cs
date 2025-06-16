using KunigiArchive.Application.Services;
using KunigiArchive.Contracts.User;
using KunigiArchive.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KunigiArchive.Application.Data;

public class DataSeed
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DataSeed>>();
        var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
        
        // Seed app Roles
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
        
        // Seed main admins
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
    }
}