using System.Diagnostics.CodeAnalysis;
using DotNetErrorKit;
using DotNetErrorKit.Abstractions;

namespace DotNetErrorKit.Results;

/// <summary>
/// Represents the outcome of an operation that returns a value.
/// </summary>
/// <typeparam name="T">The result value type.</typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Static factory methods provide explicit Result<T> creation.")]
public readonly struct Result<T>
{
    private readonly T? _value;
    private readonly IError? _error;

    private Result(T? value)
    {
        _value = value;
        _error = null;
        IsSuccess = true;
    }

    private Result(IError error)
    {
        _value = default;
        _error = error;
        IsSuccess = false;
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
    /// Gets the value for a successful result.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the result represents failure.</exception>
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Result does not contain a value.");

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <returns>A successful result.</returns>
    public static Result<T> Success(T value) => new(value);

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="error">The error to attach.</param>
    /// <returns>A failed result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="error"/> is null.</exception>
    public static Result<T> Failure(IError error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return new Result<T>(error);
    }

    /// <summary>
    /// Attempts to get the value for a successful result.
    /// </summary>
    /// <param name="value">The resulting value.</param>
    /// <returns><c>true</c> when successful; otherwise, <c>false</c>.</returns>
    public bool TryGetValue([MaybeNullWhen(false)] out T value)
    {
        if (IsSuccess)
        {
            value = _value!;
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Converts the result into a non-generic <see cref="Result"/>.
    /// </summary>
    /// <returns>A non-generic result.</returns>
    public Result ToResult() => IsSuccess ? Result.Success() : Result.Failure(_error!);

    /// <inheritdoc />
    public override string ToString() => IsSuccess ? "Success" : $"Failure: {_error?.Code.Value}";

    /// <summary>
    /// Converts a value into a successful result.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator Result<T>(T value) => Success(value);

    /// <summary>
    /// Converts an application error into a failed result.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    public static implicit operator Result<T>(AppError error) => Failure(error);

    /// <summary>
    /// Converts an error exception into a failed result.
    /// </summary>
    /// <param name="exception">The exception to convert.</param>
    public static implicit operator Result<T>(ErrorException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        return Failure(exception.Error);
    }
}
