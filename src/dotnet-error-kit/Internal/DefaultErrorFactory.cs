using System.Threading;
using DotNetErrorKit.Abstractions;

namespace DotNetErrorKit.Internal;

/// <summary>
/// Provides the default error factory implementation.
/// </summary>
internal sealed class DefaultErrorFactory : IErrorFactory, IAsyncErrorFactory
{
    /// <inheritdoc />
    public IError Create(
        IErrorCode code,
        string? message = null,
        IError? innerError = null,
        IReadOnlyList<IErrorContext>? context = null)
    {
        ArgumentNullException.ThrowIfNull(code);

        var error = AppError.From(code, message, innerError);
        if (context is null || context.Count == 0)
        {
            return error;
        }

        var current = error;
        for (var i = 0; i < context.Count; i++)
        {
            current = current.WithContext(context[i]);
        }

        return current;
    }

    /// <inheritdoc />
    public ValueTask<IError> CreateAsync(
        IErrorCode code,
        string? message = null,
        IError? innerError = null,
        IReadOnlyList<IErrorContext>? context = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return ValueTask.FromResult(Create(code, message, innerError, context));
    }
}
