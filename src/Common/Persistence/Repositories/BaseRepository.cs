using BIManagement.Common.Shared.Errors;
using BIManagement.Common.Shared.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BIManagement.Common.Persistence.Repositories;

/// <summary>
/// Base class for specific repositories.
/// </summary>
/// <typeparam name="TDerived">Type of the derived final class, used for <paramref name="logger"/>.</typeparam>
/// <typeparam name="TEntity">Type which this repository persists.</typeparam>
/// <typeparam name="TContext">Type of context of database in which the <typeparamref name="TEntity"/> is persisted.</typeparam>
/// <param name="logger">The logger.</param>
/// <param name="dbContext">The context of database in which <typeparamref name="TEntity"/> entities are stored</param>
public abstract class BaseRepository<TDerived, TEntity, TContext>(ILogger<TDerived> logger, TContext dbContext)
    where TEntity : class
    where TContext : DbContext
{
    /// <summary>
    /// Gets the logger.
    /// </summary>
    protected ILogger<TDerived> Logger => logger;

    /// <summary>
    /// The <see cref="DbSet{TEntity}"/> for the entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    protected readonly DbSet<TEntity> entities = dbContext.Set<TEntity>();

    protected abstract Error DatabaseOperationFailedError { get; }

    /// <summary>
    /// Asynchronously updates an instance of <see cref="TEntity"/> in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="Result"/> that represents the result
    /// of the operation.
    /// </returns>
    protected async Task<Result> UpdateAsync(TEntity entity)
    {
        entities.Update(entity);
        return await SaveChangesAsync();
    }


    /// <summary>
    /// Asynchronously adds a new <see cref="TEntity"/> to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="Result"/> that represents the result
    /// of the operation.
    /// </returns>
    protected async Task<Result> AddAsync(TEntity entity)
    {
        await entities.AddAsync(entity);
        return await SaveChangesAsync();
    }

    /// <summary>
    /// Saves change to the database.
    /// </summary>
    /// <returns>
    /// Task object that represents the asynchronous operation.
    /// The Value property returns instance of <see cref="Result"/> that represents the result
    /// of the operation.
    /// </returns>
    protected async Task<Result> SaveChangesAsync()
    {
        try
        {
            await dbContext.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Saving changes to database failed");
            return Result.Failure(DatabaseOperationFailedError);
        }
    }
}
