using System.Text.Json.Serialization;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;

/// <summary>
/// 
/// </summary>
public class SourceTable(string name, string[] selectedColumns) : ISourceEntity
{
    public const string TypeDiscriminator = "sourceTable";

    public string Name { get; } = name;

    [JsonIgnore]
    public bool HasDependency => false;

    public string[] SelectedColumns { get; } = selectedColumns;

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}