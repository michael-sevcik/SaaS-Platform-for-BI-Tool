using System.ComponentModel.DataAnnotations;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping;

/// <summary>
/// Encapsulates the <see cref="Mapping"/> of a given <see cref="TargetDbTable"/> to tables from database of customer with <see cref="CustomerId"/>.
/// </summary>
public class SchemaMapping
{
    /// <summary>
    /// Gets or sets the identifier of a customer.
    /// </summary>
    [Required] public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of a <see cref="TargetDbTable"/>.
    /// </summary>
    [Required] public int TargetDbTableId { get; set; }

    /// <summary>
    /// Gets or sets the flag indicating whether the mapping is complete.
    /// </summary>
    /// <remarks>
    /// Indicates whether all non-nullable columns are mapped.
    /// </remarks>
    [Required] public bool IsComplete { get; set; }

    /// <summary>
    /// Gets or sets the mapping of a <see cref="TargetDbTable"/> to tables from database of customer with <see cref="CustomerId"/>.
    /// </summary>
    [Required] public string Mapping { get; set; } = string.Empty;
}
