namespace BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;

/// <summary>
/// Represents a repository for <see cref="CustomerDbConnectionConfiguration"/>.
/// </summary>
public interface ICustomerDbConnectionConfigurationRepository
{
    /// <summary>
    /// Updates the customer database connection configuration.
    /// </summary>
    /// <param name="configuration">The customer database connection configuration to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(CustomerDbConnectionConfiguration configuration);

    /// <summary>
    /// Adds a new customer database connection configuration.
    /// </summary>
    /// <param name="configuration">The customer database connection configuration to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(CustomerDbConnectionConfiguration configuration);

    /// <summary>
    /// Deletes the customer database connection configuration by customer ID.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(string customerId);

    /// <summary>
    /// Gets the customer database connection configuration by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the customer database connection configuration,
    /// or null if not found.
    /// </returns>
    Task<CustomerDbConnectionConfiguration?> GetAsync(string userId);
}
