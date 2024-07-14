namespace BIManagement.Modules.Deployment.Application.MetabaseDeployment;

/// <summary>
/// Represents the option for the Kubernetes public URL.
/// </summary>
public class KubernetesPublicUrlOption
{
    /// <summary>
    /// Gets or sets the URL for the Kubernetes public URL option.
    /// </summary>
    public string PublicUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL for the Kubernetes internal URL option. Used for development.
    /// </summary>
    public string InternalUrl { get; set; } = string.Empty;
}
