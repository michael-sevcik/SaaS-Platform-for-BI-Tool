namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;

public class ColumnMapping
{
    public ColumnMapping(ISourceEntity sourceEntity, string sourceColumn)
        => (SourceEntity, SourceColumn) = (sourceEntity, sourceColumn);

    public ISourceEntity SourceEntity { get; }

    public string SourceColumn { get; }
}