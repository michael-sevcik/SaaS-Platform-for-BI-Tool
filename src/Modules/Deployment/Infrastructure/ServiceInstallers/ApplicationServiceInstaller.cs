using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BIManagement.Modules.Deployment.Application;
using k8s;

namespace BIManagement.Modules.Deployment.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents a service installer of the user's application layer.
/// </summary>
internal class ApplicationServiceInstaller : IServiceInstaller
{
    /// <inheritdoc/>
    public static void Install(IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Get external configuration for external cluster
        //var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
        var config = KubernetesClientConfiguration.BuildDefaultConfig();
        var kubernetesClient = new Kubernetes(config);
        services.AddSingleton<IKubernetes>(kubernetesClient);

        services.AddServicesWithLifetimeAsMatchingInterfaces(Application.AssemblyReference.Assembly);
    }
}
