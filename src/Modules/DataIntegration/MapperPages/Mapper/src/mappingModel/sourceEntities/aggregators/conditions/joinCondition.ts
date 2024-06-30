import { ReferenceHolder } from "../../../referenceHolder";
import { MappingVisitor } from "../../../mappingVisitor";
import { MappingVisitable } from "../../../converting/mappingVisitable";
import { SourceColumn } from "../../sourceColumn";
import type { ConditionLink } from "./conditionLink";

export enum Operator {
    equals = "equals",
    notEquals = "notEquals",
    greaterThan = "greaterThan",
    lessThan = "lessThan",
}

export class JoinCondition implements MappingVisitable, ReferenceHolder { 
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
        public linkedCondition : ConditionLink | null = null) {
        this._leftColumn = leftColumn;
        this._rightColumn = rightColumn;
    }
    removeReferences(): void {
        this.leftColumn.removeReference(this);
        this.rightColumn.removeReference(this);
        this.linkedCondition?.removeReferences();
    }

    accept(visitor: MappingVisitor): void {
        visitor.visitJoinCondition(this);
    }

    createBackwardConnections(): void {
        this._leftColumn.addReference(this);
        this._rightColumn.addReference(this);
        this.linkedCondition?.createBackwardConnections();
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