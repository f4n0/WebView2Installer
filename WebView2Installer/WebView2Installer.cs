using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.Core;

namespace WebView2Installer;

/// <summary>
/// A simple class that will check and, if needed, install the WebView2 Runtime locally
/// </summary>
public static class WebViewInstaller
{
    /// <summary>
    /// Subscribe to this Action to get the logs of what the EnsureWebView2Installed method is doing
    /// </summary>
    public static Action<string>? Logger { get; set; }
    
    /// <summary>
    /// Execute this to see if the WebView2 is installed locally
    /// </summary>
    public static async Task EnsureWebView2Installed()
    {
        try
        {
            Logger?.Invoke("Checking if WebView2 is installed");
            CoreWebView2Environment.GetAvailableBrowserVersionString();
            Logger?.Invoke("WebView2 already installed");
        }
        catch (WebView2RuntimeNotFoundException)
        {
            Logger?.Invoke("WebView2 is not installed");
            var arch = RuntimeInformation.ProcessArchitecture;
            var url = "";
            switch (arch)
            {
                case Architecture.X86:
                    url = "https://go.microsoft.com/fwlink/?linkid=2099617";
                    break;
                case Architecture.X64:
                    url = "https://go.microsoft.com/fwlink/?linkid=2124701";
                    break;
                case Architecture.Arm64:
                    url = "https://go.microsoft.com/fwlink/?linkid=2099616";
                    break;
                default:
                    Logger?.Invoke($"This architecture ({arch}) is not supported.");
                    break;
            }

            if (string.IsNullOrWhiteSpace(url))
                return;


            var file = Path.Join(AppContext.BaseDirectory, "webview2.exe");

            if (!(await TryDownload3Times(url, file)))
            {
                Logger?.Invoke($"Cannot download WebView2 from: {url}");
                return;
            }
            Logger?.Invoke("Starting WebView2 Installation");
            var process = Process.Start(file, "/silent /install");
            await process.WaitForExitAsync();
            
            Logger?.Invoke("WebView2 Installation finished");
        }
    }

    static async Task<bool> TryDownload3Times(string url, string file)
    {
        int attempts = 0;
        while (attempts < 2)
        {
            try
            {
                await DownloadFromUrl(url, file);
                return true;
            }
            catch (Exception)
            {
                Logger?.Invoke("Cannot download, retrying in 2 seconds");
                await Task.Delay(TimeSpan.FromSeconds(2));
                attempts++;
            }
        }

        return false;
    }
    static async Task DownloadFromUrl(string url, string file)
    {
        Logger?.Invoke("Downloading the WebView2 ...");
        using var client = new HttpClient();
        await using var s = await client.GetStreamAsync(url);
        await using var fs = new FileStream(file, FileMode.OpenOrCreate);
        await s.CopyToAsync(fs);
    }
}