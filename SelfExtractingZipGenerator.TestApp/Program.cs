
// This will get the current WORKING directory (i.e. \bin\Debug)
string workingDirectory = Environment.CurrentDirectory;
// or: Directory.GetCurrentDirectory() gives the same result
// This will get the current PROJECT directory
string slnDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;

string filesToPackage = Path.Combine(slnDirectory, @"SelfExtractingZipGenerator.ConsoleApp\bin\Release\net8.0\publish\win-x64");


await SelfExtractingZipGenerator.ZipGenerator.GenerateSelfExtractingZipAsync(filesToPackage, Environment.CurrentDirectory, "ConsoleAppZipInstall.exe", null, "Install Console App", "%TEMP%\\ConsoleApp", "SelfExtractingZipGenerator.ConsoleApp.exe", false);


Console.WriteLine("Hello, World!");