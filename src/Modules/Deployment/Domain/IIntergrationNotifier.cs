namespace BIManagement.Modules.Deployment.Domain;

/// <summary>
/// Represents an integration notifier interface.
/// </summary>
public interface IIntegrationNotifier
{
    /// <summary>
    /// Sends a notification for the deployed Metabase.
    /// </summary>
    /// <param name="customerId">The customer ID.</param>
    /// <param name="metabaseUrl">The Metabase URL.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SentMetabaseDeployedNotification(string customerId, string metabaseUrl);
}
