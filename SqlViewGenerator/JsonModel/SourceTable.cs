using System.Text.Json.Serialization;

namespace SqlViewGenerator.JsonModel;

public class SourceTable : ISourceEntity
{
    public SourceTable(string name, string[] selectedColumns)
        => (this.Name, this.SelectedColumns) = (name, selectedColumns);

    public string Name { get; }

    [JsonIgnore]
    public bool HasDependency => false;

    public string[] SelectedColumns { get; }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}