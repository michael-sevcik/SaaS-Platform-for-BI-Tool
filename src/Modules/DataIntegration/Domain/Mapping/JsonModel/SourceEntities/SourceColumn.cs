using BIManagement.Modules.DataIntegration.Domain.DbModelling;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;

public class SourceColumn(string name, DataTypeBase dataType)
{
    public ISourceEntity? Owner { get; set; }

    public string Name { get; } = name;
    public DataTypeBase DataType { get;} = dataType;
}