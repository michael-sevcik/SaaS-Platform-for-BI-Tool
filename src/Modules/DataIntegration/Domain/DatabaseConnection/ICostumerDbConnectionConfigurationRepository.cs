namespace BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;

/// <summary>
/// Represents a repository for <see cref="CostumerDbConnectionConfiguration"/>.
/// </summary>
public interface ICostumerDbConnectionConfigurationRepository
{
    // TODO: Consider using result to pass potential errors
    Task UpdateAsync(CostumerDbConnectionConfiguration configuration);
    Task AddAsync(CostumerDbConnectionConfiguration configuration);
    Task DeleteAsync(string costumerId);
    Task<CostumerDbConnectionConfiguration?> GetAsync(string userId);
}
