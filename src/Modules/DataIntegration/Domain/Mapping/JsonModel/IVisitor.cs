using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;

public interface IVisitor
{
    /// <summary>
    /// Visits a join entity.
    /// </summary>
    /// <param name="join">The join entity to visit.</param>
    void Visit(Join join);

    /// <summary>
    /// Visits a source table entity.
    /// </summary>
    /// <param name="table">The source table entity to visit.</param>
    void Visit(SourceTable table);

    /// <summary>
    /// Visits a custom query entity.
    /// </summary>
    /// <param name="customQuery">The custom query entity to visit.</param>
    void Visit(CustomQuery customQuery);

    /// <summary>
    /// Visits a join condition entity.
    /// </summary>
    /// <param name="joinCondition">The join condition entity to visit.</param>
    void Visit(JoinCondition joinCondition);

    /// <summary>
    /// Visits a condition link entity.
    /// </summary>
    /// <param name="conditionLink">The condition link entity to visit.</param>
    void Visit(ConditionLink conditionLink);

    /// <summary>
    /// Visits an entity mapping entity.
    /// </summary>
    /// <param name="entityMapping">The entity mapping entity to visit.</param>
    void Visit(EntityMapping entityMapping);
}
