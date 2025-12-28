using System.Diagnostics;
using System.Threading.Tasks;


namespace DoomMapGuessr.Services
{

    /// <summary>
    /// Service for webbrowser usage.
    /// </summary>
    public static class WebBrowserService
    {

        public static bool OpenUrl(string url)
        {

            try
            {

                ProcessStartInfo psi = new()
                {

                    FileName = url,
                    UseShellExecute = true

                };

                using Process? _ = Process.Start(psi);

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

                ProcessStartInfo psi = new()
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
