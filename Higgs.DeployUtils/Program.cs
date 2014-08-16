using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Higgs.Core.Helpers;

namespace Higgs.DeployUtils
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new[] { @"D:\ChemInvent\ChemInvent", @"D:\ChemInvent\ChemInvent\obj\Release\Package\PackageTmp" };
            var projectDir = new DirectoryInfo(args[0]);
            var deployDir = new DirectoryInfo(args[1]);

            var viewPages = projectDir.GetFiles("*.cshtml", SearchOption.AllDirectories);

            MinifyJs(viewPages, deployDir.FullName);
            MinifyCss(viewPages, deployDir.FullName);
            RemoveEmptyDir(deployDir.FullName);
        }

        static void MinifyJs(FileInfo[] viewPages, string deployDir)
        {
            Console.WriteLine("  Minify JavaScript");

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

                    var cmdText = "uglifyjs " + inputFiles + " -o " + outputFile;
                    Console.Write("  - " + outputFile + " ");

                    var process = new System.Diagnostics.Process();
                    var startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    startInfo.FileName = "cmd.exe";
                    startInfo.WorkingDirectory = deployDir;
                    startInfo.Arguments = "/C \"" + cmdText + "\"";
                    startInfo.RedirectStandardInput = true;
                    startInfo.RedirectStandardError = true;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.CreateNoWindow = true;
                    startInfo.UseShellExecute = false;
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        inputFileList.ForEach(File.Delete);

                        Console.Write("Done" + Environment.NewLine);
                        minifyList[outputFileFullName] = true;
                    }
                    else
                    {
                        Console.Write("Failed" + Environment.NewLine);
                    }
                }
            }

            // Minify all other files
            foreach (var filePath in Directory.GetFiles(deployDir, "*.js", SearchOption.AllDirectories))
            {
                if (minifyList.ContainsKey(filePath)) continue;

                var cmdText = "uglifyjs " + filePath + " -o " + filePath;
                Console.Write("  - " + IOHelpers.GetRelativePath(deployDir, filePath) + " ");

                var process = new System.Diagnostics.Process();
                var startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                startInfo.FileName = "cmd.exe";
                startInfo.WorkingDirectory = deployDir;
                startInfo.Arguments = "/C \"" + cmdText + "\"";
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Console.Write("Done" + Environment.NewLine);
                }
                else
                {
                    Console.Write("Failed" + Environment.NewLine);
                }
            }
        }

        static void MinifyCss(FileInfo[] viewPages, string deployDir)
        {
            Console.WriteLine("  Minify CSS");

            var minifyList = new Dictionary<string, bool>();
            var regex = new Regex(@"#minify:([^\n]+.css)[^\n]*\n[\s\S]+?#end", RegexOptions.Compiled);
            var srcRegex = new Regex(@"href=""([^\""]+.css)""", RegexOptions.Compiled);

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

                    var cmdText = "cleancss " + inputFiles + " -o " + outputFile;
                    Console.Write("  - " + outputFile + " ");

                    var process = new System.Diagnostics.Process();
                    var startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    startInfo.FileName = "cmd.exe";
                    startInfo.WorkingDirectory = deployDir;
                    startInfo.Arguments = "/C \"" + cmdText + "\"";
                    startInfo.RedirectStandardInput = true;
                    startInfo.RedirectStandardError = true;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.CreateNoWindow = true;
                    startInfo.UseShellExecute = false;
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        inputFileList.ForEach(File.Delete);

                        minifyList[outputFileFullName] = true;
                        Console.Write("Done" + Environment.NewLine);
                    }
                    else
                    {
                        Console.Write("Failed" + Environment.NewLine);
                    }
                }
            }

            // Minify all other files
            foreach (var filePath in Directory.GetFiles(deployDir, "*.css", SearchOption.AllDirectories))
            {
                if (minifyList.ContainsKey(filePath)) continue;

                var cmdText = "cleancss " + filePath + " -o " + filePath;
                Console.Write("  - " + IOHelpers.GetRelativePath(deployDir, filePath) + " ");

                var process = new System.Diagnostics.Process();
                var startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                startInfo.FileName = "cmd.exe";
                startInfo.WorkingDirectory = deployDir;
                startInfo.Arguments = "/C \"" + cmdText + "\"";
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Console.Write("Done" + Environment.NewLine);
                }
                else
                {
                    Console.Write("Failed" + Environment.NewLine);
                }
            }
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
