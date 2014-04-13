using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Higgs.Core.Helpers;

namespace Higgs.Web.Helpers
{
    public static class PageHelper
    {
        public static MvcHtmlString JavaScript(this HtmlHelper helper, string path, string javaScript)
        {
            return new MvcHtmlString(String.Format
            (
                "<script type=\"text/javascript\" {0}>{1}</script>",  
                (String.IsNullOrEmpty(path)?String.Empty: "src='" + path + "' "),
                (String.IsNullOrEmpty(javaScript) ? String.Empty : Environment.NewLine + javaScript + Environment.NewLine)
            ));
        }

        public static MvcHtmlString StyleSheetFiles(this HtmlHelper helper, params string[] path)
        {
            var sb = new StringBuilder();

            foreach (var p in path)
            {
                sb.Add("<link href=\"{0}\" rel=\"stylesheet\" />", helper.ResolveUrl(p));
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString IconLink(this HtmlHelper helper, string path)
        {
            return new MvcHtmlString(String.Format
            (
                "<link href=\"{0}\" rel=\"shortcut icon\" type=\"image/x-icon\"/>\n" + 
                "<link href=\"{0}\" rel=\"icon\" type=\"image/x-icon\"/>",
                helper.ResolveUrl(path)
            ));
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string id, string path, string alt)
        {
            return new MvcHtmlString(String.Format("<img id=\"{0}\" src=\"{1}\" alt=\"{2}\" />", id, helper.ResolveUrl(path), alt));
        }

        public static MvcHtmlString GetApplicationUrl(this HtmlHelper helper)
        {
            return new MvcHtmlString(helper.ViewContext.HttpContext.Request.GetApplicationUrl());
        }

        public static MvcHtmlString HiggsInit(this HtmlHelper helper)
        {
            return new MvcHtmlString
            (
                "<script type=\"text/javascript\">" +
                "$.baseUrl('" + helper.ViewContext.HttpContext.Request.GetApplicationUrl() + "');" +
                "</script>"
            );
        }

        public static bool IsReadOnly(this HtmlHelper helper)
        {
            var readOnly = helper.ViewContext.RequestContext.HttpContext.Request["_readonly"];

            return readOnly != null && readOnly.Equals("true", StringComparison.CurrentCultureIgnoreCase);
        }

        public static HtmlHelper AddRequiredScript(this HtmlHelper helper, params string[] scriptPath)
        {
            HiggsScriptManager.AddRequiredScript(scriptPath);

            return helper;
        }

        public static HtmlHelper AddRequiredStyleSheet(this HtmlHelper helper, params string[] styleSheetPath)
        {
            HiggsScriptManager.AddRequiredStyleSheet(styleSheetPath);

            return helper;
        }

        public static HtmlHelper AddScript(this HtmlHelper helper, string script)
        {
            HiggsScriptManager.AddScript(script);

            return helper;
        }

        public static HtmlHelper AddScript(this HtmlHelper helper, string groupName, string script)
        {
            HiggsScriptManager.AddScript(groupName, script);

            return helper;
        }

// ReSharper disable MethodOverloadWithOptionalParameter
        public static HtmlHelper AddScript(this HtmlHelper helper, string groupName, string script, params object[] args)
// ReSharper restore MethodOverloadWithOptionalParameter
        {
            HiggsScriptManager.AddScript(groupName, script, args);

            return helper;
        }

        public static HtmlHelper InsertScript(this HtmlHelper helper, string insertBeforeGroupName, string groupName, string script)
        {
            HiggsScriptManager.InsertScript(insertBeforeGroupName, groupName, script);

            return helper;
        }

// ReSharper disable MethodOverloadWithOptionalParameter
        public static HtmlHelper InsertScript(this HtmlHelper helper, string insertBeforeGroupName, string groupName, string script, params object[] args)
// ReSharper restore MethodOverloadWithOptionalParameter
        {
            HiggsScriptManager.InsertScript(insertBeforeGroupName, groupName, script, args);

            return helper;
        }

        public static HtmlHelper MoveGroupingScript(this HtmlHelper helper, string movedGroupName, string insertBeforeGroupName)
        {
            HiggsScriptManager.MoveGroupingScript(movedGroupName, insertBeforeGroupName);

            return helper;
        }

        public static HtmlHelper HasScriptGroup(this HtmlHelper helper, string groupName)
        {
            HiggsScriptManager.HasScriptGroup(groupName);

            return helper;
        }

        public static HtmlHelper AddjQueryObjectScript(this HtmlHelper helper, string id, string scriptControlId, string script, params object[] functionPars)
        {
            HiggsScriptManager.AddjQueryObjectScript(id, scriptControlId, script, functionPars);

            return helper;
        }

        public static string ResolveUrl(this HtmlHelper helper, string path) 
        {   
            if (String.IsNullOrEmpty(path))
            {
                path = "~/";
            }   
            
            return path[0] == '~' ? VirtualPathUtility.ToAbsolute(path, helper.ViewContext.RequestContext.HttpContext.Request.ApplicationPath) : path;
        }

        public static MvcHtmlString Minify(this HtmlHelper helper, string combinedPath, params string[] filePaths)
        {
            if(filePaths.Length == 0) filePaths = new[] {combinedPath};

            var fileType = combinedPath.EndsWith("js", StringComparison.CurrentCultureIgnoreCase) ? CombinedFileType.JavaScript : CombinedFileType.Css;

            if (HttpContext.Current.IsDebuggingEnabled)
            {
                return fileType == CombinedFileType.JavaScript ? helper.JavaScriptFiles(filePaths) : helper.StyleSheetFiles(filePaths);
            }

            var currentExt = Path.GetExtension(combinedPath);
            var file = CommonHelper.CombinedFile( new CombinedFileInfo
            {
                CombinedPath = currentExt != null && !currentExt.StartsWith(".min") ? Path.ChangeExtension(combinedPath, ".min" + currentExt) : combinedPath,
                FilePaths = filePaths,
                FileType = fileType
            });

            return file.FileType == CombinedFileType.JavaScript ? helper.JavaScriptFiles(file.CombinedPath + "?" + file.CheckSum) : helper.StyleSheetFiles(file.CombinedPath + "?" + file.CheckSum);
        }

        public static MvcHtmlString Minify<TController>(this HtmlHelper helper, string combinedPath, params ResourceInfo[] pages)
            where TController : IController
        {
            return helper.Minify
            (
                combinedPath,
                pages.Select
                (
                    x => typeof(TController).Assembly.GetJavaScriptResource(x.ControllerName, x.ViewName).FileName
                ).ToArray()
            );
        }

        public static MvcHtmlString JavaScriptFiles(this HtmlHelper helper, params string[] path)
        {
            var sb = new StringBuilder();

            foreach (var p in path)
            {
                sb.Add(helper.JavaScript(helper.ResolveUrl(p), String.Empty).ToHtmlString());
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString JavaScript(this HtmlHelper helper, string javaScript)
        {
            return helper.JavaScript(null, javaScript);
        }

        public static MvcHtmlString StaticHtml(this HtmlHelper helper, string path)
        {
            path = helper.ViewContext.HttpContext.Server.MapPath(path);

            return new MvcHtmlString(File.ReadAllText(path));
        }
    }
}
