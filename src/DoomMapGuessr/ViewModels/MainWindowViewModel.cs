using Avalonia.Controls;

using CommunityToolkit.Mvvm.ComponentModel;

using DoomMapGuessr.Services;
using DoomMapGuessr.Views;


namespace DoomMapGuessr.ViewModels
{

	/// <summary>
	/// View model for the <see cref="MainWindow"/>.
	/// </summary>
    public partial class MainWindowViewModel : ViewModelBase, INavigationService
    {

        [ObservableProperty]
        private UserControl currentPage;

        [ObservableProperty]
        private bool isSidebarOpen = true;

		/// <summary>
		/// Initializes the ViewModel for the <see cref="MainWindow"/>.
		/// </summary>
        public MainWindowViewModel()
        {

            HomePageViewModel dataCtx = new(this);
            ViewModelArchive.Archive(dataCtx);

            CurrentPage = new HomePage
            {
                DataContext = dataCtx
            };

        }

		/// <inheritdoc />
		public void NavigateTo(string pageName)
        {

            switch (pageName)
            {

                case "Home":
                    if (!ViewModelArchive.TryGet<HomePageViewModel>(out var homePageViewModel))
                    {

                        homePageViewModel = new(this);
                        ViewModelArchive.Archive(homePageViewModel);

                    }

                    CurrentPage = new HomePage
                    {
                        DataContext = homePageViewModel
                    };

                    break;

				case "ClassicMode":
					if (!ViewModelArchive.TryGet<ClassicModeViewModel>(out var classicModeViewModel))
					{

						classicModeViewModel = new();
						ViewModelArchive.Archive(classicModeViewModel);

					}

					CurrentPage = new ClassicModePage
					{
						DataContext = classicModeViewModel
					};

					break;

				case "GeoMode":
					if (!ViewModelArchive.TryGet<GeoModeViewModel>(out var geoModeViewModel))
					{

						geoModeViewModel = new();
						ViewModelArchive.Archive(geoModeViewModel);

					}

					CurrentPage = new GeoModePage
					{
						DataContext = geoModeViewModel
					};

					break;

				case "AchievementsUnlockables":
                    if (!ViewModelArchive.TryGet<AchievementsUnlockablesViewModel>(out var achievementsUnlockablesViewModel))
                    {

                        achievementsUnlockablesViewModel = new();
                        ViewModelArchive.Archive(achievementsUnlockablesViewModel);

                    }

                    CurrentPage = new AchievementsUnlockablesPage
                    {
                        DataContext = achievementsUnlockablesViewModel
                    };

                    break;

                case "Settings":
                    if (!ViewModelArchive.TryGet<SettingsPageViewModel>(out var settingsPageViewModel))
                    {

                        settingsPageViewModel = new();
                        ViewModelArchive.Archive(settingsPageViewModel);

                    }

                    CurrentPage = new SettingsPage
                    {
                        DataContext = settingsPageViewModel
                    };

                    break;

                case "CloseSidebarPane":
                    IsSidebarOpen = false;

                    break;

                case "OpenSidebarPane":
                    IsSidebarOpen = true;

                    break;

                case "ToggleSidebarPane":
                    IsSidebarOpen = !IsSidebarOpen;

                    break;

            }

        }

		/// <inheritdoc />
        public void NavigateTo(string pageName, object? parameter) => NavigateTo(pageName);

    }

}
