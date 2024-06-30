// <copyright file="ISourceEntity.cs" company="Merica s.r.o.">
// Copyright © Merica
// </copyright>


// <copyright file="ISourceEntity.cs" company="Merica s.r.o.">
// Copyright © Merica
// </copyright>


// <copyright file="ISourceEntity.cs" company="Merica s.r.o.">
// Copyright © Merica
// </copyright>


// <copyright file="ISourceEntity.cs" company="Merica s.r.o.">
// Copyright © Merica
// </copyright>

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;

/// <summary>
/// Represents a named entity that is a source of column based data.
/// </summary>
public interface ISourceEntity : IVisitable
{
    /// <summary>
    /// Gets the name of the source entity.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets a value indicating whether the source entity has a dependency - child source entity.
    /// </summary>
    bool HasDependency { get; }

    /// <summary>
    /// Gets the selected columns of the source entity.
    /// </summary>
    SourceColumn[] SelectedColumns { get; }

    /// <summary>
    /// Gets the column mapping for the specified column name.
    /// </summary>
    /// <param name="columnName">The name of the column.</param>
    /// <returns>The column mapping.</returns>
    virtual SourceColumn GetColumnMapping(string columnName)
    {
        SourceColumn? column = SelectedColumns.SingleOrDefault(column => column.Name == columnName);
        if (column is null)
        {
            throw new InvalidOperationException($"The source entity \"{Name}\" does not contain a column with name \"{columnName}\".");
        }

        return column;
    }

    /// <summary>
    /// Assigns owners to the owned columns recursively starting with this entity and continuing to the child entities.
    /// </summary>
    void AssignColumnOwnership();
}
