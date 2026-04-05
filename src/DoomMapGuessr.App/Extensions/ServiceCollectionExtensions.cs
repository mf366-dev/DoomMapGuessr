using System;
using System.IO;
using System.Reflection;
using DoomMapGuessr.Services;
using DoomMapGuessr.Services.Abstractions;
using DoomMapGuessr.Settings;
using DoomMapGuessr.ViewModels;

using Microsoft.Extensions.DependencyInjection;

using Octokit;


namespace DoomMapGuessr.Extensions
{

	/// <summary>
	/// Extensions for the <see cref="IServiceCollection"/> (DI).
	/// </summary>
	public static class ServiceCollectionExtensions
	{

		extension(IServiceCollection services)
		{

			/// <summary>
			/// Adds common services.
			/// </summary>
			public void AddCommonServices() => services.AddSingleton<ISettingsService>(new IniSettingsService(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dev.mf366.DoomMapGuessr", "config.ini")).Load().Parse())
														.AddSingleton<SettingsPageViewModel>()

														.AddSingleton<MainWindowViewModel>()
														.AddSingleton<AchievementsUnlockablesViewModel>()
														.AddSingleton<ClassicModeViewModel>()
														.AddSingleton<GeoModeViewModel>()
														.AddSingleton<HomePageViewModel>()

														// todo: this release is only added here to avoid DoomMapGuessr breaking
														// but it must be moved to App.axaml.cs and actually initialized there
														.AddSingleton<Release>();

		}

	}

}
