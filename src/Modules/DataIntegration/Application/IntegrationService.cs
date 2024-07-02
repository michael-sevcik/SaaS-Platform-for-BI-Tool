using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Api;

namespace BIManagement.Modules.DataIntegration.Application;

// TODO: Implement the integration service AND update documentation.
/// <summary>
/// No op integration service implementation.
/// </summary>
public class IntegrationService : IIntegrationService, IScoped
{
    /// <inheritdoc/>
    public Task HandleCustomerDeletionAsync(string userId)
    {
        // todo: implement HandleUserDeletionAsync
        return Task.CompletedTask;
    }
}
