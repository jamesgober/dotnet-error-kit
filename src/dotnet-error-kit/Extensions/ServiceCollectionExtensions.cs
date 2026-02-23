using DotNetErrorKit.Abstractions;
using DotNetErrorKit.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace DotNetErrorKit.Extensions;

/// <summary>
/// Registers error kit services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds error kit services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">The optional configuration action.</param>
    /// <returns>The service collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
    public static IServiceCollection AddErrorKit(
        this IServiceCollection services,
        Action<ErrorKitOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (configure is not null)
        {
            services.Configure(configure);
        }
        else
        {
            services.AddOptions<ErrorKitOptions>();
        }

        services.TryAddSingleton(sp => sp.GetRequiredService<IOptions<ErrorKitOptions>>().Value);
        services.TryAddSingleton<IErrorRegistry>(sp => sp.GetRequiredService<ErrorKitOptions>().Registry);
        services.TryAddSingleton<IAsyncErrorRegistry>(sp => sp.GetRequiredService<ErrorKitOptions>().AsyncRegistry);
        services.TryAddSingleton<IErrorHub>(sp => sp.GetRequiredService<ErrorKitOptions>().Hub);
        services.TryAddSingleton<IErrorFactory>(sp => sp.GetRequiredService<ErrorKitOptions>().Factory);
        services.TryAddSingleton<IAsyncErrorFactory>(sp => sp.GetRequiredService<ErrorKitOptions>().AsyncFactory);
        services.TryAddSingleton<IErrorExceptionBridge>(sp => sp.GetRequiredService<ErrorKitOptions>().ExceptionBridge);
        services.TryAddSingleton<IErrorReporter, ErrorReporter>();

        return services;
    }
}
