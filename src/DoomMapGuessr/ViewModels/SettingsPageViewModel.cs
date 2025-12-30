using System;
using System.Linq;

using Avalonia;
using Avalonia.Styling;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoomMapGuessr.Settings;
using DoomMapGuessr.Strings;


namespace DoomMapGuessr.ViewModels
{

    public partial class SettingsPageViewModel : ViewModelBase
    {

        [ObservableProperty]
        private int currentIndex = Array.IndexOf(App.allowedCultures, ApplicationSettings.Shared.Settings["Language"]["Culture"]) +
                                   1;

        [ObservableProperty]
        private int proportions = Int32.TryParse(ApplicationSettings.Shared.Settings["Screenshots"]["Proportions"], out int result)
                                      ? (result is > 3 or < 0 // invalid result either ways lol
                                             ? 0
                                             : result)
                                      : 0;

        [ObservableProperty]
        private bool customTheme = ApplicationSettings.Shared.Settings["GUI"]["FollowSystem"] == "0";

        [ObservableProperty]
        private bool darkMode = ApplicationSettings.Shared.Settings["GUI"]["DarkMode"] == "1";

        [ObservableProperty]
        private string[] languageComboBoxItems =
        [
            Resources.Settings_Language_FollowSystem, "English (USA)", "Português (Brasil)", "Português (Portugal)"
        ];

        private void RunLanguageChangeProtocol() =>
            ApplicationSettings.Shared.Settings["Language"]["Culture"] = CurrentIndex == 0 // same as system
                                                                             ? App.allowedCultures.Contains(App.systemCulture, StringComparer.OrdinalIgnoreCase) // same as system is allowed
                                                                                   ? App.systemCulture      // same as system
                                                                                   : App.allowedCultures[0] // en-US
                                                                             : App.allowedCultures[CurrentIndex - 1];

        private void RunThemeChangeProtocol()
        {

            ApplicationSettings.Shared.Settings["GUI"]["FollowSystem"] = CustomTheme ? "0" : "1";
            ApplicationSettings.Shared.Settings["GUI"]["DarkMode"] = DarkMode ? "1" : "0";

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
            ApplicationSettings.Shared.Save("config");

        }

    }

}
