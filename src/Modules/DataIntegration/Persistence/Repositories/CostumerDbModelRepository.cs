using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Domain.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BIManagement.Modules.DataIntegration.Persistence.Repositories;

/// <summary>
/// Default implementation of <see cref="ICostumerDbModelRepository"/>.
/// </summary>
/// <param name="dbContext">The context of database in which costumer db models are stored.</param>
internal class CostumerDbModelRepository(ILogger<CostumerDbModelRepository> logger, DataIntegrationDbContext dbContext)
    : ICostumerDbModelRepository, IScoped
{
    private readonly DbSet<CostumerDbModel> costumerDbModels
        = dbContext.Set<CostumerDbModel>();

    /// <inheritdoc/>
    public async Task<Result> AddAsync(CostumerDbModel costumerDbModel)
    {
        await costumerDbModels.AddAsync(costumerDbModel);
        return await SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<CostumerDbModel?> GetAsync(string costumerId)
        => await costumerDbModels.SingleOrDefaultAsync(model => model.CostumerId == costumerId);

    /// <inheritdoc/>
    public async Task<Result> UpdateAsync(CostumerDbModel costumerDbModel)
    {
        costumerDbModels.Update(costumerDbModel);
        return await SaveChangesAsync();
    }

    private async Task<Result> SaveChangesAsync()
    {
        try
        {
            await dbContext.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Saving changes to database failed");
            return Result.Failure(RepositoryErrors.DatabaseOperationFailed);
        }
    }
}
