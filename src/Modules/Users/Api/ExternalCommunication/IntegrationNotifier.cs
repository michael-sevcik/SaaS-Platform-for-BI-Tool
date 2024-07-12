using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.Users.Domain;

namespace BIManagement.Modules.Users.Api.ExternalCommunication;

/// <summary>
/// Implementation of the <see cref="IIntegrationNotifier"/> interface.
/// </summary>
internal class IntegrationNotifier(
    DataIntegration.Api.IIntegrationService DIIntegrationService,
    Deployment.Api.IIntegrationService DeploymentIntegrationService) : IIntegrationNotifier, ISingleton
{
    /// <inheritdoc/>
    public async Task SentCustomerDeletionNotification(string userId)
    {
        await DIIntegrationService.HandleCustomerDeletionAsync(userId);
        await DeploymentIntegrationService.HandleCustomerDeletionAsync(userId);
    }
}
