import { Type } from "class-transformer";

/**
 * Represents a base class data type of a {@link Column} in a database {@link Table}
 */
export abstract class DataTypeBase {
    protected abstract get TypeDescriptor(): string; 

    public get Descriptor(): string {
        return this.TypeDescriptor + (this.isNullable ? "?" : "");
    }

    /**
     * @param isNullable Value indicating whether the data type is nullable.
     */
    constructor(public readonly isNullable: boolean) { }

}

/**
 * Represents an unknown data type. Used when the data type name is not recognized
 */
export class UnknownDataType extends DataTypeBase {
    protected get TypeDescriptor(): string {
        return this.storeType;
    }
    /**
     * Descriptor for the serialization of {@link UnknownDataType}.
     */
    public static readonly Descriptor = "unknown";


    /**
     * Constructs a new instance of {@link UnknownDataType} with a given {@link storeType} parameter.
     * @param storeType The name of the given type.
     * @param isNullable Value indicating whether the data type is nullable.
     */
    constructor(
        public readonly storeType: string,
        isNullable: boolean) { super(isNullable); }
}

/**
 * Represents the type of the simple data type.
 */
export enum SimpleDataTypes {
    TinyInteger = "TinyInteger",
    SmallInteger = "SmallInteger",
    Integer = "Integer",
    BigInteger = "BigInteger",
    Money = "Money",
    Float = "Float",
    Decimal = "Decimal",
    Numeric = "Numeric", // TODO: consider creating a separate class for this type with precision and scale properties
    Boolean = "Boolean",
    Date = "Date",
    Datetime = "Datetime",
    DatetimeOffset = "DatetimeOffset",
    Timestamp = "Timestamp",
    Time = "Time",
}

/**
 * Encapsulates simple data types - {@link SimpleDataTypes}.
 */
export class SimpleType extends DataTypeBase
{
    protected get TypeDescriptor(): string {
        return this.type;
    }

    /**
     * Descriptor for the serialization of simple data types.
     */
    public static readonly Descriptor = "simple";

    /**
     * Constructs a new instance of {@link SimpleType} with a given {@link type}.
     * @param type The type of the simple data type.
     * @param isNullable Value indicating whether this data type is nullable.
     */
    constructor(
        public readonly type: SimpleDataTypes,
        isNullable : boolean ) { super(isNullable);}
}

/**
 * Represents a Unicode string data type with a fixed length.
 */
export class NVarChar extends DataTypeBase
{
    protected get TypeDescriptor(): string {
        return `NVarChar(${this.length})`;
    }

    /**
     * Descriptor for the serialization of {@link NVarChar} data type.
     */
    public static readonly Descriptor = "nVarChar";

    /**
     * Constructs a new instance of {@link NVarChar} with a given {@link length}.
     * @param length The number of allowed characters. Must be positive.
     * @param isNullable Value indicating whether this data type is nullable.
     */
    constructor(public readonly length : number, isNullable: boolean) { super(isNullable); }
}

/**
 * Represents a Unicode string data type with a variable length.
 */
export class NVarCharMax extends DataTypeBase
{
    protected get TypeDescriptor(): string {
        return "NVarChar(Max)";
    }

    /**
     * Descriptor for the serialization of {@link NVarCharMax} data type.
     */
    public static readonly Descriptor = "nVarCharMax";
    
    /**
     * Constructs a new instance of {@link NVarCharMax}.
     * @param isNullable Value indicating whether this data type is nullable.
     */
    constructor(isNullable: boolean) { super(isNullable); }
}