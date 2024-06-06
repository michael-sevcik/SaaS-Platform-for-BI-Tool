using BIManagement.Common.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BIManagement.Common.Infrastructure.Extensions;

namespace BIManagement.Modules.Deployment.Infrastructure;

/// <summary>
/// Deployment module installer.
/// 
/// Installs the services of the deployment module.
/// </summary>
public class DeploymentModuleInstaller : IModuleInstaller
{
    /// <inheritdoc/>
    public static void Install(IServiceCollection services, IConfiguration configuration)
        => services.InstallServicesFromAssemblies(configuration, AssemblyReference.Assembly);
}
