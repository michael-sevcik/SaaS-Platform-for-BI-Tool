/**
 * Represents model of a database.
 */
export class Database {
    /**
     * Initializes a new instance of {@link Database}
     * @param name the name of the database
     * @param tables the tables of the database.
     */
    constructor(public readonly name: string, public readonly tables: Table[]) {
    }
}

/**
 * Represents a table in a database schema.
 */
export class Table {
    /**
     * Initializes a new instance of {@link Table}
     * @param name the name of the table.
     * @param schema the schema (prefix) of the table. 
     * @param columns the columns of the table. 
     * @param description the description of the table.
     */
    constructor(
        public readonly name: string,
        public readonly columns: Column[],
        public readonly schema: string | null = null,
        public readonly description: string | null = null) { }
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
    constructor(
        public readonly name: string,
        public readonly type: ColumnType,
        public readonly description: string | null = null) {
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