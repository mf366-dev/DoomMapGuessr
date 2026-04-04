using System;
using System.IO;

using DoomMapGuessr.Services;
using DoomMapGuessr.Services.Abstractions;
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
			public void AddCommonServices()
			{

				services.AddSingleton<ISettingsService>(new IniSettingsService(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dev.mf366.DoomMapGuessr", "config.ini")).Load().Parse());
				services.AddSingleton<SettingsPageViewModel>();

				services.AddSingleton<MainWindowViewModel>();
				services.AddSingleton<AchievementsUnlockablesViewModel>();
				services.AddSingleton<ClassicModeViewModel>();
				services.AddSingleton<GeoModeViewModel>();
				services.AddSingleton<HomePageViewModel>();

				services.AddSingleton<Release>();

			}

		}

	}

}
