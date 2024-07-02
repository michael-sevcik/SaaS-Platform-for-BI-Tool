using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Api;

namespace BIManagement.Modules.Deployment.Application.ViewDeployment;

/// <summary>
/// Represents an interface for deploying database views.
/// </summary>
public interface IDatabaseViewDeployer
{
    /// <summary>
    /// Deploys the specified views to the database.
    /// </summary>
    /// <param name="databaseProvider">The database provider.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="views">The views to deploy.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Result> DeployViewsAsync(DatabaseProvider databaseProvider, string connectionString, string[] views);
}
