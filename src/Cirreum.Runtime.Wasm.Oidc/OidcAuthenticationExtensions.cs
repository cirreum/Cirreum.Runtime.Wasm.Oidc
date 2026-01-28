namespace Cirreum.Runtime;

using Cirreum.Authorization;
using Cirreum.Runtime.Authentication.Builders;

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
	/// Registers application user services with the specified user type and loader implementation.
	/// </summary>
	/// <typeparam name="TApplicationUser">The type of the application user that implements <see cref="IApplicationUser"/>.</typeparam>
	/// <typeparam name="TApplicationUserLoader">The type of the application user loader that implements <see cref="IApplicationUserLoader{TApplicationUser}"/>.</typeparam>
	/// <param name="builder">The <see cref="IOidcAuthenticationBuilder"/> to add services to.</param>
	/// <returns>The <see cref="IOidcAuthenticationBuilder"/> so that additional calls can be chained.</returns>
	public static IOidcAuthenticationBuilder AddApplicationUser<TApplicationUser, TApplicationUserLoader>(
		this IOidcAuthenticationBuilder builder)
		where TApplicationUser : class, IApplicationUser
		where TApplicationUserLoader : class, IApplicationUserLoader<TApplicationUser> {

		// Forward to real method
		builder.Services.AddApplicationUser<TApplicationUser, TApplicationUserLoader>();

		return builder;

	}

	/// <summary>
	/// Registers application user services with a custom loader factory function.
	/// </summary>
	/// <typeparam name="TApplicationUser">The type of the application user that implements <see cref="IApplicationUser"/>.</typeparam>
	/// <param name="builder">The <see cref="IOidcAuthenticationBuilder"/> to add services to.</param>
	/// <param name="loaderFactory">A factory function that creates an instance of <see cref="IApplicationUserLoader{TApplicationUser}"/> using the service provider.</param>
	/// <returns>The <see cref="IOidcAuthenticationBuilder"/> so that additional calls can be chained.</returns>
	public static IOidcAuthenticationBuilder AddApplicationUser<TApplicationUser>(
		this IOidcAuthenticationBuilder builder,
		Func<IServiceProvider, IApplicationUserLoader<TApplicationUser>> loaderFactory)
		where TApplicationUser : class, IApplicationUser {

		// Forward to real method
		builder.Services.AddApplicationUser(loaderFactory);

		return builder;

	}

}