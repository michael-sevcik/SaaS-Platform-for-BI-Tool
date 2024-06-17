/**
 * Represents a base class data type of a {@link Column} in a database {@link Table}
 */
export abstract class DataTypeBase {
    protected abstract get TypeDescriptor(): string; 

    public get Descriptor(): string {
        return this.TypeDescriptor + (this.isNullable ? "?" : "");
    }

    /**
     * Checks whether this data type is assignable with another data type.
     * @param otherDataType The other data type. 
     */
    public abstract isAssignableWith(otherDataType: DataTypeBase): boolean;

    /**
     * Checks whether this data type is comparable with another data type.
     * @param otherDataType The other data type.
     */
    public abstract isComparableWith(otherDataType: DataTypeBase): boolean;

    /**
     * @param isNullable Value indicating whether the data type is nullable.
     */
    constructor(public readonly isNullable: boolean) { }

    /**
     * The method checks if the data type is compatible with another data type.
     * @param otherDataType The data type to compare with.
     * @returns Returns true if the this data type is nullable
     *  or if the nullability of the data types is the same.   
     */
    protected isNullabilityCompatible(otherDataType: DataTypeBase): boolean {
        return this.isNullable || this.isNullable === otherDataType.isNullable;
    }
}

/**
 * Represents an unknown data type. Used when the data type name is not recognized
 */
export class UnknownDataType extends DataTypeBase {
    /** @inheritdoc */
    public isComparableWith(otherDataType: DataTypeBase): boolean {
        return otherDataType instanceof UnknownDataType && this.storeType === otherDataType.storeType;
    }
    
    /** @inheritdoc */
    public isAssignableWith(dataType: DataTypeBase) : boolean {
         return this.isNullabilityCompatible(dataType) && this.isComparableWith(dataType);
    }

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
    Datetime2 = "Datetime2",
    DatetimeOffset = "DatetimeOffset",
    Timestamp = "Timestamp",
    Time = "Time",
}

/**
 * Encapsulates simple data types - {@link SimpleDataTypes}.
 */
export class SimpleType extends DataTypeBase
{
    /** @inheritdoc */
    public isComparableWith(otherDataType: DataTypeBase): boolean {
        return otherDataType instanceof SimpleType && (
            this.type === otherDataType.type ||
            (SimpleType.isNumericType(this.type) && SimpleType.isNumericType(otherDataType.type)) ||
            (this.type === SimpleDataTypes.Datetime && otherDataType.type === SimpleDataTypes.DatetimeOffset) ||
            (this.type === SimpleDataTypes.DatetimeOffset && otherDataType.type === SimpleDataTypes.Datetime) ||
            (this.type === SimpleDataTypes.Datetime2 && otherDataType.type === SimpleDataTypes.DatetimeOffset) ||
            (this.type === SimpleDataTypes.DatetimeOffset && otherDataType.type === SimpleDataTypes.Datetime2) ||
            (this.type === SimpleDataTypes.Datetime && otherDataType.type === SimpleDataTypes.Datetime2) ||
            (this.type === SimpleDataTypes.Datetime2 && otherDataType.type === SimpleDataTypes.Datetime));
    }

    /** @inheritdoc */
    public isAssignableWith(dataType: DataTypeBase): boolean {
        return dataType instanceof SimpleType
         && this.isNullabilityCompatible(dataType)
         // check if the types are the same or if the types are compatible
            && (this.type === dataType.type ||
                (this.type === SimpleDataTypes.Numeric && dataType.type === SimpleDataTypes.Decimal) ||
                (this.type === SimpleDataTypes.Decimal && 
                    (dataType.type === SimpleDataTypes.Numeric ||
                         SimpleType.isIntegralType(dataType.type))) ||
                (this.type === SimpleDataTypes.Numeric &&
                    (dataType.type === SimpleDataTypes.Decimal ||
                        SimpleType.isIntegralType(dataType.type))) ||
                (this.type === SimpleDataTypes.SmallInteger && dataType.type === SimpleDataTypes.TinyInteger) ||
                (this.type === SimpleDataTypes.Integer && 
                    (dataType.type === SimpleDataTypes.TinyInteger ||
                        dataType.type === SimpleDataTypes.SmallInteger)) ||
                (this.type === SimpleDataTypes.BigInteger && SimpleType.isIntegralType(dataType.type)));
    }

    private static isIntegralType(type: SimpleDataTypes): boolean {
        switch (type) {
            case SimpleDataTypes.TinyInteger:
            case SimpleDataTypes.SmallInteger:
            case SimpleDataTypes.Integer:
            case SimpleDataTypes.BigInteger:
                return true;
            default:
                return false;
        
        }
    }

    private static isNumericType(type: SimpleDataTypes): boolean {
        switch (type) {
            case SimpleDataTypes.Numeric:
            case SimpleDataTypes.Decimal:
            case SimpleDataTypes.Money:
                return true;
            default:
                return this.isIntegralType(type);
        }
    }
    
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
    /** @inheritdoc */
    public isComparableWith(otherDataType: DataTypeBase): boolean {
        return otherDataType instanceof NVarChar || otherDataType instanceof NVarCharMax;
    }

    /** @inheritdoc */
    public isAssignableWith(dataType: DataTypeBase): boolean {
        return dataType instanceof NVarChar
         && this.isNullabilityCompatible(dataType)
         && this.length >= dataType.length;
    }
    
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
    /** @inheritdoc */
    public isComparableWith(otherDataType: DataTypeBase): boolean {
        return otherDataType instanceof NVarChar || otherDataType instanceof NVarCharMax;
    }

    /** @inheritdoc */
    public isAssignableWith(dataType: DataTypeBase): boolean {
        return this.isNullabilityCompatible(dataType) && 
        (dataType instanceof NVarCharMax || dataType instanceof NVarChar);
    }

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