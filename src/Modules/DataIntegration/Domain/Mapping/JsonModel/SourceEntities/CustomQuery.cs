using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities
{
    public class CustomQuery(string name, string query, SourceColumn[] selectedColumns) : ISourceEntity
    {
        public const string TypeDiscriminator = "customQuery";

        /// <inheritdoc/>
        public string Name { get; } = name;

        /// <summary>
        /// The query to execute.
        /// </summary>
        public string Query { get; } = query;

        /// <inheritdoc/>
        public bool HasDependency => false;

        /// <inheritdoc/>
        public SourceColumn[] SelectedColumns { get; } = selectedColumns;

        /// <inheritdoc/>
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <inheritdoc/>
        public void AssignColumnOwnership()
        {
            foreach (var column in SelectedColumns)
            {
                column.Owner = this;
            }
        }
    }
}
