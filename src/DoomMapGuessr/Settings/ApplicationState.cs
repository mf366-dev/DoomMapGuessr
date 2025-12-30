using System;
using System.IO;
using System.Reflection;

using Octokit;


namespace DoomMapGuessr.Settings
{

    /// <summary>
    /// An applciation's state.
    /// </summary>
    /// <param name="assembly">The assembly the application is part of</param>
    /// <param name="settings">The application's settings</param>
    public sealed class ApplicationState(Assembly? assembly, ApplicationSettings settings)
    {

        /// <summary>
        /// The application's settings.
        /// </summary>
        public ApplicationSettings Settings { get; } = settings;

        /// <summary>
        /// A ready-to-use shared instance of <see cref="ApplicationState"/>.
        /// </summary>
        public static ApplicationState Shared => new ApplicationState(
            Assembly.GetEntryAssembly(),
            new ApplicationSettings(
                Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dev.mf366.DoomMapGuessr"))
                .PrepareDirectoryForDefaultUsage()
        ).WithCache();

        /// <summary>
        /// The application's cache or <c>null</c>.
        /// </summary>
        public ApplicationCache? Cache { get; private set; } = null;

        /// <summary>
        /// Sets up cache for this state.
        /// </summary>
        /// <returns>The state, now with cache set up</returns>
        public ApplicationState WithCache()
        {

            Cache = new(Settings);
            return this;

        }

        /// <summary>
        /// Sets up cache for this state, with a custom directory for the cache.
        /// </summary>
        /// <param name="cacheDirName">
        /// The name of the directory for the cache to be located in -
        /// <see cref="ApplicationCache(ApplicationSettings, String)"/>
        /// </param>
        /// <returns>The state, now with cache set up</returns>
        public ApplicationState WithCache(string cacheDirName)
        {

            Cache = new(Settings, cacheDirName);
            return this;

        }

        /// <summary>
        /// Versioning information for this assembly.
        /// </summary>
        public ApplicationVersioningInformation VersionInfo { get; } = new(assembly);

        /// <summary>
        /// The saved release or <c>null</c>.
        /// </summary>
        public Release? SavedRelease { get; set; } = null;


    }

}
