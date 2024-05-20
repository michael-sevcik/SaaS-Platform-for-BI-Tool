﻿// <copyright file="ISourceEntity.cs" company="Merica s.r.o.">
// Copyright © Merica
// </copyright>

using BIManagementPlatform.Modules.DataIntegration.SqlViewGenerator;

namespace BIManagementPlatform.Modules.DataIntegration.SqlViewGenerator.JsonModel;

/// <summary>
/// Represents a named entity that is a source of column based data.
/// </summary>
public interface ISourceEntity : IVisitable
{
    string Name { get; }

    bool HasDependency { get; }

    string[] SelectedColumns { get; }

    virtual ColumnMapping GetColumnMapping(string columnName)
    {
        if (!SelectedColumns.Contains(columnName))
        {
            throw new InvalidOperationException($"The source table \"{columnName}\" does not contain a column with name \"{columnName}\".");
        }

        return new(this, columnName);
    }
}