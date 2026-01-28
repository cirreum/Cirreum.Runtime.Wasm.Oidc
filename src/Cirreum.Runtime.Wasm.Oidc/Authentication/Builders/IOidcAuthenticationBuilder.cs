namespace Cirreum.Runtime.Authentication.Builders;

/// <summary>
/// Defines a builder for configuring OpenID Connect (OIDC) authentication and user profile enrichment.
/// </summary>
/// <remarks>Implementations of this interface allow customization of OIDC authentication flows and the enrichment
/// of user profiles with additional claims or data. This interface extends IUserProfileEnrichmentBuilder to provide a
/// unified configuration experience for authentication and profile enrichment scenarios.</remarks>
public interface IOidcAuthenticationBuilder : IUserProfileEnrichmentBuilder {
	// Marker interface for OIDC authentication builder
}