# ComboScrapper

A C# tool designed to process URL logs containing credentials in the format `URL:EMAIL:PASSWORD` and organize them into separate files based on email domains and usernames.

## Features
- Extracts email/password combinations from text files
- Sorts emails by popular domains (loaded from `domains.json`)
- Separates username/password combinations without @ symbols
- Creates organized output folders with timestamp
- Displays statistics of processed data
- User-friendly console interface

## Prerequisites
- .NET Framework (version compatible with Windows Forms)
- Newtonsoft.Json NuGet package
- Windows operating system (due to Windows Forms dependency)

## Installation
1. Clone or download the repository
2. Open the solution in Visual Studio
3. Install the Newtonsoft.Json NuGet package:
