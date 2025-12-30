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

            if (!ApplicationState.Shared.Settings.Data.Sections.ContainsSection("Language"))
                ApplicationState.Shared.Settings.Data.Sections.Add(new("Language"));

            if (!ApplicationState.Shared.Settings.Data.Sections.ContainsSection("GUI"))
                ApplicationState.Shared.Settings.Data.Sections.Add(new("GUI"));

            if (!ApplicationState.Shared.Settings.Data["Language"].ContainsKey("Culture"))
            {

                ApplicationState.Shared.Settings.Data["Language"]["Culture"] = allowedCultures.Contains(systemCulture, StringComparer.OrdinalIgnoreCase)
                                                                                 ? systemCulture
                                                                                 : allowedCultures[0];

            }

            if (!ApplicationState.Shared.Settings.Data["GUI"].ContainsKey("FollowSystem"))
                ApplicationState.Shared.Settings.Data["GUI"]["FollowSystem"] = "1";

            if (!ApplicationState.Shared.Settings.Data["GUI"].ContainsKey("DarkMode"))
                ApplicationState.Shared.Settings.Data["GUI"]["DarkMode"] = "1";

            ApplicationState.Shared.Settings.Save("config");

        }

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static async Task<int> Main(string[] args)
        {

            PrepareApplicationSettings();
            ApplicationState.Shared.SavedRelease = await new GitHubClient(new ProductHeaderValue("DoomMapGuessr")).Repository.Release.GetLatest("MF366-Coding", "DoomMapGuessr");

            BuildAvaloniaApp()
#if DEBUG
                .WithDeveloperTools()
#endif
                .StartWithClassicDesktopLifetime(args);

            return 0;

        }

    }

}
