// <copyright file="SqlViewVisitor.cs" company="Merica s.r.o.">
// Copyright © Merica
// </copyright>

namespace SqlViewGenerator.SqlViewGenerating;

using System.Text;
using JsonModel;
using JsonModel.Agregators;
using JsonModel.Agregators.Conditions;
using static JsonModel.Agregators.Conditions.ConditionLink;
using static JsonModel.Agregators.Conditions.JoinCondition;

public class SqlViewVisitor : IVisitor
{
    private readonly StringBuilder sb = new(); // TODO: re usability of the visitor.

    private readonly string databasePrefix;

    public SqlViewVisitor(string tableNamePrefix = "")
        => this.databasePrefix = tableNamePrefix;

    internal string GetSqlView() => sb.ToString();

    public void Visit(Join join)
    {
        this.sb.Append('(');
        this.sb.Append("SELECT ");

        var leftSource = join.LeftSourceEntity;
        var rightSource = join.RightSourceEntity;

        // ENTITIES MUST HAVE UNIQUE NAMES - selecting columns
        if (leftSource.Name == rightSource.Name)
        {
            throw new NotSupportedException("Identical names of the join source entities are not supported.");
        }

        var outputColumns = join.OutputColumns;
        if (outputColumns.Length < 1)
        {
            throw new NotSupportedException("Join entity must at least 1 output column.");
        }

        for (int i = 0; i < outputColumns.Length - 1; ++i)
        {
            this.sb.AppendColumnMapping(outputColumns[i]).Append(", ");
        }

        var lastColumn = outputColumns[^1];
        this.sb.AppendColumnMapping(lastColumn);

        // Get the tables
        this.sb.Append(" FROM ");

        // Visit the left source entity
        leftSource.Accept(this);
        this.sb.Append(' ').Append(leftSource.Name);

        // Append join type
        this.sb.Append(' ').Append(join.JoinType.ToString().ToUpperInvariant()).Append(" JOIN ");

        // Visit the right entity
        rightSource.Accept(this);
        this.sb.Append(' ').Append(rightSource.Name);

        // Specify the condition
        this.sb.Append(" ON ");
        join.Condition.Accept(this);

        this.sb.Append(')');
    }

    public void Visit(JoinCondition joinCondition)
    {
        // Process left column
        this.sb.AppendColumnMapping(joinCondition.LeftColumn).Append(' ');

        // Append join condition
        this.sb.Append(GetCondtionOperatorString(joinCondition.Relation)).Append(' ');

        // Process right column
        this.sb.AppendColumnMapping(joinCondition.RightColumn);

        // If there is a linked condition, Visit it
        joinCondition.LinkedCondition?.Accept(this);
    }


    public void Visit(ConditionLink link)
    {
        this.sb.Append(' ')
            .Append(GetConditionLinkRelationString(link.Relation))
            .Append(' ');

        link.Condition.Accept(this);
    }

    public void Visit(SourceTable table)
    {
        this.sb.Append('(');

        this.sb.Append("SELECT ");
        this.sb.AppendJoin(", ", table.SelectedColumns);

        this.sb.Append(" FROM ");
        this.sb.Append(this.databasePrefix).Append(table.Name);

        this.sb.Append(')');
    }

    /// <summary>
    /// Visit starting point.
    /// </summary>
    /// <param name="config"></param>
    public void Visit(MappingConfig config)
    {
        throw new NotImplementedException();
    }

    public void Visit(EntityMapping entityMapping)
    {
        sb.Append("CREATE VIEW ").Append(this.databasePrefix).Append(entityMapping.Name);
        sb.Append(" AS SELECT ");

        var mappingEnumerator = entityMapping.ColumnMappings.GetEnumerator();
        if (!mappingEnumerator.MoveNext())
        {
            throw new NotSupportedException("EntityMappings must have at least 1 column.");
        }

        // TODO: Identical names of columns coming from the joins need to be handled.
        var lastNamedColumnMapping = mappingEnumerator.Current;
        while (mappingEnumerator.MoveNext())
        {
            // Save the last mapping
            var current = lastNamedColumnMapping;
            lastNamedColumnMapping = mappingEnumerator.Current;

            // Append the column and its name
            this.sb.Append(current.Value.SourceColumn).Append(" AS ").Append(current.Key);
            this.sb.Append(", ");
        }

        mappingEnumerator.Dispose();
        this.sb.Append(lastNamedColumnMapping.Value.SourceColumn).Append(" AS ").Append(lastNamedColumnMapping.Key);

        // Add source
        this.sb.Append(" FROM ");
        entityMapping.SourceEntity.Accept(this);
    }

    private static string GetConditionLinkRelationString(LinkRelation relation)
        => relation switch
        {
            LinkRelation.And => "and", // TODO: CHECK
            LinkRelation.Or => "or",
            _ => throw new NotImplementedException(),
        };

    private static string GetCondtionOperatorString(Operator oper)
        => oper switch
        {
            Operator.LessThan => "<=",
            Operator.Equal => "=",
            Operator.NotEqual => "!=",
            Operator.GreaterThan => ">",
            _ => throw new NotImplementedException()
        };
}
