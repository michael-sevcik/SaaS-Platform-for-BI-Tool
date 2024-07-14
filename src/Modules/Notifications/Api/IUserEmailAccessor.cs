namespace BIManagement.Modules.Notifications.Api;

/// <summary>
/// Represents an interface for accessing user email addresses.
/// </summary>
public interface IUserEmailAccessor
{
    /// <summary>
    /// Gets the email address of a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The email address of the user.</returns>
    Task<string?> GetUserEmailAsync(string userId);
}
