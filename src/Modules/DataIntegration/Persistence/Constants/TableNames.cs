using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Domain.Mapping;

namespace BIManagement.Modules.DataIntegration.Persistence.Constants;

/// <summary>
/// Encapsulates the table names used in the DataIntegration module.
/// </summary>
internal static class TableNames
{
    /// <summary>
    /// The table name for the database connection configurations - <see cref="CostumerDbConnectionConfiguration"/>.
    /// </summary>
    public const string DatabaseConnectionConfigurations = "DatabaseConnectionConfigurations";

    /// <summary>
    /// The table name for the database models of customers - <see cref="CostumerDbModel"/>.
    /// </summary>
    public const string CostumerDbModels = "CostumerDbModels";

    /// <summary>
    /// The table name for the schema mappings - <see cref="SchemaMapping"/>.
    /// </summary>
    public const string SchemaMappings = "SchemaMappings";

    /// <summary>
    /// The table name for the models of target database - <see cref="TargetDbTable"/>.
    /// </summary>
    public const string TargetDbTables = "TargetDbTables";
}
