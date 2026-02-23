using DotNetErrorKit.Abstractions;
using DotNetErrorKit.Codes;

namespace DotNetErrorKit.Internal;

/// <summary>
/// Provides the default exception bridge implementation.
/// </summary>
internal sealed class ErrorExceptionBridge : IErrorExceptionBridge
{
    private readonly IErrorCode _fallbackCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorExceptionBridge"/> class.
    /// </summary>
    /// <param name="fallbackCode">The fallback error code used for non-error exceptions.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fallbackCode"/> is null.</exception>
    public ErrorExceptionBridge(IErrorCode fallbackCode)
    {
        ArgumentNullException.ThrowIfNull(fallbackCode);
        _fallbackCode = fallbackCode;
    }

    /// <inheritdoc />
    public Exception ToException(IError errorInfo)
    {
        ArgumentNullException.ThrowIfNull(errorInfo);
        return new ErrorException(errorInfo);
    }

    /// <inheritdoc />
    public bool TryGetError(Exception exception, out IError? errorInfo)
    {
        ArgumentNullException.ThrowIfNull(exception);

        if (exception is ErrorException errorException)
        {
            errorInfo = errorException.Error;
            return true;
        }

        errorInfo = null;
        return false;
    }

    /// <inheritdoc />
    public IError FromException(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        if (TryGetError(exception, out var errorInfo))
        {
            return errorInfo!;
        }

        var error = AppError.From(_fallbackCode)
            .WithContext("exceptionType", exception.GetType().FullName ?? "UnknownException");

        return error;
    }
}
