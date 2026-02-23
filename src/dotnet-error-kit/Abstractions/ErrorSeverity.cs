namespace DotNetErrorKit.Abstractions;

/// <summary>
/// Defines the severity of an error.
/// </summary>
public enum ErrorSeverity
{
    /// <summary>
    /// Informational event that does not indicate a failure.
    /// </summary>
    Info,
    /// <summary>
    /// Indicates a recoverable condition that requires attention.
    /// </summary>
    Warning,
    /// <summary>
    /// Indicates a failure that should be handled by the caller.
    /// </summary>
    Error,
    /// <summary>
    /// Indicates a critical failure that impacts stability or availability.
    /// </summary>
    Critical,
    /// <summary>
    /// Indicates a fatal failure that prevents continued operation.
    /// </summary>
    Fatal
}
