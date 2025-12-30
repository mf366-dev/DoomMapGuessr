using System.IO;
using System.Threading.Tasks;

using Avalonia.Data;


namespace DoomMapGuessr.Settings
{

    /// <summary>
    /// The cache for an application.
    /// </summary>
    /// <param name="applicationSettings">The application's settings - used for establishing the directory path</param>
    /// <param name="cacheDirName">The name of the cache directory - defaults to <c>"AppCache"</c></param>
    public sealed class ApplicationCache(
        ApplicationSettings applicationSettings,
        string cacheDirName = "AppCache"
    )
    {

        /// <summary>
        /// The cache directory.
        /// </summary>
        public string CacheDirectory => Path.Join(applicationSettings.DirectoryPath, cacheDirName);

        /// <summary>
        /// Clears the cache and returns the statistics.
        /// </summary>
        public (double MbCounter, int FilesRemoved, int DirsRemoved) Clear()
        {

            double megabyteCounter = 0;
            int filesRemoved = 0;
            int directoriesRemoved = 0;

            foreach (string filepath in Directory.EnumerateFiles(CacheDirectory))
            {

                double fileLength = new FileInfo(filepath).Length / 1_000_000.0; // gimme that shit in megabytes pwease
                megabyteCounter += fileLength;

                File.Delete(filepath);

                filesRemoved++;

            }

            foreach (string directory in Directory.EnumerateDirectories(CacheDirectory))
            {
                File.Delete(directory);
                directoriesRemoved++;
            }

            return (megabyteCounter, filesRemoved, directoriesRemoved);

        }

        /// <summary>
        /// Performs an hard clear (removes everything from cache).
        /// </summary>
        public void HardClear()
        {

            foreach (string filepath in Directory.EnumerateFiles(CacheDirectory))
                File.Delete(filepath);

            foreach (string dir in Directory.EnumerateDirectories(CacheDirectory))
                File.Delete(dir);

        }

        /// <summary>
        /// Gets something from the cache.
        /// </summary>
        /// <param name="id">The ID of the element</param>
        /// <returns>The element or an empty result</returns>
        public Optional<byte[]> Get(string id)
        {

            string filepath = Path.Join(CacheDirectory, id);

            return !File.Exists(filepath) ? Optional<byte[]>.Empty : File.ReadAllBytes(filepath);

        }

        /// <summary>
        /// Gets something from the cache async.
        /// </summary>
        /// <param name="id">The ID of the element</param>
        /// <returns>The element or null</returns>
        public async Task<byte[]?> GetAsync(string id)
        {

            string filepath = Path.Join(CacheDirectory, id);

            return !File.Exists(filepath) ? null : await File.ReadAllBytesAsync(filepath);

        }

        /// <summary>
        /// Adds to or overwrites from the cache.
        /// </summary>
        /// <param name="id">The new ID</param>
        /// <param name="data">The data to fill in</param>
        public void Set(string id, byte[] data) => File.WriteAllBytes(Path.Join(CacheDirectory, id), data);

        /// <summary>
        /// Adds to or overwrites from the cache async.
        /// </summary>
        /// <param name="id">The new ID</param>
        /// <param name="data">The data to fill in</param>
        public async Task SetAsync(string id, byte[] data) => await File.WriteAllBytesAsync(Path.Join(CacheDirectory, id), data);

    }

}
