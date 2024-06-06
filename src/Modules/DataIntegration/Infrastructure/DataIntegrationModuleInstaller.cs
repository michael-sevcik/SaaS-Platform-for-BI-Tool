using BIManagement.Common.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BIManagement.Common.Infrastructure.Extensions;


namespace BIManagement.Modules.DataIntegration.Infrastructure;

/// <summary>
/// Data integration module installer.
/// 
/// Installs the services of the data integration module.
/// </summary>
public class DataIntegrationModuleInstaller : IModuleInstaller
{
    /// <inheritdoc/>
    public static void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.InstallServicesFromAssemblies(configuration,
            AssemblyReference.Assembly
            //  TODO: ADD all ADDITIONAL ASSEMBLIES
            );
    }
}
