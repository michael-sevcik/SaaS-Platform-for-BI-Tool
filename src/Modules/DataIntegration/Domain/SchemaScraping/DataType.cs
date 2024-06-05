namespace BIManagement.Modules.DataIntegration.Domain.SchemaScraping;

public class DataType
{
    public enum Simpletype
    {
        Integer,
        Boolean,
        Decimal,
        Date,
        Datetime,
        Time,
    }

    public virtual bool IsSimple => true;

    
}

public class NVarChar : DataType
{
    public override bool IsSimple => false;
    public int lenght { get; set; }
}