# Simple WebView2 Runtime Checker

## Overview
This C# Console Application checks if the Microsoft WebView2 Runtime is installed on the system. If the runtime is not found, the application will automatically download and install it.

## Prerequisites
- Windows operating system

## Installation
1. Download the `Release` folder (or download Release.zip from the Releases page)
3. Run the executable (`WebView2Installer.exe`).

## How It Works
1. The application checks the system for the WebView2 Runtime.
2. If found, the program exits with a success message.
3. If not found, the application downloads the WebView2 Runtime installer from the official Microsoft source.
4. The installer runs automatically to install WebView2.
5. Once the installation is complete, the program confirms successful installation.

## Notes
- Administrator privileges may be required to install WebView2.
- An internet connection is necessary to download the runtime.


