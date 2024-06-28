using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;

public class EntityMapping : IVisitable
{
    public EntityMapping(string name, ISourceEntity[] sourceEntities, ISourceEntity sourceEntity, Dictionary<string, ColumnMapping> columnMappings)
    {
        Name = name;
        SourceEntities = sourceEntities;
        SourceEntity = sourceEntity;
        ColumnMappings = columnMappings;
    }

    public string Name { get; }

    public ISourceEntity[] SourceEntities { get; }

    public ISourceEntity SourceEntity { get; }

    public Dictionary<string, ColumnMapping> ColumnMappings { get; }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}