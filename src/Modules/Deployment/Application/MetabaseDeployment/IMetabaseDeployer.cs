using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Deployment.Domain.Configuration;

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
    /// <param name="defaultAdminSettings">The configuration for the default admin of the new Metabase instance.</param>
    /// <returns>A task that represents the asynchronous operation and returns a Result indicating the success or failure of the operation.</returns>
    Task<Result> DeployMetabaseAsync(string customerId, DefaultAdminSettings defaultAdminSettings);
}
