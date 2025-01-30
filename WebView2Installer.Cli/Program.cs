using WebView2Installer;

WebViewInstaller.Logger += log => Console.WriteLine(log);
await WebViewInstaller.EnsureWebView2Installed();
