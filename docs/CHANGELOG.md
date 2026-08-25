# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Updated

- Updated NuGet packages.

## [2.0.2] - 2026-08-20

### Updated

- Updated NuGet packages.

## [2.0.1] - 2026-08-04

### Updated

- Re-pinned `Cirreum.Runtime.Wasm` `2.0.0` → `2.0.1` — **take this immediately if you took
  2.0.0**: it makes `AppRouteView` probe the framework bootstrap client (in 2.0.0 the
  `NotProvisioned`/`Disabled` states were unreachable under `AddApplicationUser`), and renames
  its pass-through fragments `NotAuthorizedContent`/`AuthorizingContent` →
  `NotAuthorized`/`Authorizing` (compile-time markup error where the old names were used).

## [2.0.0] - 2026-08-04

### Breaking

- **The two `AddApplicationUserResolver` wrapper verbs removed** (type + factory overloads on
  `IOidcAuthenticationBuilder`), following `Cirreum.Runtime.Wasm` 2.0.0's removal of the
  client-side resolver. Replaced by **`AddApplicationUser<TUser>(Uri)`**: the app supplies its
  user type and its server's base URI, and the framework fetches the caller's own record from
  the server's bootstrap endpoint during initialization — reaching disabled callers, whom the
  old resolver-through-operations path could never serve. See `MIGRATION-v2.md` and the
  `Cirreum.Runtime.Wasm` 2.0.0 migration guide.

### Updated

- Re-pinned `Cirreum.Runtime.Wasm` `1.2.4` → `2.0.0` (Cirreum spine 4.2.0 wave).

## [1.0.52] - 2026-07-31

### Updated

- Updated NuGet packages (Cirreum spine 4.0.1 wave: records-only grant semantics via `Cirreum.Domain` 4.0.1 / `Cirreum.Contracts` 4.0.1; Infrastructure and Runtime repins).

## [1.0.51] - 2026-07-30

### Updated

- Updated NuGet packages — picks up the `Cirreum.Domain` 3.0.0 authorization-enforcement wave
  (fail-open operation-authorization fix + `IPolicyAuthorizer` rename) through the re-pinned
  lower-layer packages; see Cirreum.Domain `MIGRATION-v3.md`.

## [1.0.50] - 2026-07-29

### Updated

- Updated NuGet packages.

## [1.0.49] - 2026-07-28

### Updated

- Updated NuGet packages.

## [1.0.48] - 2026-07-27

### Updated

- Updated NuGet packages.

## [1.0.47] - 2026-07-24

### Updated

- Updated NuGet packages.

## [1.0.46] - 2026-07-24

### Updated

- `Cirreum.Runtime.Wasm` 1.0.52 → 1.1.0. `OidcClaimsPrincipalFactory` inherits the new built-in
  claim processing: provisioned `custom*` claims (`customRoles`, `customName`, …) are
  canonicalized to their native names automatically — `customRoles` aliases to the configured
  `roleClaimType` and JSON-array values split into individual claims for `IsInRole` — and the
  authentication-state publication fixes ship transitively (claim transforms always run,
  publication dedupes on user id + claims-content fingerprint). No API change in this package; an
  `IClaimsExtender` is now needed only for app-specific transformations (e.g. native-vs-minted
  precedence), not for `custom*` remapping.

### Fixed

- `OidcClaimsPrincipalFactory` doc remarks and the README no longer instruct apps to register an
  `IClaimsExtender` to remap `customRoles` — that canonicalization is built into the
  authentication pipeline; extender guidance now covers only the advanced cases.

## [1.0.45] - 2026-07-20

### Updated

- Updated NuGet packages.

## [1.0.44] - 2026-07-19

### Updated

- Updated NuGet packages.

## [1.0.43] - 2026-07-11

### Updated

- Updated NuGet packages (`Cirreum.Runtime.Wasm` 1.0.47 → 1.0.50).

## [1.0.42] - 2026-07-09

### Updated

- Updated NuGet packages.

## [1.0.41] - 2026-07-04

### Updated

- Updated NuGet packages.

## [1.0.40] - 2026-05-10

### Updated

- Updated NuGet packages.

## [1.0.39] - 2026-05-10

### Updated

- Updated NuGet packages.

## [1.0.38] - 2026-05-01

### Updated
- Updated NuGet packages.

