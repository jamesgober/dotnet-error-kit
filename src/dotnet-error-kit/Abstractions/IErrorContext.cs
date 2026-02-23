namespace DotNetErrorKit.Abstractions;

/// <summary>
/// Represents a key/value context entry attached to an error.
/// </summary>
public interface IErrorContext
{
    /// <summary>
    /// Gets the context key.
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Gets the context value.
    /// </summary>
    string Value { get; }
}
