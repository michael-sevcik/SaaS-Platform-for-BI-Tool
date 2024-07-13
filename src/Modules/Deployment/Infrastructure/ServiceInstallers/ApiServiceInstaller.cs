using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIManagement.Modules.Deployment.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents a service installer of the deployment module's API layer.
/// </summary>
internal class ApiServiceInstaller : IServiceInstaller
{
    /// <inheritdoc/>
    public static void Install(IServiceCollection services, IConfiguration configuration)
        => services.AddServicesWithLifetimeAsMatchingInterfaces(Api.AssemblyReference.Assembly);
}
