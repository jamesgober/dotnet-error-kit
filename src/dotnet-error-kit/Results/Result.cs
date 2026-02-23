using DotNetErrorKit;
using DotNetErrorKit.Abstractions;

namespace DotNetErrorKit.Results;

/// <summary>
/// Represents the outcome of an operation without a return value.
/// </summary>
public readonly struct Result
{
    private readonly IError? _error;

    private Result(bool isSuccess, IError? error)
    {
        IsSuccess = isSuccess;
        _error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the result represents success.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result represents failure.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error for a failed result, if any.
    /// </summary>
    public IError? Error => _error;

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns>A successful result.</returns>
    public static Result Success() => new(true, null);

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="error">The error to attach.</param>
    /// <returns>A failed result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="error"/> is null.</exception>
    public static Result Failure(IError error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return new Result(false, error);
    }

    /// <summary>
    /// Throws an <see cref="ErrorException"/> when the result represents failure.
    /// </summary>
    public void ThrowIfFailure()
    {
        if (!IsSuccess)
        {
            throw new ErrorException(_error ?? throw new InvalidOperationException("Failure result did not contain an error."));
        }
    }

    /// <inheritdoc />
    public override string ToString() => IsSuccess ? "Success" : $"Failure: {_error?.Code.Value}";

    /// <summary>
    /// Converts an error into a failed result.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    public static implicit operator Result(AppError error) => Failure(error);

    /// <summary>
    /// Converts an error exception into a failed result.
    /// </summary>
    /// <param name="exception">The exception to convert.</param>
    public static implicit operator Result(ErrorException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        return Failure(exception.Error);
    }
}
