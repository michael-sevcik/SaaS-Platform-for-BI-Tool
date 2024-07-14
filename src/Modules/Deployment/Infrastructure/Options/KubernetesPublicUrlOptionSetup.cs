using BIManagement.Common.Shared.Exceptions;
using BIManagement.Modules.Deployment.Application.MetabaseDeployment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BIManagement.Modules.Deployment.Infrastructure.Options;

/// <summary>
/// Represents the <see cref="KubernetesPublicUrlOption"/> options setup.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="KubernetesPublicUrlOptionSetup"/> class.
/// </remarks>
/// <param name="configuration">The configuration.</param>
class KubernetesPublicUrlOptionSetup(IConfiguration configuration) : IConfigureOptions<KubernetesPublicUrlOption>
{
    private const string DeploymentGroup = "Modules:Deployment";
    private const string KubernetesPublicUrlKey = "KubernetesPublicUrl";
    private const string KubernetesInternalUrlKey = "KubernetesInternalUrl";

    public void Configure(KubernetesPublicUrlOption options)
    {
        var kubernetesPublicUrl = configuration.GetSection(DeploymentGroup)?.GetValue<string>(KubernetesPublicUrlKey)
            ?? throw new InvalidConfigurationException($"Section {DeploymentGroup}:{KubernetesPublicUrlKey} is missing in the configuration.");

        var kubernetesInternalUrl = configuration.GetSection(DeploymentGroup)?.GetValue<string>(KubernetesInternalUrlKey)
            ?? throw new InvalidConfigurationException($"Section {DeploymentGroup}:{KubernetesInternalUrlKey} is missing in the configuration.");

        options.PublicUrl = kubernetesPublicUrl;
        options.InternalUrl = kubernetesInternalUrl;
    }
}
