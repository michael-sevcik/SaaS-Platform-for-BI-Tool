using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Domain.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BIManagement.Modules.DataIntegration.Persistence.Repositories
{
    /// <summary>
    /// Default implementation of <see cref="ISchemaMappingRepository"/>.
    /// </summary>
    /// <inheritdoc/>
    internal class SchemaMappingRepository(ILogger<SchemaMappingRepository> logger, DataIntegrationDbContext dbContext) :
    DataIntegrationBaseRepository<SchemaMappingRepository, SchemaMapping>(logger, dbContext),
    ISchemaMappingRepository,
    IScoped
    {
        /// <inheritdoc/>
        public async Task<IReadOnlyList<SchemaMapping>> GetSchemaMappings(string customerId)
            => await entities.AsNoTracking().Where(mapping => mapping.CustomerId == customerId).ToListAsync();

        /// <inheritdoc/>
        public async Task<Result> SaveAsync(SchemaMapping schemaMapping)
        {
            Result result;

            // check if it exists and update it
            if (entities.Any(model => model.CustomerId == schemaMapping.CustomerId
                && model.TargetDbTableId == schemaMapping.TargetDbTableId))
            {
                result = await UpdateAsync(schemaMapping);
            }
            else
            {
                // else add it
                result = await AddAsync(schemaMapping);
            }

            entities.Entry(schemaMapping).State = EntityState.Detached;
            return result;
        }

        /// <inheritdoc/>
        public async Task<SchemaMapping?> GetSchemaMapping(string customerId, int targetDbTableId)
            => await entities.AsNoTracking().SingleOrDefaultAsync(model => model.CustomerId == customerId && model.TargetDbTableId == targetDbTableId);
    }
}
