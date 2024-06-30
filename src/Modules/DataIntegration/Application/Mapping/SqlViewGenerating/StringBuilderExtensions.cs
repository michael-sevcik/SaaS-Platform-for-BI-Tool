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
    /// <exception cref="NullReferenceException">
    /// Thrown when the <see cref="SourceColumn.Owner"/> property of the <paramref name="columnMapping"/> is not initialized.
    /// </exception>
    public static StringBuilder AppendColumnMapping(this StringBuilder sb, SourceColumn columnMapping)
    {
        var owner = columnMapping.Owner ?? throw new NullReferenceException($"column \"{nameof(columnMapping.Owner)}\" is not initialized.");
        return sb.Append(columnMapping.Owner.Name).Append('.').Append(columnMapping.Name);
    }
}
