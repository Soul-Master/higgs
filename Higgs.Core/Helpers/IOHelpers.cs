using System;
using System.IO;

namespace Higgs.Core.Helpers
{
    public static class IOHelpers
    {
        public static void WriteFile(this FileInfo file, Stream inputStream)
        {
            if (file.Exists) file.Delete();

            const int length = 256;
            var buffer = new Byte[length];
            var bytesRead = inputStream.Read(buffer, 0, length);
            var outputStream = file.Create();

            while(bytesRead > 0) 
            {
                outputStream.Write(buffer, 0, bytesRead);
                bytesRead = inputStream.Read(buffer, 0, length);
            }

            inputStream.Close();
            outputStream.Close();
        }

        public static string GetLogicalPath(string baseDirectory, string filePath)
        {
            return "~/" + filePath.Replace(baseDirectory, "").TrimStart('\\').Replace("\\", "/");
        }

        public static string GetRelativePath(string currentDirectory, string filePath)
        {
            var uriFile = new Uri(filePath);
            var uriCurrentDir = new Uri(currentDirectory + (!currentDirectory.EndsWith("\\") ? "\\" : ""));

            return uriCurrentDir.MakeRelativeUri(uriFile).ToString();
        }

        public static string GetFullPath(string path, DirectoryInfo baseDirectory)
        {
            return GetFullPath(path, baseDirectory.FullName);
        }

        public static string GetFullPath(string path, string baseDirectory = null)
        {
            if(string.IsNullOrWhiteSpace(baseDirectory))
            {
                baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            path = path.Replace("\\","/");
            baseDirectory = baseDirectory.Replace("/","\\");

            if (baseDirectory.EndsWith("\\"))
            {
                baseDirectory = baseDirectory.Substring(0, baseDirectory.Length - 1);
            }

            if (Path.IsPathRooted(path))
            {
                return path;
            }

            if (path.StartsWith("./") || path.StartsWith("~/"))
            {
                return Path.Combine(baseDirectory, path.Substring(2)).Replace("/","\\");
            }

            while (path.StartsWith("../"))
            {
                if (!baseDirectory.EndsWith("\\\\") && !baseDirectory.EndsWith(":\\"))
                {
                    baseDirectory = baseDirectory.LastIndexOf("\\") - baseDirectory.IndexOf("\\") > 1 ? 
                                                                                                          baseDirectory.Substring(0, baseDirectory.LastIndexOf("\\")) : 
                                                                                                                                                                          baseDirectory.Substring(0, baseDirectory.LastIndexOf("\\") + 1);
                }

                path = path.Substring(3);
            }

            return Path.Combine(baseDirectory, path.Replace("/", "\\"));
        }
    }
}
