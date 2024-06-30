using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;

public class EntityMapping(
    string name,
    string? schema,
    ISourceEntity[] sourceEntities,
    ISourceEntity? sourceEntity,
    Dictionary<string, ColumnMapping> columnMappings,
    string? description = null) : IVisitable
{
    public string Name { get; } = name;
    public string? Schema { get; } = schema;
    public string? Description { get; } = description;

    public ISourceEntity[] SourceEntities { get; } = sourceEntities;

    public ISourceEntity? SourceEntity { get; } = sourceEntity;

    public Dictionary<string, ColumnMapping> ColumnMappings { get; } = columnMappings;

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}