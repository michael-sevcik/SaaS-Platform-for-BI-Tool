using BIManagement.Modules.Users.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BIManagement.ManagementApp.StartupTasks;

/// <summary>
/// Represents a startup task for migrating the databases in the development environment.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MigrateDatabasesTask"/> class.
/// </remarks>
/// <param name="environment">The environment.</param>
/// <param name="serviceProvider">The service provider.</param>
internal sealed class MigrateDatabasesTask(IHostEnvironment environment, IServiceProvider serviceProvider, ILogger<MigrateDatabasesTask> looger) : BackgroundService
{
    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!environment.IsDevelopment())
        {
            return;
        }
        looger.LogInformation("Migrating databases...");
        using IServiceScope scope = serviceProvider.CreateScope();

        await MigrateDatabaseAsync<UsersContext>(scope, stoppingToken);
        looger.LogInformation("Migration done.");
    }

    private static async Task MigrateDatabaseAsync<TDbContext>(IServiceScope scope, CancellationToken cancellationToken)
        where TDbContext : DbContext
    {
        TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }
}
