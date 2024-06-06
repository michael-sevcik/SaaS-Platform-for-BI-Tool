using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.Domain.SchemaScraping;

[JsonDerivedType(typeof(SimpleType), typeDiscriminator: SimpleType.Descriptor)]
[JsonDerivedType(typeof(NVarChar), typeDiscriminator: NVarChar.Descriptor)]
[JsonDerivedType(typeof(VarChar), typeDiscriminator: VarChar.Descriptor)]
public abstract class DataTypeBase
{
}

public class SimpleType : DataTypeBase
{
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

public class NVarChar : DataTypeBase
{
    public const string Descriptor = "nVarChar";

    public int Lenght { get; set; }
}

public class VarChar : DataTypeBase
{
    public const string Descriptor = "varChar";
}