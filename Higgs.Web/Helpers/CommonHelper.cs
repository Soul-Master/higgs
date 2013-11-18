using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using Higgs.Core.Helpers;
using System.Web.Caching;
using Higgs.Core.Security;
using System.Text;
using Microsoft.Ajax.Utilities;

namespace Higgs.Web.Helpers
{
    public static class CommonHelper
    {
        public static Dictionary<string, CombinedFileInfo> CombinedFiles
        {
            get
            {
                var combinedFiles = HttpContext.Current.Application["Higgs.CombinedFiles"] as Dictionary<string, CombinedFileInfo>;

                if (combinedFiles == null)
                {
                    combinedFiles = new Dictionary<string, CombinedFileInfo>();
                    HttpContext.Current.Application["Higgs.CombinedFiles"] = combinedFiles;
                }

                return combinedFiles;
            }
            set
            {
                HttpContext.Current.Application["Higgs.CombinedFiles"] = value;
            }
        }
        
        private static readonly object LockObj = new object();
        public static CombinedFileInfo CombinedFile(CombinedFileInfo combinedFile)
        {
            Func<string, string> mapPath = HttpContext.Current.Server.MapPath;
            var chkSum = HttpContext.Current.Cache[combinedFile.CombinedPath] as string;
            var hasCombinedFile = CombinedFiles.ContainsKey(combinedFile.CombinedPath);

            if (chkSum != null && hasCombinedFile && chkSum == CombinedFiles[combinedFile.CombinedPath].CheckSum) return CombinedFiles[combinedFile.CombinedPath];

            var combinedDirPath = Path.GetDirectoryName(mapPath(combinedFile.CombinedPath));
            var tempFile = new FileInfo(combinedFile.ExactFilePath);
            var isValidCachedFile = false;
            
            lock (LockObj)
            {
                if (tempFile.Directory != null && !tempFile.Directory.Exists) tempFile.Directory.Create();
                if (tempFile.Exists)
                {
                    if (hasCombinedFile)
                    {
                        tempFile.Delete();
                    }
                    else
                    {
                        var latestModifiedDate = combinedFile.FilePaths.Max(x => File.GetLastWriteTime(mapPath(x)));

                        if (tempFile.LastWriteTime < latestModifiedDate)
                        {
                            tempFile.Delete();
                        }
                        else
                        {
                            isValidCachedFile = true;
                        }
                    }
                }

                if (!isValidCachedFile)
                {
                    var temp = new StringBuilder();
                    Action<StringBuilder, string, string, bool> addFile = (sb, name, content, isMinified) =>
                    {
                        if(!isMinified)
                        {
                            content = combinedFile.FileType == CombinedFileType.JavaScript ? MinifyJavaScript(content) : MinifyCss(content);
                        }

                        if (!content.StartsWith("/*!")) sb.Add("/*! File: {0} */", name);

                        sb.AppendLine(content);
                    };

                    foreach (var filePath in combinedFile.FilePaths)
                    {
                        var cPath = mapPath(filePath);
                        var cExt = Path.GetExtension(cPath);
                        var minFilePath = cExt != null && !cExt.StartsWith(".min") ? Path.ChangeExtension(cPath, ".min" + cExt) : filePath;

                        if(combinedFile.FileType != CombinedFileType.Css && File.Exists(minFilePath))
                        {
                            addFile(temp, Path.GetFileName(cPath), File.ReadAllText(minFilePath), true);
                            continue;
                        }

                        var content = File.ReadAllText(cPath);
                        if (combinedFile.FileType == CombinedFileType.Css)
                        {
                            content = UpdateCssFilePath(content, combinedDirPath, Path.GetDirectoryName(cPath));
                        }

                        addFile(temp, Path.GetFileName(cPath), content, false);
                    }

                    using (var stream = tempFile.CreateText())
                    {
                        stream.Write(temp);
                        stream.Close();
                    }
                }

                using(var reader = tempFile.OpenRead())
                {
                    combinedFile.CheckSum = HashFunction.CheckSum(reader).EncodeToBase62();
                }

                if (!CombinedFiles.ContainsKey(combinedFile.CombinedPath))
                {
                    CombinedFiles[combinedFile.CombinedPath] = combinedFile;
                }

                if (chkSum == null)
                {
                    HttpContext.Current.Cache.Add
                    (
                        combinedFile.CombinedPath,
                        combinedFile.CheckSum,
                        new CacheDependency(combinedFile.FilePaths.Select(mapPath).ToArray().Add(combinedFile.ExactFilePath)),
                        DateTime.MaxValue,
                        TimeSpan.Zero,
                        CacheItemPriority.NotRemovable,
                        null
                    );
                }
            }

            return combinedFile;
        }
        
