namespace BIManagement.Modules.DataIntegration.Domain.SchemaScraping;

/// <summary>
/// Represents a column in a database table - <see cref="Table"/>.
/// </summary>
public class Column
{
    string Name { get; set; } = string.Empty;
    // TODO: define a data type 
    DataType DataType { get; set; } = string.Empty;
}