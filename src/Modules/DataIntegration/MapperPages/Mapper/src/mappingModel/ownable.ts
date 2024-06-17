import { Owner } from "./owner";

export interface Ownable {
    /**
     * Sets the owner of this entity for propagating changes.
     */
    set owner(owner : Owner)
    
    /**
     * Gets the owner of this entity for propagating changes.
     * @returns The owner entity or throws error.
     */
    get owner() : Owner;
    
}