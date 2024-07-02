namespace BIManagement.Modules.Deployment.Api;

public class PublicModuleRoutes
{
    /// <summary>
    /// Represents a base route for the Deployment module.
    /// </summary>
    public const string DeploymentPrefix = "/Deployment";

    #region CustomersPages

    /// <summary>
    /// Represents a route to the customer's deployment info.
    /// </summary>
    public const string CustomerMetabaseDeployment = $"{DeploymentPrefix}/DeployMetabase";
    #endregion


}
