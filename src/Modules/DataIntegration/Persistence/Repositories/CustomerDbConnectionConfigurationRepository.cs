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
    private readonly DbSet<CustomerDbConnectionConfiguration> databaseConnectionConfigurations = dbContext.Set<CustomerDbConnectionConfiguration>();

    /// <inheritdoc/>
    public Task DeleteAsync(string userId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<CustomerDbConnectionConfiguration?> GetAsync(string userId)
        => await databaseConnectionConfigurations.FirstOrDefaultAsync(x => x.CustomerId == userId);

    /// <inheritdoc/>
    public async Task UpdateAsync(CustomerDbConnectionConfiguration configuration)
    {
        databaseConnectionConfigurations.Update(configuration);
        await dbContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task AddAsync(CustomerDbConnectionConfiguration configuration)
    {
        await databaseConnectionConfigurations.AddAsync(configuration);
        await dbContext.SaveChangesAsync();
    }
}
