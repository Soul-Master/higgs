using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Higgs.Core.Helpers;

namespace Higgs.DeployUtils
{
    class Program
    {
        static void Main(string[] args)
        {
            var projectDir = new DirectoryInfo(args[0]);
            var deployDir = new DirectoryInfo(args[1]);

            var viewPages = projectDir.GetFiles("*.cshtml", SearchOption.AllDirectories);

            MinifyJs(viewPages, deployDir.FullName);
            MinifyCss(viewPages, deployDir.FullName);
            RemoveEmptyDir(deployDir.FullName);
        }

        static void MinifyJs(IEnumerable<FileInfo> viewPages, string deployDir)
        {
            Console.WriteLine("Minify JavaScript");

            var minifyList = new Dictionary<string, bool>();
            var regex = new Regex(@"#minify:([^\n]+.js)[^\n]*\n[\s\S]+?#end", RegexOptions.Compiled);
            var srcRegex = new Regex(@"src=""([^\""]+.js)""", RegexOptions.Compiled);

            foreach (var page in viewPages)
            {
                var content = File.ReadAllText(page.FullName);

                var scriptResults = regex.Matches(content);

                foreach (Match m in scriptResults)
                {
                    var scriptText = m.Value;
                    var logicalOutputPath = m.Groups[1].Value.Trim();
                    var outputFileFullName = IOHelpers.GetFullPath(logicalOutputPath, deployDir);
                    var outputFile = IOHelpers.GetRelativePath(deployDir, outputFileFullName);
                    var inputFileList = new List<string>();
                    var inputFiles = "";

                    foreach (Match scriptMatch in srcRegex.Matches(scriptText))
                    {
                        var inputFileFullName = IOHelpers.GetFullPath(scriptMatch.Groups[1].Value.Trim(), deployDir);
                        inputFileList.Add(inputFileFullName);

                        inputFiles += IOHelpers.GetRelativePath(deployDir, inputFileFullName) + " ";
                    }

                    inputFiles = inputFiles.Trim();

                    if (File.Exists(outputFileFullName)) continue;

                    if (ExecuteMinifyJs(inputFiles, outputFile, deployDir))
                    {
                        inputFileList.ForEach(File.Delete);
                        minifyList[outputFileFullName] = true;
                    }
                }
            }

            // Minify all other files
            foreach (var filePath in Directory.GetFiles(deployDir, "*.js", SearchOption.AllDirectories))
            {
                if (minifyList.ContainsKey(filePath)) continue;
                
                ExecuteMinifyJs(filePath, filePath, deployDir);
            }
        }
        
        static bool ExecuteMinifyJs(string inputFiles, string outputFile, string workingDirectory)
        {
            var cmdText = "uglifyjs " + inputFiles + " -o " + outputFile;
            Console.WriteLine("- " + IOHelpers.GetRelativePath(workingDirectory, outputFile) + " ");

            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                FileName = "cmd.exe",
                WorkingDirectory = workingDirectory,
                Arguments = "/C \"" + cmdText + "\"",
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

        static void MinifyCss(IEnumerable<FileInfo> viewPages, string deployDir)
        {
            Console.WriteLine("Minify CSS");

            var minifyList = new Dictionary<string, bool>();
            // Raw: @if[^\{]+{(\s*<link [^>]*href="([^"]+)"[^>]*/>\s*){2,}}\s*else\s*{\s*<link [^>]*href="([^"]+)"[^>]*data-minify[^>]*/>\s*}
            var regex = new Regex("@if[^\\{]+{(\\s*<link [^>]*href=\"([^\"]+)\"[^>]*/>\\s*){2,}}\\s*else\\s*{\\s*<link [^>]*href=\"([^\"]+)\"[^>]*data-minify[^>]*/>\\s*}", RegexOptions.Compiled);
            
            foreach (var page in viewPages)
            {
                var content = File.ReadAllText(page.FullName);

                var scriptResults = regex.Matches(content);

                foreach (Match m in scriptResults)
                {
                    var logicalOutputPath = m.Groups[3].Value;
                    var outputFileFullName = IOHelpers.GetFullPath(logicalOutputPath, deployDir);
                    var outputFile = IOHelpers.GetRelativePath(deployDir, outputFileFullName);
                    var inputFileList = new List<string>();
                    var inputFiles = string.Empty;

                    foreach (Capture c in m.Groups[2].Captures)
                    {
                        var inputFileFullName = IOHelpers.GetFullPath(c.Value, deployDir);
                        inputFileList.Add(inputFileFullName);

                        inputFiles += IOHelpers.GetRelativePath(deployDir, inputFileFullName) + " ";
                    }

                    inputFiles = inputFiles.Trim();

                    if (ExecuteMinifyCss(inputFiles, outputFile, deployDir))
                    {
                        inputFileList.ForEach(File.Delete);
                        minifyList[outputFileFullName] = true;
                    }
                }
            }

            // Minify all other files
            foreach (var filePath in Directory.GetFiles(deployDir, "*.css", SearchOption.AllDirectories))
            {
                if (minifyList.ContainsKey(filePath)) continue;

                ExecuteMinifyCss(filePath, filePath, deployDir);
            }
        }
        
        static bool ExecuteMinifyCss(string inputFiles, string outputFile, string workingDirectory)
        {
            var cmdText = "cleancss " + inputFiles + " -o " + outputFile;
            Console.WriteLine("- " + IOHelpers.GetRelativePath(workingDirectory, outputFile) + " ");

            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                FileName = "cmd.exe",
                WorkingDirectory = workingDirectory,
                Arguments = "/C \"" + cmdText + "\"",
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
    }
}
