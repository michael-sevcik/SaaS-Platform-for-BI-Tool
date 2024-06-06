using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.Domain.SchemaScraping;

/// <summary>
/// Represents a base class data type of a <see cref="Column"/> in a database <see cref="Table"/>.
/// </summary>
[JsonDerivedType(typeof(SimpleType), typeDiscriminator: SimpleType.Descriptor)]
[JsonDerivedType(typeof(NVarChar), typeDiscriminator: NVarChar.Descriptor)]
[JsonDerivedType(typeof(VarChar), typeDiscriminator: VarChar.Descriptor)]
public abstract class DataTypeBase
{
}

/// <summary>
/// Encapsulates simple data types - <see cref="SimpleType.SimpleType"/>.
/// </summary>
public class SimpleType : DataTypeBase
{
    /// <summary>
    /// Descriptor for the serialization of simple data types.
    /// </summary>
    public const string Descriptor = "simple";
    public enum Simpletype
    {
        Integer,
        Boolean,
        Decimal,
        Date,
        Datetime,
        Time,
    }

    public Simpletype Type { get; set; }
}

/// <summary>
/// Represents a string data type with a fixed length.
/// </summary>
public class NVarChar : DataTypeBase
{
    /// <summary>
    /// Descriptor for the serialization of <see cref="NVarChar"/> data type.
    /// </summary>
    public const string Descriptor = "nVarChar";

    /// <summary>
    /// Gets or sets the length limit of the string.
    /// </summary>
    public int Lenght { get; set; }
}


/// <summary>
/// Represents a string data type with a variable length.
/// </summary>
public class VarChar : DataTypeBase
{
    /// <summary>
    /// Descriptor for the serialization of <see cref="VarChar"/> data type.
    /// </summary>
    public const string Descriptor = "varChar";
}