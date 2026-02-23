using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotNetErrorKit.ProblemDetails;

/// <summary>
/// Provides JSON serialization helpers for <see cref="ErrorProblemDetails"/>.
/// </summary>
public static class ErrorProblemDetailsJson
{
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };

    /// <summary>
    /// Serializes the specified Problem Details payload to JSON.
    /// </summary>
    /// <param name="details">The Problem Details payload.</param>
    /// <param name="options">Optional JSON serializer options.</param>
    /// <returns>The serialized JSON string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="details"/> is null.</exception>
    public static string Serialize(ErrorProblemDetails details, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(details);
        return JsonSerializer.Serialize(details, options ?? DefaultOptions);
    }

    /// <summary>
    /// Deserializes a JSON payload into <see cref="ErrorProblemDetails"/>.
    /// </summary>
    /// <param name="json">The JSON payload.</param>
    /// <param name="options">Optional JSON serializer options.</param>
    /// <returns>The deserialized Problem Details payload.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="json"/> is null or whitespace.</exception>
    public static ErrorProblemDetails Deserialize(string json, JsonSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentException("JSON payload must not be null or whitespace.", nameof(json));
        }

        var result = JsonSerializer.Deserialize<ErrorProblemDetails>(json, options ?? DefaultOptions);
        return result ?? throw new JsonException("Failed to deserialize Problem Details payload.");
    }

    /// <summary>
    /// Writes the specified Problem Details payload to a JSON writer.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="details">The Problem Details payload.</param>
    /// <param name="options">Optional JSON serializer options.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="writer"/> or <paramref name="details"/> are null.</exception>
    public static void Write(Utf8JsonWriter writer, ErrorProblemDetails details, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(details);
        JsonSerializer.Serialize(writer, details, options ?? DefaultOptions);
    }
}
