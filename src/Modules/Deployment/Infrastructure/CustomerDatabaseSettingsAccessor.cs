using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Api;
using BIManagement.Modules.Deployment.Application;
using BIManagement.Modules.Deployment.Domain.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using DatabaseProvider = BIManagement.Modules.Deployment.Domain.Configuration.DatabaseProvider;

namespace BIManagement.Modules.Deployment.Infrastructure;

/// <summary>
/// Default implementation of <see cref="ICustomerDatabaseSettingsAccessor"/> interface.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="dataIntegrationService">The data integration service used to access the connection string of customers.</param>
internal class CustomerDatabaseSettingsAccessor(
    ILogger<CustomerDatabaseSettingsAccessor> logger,
    IDataIntegrationService dataIntegrationService) : ICustomerDatabaseSettingsAccessor, IScoped
{
    /// <summary>
    /// The default SQL Server port.
    /// </summary>
    private const int DefaultSqlServerPort = 1433;

    /// <inheritdoc/>
    public async Task<Result<DatabaseSettings>> GetDatabaseSettings(string customerId)
    {
        var dataIntegrationSettings = await dataIntegrationService.GetCustomerDbConnectionString(customerId);
        if (dataIntegrationSettings is null)
        {
            return Result.Failure<DatabaseSettings>(new(
                "Deployment.CustomerDatabaseSettingsAccessor.CustomerDbConnectionStringNotFound",
                "Cannot find a connection string associated with the given customer."));
        }

        var (dataIntegrationDatabaseProviders, connectionString) = dataIntegrationSettings.Value;

        return MapDatabaseProviders(dataIntegrationDatabaseProviders)
            .Map(databaseProvider => TransformConnectionStringToDatabaseSettings(databaseProvider, connectionString));
    }

    /// <summary>
    /// Maps the <see cref="DataIntegration.Api.DatabaseProvider"/> to <see cref="DatabaseProvider"/>.
    /// </summary>
    /// <param name="databaseProvider">The database provider to map.</param>
    /// <returns>The mapped <see cref="DatabaseProvider"/>.</returns>
    private Result<DatabaseProvider> MapDatabaseProviders(DataIntegration.Api.DatabaseProvider databaseProvider)
    {
        DatabaseProvider mappedDbProvider;
        switch (databaseProvider)
        {
            case DataIntegration.Api.DatabaseProvider.SqlServer:
                mappedDbProvider = DatabaseProvider.SqlServer;
                break;
            default:
                logger.LogError("The database provider '{DatabaseProvider}' is not supported.", databaseProvider);
                return Result.Failure<DatabaseProvider>(new(
                    "Deployment.CustomerDatabaseSettingsAccessor.DatabaseProviderNotSupported",
                    "The database provider is not supported."));
        }

        return Result.Success(mappedDbProvider);
    }

    /// <summary>
    /// Transforms the connection string to <see cref="DatabaseSettings"/>.
    /// </summary>
    /// <param name="databaseProvider">The database provider.</param>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>The transformed <see cref="DatabaseSettings"/>.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Result<DatabaseSettings> TransformConnectionStringToDatabaseSettings(DatabaseProvider databaseProvider, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return Result.Failure<DatabaseSettings>(new(
                "Deployment.CustomerDatabaseSettingsAccessor.ConnectionStringEmpty",
                "The connection string is empty."));
        }

        SqlConnectionStringBuilder connectionStringBuilder = new(connectionString);

        try
        {
            var (host, port) = ParseSqlServerDataSource(connectionStringBuilder.DataSource);

            var databaseSettings = new DatabaseSettings(
                databaseProvider,
                connectionStringBuilder.InitialCatalog,
                host,
                port,
                connectionStringBuilder.UserID,
                connectionStringBuilder.Password);

            return Result.Success(databaseSettings);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred during parsing of connection string: {connectionString}", connectionString);
            return Result.Failure<DatabaseSettings>(new(
                "Deployment.CustomerDatabaseSettingsAccessor.ConnectionStringParsingError",
                "An error occurred during parsing of the connection string."));
        }
    }

    /// <summary>
    /// Parses the SQL Server data source into host and port.
    /// </summary>
    /// <param name="dataSource">The SQL Server data source.</param>
    /// <exception cref="FormatException">The data source has two parts, but the port part is not parseable.</exception>
    /// <returns>A tuple containing the host and port.</returns>
    public static (string, int) ParseSqlServerDataSource(string dataSource)
    {
        var dataSourceParts = dataSource.Split(',');
        if (dataSourceParts.Length == 1)
        {
            return (dataSource, DefaultSqlServerPort);
        }

        var port = int.Parse(dataSourceParts[1]);
        if (port <= 0 || port > 65535)
        {
            throw new FormatException("The port part of the data source is not parsable.");
        }

        return (dataSourceParts[0], port);
    }
}
