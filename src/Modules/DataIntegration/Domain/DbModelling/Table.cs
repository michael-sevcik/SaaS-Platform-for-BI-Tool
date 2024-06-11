using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Domain.DbModelling;

/// <summary>
/// Represents a table in a database schema.
/// </summary>
public class Table
{
    /// <summary>
    /// Gets or sets the name of the table.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the schema (prefix) of the table.
    /// </summary>
    public string Schema { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the columns that are part of the primary key of the table.
    /// </summary>
    public ICollection<Column> PrimaryKeys { get; set; } = [];

    /// <summary>
    /// Gets or sets columns of the table.
    /// </summary>
    public ICollection<Column> Columns { get; set; } = [];

    /// <summary>
    /// Gets or sets the description of the table.
    /// </summary>
    public string? Description { get; set; }
}
