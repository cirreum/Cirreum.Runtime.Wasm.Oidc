# Cirreum.Runtime.Wasm.Oidc 2.0.0 — the wrapper follows the bootstrap client

## Why this release exists

`Cirreum.Runtime.Wasm` 2.0.0 removed the client-side application-user resolver — the path that
could never serve a disabled caller, because reading the record that says "disabled" required
not being disabled. This package's two wrapper verbs forwarded to that removed surface; they
go with it, deliberately as a compile error rather than a documentation note.

## What's new

**`AddApplicationUser<TUser>(Uri)`** on `IOidcAuthenticationBuilder`:

```csharp
builder.AddOidcAuth(options => { /* ... */ })
    .AddApplicationUser<MyUser>(new Uri("https://api.example.com/"));
```

Works identically after `AddDynamicAuth()`. The framework fetches the caller's own record from
the server's bootstrap endpoint during initialization — authentication-only, so it reaches
disabled callers and `ViewState.Disabled` renders for the first time. Apps delete their
resolver class and its caching apparatus.

## Compatibility

Breaking: the two `AddApplicationUserResolver` wrapper verbs are removed. See
`MIGRATION-v2.md` (wrapper delta) and `Cirreum.Runtime.Wasm`'s `MIGRATION-v2.md` (the full
walkthrough). Everything else — `AddOidcAuth` / `AddDynamicAuth` composition, claims
factories/extenders — is unchanged.

## See also

- `docs/MIGRATION-v2.md`
- `Cirreum.Runtime.Wasm` 2.0.0 release notes — the bootstrap-client design
