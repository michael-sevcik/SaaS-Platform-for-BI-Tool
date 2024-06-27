import type { Join } from "../../mappingModel/sourceEntities/aggregators/join";
import type { CustomQuery } from "../../mappingModel/sourceEntities/customQuery";
import type { SourceEntityBase } from "../../mappingModel/sourceEntities/sourceEntityBase";
import type { SourceTable } from "../../mappingModel/sourceEntities/sourceTable";
import { SourceEntityVisitor } from "../../mappingModel/sourceEntityVisitor";

/**
 * Helper {@link MappingVisitor} derived class for finding the rightmost 
 * instance of {@link SourceEntityBase} in the joining tree.
 */
export class ConcreteSourceEntityFinder extends SourceEntityVisitor {
    public desiredSourceEntity: SourceEntityBase | null;

    /**
     * Visits a custom query source entity.
     * @param customQuery The custom query source entity to visit.
     */
    public visitCustomQuery(customQuery: CustomQuery): void {
        this.desiredSourceEntity = customQuery;
    }

    /**
     * Visits a source table source entity.
     * @param sourceTable The source table source entity to visit.
     */
    public visitSourceTable(sourceTable: SourceTable): void {
        this.desiredSourceEntity = sourceTable;
    }

    /**
     * Visits a join source entity.
     * @param join The join source entity to visit.
     */
    public visitJoin(join: Join): void {
        join.rightSourceEntity.accept(this);
    }
}