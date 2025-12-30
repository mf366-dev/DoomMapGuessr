using System;
using System.Globalization;
using System.Threading.Tasks;

using Avalonia;
using DoomMapGuessr.Settings;
using Octokit;


namespace DoomMapGuessr
{

    internal sealed class Program
    {

        private static readonly string[] allowedCultures = ["en-US", "pt-br", "pt-PT", "sk-sk"];
        private static readonly string systemCulture = CultureInfo.CurrentCulture.Name;

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                      .UsePlatformDetect()
                      .WithInterFont()
                      .LogToTrace();

        public static void PrepareApplicationSettings()
        {

            if (!ApplicationSettings.Shared.Settings.Sections.ContainsSection("Language"))
                ApplicationSettings.Shared.Settings.Sections.Add(new("Language"));

            if (!ApplicationSettings.Shared.Settings.Sections.ContainsSection("GUI"))
                ApplicationSettings.Shared.Settings.Sections.Add(new("GUI"));

            if (!ApplicationSettings.Shared.Settings["Language"].ContainsKey("Culture"))
            {

                ApplicationSettings.Shared.Settings["Language"]["Culture"] = allowedCultures.Contains(systemCulture, StringComparer.OrdinalIgnoreCase)
                                                                                 ? systemCulture
                                                                                 : allowedCultures[0];

            }

            if (!ApplicationSettings.Shared.Settings["GUI"].ContainsKey("FollowSystem"))
                ApplicationSettings.Shared.Settings["GUI"]["FollowSystem"] = "1";

            if (!ApplicationSettings.Shared.Settings["GUI"].ContainsKey("DarkMode"))
                ApplicationSettings.Shared.Settings["GUI"]["DarkMode"] = "1";

            ApplicationSettings.Shared.Save("config");

        }

        public static void PrepareApplicationCache()
        {

            ApplicationCache cache = new(ApplicationSettings.Shared);
            ApplicationSettings.Shared.Cache = cache;

        }

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static async Task<int> Main(string[] args)
        {

            PrepareApplicationSettings();
            PrepareApplicationCache();
            ApplicationSettings.Shared.Cache!.LatestRelease = await new GitHubClient(new ProductHeaderValue("DoomMapGuessr")).Repository.Release.GetLatest("MF366-Coding", "DoomMapGuessr");

            BuildAvaloniaApp()
#if DEBUG
                .WithDeveloperTools()
#endif
                .StartWithClassicDesktopLifetime(args);

            return 0;

        }

    }

}
