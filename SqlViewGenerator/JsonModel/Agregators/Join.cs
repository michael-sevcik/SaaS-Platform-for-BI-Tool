using SqlViewGenerator.JsonModel.Agregators.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SqlViewGenerator.JsonModel.Agregators;


public class Join : IAgregator
{
    public enum Type
    {
        Left,
        Natural,
        Inner,
    }

    public ISourceEntity LeftSourceEntity => this.SourceEntities[0];

    public ISourceEntity RightSourceEntity => this.SourceEntities[1];

    public Join(
        Type joinType,
        ISourceEntity leftSourceEntity,
        ISourceEntity rightSourceEntity,
        string name,
        ColumnMapping[] outputColumns,
        JoinCondition condition)
    {
        this.JoinType = joinType;
        //if (sourceEntities.Length != 2) // TODO: delete
        //{
        //    throw new ArgumentException("Wrong number of source entities.", nameof(sourceEntities));
        //}

        this.SourceEntities = new ISourceEntity[] { leftSourceEntity, rightSourceEntity };
        this.Name = name;
        this.OutputColumns = outputColumns;
        this.Condition = condition;

        //this.SelectedColumns = outputColumns.Select(cm => cm.SourceColumn).ToArray(); // TODO:
    }

    public Type JoinType { get; }

    [JsonIgnore]
    public ISourceEntity[] SourceEntities { get; }

    public string Name { get; }

    [JsonIgnore]
    public bool HasDependency => true;

    [JsonIgnore]
    public string[] SelectedColumns => this.OutputColumns.Select(cm => cm.SourceColumn).ToArray();

    public ColumnMapping[] OutputColumns { get; }

    public JoinCondition Condition { get; }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
