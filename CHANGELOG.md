# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

- _No changes yet._

## [1.0.0] 2026-02-23

### Added
- Core error abstractions, error codes, registry, result types, exception bridge, and RFC 7807 Problem Details model.
- Dependency injection registration with options-based configuration.
- Error reporting service and category registration helpers for error codes.
- Async error observers and async reporting support.
- Exception bridge utilities and default system error codes.
- Structured metadata payloads on errors and Problem Details.
- JSON serialization helpers for RFC 7807 Problem Details.
- Async registry and factory abstractions.
- API reference documentation in `docs/API.md`.
- Unit tests covering core error kit behavior.
- Benchmark project for hot-path validation.
- Edge case tests and documentation updates for validation and async cancellation.
- Getting started guide and expanded README examples.

### Changed
- CI workflow now validates packaging with `dotnet pack`.

[Unreleased]: https://github.com/jamesgober/dotnet-error-kit/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/jamesgober/dotnet-error-kit/compare/v0.1.0...v1.0.0
