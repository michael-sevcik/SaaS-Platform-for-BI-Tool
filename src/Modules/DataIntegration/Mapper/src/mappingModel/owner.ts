import { ReferenceHolder } from "./referenceHolder";
import type { Ownable } from "./ownable";

export interface Owner extends ReferenceHolder{
    /**
     * Replaces the old child with the new child.
     * @param oldChild The child to be replaced - caller
     * @param newChild The new child - null if there is no child
     */
    replaceChild(oldChild: Ownable, newChild: Ownable | null): void;
}