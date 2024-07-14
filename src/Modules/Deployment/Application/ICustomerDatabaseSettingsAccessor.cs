using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Deployment.Domain.Configuration;

namespace BIManagement.Modules.Deployment.Application;

/// <summary>
/// Represents an accessor for customer database settings.
/// </summary>
public interface ICustomerDatabaseSettingsAccessor
{
    /// <summary>
    /// Retrieves the database settings for a specific customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the database settings or error.
    /// </returns>
    Task<Result<DatabaseSettings>> GetDatabaseSettings(string customerId);
}
