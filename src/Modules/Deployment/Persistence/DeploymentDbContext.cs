using BIManagement.Modules.Deployment.Persistence.Constants;
using Microsoft.EntityFrameworkCore;

namespace BIManagement.Modules.Deployment.Persistence;

/// <summary>
/// Represents a database context of the Deployment module.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DeploymentDbContext"/> class.
/// </remarks>
/// <param name="options">The options for this context.</param>
public class DeploymentDbContext(DbContextOptions<DeploymentDbContext> options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);
        mb.HasDefaultSchema(Schemas.Deployment);
        mb.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}
