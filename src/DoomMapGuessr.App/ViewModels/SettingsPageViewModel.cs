using System;
using System.Linq;

using Avalonia;
using Avalonia.Styling;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DoomMapGuessr.Services.Abstractions;
using DoomMapGuessr.Strings;

using Microsoft.Extensions.DependencyInjection;


namespace DoomMapGuessr.ViewModels
{

	public partial class SettingsPageViewModel : ViewModelBase
	{

		[ObservableProperty]
		public partial int CurrentIndex { get; set; } = Array.IndexOf(App.allowedCultures, ((App)Application.Current!)
			.ServiceProvider
			.GetRequiredService<ISettingsService>()
			.GetString("Language.Culture")
		+ 1);

		[ObservableProperty]
		public partial int Proportions { get; set; } = Math.Min(Math.Clamp(((App)Application.Current!)
			.ServiceProvider
			.GetRequiredService<ISettingsService>()
			.GetInt32("Screenshots.Proportions"), 0, 3)
		, 0);

		[ObservableProperty]
		public partial bool CustomTheme { get; set; } = !((App)Application.Current!)
			.ServiceProvider
			.GetRequiredService<ISettingsService>()
			.GetBoolean("GUI.FollowSystem");

		[ObservableProperty]
		public partial bool DarkTheme { get; set; } = ((App)Application.Current!)
			.ServiceProvider
			.GetRequiredService<ISettingsService>()
			.GetBoolean("GUI.DarkTheme");

		[ObservableProperty]
		public partial string[] LanguageComboBoxItems { get; set; } =
		[

			// The comments to the right of the items
			// are easy ways to match languages to codes.
			// Item: Native name
			// In comment, by this order:
			// English name, ISO language name (2 letters),
			// ISO language name (3 letters),
			// Windows (3 letter language name), Windows LCID
			Resources.Settings_Language_FollowSystem,		// Same as System (Not Invariant)
			"English (United States)",						// English (United States)			// en // eng // ENU // 1033
			"Português (Brasil)",							// Portuguese (Brazil)				// pt // por // PTB // 1046
			"Português (Portugal)"							// Portuguese (Portugal)			// pt // por // PTG // 2070

		];

		private void RunLanguageChangeProtocol()
		{

			((App)Application.Current!)
			.ServiceProvider
			.GetRequiredService<ISettingsService>()
			.Set("Language.Culture", CurrentIndex == 0 // same as system
				? App.allowedCultures.Contains(App.systemCulture, StringComparer.OrdinalIgnoreCase) // same as system is allowed
					? App.systemCulture      // same as system
					: App.allowedCultures[0] // en-US
				: App.allowedCultures[CurrentIndex - 1]);

			Resources.Culture = new(App.allowedCultures[CurrentIndex - 1]); // auto updates UI

		}

		private void RunThemeChangeProtocol()
		{

			((App)Application.Current!).ServiceProvider.GetRequiredService<ISettingsService>().Set("GUI.FollowSystem", CustomTheme ? "0" : "1");
			((App)Application.Current!).ServiceProvider.GetRequiredService<ISettingsService>().Set("GUI.DarkTheme", CustomTheme ? "1" : "0");

			_ = (Application.Current?.RequestedThemeVariant = !CustomTheme
																? ThemeVariant.Default
																: (DarkTheme
																	   ? ThemeVariant.Dark
																	   : ThemeVariant.Light));

		}

		[RelayCommand]
		private void SaveSettings()
		{

			RunLanguageChangeProtocol();
			RunThemeChangeProtocol();
			((App)Application.Current!).ServiceProvider.GetRequiredService<ISettingsService>().Save();

		}

	}

}
