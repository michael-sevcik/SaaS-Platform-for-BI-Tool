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
/// <param name="Host">The SMTP host.</param>
/// <param name="Port">The SMTP port.</param>
/// <param name="Security">The SMTP security option.</param>
/// <param name="Username">The username for SMTP authentication.</param>
/// <param name="Password">The password for SMTP authentication.</param>
public record SmtpSettings(string Host, int Port, SmtpSecurity Security, string Username, string Password);

/// <summary>
/// Represents the configuration for Metabase.
/// </summary>
/// <param name="Admin">The default admin settings.</param>
/// <param name="DatabaseSettings">The database settings.</param>
/// <param name="SmtpSettings">The SMTP settings.</param>
public record MetabaseConfiguration(DefaultAdminSettings Admin, DatabaseSettings DatabaseSettings, SmtpSettings SmtpSettings);
