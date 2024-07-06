using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Deployment.Domain;
using BIManagement.Modules.Deployment.Domain.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BIManagement.Modules.Deployment.Persistence.Repositories;

/// <summary>
/// Default implementation of <see cref="IMetabaseDeploymentRepository"/>.
/// </summary>
/// <inheritdoc/>
internal class MetabaseDeploymentRepository(ILogger<MetabaseDeploymentRepository> logger, DeploymentDbContext dbContext) :
DeploymentBaseRepository<MetabaseDeploymentRepository, MetabaseDeployment>(logger, dbContext),
IMetabaseDeploymentRepository,
IScoped
{
    /// <inheritdoc/>
    public async Task<Result> SaveAsync(MetabaseDeployment costumerDbModel)
    {
        Result result;

        // check if it exists and update it
        if (entities.Any(model => model.CustomerId == costumerDbModel.CustomerId))
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
    public async Task<MetabaseDeployment?> GetAsync(string costumerId)
        => await entities.AsNoTracking().SingleOrDefaultAsync(model => model.CustomerId == costumerId);


    /// <inheritdoc/>
    public async Task<Result> AddDeploymentAsync(MetabaseDeployment deployment)
        => await AddAsync(deployment);

    /// <inheritdoc/>
    public async Task<Result> DeleteDeploymentAsync(string customerId)
    {
        int rowCount = await entities.Where(entities => entities.CustomerId == customerId).ExecuteDeleteAsync();

        Result result;
        if (rowCount == 1)
        {
            result = Result.Success();
        }
        else if (rowCount < 1)
        {
            result = Result.Failure(RepositoryErrors.EntityNotFound);
        }
        else
        {
            result = Result.Failure(RepositoryErrors.DatabaseOperationFailed);
        }

        return result;
    }
}