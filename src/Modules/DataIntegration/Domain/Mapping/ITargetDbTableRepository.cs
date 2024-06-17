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
    /// The Value property returns <see cref="IEnumerable{TargetDbTable}"/>.
    /// </returns>
    Task<IReadOnlyList<TargetDbTable>> GetTargetDbTables();
}
