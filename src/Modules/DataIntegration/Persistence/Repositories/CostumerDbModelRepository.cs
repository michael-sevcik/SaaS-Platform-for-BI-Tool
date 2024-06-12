using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BIManagement.Modules.DataIntegration.Persistence.Repositories;

/// <summary>
/// Default implementation of <see cref="ICostumerDbModelRepository"/>.
/// </summary>
/// <param name="dbContext">The context of database in which costumer db models are stored.</param>
/// <inheritdoc/>
internal class CostumerDbModelRepository(ILogger<CostumerDbModelRepository> logger, DataIntegrationDbContext dbContext) :
    DataIntegrationBaseRepository<CostumerDbModelRepository, CostumerDbModel>(logger, dbContext),
    ICostumerDbModelRepository,
    IScoped
{
    /// <inheritdoc/>
    public async Task<Result> SaveAsync(CostumerDbModel costumerDbModel)
    {
        Result result;

        // check if it exists and update it
        if (entities.Any(model => model.CostumerId == costumerDbModel.CostumerId))
        {
            result = await UpdateAsync(costumerDbModel);
        }
        else
        {
            // else add it
            result = await AddAsync(costumerDbModel);
        }

        entities.Entry(costumerDbModel).State = EntityState.Detached;
        return result;
    }

    /// <inheritdoc/>
    public async Task<CostumerDbModel?> GetAsync(string costumerId)
        => await entities.AsNoTracking().SingleOrDefaultAsync(model => model.CostumerId == costumerId);
}
