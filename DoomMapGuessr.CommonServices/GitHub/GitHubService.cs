using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace DoomMapGuessr.CommonServices.GitHub
{

    /// <summary>
    /// The GitHub service.
    /// </summary>
    public class GitHubService
    {

        private readonly HttpClient client;
		private readonly JsonSerializerOptions options = new()
		{

			PropertyNameCaseInsensitive = true

		};

		/// <summary>
		/// Initializes a new GitHub service.
		/// </summary>
		/// <param name="owner">The repo owner</param>
		/// <param name="repo">The repo name</param>
		/// <param name="tag">The tag name or <c>latest</c> if <c>null</c></param>
		/// <param name="userAgent">The user agent; mark <c>null</c> to use the default</param>
		public GitHubService(string owner, string repo, string? tag = null, string? userAgent = null)
        {

            Owner = owner;
            Repo = repo;
            Tag = tag ?? "latest";
            UserAgent = userAgent ?? "DoomMapGuessr/3.0.0-by-MF366";

            client = new();
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

        }

        /// <summary>
        /// Tries to get the requested release.
        /// </summary>
        /// <returns>A GitHub release or null if something went wrong</returns>
        public async Task<GitHubRelease?> TryGetReleaseAsync()
        {

            try
            {

                string response = await client.GetStringAsync(ApiUrl);
                return JsonSerializer.Deserialize<GitHubRelease>(response, options);

            }
            catch (HttpRequestException)
            {

                return null;

            }

        }

        /// <summary>
        /// The URL to the API endpoint.
        /// </summary>
        public string ApiUrl => $"https://api.github.com/repos/{Owner}/{Repo}/releases/{Tag}";

        /// <summary>
        /// The repo owner.
        /// </summary>
        public string Owner { get; }

        /// <summary>
        /// The repo name.
        /// </summary>
        public string Repo { get; }

        /// <summary>
        /// The tag name.
        /// </summary>
        public string Tag { get; }

        /// <summary>
        /// The user agent.
        /// </summary>
        public string UserAgent { get; }

    }

}
