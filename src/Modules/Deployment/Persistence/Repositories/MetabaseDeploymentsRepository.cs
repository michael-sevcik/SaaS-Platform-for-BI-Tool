﻿using BIManagement.Common.Application.ServiceLifetimes;
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
    public async Task<Result> SaveAsync(MetabaseDeployment deployment)
    {
        Result result;

        // check if it exists and update it
        if (entities.Any(model => model.CustomerId == deployment.CustomerId))
        {
            result = await UpdateAsync(deployment);
        }
        else
        {
            // else add it
            result = await AddAsync(deployment);
        }

        entities.Entry(deployment).State = EntityState.Detached;
        return result;
    }

    /// <inheritdoc/>
    public async Task<MetabaseDeployment?> GetAsync(string customerId)
        => await entities.AsNoTracking().SingleOrDefaultAsync(model => model.CustomerId == customerId);

    /// <inheritdoc/>
    public async Task<Result> SaveDeploymentAsync(MetabaseDeployment deployment)
    {
        Result result;

        // check if it exists and update it
        if (entities.Any(model => model.CustomerId == deployment.CustomerId))
        {
            result = await UpdateAsync(deployment);
        }
        else
        {
            // else add it
            result = await AddAsync(deployment);
        }

        entities.Entry(deployment).State = EntityState.Detached;
        return result;
    }

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

    public async Task<IReadOnlyList<MetabaseDeployment>> GetAsync()
        => await entities.AsNoTracking().ToListAsync();
}