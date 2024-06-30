using System.Text.Json.Serialization;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;

/// <summary>
/// 
/// </summary>
[method: JsonConstructor]
public class SourceTable(string name, string? schema, SourceColumn[] selectedColumns) : ISourceEntity
{
    public const string TypeDiscriminator = "sourceTable";
    public string Name { get; } = name;
    public string? Schema { get; } = schema;

    [JsonIgnore]
    string ISourceEntity.Name => Schema is null? Name : $"{Schema}.{Name}";

    [JsonIgnore]
    public bool HasDependency => false;

    /// <inheritdoc/>
    public SourceColumn[] SelectedColumns { get; } = selectedColumns;

    /// <inheritdoc/>
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    /// <inheritdoc/>
    public void AssignColumnOwnership()
    {
        foreach (var column in SelectedColumns)
        {
            column.Owner = this;
        }
    }
}