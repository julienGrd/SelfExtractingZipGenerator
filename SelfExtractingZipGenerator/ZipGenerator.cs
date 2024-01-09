using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SelfExtractingZipGenerator
{
    public static class ZipGenerator
    {
        public static void GenerateSelfExtractingZip(string pPathFiles, string pPathToSave, string pFilenameToGenerate, string[] pFilesToExclude, string pTitle, string pInstallPath, string pExeToLaunch, bool pWithAdmin)
        {
            var task = Task.Run(() => GenerateSelfExtractingZipAsync(pPathFiles, pPathToSave, pFilenameToGenerate, pFilesToExclude, pTitle, pInstallPath, pExeToLaunch, pWithAdmin));
            task.Wait();
            //var response = task.Result;
            //return response;
        }
        public static async Task GenerateSelfExtractingZipAsync(string pPathFiles, string pPathToSave, string pFilenameToGenerate, string[] pFilesToExclude, string pTitle, string pInstallPath, string pExeToLaunch, bool pWithAdmin)
        {
            var lFinalFile = Path.Combine(pPathToSave, pFilenameToGenerate + ".exe");
            if (File.Exists(lFinalFile))
                File.Delete(lFinalFile);
            //BeginPrompt=""Do you want to install SOFTWARE v1.0.0.0?""
            var lConfig = @$";!@Install@!UTF-8!
Title=""{pTitle}""      
InstallPath=""{pInstallPath}""
RunProgram=""{pExeToLaunch}""
;!@InstallEnd@!";

            if (File.Exists("config.txt"))
                File.Delete("config.txt");

            await File.WriteAllTextAsync("config.txt", lConfig, encoding: Encoding.UTF8);

            var lProcess = new Process();
            //lProcess.StartInfo.WorkingDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "7zip");
            //lProcess.StartInfo.WorkingDirectory = ;
            lProcess.StartInfo.FileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "7zip", "7z.exe");
            //lProcess.StartInfo.Arguments = $"a -t7z -mx5 -sfx {pPathToSave} {pPathFiles} -mmt";
            lProcess.StartInfo.Arguments = $"a -r -t7z {pFilenameToGenerate}.7z {pPathFiles}/* -mmt";
            if (pFilesToExclude?.Any() ?? false)
            {
                lProcess.StartInfo.Arguments += " -xr!" + string.Join(" -xr!", pFilesToExclude);
            }
            lProcess.StartInfo.CreateNoWindow = true;
            lProcess.StartInfo.UseShellExecute = false;

            lProcess.StartInfo.RedirectStandardInput = true;
            lProcess.StartInfo.RedirectStandardOutput = true;
            lProcess.StartInfo.RedirectStandardError = true;
            //lProcess.StartInfo.StandardErrorEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);
            //lProcess.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);
            if (lProcess.Start())
            {
                var lOutput = await lProcess.StandardOutput.ReadToEndAsync();

                var lError = await lProcess.StandardError.ReadToEndAsync();

                if (!string.IsNullOrEmpty(lError))
                    throw new Exception(lError);

                await lProcess.WaitForExitAsync();

                //if(File.Exists(pPathToSave))
                //{
                //    var lBytes = File.ReadAllBytes(pPathToSave);
                //    File.Delete(pPathToSave);

                //    return lBytes;
                //}
                //else
                //    throw new FileNotFoundException("Archive introuvable");
            }
            else
                throw new Exception("Impossible de lancer 7zip");

            lProcess = new Process();
            //lProcess.StartInfo.WorkingDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "7zip");
            //lProcess.StartInfo.WorkingDirectory = ;
            var lSfx = pWithAdmin ? "7zSD_Admin.sfx" : "7zSD_Normal.sfx";

            lProcess.StartInfo.FileName = "cmd.exe";
            lProcess.StartInfo.Arguments = $"/C copy /b 7zip\\{lSfx} + config.txt + {pFilenameToGenerate}.7z {lFinalFile}";
            lProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            lProcess.StartInfo.CreateNoWindow = true;
            //lProcess.StartInfo.UseShellExecute = false;

            lProcess.StartInfo.RedirectStandardInput = true;
            lProcess.StartInfo.RedirectStandardOutput = true;
            lProcess.StartInfo.RedirectStandardError = true;
            //lProcess.StartInfo.StandardErrorEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);
            //lProcess.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);
            if (lProcess.Start())
            {
                var lOutput = await lProcess.StandardOutput.ReadToEndAsync();

                var lError =await  lProcess.StandardError.ReadToEndAsync();

                //if (!string.IsNullOrEmpty(lOutput))
                //    Console.WriteLine(lOutput);
                if (!string.IsNullOrEmpty(lError))
                    throw new Exception(lError);

                await lProcess.WaitForExitAsync();

                //if(File.Exists(pPathToSave))
                //{
                //    var lBytes = File.ReadAllBytes(pPathToSave);
                //    File.Delete(pPathToSave);

                //    return lBytes;
                //}
                //else
                //    throw new FileNotFoundException("Archive introuvable");
            }
            else
                throw new Exception("Impossible de lancer copy");

            File.Delete("config.txt");

            if (File.Exists($"{pFilenameToGenerate}.7z"))
                File.Delete($"{pFilenameToGenerate}.7z");
        }
    }
}
