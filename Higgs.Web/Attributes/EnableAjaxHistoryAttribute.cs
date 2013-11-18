using System;
using System.Web.Mvc;

namespace Higgs.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    public class EnableAjaxHistoryAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.Url == null) return;

            if (!filterContext.RequestContext.HttpContext.Request.Url.AbsolutePath.EndsWith("/"))
                filterContext.RequestContext.HttpContext.Response.Redirect(filterContext.RequestContext.HttpContext.Request.Url.AbsolutePath + "/");

            base.OnActionExecuting(filterContext);
        }
    }
}
