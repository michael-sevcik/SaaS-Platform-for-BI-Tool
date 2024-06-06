using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Infrastructure.Extensions;
using BIManagement.Common.Persistence.Extensions;
using BIManagement.Common.Persistence.Options;
using BIManagement.Modules.DataIntegration.Persistence;
using BIManagement.Modules.DataIntegration.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BIManagement.Modules.DataIntegration.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents a service installer of the data integration's persistence layer.
/// </summary>
internal class PersistenceServiceInstaller : IServiceInstaller
{
    /// <inheritdoc/>
    public static void Install(IServiceCollection services, IConfiguration configuration)
        => services
            .AddDbContext<DataIntegrationDbContext>((serviceProvider, options) => {
                ConnectionStringOptions connectionString = serviceProvider.GetService<IOptions<ConnectionStringOptions>>()?.Value
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

                options.UseSqlServer(
                    connectionString,
                    options => options.WithMigrationHistoryTableInSchema(Schemas.DataIntegration));
            })
            .AddServicesWithLifetimeAsMatchingInterfaces(Persistence.AssemblyReference.Assembly);
}
