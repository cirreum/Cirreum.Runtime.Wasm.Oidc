namespace Cirreum.Runtime.Authentication;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

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

	protected override async ValueTask MapIdentityAsync(ClaimsIdentity identity, RemoteUserAccount account) {

		try {

			var accessTokenResult = await this.TokenProvider.RequestAccessToken();
			if (accessTokenResult.TryGetToken(out var accessToken)) {
				var handler = new JsonWebTokenHandler();
				var jwtToken = handler.ReadJsonWebToken(accessToken.Value);
				MapIssuer(identity, jwtToken);
				MapRoles(identity, jwtToken);
			}

		} catch (AccessTokenNotAvailableException ae) {
			logger.LogError(ae, "Access token failure: {Message}", ae.Message);
		} catch (Exception e) {
			logger.LogError(e, "Access token Error: {Message}", e.Message);
		}

	}

	private static void MapIssuer(ClaimsIdentity identity, JsonWebToken jwtToken) {
		var issuerClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "iss");
		var existingClaim = identity.FindFirst("iss");
		if (existingClaim != null) {
			// Has existing claim
			if (issuerClaim != null && !existingClaim.Value.Equals(issuerClaim.Value, StringComparison.OrdinalIgnoreCase)) {
				// JWT has different issuer - update it
				identity.RemoveClaim(existingClaim);
				identity.AddClaim(issuerClaim);
			}
			// Else - either JWT has no issuer or values match, keep existing
		} else {
			// No existing claim - add JWT issuer or unknown
			identity.AddClaim(
				issuerClaim
				?? throw new SecurityTokenInvalidIssuerException(
					"Issuer claim is missing or invalid."));
		}
	}
	private static void MapRoles(ClaimsIdentity identity, JsonWebToken jwtToken) {
		var roleClaimType = identity.RoleClaimType;
		var existingRoles = identity.FindAll(roleClaimType)
			.Select(c => c.Value)
			.ToHashSet(StringComparer.OrdinalIgnoreCase);

		foreach (var c in jwtToken.Claims) {
			if (string.Equals(c.Type, roleClaimType, StringComparison.OrdinalIgnoreCase)
				&& !existingRoles.Contains(c.Value)) {
				identity.AddClaim(new Claim(roleClaimType, c.Value));
			}
		}

	}

}