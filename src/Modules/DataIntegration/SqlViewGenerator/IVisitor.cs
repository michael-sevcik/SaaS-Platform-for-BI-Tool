using BIManagement.Modules.DataIntegration.SqlViewGenerator.JsonModel;
using BIManagement.Modules.DataIntegration.SqlViewGenerator.JsonModel.Agregators;
using BIManagement.Modules.DataIntegration.SqlViewGenerator.JsonModel.Agregators.Conditions;

namespace BIManagement.Modules.DataIntegration.SqlViewGenerator;

public interface IVisitor
{
    void Visit(MappingConfig config);

    void Visit(Join join);

    void Visit(SourceTable table);

    void Visit(JoinCondition joinCondition);

    void Visit(ConditionLink conditionLink);
    void Visit(EntityMapping entityMapping);
}