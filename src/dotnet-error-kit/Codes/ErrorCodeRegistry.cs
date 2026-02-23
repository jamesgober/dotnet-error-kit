using System.Collections.Concurrent;
using System.Threading;
using DotNetErrorKit.Abstractions;

namespace DotNetErrorKit.Codes;

/// <summary>
/// Provides a thread-safe registry for error codes.
/// </summary>
public sealed class ErrorCodeRegistry : IErrorRegistry, IAsyncErrorRegistry
{
    private readonly ConcurrentDictionary<string, IErrorCode> _codes = new(StringComparer.Ordinal);

    /// <summary>
    /// Gets the shared registry instance.
    /// </summary>
    public static ErrorCodeRegistry Shared { get; } = new();

    /// <inheritdoc />
    public int Count => _codes.Count;

    /// <inheritdoc />
    public void Register(IErrorCode errorCode)
    {
        if (!TryRegister(errorCode))
        {
            throw new InvalidOperationException($"Error code '{errorCode.Value}' is already registered.");
        }
    }

    /// <inheritdoc />
    public bool TryRegister(IErrorCode errorCode)
    {
        ArgumentNullException.ThrowIfNull(errorCode);

        if (string.IsNullOrWhiteSpace(errorCode.Value))
        {
            throw new ArgumentException("Error code value must not be null or whitespace.", nameof(errorCode));
        }

        return _codes.TryAdd(errorCode.Value, errorCode);
    }

    /// <inheritdoc />
    public bool TryGet(string code, out IErrorCode? errorCode)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Error code must not be null or whitespace.", nameof(code));
        }

        return _codes.TryGetValue(code, out errorCode);
    }

    /// <inheritdoc />
    public ValueTask<bool> TryRegisterAsync(IErrorCode errorCode, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return ValueTask.FromResult(TryRegister(errorCode));
    }

    /// <inheritdoc />
    public ValueTask<IErrorCode?> TryGetAsync(string code, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (TryGet(code, out var errorCode))
        {
            return ValueTask.FromResult<IErrorCode?>(errorCode);
        }

        return ValueTask.FromResult<IErrorCode?>(null);
    }
}
