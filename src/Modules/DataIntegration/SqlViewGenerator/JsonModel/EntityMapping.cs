﻿namespace SqlViewGenerator.JsonModel;

public class EntityMapping : IVisitable
{
    public EntityMapping(string name, ISourceEntity[] sourceEntities, ISourceEntity sourceEntity, Dictionary<string, ColumnMapping> columnMappings)
    {
        this.Name = name;
        this.SourceEntities = sourceEntities;
        this.SourceEntity = sourceEntity;
        this.ColumnMappings = columnMappings;
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