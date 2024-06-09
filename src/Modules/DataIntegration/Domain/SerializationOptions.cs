using System.Text.Json;
using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.Domain;

/// <summary>
/// Represents options for serialization using <see cref="JsonSerializer"/> used in data integration module.
/// </summary>
public static class SerializationOptions
{
    /// <summary>
    /// Gets or sets the default options for serialization.
    /// 
    /// Serialization options are set to: convert property names to camel case and convert enum values to strings.
    /// </summary>
    public static JsonSerializerOptions Default { get; set; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };
}
