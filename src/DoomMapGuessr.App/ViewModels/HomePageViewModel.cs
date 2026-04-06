using System;

using CommunityToolkit.Mvvm.Input;

using DoomMapGuessr.Helpers;
using DoomMapGuessr.Services;
using DoomMapGuessr.Strings;

using Octokit;


namespace DoomMapGuessr.ViewModels
{

	public partial class HomePageViewModel : ViewModelBase
	{

		public HomePageViewModel() { }

		public HomePageViewModel(INavigationService navigationService) => NavigationService = navigationService;

		private INavigationService? NavigationService { get; }

		public string LatestReleaseBody =>
			ApplicationServices.Get<Release>().Body is null
				? "# No release data found.\nSomething went wrong."
				: (ApplicationServices.Get<Release>().TagName[1..] == ApplicationServices.VersionInfo.ApplicationVersion
					? $"# What's new in this version?\n**DoomMapGuessr {ApplicationServices.Get<Release>().TagName}**\n\n{ApplicationServices.Get<Release>().Body}"
					: (ApplicationServices.VersionInfo.IsDevVersion // now we actually have a property named "IsDevVersion" how fucking cool right?
						? $"# No release data found.\nIt seems this is a build contained in a developer channel of DoomMapGuessr. If you are not a tester nor a developer, it is recommended you update to the latest stable version."
						: $"# New version available! What's new?\n**DoomMapGuessr {ApplicationServices.Get<Release>().TagName}**\n\n{ApplicationServices.Get<Release>().Body}"));

		private readonly Random random = new();
		private readonly string[] greetings = [Resources.Greetings_Regular01, Resources.Greetings_Regular02];

		public string RandomGreeting => DateTime.Now.Hour switch
		{

			3 => Resources.Greetings_CreepyHour01,
			18 or 19 => Resources.Greetings_Sunset01,
			_ => random.GetItems(greetings, 1)[0]

		};

		[RelayCommand]
		private void NavigateToUnlockables() => NavigationService?.NavigateTo("AchievementsUnlockables");

		[RelayCommand]
		private void NavigateToClassicMode() => NavigationService?.NavigateTo("ClassicMode");

		[RelayCommand]
		private void OpenGitHubRepo()
		{

			if (ApplicationServices.Get<Release>().HtmlUrl is null)
				return;

			_ = WebBrowser.OpenUrl(ApplicationServices.Get<Release>().HtmlUrl);

		}

	}

}
