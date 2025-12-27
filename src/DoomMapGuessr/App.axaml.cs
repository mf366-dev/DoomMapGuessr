using System;
using System.Globalization;
using System.Linq;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

using DoomMapGuessr.Services;
using DoomMapGuessr.ViewModels;
using DoomMapGuessr.Views;


namespace DoomMapGuessr
{

	public class App : Application
	{

		public static readonly string[] allowedCultures = ["en-US", "pt-br", "pt-PT", "sk-sk"];
		public static readonly string systemCulture = CultureInfo.CurrentCulture.Name;

		private static void DisableAvaloniaDataAnnotationValidation()
		{

			// Get an array of plugins to remove
			var dataValidationPluginsToRemove = BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

			// remove each entry found
			foreach (var plugin in dataValidationPluginsToRemove)
				_ = BindingPlugins.DataValidators.Remove(plugin);

		}

		public override void Initialize() => AvaloniaXamlLoader.Load(this);

		public override void OnFrameworkInitializationCompleted()
		{

			RequestedThemeVariant = ApplicationSettings.Shared.Settings["GUI"]["FollowSystem"] == "1"
										? ThemeVariant.Default
										: (ApplicationSettings.Shared.Settings["GUI"]["DarkMode"] == "1"
											   ? ThemeVariant.Dark
											   : ThemeVariant.Light);
			Strings.Resources.Culture = new(ApplicationSettings.Shared.Settings["Language"]["Culture"]);

			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{

				// ReSharper disable CommentTypo
				// Avoid duplicate validations from both Avalonia and the CommunityToolkit.
				// More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
				// ReSharper restore CommentTypo
				DisableAvaloniaDataAnnotationValidation();

				desktop.MainWindow = new MainWindow
				{
					DataContext = new MainWindowViewModel()
				};

			}

			base.OnFrameworkInitializationCompleted();

		}

	}

}
