using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;

/// <summary>
/// Represents a source table in the JSON mapping model.
/// </summary>
public class SourceTable : ISourceEntity
{
    public const string TypeDiscriminator = "sourceTable";

    public SourceTable()
    {
        this.Name = "";
        this.Schema = null;
        this.SelectedColumns = Array.Empty<SourceColumn>();
    }

    public SourceTable(string name, string? schema, SourceColumn[] selectedColumns)
    {
        Name= name;
        Schema= schema;
        SelectedColumns= selectedColumns;
    }

    /// <summary>
    /// Gets or sets the name of the source table.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the schema of the source table.
    /// </summary>
    public string? Schema { get; set; }

    [JsonIgnore]
    string ISourceEntity.Name => Schema is null ? Name : $"{Schema}__{Name}";

    [JsonIgnore]
    public bool HasDependency => false;

    /// <inheritdoc/>
    public SourceColumn[] SelectedColumns { get; set; }

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
