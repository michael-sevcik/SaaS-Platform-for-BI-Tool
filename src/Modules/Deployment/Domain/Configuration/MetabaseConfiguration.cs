using System;

namespace BIManagement.Modules.Deployment.Domain.Configuration;

/// <summary>
/// Represents the default admin settings.
/// </summary>
/// <param name="Email">The email of the default admin.</param>
/// <param name="Password">The password of the default admin.</param>
public record DefaultAdminSettings(string Email, string Password);

/// <summary>
/// Enumeration for database providers.
/// </summary>
public enum DatabaseProvider
{
    /// <summary>
    /// The MSSQL database provider.
    /// </summary>
    SqlServer = 1
}

/// <summary>
/// Represents the database settings.
/// </summary>
/// <param name="Provider">The database provider..</param>
/// <param name="DatabaseName">The name of the database.</param>
/// <param name="Host">The host of the database.</param>
/// <param name="Port">The port of the database.</param>
/// <param name="Username">The username for accessing the database.</param>
/// <param name="Password">The password for accessing the database.</param>
public record DatabaseSettings(DatabaseProvider Provider, string DatabaseName, string Host, int Port, string Username, string Password);

/// <summary>
/// Enumeration for SMTP security options.
/// </summary>
public enum SmtpSecurity
{
    None,
    Ssl,
    Tls,
    StartTls
}

/// <summary>
/// Represents the SMTP settings.
/// </summary>
public class SmtpSettings
{
    public string Host { get; init; } = string.Empty;
    public int Port {get; init; }
    public SmtpSecurity Security { get; init; } = SmtpSecurity.None;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="SmtpSettings"/> class.
    /// </summary>
    /// <param name="host">The SMTP host.</param>
    /// <param name="port">The SMTP port.</param>
    /// <param name="security">The SMTP security option.</param>
    /// <param name="username">The username for SMTP authentication.</param>
    /// <param name="password">The password for SMTP authentication.</param>
    public SmtpSettings(string host, int port, SmtpSecurity security, string username, string password)
    {
        this.Host = host;
        this.Port = port;
        this.Security = security;
        this.Username = username;
        this.Password = password;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmtpSettings"/> class.
    /// </summary>
    /// <remarks>
    /// Used for options binding.
    /// </remarks>
    public SmtpSettings()
    {
    }
};

/// <summary>
/// Represents the configuration for Metabase.
/// </summary>
/// <param name="Admin">The default admin settings.</param>
/// <param name="DatabaseSettings">The database settings.</param>
/// <param name="SmtpSettings">The SMTP settings.</param>
public record MetabaseConfiguration(DefaultAdminSettings Admin, DatabaseSettings DatabaseSettings, SmtpSettings SmtpSettings);
