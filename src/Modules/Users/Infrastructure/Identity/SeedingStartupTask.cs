using BIManagement.Modules.Users.Domain;
using BIManagement.Modules.Users.Infrastructure.Options.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BIManagement.Modules.Users.Infrastructure.Identity;

/// <summary>
/// Represents a startup task for seeding the users data (Roles and default admin) in the development environment.
/// </summary>
/// <param name="environment">The environment.</param>
/// <param name="serviceProvider">The service provider.</param>
internal class SeedingStartupTask(
    IHostEnvironment environment,
    IServiceProvider serviceProvider,
    ILogger<SeedingStartupTask> logger,
    IOptions<DefaultAdminOptions> adminConfiguration) : BackgroundService
{
    private readonly DefaultAdminOptions adminOptions = adminConfiguration.Value;

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!environment.IsDevelopment())
        {
            return;
        }

        logger.LogInformation("Seeding users data...");

        await Task.Delay(10000);
        
        using IServiceScope scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        await EnsureRoleIsCreated(roleManager, Roles.Customer);
        await EnsureRoleIsCreated(roleManager, Roles.Admin);
        logger.LogInformation("Roles created.");
        
        var userManager = scope.ServiceProvider.GetRequiredService<IUserManager>();
        await CreateAdmin(userManager, adminOptions.Email, adminOptions.Name);
        logger.LogInformation("Admin Created.");
    }

    private static async Task CreateAdmin(IUserManager userManager, string email, string name)
    {
        if (await userManager.GetUserByEmail(email) is not null)
        {
            return;
        }

        var creationResult = await userManager.CreateAdminAsync(email, name);
        if (creationResult.IsFailure)
        {
            throw new InvalidOperationException($"Creation of the admin with email: \"{email}\" failed. " + creationResult.Error.Message);
        }
    }

    private static async Task EnsureRoleIsCreated(RoleManager<IdentityRole> roleManager, Role roleToCreate)
    {
        if (await roleManager.RoleExistsAsync(roleToCreate))
        {
            return;
        }

        var identityResult = await roleManager.CreateAsync(new(roleToCreate));
        if (!identityResult.Succeeded)
        {
            throw new InvalidOperationException($"Creation of the role: \"{roleToCreate}\" failed. " + identityResult.ToString());
        }
    }

}
