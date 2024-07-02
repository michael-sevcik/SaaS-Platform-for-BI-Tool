
using System.Text;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions;
using static BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions.ConditionLink;
using static BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions.JoinCondition;

namespace BIManagement.Modules.DataIntegration.Application.Mapping.SqlViewGenerating;

//  TODO: REMOVE ALL AppendLine() calls

public class SqlViewVisitor(string tableNamePrefix = "") : IVisitor
{
    private readonly string databasePrefix = tableNamePrefix;

    private readonly StringBuilder sb = new();

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
            sb.AppendChildColumnReference(GetJoinColumnSourceEntity(join, column), column).Append(',').AppendLine();
        }

        var lastColumn = outputColumns[^1];
        sb.AppendChildColumnReference(GetJoinColumnSourceEntity(join, lastColumn), lastColumn);

        // Get the tables
        sb.AppendLine().Append("FROM ");

        // Visit the left source entity
        leftSource.Accept(this);
        sb.Append(' ').Append(leftSource.Name);

        // Append join type
        sb.AppendLine().Append(join.JoinType.ToString().ToUpperInvariant()).Append(" JOIN ");

        // Visit the right entity
        rightSource.Accept(this);
        sb.Append(' ').Append(rightSource.Name).AppendLine();

        // Specify the condition
        sb.Append("ON ");
        ProcessJoinCondition(join.JoinCondition, join); 

        sb.Append(')');
    }

    public void ProcessConditionLink(Join join, ConditionLink link)
    {
        sb.Append(' ')
            .Append(GetConditionLinkRelationString(link.Relation))
            .Append(' ');

        ProcessJoinCondition(link.Condition, join);
    }

    private StringBuilder ProcessJoinCondition(JoinCondition joinCondition, Join join)
    {
        sb.AppendChildColumnReference(GetJoinColumnSourceEntity(join, joinCondition.LeftColumn), joinCondition.LeftColumn);

        // Append join condition
        sb.Append(GetCondtionOperatorString(joinCondition.Relation)).Append(' ');

        // Process right column
        sb.AppendChildColumnReference(GetJoinColumnSourceEntity(join, joinCondition.RightColumn), joinCondition.RightColumn);

        // If there is a linked condition, Visit it
        if (joinCondition.LinkedCondition is not null)
        {
            ProcessConditionLink(join, joinCondition.LinkedCondition);
        }

        return sb;
    }

    public void Visit(JoinCondition joinCondition)
    {
        // TODO: REMOVE
        throw new NotImplementedException();
    }


    public void Visit(ConditionLink link)
    {
        // TODO: REMOVE
        throw new NotImplementedException();
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
        if (entityMapping.SourceEntity is null)
        {
            throw new InvalidOperationException("EntityMapping must have a source entity.");
        }

        sb.Append("CREATE VIEW ").Append(databasePrefix).Append(entityMapping.Name).AppendLine();
        sb.Append("AS SELECT ");

        foreach (var (targetColumnName, sourceColumn) in entityMapping.ColumnMappings)
        {
            AppendTargetColumnMapping(entityMapping, targetColumnName, sourceColumn);
            sb.Append(", ");
        }

        // Remove last comma and space
        if (entityMapping.ColumnMappings.Any())
        {
            sb.Remove(sb.Length - 2, 2);
        }

        // Add source
        sb.AppendLine().Append("FROM ");
        entityMapping.SourceEntity!.Accept(this);
        sb.Append(' ').Append(entityMapping.SourceEntity.Name);
    }

    private void AppendTargetColumnMapping(EntityMapping entityMapping, string targetColumnName, SourceColumn? sourceColumn)
    {
        if (sourceColumn != null)
        {
            sb.AppendChildColumnReferenceWithRenaming(entityMapping.SourceEntity!, sourceColumn, targetColumnName);
        }
        else
        {
            sb.Append("NULL AS ").Append(targetColumnName);
        }
    }

    public void Visit(CustomQuery customQuery)
    {
        sb.Append('(');

        sb.Append("SELECT ");
        sb.AppendSelectedColumnsWithRenaming(customQuery.SelectedColumns);

        sb.Append(" FROM ");
        sb.Append('(');
        sb.Append(customQuery.Query);
        sb.Append(") ").Append(customQuery.Name);

        sb.Append(" )");
    }

    private static string GetConditionLinkRelationString(LinkRelation relation)
        => relation switch
        {
            LinkRelation.And => "AND",
            LinkRelation.Or => "OR",
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
