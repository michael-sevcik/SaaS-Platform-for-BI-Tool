import type { ConditionLink } from "./sourceEntities/aggregators/conditions/conditionLink";
import type { JoinCondition } from "./sourceEntities/aggregators/conditions/joinCondition";
import type { Join } from "./sourceEntities/aggregators/join";
import type { SourceTable } from "./sourceEntities/sourceTable";
import type { SourceColumn } from "./sourceEntities/sourceColumn";
import type { CustomQuery } from "./sourceEntities/customQuery";


/**
 * Abstract class for visitor of the source entities.
 */
export abstract class SourceEntityVisitor {
    public abstract visitJoin(join: Join): void;
    public abstract visitSourceTable(sourceTable: SourceTable): void;
    public abstract visitCustomQuery(customQuery: CustomQuery): void;
}