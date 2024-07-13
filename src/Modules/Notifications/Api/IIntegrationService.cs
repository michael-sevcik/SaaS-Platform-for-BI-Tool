namespace BIManagement.Modules.Notifications.Api;

/// <summary>
/// Represents a service that integrates the notification module with other modules.
/// </summary>
public interface IIntegrationService
{
    /// <summary>
    /// Handles the deployment of a user.
    /// </summary>
    /// <param name="customerId">The customer ID.</param>
    /// <param name="metabaseUrl">The Metabase URL.</param>
    /// <returns>Task object that represents the asynchronous operation.</returns>
    Task HandleMetabaseDeployment(string userId, string metabaseUrl);
}
