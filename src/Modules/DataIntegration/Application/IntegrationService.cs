using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.DataIntegration.Api;

namespace BIManagement.Modules.DataIntegration.Application;

// TODO: Implement the integration service AND update documentation.
/// <summary>
/// No op integration service implementation.
/// </summary>
public class IntegrationService : IIntegrationService, ISigleton
{
    /// <inheritdoc/>
    public Task HandleUserDeletionAsync(string userId)
    {
        // todo: implement HandleUserDeletionAsync
        return Task.CompletedTask;
    }
}
