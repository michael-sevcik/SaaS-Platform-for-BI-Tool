using BIManagementPlatform.Modules.DataIntegration.SqlViewGenerator.JsonModel;
using BIManagementPlatform.Modules.DataIntegration.SqlViewGenerator.JsonModel.Agregators;
using BIManagementPlatform.Modules.DataIntegration.SqlViewGenerator.JsonModel.Agregators.Conditions;

namespace BIManagementPlatform.Modules.DataIntegration.SqlViewGenerator;

public interface IVisitor
{
    void Visit(MappingConfig config);

    void Visit(Join join);

    void Visit(SourceTable table);

    void Visit(JoinCondition joinCondition);

    void Visit(ConditionLink conditionLink);
    void Visit(EntityMapping entityMapping);
}