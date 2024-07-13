using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.Deployment.Domain;

namespace BIManagement.Modules.Deployment.Api;

/// <summary>
/// Implementation of the <see cref="IIntegrationNotifier"/> interface.
/// </summary>
internal class IntegrationNotifier(Notifications.Api.IIntegrationService notificationIntegrationService) : IIntegrationNotifier, IScoped
{
    /// <inheritdoc/>
    public async Task SentMetabaseDeployedNotification(string customerId, string metabaseUrl)
    {
        await notificationIntegrationService.HandleMetabaseDeployment(customerId, metabaseUrl);
    }
}
