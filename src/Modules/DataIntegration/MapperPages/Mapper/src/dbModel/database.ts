import { Exclude, Type } from "class-transformer";
import { NVarChar, NVarCharMax, SimpleType, UnknownDataType, DataTypeBase } from "./dataTypes";

/**
 * Represents model of a database.
 */
export class Database {
    /**
     * the Tables of the database
     */
    @Type(() => Table)
    public readonly tables: Table[];

    /**
     * Initializes a new instance of {@link Database}
     * @param name the name of the database
     * @param tables the tables of the database.
     */
    constructor(public readonly name: string, tables: Table[]) {
        this.tables = tables;
    }
}

/**
 * Represents a column in a database table.
 */
export class Column {
    /**
     * Type of column.
     */
    @Type(() => DataTypeBase, {
        discriminator: {
            property: '$type',
            subTypes: [
                { value: UnknownDataType, name: UnknownDataType.Descriptor },
                { value: SimpleType, name: SimpleType.Descriptor },
                { value: NVarChar, name: NVarChar.Descriptor },
                { value: NVarCharMax, name: NVarCharMax.Descriptor },
            ],
        },
        keepDiscriminatorProperty: false,
    })
    public readonly dataType: DataTypeBase;


    /**
     * Creates a new instance of the Column class.
     * @param name The name of the column.
     * @param dataType The type of the column.
     */
    constructor(
        public readonly name: string,
        dataType: DataTypeBase,
        public readonly description: string | null = null) {
        this.dataType = dataType;
    }

    /**
     * Checks if the current column is assignable with another column.
     * @param column The column to compare with.
     * @returns True if the if the other column can be assigned to this column, false otherwise.
     */
    public isAssignableWith(column: Column): boolean {
        return this.dataType.isAssignableWith(column.dataType);
    }

    /**
     * Checks if the current column is comparable with the other column.
     * @param column The column to compare with.
     * @returns True if the columns are comparable, false otherwise.
     */
    public isComparableWith(column: Column): boolean {
        return this.dataType.isComparableWith(column.dataType);
    }

}


/**
 * Represents a table in a database schema.
 */
export class Table {
    @Exclude()
    public get fullName(): string {
        return this.schema ? `${this.schema}.${this.name}` : this.name;
    }

    /**
     * Columns of the table.
     */
    @Type(() => Column)
    public readonly columns: Column[];

    /**
     * Initializes a new instance of {@link Table}
     * @param name the name of the table.
     * @param schema the schema (prefix) of the table. 
     * @param columns the columns of the table. 
     * @param description the description of the table.
     */
    constructor(
        public readonly name: string,
        columns: Column[],
        public readonly schema: string | null = null,
        public readonly description: string | null = null) {
        this.columns = columns;
        }
}