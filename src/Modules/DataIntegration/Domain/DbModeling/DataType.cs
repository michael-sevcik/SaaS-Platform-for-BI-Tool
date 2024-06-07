﻿using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.Domain.DbModelling;

/// <summary>
/// Represents a base class data type of a <see cref="Column"/> in a database <see cref="Table"/>.
/// </summary>
[JsonDerivedType(typeof(SimpleType), typeDiscriminator: SimpleType.Descriptor)]
[JsonDerivedType(typeof(NVarChar), typeDiscriminator: NVarChar.Descriptor)]
[JsonDerivedType(typeof(NVarCharMax), typeDiscriminator: NVarCharMax.Descriptor)]
//[JsonDerivedType(typeof(VarChar), typeDiscriminator: VarChar.Descriptor)]
//[JsonDerivedType(typeof(VarCharMax), typeDiscriminator: VarCharMax.Descriptor)]
[JsonDerivedType(typeof(UnknownDataType), typeDiscriminator: UnknownDataType.Descriptor)]
public abstract class DataTypeBase
{
    /// <summary>
    /// Gets or sets a value indicating whether the data type is nullable.
    /// </summary>
    public bool IsNullable { get; set; } = false;
}

/// <summary>
/// Represents an unknown data type. Used when the data type name is not recognized.
/// </summary>
public sealed class UnknownDataType : DataTypeBase
{
    /// <summary>
    /// Descriptor for the serialization of <see cref="UnknownDataType"/>.
    /// </summary>
    public const string Descriptor = "unknown";

    /// <summary>
    /// Constructs a new instance of <see cref="UnknownDataType"/>.
    /// </summary>
    /// <remarks>Meant for Deserialization purposes.</remarks>
    public UnknownDataType() { }

    /// <summary>
    /// Constructs a new instance of <see cref="UnknownDataType"/> with a given <paramref name="typeName"/>.
    /// </summary>
    /// <param name="typeName">The name of the given type.</param>
    public UnknownDataType(string typeName) => StoreType= typeName;

    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    public string StoreType { get; set; } = string.Empty;
}

/// <summary>
/// Encapsulates simple data types - <see cref="SimpleType.SimpleType"/>.
/// </summary>
public sealed class SimpleType : DataTypeBase
{
    /// <summary>
    /// Descriptor for the serialization of simple data types.
    /// </summary>
    public const string Descriptor = "simple";

    /// <summary>
    /// Constructs a new instance of <see cref="SimpleType"/>.
    /// </summary>
    /// <remarks>Meant for Deserialization purposes.</remarks>
    public SimpleType() { }

    /// <summary>
    /// Constructs a new instance of <see cref="SimpleType"/> with a given <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type of the simple data type.</param>
    public SimpleType(Types type) => Type = type;

    /// <summary>
    /// Represents the type of the simple data type.
    /// </summary>
    public enum Types
    {
        TinyInteger,
        SmallInteger,
        Integer,
        BigInteger,
        Money,
        Float,
        Decimal,
        Numeric, // TODO: consider creating a separate class for this type with precision and scale properties
        Boolean,
        Date,
        Datetime,
        DatetimeOffset,
        Timestamp,
        Time,
    }

    /// <summary>
    /// Gets or sets the type of the simple data type.
    /// </summary>
    public Types Type { get; set; }
}

/// <summary>
/// Represents a Unicode string data type with a fixed length.
/// </summary>
public sealed class NVarChar : DataTypeBase
{
    /// <summary>
    /// Descriptor for the serialization of <see cref="NVarChar"/> data type.
    /// </summary>
    public const string Descriptor = "nVarChar";

    /// <summary>
    /// Constructs a new instance of <see cref="NVarChar"/>.
    /// </summary>
    /// <remarks>Meant for Deserialization purposes.</remarks>
    public NVarChar() { }

    /// <summary>
    /// Constructs a new instance of <see cref="NVarChar"/> with a given <paramref name="lenght"/>.
    /// </summary>
    /// <param name="lenght">The type of the simple data type. Must be positive.</param>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="lenght"/> is not positive.</exception>
    public NVarChar(int lenght)
    {
        if (lenght <= 0)
        {
            throw new ArgumentException("The length of the string must be positive.", nameof(lenght));
        }

        Lenght = lenght;
    }

    /// <summary>
    /// Gets or sets the length limit of the string.
    /// </summary>
    public int Lenght { get; set; }
}


/// <summary>
/// Represents a Unicode string data type with a variable length.
/// </summary>
public sealed class NVarCharMax : DataTypeBase
{
    /// <summary>
    /// Descriptor for the serialization of <see cref="NVarCharMax"/> data type.
    /// </summary>
    public const string Descriptor = "nVarCharMax";
}


///// <summary>
///// Represents a string data type with a variable length.
///// </summary>
//public sealed class VarCharMax : DataTypeBase
//{
//    /// <summary>
//    /// Descriptor for the serialization of <see cref="VarCharMax"/> data type.
//    /// </summary>
//    public const string Descriptor = "varCharMax";
//}


///// <summary>
///// Represents a string data type with a fixed length.
///// </summary>
//public sealed class VarChar : DataTypeBase
//{
//    /// <summary>
//    /// Descriptor for the serialization of <see cref="NVarChar"/> data type.
//    /// </summary>
//    public const string Descriptor = "nVarChar";

//    /// <summary>
//    /// Constructs a new instance of <see cref="VarChar"/>.
//    /// </summary>
//    /// <remarks>Meant for Deserialization purposes.</remarks>
//    public VarChar() { }

//    /// <summary>
//    /// Constructs a new instance of <see cref="VarChar"/> with a given <paramref name="lenght"/>.
//    /// </summary>
//    /// <param name="lenght">The type of the simple data type. Must be positive.</param>
//    /// <exception cref="ArgumentException">Thrown when the <paramref name="lenght"/> is not positive.</exception>
//    public VarChar(int lenght)
//    {
//        if (lenght <= 0)
//        {
//            throw new ArgumentException("The length of the string must be positive.", nameof(lenght));
//        }

//        Lenght = lenght;
//    }

//    /// <summary>
//    /// Gets or sets the length limit of the string.
//    /// </summary>
//    public int Lenght { get; set; }
//}