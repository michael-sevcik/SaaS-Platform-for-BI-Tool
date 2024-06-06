﻿using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.DataIntegration.Domain;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using Microsoft.EntityFrameworkCore;

namespace BIManagement.Modules.DataIntegration.Persistence.Repositories;

/// <summary>
/// Default implementation of <see cref="IDbConnectionConfigurationRepository"/>.
/// </summary>
/// <param name="dbContext">The context of database in which Database connection configurations are stored.</param>
internal class DbConnectionConfigurationRepository(DataIntegrationDbContext dbContext)
    : IDbConnectionConfigurationRepository, IScoped
{
    private readonly DbSet<DbConnectionConfiguration> databaseConnectionConfigurations = dbContext.Set<DbConnectionConfiguration>();

    /// <inheritdoc/>
    public Task DeleteAsync(string userId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<DbConnectionConfiguration?> GetAsync(string userId)
        => await databaseConnectionConfigurations.FirstOrDefaultAsync(x => x.UserId == userId);

    /// <inheritdoc/>
    public async Task SaveAsync(string userId, DbConnectionConfiguration configuration)
    {
        await databaseConnectionConfigurations.AddAsync(configuration);
        await dbContext.SaveChangesAsync();
    }
}
