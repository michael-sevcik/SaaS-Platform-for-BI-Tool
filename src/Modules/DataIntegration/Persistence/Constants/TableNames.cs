using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;

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

    // TODO: Add more table names here
}
