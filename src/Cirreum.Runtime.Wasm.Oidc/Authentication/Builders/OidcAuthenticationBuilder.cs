namespace Cirreum.Runtime.Authentication.Builders;

using Microsoft.Extensions.DependencyInjection;

public sealed class OidcAuthenticationBuilder(
	IServiceCollection services
) : IOidcAuthenticationBuilder {
	/// <summary>
	/// Gets the service collection.
	/// </summary>
	public IServiceCollection Services { get; } = services;
}