import type { Table } from "../../dbModel/database";
import type { Join } from "../../mappingModel/aggregators/join";
import type { SourceTable } from "../../mappingModel/sourceEntities/sourceTable";
import type { JoinModal } from "../modals/joinModal";
import type { BaseSourceEntityShape } from "../shapes/baseSourceEntityShape";

export type SourceEntityShapeFactory = () => BaseSourceEntityShape;

/**
 * Helper class for encapsulating objects needed to finish joining process
 * that starts after an addition of a new source entity.
 */
export class JoinedSourceEntityData {

    /**
     * Initializes a new instance o of {@link JoinedSourceEntityData}.
     * @param sourceEntityShapeFactory The table that is being added and needs to be joined.
     * @param tableModel The model of table that is being added.
     *  Used for construction of {@link SourceTableShape}.
     * @param joinModal the modal responsible for joining the new table.
     * @param unfinishedJoin Entity representing the join between old table and newly added.
     */
    public constructor(
        public readonly sourceEntityShapeFactory: SourceEntityShapeFactory,
        public readonly joinModal: JoinModal,
        public readonly unfinishedJoin: Join) { }
}