import { Type } from "class-transformer";

import { SourceEntity } from "../sourceEntity";
import { SourceConcreteEntity } from "../sourceConcreteEntity";
import { MappingVisitor } from "../serialization/mappingVisitor";
import { JoinCondition } from "./Conditions/joinCondition";
import { SourceColumn } from "../sourceColumn";

export class Join extends SourceEntity{
    replaceChild(oldChild: any, newChild: any): void {
        if (oldChild === this.leftSourceEntity) {
            this.leftSourceEntity = newChild;
        } else if (oldChild === this.rightSourceEntity) {
            this.rightSourceEntity = newChild;
        }
    }
    public createBackwardConnections(): void {
        this.leftSourceEntity.owner = this;
        this.rightSourceEntity.owner = this;
        this.leftSourceEntity.createBackwardConnections();
        this.rightSourceEntity.createBackwardConnections();
        this.condition.createBackwardConnections();
    }
    

    public get selectedColumns() : SourceColumn[] {
        return this.leftSourceEntity.selectedColumns.concat(this.rightSourceEntity.selectedColumns);
    }
/**
 * Creates an instance of join.
 * @param type type of the join
 * @param leftSourceEntity the left source entity - either a source table or a join
 * @param rightSourceEntity the right concrete source entity
 * @param name name of the join
 * @param outputColumns the columns that are selected from the join
 * @param condition the join condition
 */
public constructor(
        name : string,
        public type : JoinType,
        public leftSourceEntity : SourceEntity,
        public rightSourceEntity : SourceConcreteEntity,
        public condition : JoinCondition | null = null) {
        super(name);
    }

    public accept(visitor: MappingVisitor): void {
      visitor.visitJoin(this);
    }

    public isInitialized() : boolean {
        return this.condition !== null;
    }
}

export enum JoinType {
    left = "left",
    natural = "natural",
    inner = "inner"
} 