using BIManagement.Common.Shared.Results;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping;

/// <summary>
/// Represents a repository for <see cref="SchemaMapping"/> entities.
/// </summary>
public interface ITargetDbTableRepository
{
    /// <summary>
    /// Gets all target database tables.
    /// </summary>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="Result{IEnumerable{TargetDbTable}}"/> which contains
    /// The <see cref="IEnumerable{TargetDbTable}"/> on success, otherwise an error message.
    /// </returns>
    Task<Result<IEnumerable<TargetDbTable>>> GetTargetDbTables();
}
