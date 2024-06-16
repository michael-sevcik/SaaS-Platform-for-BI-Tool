import type { ConditionLink } from "./aggregators/conditions/conditionLink";
import type { JoinCondition } from "./aggregators/conditions/joinCondition";
import type { Join } from "./aggregators/join";
import type { SourceTable } from "./sourceEntities/sourceTable";
import type { SourceColumn } from "./sourceColumn";
import type { CustomQuery } from "./sourceEntities/customQuery";


/**
 * Abstract class for visitor of the source entities.
 */
export abstract class SourceEntityVisitor {
    public abstract visitJoin(join: Join): void;
    public abstract visitSourceTable(sourceTable: SourceTable): void;
    public abstract visitCustomQuery(customQuery: CustomQuery): void;
}