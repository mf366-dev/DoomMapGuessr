using CommunityToolkit.Mvvm.Input;

using DoomMapGuessr.Services;


namespace DoomMapGuessr.ViewModels
{

    public partial class HomePageViewModel(INavigationService? navigationService = null) : ViewModelBase
    {

        private INavigationService? NavigationService { get; } = navigationService;

        public string LatestReleaseBody =>
            ApplicationSettings.Shared.Cache?.SavedRelease?.Body is null
                ? "# No release data found.\nSomething went wrong."
                : (ApplicationSettings.Shared.Cache.SavedRelease.TagName[1..] == ApplicationSettings.AppVersion
                    ? $"# What's new in this version?\n> DoomMapGuessr {ApplicationSettings.Shared.Cache.SavedRelease.TagName}\n{ApplicationSettings.Shared.Cache.SavedRelease.Body}"
                    : (ApplicationSettings.AssemblyVersion?.Revision == 1
                        ? $"# You're using a dev build of DoomMapGuessr\nUpdate to a stable build to see release info."
                        : $"# New version available! What's new?\n> DoomMapGuessr {ApplicationSettings.Shared.Cache.SavedRelease.TagName}\n{ApplicationSettings.Shared.Cache.SavedRelease.Body}"));

        [RelayCommand]
        private void NavigateToUnlockables() => NavigationService?.NavigateTo("AchievementsUnlockables");

        [RelayCommand]
        private void OpenGitHubRepo()
        {

            if (ApplicationSettings.Shared.Cache?.SavedRelease?.HtmlUrl is null)
                return;

            _ = WebBrowserService.OpenUrl(ApplicationSettings.Shared.Cache.SavedRelease.HtmlUrl);

        }

    }

}
