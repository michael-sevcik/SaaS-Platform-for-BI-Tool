using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;

namespace BIManagement.Modules.DataIntegration.Domain.DbModelling;

/// <summary>
/// Represents a manager for <see cref="CostumerDbModel"/>, which is responsible for managing
/// the database model of a costumer.
/// </summary>
public interface ICostumerDbModelManager
{
    // TODO: Add methods for managing the database model of a costumer.
    /// <summary>
    /// Asynchronously gets a <see cref="DbModel"/> associated with costumer whose Id is <paramref name="costumerId"/>.
    /// </summary>
    /// <param name="costumerId">Id of the respective costumer</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="DbModel"/> on success,
    /// otherwise null.
    /// </returns>
    Task<DbModel?> GetAsync(string costumerId);

    /// <summary>
    /// Creates a <see cref="DbModel"/> for a costumer based on the given <paramref name="configuration"/>.
    /// </summary>
    /// <param name="configuration">The configuration for connection.</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="Result{DbModel}"/> which contains
    /// The <see cref="DbModel"/> on success, otherwise an error message.
    /// </returns>
    Task<Result<DbModel>> CreateDbModelAsync(CostumerDbConnectionConfiguration configuration);
}
