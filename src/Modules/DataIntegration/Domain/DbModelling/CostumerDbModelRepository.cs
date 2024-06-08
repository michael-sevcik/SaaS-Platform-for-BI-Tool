using BIManagement.Common.Shared.Results;

namespace BIManagement.Modules.DataIntegration.Domain.DbModelling;

/// <summary>
/// Represents a repository for <see cref="CostumerDbModel"/>.
/// </summary>
public interface ICostumerDbModelRepository
{
    /// <summary>
    /// Asynchronously gets a <see cref="CostumerDbModel"/> by its <paramref name="costumerId"/>.
    /// </summary>
    /// <param name="costumerId">Id of the respective costumer</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="CostumerDbModel"/> on success,
    /// otherwise null.
    /// </returns>
    Task<CostumerDbModel?> GetAsync(string costumerId);

    /// <summary>
    /// Asynchronously adds a new <see cref="CostumerDbModel"/> to the repository.
    /// </summary>
    /// <param name="costumerDbModel">The model to add.</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="Result"/> that represents the result
    /// of the operation.
    /// </returns>
    Task<Result> AddAsync(CostumerDbModel costumerDbModel);

    /// <summary>
    /// Asynchronously adds a new <see cref="CostumerDbModel"/> to the repository.
    /// </summary>
    /// <param name="costumerDbModel">The model to update.</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="Result"/> that represents the result
    /// of the operation.
    /// </returns>
    Task<Result> UpdateAsync(CostumerDbModel costumerDbModel);
}
