namespace Cirreum.Runtime.Authentication;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

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
	IEnumerable<IClaimsExtender>? claimsExtenders = null,
	IEnumerable<IAuthenticationPostProcessor>? postProcessors = null
) : CommonClaimsPrincipalFactory<RemoteUserAccount>(
		logger,
		serviceProvider,
		accessor ?? throw new ArgumentNullException(nameof(accessor)),
		claimsExtenders,
		postProcessors) {

	/// <summary>
	/// Maps identity claims from the OIDC provider.
	/// </summary>
	/// <remarks>
	/// The <see cref="RemoteUserAccount"/> already contains claims from the ID token,
	/// including roles when the provider is configured to emit them. Custom claim
	/// remapping (e.g. <c>customRoles</c> → <c>roles</c>) is handled by registered
	/// <see cref="IClaimsExtender"/> instances in the base class pipeline.
	/// </remarks>
	protected override ValueTask MapIdentityAsync(ClaimsIdentity identity, RemoteUserAccount account) =>
		ValueTask.CompletedTask;

}
