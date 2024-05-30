namespace BIManagement.Modules.DataIntegration.Api;

/// <summary>
/// Represents a service that integrates the DataIntegration module with other services.
/// </summary>
public interface IIntegrationService
{
    /// <summary>
    /// Handles the deletion of a user.
    /// </summary>
    /// <param name="userId">Id of the deleted user</param>
    /// <returns>Task object that represents the asynchronous operation.</returns>
    Task HandleUserDeletionAsync(string userId);
}
