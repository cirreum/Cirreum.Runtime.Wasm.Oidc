namespace Cirreum.Runtime;

using Cirreum.Authorization;
using Cirreum.Components;
using Cirreum.Runtime.Authentication;
using Cirreum.Runtime.Authentication.Builders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;

public static class HostingExtensions {

	/// <summary>
	/// Adds standard Oidc Authentication.
	/// </summary>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="configure">Configure the <see cref="OidcProviderOptions"/>.</param>
	/// <param name="authorization">
	/// Optional callback to add additional policies. 
	/// Default policies already included are: 
	/// <see cref="AuthorizationPolicies.Standard"/>, 
	/// <see cref="AuthorizationPolicies.StandardInternal"/>,
	/// <see cref="AuthorizationPolicies.StandardAgent"/>, 
	/// <see cref="AuthorizationPolicies.StandardManager"/> and 
	/// <see cref="AuthorizationPolicies.StandardAdmin"/></param>
	/// <param name="roleClaimType">The custom role claim type. Default: roles</param>
	/// <param name="nameClaimType">The custom name claim type. Default: name</param>
	/// <returns>The <see cref="IUserProfileEnrichmentBuilder"/> to support optional profile enrichment.</returns>
	public static IOidcAuthenticationBuilder AddOidcAuth(this IClientDomainApplicationBuilder builder,
		Action<OidcProviderOptions> configure,
		Action<AuthorizationOptions>? authorization = null,
		string roleClaimType = "roles",
		string nameClaimType = "name") {

		//
		// Authentication
		//
		builder.Services
			.AddOidcAuthentication(o => {
				o.UserOptions.NameClaim = nameClaimType;
				o.UserOptions.RoleClaim = roleClaimType;
				configure(o.ProviderOptions);
			})
			.AddAccountClaimsPrincipalFactory<OidcClaimsPrincipalFactory>();

		//
		// Authorization
		//
		builder.AddDefaultAuthorization(authorization);

		//
		// Return Enrichment Builder
		//
		return new OidcAuthenticationBuilder(builder.Services);

	}

	/// <summary>
	/// Adds standard Oidc Authentication and registers the defined <typeparamref name="TClaimsExtender"/>
	/// as a scoped service.
	/// </summary>
	/// <typeparam name="TClaimsExtender">The claims extender to register</typeparam>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="configure">Configure the <see cref="OidcProviderOptions"/>.</param>
	/// <param name="authorization">
	/// Optional callback to add additional policies. 
	/// Default policies already included are: 
	/// <see cref="AuthorizationPolicies.Standard"/>, 
	/// <see cref="AuthorizationPolicies.StandardInternal"/>,
	/// <see cref="AuthorizationPolicies.StandardAgent"/>, 
	/// <see cref="AuthorizationPolicies.StandardManager"/> and 
	/// <see cref="AuthorizationPolicies.StandardAdmin"/></param>
	/// <param name="roleClaimType">The custom role claim type. Default: roles</param>
	/// <param name="nameClaimType">The custom name claim type. Default: name</param>
	/// <returns>The <see cref="IUserProfileEnrichmentBuilder"/> to support optional profile enrichment.</returns>
	public static IOidcAuthenticationBuilder AddOidcAuth<TClaimsExtender>(this IClientDomainApplicationBuilder builder,
		Action<OidcProviderOptions> configure,
		Action<AuthorizationOptions>? authorization = null,
		string roleClaimType = "roles",
		string nameClaimType = "name")
		where TClaimsExtender : class, IClaimsExtender {

		builder.Services.AddScoped<IClaimsExtender, TClaimsExtender>();

		return builder.AddOidcAuth(configure, authorization, roleClaimType, nameClaimType);

	}

