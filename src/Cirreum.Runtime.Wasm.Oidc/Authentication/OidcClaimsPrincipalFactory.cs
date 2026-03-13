namespace Cirreum.Runtime.Authentication;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Logging;

/// <summary>
/// OIDC-specific <see cref="CommonClaimsPrincipalFactory{TAccount}"/> implementation
/// for Blazor WebAssembly clients.
/// </summary>
/// <remarks>
/// <para>
/// Client-side role claims (used by <c>AuthorizeView</c>, <c>[Authorize(Roles)]</c>,
/// and <c>AuthenticationStateProvider</c>) flow through one of two paths — no access
/// token round-trip is needed:
/// </para>
/// <list type="bullet">
///   <item>
///     <b>ID token roles</b> — Standard OIDC providers (Auth0, Okta, Entra) can include
///     roles in the ID token. These arrive in the <see cref="RemoteUserAccount"/> and are
///     mapped automatically by the base class.
///   </item>
///   <item>
///     <b>Custom claim remapping</b> — For providers that use non-standard claim types
///     (e.g. <c>customRoles</c> from Entra External ID), register an
///     <see cref="IClaimsExtender"/> via <c>AddOidcAuth&lt;TClaimsExtender&gt;()</c>
///     to remap them during Phase 1.
///   </item>
/// </list>
/// </remarks>
internal sealed class OidcClaimsPrincipalFactory(
	IAccessTokenProviderAccessor accessor,
	IServiceProvider serviceProvider,
	ILogger<OidcClaimsPrincipalFactory> logger,
	IEnumerable<IClaimsExtender>? claimsExtenders = null
) : CommonClaimsPrincipalFactory<RemoteUserAccount>(
		logger,
		serviceProvider,
		accessor ?? throw new ArgumentNullException(nameof(accessor)),
		claimsExtenders);