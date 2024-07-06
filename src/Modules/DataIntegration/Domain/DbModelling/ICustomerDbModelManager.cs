using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;

namespace BIManagement.Modules.DataIntegration.Domain.DbModelling;

/// <summary>
/// Represents a manager for <see cref="CustomerDbModel"/>, which is responsible for managing
/// the database model of a customer.
/// </summary>
public interface ICustomerDbModelManager
{
    // TODO: Add methods for managing the database model of a customer.
    /// <summary>
    /// Asynchronously gets a <see cref="DbModel"/> associated with customer whose Id is <paramref name="customerId"/>.
    /// </summary>
    /// <param name="customerId">Id of the respective customer</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="DbModel"/> on success,
    /// otherwise null.
    /// </returns>
    Task<DbModel?> GetAsync(string customerId);

    /// <summary>
    /// Creates a <see cref="DbModel"/> for a customer based on the given <paramref name="configuration"/>.
    /// </summary>
    /// <param name="configuration">The configuration for connection.</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="Result{DbModel}"/> which contains
    /// The <see cref="DbModel"/> on success, otherwise an error message.
    /// </returns>
    Task<Result<DbModel>> CreateDbModelAsync(CustomerDbConnectionConfiguration configuration);
}
