using BIManagement.Common.Shared.Results;

namespace BIManagement.Modules.DataIntegration.Domain.SchemaScraping;

/// <summary>
/// Represents a object for building a model of a database.
/// </summary>
public interface IDbModelBuilder
{
    /// <summary>Asynchronously creates a model of a database.</summary>
    /// <param name="configuration">
    /// The configuration for connecting to the database,
    /// which is supposed to be modeled.
    /// </param>
    /// <returns>
    /// A task object that represents the asynchronous operation.
    /// Result property on success contains the model of the database,
    /// otherwise error.
    /// </returns>
    Task<Result<DbModel>> CreateAsync(DbConnectionConfiguration configuration);
}
