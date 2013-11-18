using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Higgs.Core.Helpers;
using System.Web.Mvc;

namespace Higgs.Web.Helpers
{
    public static class ResourceHelper
    {
        public static FilePathResult GetJavaScriptResource(this Assembly resourceAssembly, string controllerName, string resourceName)
        {
            var cachedJavaScriptFile = string.Format("~/{0}/Resources/{1}/{2}.{3}.js", WebAppConfig.AppCacheDirectory, controllerName, resourceName, System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
            var physicalPath = HttpContext.Current.Server.MapPath(cachedJavaScriptFile);
            
            if (!File.Exists(physicalPath))
            {
                var requestedType = resourceAssembly.GetTypes().SingleOrDefault(x => x.FullName.EndsWith(controllerName + "." + resourceName));

                if (requestedType == null)
                {
                    throw new FileNotFoundException();
                }

                var sb = new StringBuilder();

                sb.Add("if (typeof Views == 'undefined')");
                sb.Add("    window.Views = Views = {};");
                sb.Add("if (typeof Views.{0} == 'undefined')", controllerName);
                sb.Add("    window.Views.{0} = Views.{0} = {{}};", controllerName);
                sb.Add("if (typeof Views.{0}.{1} == 'undefined')", controllerName, resourceName);
                sb.Add("    window.Views.{0}.{1} = Views.{0}.{1} = ", controllerName, resourceName);
                sb.Add("    {");

                var properties = requestedType.GetProperties();
                for (var i = 0; i < properties.Length; i++)
                {
                    if (properties[i].CanRead)
                    {
                        sb.Add("        {0} : {1}{2}", properties[i].Name, properties[i].GetValue(null, null).ToEscapeString(), ((i == properties.Length - 1) ? "" : ","));
                    }
                }

                sb.Add("    };");
                sb.Add("var {1} = Views.{0}.{1};", controllerName, resourceName);

                WriteFileContent(physicalPath, sb.ToString());
            }
            else
            {
#if DEBUG
                File.Delete(physicalPath);
#endif
            }

            return new FilePathResult(cachedJavaScriptFile, "application/x-javascript");
        }

        public static void WriteFileContent(string filePath, string fileContent)
        {
            FileStream fs;

            if (!File.Exists(filePath))
            {
                var dir = Path.GetDirectoryName(filePath);
                if (dir == null) throw new Exception();

                Directory.CreateDirectory(dir);
                File.Create(filePath).Close();
            }

            using (fs = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.Read))
            {
                StreamWriter sw;
                using (sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.BaseStream.SetLength(fileContent.Length);
                    sw.Write(fileContent);
                    sw.Flush();
                }
            }
        }

        public static void CreateDirectory(string directoryPath)
        {
            var temp = new DirectoryInfo(directoryPath);
            var directoryPaths = new List<string>();

            while (temp != null && !temp.Exists)
            {
                directoryPaths.Insert(0, temp.FullName);

                temp = temp.Parent;
            }

            foreach (var s in directoryPaths)
            {
                Directory.CreateDirectory(s);
            }
        }
    }
}
