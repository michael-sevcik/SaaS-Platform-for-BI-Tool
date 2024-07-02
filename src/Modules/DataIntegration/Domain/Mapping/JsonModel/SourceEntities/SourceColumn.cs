using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;

/// <summary>
/// Represents a column of a source entity.
/// </summary>
public class SourceColumn
{
    private static DataTypeBase placeHolderDataType = new UnknownDataType("NotInitialized", false);

    [JsonIgnore]
    public ISourceEntity? Owner { get; set; }
    public string Name { get; set; }
    public DataTypeBase DataType { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceColumn"/> class.
    /// </summary>
    /// <remarks>Use only for deserialization.</remarks>
    public SourceColumn()
    {
        this.Name = "";
        this.DataType = SourceColumn.placeHolderDataType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceColumn"/> class with the specified name and data type.
    /// </summary>
    /// <param name="name">The name of the source column.</param>
    /// <param name="dataType">The data type of the source column.</param>
    public SourceColumn(string name, DataTypeBase dataType)
    {
        Name = name;
        DataType = dataType;
    }
}
