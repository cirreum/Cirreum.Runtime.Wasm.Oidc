# Cirreum.Runtime.Wasm.Oidc

[![NuGet Version](https://img.shields.io/nuget/v/Cirreum.Runtime.Wasm.Oidc.svg?style=flat-square&labelColor=1F1F1F&color=003D8F)](https://www.nuget.org/packages/Cirreum.Runtime.Wasm.Oidc/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Cirreum.Runtime.Wasm.Oidc.svg?style=flat-square&labelColor=1F1F1F&color=003D8F)](https://www.nuget.org/packages/Cirreum.Runtime.Wasm.Oidc/)
[![GitHub Release](https://img.shields.io/github/v/release/cirreum/Cirreum.Runtime.Wasm.Oidc?style=flat-square&labelColor=1F1F1F&color=FF3B2E)](https://github.com/cirreum/Cirreum.Runtime.Wasm.Oidc/releases)
[![License](https://img.shields.io/github/license/cirreum/Cirreum.Runtime.Wasm.Oidc?style=flat-square&labelColor=1F1F1F&color=F2F2F2)](https://github.com/cirreum/Cirreum.Runtime.Wasm.Oidc/blob/main/LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-003D8F?style=flat-square&labelColor=1F1F1F)](https://dotnet.microsoft.com/)

**Seamless OIDC authentication for Blazor WebAssembly applications in the Cirreum ecosystem**

## Overview

**Cirreum.Runtime.Wasm.Oidc** provides OpenID Connect (OIDC) authentication support for Blazor WebAssembly applications built on the Cirreum framework. It extends Microsoft's WebAssembly authentication with enhanced claims handling, session monitoring, and application user management.

## Features

- **OIDC Authentication** - Full OpenID Connect support for Blazor WebAssembly applications
- **Enhanced Claims Processing** - Custom claims principal factory with JWT token parsing and role mapping
- **Session Monitoring** - Built-in support for user activity tracking and session timeout management
- **Application User Integration** - Seamless integration with custom application user types and loaders
- **Authorization Policies** - Pre-configured standard authorization policies (Standard, Internal, Agent, Manager, Admin)
- **Fluent Configuration API** - Intuitive builder pattern for authentication setup

## Installation

```bash
dotnet add package Cirreum.Runtime.Wasm.Oidc
```

## Usage

### Basic Setup

```csharp
builder.AddOidcAuth(options =>
{
    options.Authority = "https://your-identity-provider.com";
    options.ClientId = "your-client-id";
    options.ResponseType = "code";
    options.DefaultScopes.Add("openid");
    options.DefaultScopes.Add("profile");
});
```

### With Custom Claims Extender

```csharp
builder.AddOidcAuth<MyCustomClaimsExtender>(options =>
{
    options.Authority = "https://your-identity-provider.com";
    options.ClientId = "your-client-id";
});
```

### Adding Session Monitoring

```csharp
builder.AddOidcAuth(options => { /* ... */ })
    .AddSessionMonitoring(sessionOptions =>
    {
        sessionOptions.IdleTimeout = TimeSpan.FromMinutes(30);
        sessionOptions.SessionTimeout = TimeSpan.FromHours(8);
    });
```

### Adding Application User Support

```csharp
builder.AddOidcAuth(options => { /* ... */ })
    .AddApplicationUser<MyApplicationUser, MyApplicationUserLoader>();
```

## Architecture

The library is built on top of:
- `Cirreum.Runtime.Client` - Core client runtime functionality
- `Microsoft.AspNetCore.Components.WebAssembly.Authentication` - Microsoft's Blazor WASM authentication
- `Microsoft.IdentityModel.JsonWebTokens` - JWT token handling

## Contribution Guidelines

1. **Be conservative with new abstractions**  
   The API surface must remain stable and meaningful.

2. **Limit dependency expansion**  
   Only add foundational, version-stable dependencies.

3. **Favor additive, non-breaking changes**  
   Breaking changes ripple through the entire ecosystem.

4. **Include thorough unit tests**  
   All primitives and patterns should be independently testable.

5. **Document architectural decisions**  
   Context and reasoning should be clear for future maintainers.

6. **Follow .NET conventions**  
   Use established patterns from Microsoft.Extensions.* libraries.

## Versioning

Cirreum.Runtime.Wasm.Oidc follows [Semantic Versioning](https://semver.org/):

- **Major** - Breaking API changes
- **Minor** - New features, backward compatible
- **Patch** - Bug fixes, backward compatible

Given its foundational role, major version bumps are rare and carefully considered.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Cirreum Foundation Framework**  
*Layered simplicity for modern .NET*