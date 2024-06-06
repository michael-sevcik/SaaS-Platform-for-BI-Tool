using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Infrastructure.Extensions;
using BIManagement.Common.Persistence.Extensions;
using BIManagement.Common.Persistence.Options;
using BIManagement.Modules.Deployment.Persistence;
using BIManagement.Modules.Deployment.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BIManagement.Modules.Deployment.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents a service installer of the deployment module's persistence layer.
/// </summary>
internal class PersistenceServiceInstaller : IServiceInstaller
{
    /// <inheritdoc/>
    public static void Install(IServiceCollection services, IConfiguration configuration)
        => services
            .AddDbContext<DeploymentDbContext>((serviceProvider, options) =>
            {
                ConnectionStringOptions connectionString = serviceProvider.GetService<IOptions<ConnectionStringOptions>>()?.Value
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

                options.UseSqlServer(
                    connectionString,
                    options => options.WithMigrationHistoryTableInSchema(Schemas.Deployment));
            })
            .AddServicesWithLifetimeAsMatchingInterfaces(Persistence.AssemblyReference.Assembly);
}
