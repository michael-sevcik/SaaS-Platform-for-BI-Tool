namespace SqlViewGenerator.JsonModel;

public class ColumnMapping
{
    public ColumnMapping(ISourceEntity sourceEntity, string sourceColumn)
        => (this.SourceEntity, this.SourceColumn) = (sourceEntity, sourceColumn);

    public ISourceEntity SourceEntity { get; }

    public string SourceColumn { get; }
}