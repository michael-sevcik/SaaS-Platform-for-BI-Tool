// <copyright file="SourceEntitiyConvertor.cs" company="Merica s.r.o.">
// Copyright © Merica
// </copyright>

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SqlViewGeneratorTests")]

namespace SqlViewGenerator.MappingParser;

using JsonModel;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Converter for the <see cref="ISourceEntity"/> implementations.
/// </summary>
internal class SourceEntitiyConvertor : JsonConverter<ISourceEntity>
{
    /// <summary>
    /// Source entity types by their names used for JSON parsing.
    /// </summary>
    private readonly IReadOnlyDictionary<string, Type> sourceEntityTypesByNames;

    /// <summary>
    /// Source entity type names by the appropriate types used for JSON writing.
    /// </summary>
    private readonly IReadOnlyDictionary<Type, string> sourceEntityNamesByType;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceEntitiyConvertor"/> class.
    /// </summary>
    /// <param name="sourceTypesByName">The recognized source entity types with their names.</param>
    public SourceEntitiyConvertor(IEnumerable<KeyValuePair<string, Type>> sourceTypesByName)
    {
        this.sourceEntityTypesByNames = new Dictionary<string, Type>(sourceTypesByName);
        this.sourceEntityNamesByType = this.sourceEntityTypesByNames.ToDictionary(p => p.Value, p => p.Key);
    }

    /// <inheritdoc/>
    public override bool CanConvert(Type typeToConvert)
        => typeof(ISourceEntity) == typeToConvert;

    /// <inheritdoc/>
    public override ISourceEntity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var typeResolverReader = reader;
        using var doc = JsonDocument.ParseValue(ref typeResolverReader);


        if (doc.RootElement.TryGetProperty("$ref", out var refProperty))
        {
            // There has to be a instance of SourceEntityReferenceHandler
            var referenceResolver = options.ReferenceHandler!.CreateResolver();
            reader = typeResolverReader;
            return (ISourceEntity)referenceResolver.ResolveReference(
                refProperty.GetString() ?? throw new JsonException());
        }

        string typeValue = doc.RootElement.GetProperty("type").GetString() ?? throw new JsonException();
        if (!this.sourceEntityTypesByNames.TryGetValue(typeValue!, out var type))
        {
            throw new JsonException($"\"{typeValue}\"is not a recognized type name.");
        }

        var entity = (ISourceEntity?)JsonSerializer.Deserialize(ref reader, type, options);

        return entity;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ISourceEntity value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // Write the type info
        var type = value.GetType();
        writer.WriteString("$type", type.FullName);

        // Get the source entity body
        var sourceEntityBody = JsonDocument.Parse(JsonSerializer.Serialize(value, type, options));

        // Write the source entity body
        foreach (var element in sourceEntityBody.RootElement.EnumerateObject())
        {
            element.WriteTo(writer);
        }

        writer.WriteEndObject();
    }
}
