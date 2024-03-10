using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlViewGenerator.JsonModel.Agregators.Conditions;

public class JoinCondition : IVisitable
{
    public enum Operator
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
    }

    public JoinCondition(
        Operator relation,
        ColumnMapping leftColumn,
        ColumnMapping rightColumn,
        ConditionLink? linkedCondition = null)
    {
        this.Relation = relation;
        this.LeftColumn = leftColumn;
        this.RightColumn = rightColumn;
        this.LinkedCondition = linkedCondition;
    }

    public Operator Relation { get; }

    public ColumnMapping LeftColumn { get; }

    public ColumnMapping RightColumn { get; }

    public ConditionLink? LinkedCondition { get; }

    /// <inheritdoc/>
    public void Accept(IVisitor visitor)
        => visitor.Visit(this);
}
