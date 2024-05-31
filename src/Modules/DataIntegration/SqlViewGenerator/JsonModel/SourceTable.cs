using BIManagement.Modules.DataIntegration.SqlViewGenerator;
using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.SqlViewGenerator.JsonModel;

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