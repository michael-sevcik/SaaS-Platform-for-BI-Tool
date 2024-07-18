using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.Json;

[assembly: InternalsVisibleTo("SqlViewGeneratorTests")]

namespace BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing
{
    /// <summary>
    /// Represents a reference resolver used for mapping references in JSON parsing.
    /// </summary>
    internal class MappingReferenceResolver : ReferenceResolver
    {
        private uint referenceCount;
        private readonly Dictionary<string, object> referenceIdToObjectMap = new Dictionary<string, object>();
        private readonly Dictionary<object, string> objectToReferenceIdMap = new Dictionary<object, string>(ReferenceEqualityComparer.Instance);

        /// <summary>
        /// Adds a reference to the reference resolver.
        /// </summary>
        /// <param name="referenceId">The reference ID.</param>
        /// <param name="value">The value to be referenced.</param>
        public override void AddReference(string referenceId, object value)
        {
            if (!referenceIdToObjectMap.TryAdd(referenceId, value))
            {
                throw new JsonException();
            }
        }

        /// <summary>
        /// Gets the reference ID for the specified value and indicates whether it already exists.
        /// </summary>
        /// <param name="value">The value to get the reference ID for.</param>
        /// <param name="alreadyExists">Indicates whether the reference ID already exists.</param>
        /// <returns>The reference ID for the specified value.</returns>
        public override string GetReference(object value, out bool alreadyExists)
        {
            if (objectToReferenceIdMap.TryGetValue(value, out string? referenceId))
            {
                alreadyExists = true;
            }
            else
            {
                referenceCount++;
                referenceId = referenceCount.ToString();
                objectToReferenceIdMap.Add(value, referenceId);
                alreadyExists = false;
            }

            return referenceId;
        }

        /// <summary>
        /// Resolves the reference with the specified reference ID.
        /// </summary>
        /// <param name="referenceId">The reference ID to resolve.</param>
        /// <returns>The resolved object.</returns>
        public override object ResolveReference(string referenceId)
        {
            if (!referenceIdToObjectMap.TryGetValue(referenceId, out object? value))
            {
                throw new JsonException();
            }

            return value;
        }
    }
}
