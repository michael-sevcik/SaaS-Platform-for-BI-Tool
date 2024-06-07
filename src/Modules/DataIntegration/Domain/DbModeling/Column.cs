using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.Domain.DbModelling;

/// <summary>
/// Represents a column in a database table - <see cref="Table"/>.
/// </summary>
public class Column
{
    /// <summary>
    /// The name of the column.
    /// </summary>
    [JsonRequired]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The data type of the column.
    /// </summary>
    [JsonRequired]
    public DataTypeBase DataType { get; set; } = default!;
}