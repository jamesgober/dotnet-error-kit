using System.Collections.Generic;

namespace DotNetErrorKit.Abstractions;

/// <summary>
/// Creates errors with validated inputs.
/// </summary>
public interface IErrorFactory
{
    /// <summary>
    /// Creates a new error instance.
    /// </summary>
    /// <param name="code">The error code metadata.</param>
    /// <param name="message">The error message override.</param>
    /// <param name="innerError">An optional inner error.</param>
    /// <param name="context">Optional context entries.</param>
    /// <returns>The created error.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/> is null.</exception>
    IError Create(IErrorCode code, string? message = null, IError? innerError = null, IReadOnlyList<IErrorContext>? context = null);
}
