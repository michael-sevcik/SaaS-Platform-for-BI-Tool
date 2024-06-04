using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;

namespace BIManagement.Modules.DataIntegration.Domain;

/// <summary>
/// Represents the configuration of a database connection.
/// </summary>
public class DatabaseConnectionConfiguration
{
    /// <summary>
    /// Gets or sets the provider of the database.
    /// </summary>
    public DatabaseProvider Provider { get; set; } = 0;

    /// <summary>
    /// Gets or sets the connection string in the native format of specified <see cref="Provider"/>.
    /// </summary>
    string ConnectionString { get; set; } = string.Empty;
}
