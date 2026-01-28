# Cirreum.Runtime.Wasm.Oidc

[![NuGet Version](https://img.shields.io/nuget/v/Cirreum.Runtime.Wasm.Oidc.svg?style=flat-square\&labelColor=1F1F1F\&color=003D8F)](https://www.nuget.org/packages/Cirreum.Runtime.Wasm.Oidc/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Cirreum.Runtime.Wasm.Oidc.svg?style=flat-square\&labelColor=1F1F1F\&color=003D8F)](https://www.nuget.org/packages/Cirreum.Runtime.Wasm.Oidc/)
[![GitHub Release](https://img.shields.io/github/v/release/cirreum/Cirreum.Runtime.Wasm.Oidc?style=flat-square\&labelColor=1F1F1F\&color=FF3B2E)](https://github.com/cirreum/Cirreum.Runtime.Wasm.Oidc/releases)
[![License](https://img.shields.io/github/license/cirreum/Cirreum.Runtime.Wasm.Oidc?style=flat-square\&labelColor=1F1F1F\&color=F2F2F2)](https://github.com/cirreum/Cirreum.Runtime.Wasm.Oidc/blob/main/LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-003D8F?style=flat-square\&labelColor=1F1F1F)](https://dotnet.microsoft.com/)

**Seamless OIDC authentication for Blazor WebAssembly applications in the Cirreum ecosystem**

---

## Overview

**Cirreum.Runtime.Wasm.Oidc** provides OpenID Connect (OIDC) authentication for Blazor WebAssembly applications built on the Cirreum framework.

It supports both:

* **Static OIDC configuration** (traditional single-tenant apps)
* **Dynamic, runtime-resolved OIDC configuration** (multi-tenant / white-label / BYOID scenarios)

The library extends Microsoft’s WASM authentication stack with enhanced claims handling, authorization defaults, session monitoring, and fluent configuration.

---

## Features

* **OIDC Authentication**

  * Full OpenID Connect support for Blazor WebAssembly
  * Works with any OIDC-compliant identity provider

* **Dynamic Tenant Authentication**

  * Resolve OIDC settings at runtime per tenant
  * No compile-time dependency on a specific IdP
  * Supports Okta, Auth0, Ping, Keycloak, Duende, and others

* **Enhanced Claims Processing**

  * Custom claims principal factory
  * Optional claims extenders for provider-specific mapping

* **Authorization Policies**

  * Standard policies included by default:

    * `Standard`
    * `StandardInternal`
    * `StandardAgent`
    * `StandardManager`
    * `StandardAdmin`

* **Session Monitoring**

  * Idle timeout and absolute session lifetime support

* **Application User Integration**

  * Fluent registration of application user loaders

* **Fluent Configuration API**

  * Builder-based chaining for auth-related concerns

---

## Installation

```bash
dotnet add package Cirreum.Runtime.Wasm.Oidc
```

---

## Usage

### Static (Traditional) OIDC Setup

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

---

### Static OIDC with Custom Claims Extender

```csharp
builder.AddOidcAuth<MyClaimsExtender>(options =>
{
    options.Authority = "https://your-identity-provider.com";
    options.ClientId = "your-client-id";
});
```

---

## Dynamic (Tenant-Resolved) OIDC Authentication

`AddDynamicAuth` enables **runtime OIDC configuration**, allowing the application to authenticate users against different identity providers based on the current tenant.

This is ideal for:

* Multi-tenant SaaS
* White-label platforms
* Bring-Your-Own-Identity (BYOID)
* Customer-hosted IdPs

### Program.cs

```csharp
var builder = DomainApplication.CreateBuilder(args);

builder.AddDynamicAuth();

await builder.BuildAndRunAsync<MyDomain>();
```

---

### Dynamic Auth with Claims Extender

```csharp
builder.AddDynamicAuth<OktaClaimsExtender>();
```

---

### How Dynamic Auth Works

1. A lightweight **loader script** runs before Blazor starts
2. The loader fetches tenant-specific OIDC configuration
3. Configuration is written to:

```js
cirreum.tenant
```

4. `AddDynamicAuth()` reads this configuration during startup
5. OIDC authentication is configured dynamically

---

### Loader Contract

To enable dynamic auth, the loader must define:

```html
<script
  src="cirreum-wasm-loader.js"
  auth-type="dynamic"
  auth-type-url="https://auth.example.com/tenants/{tenant}/oidc">
</script>
```

* `{tenant}` is replaced with the tenant slug derived from the URL
* The endpoint must return a valid tenant auth configuration payload

---

### Required Tenant Configuration Fields

```json
{
  "authority": "https://idp.example.com",
  "clientId": "client-id",
  "responseType": "code",
  "scopes": ["openid", "profile", "email"]
}
```

### Validation

`AddDynamicAuth` throws an exception at startup if:

* Tenant configuration is missing
* `Authority` is not defined
* `ClientId` is not defined

This ensures authentication failures are detected early and explicitly.

---

## Adding Session Monitoring

```csharp
builder.AddOidcAuth(options => { /* ... */ })
    .AddSessionMonitoring(session =>
    {
        session.IdleTimeout = TimeSpan.FromMinutes(30);
        session.SessionTimeout = TimeSpan.FromHours(8);
    });
```

Works identically for **static** and **dynamic** authentication.

---

## Adding Application User Support

```csharp
builder.AddOidcAuth(options => { /* ... */ })
    .AddApplicationUser<MyUser, MyUserLoader>();
```

Or with dynamic auth:

```csharp
builder.AddDynamicAuth()
    .AddApplicationUser<MyUser, MyUserLoader>();
```

---

## Architecture

Built on top of:

* `Cirreum.Runtime.Wasm`
* `Microsoft.AspNetCore.Components.WebAssembly.Authentication`
* `Microsoft.IdentityModel.JsonWebTokens`

Dynamic auth is layered cleanly on top of the standard WASM auth pipeline and does not replace or fork Microsoft’s implementation.

---

## Versioning

This package follows **Semantic Versioning**:

* **Major** — Breaking changes
* **Minor** — Backward-compatible features
* **Patch** — Bug fixes

Dynamic authentication was introduced as a **non-breaking additive feature**.

---

## License

MIT License. See [LICENSE](LICENSE).

---

**Cirreum Foundation Framework**
*Layered simplicity for modern .NET*
