using BIManagement.Modules.DataIntegration.Domain.JsonModel.SourceEntities;
using BIManagement.Modules.DataIntegration.Domain.JsonModel.SourceEntities.Agregators;
using BIManagement.Modules.DataIntegration.Domain.JsonModel.SourceEntities.Agregators.Conditions;

namespace BIManagement.Modules.DataIntegration.Domain.JsonModel;

public interface IVisitor
{
    void Visit(Join join);

    void Visit(SourceTable table);

    void Visit(JoinCondition joinCondition);

    void Visit(ConditionLink conditionLink);

    void Visit(EntityMapping entityMapping);
}