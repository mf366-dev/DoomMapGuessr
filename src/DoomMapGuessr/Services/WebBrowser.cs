using System.Diagnostics;
using System.Threading.Tasks;


namespace DoomMapGuessr.Services
{

	public static class WebBrowser
	{

		public static bool OpenUrl(string url)
		{

			try
			{

				var psi = new ProcessStartInfo
				{

					FileName = url,
					UseShellExecute = true

				};

                using var _ = Process.Start(psi);

				return true;

			}
			catch
			{

				return false;

			}

		}

		public static async Task<Process?> OpenUrlAsync(string url)
		{

			try
			{

				var psi = new ProcessStartInfo
				{

					FileName = url,
					UseShellExecute = true

				};

				return await Task.Run(() => Process.Start(psi));

			}
			catch
			{

				return null;

			}

		}

	}

}
