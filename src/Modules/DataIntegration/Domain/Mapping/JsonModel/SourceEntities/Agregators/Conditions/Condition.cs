namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions;

public class JoinCondition : IVisitable
{
    public enum Operator
    {
        equals,
        notEquals,
        greaterThan,
        lessThan,
    }

    public JoinCondition(
        Operator relation,
        SourceColumn leftColumn,
        SourceColumn rightColumn,
        ConditionLink? linkedCondition = null)
    {
        Relation = relation;
        LeftColumn = leftColumn;
        RightColumn = rightColumn;
        LinkedCondition = linkedCondition;
    }

    public Operator Relation { get; }

    public SourceColumn LeftColumn { get; }

    public SourceColumn RightColumn { get; }

    public ConditionLink? LinkedCondition { get; }

    /// <inheritdoc/>
    public void Accept(IVisitor visitor)
        => visitor.Visit(this);
}
