/*
 * ________                   ______  ___           _________
 * ___  ___\_______________ _____   |/ _/_____________  ____/__  ________________________
 * __  / /_/ ___\ ___\  __ `___\  /|_/ /  __ `/_  ___\ / ___  / /_/ __\  ___/  ___/  ___/
 * _  /_/ / /_////_/_/ / / / /_/ /  / // /_/ /_  /_////_/ // /_/ /  __(__  )(__  )  /
 * /_____/\____\____/_/ /_/ /_/_/  /_/ \__,_/_  .___\____/ \__,_/\___/____//____//_/
 *                                           /_/
 *
 * Copyright (c) 2024-2026 Matthew
 * MIT License
 *
 * DoomMapGuessr - the GeoGuessr of DOOM
 *
 */

using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Avalonia;

using DoomMapGuessr.Helpers;
using DoomMapGuessr.Services.Cache;
using DoomMapGuessr.Services.Cache.Abstractions;
using DoomMapGuessr.Services.Settings;
using DoomMapGuessr.ViewModels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace DoomMapGuessr
{

	internal sealed class Program
	{

		private const string DB_URL = "https://raw.githubusercontent.com/MF366-Coding/DoomMapGuessr/refs/heads/main/data/MAPDAT3.db";

		public static IHost Host { get; private set; } = null!;

		public static string AppDataDirectory =>
			Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dev.mf366.doommapguessr");

		// Avalonia configuration, don't remove; also used by visual designer.
		public static AppBuilder BuildAvaloniaApp() =>
			AppBuilder.Configure<App>()
					  .UsePlatformDetect()
					  .WithInterFont()
					  .LogToTrace();

		public static IHostBuilder CreateHostBuilder(
			string[] args
		) =>
			Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
					 .ConfigureServices((
											ctx,
											services
										) =>
										{

											services.AddSingleton<ISettingsService>(_ => new IniSettingsService(
																						Path.Join(AppDataDirectory, "config.ini")
																					)
											);

											services.AddSingleton<ICachingService>(_ => new CachingService(Path.Join(AppDataDirectory, "AppCache")));

											services.AddSingleton<MainWindowViewModel>();
											services.AddSingleton<MainWindowViewModel>();
											services.AddSingleton<HomePageViewModel>();
											services.AddSingleton<ClassicModeViewModel>();
											services.AddSingleton<GeoModeViewModel>();
											services.AddSingleton<AchievementsUnlockablesViewModel>();
											services.AddSingleton<SettingsPageViewModel>();

										}
					 );

		public static void PrepareApplicationSettings(
        			ISettingsService settings
        		)
        		{

        			if (settings is IniSettingsService { IsIniParsed: false } ini)
        				ini.Load().Parse();

        			#region Language Settings

        			if (!settings.Contains("Language.?"))
        				settings.Set<string?>("Language.*", null);

        			if (!settings.Contains("Language.Culture"))
        			{

        				settings.Set(
        					"Language.Culture", App.AllowedCultures.Contains(App.SystemCulture.Name, StringComparer.OrdinalIgnoreCase)
        											? App.SystemCulture.Name
        											: App.AllowedCultures[0]
        				);

        			}

        			#endregion

        			#region GUI Settings

        			if (!settings.Contains("GUI.?"))
        				settings.Set<string?>("GUI.*", null);

        			if (!settings.Contains("GUI.FollowSystem"))
        				settings.Set("GUI.FollowSystem", 1);

        			if (!settings.Contains("GUI.DarkTheme"))
        				settings.Set("GUI.DarkTheme", 1);

        			#endregion

        			#region Database Settings

        			if (!settings.Contains("Database.?"))
        				settings.Set<string?>("Database.*", null);

        			if (!settings.Contains("Database.CheckPeriodicityMode"))
        				settings.Set("Database.CheckPeriodicityMode", 4); // check weekly

        			if (!settings.Contains("Database.DateOfLastCheck"))
        				settings.Set("Database.DateOfLastCheck", new DateTime(0).Ticks.ToString());

        			#endregion

        			#region Update Settings

        			if (!settings.Contains("Update.?"))
        				settings.Set<string?>("Update.*", null);

        			if (!settings.Contains("Update.Check"))
        				settings.Set("Update.Check", 1); // 1 for always check, 0 for never check

        			#endregion

        			settings.Save();

        		}

		public static async Task PrepareApplicationSettingsAsync(
			ISettingsService settings
		)
		{

			if (settings is IniSettingsService { IsIniParsed: false } ini)
				ini.Load().Parse();

			#region Language Settings

			if (!settings.Contains("Language.?"))
				settings.Set<string?>("Language.*", null);

			if (!settings.Contains("Language.Culture"))
			{

				settings.Set(
					"Language.Culture", App.AllowedCultures.Contains(App.SystemCulture.Name, StringComparer.OrdinalIgnoreCase)
											? App.SystemCulture.Name
											: App.AllowedCultures[0]
				);

			}

			#endregion

			#region GUI Settings

			if (!settings.Contains("GUI.?"))
				settings.Set<string?>("GUI.*", null);

			if (!settings.Contains("GUI.FollowSystem"))
				settings.Set("GUI.FollowSystem", 1);

			if (!settings.Contains("GUI.DarkTheme"))
				settings.Set("GUI.DarkTheme", 1);

			#endregion

			#region Database Settings

			if (!settings.Contains("Database.?"))
				settings.Set<string?>("Database.*", null);

			if (!settings.Contains("Database.CheckPeriodicityMode"))
				settings.Set("Database.CheckPeriodicityMode", 4); // check weekly

			if (!settings.Contains("Database.DateOfLastCheck"))
				settings.Set("Database.DateOfLastCheck", new DateTime(0).Ticks.ToString());

			#endregion

			#region Update Settings

			if (!settings.Contains("Update.?"))
				settings.Set<string?>("Update.*", null);

			if (!settings.Contains("Update.Check"))
				settings.Set("Update.Check", 1); // 1 for always check, 0 for never check

			#endregion

			await settings.SaveAsync();

		}

		public static async Task DownloadSqliteDatabaseAsync()
		{

			// todo: fetch the database (even if it's the first time playing DoomMapGuessr)

			// xxx: make sure to respect caching settings

			/*
			// for now this simply downloads even if its already cached
			byte[] bytes = await client.GetByteArrayAsync(DB_URL);
			await ApplicationState.Shared.Cache!.SetAsync("__cached_db", bytes);

			ApplicationState.Shared.SqliteConnection = new Microsoft.Data.Sqlite.SqliteConnection(
				$"Data Source={Path.Join(ApplicationState.Shared.Cache!.CacheDirectory, "__cached_db")}"
			);

			ApplicationState.Shared.SqliteConnection.Open();
			*/

		}

		/// <summary>
		/// DoomMapGuessr entry point.
		/// </summary>
		/// <param name="args">Commandline arguments</param>
		/// <remarks>
		/// Initialization code. Don't use any Avalonia, third-party APIs or any
		/// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
		/// yet and stuff might break.
		/// </remarks>
		[STAThread]
		public static async Task<int> Main(
			string[] args
		)
		{

			// Host and DI
			Host = CreateHostBuilder(args).Build();
			await Host.StartAsync();
			ApplicationServices.Root = Host.Services;
			ApplicationServices.VersionInfo = new(Assembly.GetExecutingAssembly());

			await PrepareApplicationSettingsAsync(ApplicationServices.Get<ISettingsService>());

			// Fetch latest release
			if (ApplicationServices.Get<ISettingsService>().GetBoolean("Update.Check"))
				ApplicationServices.SavedRelease = await ReleaseFetcher.FetchLatestAsync("mf366-dev", "DoomMapGuessr");

			await DownloadSqliteDatabaseAsync();

			BuildAvaloniaApp()
				#if DEBUG
				.WithDeveloperTools()
				#endif
				.StartWithClassicDesktopLifetime(args);

			return 0;

		}

		// todo: close SqLite connection
		~Program()
		{
			/*ApplicationState.Shared.SqliteConnection?.Close();*/
		}

	}

}
