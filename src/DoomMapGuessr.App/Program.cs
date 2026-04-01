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
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Avalonia;

using DoomMapGuessr.Services;
using DoomMapGuessr.Services.Abstractions;
using DoomMapGuessr.Settings;

using Octokit;


namespace DoomMapGuessr
{

	internal sealed class Program
	{

		public static IHost Host { get; private set; } = null!;

		public static string AppDataDirectory =>
			Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dev.mf366.doommapguessr");

		private static readonly HttpClient client = new();
		private static readonly string[] allowedCultures = [ "en-US", "pt-br", "pt-PT", "sk-sk" ];
		private static readonly string systemCulture = CultureInfo.CurrentCulture.Name;

		// Avalonia configuration, don't remove; also used by visual designer.
		public static AppBuilder BuildAvaloniaApp() =>
			AppBuilder.Configure<App>()
					  .UsePlatformDetect()
					  .WithInterFont()
					  .LogToTrace();

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
					 .ConfigureServices((ctx, services) =>
						 {

							 services.AddSingleton<ISettingsService>(_ => new IniSettingsService(Path.Join(AppDataDirectory, "config.ini")));
							 services.AddSingleton<ICachingService>(_ => new CachingService(Path.Join(AppDataDirectory, "AppCache")));

						 }
					 );

		public static async Task PrepareApplicationSettingsAsync(ISettingsService settings)
		{

			if (settings is IniSettingsService { IsIniParsed: false } ini)
				ini.Load().Parse();

			if (!settings.Contains("Language.?"))
				settings.Set<string?>("Language.*", null);

			if (!settings.Contains("GUI.?"))
				settings.Set<string?>("GUI.*", null);

			if (!settings.Contains("Language.Culture"))
			{

				settings.Set(
					"Language.Culture", allowedCultures.Contains(systemCulture, StringComparer.OrdinalIgnoreCase)
											? systemCulture
											: allowedCultures[0]
				);

			}

			if (!settings.Contains("GUI.FollowSystem"))
				settings.Set("GUI.FollowSystem", 1);

			if (!settings.Contains("GUI.DarkMode"))
				settings.Set("GUI.DarkMode", 1);

			await settings.SaveAsync();

		}

		private const string DB_URL = "https://raw.githubusercontent.com/MF366-Coding/DoomMapGuessr/refs/heads/main/data/MAPDAT3.db";

		public static async Task DownloadSqliteDatabaseAsync()
		{

			// for now this simply downloads even if its already cached
			// todo: actually do the cache thing and the y'know what im talking bout
			byte[] bytes = await client.GetByteArrayAsync(DB_URL);
			await ApplicationState.Shared.Cache!.SetAsync("__cached_db", bytes);

			ApplicationState.Shared.SqliteConnection = new Microsoft.Data.Sqlite.SqliteConnection(
				$"Data Source={Path.Join(ApplicationState.Shared.Cache!.CacheDirectory, "__cached_db")}"
			);

			ApplicationState.Shared.SqliteConnection.Open();

		}

		// Initialization code. Don't use any Avalonia, third-party APIs or any
		// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
		// yet and stuff might break.
		[STAThread]
		public static async Task<int> Main(string[] args)
		{

			Host = CreateHostBuilder(args).Build();
			await Host.StartAsync();
			ApplicationServices.Root = Host.Services;

			await PrepareApplicationSettingsAsync(ApplicationServices.Get<ISettingsService>());

			// todo: fix what's below this comment (see #22)

			ApplicationState.Shared.Cache?.CreateDirectory();

			ApplicationState.Shared.SavedRelease =
				await new GitHubClient(new ProductHeaderValue("DoomMapGuessr")).Repository.Release.GetLatest("MF366-Coding", "DoomMapGuessr");

			await DownloadSqliteDatabaseAsync();

			BuildAvaloniaApp()
				#if DEBUG
				.WithDeveloperTools()
				#endif
				.StartWithClassicDesktopLifetime(args);

			return 0;

		}

		~Program() { ApplicationState.Shared.SqliteConnection?.Close(); }

	}

}
