namespace BIManagement.Modules.Deployment.Api;

/// <summary>
/// Represents a service that integrates the deployment module with other modules.
/// </summary>
public interface IIntegrationService
{
    /// <summary>
    /// Handles the deletion of a user.
    /// </summary>
    /// <param name="userId">Id of the deleted user</param>
    /// <returns>Task object that represents the asynchronous operation.</returns>
    Task HandleCustomerDeletionAsync(string customerId);
}
