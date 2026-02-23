namespace DotNetErrorKit.Abstractions;

/// <summary>
/// Provides a registry for error codes.
/// </summary>
public interface IErrorRegistry
{
    /// <summary>
    /// Gets the number of registered error codes.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Registers a new error code.
    /// </summary>
    /// <param name="errorCode">The error code to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorCode"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the error code already exists.</exception>
    void Register(IErrorCode errorCode);

    /// <summary>
    /// Attempts to register a new error code.
    /// </summary>
    /// <param name="errorCode">The error code to register.</param>
    /// <returns><c>true</c> when the error code was registered; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorCode"/> is null.</exception>
    bool TryRegister(IErrorCode errorCode);

    /// <summary>
    /// Attempts to get a registered error code by value.
    /// </summary>
    /// <param name="code">The error code value.</param>
    /// <param name="errorCode">The resolved error code.</param>
    /// <returns><c>true</c> when the code is found; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="code"/> is null or whitespace.</exception>
    bool TryGet(string code, out IErrorCode? errorCode);
}
