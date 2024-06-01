namespace BIManagement.Modules.Users.Infrastructure.Options.Identity;

/// <summary>
/// Represents the configuration for the default admin's account.
/// </summary>
public sealed class DefaultAdminOptions
{
    /// <summary>
    /// Gets the email address of the default admin.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Gets the name of the default admin.
    /// </summary>
    public string Name { get; init; } = string.Empty;
}
