using System;
using System.IO;
using System.Threading.Tasks;

using DoomMapGuessr.Cache.Abstractions;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;


namespace DoomMapGuessr.Cache
{

	/// <summary>
	/// The caching service used by DoomMapGuessr.
	/// </summary>
	/// <param name="cacheDirectory"></param>
	public class GlobalCachingService(
		string cacheDirectory
	) : ICachingService,
		ICachingServiceAsync
	{

		private MemoryCache memory = new(
			new MemoryCacheOptions
			{
				Clock = new SystemClock(),
				CompactionPercentage = 0.5,
				ExpirationScanFrequency = new(0, 3, 0),
				SizeLimit = 1000,
				TrackStatistics = true
			}
		);

		/// <summary>
		/// The directory used for persistent cache.
		/// </summary>
		public string CacheDirectory => cacheDirectory;

		/// <summary>
		/// The directory used for temporary cache.
		/// </summary>
		public string TempCacheDirectory => Path.Join(CacheDirectory, "Temp");

		private void ClearTemporaryCache() => throw new NotImplementedException("This is a WIP"); // todo: implement this

		private void ClearPersistentCache() => throw new NotImplementedException("This is a WIP"); // todo: implement this

		private void ClearNoFlagSupport(CacheTarget target)
		{

			switch (target)
			{

				case CacheTarget.Memory:
					memory.Clear();
					break;

				case CacheTarget.Temporary:
					ClearTemporaryCache();
					break;

				case CacheTarget.Persistent:
					ClearPersistentCache();
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(target), target, "No such cache target");

			}

		}

		/// <inheritdoc />
		public void Clear(CacheTarget target)
		{

			foreach (var flag in Enum.GetValues<CacheTarget>())
			{

				if ((target & flag) == flag)
					ClearNoFlagSupport(flag);

			}

		}

		/// <inheritdoc />
		public T Get<T>(string key) => throw new NotImplementedException();

		/// <inheritdoc />
		public string? GetString(string key) => throw new NotImplementedException();

		/// <inheritdoc />
		public void Remove(string key) => throw new NotImplementedException();

		/// <inheritdoc />
		public void Set<T>(string key, T value, CacheTarget target) => throw new NotImplementedException();

		/// <inheritdoc />
		public async Task ClearAsync(CacheTarget target) => throw new NotImplementedException();

		/// <inheritdoc />
		public async Task<T> GetAsync<T>(string key) => throw new NotImplementedException();

		/// <inheritdoc />
		public async Task<string?> GetStringAsync(string key) => throw new NotImplementedException();

		/// <inheritdoc />
		public async Task RemoveAsync(string key) => throw new NotImplementedException();

		/// <inheritdoc />
		public async Task SetAsync<T>(string key, T value, CacheTarget target) => throw new NotImplementedException();

	}

}
