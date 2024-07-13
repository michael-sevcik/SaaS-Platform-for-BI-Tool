using BIManagement.Common.Shared.Results;

namespace BIManagement.Modules.Deployment.Domain;

/// <summary>
/// Represents a repository for managing Metabase deployments.
/// </summary>
public interface IMetabaseDeploymentRepository
{
    /// <summary>
    /// Retrieves a Metabase deployment asynchronously based on the customer ID.
    /// </summary>
    /// <param name="customerId">The customer ID.</param>
    /// <returns>The retrieved Metabase deployment, or null if not found.</returns>
    Task<MetabaseDeployment?> GetAsync(string customerId);

    /// <summary>
    /// Retrieves all Metabase deployments asynchronously.
    /// </summary>
    /// <returns>The retrieved Metabase deployments.</returns>
    Task<IReadOnlyList<MetabaseDeployment>> GetAsync();

    /// <summary>
    /// Asynchronously adds or updates the given instance of <see cref="MetabaseDeployment"/>.
    /// </summary>
    /// <param name="deployment">The Metabase deployment to create.</param>
    /// <returns>The result of <paramref name="deployment"/> saving into the repository.</returns>
    Task<Result> SaveDeploymentAsync(MetabaseDeployment deployment);

    /// <summary>
    /// Deletes a Metabase deployment asynchronously based on the customer ID.
    /// </summary>
    /// <param name="customerId">The customer ID.</param>
    Task<Result> DeleteDeploymentAsync(string customerId);
}
