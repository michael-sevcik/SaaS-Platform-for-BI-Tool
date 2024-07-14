using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.Notifications.Api;
using Microsoft.Extensions.Logging;

namespace BIManagement.Modules.Notifications.Application;

/// <summary>
/// Implementation of the <see cref="IIntegrationService"/> interface.
/// </summary>
class IntegrationService(ILogger<IntegrationService> logger, IEmailSender emailSender, IUserEmailAccessor userEmailAccessor) : IIntegrationService, IScoped
{
    /// <inheritdoc/>
    public async Task HandleMetabaseDeployment(string userId, string metabaseUrl)
    {
        var userEmail = await userEmailAccessor.GetUserEmailAsync(userId);
        if (userEmail is null)
        {
            logger.LogError("User with ID {UserId} does not have an email address.", userId);
            return;
        }

        await emailSender.SendGeneralNotification(userEmail, "Metabase deployment", $"Metabase has been deployed to {metabaseUrl}.");
    }
}
