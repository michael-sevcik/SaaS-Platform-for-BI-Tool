import { MappingVisitor } from "../mappingVisitor";

/**
 * Represents an entity that can be visited by a mapping visitor.
 */
export interface Visitable {
    /**
     * Accepts a visitor for visiting the entity.
     * @param visitor The visitor to accept.
     */
    accept(visitor: MappingVisitor): void;
}