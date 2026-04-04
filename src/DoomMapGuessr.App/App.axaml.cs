using System.Globalization;
using System.Linq;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

using DoomMapGuessr.Extensions;
using DoomMapGuessr.Services.Abstractions;
using DoomMapGuessr.Settings;
using DoomMapGuessr.ViewModels;
using DoomMapGuessr.Views;

using Microsoft.Extensions.DependencyInjection;


namespace DoomMapGuessr
{

	/// <summary>
	/// Avalonia app.
	/// Contains Avalonia-specific functionality.
	/// </summary>
	public class App : Application
	{

		/// <summary>
		/// Globally used service provider.
		/// </summary>
		public ServiceProvider ServiceProvider { get; private set; } = null!;

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

		/// <inheritdoc/>
		public override void Initialize() => AvaloniaXamlLoader.Load(this);

		/// <summary>
		/// Runs after Avalonia is initialized.
		/// </summary>
		public override void OnFrameworkInitializationCompleted()
		{

			// Dependency Injection setup lesgo
			ServiceCollection collection = new();
			collection.AddCommonServices();
			// todo: add octokit fetch here
			ServiceProvider = collection.BuildServiceProvider();
			var vm = ServiceProvider.GetRequiredService<MainWindowViewModel>();

			RequestedThemeVariant =
				ServiceProvider.GetRequiredService<ISettingsService>().GetBoolean("GUI.FollowSystem")
										? ThemeVariant.Default
										: (ServiceProvider.GetRequiredService<ISettingsService>().GetBoolean("GUI.DarkTheme")
											   ? ThemeVariant.Dark
											   : ThemeVariant.Light);
			Strings.Resources.Culture = new(ServiceProvider.GetRequiredService<ISettingsService>().GetString("Language.Culture") ?? "en-US");

			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{

				// ReSharper disable CommentTypo
				// Avoid duplicate validations from both Avalonia and the CommunityToolkit.
				// More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
				// ReSharper restore CommentTypo
				DisableAvaloniaDataAnnotationValidation();

				desktop.MainWindow = new MainWindow
				{
					DataContext = vm
				};

			}

			base.OnFrameworkInitializationCompleted();

		}

	}

}
