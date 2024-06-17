using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.Deployment.Pages;

/// <summary>
/// Represents an encapsulation of routes for pages of the Deployment module.
/// </summary>
internal static class Routes
{
    /// <summary>
    /// Represents a base route for the Deployment module.
    /// </summary>
    public const string DeploymentPrefix = "/Deployment";

    #region CostumersPages

    /// <summary>
    /// Represents a route to the costumer's deployment info.
    /// </summary>
    public const string Deployment = $"{DeploymentPrefix}/Configure-db-connection";

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
    /// Gets a route to the page displaying details about a deployment for a given costumer with <paramref name="costumerId"/>.
    /// </summary>
    public static string GetDeploymentInfoRoute(string costumerId)
        => $"{Deployments}/{costumerId}";

    #endregion
}
