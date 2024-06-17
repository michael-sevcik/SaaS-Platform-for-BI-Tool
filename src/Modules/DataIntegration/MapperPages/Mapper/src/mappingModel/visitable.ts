import type { SourceEntityVisitor } from "./sourceEntityVisitor";

export interface VisitableSourceEntity {
    /**
     * Accepts a visitor for visiting the entity.
     * @param visitor The visitor to accept.
     */
    accept(visitor: SourceEntityVisitor): void;
}