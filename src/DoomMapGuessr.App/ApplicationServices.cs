using System;

using Microsoft.Extensions.DependencyInjection;


namespace DoomMapGuessr
{

	/// <summary>
	/// Helper class for getting required services (DI).
	/// </summary>
	public static class ApplicationServices
	{

		/// <summary>
		/// The Service Provider.
		/// </summary>
		public static IServiceProvider Root { get; internal set; } = null!;

		/// <summary>
		/// Gets a required service.
		/// </summary>
		/// <remarks>Type constraint is <c>notnull</c></remarks>
		/// <typeparam name="T">The type of service</typeparam>
		/// <returns>The service</returns>
		public static T Get<T>()
			where T : notnull =>
			Root.GetRequiredService<T>();

	}

}
