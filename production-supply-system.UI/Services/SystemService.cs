using System.Diagnostics;
using System.Threading.Tasks;

using UI_Interface.Contracts;

namespace UI_Interface.Services
{
    public class SystemService : ISystemService
    {
        public async Task OpenExcelFile(string filePath)
        {
            await Task.Run(() =>
            {
                ProcessStartInfo startInfo = new()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c start excel \"{filePath}\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                _ = Process.Start(startInfo);
            });
        }

        public async Task OpenInWebBrowser(string url)
        {
            await Task.Run(() =>
            {
                ProcessStartInfo psi = new()
                {
                    FileName = url,
                    UseShellExecute = true
                };
                _ = Process.Start(psi);
            });
        }
    }
}
