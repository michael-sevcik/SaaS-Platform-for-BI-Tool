using BIManagement.Common.Persistence.Repositories;
using BIManagement.Common.Shared.Errors;
using BIManagement.Modules.Deployment.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace BIManagement.Modules.Deployment.Persistence.Repositories;

/// <summary>
/// Base Repository for Data Integration module.
/// </summary>
/// <inheritdoc/>
internal abstract class DeploymentBaseRepository<TDerived, TEntity>(ILogger<TDerived> logger, DeploymentDbContext dbContext) :
    BaseRepository<TDerived, TEntity, DeploymentDbContext>(logger, dbContext)
    where TEntity : class
{
    /// <inheritdoc/>
    protected override Error DatabaseOperationFailedError => RepositoryErrors.DatabaseOperationFailed;

}

