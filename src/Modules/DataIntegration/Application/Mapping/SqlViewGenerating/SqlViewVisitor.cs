
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

    public string GetSqlView() => sb.ToString();

    private ISourceEntity GetJoinColumnSourceEntity(Join join, SourceColumn column)
    {
        if (join.LeftSourceEntity.SelectedColumns.Contains(column))
        {
            return join.LeftSourceEntity;
        }
        else if (join.RightSourceEntity.SelectedColumns.Contains(column))
        {
            return join.RightSourceEntity;
        }
        else
        {
            throw new InvalidOperationException("The column does not belong to any of the source entities.");
        }
    }
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

        var outputColumns = join.SelectedColumns;
        if (outputColumns.Length < 1)
        {
            throw new NotSupportedException("Join entity must have at least 1 output column.");
        }

        for (int i = 0; i < outputColumns.Length - 1; ++i)
        {
            var column = outputColumns[i];
            sb.AppendChildColumnReference(GetJoinColumnSourceEntity(join, column), column).Append(", ");
        }

        var lastColumn = outputColumns[^1];
        // TODO: use column references
        sb.AppendSourceColumn(lastColumn);

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
        join.JoinCondition.Accept(this);

        sb.Append(')');
    }

    public void Visit(JoinCondition joinCondition)
    {
        // TODO: use refences or maybe move this to the join visitation
        // Process left column
        sb.AppendSourceColumn(joinCondition.LeftColumn).Append(' ');

        // Append join condition
        sb.Append(GetCondtionOperatorString(joinCondition.Relation)).Append(' ');

        // Process right column
        sb.AppendSourceColumn(joinCondition.RightColumn);

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
        sb.AppendSelectedColumnsWithRenaming(table.SelectedColumns);

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

        // TODO: Identical names of columns coming from the joins need to be handled. - should be handled with the full name convention.
        var lastNamedColumnMapping = mappingEnumerator.Current;
        while (mappingEnumerator.MoveNext())
        {
            // Save the last mapping
            var current = lastNamedColumnMapping;
            lastNamedColumnMapping = mappingEnumerator.Current;

            // Append the column and its name
            // todo:
            //sb.Append(current.Value.SourceColumn).Append(" AS ").Append(current.Key);
            sb.Append(", ");
        }

        mappingEnumerator.Dispose();
        // TODO:
        sb.Append(lastNamedColumnMapping.Value.Name).Append(" AS ").Append(lastNamedColumnMapping.Key);

        // Add source
        sb.Append(" FROM ");
        // TODO:
        entityMapping.SourceEntity!.Accept(this);
    }

    public void Visit(CustomQuery customQuery)
    {
        // TODO:
        throw new NotImplementedException();
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
            Operator.lessThan => "<=",
            Operator.equals => "=",
            Operator.notEquals => "!=",
            Operator.greaterThan => ">",
            _ => throw new NotImplementedException()
        };
}
