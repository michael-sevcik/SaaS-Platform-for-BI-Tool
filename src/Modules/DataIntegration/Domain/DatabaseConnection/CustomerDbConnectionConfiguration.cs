namespace BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;

/// <summary>
/// Represents the configuration of a database connection.
/// </summary>
public class DbConnectionConfiguration
{
    /// <summary>
    /// Gets or sets the provider of the database.
    /// </summary>
    public DatabaseProvider Provider { get; set; } = 0;

    /// <summary>
    /// Gets or sets the connection string in the native format of specified <see cref="Provider"/>.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;
}



/// <summary>
/// Represents the configuration of a database connection to customers database.
/// </summary>
public class CustomerDbConnectionConfiguration : DbConnectionConfiguration
{
    /// <summary>
    /// Gets or sets the identifier of user with which is this configuration associated.
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;
}
