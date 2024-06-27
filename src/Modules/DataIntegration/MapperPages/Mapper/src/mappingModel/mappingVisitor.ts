import type { ConditionLink } from "./sourceEntities/aggregators/conditions/conditionLink";
import type { JoinCondition } from "./sourceEntities/aggregators/conditions/joinCondition";
import type { SourceColumn } from "./sourceEntities/sourceColumn";
import { SourceEntityVisitor } from "./sourceEntityVisitor";


/**
 * Abstract class for visitor of the mapping entities.
 * The visit method's parameters are not typed because of the circular dependency.
 */
export abstract class MappingVisitor extends SourceEntityVisitor {
    public abstract visitConditionLink(conditionLink: ConditionLink): void;
    public abstract visitJoinCondition(joinCondition: JoinCondition): void;
    public abstract visitSourceColumn(sourceColumn: SourceColumn): void;
}