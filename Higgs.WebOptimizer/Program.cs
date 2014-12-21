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
            var projectDir = new DirectoryInfo(args[0]);
            var deployDir = new DirectoryInfo(args[1]);

            var viewPages = projectDir.GetFiles("*.cshtml", SearchOption.AllDirectories);

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
                var cmd = "uglifyjs " + inputFiles + " -o " + outputFile;

                return ExecuteProcess(workingDir, cmd);
            });
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
                }
            }

            // Minify all other files
            foreach (var filePath in Directory.GetFiles(workingDir, "*." + extension, SearchOption.AllDirectories))
            {
                if (outputFiles.ContainsKey(filePath)) continue;

                Console.WriteLine("- " + IOHelpers.GetRelativePath(workingDir, filePath));
                minifyFn(filePath, filePath);
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
