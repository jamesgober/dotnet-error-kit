# dotnet-error-kit

[![NuGet](https://img.shields.io/nuget/v/JG.ErrorKit?logo=nuget)](https://www.nuget.org/packages/JG.ErrorKit)
[![Downloads](https://img.shields.io/nuget/dt/JG.ErrorKit?color=%230099ff&logo=nuget)](https://www.nuget.org/packages/JG.ErrorKit)
[![License](https://img.shields.io/badge/license-Apache--2.0-blue.svg)](./LICENSE)
[![CI](https://github.com/jamesgober/dotnet-error-kit/actions/workflows/ci.yml/badge.svg)](https://github.com/jamesgober/dotnet-error-kit/actions)

---

A structured error handling framework for .NET that replaces scattered exception patterns with typed, context-rich errors. Built around RFC 7807 Problem Details, with error codes, context chains, severity levels, global error hooks, a Result pattern for explicit error handling, and an exception bridge for boundary crossing.


## Features

- **Typed Errors** — Define application-specific error types with codes, severity, and metadata
- **Context Chains** — Wrap errors with additional context as they propagate up the call stack
- **RFC 7807 Problem Details** — Automatic serialization to standard Problem Details JSON for API responses
- **Error Codes** — Centralized error code registry with documentation URLs and categorization
- **Severity Levels** — Classify errors as Info, Warning, Error, Critical, or Fatal
- **Global Hooks** — Register error handlers that fire on any error (logging, telemetry, alerting)
- **Result Pattern** — `Result<T>` type for explicit error handling without exceptions
- **Exception Bridge** — Convert between typed errors and exceptions when crossing boundaries

## Installation

```bash
dotnet add package JG.ErrorKit
```

## Quick Start

```csharp
// Define application errors
public class UserErrors : ErrorCategory
{
    public static readonly ErrorCode NotFound = new("USER_001", "User not found");
    public static readonly ErrorCode InvalidEmail = new("USER_002", "Invalid email format");
}

// Return typed results
public Result<User> GetUser(string id)
{
    var user = _repo.Find(id);
    if (user is null)
        return AppError.From(UserErrors.NotFound).WithContext("id", id);

    return user;
}
```

## Getting Started

- **Define error codes** and register them with `services.AddErrorKit(...)`.
- **Return typed results** for predictable error handling.
- **Publish errors** to observers for logging or telemetry.
- **Map to RFC 7807** Problem Details when returning API responses.

See **[Getting Started](./docs/GETTING-STARTED.md)** for full walkthroughs.

## Documentation

- **[API Reference](./docs/API.md)** — Full API documentation and examples
- **[Getting Started](./docs/GETTING-STARTED.md)** — Setup guide and common workflows

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

Licensed under the Apache License 2.0. See [LICENSE](./LICENSE) for details.

---

**Ready to get started?** Install via NuGet and check out the [API reference](./docs/API.md).
