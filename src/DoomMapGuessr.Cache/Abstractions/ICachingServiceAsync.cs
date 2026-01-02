using System.Threading.Tasks;


namespace DoomMapGuessr.Cache.Abstractions
{

	/// <summary>
	/// An asynchronous caching service.
	/// </summary>
	public interface ICachingServiceAsync
	{

		/// <summary>
		/// Clears one or multiple cache location (bitwise operations).
		/// </summary>
		/// <param name="target">The target</param>
		Task ClearAsync(CacheTarget target);

		/// <summary>
		/// Gets a cached item async.
		/// </summary>
		/// <param name="key">The key</param>
		/// <typeparam name="T">The type of data to store</typeparam>
		/// <returns>The value</returns>
		Task<T> GetAsync<T>(string key);

		/// <summary>
		/// Gets a cached string async.
		/// </summary>
		/// <param name="key">The key</param>
		/// <returns>The string</returns>
		Task<string?> GetStringAsync(string key);

		/// <summary>
		/// Removes something from cache async.
		/// </summary>
		/// <param name="key">The key</param>
		/// <returns></returns>
		Task RemoveAsync(string key);

		/// <summary>
		/// Caches an item async.
		/// </summary>
		/// <param name="key">The key</param>
		/// <param name="value">The value to cache</param>
		/// <param name="target">The cache location</param>
		/// <typeparam name="T">The type</typeparam>
		/// <returns></returns>
		Task SetAsync<T>(string key, T value, CacheTarget target);

	}

}
