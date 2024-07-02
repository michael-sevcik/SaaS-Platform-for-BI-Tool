using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;

namespace BIManagement.Modules.DataIntegration.Application.Mapping.SqlViewGenerating
{
    /// <summary>
    /// Generates SQL view for entity mapping.
    /// </summary>
    internal static class EntityMappingViewGenerator
    {
        /// <summary>
        /// Generates the SQL view for the specified entity mapping.
        /// </summary>
        /// <param name="entityMapping">The entity mapping.</param>
        /// <returns>The generated SQL view.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the source entity is not set.</exception>
        /// <exception cref="NotSupportedException">Thrown when the entityMapping is not fully mapped.</exception>"
        public static string GenerateSqlView(EntityMapping entityMapping)
        {
            if (entityMapping.SourceEntity == null)
            {
                throw new InvalidOperationException("Parameter sour");
            }

            entityMapping.SourceEntity.AssignColumnOwnership();

            var visitor = new SqlViewVisitor();
            entityMapping.Accept(visitor);
            return visitor.GetSqlView();
        }
    }
}
