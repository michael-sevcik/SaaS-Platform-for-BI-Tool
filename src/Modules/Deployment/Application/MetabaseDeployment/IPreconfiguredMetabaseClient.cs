using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Deployment.Domain.Configuration;

namespace BIManagement.Modules.Deployment.Application.MetabaseDeployment;


/// <summary>
/// Represents a client for interacting with an preconfigured Metabase.
/// </summary>
public interface IPreconfiguredMetabaseClient : IDisposable
{
    /// <summary>
    /// Changes the default admin email.
    /// </summary>
    /// <param name="email">The new email address.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Result> ChangeDefaultAdminEmail(string email);

    /// <summary>
    /// Changes the default admin password.
    /// </summary>
    /// <param name="password">The new password.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Result> ChangeDefaultAdminPassword(string password);

    /// <summary>
    /// Configures the database settings.
    /// </summary>
    /// <param name="databaseSettings">The database configuration.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Result> ConfigureDatabaseAsync(DatabaseSettings databaseSettings);

    /// <summary>
    /// Configures the SMTP settings.
    /// </summary>
    /// <param name="smtpConfiguration">The SMTP configuration.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Result> ConfigureSmtpAsync(SmtpSettings smtpConfiguration);

    /// <summary>
    /// Deletes the default token.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Result> DeleteDefaultTokenAsync();
}
