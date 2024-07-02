using System.Text.Json.Serialization;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;

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

    public string Name { get; set; }
    public string? Schema { get; set; }

    [JsonIgnore]
    string ISourceEntity.Name => Schema is null? Name : $"{Schema}.{Name}";

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