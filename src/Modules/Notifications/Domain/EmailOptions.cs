namespace BIManagement.Modules.Notifications.Domain;

// TODO: USE
/// <summary>
/// Represents the options for mailing configuration.
/// </summary>
public sealed class EmailOptions
{
    /// <summary>
    /// Gets sets the base URL.
    /// </summary>
    public string BaseUrl { get; init; } = string.Empty;

    // TODO: IS THIS NEEDED?
    /// <summary>
    /// Gets the sender email.
    /// </summary>
    public string SenderEmail { get; init; } = string.Empty;

    /// <summary>
    /// Gets the SMTP server address.
    /// </summary>
    public string SmtpServer { get; init; } = string.Empty;

    /// <summary>
    /// Gets the SMTP port.
    /// </summary>
    public int SmtpPort { get; init; }

    /// <summary>
    /// Gets the SMTP user name.
    /// </summary>
    public string SmtpUsername { get; init; } = string.Empty;

    /// <summary>
    /// Gets the SMTP password.
    /// </summary>
    public string SmtpPassword { get; init; } = string.Empty;
}
