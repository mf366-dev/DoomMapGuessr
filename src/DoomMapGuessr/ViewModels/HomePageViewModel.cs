using CommunityToolkit.Mvvm.Input;

using DoomMapGuessr.Services;


namespace DoomMapGuessr.ViewModels
{

	public partial class HomePageViewModel(INavigationService? navigationService = null) : ViewModelBase
	{

		private INavigationService? NavigationService { get; } = navigationService;

		[RelayCommand]
		private void NavigateToUnlockables() => NavigationService?.NavigateTo("AchievementsUnlockables");

		[RelayCommand]
		private void OpenGitHubRepo()
		{

			if (ApplicationSettings.Shared.Cache?.SavedRelease?.HtmlUrl is null)
				return;

			_ = WebBrowser.OpenUrl(ApplicationSettings.Shared.Cache.SavedRelease.HtmlUrl);

		}

	}

}
