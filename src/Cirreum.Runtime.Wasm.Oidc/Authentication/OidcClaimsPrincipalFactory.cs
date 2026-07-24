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
///     <b>Provisioned <c>custom*</c> claims</b> — Claims minted by a Cirreum Identity
///     provisioner (<c>customRoles</c>, <c>customName</c>, …) are canonicalized to their
///     native names by the base factory automatically: <c>customRoles</c> aliases to the
///     configured role claim type and JSON-array values split into individual claims, so
///     no extender is required. Register an <see cref="IClaimsExtender"/> via
///     <c>AddClaimsExtender&lt;TClaimsExtender&gt;()</c> only for app-specific
///     transformations — e.g. precedence between a native claim and its minted
///     <c>custom*</c> counterpart, or parsing a bespoke claim shape.
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