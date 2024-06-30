using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators;


/// <summary>
/// Represents a join aggregator in the JSON model of source entities.
/// </summary>
public class Join(
    Join.Type joinType,
    ISourceEntity leftSourceEntity,
    ISourceEntity rightSourceEntity,
    string name,
    SourceColumn[] selectedColumns,
    JoinCondition condition) : IAgregator
{
    public const string TypeDiscriminator = "join";

    /// <summary>
    /// The type of the join.
    /// </summary>
    public enum Type
    {
        left,
        right,
        inner,
        full,
    }

    /// <summary>
    /// Gets the left source entity of the join.
    /// </summary>
    public ISourceEntity LeftSourceEntity => SourceEntities[0];

    /// <summary>
    /// Gets the right source entity of the join.
    /// </summary>
    public ISourceEntity RightSourceEntity => SourceEntities[1];

    /// <summary>
    /// Gets the type of the join.
    /// </summary>
    public Type JoinType { get; } = joinType;

    /// <inheritdoc/>
    public string Name { get; } = name;

    /// <inheritdoc/>
    [JsonIgnore]
    public ISourceEntity[] SourceEntities { get; } = [leftSourceEntity, rightSourceEntity];

    /// <inheritdoc/>
    [JsonIgnore]
    public bool HasDependency => true;

    /// <inheritdoc/>
    public SourceColumn[] SelectedColumns = selectedColumns;

    /// <summary>
    /// Gets the condition for the join.
    /// </summary>
    public JoinCondition Condition { get; } = condition;

    SourceColumn[] ISourceEntity.SelectedColumns => throw new NotImplementedException();

    /// <inheritdoc/>
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    /// <inheritdoc/>
    public void AssignColumnOwnership()
    {
        foreach (var child in SourceEntities)
        {
            child.AssignColumnOwnership();
        }
    }
}
