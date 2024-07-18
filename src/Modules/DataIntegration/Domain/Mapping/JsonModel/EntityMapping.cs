using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel
{
    /// <summary>
    /// Represents an entity mapping in JSON format.
    /// </summary>
    public class EntityMapping : IVisitable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMapping"/> class.
        /// </summary>
        /// <param name="name">The name of the entity mapping.</param>
        /// <param name="schema">The schema of the entity mapping.</param>
        /// <param name="sourceEntities">The source entities associated with the entity mapping.</param>
        /// <param name="sourceEntity">The source entity associated with the entity mapping.</param>
        /// <param name="columnMappings">The column mappings for the entity mapping.</param>
        /// <param name="description">The description of the entity mapping.</param>
        public EntityMapping(
            string name,
            string? schema,
            ISourceEntity[] sourceEntities,
            ISourceEntity? sourceEntity,
            Dictionary<string, SourceColumn?> columnMappings,
            string? description = null)
        {
            Name = name;
            Schema = schema;
            SourceEntities = sourceEntities;
            SourceEntity = sourceEntity;
            ColumnMappings = columnMappings;
            Description = description;
        }

        /// <summary>
        /// Gets the name of the entity mapping.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the schema of the entity mapping.
        /// </summary>
        public string? Schema { get; }

        /// <summary>
        /// Gets the description of the entity mapping.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Gets the source entities associated with the entity mapping.
        /// </summary>
        public ISourceEntity[] SourceEntities { get; }

        /// <summary>
        /// Gets the source entity associated with the entity mapping.
        /// </summary>
        public ISourceEntity? SourceEntity { get; }

        /// <summary>
        /// Gets the column mappings for the entity mapping.
        /// </summary>
        public Dictionary<string, SourceColumn?> ColumnMappings { get; }

        /// <summary>
        /// Accepts a visitor and performs an operation on the entity mapping.
        /// </summary>
        /// <param name="visitor">The visitor to accept.</param>
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
