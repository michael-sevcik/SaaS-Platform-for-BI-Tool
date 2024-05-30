using BIManagement.Common.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BIManagement.Common.Infrastructure.Extensions;

namespace BIManagement.Modules.DataIntegration.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents a service installer of the data integration's application layer.
/// </summary>
internal class ApplicationServiceInstaller : IServiceInstaller
{
    /// <inheritdoc/>
    public static void Install(IServiceCollection services, IConfiguration configuration)
        => services.AddServicesWithLifetimeAsMatchingInterfaces(Application.AssemblyReference.Assembly);
}
