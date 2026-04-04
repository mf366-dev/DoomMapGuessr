using System;
using System.IO;

using DoomMapGuessr.Services;
using DoomMapGuessr.Services.Abstractions;
using DoomMapGuessr.ViewModels;

using Microsoft.Extensions.DependencyInjection;


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
				
				services.AddSingleton<ISettingsService>(new IniSettingsService(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dev.mf366.DoomMapGuessr", "config.ini")));
				services.AddSingleton<SettingsPageViewModel>();

				services.AddSingleton<MainWindowViewModel>();

			}

		}

	}

}
