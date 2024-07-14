namespace BIManagement.Modules.DataIntegration.Domain.DbModelling;

/// <summary>
/// Represents a model of a customer's database.
/// </summary>
public class CustomerDbModel
{
    /// <summary>
    /// Gets or sets the name of the database.
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the model of the database.
    /// </summary>
    public DbModel DbModel { get; set; } = new();
}
