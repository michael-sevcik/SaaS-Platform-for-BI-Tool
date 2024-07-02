using BIManagement.Common.Shared.Results;

namespace BIManagement.Modules.DataIntegration.Api;

/// <summary>
/// Represents the database provider.
/// </summary>
public enum DatabaseProvider
{
    /// <summary>
    /// The MSSQL database provider.
    /// </summary>
    SqlServer = 1
}


/// <summary>
/// Represents a service for data integration.
/// </summary>
public interface IDataIntegrationService
{
    /// <summary>
    /// Generates SQL views for a customer.
    /// </summary>
    /// <param name="customerId">Id of the customer</param>
    /// <returns>Task object that represents the asynchronous operation.</returns>
    Task<Result<string[]>> GenerateSqlViewsForCustomer(string customerId);

    /// <summary>
    /// Retrieves the database connection string for a customer.
    /// </summary>
    /// <param name="customerId">Id of the customer</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The result is a tuple with the database provider and the connection string, if the configuration exists.
    /// </returns>
    Task<(DatabaseProvider, string)?> GetCustomerDbConnectionString(string customerId);
}
