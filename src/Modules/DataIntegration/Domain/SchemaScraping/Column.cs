using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.Domain.SchemaScraping;

/// <summary>
/// Represents a column in a database table - <see cref="Table"/>.
/// </summary>
public class Column
{
    [JsonRequired]
    string Name { get; set; } = string.Empty;

    [JsonRequired]
    DataTypeBase DataType { get; set; } = default!;
}