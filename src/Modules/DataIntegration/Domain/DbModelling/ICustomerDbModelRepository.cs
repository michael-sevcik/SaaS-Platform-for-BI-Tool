using BIManagement.Common.Shared.Results;

namespace BIManagement.Modules.DataIntegration.Domain.DbModelling;

/// <summary>
/// Represents a repository for <see cref="CustomerDbModel"/>.
/// </summary>
public interface ICustomerDbModelRepository
{
    /// <summary>
    /// Asynchronously gets a <see cref="CustomerDbModel"/> by its <paramref name="customerId"/>.
    /// </summary>
    /// <param name="customerId">Id of the respective customer</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="CustomerDbModel"/> on success,
    /// otherwise null.
    /// </returns>
    Task<CustomerDbModel?> GetAsync(string customerId);

    /// <summary>
    /// Asynchronously gets all <see cref="CustomerDbModel"/>s.
    /// </summary>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns list of <see cref="CustomerDbModel"/> on success.
    /// </returns>
    Task<IReadOnlyList<CustomerDbModel>> GetAsync();

    /// <summary>
    /// Deletes a <see cref="CustomerDbModel"/> by its <paramref name="customerId"/>.
    /// </summary>
    /// <param name="customerId">Id of the respective customer</param>
    /// <returns>Task object that represents the asynchronous operation.</returns>
    Task DeleteAsync(string customerId);

    /// <summary>
    /// Asynchronously adds or updates the given instance of <see cref="CustomerDbModel"/>.
    /// </summary>
    /// <param name="customerDbModel">The model to save.</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="Result"/> that represents the result
    /// of the operation.
    /// </returns>
    Task<Result> SaveAsync(CustomerDbModel customerDbModel);
}
