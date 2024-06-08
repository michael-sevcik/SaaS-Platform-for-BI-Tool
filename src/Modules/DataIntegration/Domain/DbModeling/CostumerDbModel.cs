using BIManagement.Modules.DataIntegration.Domain.DbModelling;

namespace BIManagement.Modules.DataIntegration.Domain.DbModeling;

/// <summary>
/// Represents a model of a costumer's database.
/// </summary>
public class CostumerDbModel
{
    /// <summary>
    /// Gets or sets the name of the database.
    /// </summary>
    public string CostumerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the model of the database.
    /// </summary>
    public DbModel DbModel { get; set; } = new();
}
