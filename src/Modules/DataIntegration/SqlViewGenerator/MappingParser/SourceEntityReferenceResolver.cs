using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

[assembly: InternalsVisibleTo("SqlViewGeneratorTests")]

namespace SqlViewGenerator.MappingParser;

internal class SourceEntityReferenceResolver : ReferenceResolver
{
    private uint referenceCount;
    private readonly Dictionary<string, object> referenceIdToObjectMap = new ();
    private readonly Dictionary<object, string> objectToReferenceIdMap = new (ReferenceEqualityComparer.Instance);

    public override void AddReference(string referenceId, object value)
    {
        if (!this.referenceIdToObjectMap.TryAdd(referenceId, value))
        {
            throw new JsonException();
        }
    }

    public override string GetReference(object value, out bool alreadyExists)
    {
        if (this.objectToReferenceIdMap.TryGetValue(value, out string? referenceId))
        {
            alreadyExists = true;
        }
        else
        {
            this.referenceCount++;
            referenceId = this.referenceCount.ToString();
            this.objectToReferenceIdMap.Add(value, referenceId);
            alreadyExists = false;
        }

        return referenceId;
    }

    public override object ResolveReference(string referenceId)
    {
        if (!this.referenceIdToObjectMap.TryGetValue(referenceId, out object? value))
        {
            throw new JsonException();
        }

        return value;
    }
}
