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
    /// <param name="column">The column that should be processed.</param>
    /// <returns>Appended instance of string builder passed as <paramref name="sb"/>.</returns>
    /// <exception cref="NullReferenceException">
    /// Thrown when the <see cref="SourceColumn.Owner"/> property of the <paramref name="column"/> is not initialized.
    /// </exception>
    public static StringBuilder AppendSourceColumn(this StringBuilder sb, SourceColumn column)
    {
        var owner = column.Owner ?? throw new NullReferenceException($"column \"{nameof(column.Owner)}\" is not initialized.");
        return sb.Append(column.Owner.Name).Append('.').Append(column.Name);
    }

    /// <summary>
    /// Appends the string builder by the representation of the column mapping - 'OWNER_ENTITY_NAME__COLUMN_NAME'.
    /// </summary>
    /// <param name="sb">The string builder that should be appended.</param>
    /// <param name="column">The column that should be processed.</param>
    /// <returns>Appended instance of string builder passed as <paramref name="sb"/>.</returns>
    /// <exception cref="NullReferenceException">
    /// Thrown when the <see cref="SourceColumn.Owner"/> property of the <paramref name="column"/> is not initialized.
    /// </exception>
    public static StringBuilder AppendRenamedSourceColumn(this StringBuilder sb, SourceColumn column)
    {
        var owner = column.Owner ?? throw new NullReferenceException($"column \"{nameof(column.Owner)}\" is not initialized.");
        return sb.Append(column.Owner.Name).Append("__").Append(column.Name);
    }

    /// <summary>
    /// Appends the string builder with the representation of the column mapping - 'ENTITY_NAME.COLUMN_NAME AS ENTITY_NAME__COLUMN_NAME'.
    /// </summary>
    /// <param name="sb">The string builder that should be appended.</param>
    /// <param name="column">The column that should be processed.</param>
    /// <returns>Appended instance of string builder passed as <paramref name="sb"/>.</returns>
    /// <exception cref="NullReferenceException">
    /// Thrown when the <see cref="SourceColumn.Owner"/> property of the <paramref name="column"/> is not initialized.
    /// </exception>
    public static StringBuilder AppendSourceColumnWithRenaming(this StringBuilder sb, SourceColumn column)
        => sb.AppendSourceColumn(column).Append(" AS ").AppendRenamedSourceColumn(column);


    /// <summary>
    /// Appends the selected columns to the string builder.
    /// </summary>
    /// <param name="sb">The string builder that should be appended.</param>
    /// <param name="columns">The array of source columns to be appended.</param>
    /// <returns>The appended instance of the string builder passed as <paramref name="sb"/>.</returns>
    public static StringBuilder AppendSelectedColumns(this StringBuilder sb, SourceColumn[] columns)
    {
        // TODO: DELELE IF NOT USED
        for (int i = 0; i < columns.Length - 1; ++i)
        {
            sb.AppendSourceColumn(columns[i]).Append(", ");
        }

        if (columns.Length > 0)
        {
            sb.AppendSourceColumn(columns[^1]);
        }

        return sb;
    }

    /// <summary>
    /// Appends the child column reference to the string builder - ENTITY_NAME.ENTITY_NAME__COLUMN_NAME.
    /// </summary>
    /// <param name="sb">The string builder that should be appended.</param>
    /// <param name="childEntity">The child entity.</param>
    /// <param name="column">The column that should be processed.</param>
    /// <returns>The appended instance of the string builder passed as <paramref name="sb"/>.</returns>
    public static StringBuilder AppendChildColumnReference(this StringBuilder sb, ISourceEntity childEntity, SourceColumn column)
    {
        return sb.Append(childEntity.Name).Append('.').AppendRenamedSourceColumn(column);
    }

    /// <summary>
    /// Appends the child column reference to the string builder - ENTITY_NAME.ENTITY_NAME__COLUMN_NAME.
    /// </summary>
    /// <param name="sb">The string builder that should be appended.</param>
    /// <param name="childEntity">The child entity.</param>
    /// <param name="column">The column that should be processed.</param>
    /// <returns>The appended instance of the string builder passed as <paramref name="sb"/>.</returns>
    public static StringBuilder AppendChildColumnReferenceWithRenaming(
        this StringBuilder sb,
        ISourceEntity childEntity,
        SourceColumn column,
        string newName)
    {
        return sb.AppendChildColumnReference(childEntity, column).Append(" AS ").Append(newName);
    }

    /// <summary>
    /// Appends the selected columns to the string builder
    /// and renames them using <see cref="AppendSourceColumnWithRenaming(StringBuilder, SourceColumn)"/>.
    /// </summary>
    /// <param name="sb">The string builder that should be appended.</param>
    /// <param name="columns">The array of source columns to be appended.</param>
    /// <returns>The appended instance of the string builder passed as <paramref name="sb"/>.</returns>
    public static StringBuilder AppendSelectedColumnsWithRenaming(this StringBuilder sb, SourceColumn[] columns)
    {
        for (int i = 0; i < columns.Length - 1; ++i)
        {
            sb.AppendSourceColumnWithRenaming(columns[i]).Append(',').AppendLine();
        }

        if (columns.Length > 0)
        {
            sb.AppendSourceColumnWithRenaming(columns[^1]).AppendLine();
        }

        return sb;
    }
}
