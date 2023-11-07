using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Higgs.Core.Helpers;

namespace Higgs.WebOptimizer
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            Console.SetOut(new DebugWriter());
            var projectDir = new DirectoryInfo(@"D:\CBSPayment\CBSPayment");
            var deployDir = new DirectoryInfo(@"D:\CBSPayment.Deploy");
            if (deployDir.Exists) deployDir.Delete(true);
            deployDir.Create();
            DirectoryCopy(projectDir.FullName, deployDir.FullName, true);
            DeleteDirectory(Path.Combine(deployDir.FullName, "bin"));
            DeleteDirectory(Path.Combine(deployDir.FullName, ".bin"));
            DeleteDirectory(Path.Combine(deployDir.FullName, "obj"));
            DeleteDirectory(Path.Combine(deployDir.FullName, "Properties"));
            DeleteFiles(deployDir.FullName, "ts,less");
#else
            var projectDir = new DirectoryInfo(args[0]);
            var deployDir = new DirectoryInfo(args[1]);
#endif
            var viewDir = new DirectoryInfo(Path.Combine(projectDir.FullName, "Views"));
            var viewPages = viewDir.GetFiles("*.cshtml", SearchOption.AllDirectories);

            MinifyJs(viewPages, deployDir.FullName);
            MinifyCss(viewPages, deployDir.FullName);
            RemoveEmptyDir(deployDir.FullName);
        }

        static void MinifyJs(IEnumerable<FileInfo> viewPages, string workingDir)
        {
            Console.WriteLine("Minify JavaScript");

            // Raw: @if[^\{]+{(\s*<script [^>]*src="([^"]+)"[^>]*><\/script>\s*){2,}}\s*else\s*{\s*<script [^>]*src="([^"]+)"[^>]*data-minify[^>]*><\/script>\s*}
            var pattern = new Regex("@if[^\\{]+{(\\s*<script [^>]*src=\"([^\"]+)\"[^>]*><\\/script>\\s*){2,}}\\s*else\\s*{\\s*<script [^>]*src=\"([^\"]+)\"[^>]*data-minify[^>]*><\\/script>\\s*}", RegexOptions.Compiled);

            MinifyFile(viewPages, workingDir, pattern, "js", (inputFiles, outputFile) =>
            {
                var cmd = "terser " + inputFiles + " --compress warnings=false --mangle --output " + outputFile;

                return ExecuteProcess(workingDir, cmd);
            });
        }

        static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        static void DeleteFiles(string path, string extension)
        {
            var extensions = extension.Split(',');
            foreach (var f in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
            {
                var ext = Path.GetExtension(f);
                if(ext.Length > 0 && extensions.Contains(ext.Substring(1)))
                {
                    File.Delete(f);
                }
            }
        }

        static void MinifyCss(IEnumerable<FileInfo> viewPages, string workingDir)
        {
            Console.WriteLine("Minify CSS");

            // Raw: @if[^\{]+{(\s*<link [^>]*href="([^"]+)"[^>]*/>\s*){2,}}\s*else\s*{\s*<link [^>]*href="([^"]+)"[^>]*data-minify[^>]*/>\s*}
            var pattern = new Regex("@if[^\\{]+{(\\s*<link [^>]*href=\"([^\"]+)\"[^>]*/>\\s*){2,}}\\s*else\\s*{\\s*<link [^>]*href=\"([^\"]+)\"[^>]*data-minify[^>]*/>\\s*}", RegexOptions.Compiled);

            MinifyFile(viewPages, workingDir, pattern, "css", (inputFiles, outputFile) =>
            {
                var cmd = "cleancss " + inputFiles + " -o " + outputFile;

                return ExecuteProcess(workingDir, cmd);
            });
        }

        static void RemoveEmptyDir(string dir)
        {
            foreach (var directory in Directory.GetDirectories(dir))
            {
                RemoveEmptyDir(directory);

                if (Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        static void MinifyFile(IEnumerable<FileInfo> viewPages, string workingDir, Regex pattern, string extension, Func<string, string, bool> minifyFn)
        {
            var outputFiles = new Dictionary<string, bool>();

            // Search for multiple files minification.
            foreach (var page in viewPages)
            {
                var content = File.ReadAllText(page.FullName);

                var scriptResults = pattern.Matches(content);

                foreach (Match m in scriptResults)
                {
                    var logicalOutputPath = m.Groups[3].Value;

                    // Prevent ?query suffix
                    if (!logicalOutputPath.EndsWith("." + extension, StringComparison.CurrentCultureIgnoreCase))
                    {
                        var extensionLength = ("." + extension).Length;
                        logicalOutputPath = logicalOutputPath.Substring(0, logicalOutputPath.LastIndexOf("." + extension, StringComparison.CurrentCultureIgnoreCase) + extensionLength);
                    }

                    var outputFileFullName = IOHelpers.GetFullPath(logicalOutputPath, workingDir);
                    var outputFile = IOHelpers.GetRelativePath(workingDir, outputFileFullName);
                    var inputFileList = new List<string>();
                    var inputFiles = string.Empty;

                    if (outputFiles.ContainsKey(outputFileFullName)) continue;

                    foreach (Capture c in m.Groups[2].Captures)
                    {
                        var inputFileFullName = IOHelpers.GetFullPath(c.Value, workingDir);
                        inputFileList.Add(inputFileFullName);

                        inputFiles += IOHelpers.GetRelativePath(workingDir, inputFileFullName) + " ";
                    }

                    inputFiles = inputFiles.Trim();

                    Console.WriteLine("- " + IOHelpers.GetRelativePath(workingDir, outputFile));
                    if (minifyFn(inputFiles, outputFile))
                    {
                        outputFiles[outputFileFullName] = true;

                        inputFileList.ForEach(File.Delete);
                    }
                    else
                    {
                        Console.WriteLine("Error: " + outputFile);
                    }
                }
            }
            
            // Minify all other files
            foreach (var filePath in Directory.GetFiles(workingDir, "*." + extension, SearchOption.AllDirectories))
            {
                if (outputFiles.ContainsKey(filePath)) continue;

                Console.WriteLine("- " + IOHelpers.GetRelativePath(workingDir, filePath));
                if (!minifyFn(filePath, filePath))
                {
                    Console.WriteLine("Error: " + filePath);
                }
            }
        }

        static bool ExecuteProcess(string workingDir, string cmd)
        {
            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                FileName = "cmd.exe",
                WorkingDirectory = workingDir,
                Arguments = "/C \"" + cmd + "\"",
                RedirectStandardInput = true,
                RedirectStandardError = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                UseShellExecute = false
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            return process.ExitCode == 0;
        }
    }
}
