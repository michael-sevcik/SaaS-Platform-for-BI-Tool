using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using System.ComponentModel.DataAnnotations;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping;

/// <summary>
/// Encapsulates the model of a target database table.
/// </summary>
public class TargetDbTable
{
    /// <summary>
    /// Gets or sets the identifier of this entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the schema of a table <see cref="TableModel"/>.
    /// </summary>
    [Required]
    public string Schema { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of a table <see cref="TableModel"/>.
    /// </summary>
    [Required]
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the model of a table from target database.
    /// </summary>
    [Required]
    public Table TableModel { get; set; } = default!;
}
