using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using k8s;
using System.Text.Json;

namespace BIManagement.Modules.Deployment.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents a service installer of the user's application layer.
/// </summary>
internal class ApplicationServiceInstaller : IServiceInstaller
{
    /// <inheritdoc/>
    public static void Install(IServiceCollection services, IConfiguration configuration)
    {
        var config = KubernetesClientConfiguration.BuildDefaultConfig();
        var kubernetesClient = new Kubernetes(config);
        Console.WriteLine(config.ClientCertificateData);

        services.AddSingleton<IKubernetes>(kubernetesClient);

        services.AddServicesWithLifetimeAsMatchingInterfaces(Application.AssemblyReference.Assembly);
    }
}
