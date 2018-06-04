# Mandelbrot
An implementation of a binary protocol for distributed computation of mandelbrot sets.
This repository focuses on a server, but a stub client is provided for testing purpose.

To build this C# soulution you'll need: 

- dotnet-sdk-2.0: (https://www.microsoft.com/net/learn/get-started/)
- mono-devel-5.12: (https://www.mono-project.com/download/stable/) (Or .net Framework SDK 4.7)
- gtk-sharp2: (https://www.mono-project.com/download/stable/)
- MSBuild 15.0 along with msbuild package provided by mono package source

Additionally the following NuGet package which is not provided by nuget.org:
https://aka.ms/xf-xamlstandard-nuget
