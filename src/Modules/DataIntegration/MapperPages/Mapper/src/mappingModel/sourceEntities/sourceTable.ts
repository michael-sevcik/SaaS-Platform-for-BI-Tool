import { MappingVisitor } from "../mappingVisitor";
import { SourceEntityBase } from "./sourceEntityBase";
import { SourceColumn } from "../sourceColumn";
import { Column } from "../../dbModel/database";

export class SourceTable extends SourceEntityBase {
    public static readonly typeDescriptor = 'sourceTable';

    /** @inheritdoc */
    public get fullName(): string {
        return this.schema ? `${this.schema}.${this.name}` : this.name;
    }

    /** @inheritdoc */
    public removeReferences(): void {
        // no references to remove
    }
    
    /** @inheritdoc */
    replaceChild(oldChild: any, newChild: any): void {
    }
    
    /** @inheritdoc */
    public createBackwardConnections(): void {
        for (const column of this._selectedColumns) {
            column.owner = this;
        }
    }

    public addSelectedColumn(column: Column) : SourceColumn {
        const sourceColumn = new SourceColumn(column);
        sourceColumn.owner = this;
        this._selectedColumns.push(sourceColumn);
        return sourceColumn;
    }

    public removeSelectedColumn(column: Column) {
        const sourceColumn = new SourceColumn(column);
        const index = this._selectedColumns.findIndex((value) => value.name === sourceColumn.name);
        if (index === -1) {
            throw new Error(`Column ${sourceColumn.name} is not selected in source table ${this.name}`);
        }

        this._selectedColumns.splice(index, 1);
    } 

    public constructor(name: string,
        public readonly schema: string | null,
        selectedColumns: SourceColumn[] = [],
        public readonly description: string | null = null) {
        super(name);
        this._selectedColumns = selectedColumns;
        this.createBackwardConnections();
    }

    /** @inheritdoc */
    public accept(visitor: MappingVisitor): void {
        visitor.visitSourceTable(this);  
    }
}
