using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.Notifications.Api;

namespace BIManagement.Modules.Notifications.Application;

/// <summary>
/// Implementation of the <see cref="IIntegrationService"/> interface.
/// </summary>
class IntegrationService : IIntegrationService, IScoped
{
    /// <inheritdoc/>
    public Task HandleMetabaseDeployment(string userId, string metabaseUrl)
    {
        // TODO: Implement this method
        return Task.CompletedTask;
    }
}
