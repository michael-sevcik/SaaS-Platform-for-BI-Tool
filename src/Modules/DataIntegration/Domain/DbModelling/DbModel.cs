namespace BIManagement.Modules.DataIntegration.Domain.DbModelling;

/// <summary>
/// Represents model of a database.
/// </summary>
public class DbModel
{
    /// <summary>
    /// Gets or sets the name of the database.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tables of the database.
    /// </summary>
    public ICollection<Table> Tables { get; set; } = [];
}
