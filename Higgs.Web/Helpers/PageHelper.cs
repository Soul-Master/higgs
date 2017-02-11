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

        public static bool IsReadOnly(this HtmlHelper helper)
        {
            var readOnly = helper.ViewContext.RequestContext.HttpContext.Request["_readonly"];

            return readOnly != null && readOnly.Equals("true", StringComparison.CurrentCultureIgnoreCase);
        }

        public static string ResolveUrl(this HttpRequestBase request, string path)
        {
            return path[0] == '~' ? VirtualPathUtility.ToAbsolute(path, request.ApplicationPath) : path;
        }

        public static string ResolveAbsoluteUrl(this HttpRequestBase request, string path)
        {
            return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, request.ResolveUrl(path));
        }

        public static string ResolveUrl(this HtmlHelper helper, string path) 
        {
            return path[0] == '~' ? VirtualPathUtility.ToAbsolute(path, helper.ViewContext.RequestContext.HttpContext.Request.ApplicationPath) : path;
        }

        public static MvcHtmlString StaticHtml(this HtmlHelper helper, string path)
        {
            path = helper.ViewContext.HttpContext.Server.MapPath(path);

            return new MvcHtmlString(File.ReadAllText(path));
        }

        public static MvcHtmlString HiggsInit(this HtmlHelper helper)
        {
            var statement = "$.baseUrl(\"" + helper.ResolveUrl("~") + "\");";

            return new MvcHtmlString("<script type=\"text/javascript\">" + statement + "</script>");
        }
    }
}
