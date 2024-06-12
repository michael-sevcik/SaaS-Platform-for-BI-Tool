using System.ComponentModel.DataAnnotations;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping;

/// <summary>
/// Encapsulates the <see cref="Mapping"/> of a given <see cref="TargetDbTable"/> to tables from database of costumer with <see cref="CostumerId"/>.
/// </summary>
public class SchemaMapping
{
    /// <summary>
    /// Gets or sets the identifier of a costumer.
    /// </summary>
    [Required]
    public string CostumerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of a <see cref="TargetDbTable"/>.
    /// </summary>
    [Required]
    public int TargetDbTableId { get; set; }


    /// <summary>
    /// Gets or sets the mapping of a <see cref="TargetDbTable"/> to tables from database of costumer with <see cref="CostumerId"/>.
    /// </summary>
    [Required]
    public string Mapping { get; set; } = string.Empty;
}
