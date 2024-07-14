using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities
{
    public class CustomQuery : ISourceEntity
    {
        public const string TypeDiscriminator = "customQuery";

        public CustomQuery()
        {
            this.SelectedColumns = Array.Empty<SourceColumn>();
            this.Name = string.Empty;
            this.Query = string.Empty;
        }

        public CustomQuery(string name, string query, SourceColumn[] selectedColumns)
        {
            Name = name;
            Query = query;
            SelectedColumns = selectedColumns;
        }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <summary>
        /// The query to execute.
        /// </summary>
        public string Query { get; set; }

        /// <inheritdoc/>
        public bool HasDependency => false;

        /// <inheritdoc/>
        public SourceColumn[] SelectedColumns { get; set; }

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
