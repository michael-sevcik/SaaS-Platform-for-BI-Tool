import { ReferenceHolder } from "../../referenceHolder";
import { MappingVisitor } from "../../mappingVisitor";
import { Visitable } from "../../converting/visitable";
import type { JoinCondition } from "./joinCondition";

export enum LinkRelation {
    and = "and",
    or = "or"
}

export class ConditionLink implements Visitable, ReferenceHolder {
    /**
     * Inializes a new instance of ConditionLink
     * @param relation The relation between the conditions.
     * @param condition The next condition in the chain.
     * Should be of type JoinCondition, but we can't use it because of the circular dependency.
     */
    public constructor(
        public relation : LinkRelation,
        public condition : JoinCondition) {
    }
    removeReferences(): void {
        this.condition.removeReferences();
    }

    accept(visitor: MappingVisitor): void {
        visitor.visitConditionLink(this);
    }

    createBackwardConnections(): void {
        this.condition.createBackwardConnections();
    }
}