namespace BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;

/// <summary>
/// Represents a repository for <see cref="CustomerDbConnectionConfiguration"/>.
/// </summary>
public interface ICustomerDbConnectionConfigurationRepository
{
    // TODO: Consider using result to pass potential errors
    Task UpdateAsync(CustomerDbConnectionConfiguration configuration);
    Task AddAsync(CustomerDbConnectionConfiguration configuration);
    Task DeleteAsync(string customerId);
    Task<CustomerDbConnectionConfiguration?> GetAsync(string userId);
}
