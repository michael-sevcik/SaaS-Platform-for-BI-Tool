﻿namespace SqlViewGenerator.JsonModel.Agregators.Conditions;

public class ConditionLink : IVisitable
{
    public enum LinkRelation
    {
        And,
        Or,
    }

    public ConditionLink(LinkRelation relation, JoinCondition condition)
    {
        this.Relation = relation;
        this.Condition = condition;
    }

    public LinkRelation Relation { get; }

    public JoinCondition Condition { get; }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}