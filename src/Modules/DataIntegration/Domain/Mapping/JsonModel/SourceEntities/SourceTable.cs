using System.Text.Json.Serialization;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;

/// <summary>
/// 
/// </summary>
public class SourceTable : ISourceEntity
{
    public SourceTable(string name, string[] selectedColumns)
        => (Name, SelectedColumns) = (name, selectedColumns);

    public string Name { get; }

    [JsonIgnore]
    public bool HasDependency => false;

    public string[] SelectedColumns { get; }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}