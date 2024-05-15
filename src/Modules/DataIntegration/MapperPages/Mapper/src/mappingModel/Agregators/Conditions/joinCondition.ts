import { ReferenceHolder } from "../../referenceHolder";
import { MappingVisitor } from "../../serialization/mappingVisitor";
import { Visitable } from "../../serialization/visitable";
import { SourceColumn } from "../../sourceColumn";
import type { ConditionLink } from "./conditionLink";

export enum Operator {
    equals = "equals",
    notEquals = "notEquals",
    greaterThan = "greaterThan",
    lessThan = "lessThan",
}

export class JoinCondition implements Visitable, ReferenceHolder { 
    private _leftColumn : SourceColumn;
    private _rightColumn : SourceColumn;

    public get leftColumn() : SourceColumn {
        return this._leftColumn;
    }

    public get rightColumn() : SourceColumn {
        return this._rightColumn;
    }

    public constructor(
        public relation : Operator,
        leftColumn : SourceColumn,
        rightColumn : SourceColumn,
        public linkedCondition? : ConditionLink) {
        this._leftColumn = leftColumn;
        this._rightColumn = rightColumn;
    }

    accept(visitor: MappingVisitor): void {
        visitor.visitJoinCondition(this);
    }

    createBackwardConnections(): void {
        this._leftColumn.addReference(this);
        this._rightColumn.addReference(this);
        if (this.linkedCondition !== undefined) {
            this.linkedCondition.createBackwardConnections();
        }
    }

    public set leftColumn(leftColumn : SourceColumn) {
        this._leftColumn.removeReference(this);
        this._leftColumn = leftColumn;
        leftColumn.addReference(this);
    }

    public set rightColumn(rightColumn : SourceColumn) {
        this._rightColumn.removeReference(this);
        this._rightColumn = rightColumn;
        rightColumn.addReference(this);
    }
}