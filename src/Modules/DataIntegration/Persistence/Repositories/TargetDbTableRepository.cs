using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Domain.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BIManagement.Modules.DataIntegration.Persistence.Repositories;

/// <summary>
/// Default implementation of <see cref="ITargetDbTableRepository"/>.
/// </summary>
/// <inheritdoc/>
internal class TargetDbTableRepository(ILogger<TargetDbTableRepository> logger, DataIntegrationDbContext dbContext) :
    DataIntegrationBaseRepository<TargetDbTableRepository, TargetDbTable>(logger, dbContext),
    ITargetDbTableRepository,
    IScoped
{
    /// <inheritdoc/>
    public async Task<Result<IEnumerable<TargetDbTable>>> GetTargetDbTables()
    {
        IEnumerable<TargetDbTable> targetDbTables = await entities.AsNoTracking().ToListAsync();
        return Result.Success(targetDbTables);
    }
}
