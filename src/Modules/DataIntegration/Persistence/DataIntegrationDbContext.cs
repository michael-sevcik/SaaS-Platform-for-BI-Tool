using BIManagement.Modules.DataIntegration.Domain;
using BIManagement.Modules.DataIntegration.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Persistence;

/// <summary>
/// Represents the Data integration database context.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DataIntegrationDbContext"/> class.
/// </remarks>
/// <param name="options"></param>
public class DataIntegrationDbContext(DbContextOptions<DataIntegrationDbContext> options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);
        mb.HasDefaultSchema(Schemas.DataIntegration);
        mb.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}
