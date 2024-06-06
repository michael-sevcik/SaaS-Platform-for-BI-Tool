using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.Users.Domain;

namespace BIManagement.Modules.Users.Api.ExternalCommunication;

/// <summary>
/// Implementation of the <see cref="IIntegrationNotifier"/> interface.
/// </summary>
internal class IntegrationNotifier(
    DataIntegration.Api.IIntegrationService DIIntegrationService,
    Deployment.Api.IIntegrationService DeploymentIntegrationService) : IIntegrationNotifier, ISigleton
{
    /// <inheritdoc/>
    public async Task SentCostumerDeletionNotification(string userId)
    {
        await DIIntegrationService.HandleCostumerDeletionAsync(userId);
        await DeploymentIntegrationService.HandleCostumerDeletionAsync(userId);
    }
}
