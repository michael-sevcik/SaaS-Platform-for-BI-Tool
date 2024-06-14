import { MappingVisitor } from "./mappingVisitor";
import { SourceEntityBase } from "./sourceEntityBase";
import { SourceColumn } from "./sourceColumn";
import { Column } from "../dbModel/database";

export class SourceTable extends SourceEntityBase {
    replaceChild(oldChild: any, newChild: any): void {
    }
    
    public createBackwardConnections(): void {
        for (const column of this._selectedColumns) {
            column.owner = this;
        }
    }

    // TODO: Consider moving this to SourceConcreteEntity
    public get selectedColumns(): SourceColumn[] {
        return this._selectedColumns;
    }

    // TODO: Consider moving this to an interface
    public addSelectedColumn(column: Column) : SourceColumn {
        const sourceColumn = new SourceColumn(column);
        sourceColumn.owner = this;
        return sourceColumn;
    }

    public removeSelectedColumn(column: Column) {
        const sourceColumn = new SourceColumn(column);
        const index = this._selectedColumns.findIndex((value) => value.name === sourceColumn.name);
        // TODO: this way of assert does not work
        // assert(index >= 0, `Column ${sourceColumn.name} is not selected in source table ${this.name}`);
        this._selectedColumns = this._selectedColumns.splice(index, 1);
    } 

    public constructor(name: string,
        selectedColumns: SourceColumn[] = [],
        public readonly description: string | null = null) {
        super(name);
        this._selectedColumns = selectedColumns;
        // this.createBackwardConnections(); // TODO: Is this needed?
    }

    public accept(visitor: MappingVisitor): void {
        visitor.visitSourceTable(this);  
    }

    /** Finds a selected column by its name and returns it.
     * 
     * @param columnName of the column to be returned.
     * @returns the column mapping for the given column name.
     * @throws Error if the columnName is not name of any selected column.
     */
    public getSelectedColumn(columnName : string): SourceColumn {
        // TODO: document this
        const column = this._selectedColumns.find((value) => value.name === columnName);
        if (column === undefined) {
            throw new Error(`Column ${columnName} is not selected in source table ${this.name}`);
        }

        return column;
    }
}
