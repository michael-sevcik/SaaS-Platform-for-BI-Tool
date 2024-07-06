using BIManagement.Common.Persistence.Repositories;
using BIManagement.Common.Shared.Errors;
using BIManagement.Modules.DataIntegration.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace BIManagement.Modules.DataIntegration.Persistence.Repositories;

/// <summary>
/// Base Repository for Data Integration module.
/// </summary>
/// <inheritdoc/>
internal abstract class DataIntegrationBaseRepository<TDerived, TEntity>(ILogger<TDerived> logger, DataIntegrationDbContext dbContext) :
    BaseRepository<TDerived, TEntity, DataIntegrationDbContext>(logger, dbContext)
    where TEntity : class
{
    /// <inheritdoc/>
    protected override Error DatabaseOperationFailedError => RepositoryErrors.DatabaseOperationFailed;

}
