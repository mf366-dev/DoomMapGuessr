using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

using DoomMapGuessr.Services.Abstractions;

using IniParser.Model;
using IniParser.Model.Configuration;
using IniParser.Model.Formatting;
using IniParser.Parser;


namespace DoomMapGuessr.Services
{

	/// <summary>
	/// A service that handles storage of settings in INI format.
	/// </summary>
	/// <remarks>
	/// Initializes this service with a path to the settings file.
	/// </remarks>
	/// <param name="filepath">The filepath</param>
	public sealed class IniSettingsService(string filepath) : ISettingsService, IDisposable, IAsyncDisposable
	{

		private readonly IniParserConfiguration defaultConfiguration = new()
		{

			AllowCreateSectionsOnFly = true,
			AllowDuplicateKeys = false,
			AllowDuplicateSections = false,
			AllowKeysWithoutSection = false,
			AssigmentSpacer = " ",
			CaseInsensitive = true,
			CommentString = ";",
			ConcatenateDuplicateKeys = true,
			KeyValueAssigmentChar = '=',
			NewLineStr = Environment.NewLine,
			OverrideDuplicateKeys = false,
			SectionEndChar = ']',
			SectionStartChar = '[',
			ThrowExceptionsOnError = true

		};

		private readonly string pathToSettings = filepath;
		private string stringifiedData = "";
		private IniData? actualIni;

		/// <summary>
		/// Initializes this service with a representation of the settings file.
		/// </summary>
		/// <param name="file">The file</param>
		public IniSettingsService(FileInfo file) : this(file.FullName) { }

		private bool disposedValue;

		/// <summary>
		/// Loads the settings as a string.
		/// </summary>
		/// <returns>Self</returns>
		public IniSettingsService Load() => Load("; This file was created by DoomMapGuessr.Services.IniSettingsService");

		/// <summary>
		/// Loads the settings as a string.
		/// </summary>
		/// <param name="defaultData">The data to use if the file is missing</param>
		/// <returns>Self</returns>
		public IniSettingsService Load(string defaultData)
		{

			var directoryPath = Path.GetDirectoryName(pathToSettings);

			if (!File.Exists(pathToSettings) && directoryPath is not null)
			{

				Directory.CreateDirectory(directoryPath);
				File.WriteAllText(pathToSettings, defaultData);

			}

			stringifiedData = File.ReadAllText(pathToSettings);

			return this;

		}

		/// <summary>
		/// Parses the settings and converts them to actual INI,
		/// using the default parser configuration.
		/// </summary>
		/// <returns>Self</returns>
		public IniSettingsService Parse() => Parse(defaultConfiguration);

		/// <summary>
		/// Parses the settings and converts them to actual INI.
		/// </summary>
		/// <param name="configuration">The parser configuration</param>
		/// <returns>Self</returns>
		public IniSettingsService Parse(IniParserConfiguration configuration)
		{

			configuration.ThrowExceptionsOnError = true; // force this to be true
			IniDataParser parser = new(configuration);

			actualIni = parser.Parse(stringifiedData);

			return this;

		}

		/// <summary>
		/// Whether or not the data has been loaded parsed.
		/// </summary>
		[MemberNotNullWhen(true, nameof(actualIni))]
		public bool IsIniParsed => actualIni is not null;

		[MemberNotNull(nameof(actualIni))]
		private void EnsureIniParsed()
		{

			if (!IsIniParsed)
				throw new InvalidDataException("The settings have not been parsed yet");

		}

		/// <inheritdoc/>
		public void Add<T>(string key, T value)
		{

			EnsureIniParsed();

			var longFormKey = key.Split('.', 2);

			if (longFormKey.Length < 2)
				throw new ArgumentException("The key must be in a <section>.<key> format", nameof(key));

			actualIni[longFormKey[0]][longFormKey[1]] = value?.ToString() ?? "null";

		}

		/// <inheritdoc/>
		public bool Contains(string key)
		{

			EnsureIniParsed();

			return actualIni.TryGetKey(key, out _);

		}

		/// <summary>
		/// Not supported for INI.
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		public T Get<T>(string key, T defaultValue = default!) => throw new InvalidOperationException("Cannot Get<T>() for INI. Try GetString() instead.");

		/// <inheritdoc/>
		public bool GetBoolean(string key)
		{

			var str = GetString(key);

			if (String.IsNullOrEmpty(str))
				return false;

			if (Boolean.TryParse(str, out var b))
				return b;

			if (Double.TryParse(str, out var d) && Math.Abs(d) == 1.0)
				return true;

			return Int32.TryParse(str, out var i) && Math.Abs(i) == 1;

		}

		/// <inheritdoc/>
		public double GetDouble(string key) => Double.TryParse(GetString(key), out var result) ? result : default;

		/// <summary>
		/// Gets a copy of the actual INI data.
		/// </summary>
		/// <returns>The copy</returns>
		public IniData GetCopy()
		{

			EnsureIniParsed();
			return new(actualIni);

		}

		/// <inheritdoc/>
		public int GetInt32(string key) => Int32.TryParse(GetString(key), out var result) ? result : default;

		/// <inheritdoc/>
		public string? GetString(string key)
		{

			EnsureIniParsed();

			return actualIni.TryGetKey(key, out var value) ? value : null;

		}

		/// <inheritdoc/>
		public void Save()
		{

			EnsureIniParsed();

			var contents = actualIni.ToString(
				new DefaultIniDataFormatter(defaultConfiguration)
			);

			File.WriteAllText(pathToSettings, contents);

		}

		/// <inheritdoc/>
		public async Task SaveAsync()
		{

			EnsureIniParsed();

			var contents = actualIni.ToString(
				new DefaultIniDataFormatter(defaultConfiguration)
			);

			await File.WriteAllTextAsync(pathToSettings, contents);

		}

		/// <inheritdoc/>
		public void Set<T>(string key, T value) => throw new NotImplementedException();

		/// <inheritdoc/>
		public bool TryGet<T>(string key, out T value) => throw new NotImplementedException();

		/// <inheritdoc/>
		public void Dispose()
		{

			if (!disposedValue)
			{

				Save();

				actualIni = null;
				stringifiedData = String.Empty;

				disposedValue = true;

			}

			GC.SuppressFinalize(this);

		}

		/// <inheritdoc/>
		public async ValueTask DisposeAsync()
		{

			if (!disposedValue)
			{

				await SaveAsync();

				actualIni = null;
				stringifiedData = String.Empty;

				disposedValue = true;

			}

			GC.SuppressFinalize(this);

			await ValueTask.CompletedTask;

		}

	}

}
