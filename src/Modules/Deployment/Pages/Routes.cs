using BIManagement.Modules.Deployment.Api;

namespace BIManagement.Modules.Deployment.Pages;

/// <summary>
/// Represents an encapsulation of routes for pages of the Deployment module.
/// </summary>
public static class Routes
{
    /// <summary>
    /// Represents a base route for the Deployment module.
    /// </summary>
    public const string DeploymentPrefix = PublicModuleRoutes.DeploymentPrefix;

    #region CustomersPages

    /// <summary>
    /// Represents a route to the customer's deployment info.
    /// </summary>
    public const string MetabaseDeployment = PublicModuleRoutes.CustomerMetabaseDeployment;

    #endregion

    #region AdminsPages

    /// <summary>
    /// Represents a route to the list of MappingProjects
    /// </summary>
    public const string Deployments = $"{DeploymentPrefix}/Deployments";

    /// <summary>
    /// Represents a route template for the all deployments info page.
    /// </summary>
    public const string DeploymetsInfo = $"{Deployments}/{{id}}";

    /// <summary>
    /// Gets a route to the page displaying details about a deployment for a given customer with <paramref name="customerId"/>.
    /// </summary>
    public static string GetDeploymentInfoRoute(string customerId)
        => $"{Deployments}/{customerId}";

    #endregion
}