	/// <summary>
	/// Adds dynamic OIDC authentication resolved at runtime from tenant configuration.
	/// </summary>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="authorization">
	/// Optional callback to add additional authorization policies.
	/// Default policies are automatically included.
	/// </param>
	/// <param name="roleClaimType">The claim type for roles. Default: "roles"</param>
	/// <param name="nameClaimType">The claim type for name. Default: "name"</param>
	/// <returns>
	/// An <see cref="IOidcAuthenticationBuilder"/> for optionally adding profile enrichment.
	/// </returns>
	/// <remarks>
	/// <para>
	/// This method reads tenant configuration from <c>window.tenantAuthConfig</c>,
	/// which is populated by the loader when <c>auth-type="dynamic"</c> is set.
	/// </para>
	/// <para>
	/// The loader fetches configuration from the URL specified in <c>auth-type-url</c>,
	/// replacing <c>{tenant}</c> with the tenant slug extracted from the URL path.
	/// </para>
	/// <para>
	/// Works with any OIDC-compliant identity provider including:
	/// </para>
	/// <list type="bullet">
	///   <item>Okta</item>
	///   <item>Auth0</item>
	///   <item>Ping Identity</item>
	///   <item>Keycloak</item>
	///   <item>IdentityServer / Duende</item>
	///   <item>Any other OIDC-compliant provider</item>
	/// </list>
	/// </remarks>
	/// <example>
	/// <code>
	/// // Program.cs
	/// var builder = DomainApplication.CreateBuilder(args);
	/// 
	/// builder.AddDynamicAuth();
	/// 
	/// await builder.BuildAndRunAsync&lt;MyDomain&gt;();
	/// </code>
	/// </example>
	/// <exception cref="InvalidOperationException">
	/// Thrown when:
	/// <list type="bullet">
	///   <item>Tenant configuration is not found (ensure <c>auth-type="dynamic"</c> is set)</item>
	///   <item>Required <see cref="TenantAuthConfig.Authority"/> is missing</item>
	///   <item>Required <see cref="TenantAuthConfig.ClientId"/> is missing</item>
	/// </list>
	/// </exception>
	public static IOidcAuthenticationBuilder AddDynamicAuth(
		this IClientDomainApplicationBuilder builder,
		Action<AuthorizationOptions>? authorization = null,
		string roleClaimType = "roles",
		string nameClaimType = "name") {

		var config = DynamicAuthInterop.GetTenantAuthConfig() ?? throw new InvalidOperationException(
				"Tenant auth configuration not found. " +
				"Ensure auth-type=\"dynamic\" and auth-type-url are set in the loader script, " +
				"and the auth configuration endpoint is accessible.");

		if (string.IsNullOrEmpty(config.Authority)) {
			throw new InvalidOperationException(
				"Tenant auth configuration is missing required Authority for OIDC.");
		}

		if (string.IsNullOrEmpty(config.ClientId)) {
			throw new InvalidOperationException(
				"Tenant auth configuration is missing required ClientId.");
		}

		return builder.AddOidcAuth(
			configure: options => {
				options.Authority = config.Authority;
				options.ClientId = config.ClientId;
				options.ResponseType = config.ResponseType ?? "code";

				if (config.Scopes is { Count: > 0 }) {
					foreach (var scope in config.Scopes) {
						options.DefaultScopes.Add(scope);
					}
				}
			},
			authorization: authorization,
			roleClaimType: roleClaimType,
			nameClaimType: nameClaimType);
	}

	/// <summary>
	/// Adds dynamic OIDC authentication with a custom claims extender.
	/// </summary>
	/// <typeparam name="TClaimsExtender">
	/// The claims extender type to register for custom claims processing.
	/// </typeparam>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="authorization">
	/// Optional callback to add additional authorization policies.
	/// </param>
	/// <param name="roleClaimType">The claim type for roles. Default: "roles"</param>
	/// <param name="nameClaimType">The claim type for name. Default: "name"</param>
	/// <returns>
	/// An <see cref="IOidcAuthenticationBuilder"/> for optionally adding profile enrichment.
	/// </returns>
	/// <remarks>
	/// Use this overload when you need to transform or extend claims from the IdP,
	/// for example to map provider-specific role claims to your application's role format.
	/// </remarks>
	/// <example>
	/// <code>
	/// // Program.cs
	/// var builder = DomainApplication.CreateBuilder(args);
	/// 
	/// builder.AddDynamicAuth&lt;OktaClaimsExtender&gt;();
	/// 
	/// await builder.BuildAndRunAsync&lt;MyDomain&gt;();
	/// </code>
	/// </example>
	public static IOidcAuthenticationBuilder AddDynamicAuth<TClaimsExtender>(
		this IClientDomainApplicationBuilder builder,
		Action<AuthorizationOptions>? authorization = null,
		string roleClaimType = "roles",
		string nameClaimType = "name")
		where TClaimsExtender : class, IClaimsExtender {

		builder.Services.AddScoped<IClaimsExtender, TClaimsExtender>();

		return builder.AddDynamicAuth(authorization, roleClaimType, nameClaimType);
	}

}