import { Owner } from "./owner";

export interface Ownable {
    /**
     * Sets the owner of this entity for propagating changes.
     */
    set owner(owner : Owner)
    
    /**
     * Gets the owner of this entity for propagating changes.
     */
    get owner() : Owner | null;
    
}