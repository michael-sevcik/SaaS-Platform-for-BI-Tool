using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;

public interface IVisitor
{
    void Visit(Join join);

    void Visit(SourceTable table);

    void Visit(JoinCondition joinCondition);

    void Visit(ConditionLink conditionLink);

    void Visit(EntityMapping entityMapping);
}