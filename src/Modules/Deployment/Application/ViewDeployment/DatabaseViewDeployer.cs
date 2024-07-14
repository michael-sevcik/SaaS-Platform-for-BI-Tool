using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Api;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace BIManagement.Modules.Deployment.Application.ViewDeployment;

/// <summary>
/// Represents a database view deployer.
/// </summary>
/// <param name="logger">The logger instance.</param>
internal class DatabaseViewDeployer(ILogger<DatabaseViewDeployer> logger) : IDatabaseViewDeployer, ISingleton
{
    /// <summary>
    /// Deploys views to the specified database.
    /// </summary>
    /// <param name="databaseProvider">The database provider.</param>
    /// <param name="connectionString">The database connection string.</param>
    /// <param name="views">The views to deploy.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<Result> DeployViewsAsync(DatabaseProvider databaseProvider, string connectionString, string[] views)
    {
        using var connection = databaseProvider switch
        {
            DatabaseProvider.SqlServer => GetMSSqlConnection(connectionString),
            _ => throw new NotSupportedException($"Database provider {databaseProvider} is not supported.")
        };

        try
        {
            await connection.OpenAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to open database connection.");
            return Result.Failure(new("Deployment.DatabaseViewDeployer.ConnectionFailed", "Failed to open database connection."));
        }

        using var transaction = connection.BeginTransaction();
        try
        {
            foreach (var view in views)
            {
                using var command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = view;
                await command.ExecuteNonQueryAsync();
            }

            transaction.Commit();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to deploy views.");
            transaction.Rollback();
            return Result.Failure(new("Deployment.DatabaseViewDeployer.DeploymentFailed", "Failed to deploy views"));
        }

        connection.Close();
        return Result.Success();
    }

    /// <summary>
    /// Gets a new instance of the MSSQL database connection.
    /// </summary>
    /// <param name="connectionString">The database connection string.</param>
    /// <returns>The MSSQL database connection.</returns>
    private DbConnection GetMSSqlConnection(string connectionString) => new SqlConnection(connectionString);
}
