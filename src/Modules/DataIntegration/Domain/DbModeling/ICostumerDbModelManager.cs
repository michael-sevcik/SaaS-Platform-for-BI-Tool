using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;

namespace BIManagement.Modules.DataIntegration.Domain.DbModeling;

/// <summary>
/// Represents a manager for <see cref="CostumerDbModel"/>, which is responsible for managing
/// the database model of a costumer.
/// </summary>
public interface ICostumerDbModelManager
{
    // TODO: Add methods for managing the database model of a costumer.
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
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    Task<Result<DbModel>> CreateDbModelAsync(CostumerDbConnectionConfiguration configuration);
}
