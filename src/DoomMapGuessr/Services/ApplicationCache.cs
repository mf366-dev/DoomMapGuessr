using System.IO;
using System.Threading.Tasks;

using Avalonia.Data;

using DoomMapGuessr.CommonServices.GitHub;


namespace DoomMapGuessr.Services
{

    public sealed class ApplicationCache(
        ApplicationSettings applicationSettings,
        string cacheDirName = "AppCache"
    )
    {

        public GitHubRelease? SavedRelease { get; set; } = null;

        public string AppCacheDirectory => Path.Join(applicationSettings.DirectoryPath, cacheDirName);

        public void Add(string id, byte[] data) => File.WriteAllBytes(Path.Join(AppCacheDirectory, id), data);

        public void Clear()
        {

            foreach (string file in Directory.EnumerateFiles(AppCacheDirectory))
                File.Delete(file);

            foreach (string dir in Directory.EnumerateDirectories(AppCacheDirectory))
                File.Delete(dir);

        }

        public Optional<byte[]> Get(string id)
        {

            string filepath = Path.Join(AppCacheDirectory, id);

            return !File.Exists(filepath) ? Optional<byte[]>.Empty : File.ReadAllBytes(filepath);

        }

        public async Task<byte[]?> GetAsync(string id)
        {

            string filepath = Path.Join(AppCacheDirectory, id);

            return !File.Exists(filepath) ? null : await File.ReadAllBytesAsync(filepath);

        }

        public void Set(string id, byte[] data) => File.WriteAllBytes(Path.Join(AppCacheDirectory, id), data);

        public async Task SetAsync(string id, byte[] data) => await File.WriteAllBytesAsync(Path.Join(AppCacheDirectory, id), data);

    }

}
