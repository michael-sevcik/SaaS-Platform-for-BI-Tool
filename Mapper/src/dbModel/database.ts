export class Database {
    constructor(public name: string, public tables: Table[]) {
    }
}

export class Table {
    constructor(public name: string, public columns: Column[]) {
    }
}

export enum ColumnType {
    string = 'string',
    int = 'int',
    decimal = 'decimal',
    date = 'date',
    boolean = 'boolean'
}

/**
 * Represents a column in a database table.
 */
export class Column {
    /**
     * Creates a new instance of the Column class.
     * @param name The name of the column.
     * @param type The type of the column.
     */
    constructor(public name: string, public type: ColumnType) {
    }

    /**
     * Checks if the current column is assignable with another column.
     * @param column The column to compare with.
     * @returns True if the columns have the same type, false otherwise.
     */
    public isAssignableWith(column: Column): boolean {
        return this.type === column.type;
    }
}