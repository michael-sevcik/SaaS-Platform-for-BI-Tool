import { MappingVisitor } from "../mappingVisitor";
import { SourceColumn } from "../sourceColumn";
import { SourceEntity } from "./sourceEntity";

/** Represents a source entity that directly provides, or generates data.
 * @note These entities are represented as elements, and not as links between elements.
 */
export abstract class SourceEntityBase extends SourceEntity {
    protected _selectedColumns: SourceColumn[];

    constructor(name: string) {
        super(name);
    }

    /** @inheritdoc */
    public get selectedColumns(): SourceColumn[] {
        return this._selectedColumns;
    }

    /**
     * Gets full name of the source entity.
     */
    public abstract get fullName(): string;

    /** Finds a selected column by its name and returns it.
     * 
     * @param columnName of the column to be returned.
     * @returns the column mapping for the given column name.
     * @throws Error if the columnName is not name of any selected column.
     */
    public getSelectedColumn(columnName: string): SourceColumn {
        const column = this._selectedColumns.find((value) => value.name === columnName);
        if (column === undefined) {
            throw new Error(`Column ${columnName} is not selected in source table ${this.name}`);
        }

        return column;
    }
}