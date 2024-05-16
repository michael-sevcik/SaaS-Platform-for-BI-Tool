import type { ConditionLink } from "./aggregators/conditions/conditionLink";
import type { JoinCondition } from "./aggregators/conditions/joinCondition";
import type { Join } from "./aggregators/join";
import type { SourceTable } from "./sourceTable";
import { SourceColumn } from "./sourceColumn";


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