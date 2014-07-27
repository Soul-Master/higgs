﻿using System;
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
            var projectDir = new DirectoryInfo(args[0]);
            var deployDir = new DirectoryInfo(args[1]);

            var viewPages = projectDir.GetFiles("*.cshtml", SearchOption.AllDirectories);
            MinifyJs(viewPages, projectDir.FullName, deployDir.FullName);
        }

        static void MinifyJs(FileInfo[] viewPages, string projectDir, string deployDir)
        {
            var regex = new Regex(@"#minify:([^\n]+.js)[^\n]*\n[\s\S]+#end", RegexOptions.Compiled);
            var srcRegex = new Regex(@"src=""([^\""]+.js)""", RegexOptions.Compiled);

            foreach (var page in viewPages)
            {
                var content = File.ReadAllText(page.FullName);

                if (content.IndexOf("#minify:", StringComparison.Ordinal) == -1) continue;

                var scriptResults = regex.Matches(content);

                foreach (Match m in scriptResults)
                {
                    var scriptText = m.Value;
                    var outputFileFullName = IOHelpers.GetFullPath(m.Groups[1].Value.Trim(), deployDir);
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
                    
                    if (File.Exists(outputFileFullName)) File.Delete(outputFileFullName);

                    var cmdText = "uglifyjs " + inputFiles + " -o " + outputFile;

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

                    if (File.Exists(outputFileFullName))
                    {
                        inputFileList.ForEach(File.Delete);
                    }
                    else
                    {
                        throw new Exception("Unable to generate minify file.");
                    }
                }
            }
        }
    }
}
