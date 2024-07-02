using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using Microsoft.EntityFrameworkCore;

namespace BIManagement.Modules.DataIntegration.Persistence.Repositories;

/// <summary>
/// Default implementation of <see cref="ICustomerDbConnectionConfigurationRepository"/>.
/// </summary>
/// <param name="dbContext">The context of database in which Database connection configurations are stored.</param>
internal class CustomerDbConnectionConfigurationRepository(DataIntegrationDbContext dbContext)
    : ICustomerDbConnectionConfigurationRepository, IScoped
{
    private readonly DbSet<CostumerDbConnectionConfiguration> databaseConnectionConfigurations = dbContext.Set<CostumerDbConnectionConfiguration>();

    /// <inheritdoc/>
    public Task DeleteAsync(string userId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<CostumerDbConnectionConfiguration?> GetAsync(string userId)
        => await databaseConnectionConfigurations.FirstOrDefaultAsync(x => x.CostumerId == userId);

    /// <inheritdoc/>
    public async Task UpdateAsync(CostumerDbConnectionConfiguration configuration)
    {
        databaseConnectionConfigurations.Update(configuration);
        await dbContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task AddAsync(CostumerDbConnectionConfiguration configuration)
    {
        await databaseConnectionConfigurations.AddAsync(configuration);
        await dbContext.SaveChangesAsync();
    }
}
