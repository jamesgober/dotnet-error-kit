using System.Text.Json.Serialization;
using DotNetErrorKit.Abstractions;

namespace DotNetErrorKit.ProblemDetails;

/// <summary>
/// Represents RFC 7807 Problem Details generated from an error.
/// </summary>
public sealed class ErrorProblemDetails
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorProblemDetails"/> class.
    /// </summary>
    /// <param name="type">The Problem Details type.</param>
    /// <param name="title">The Problem Details title.</param>
    /// <param name="status">The HTTP status code.</param>
    /// <param name="detail">The Problem Details detail message.</param>
    /// <param name="instance">The Problem Details instance.</param>
    /// <param name="extensions">The extensions collection.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="type"/>, <paramref name="title"/>, or <paramref name="detail"/> are invalid.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="extensions"/> is null.</exception>
    [JsonConstructor]
    public ErrorProblemDetails(
        string type,
        string title,
        int? status,
        string detail,
        string? instance,
        IDictionary<string, object?> extensions)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("Problem Details type must not be null or whitespace.", nameof(type));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Problem Details title must not be null or whitespace.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(detail))
        {
            throw new ArgumentException("Problem Details detail must not be null or whitespace.", nameof(detail));
        }

        ArgumentNullException.ThrowIfNull(extensions);

        Type = type;
        Title = title;
        Status = status;
        Detail = detail;
        Instance = instance;
        Extensions = extensions;
    }

    /// <summary>
    /// Gets the Problem Details type.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the Problem Details title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the HTTP status code.
    /// </summary>
    public int? Status { get; }

    /// <summary>
    /// Gets the Problem Details detail message.
    /// </summary>
    public string Detail { get; }

    /// <summary>
    /// Gets the Problem Details instance.
    /// </summary>
    public string? Instance { get; }

    /// <summary>
    /// Gets the extensions collection.
    /// </summary>
    public IDictionary<string, object?> Extensions { get; }

    /// <summary>
    /// Creates a Problem Details payload from an error.
    /// </summary>
    /// <param name="error">The source error.</param>
    /// <param name="status">The optional HTTP status code.</param>
    /// <param name="type">The optional Problem Details type URI.</param>
    /// <param name="instance">The optional Problem Details instance.</param>
    /// <returns>The created Problem Details payload.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="error"/> is null.</exception>
    public static ErrorProblemDetails FromError(
        IError error,
        int? status = null,
        Uri? type = null,
        string? instance = null)
    {
        ArgumentNullException.ThrowIfNull(error);

        var extensions = new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["code"] = error.Code.Value,
            ["severity"] = error.Severity.ToString(),
            ["timestamp"] = error.Timestamp
        };

        if (error.Context.Count > 0)
        {
            var entries = new KeyValuePair<string, string>[error.Context.Count];
            for (var i = 0; i < error.Context.Count; i++)
            {
                var context = error.Context[i];
                entries[i] = new KeyValuePair<string, string>(context.Key, context.Value);
            }

            extensions["context"] = entries;
        }

        if (error.Metadata.Count > 0)
        {
            extensions["metadata"] = error.Metadata;
        }

        if (error.InnerError is not null)
        {
            extensions["inner"] = error.InnerError.Code.Value;
        }

        var resolvedType = type?.ToString()
            ?? error.Code.DocumentationUri?.ToString()
            ?? "about:blank";

        return new ErrorProblemDetails(
            resolvedType,
            error.Code.Description,
            status,
            error.Message,
            instance,
            extensions);
    }
}
