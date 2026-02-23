using DotNetErrorKit.Abstractions;
using DotNetErrorKit.Codes;
using DotNetErrorKit.Internal;

namespace DotNetErrorKit;

/// <summary>
/// Provides configuration for error kit services.
/// </summary>
public sealed class ErrorKitOptions
{
    private static readonly DefaultErrorFactory DefaultFactory = new();

    private IErrorRegistry _registry = ErrorCodeRegistry.Shared;
    private IAsyncErrorRegistry _asyncRegistry = ErrorCodeRegistry.Shared;
    private IErrorHub _hub = new ErrorHub();
    private IErrorFactory _factory = DefaultFactory;
    private IAsyncErrorFactory _asyncFactory = DefaultFactory;
    private IErrorExceptionBridge _exceptionBridge = new ErrorExceptionBridge(SystemErrorCodes.UnhandledException);

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorKitOptions"/> class.
    /// </summary>
    public ErrorKitOptions()
    {
        _registry.TryRegister(SystemErrorCodes.UnhandledException);
    }

    /// <summary>
    /// Gets or sets the error registry.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when a null registry is assigned.</exception>
    public IErrorRegistry Registry
    {
        get => _registry;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _registry = value;
        }
    }

    /// <summary>
    /// Gets or sets the asynchronous error registry.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when a null registry is assigned.</exception>
    public IAsyncErrorRegistry AsyncRegistry
    {
        get => _asyncRegistry;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _asyncRegistry = value;
        }
    }

    /// <summary>
    /// Gets or sets the error hub.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when a null hub is assigned.</exception>
    public IErrorHub Hub
    {
        get => _hub;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _hub = value;
        }
    }

    /// <summary>
    /// Gets or sets the error factory.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when a null factory is assigned.</exception>
    public IErrorFactory Factory
    {
        get => _factory;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _factory = value;
        }
    }

    /// <summary>
    /// Gets or sets the asynchronous error factory.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when a null factory is assigned.</exception>
    public IAsyncErrorFactory AsyncFactory
    {
        get => _asyncFactory;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _asyncFactory = value;
        }
    }

    /// <summary>
    /// Gets or sets the exception bridge.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when a null bridge is assigned.</exception>
    public IErrorExceptionBridge ExceptionBridge
    {
        get => _exceptionBridge;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _exceptionBridge = value;
        }
    }
}
