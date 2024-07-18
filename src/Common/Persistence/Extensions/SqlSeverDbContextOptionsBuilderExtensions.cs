using BIManagement.Common.Persistence.Constants;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BIManagement.Common.Persistence.Extensions;


/// <summary>
/// Contains extension method for the <see cref="SqlServerDbContextOptionsBuilder"/> class.
/// </summary>
public static class SqlSeverDbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Configures the migration history table to live in the specified schema.
    /// </summary>
    /// <param name="dbContextOptionsBuilder">The database context options builder.</param>
    /// <param name="schema">The schema.</param>
    /// <returns>The same database context options builder.</returns>
    public static SqlServerDbContextOptionsBuilder WithMigrationHistoryTableInSchema(
        this SqlServerDbContextOptionsBuilder dbContextOptionsBuilder,
        string schema)
            => dbContextOptionsBuilder.MigrationsHistoryTable(TableNames.MigrationHistory, schema);
}
