using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BIManagement.Modules.DataIntegration.Persistence.Repositories;

/// <summary>
/// Default implementation of <see cref="ICustomerDbModelRepository"/>.
/// </summary>
/// <param name="dbContext">The context of database in which customer db models are stored.</param>
/// <inheritdoc/>
internal class CustomerDbModelRepository(ILogger<CustomerDbModelRepository> logger, DataIntegrationDbContext dbContext) :
    DataIntegrationBaseRepository<CustomerDbModelRepository, CustomerDbModel>(logger, dbContext),
    ICustomerDbModelRepository,
    IScoped
{
    /// <inheritdoc/>
    public async Task<Result> SaveAsync(CustomerDbModel customerDbModel)
    {
        Result result;

        // check if it exists and update it
        if (entities.Any(model => model.CustomerId == customerDbModel.CustomerId))
        {
            result = await UpdateAsync(customerDbModel);
        }
        else
        {
            // else add it
            result = await AddAsync(customerDbModel);
        }

        entities.Entry(customerDbModel).State = EntityState.Detached;
        return result;
    }

    /// <inheritdoc/>
    public async Task<CustomerDbModel?> GetAsync(string customerId)
        => await entities.AsNoTracking().SingleOrDefaultAsync(model => model.CustomerId == customerId);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CustomerDbModel>> GetAsync()
        => await entities.AsNoTracking().ToListAsync();
    /// <inheritdoc/>

    public Task DeleteAsync(string customerId)
        => entities.Where(model => model.CustomerId == customerId).ExecuteDeleteAsync();
}
