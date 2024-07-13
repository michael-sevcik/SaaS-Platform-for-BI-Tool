using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.Deployment.Api;

namespace BIManagement.Modules.Deployment.Application;

// TODO: Implement the integration service AND update documentation.
/// <summary>
/// No op integration service
/// </summary>
public class IntegrationService : IIntegrationService, ISingleton
{
    public Task HandleCustomerDeletionAsync(string userId)
    {
        // TODO: IMPLEMENT HandleUserDeletionAsync
        return Task.CompletedTask;
    }
}
