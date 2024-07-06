using BIManagement.Common.Shared.Results;

namespace BIManagement.Modules.Deployment.Application.MetabaseDeployment;

/// <summary>
/// Represents an interface for deploying and managing Metabase.
/// </summary>
internal interface IMetabaseDeployer
{
    /// <summary>
    /// Deletes the deployment for the specified customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>A task that represents the asynchronous operation and returns a Result indicating the success or failure of the operation.</returns>
    Task<Result> DeleteDeploymentAsync(string customerId);

    /// <summary>
    /// Deploys Metabase for the specified customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>A task that represents the asynchronous operation and returns a Result indicating the success or failure of the operation.</returns>
    Task<Result> DeployMetabaseAsync(string customerId);
}
