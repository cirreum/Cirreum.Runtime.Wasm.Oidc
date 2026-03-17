namespace Cirreum.Runtime;

using Cirreum.Authorization;
using Cirreum.Runtime.Authentication.Builders;
using Cirreum.Security;

public static class OidcAuthenticationExtensions {

	//
	// Session Monitoring
	//

	/// <summary>
	/// Adds session management to the authentication pipeline, allowing for the configuration
	/// of user activity monitoring and timeout behavior.
	/// </summary>
	/// <param name="builder">The <see cref="IOidcAuthenticationBuilder"/> to which session monitoring is added.</param>
	/// <param name="configure">An optional delegate to configure the <see cref="SessionOptions"/> used to
	/// customize session timeout behavior.</param>
	/// <returns>The <see cref="IOidcAuthenticationBuilder"/> instance, enabling further configuration of the authentication pipeline.</returns>
	/// <remarks>
	/// <para>
	/// This method registers session monitoring services and allows customization of session timeout
	/// behavior through the provided <paramref name="configure"/> delegate. If no configuration is provided, default
	/// options are used.
	/// </para>
	/// </remarks>
	public static IOidcAuthenticationBuilder AddSessionMonitoring(
		this IOidcAuthenticationBuilder builder,
		Action<SessionOptions>? configure) {
		builder.Services.AddSessionMonitoring(configure);
		return builder;
	}


	//
	// Application User
	//

	/// <summary>
	/// Registers the <typeparamref name="TResolver"/> as the
	/// <see cref="IApplicationUserResolver"/> used to resolve application users during initialization.
	/// </summary>
	/// <typeparam name="TResolver">
	/// The resolver implementation that resolves <see cref="IApplicationUser"/> instances
	/// from an external user identifier.
	/// </typeparam>
	/// <param name="builder">The <see cref="IOidcAuthenticationBuilder"/> to add services to.</param>
	/// <returns>The <see cref="IOidcAuthenticationBuilder"/> so that additional calls can be chained.</returns>
	public static IOidcAuthenticationBuilder AddApplicationUserResolver<TResolver>(
		this IOidcAuthenticationBuilder builder)
		where TResolver : class, IApplicationUserResolver {
		builder.Services.AddApplicationUserResolver<TResolver>();
		return builder;
	}

	/// <summary>
	/// Registers an <see cref="IApplicationUserResolver"/> using a custom factory function.
	/// </summary>
	/// <param name="builder">The <see cref="IOidcAuthenticationBuilder"/> to add services to.</param>
	/// <param name="factory">A factory function that creates an <see cref="IApplicationUserResolver"/> instance.</param>
	/// <returns>The <see cref="IOidcAuthenticationBuilder"/> so that additional calls can be chained.</returns>
	public static IOidcAuthenticationBuilder AddApplicationUserResolver(
		this IOidcAuthenticationBuilder builder,
		Func<IServiceProvider, IApplicationUserResolver> factory) {
		builder.Services.AddApplicationUserResolver(factory);
		return builder;
	}

}