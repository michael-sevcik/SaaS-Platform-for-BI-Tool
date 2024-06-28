
using System.Text;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions;
using static BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions.ConditionLink;
using static BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions.JoinCondition;

namespace BIManagement.Modules.DataIntegration.Application.Mapping.SqlViewGenerating;

public class SqlViewVisitor(string tableNamePrefix = "") : IVisitor
{
    private readonly string databasePrefix = tableNamePrefix;

    private readonly StringBuilder sb = new(); // TODO: re usability of the visitor.

    internal string GetSqlView() => sb.ToString();

    public void Visit(Join join)
    {
        sb.Append('(');
        sb.Append("SELECT ");

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
            sb.AppendColumnMapping(outputColumns[i]).Append(", ");
        }

        var lastColumn = outputColumns[^1];
        sb.AppendColumnMapping(lastColumn);

        // Get the tables
        sb.Append(" FROM ");

        // Visit the left source entity
        leftSource.Accept(this);
        sb.Append(' ').Append(leftSource.Name);

        // Append join type
        sb.Append(' ').Append(join.JoinType.ToString().ToUpperInvariant()).Append(" JOIN ");

        // Visit the right entity
        rightSource.Accept(this);
        sb.Append(' ').Append(rightSource.Name);

        // Specify the condition
        sb.Append(" ON ");
        join.Condition.Accept(this);

        sb.Append(')');
    }

    public void Visit(JoinCondition joinCondition)
    {
        // Process left column
        sb.AppendColumnMapping(joinCondition.LeftColumn).Append(' ');

        // Append join condition
        sb.Append(GetCondtionOperatorString(joinCondition.Relation)).Append(' ');

        // Process right column
        sb.AppendColumnMapping(joinCondition.RightColumn);

        // If there is a linked condition, Visit it
        joinCondition.LinkedCondition?.Accept(this);
    }


    public void Visit(ConditionLink link)
    {
        sb.Append(' ')
            .Append(GetConditionLinkRelationString(link.Relation))
            .Append(' ');

        link.Condition.Accept(this);
    }

    public void Visit(SourceTable table)
    {
        sb.Append('(');

        sb.Append("SELECT ");
        sb.AppendJoin(", ", table.SelectedColumns);

        sb.Append(" FROM ");
        sb.Append(databasePrefix).Append(table.Name);

        sb.Append(')');
    }

    public void Visit(EntityMapping entityMapping)
    {
        sb.Append("CREATE VIEW ").Append(databasePrefix).Append(entityMapping.Name);
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
            sb.Append(current.Value.SourceColumn).Append(" AS ").Append(current.Key);
            sb.Append(", ");
        }

        mappingEnumerator.Dispose();
        sb.Append(lastNamedColumnMapping.Value.SourceColumn).Append(" AS ").Append(lastNamedColumnMapping.Key);

        // Add source
        sb.Append(" FROM ");
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
