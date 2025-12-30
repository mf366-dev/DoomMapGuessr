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
            ApplicationState.Shared.SavedRelease?.Body is null
                ? "# No release data found.\nSomething went wrong."
                : (ApplicationState.Shared.SavedRelease.TagName[1..] == ApplicationState.Shared.VersionInfo.ApplicationVersion
                    ? $"# What's new in this version?\n**DoomMapGuessr {ApplicationState.Shared.SavedRelease.TagName}**\n\n{ApplicationState.Shared.SavedRelease.Body}"
                    : (ApplicationState.Shared.VersionInfo.IsDevVersion // now we actually have a property named "IsDevVersion" how fucking cool right?
                        ? $"# You're using a dev build of DoomMapGuessr\nUpdate to a stable build to see release info."
                        : $"# New version available! What's new?\n**DoomMapGuessr {ApplicationState.Shared.SavedRelease.TagName}**\n\n{ApplicationState.Shared.SavedRelease.Body}"));

        [RelayCommand]
        private void NavigateToUnlockables() => NavigationService?.NavigateTo("AchievementsUnlockables");

        [RelayCommand]
        private void NavigateToClassicMode() => NavigationService?.NavigateTo("ClassicMode");

        [RelayCommand]
        private void OpenGitHubRepo()
        {

            if (ApplicationState.Shared.SavedRelease?.HtmlUrl is null)
                return;

            _ = WebBrowserService.OpenUrl(ApplicationState.Shared.SavedRelease.HtmlUrl);

        }

    }

}
