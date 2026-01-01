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

using Avalonia;
using Octokit;


namespace DoomMapGuessr
{

	internal sealed class Program
	{

		private static readonly HttpClient client = new();
		private static readonly string[] allowedCultures = ["en-US", "pt-br", "pt-PT", "sk-sk"];
		private static readonly string systemCulture = CultureInfo.CurrentCulture.Name;

		// Avalonia configuration, don't remove; also used by visual designer.
		public static AppBuilder BuildAvaloniaApp() =>
			AppBuilder.Configure<App>()
					  .UsePlatformDetect()
					  .WithInterFont()
					  .LogToTrace();

		public static void PrepareApplicationSettings()
		{

			if (!ApplicationState.Shared.Settings.Data.Sections.ContainsSection("Language"))
				ApplicationState.Shared.Settings.Data.Sections.Add(new("Language"));

			if (!ApplicationState.Shared.Settings.Data.Sections.ContainsSection("GUI"))
				ApplicationState.Shared.Settings.Data.Sections.Add(new("GUI"));

			if (!ApplicationState.Shared.Settings.Data["Language"].ContainsKey("Culture"))
			{

				ApplicationState.Shared.Settings.Data["Language"]["Culture"] = allowedCultures.Contains(systemCulture, StringComparer.OrdinalIgnoreCase)
																				 ? systemCulture
																				 : allowedCultures[0];

			}

			if (!ApplicationState.Shared.Settings.Data["GUI"].ContainsKey("FollowSystem"))
				ApplicationState.Shared.Settings.Data["GUI"]["FollowSystem"] = "1";

			if (!ApplicationState.Shared.Settings.Data["GUI"].ContainsKey("DarkMode"))
				ApplicationState.Shared.Settings.Data["GUI"]["DarkMode"] = "1";

			ApplicationState.Shared.Settings.Save("config");

		}

		private const string DBUrl = "https://raw.githubusercontent.com/MF366-Coding/DoomMapGuessr/refs/heads/main/data/MAPDAT3.db";

		public static async Task DownloadSqliteDatabaseAsync()
		{

			// for now this simply downloads even if its already cached
			// todo: actually do the cache thing and the yknow what im talking bout
			var bytes = await client.GetByteArrayAsync(DBUrl);
			await ApplicationState.Shared.Cache!.SetAsync("__cached_db", bytes);

			ApplicationState.Shared.SqliteConnection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={Path.Join(ApplicationState.Shared.Cache!.CacheDirectory, "__cached_db")}");
			ApplicationState.Shared.SqliteConnection.Open();

		}

		// Initialization code. Don't use any Avalonia, third-party APIs or any
		// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
		// yet and stuff might break.
		[STAThread]
		public static async Task<int> Main(string[] args)
		{

			PrepareApplicationSettings();
			ApplicationState.Shared.Cache?.CreateDirectory();
			ApplicationState.Shared.SavedRelease = await new GitHubClient(new ProductHeaderValue("DoomMapGuessr")).Repository.Release.GetLatest("MF366-Coding", "DoomMapGuessr");

			await DownloadSqliteDatabaseAsync();

			BuildAvaloniaApp()
#if DEBUG
				.WithDeveloperTools()
#endif
				.StartWithClassicDesktopLifetime(args);

			return 0;

		}

		~Program()
		{

			ApplicationState.Shared.SqliteConnection.Close();

		}

	}

}
