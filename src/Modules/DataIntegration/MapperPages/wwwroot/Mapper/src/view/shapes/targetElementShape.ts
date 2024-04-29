import { PropertyPort } from "./propertyPort";

export interface TargetElementShape {
    /**
     * Sets the column mapping of the entity mapping.
     * @param sourcePort Source port of the property link
     * @param targetPort Target port of the property link - the port must belong to this entity
     */
    setColumnMapping(sourcePort: PropertyPort, targetPort: PropertyPort);

    /**
     * Removes the column mapping of the entity mapping.
     * @param targetPort Target port of the property link - the port must belong to this entity
     */
    removeColumnMapping(targetPort: PropertyPort);
}