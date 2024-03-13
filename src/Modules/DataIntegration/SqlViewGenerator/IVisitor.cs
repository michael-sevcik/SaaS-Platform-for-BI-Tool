using SqlViewGenerator.JsonModel;
using SqlViewGenerator.JsonModel.Agregators;
using SqlViewGenerator.JsonModel.Agregators.Conditions;

namespace SqlViewGenerator;

public interface IVisitor
{
    void Visit(MappingConfig config);

    void Visit(Join join);

    void Visit(SourceTable table);

    void Visit(JoinCondition joinCondition);

    void Visit(ConditionLink conditionLink);
    void Visit(EntityMapping entityMapping);
}