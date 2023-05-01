using System.Configuration;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

namespace App.BLL.YouTube;

public static class SetupDependencies
{
    private static async Task AddExecutePermission(string filePath)
    {
        var process = new System.Diagnostics.Process();
        const string bashFileName = "/bin/bash";

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && File.Exists(bashFileName))
        {
            process.StartInfo.FileName = bashFileName;
            process.StartInfo.Arguments = $"-c \"chmod +x -- '{filePath}'\"";
            process.StartInfo.UseShellExecute = false;
            process.Start();

            await process.WaitForExitAsync();
        }
    }

    private static async Task DownloadFfmpeg()
    {
        await YoutubeDLSharp.Utils.DownloadFFmpeg();
        await AddExecutePermission("ffmpeg");
    }

    private static async Task DownloadYtDlp()
    {
        await YoutubeDLSharp.Utils.DownloadYtDlp();
        await AddExecutePermission("yt-dlp");
    }

    public static async Task DownloadAndSetupDependencies(IConfiguration configuration)
    {
        var youTubeConfig = configuration.GetRequiredSection(YouTubeSettings.SectionKey).Get<YouTubeSettings>() ??
                            throw new ConfigurationErrorsException("Failed to read YouTube configuration!");

        if (youTubeConfig.DownloadFfmpeg)
        {
            await DownloadFfmpeg();
        }

        if (youTubeConfig.DownloadYtDlp)
        {
            await DownloadYtDlp();
        }
    }
}