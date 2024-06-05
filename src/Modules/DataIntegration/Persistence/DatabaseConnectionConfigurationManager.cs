using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.DataIntegration.Domain;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Persistence;

internal class DatabaseConnectionConfigurationManager(DataIntegrationDbContext dbContext) : IDatabaseConnectionConfigurationRepository, IScoped
{
    /// <inheritdoc/>
    public Task DeleteAsync(string userId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<DatabaseConnectionConfiguration?> GetAsync(string userId)
        => await dbContext.DatabaseConnectionConfigurations.FirstOrDefaultAsync(x => x.UserId == userId);

    /// <inheritdoc/>
    public async Task SaveAsync(string userId, DatabaseConnectionConfiguration configuration)
    {
        await dbContext.AddAsync(configuration);
        await dbContext.SaveChangesAsync();
    }
}
