namespace BIManagement.Modules.Users.Domain;

/// <summary>
/// Represents a service that notifies other modules about the user deletion.
/// </summary>
public interface IIntegrationNotifier
{
    /// <summary>
    /// Notifies other modules about the user deletion.
    /// </summary>
    /// <param name="userId">Id of the deleted user.</param>
    /// <returns>Task object representing the asynchronous operation.</returns>
    Task SentCustomerDeletionNotification(string userId);
}
