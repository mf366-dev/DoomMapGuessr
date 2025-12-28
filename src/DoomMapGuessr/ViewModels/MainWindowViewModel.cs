using Avalonia.Controls;

using CommunityToolkit.Mvvm.ComponentModel;

using DoomMapGuessr.Services;
using DoomMapGuessr.Views;


namespace DoomMapGuessr.ViewModels
{

    public partial class MainWindowViewModel : ViewModelBase, INavigationService
    {

        [ObservableProperty]
        private UserControl currentPage;

        [ObservableProperty]
        private bool isSidebarOpen = true;

        public MainWindowViewModel()
        {

            HomePageViewModel dataCtx = new(this);
            ViewModelArchive.Set(dataCtx);

            CurrentPage = new HomePage
            {
                DataContext = dataCtx
            };

        }

        public void NavigateTo(string pageName)
        {

            switch (pageName)
            {

                case "Home":
                    if (!ViewModelArchive.TryGet<HomePageViewModel>(out var homePageViewModel))
                    {

                        homePageViewModel = new(this);
                        ViewModelArchive.Set(homePageViewModel);

                    }

                    CurrentPage = new HomePage
                    {
                        DataContext = homePageViewModel
                    };

                    break;

                case "AchievementsUnlockables":
                    if (!ViewModelArchive.TryGet<AchievementsUnlockablesViewModel>(out var achievementsUnlockablesViewModel))
                    {

                        achievementsUnlockablesViewModel = new();
                        ViewModelArchive.Set(achievementsUnlockablesViewModel);

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
                        ViewModelArchive.Set(settingsPageViewModel);

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

        public void NavigateTo(string pageName, object? parameter) => NavigateTo(pageName);

    }

}
