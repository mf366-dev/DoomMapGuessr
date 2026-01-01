using System;
using System.Linq;

using Avalonia;
using Avalonia.Styling;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoomMapGuessr.Strings;


namespace DoomMapGuessr.ViewModels
{

    public partial class SettingsPageViewModel : ViewModelBase
    {

        [ObservableProperty]
        private int currentIndex = Array.IndexOf(App.allowedCultures, ApplicationState.Shared.Settings.Data["Language"]["Culture"]) +
                                   1;

        [ObservableProperty]
        private int proportions = Int32.TryParse(ApplicationState.Shared.Settings.Data["Screenshots"]["Proportions"], out int result)
                                      ? (result is > 3 or < 0 // invalid result either ways lol
                                             ? 0
                                             : result)
                                      : 0;

        [ObservableProperty]
        private bool customTheme = ApplicationState.Shared.Settings.Data["GUI"]["FollowSystem"] == "0";

        [ObservableProperty]
        private bool darkMode = ApplicationState.Shared.Settings.Data["GUI"]["DarkMode"] == "1";

        [ObservableProperty]
        private string[] languageComboBoxItems =
        [
            Resources.Settings_Language_FollowSystem, "English (USA)", "Português (Brasil)", "Português (Portugal)"
        ];

        private void RunLanguageChangeProtocol() =>
            ApplicationState.Shared.Settings.Data["Language"]["Culture"] = CurrentIndex == 0 // same as system
                                                                             ? App.allowedCultures.Contains(App.systemCulture, StringComparer.OrdinalIgnoreCase) // same as system is allowed
                                                                                   ? App.systemCulture      // same as system
                                                                                   : App.allowedCultures[0] // en-US
                                                                             : App.allowedCultures[CurrentIndex - 1];

        private void RunThemeChangeProtocol()
        {

            ApplicationState.Shared.Settings.Data["GUI"]["FollowSystem"] = CustomTheme ? "0" : "1";
            ApplicationState.Shared.Settings.Data["GUI"]["DarkMode"] = DarkMode ? "1" : "0";

            _ = (Application.Current?.RequestedThemeVariant = !CustomTheme
                                                                ? ThemeVariant.Default
                                                                : (DarkMode
                                                                       ? ThemeVariant.Dark
                                                                       : ThemeVariant.Light));

        }

        [RelayCommand]
        private void SaveSettings()
        {

            RunLanguageChangeProtocol();
            RunThemeChangeProtocol();
            ApplicationState.Shared.Settings.Save("config");

        }

    }

}
