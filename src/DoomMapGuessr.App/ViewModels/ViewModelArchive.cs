using System.Collections.Generic;

namespace DoomMapGuessr.ViewModels
{

	/// <summary>
	/// An archive for ViewModel reusability purposes.
	/// </summary>
	internal static class ViewModelArchive
	{

		/// <summary>
		/// The actual archive.
		/// </summary>
		private static readonly Dictionary<string, ViewModelBase> vms = [];

		/// <summary>
		/// Gets a ViewModel.
		/// </summary>
		/// <typeparam name="T">The ViewModel type</typeparam>
		/// <returns>The ViewModel</returns>
		public static T Get<T>()
			where T : ViewModelBase =>
			(vms[typeof(T).FullName!] as T)!;

		/// <summary>
		/// Archives a ViewModel.
		/// </summary>
		/// <typeparam name="T">The ViewModel type</typeparam>
		/// <param name="value">The ViewModel to archive</param>
		public static void Archive<T>(T value)
			where T : ViewModelBase =>
			vms[typeof(T).FullName!] = value;

		/// <summary>
		/// Tries to get a ViewModel.
		/// </summary>
		/// <typeparam name="T">The ViewModel type</typeparam>
		/// <param name="value">The returned ViewModel or <c>null</c></param>
		/// <returns><c>true</c> if found</returns>
		public static bool TryGet<T>(out T? value)
			where T : ViewModelBase
		{

			try
			{

				value = Get<T>();

				return true;

			}
			catch
			{

				value = null;

				return false;

			}

		}

	}

}
