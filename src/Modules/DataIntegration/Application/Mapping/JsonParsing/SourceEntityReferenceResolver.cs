using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.Json;

[assembly: InternalsVisibleTo("SqlViewGeneratorTests")]

namespace BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing;

internal class SourceEntityReferenceResolver : ReferenceResolver
{
    private uint referenceCount;
    private readonly Dictionary<string, object> referenceIdToObjectMap = [];
    private readonly Dictionary<object, string> objectToReferenceIdMap = new(ReferenceEqualityComparer.Instance);

    public override void AddReference(string referenceId, object value)
    {
        if (!referenceIdToObjectMap.TryAdd(referenceId, value))
        {
            throw new JsonException();
        }
    }

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

    public override object ResolveReference(string referenceId)
    {
        if (!referenceIdToObjectMap.TryGetValue(referenceId, out object? value))
        {
            throw new JsonException();
        }

        return value;
    }
}
