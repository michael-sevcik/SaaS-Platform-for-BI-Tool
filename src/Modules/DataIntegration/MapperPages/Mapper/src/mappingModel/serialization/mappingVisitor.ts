import type { ConditionLink } from "../Agregators/Conditions/conditionLink";
import type { JoinCondition } from "../Agregators/Conditions/joinCondition";
import type { Join } from "../Agregators/join";
import type { SourceTable } from "../sourceTable";
import { SourceColumn } from "../sourceColumn";


/**
 * Abstract class for visitor of the mapping entities.
 * The visit method's parameters are not typed because of the circular dependency.
 */
export abstract class MappingVisitor {
    public abstract visitConditionLink(conditionLink: ConditionLink): void;
    public abstract visitJoin(join: Join): void;
    public abstract visitJoinCondition(joinCondition: JoinCondition): void;
    public abstract visitSourceColumn(sourceColumn: SourceColumn): void;
    public abstract visitSourceTable(sourceTable: SourceTable): void;
}