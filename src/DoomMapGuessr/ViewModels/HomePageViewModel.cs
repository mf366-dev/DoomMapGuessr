using CommunityToolkit.Mvvm.Input;

using DoomMapGuessr.Services;
using DoomMapGuessr.Settings;


namespace DoomMapGuessr.ViewModels
{

    public partial class HomePageViewModel : ViewModelBase
    {

        public HomePageViewModel() { }

        public HomePageViewModel(INavigationService navigationService)
        {

            NavigationService = navigationService;

        }

        private INavigationService? NavigationService { get; }

        public string LatestReleaseBody =>
            ApplicationSettings.Shared.Cache?.LatestRelease?.Body is null
                ? "# No release data found.\nSomething went wrong."
                : (ApplicationSettings.Shared.Cache.LatestRelease.TagName[1..] == ApplicationSettings.AppVersion
                    ? $"# What's new in this version?\n**DoomMapGuessr {ApplicationSettings.Shared.Cache.LatestRelease.TagName}**\n\n{ApplicationSettings.Shared.Cache.LatestRelease.Body}"
                    : (ApplicationSettings.AssemblyVersion?.Revision == 1
                        ? $"# You're using a dev build of DoomMapGuessr\nUpdate to a stable build to see release info."
                        : $"# New version available! What's new?\n**DoomMapGuessr {ApplicationSettings.Shared.Cache.LatestRelease.TagName}**\n\n{ApplicationSettings.Shared.Cache.LatestRelease.Body}"));

        [RelayCommand]
        private void NavigateToUnlockables() => NavigationService?.NavigateTo("AchievementsUnlockables");

        [RelayCommand]
        private void NavigateToClassicMode() => NavigationService?.NavigateTo("GuesserMode");

        [RelayCommand]
        private void OpenGitHubRepo()
        {

            if (ApplicationSettings.Shared.Cache?.LatestRelease?.HtmlUrl is null)
                return;

            _ = WebBrowserService.OpenUrl(ApplicationSettings.Shared.Cache.LatestRelease.HtmlUrl);

        }

    }

}
