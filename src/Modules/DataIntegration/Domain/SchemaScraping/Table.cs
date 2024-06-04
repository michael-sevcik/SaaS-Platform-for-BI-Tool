using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Domain.SchemaScraping;

/// <summary>
/// Represents a table in a database schema.
/// </summary>
public class Table
{
    /// <summary>
    ///  Gets or sets the name of the table.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the columns that are part of the primary key of the table.
    /// </summary>
    public ICollection<Column> PrimaryKeys { get; set; } = new List<Column>();

    /// <summary>
    /// Gets or sets the columns that are part of the foreign key of the table.
    /// </summary>
    public ICollection<Column> Columns { get; set; } = new List<Column>();
}
