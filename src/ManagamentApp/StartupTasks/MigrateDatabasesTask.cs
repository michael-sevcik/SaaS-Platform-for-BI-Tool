using Microsoft.EntityFrameworkCore;

namespace ManagementApp.StartupTasks
{
    /// <summary>
    /// Represents a startup task for migrating the databases in the development environment.
    /// </summary>
    internal sealed class MigrateDatabasesTask : BackgroundService
    {
        private readonly IHostEnvironment _environment;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrateDatabasesTask"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public MigrateDatabasesTask(IHostEnvironment environment, IServiceProvider serviceProvider)
        {
            _environment = environment;
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_environment.IsDevelopment())
            {
                return;
            }

            using IServiceScope scope = _serviceProvider.CreateScope();

            // TODO: use for each DbContext
        }

        private static async Task MigrateDatabaseAsync<TDbContext>(IServiceScope scope, CancellationToken cancellationToken)
            where TDbContext : DbContext
        {
            TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

            await dbContext.Database.MigrateAsync(cancellationToken);
        }
    }

}
