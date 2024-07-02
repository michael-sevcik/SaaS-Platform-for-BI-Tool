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
public class Join : IAgregator
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
    public ISourceEntity LeftSourceEntity { get; set; }

    /// <summary>
    /// Gets the right source entity of the join.
    /// </summary>
    public ISourceEntity RightSourceEntity { get; set; }

    /// <summary>
    /// Gets the type of the join.
    /// </summary>
    public Type JoinType { get; set; }

    /// <inheritdoc/>
    public string Name { get; set; }

    /// <inheritdoc/>
    [JsonIgnore]
    public ISourceEntity[] SourceEntities { get => [LeftSourceEntity, RightSourceEntity]; }

    /// <inheritdoc/>
    [JsonIgnore]
    public bool HasDependency => true;

    /// <inheritdoc/>
    public SourceColumn[] SelectedColumns { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Join"/> class.
    /// </summary>
    /// <remarks>Meant for Deserialization purposes.</remarks>
    public Join()
    {
        this.Name = "";
        this.LeftSourceEntity = null!;
        this.RightSourceEntity = null!;
        this.SelectedColumns = Array.Empty<SourceColumn>();
        this.JoinCondition = null!;
    }

    public Join(
        Type joinType,
        ISourceEntity leftSourceEntity,
        ISourceEntity rightSourceEntity,
        string name,
        SourceColumn[] selectedColumns,
        JoinCondition condition)
    {
        JoinType = joinType;
        Name = name;
        this.LeftSourceEntity = leftSourceEntity;
        this.RightSourceEntity = rightSourceEntity;
        SelectedColumns = selectedColumns;
        JoinCondition = condition;
    }

    /// <summary>
    /// Gets the condition for the join.
    /// </summary>
    public JoinCondition JoinCondition { get; set; }


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
