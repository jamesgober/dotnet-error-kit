using DotNetErrorKit.Abstractions;

namespace DotNetErrorKit;

/// <summary>
/// Represents a key/value context entry attached to an error.
/// </summary>
public sealed class ErrorContext : IErrorContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorContext"/> class.
    /// </summary>
    /// <param name="key">The context key.</param>
    /// <param name="value">The context value.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> or <paramref name="value"/> are invalid.</exception>
    public ErrorContext(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Context key must not be null or whitespace.", nameof(key));
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Context value must not be null or whitespace.", nameof(value));
        }

        Key = key;
        Value = value;
    }

    /// <inheritdoc />
    public string Key { get; }

    /// <inheritdoc />
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => $"{Key}={Value}";
}
