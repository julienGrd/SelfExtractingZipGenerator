A Self extracting zip Generator which run on .net Core, 

Allow for example to package another .net app and create some kind of silent installer you can deploy to your clients. Sometimes its  easier to use this solution than create a full installer project.

## How to run
- Extract the code
- Publish at least one time SelfExtractingZipGenerator.ConsoleApp
- Run SelfExtractingZipGenerator.TestApp
- You should find the exe ConsoleAppZipInstall.exe.exe generated in this folder \SelfExtractingZipGenerator\SelfExtractingZipGenerator.TestApp\bin\Debug\net8.0
- Run it, the app will start directly, and it should be installed in the %TEMP%\\ConsoleApp folder

## Options
- string pPathFiles : path to the file which want to package, here the publish folder of the ConsoleApp
- string pPathToSave : path where the generated exe will be copied, here Environment.CurrentDirectory so the debug folder of the TestApp
- string pFilenameToGenerate : filename of the generated exe, here ConsoleAppZipInstall.exe
- string[] pFilesToExclude : optional array of files if some files need to be exclude, 
- string pTitle : text shown when the exe is launch
- string pInstallPath : path where the zip will be installed. special path like %TEMP% or %AppData% can be used, here its %TEMP%\\ConsoleApp
- string pExeToLaunch : the name of the file you want be launched when the app is installed. Should exist in the zip of course, here its ou console app SelfExtractingZipGenerator.ConsoleApp.exe
- bool pWithAdmin : this need to be true if one of these two conditions are met :
    - your install path need admin rights (for example install on C: or Program files)
    - the program you package itself need admin right, if you force it with a manifest for example

## Improvements
Very basic for now, can be improved on many way :
- add support for linux, even if the idea of a self extracting executable dont make so sense in linux. But maybe there is something to do.
- making it a service rather than a static class, with an injection of ILogger to trace all call to cmd
- more options, personnalization,
- make a nuget package, etc

The code itself is very simple because its basically a wrapper to the 7zip library which include in the project.
