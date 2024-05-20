namespace BIManagementPlatform.Modules.DataIntegration.SqlViewGenerator.JsonModel;

public class ColumnMapping
{
    public ColumnMapping(ISourceEntity sourceEntity, string sourceColumn)
        => (SourceEntity, SourceColumn) = (sourceEntity, sourceColumn);

    public ISourceEntity SourceEntity { get; }

    public string SourceColumn { get; }
}