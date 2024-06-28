using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;
using System.Text;

namespace BIManagement.Modules.DataIntegration.Application.Mapping.SqlViewGenerating;

/// <summary>
/// Class containing helper extension methods for <see cref="StringBuilder"/> that are used by <see cref="SqlViewVisitor"/>.
/// </summary>
internal static class StringBuilderExtensions
{
    /// <summary>
    /// Appends the string builder by the representation of the column mapping - 'ENTITY_NAME.COLUMN_NAME'.
    /// </summary>
    /// <param name="sb">The string builder that should be appended.</param>
    /// <param name="columnMapping">The column mapping that should be processed.</param>
    /// <returns>Appended instance of string builder passed as <paramref name="sb"/>.</returns>
    public static StringBuilder AppendColumnMapping(this StringBuilder sb, ColumnMapping columnMapping)
        => sb.Append(columnMapping.SourceEntity.Name).Append('.').Append(columnMapping.SourceColumn);
}
