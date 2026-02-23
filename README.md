<div align="center">
    <img width="120px" height="auto" src="https://raw.githubusercontent.com/jamesgober/jamesgober/main/media/icons/hexagon-3.svg" alt="Triple Hexagon">
    <h1>
        <strong>dotnet-error-kit</strong>
        <sup><br><sub>STRUCTURED ERROR HANDLING</sub></sup>
    </h1>
    <div>
        <a href="https://www.nuget.org/packages/dotnet-error-kit"><img alt="NuGet" src="https://img.shields.io/nuget/v/dotnet-error-kit"></a>
        <span>&nbsp;</span>
        <a href="https://www.nuget.org/packages/dotnet-error-kit"><img alt="NuGet Downloads" src="https://img.shields.io/nuget/dt/dotnet-error-kit?color=%230099ff"></a>
        <span>&nbsp;</span>
        <a href="./LICENSE" title="License"><img alt="License" src="https://img.shields.io/badge/license-Apache--2.0-blue.svg"></a>
        <span>&nbsp;</span>
        <a href="https://github.com/jamesgober/dotnet-error-kit/actions"><img alt="GitHub CI" src="https://github.com/jamesgober/dotnet-error-kit/actions/workflows/ci.yml/badge.svg"></a>
    </div>
</div>
<br>
<p>
    A structured error handling framework for .NET that replaces scattered exception patterns with typed, context-rich errors. Built around RFC 7807 Problem Details, with error codes, context chains, severity levels, and global error hooks — designed to make errors informative, traceable, and actionable.
</p>

## Features

- **Typed Errors** — Define application-specific error types with codes, severity, and metadata
- **Context Chains** — Wrap errors with additional context as they propagate up the call stack
- **RFC 7807 Problem Details** — Automatic serialization to standard Problem Details JSON for API responses
- **Error Codes** — Centralized error code registry with documentation URLs and categorization
- **Severity Levels** — Classify errors as Info, Warning, Error, Critical, or Fatal
- **Global Hooks** — Register error handlers that fire on any error (logging, telemetry, alerting)
- **Result Pattern** — `Result<T>` type for explicit error handling without exceptions
- **Exception Bridge** — Convert between typed errors and exceptions when crossing boundaries

<br>

## Installation

```bash
dotnet add package dotnet-error-kit
```

<br>

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

<br>

## Documentation

- **[API Reference](./docs/API.md)** — Full API documentation and examples

<br>

## Contributing

Contributions welcome. Please:
1. Ensure all tests pass before submitting
2. Follow existing code style and patterns
3. Update documentation as needed

<br>

## Testing

```bash
dotnet test
```

<br>
<hr>
<br>

<div id="license">
    <h2>⚖️ License</h2>
    <p>Licensed under the <b>Apache License</b>, version 2.0 (the <b>"License"</b>); you may not use this software, including, but not limited to the source code, media files, ideas, techniques, or any other associated property or concept belonging to, associated with, or otherwise packaged with this software except in compliance with the <b>License</b>.</p>
    <p>You may obtain a copy of the <b>License</b> at: <a href="http://www.apache.org/licenses/LICENSE-2.0" title="Apache-2.0 License" target="_blank">http://www.apache.org/licenses/LICENSE-2.0</a>.</p>
    <p>Unless required by applicable law or agreed to in writing, software distributed under the <b>License</b> is distributed on an "<b>AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND</b>, either express or implied.</p>
    <p>See the <a href="./LICENSE" title="Software License file">LICENSE</a> file included with this project for the specific language governing permissions and limitations under the <b>License</b>.</p>
    <br>
</div>

<div align="center">
    <h2></h2>
    <sup>COPYRIGHT <small>&copy;</small> 2025 <strong>JAMES GOBER.</strong></sup>
</div>
