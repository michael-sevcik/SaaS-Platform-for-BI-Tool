namespace BIManagement.Modules.Deployment.Domain;

/// <summary>
/// Represents a deployment of a metabase for a given customer.
/// </summary>
public class MetabaseDeployment
{
    /// <summary>
    /// Gets or sets the id of the deployment.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the customer id for which this instance was deployed.
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL path for the instance.
    /// </summary>
    public string UrlPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the image used for the deployment.
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// The name of the instance.
    /// </summary>
    public string InstanceName { get; set; } = string.Empty;
}
