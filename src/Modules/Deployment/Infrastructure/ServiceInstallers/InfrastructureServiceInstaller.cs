using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Infrastructure.Extensions;
using BIManagement.Modules.Deployment.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIManagement.Modules.Deployment.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents a service installer of the user's application layer.
/// </summary>
internal class InfrastructureServiceInstaller : IServiceInstaller
{
    /// <inheritdoc/>
    public static void Install(IServiceCollection services, IConfiguration configuration)
        => services
            .ConfigureOptions<SmtpSettingsOptionsSetup>()
            .ConfigureOptions<KubernetesPublicUrlOptionSetup>()
            .AddServicesWithLifetimeAsMatchingInterfaces(AssemblyReference.Assembly);
}
