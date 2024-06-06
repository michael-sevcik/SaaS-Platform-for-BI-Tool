using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.Domain.SchemaScraping;

/// <summary>
/// Represents a column in a database table - <see cref="Table"/>.
/// </summary>
public class Column
{
    /// <summary>
    /// The name of the column.
    /// </summary>
    [JsonRequired]
    string Name { get; set; } = string.Empty;

    /// <summary>
    /// The data type of the column.
    /// </summary>
    [JsonRequired]
    DataTypeBase DataType { get; set; } = default!;
}