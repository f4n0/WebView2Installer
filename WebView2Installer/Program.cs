// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.Core;

Console.WriteLine("Hello, World!");

try
{
    CoreWebView2Environment.GetAvailableBrowserVersionString();
}
catch (WebView2RuntimeNotFoundException)
{
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
            Console.WriteLine($"This architecture ({arch}) is not supported.");
            break;
    }

    if (string.IsNullOrWhiteSpace(url))
        return;

    Console.WriteLine($"Downloading WebView2 from: {url}");

    var file = Path.Join(AppContext.BaseDirectory, "webview2.exe");

    if (!(await TryDownload3Times(url, file)))
    {
        Console.WriteLine($"Cannot download WebView2 from: {url}");
        return;
    }
    Console.WriteLine("Installing WebView2");
    var process = Process.Start(file, "/silent /install");
    process.WaitForExit();
}

Console.WriteLine("All Set Up!");
Console.ReadKey();


async Task<bool> TryDownload3Times(string url, string file)
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
            Console.WriteLine("Cannot download, retrying in 2 seconds");
            await Task.Delay(TimeSpan.FromSeconds(2));
            attempts++;
        }
        
    }
    return false;
}


async Task DownloadFromUrl(string url, string file)
{
    using var client = new HttpClient();
    await using var s = await client.GetStreamAsync(url);
    await using var fs = new FileStream(file, FileMode.OpenOrCreate);
    await s.CopyToAsync(fs);
}