using System.Reflection;
using System.Runtime.CompilerServices;
using DotNetErrorKit.Abstractions;

namespace DotNetErrorKit.Codes;

/// <summary>
/// Provides registry extensions for registering error categories.
/// </summary>
public static class ErrorRegistryExtensions
{
    /// <summary>
    /// Registers all error codes defined on the specified category.
    /// </summary>
    /// <typeparam name="TCategory">The error category type.</typeparam>
    /// <param name="registry">The registry to update.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="registry"/> is null.</exception>
    public static void RegisterCategory<TCategory>(this IErrorRegistry registry)
        where TCategory : ErrorCategory
    {
        RegisterCategory(registry, typeof(TCategory));
    }

    /// <summary>
    /// Registers all error codes defined on the specified category.
    /// </summary>
    /// <param name="registry">The registry to update.</param>
    /// <param name="categoryType">The category type.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="registry"/> or <paramref name="categoryType"/> are null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="categoryType"/> is not an error category.</exception>
    public static void RegisterCategory(this IErrorRegistry registry, Type categoryType)
    {
        ArgumentNullException.ThrowIfNull(registry);
        ArgumentNullException.ThrowIfNull(categoryType);

        if (!typeof(ErrorCategory).IsAssignableFrom(categoryType))
        {
            throw new ArgumentException("Category type must derive from ErrorCategory.", nameof(categoryType));
        }

        RuntimeHelpers.RunClassConstructor(categoryType.TypeHandle);

        var fields = categoryType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        for (var i = 0; i < fields.Length; i++)
        {
            var field = fields[i];
            if (!typeof(IErrorCode).IsAssignableFrom(field.FieldType))
            {
                continue;
            }

            var value = field.GetValue(null);
            if (value is not IErrorCode errorCode)
            {
                throw new InvalidOperationException($"Error code field '{field.Name}' on '{categoryType.Name}' is null.");
            }

            registry.Register(errorCode);
        }
    }
}
