/**
 * Interface for entities that holds references to other entities.
 */
export interface ReferenceHolder {
    /**
    * Creates backward connections from source entities to their owners in the source tree of this entity.
    * Sets the ownerships and registers references to sourceColumns 
    */
    createBackwardConnections(): void;

    /**
     * Unregisters all references that the entity and its children hold. Used when the entity is removed from the mapping.
     */
    removeReferences(): void;
}