        static readonly Regex RegCssUrl = new Regex(@"([a-z-]+)[\s]*?:[^;}]*?url[\s]*?\(([^)]+)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static string UpdateCssFilePath(string content, string combinedDir, string cssDir)
        {
            var startIndex = 0;
            var match = RegCssUrl.Match(content, startIndex);
            var cacheUrl = new Dictionary<string, string>();

            while(match.Success)
            {
                // Prevent inline image (data URL) bug.
                if (!match.Groups[2].Value.StartsWith("data:"))
                {
                    if (!cacheUrl.ContainsKey(match.Groups[2].Value))
                    {
                        //var filePath = new Uri(new Uri(cssDir + (!cssDir.EndsWith("\\") ? "\\" : "")), match.Groups[2].Value).AbsolutePath;
                        var filePath = cssDir + (!cssDir.EndsWith("\\") ? "\\" : "") + match.Groups[2].Value.Replace('/', '\\');
                        filePath = IOHelpers.GetRelativePath(combinedDir, filePath);
                        cacheUrl[match.Groups[2].Value] = Uri.EscapeUriString(filePath);
                    }

                    content = content.Substring(0, match.Groups[2].Index) + cacheUrl[match.Groups[2].Value] +
                                    content.Substring(match.Groups[2].Index + match.Groups[2].Length);
                }

                startIndex = match.Index + match.Length;
                match = RegCssUrl.Match(content, startIndex);
            }

            return content;
        }
        
        static readonly CodeSettings _jsMinSettings = new CodeSettings
        {
            MinifyCode = true,
            OutputMode = OutputMode.SingleLine,
            EvalTreatment = EvalTreatment.Ignore,
            RemoveFunctionExpressionNames = true,
            LocalRenaming = LocalRenaming.KeepAll,
            PreserveFunctionNames = true,
            StripDebugStatements = true,
            TermSemicolons = true
        };

        public static string MinifyJavaScript(string content)
        {
            var ajax = new Minifier();
            
            return ajax.MinifyJavaScript(content, _jsMinSettings);
        }

        static readonly CssSettings _cssMinSettings = new CssSettings
        {
            CommentMode = CssComment.Hacks,
            ColorNames = CssColor.Hex,
            TermSemicolons = true,
            OutputMode = OutputMode.SingleLine
        };

        public static string MinifyCss(string content)
        {
            var ajax = new Minifier();

            return ajax.MinifyStyleSheet(content, _cssMinSettings);
        }

        public static string GetApplicationUrl(this HttpRequestBase request)
        {
            if (request.Url == null) return null;

            var temp = request.Url.GetLeftPart(UriPartial.Authority) + "/";

            if (request.ApplicationPath != "/")
                temp += request.ApplicationPath + "/";

            return temp.Substring(0, 8) + temp.Substring(8).Replace("//", "/");
        }

        public static string GetApplicationUrl(this HttpRequest request)
        {
            var temp = request.Url.GetLeftPart(UriPartial.Authority) + "/";

            if (request.ApplicationPath != "/")
                temp += request.ApplicationPath + "/";

            return temp.Substring(0, 8) + temp.Substring(8).Replace("//", "/");
        }

        public static string ResolveUrl(string logicalUrl, string applicationUrl = null)
        {
            if (logicalUrl == null) return null;
            if (logicalUrl.IndexOf("://") != -1) return logicalUrl;
 
            if (logicalUrl.StartsWith("~"))
            {
                if (applicationUrl != null)
                {
                    if (applicationUrl.EndsWith("/"))
                    {
                        applicationUrl = applicationUrl.Substring(0, applicationUrl.Length - 1);
                    }
                }
                else if (HttpContext.Current != null)
                {
                    applicationUrl = HttpContext.Current.Request.GetApplicationUrl();
                    logicalUrl = logicalUrl.Substring(1);
                }
                else
                {
                    throw new ArgumentException("Invalid URL: Relative URL not allowed.");
                }
                
                return applicationUrl + logicalUrl.Substring(1).Replace("//", "/");
            }
 
            return logicalUrl;
        }
    }
}
