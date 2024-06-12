using BIManagement.Common.Shared.Results;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping;

/// <summary>
/// Represents a repository for <see cref="SchemaMapping"/> entities.
/// </summary>
public interface ISchemaMappingRepository
{
    /// <summary>
    /// Gets all schema mappings of a given costumer.
    /// </summary>
    /// <param name="costumerId">Id of the respective costumer</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="Result{IEnumerable{SchemaMapping}}"/> which contains
    /// The <see cref="IEnumerable{SchemaMapping}"/> on success, otherwise an error message.
    /// </returns>
    Task<Result<IEnumerable<SchemaMapping>>> GetSchemaMappings(string costumerId);

    /// <summary>
    /// Gets a schema mapping of a given costumer and target database table.
    /// </summary>
    /// <param name="costumerId">Id of the respective costumer </param>
    /// <param name="targetDbTableId">Id of the respective table from the target database.</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="SchemaMapping"/> on success otherwise a null.
    /// </returns>
    Task<SchemaMapping?> GetSchemaMapping(string costumerId, int targetDbTableId);

    /// <summary>
    /// Asynchronously adds or updates the given instance of <see cref="SchemaMapping"/>.
    /// </summary>
    /// <param name="schemaMapping">The mapping to save.</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="Result"/> that represents the result
    /// of the operation.
    /// </returns>
    Task<Result> SaveAsync(SchemaMapping schemaMapping);
}
