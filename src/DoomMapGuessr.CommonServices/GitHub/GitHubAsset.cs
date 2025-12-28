using System.Text.Json.Serialization;


namespace DoomMapGuessr.CommonServices.GitHub
{

    /// <summary>
    /// An asset part of a release on GitHub.
    /// </summary>
    public record GitHubAsset
    {

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("browser_download_url")]
        public string BrowserDownloadUrl { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    }

}
