# Simple WebView2 Runtime Checker

## Overview
This C# Library checks if the Microsoft WebView2 Runtime is installed on the system. If the runtime is not found, the application will automatically download and install it.

## Prerequisites
- Windows operating system

## Usage
You have 2 ways:
1. Download Release.zip from the Releases page. This contains the Console application ready to Run
2. Add the WebView2Installer nuget package to your solution. The package is available from Nuget.org

## How It Works
1. The application checks the system for the WebView2 Runtime.
2. If found, the program exits with a success message.
3. If not found, the application downloads the WebView2 Runtime installer from the official Microsoft source.
4. The installer runs automatically to install WebView2.
5. Once the installation is complete, the program confirms successful installation.

## Notes
- Administrator privileges may be required to install WebView2.
- An internet connection is necessary to download the runtime.


