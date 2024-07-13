using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.Deployment.Api;
using BIManagement.Modules.Deployment.Application.MetabaseDeployment;
using BIManagement.Modules.Deployment.Domain;
using Microsoft.Extensions.Logging;

namespace BIManagement.Modules.Deployment.Application;

/// <summary>
/// Represents an integration service for handling customer deletions.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="deploymentRepository">The deployment repository.</param>
/// <param name="metabaseDeployer">The metabase deployer.</param>
public class IntegrationService(ILogger<IntegrationService> logger, IMetabaseDeploymentRepository deploymentRepository, IMetabaseDeployer metabaseDeployer) : IIntegrationService, IScoped
{
    /// <inheritdoc/>
    public async Task HandleCustomerDeletionAsync(string userId)
    {
        var deployment = await deploymentRepository.GetAsync(userId);
        if (deployment is null)
        {
            return;
        }

        var result = await metabaseDeployer.DeleteDeploymentAsync(userId);
        if (result.IsFailure)
        {
            logger.LogError("Failed to delete deployment for user {UserId}", userId);
        }
        else
        {
            logger.LogInformation("Deleted deployment for user {UserId}", userId);
        }
    }
}
