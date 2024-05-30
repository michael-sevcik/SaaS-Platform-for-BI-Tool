using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.Users.Api;

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
