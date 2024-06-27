import type { Column } from "../../dbModel/database";
import type { SourceColumn } from "../../mappingModel/sourceEntities/sourceColumn";
import { PropertyPort } from "./propertyPort";

export interface TargetElementShape {
    /**
     * Sets the column mapping of the entity mapping.
     * @param sourceColumn Source column corresponding to the source port of the property link
     * @param targetColumn Column corresponding to the target port of the property link - the port must belong to this entity
     */
    setColumnMapping(sourceColumn: SourceColumn, targetColumn: Column) : void;

    /**
     * Removes the column mapping of the entity mapping.
     * @param targetColumn Column corresponding to the target port of the property link - the port must belong to this entity
     */
    removeColumnMapping(targetColumn: Column) : void;

    /**
     * Returns the column corresponding to the given port.
     * @param portId ID of the port
     * @returns Column corresponding to the port
     */
    getColumnByPortId(portId: string): Column;
}