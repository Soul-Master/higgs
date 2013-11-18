using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Higgs.Web.Attributes
{
    /// <summary>
    /// For using this attribute, web.config file must set CustomError to true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomErrorAttribute : FilterAttribute, IExceptionFilter
    {
        private Type ExceptionType { get; set; }

        public CustomErrorAttribute(Type exceptionType, string controllerName, string actionName)
        {
            ExceptionType = exceptionType;
            ControllerName = controllerName;
            ActionName = actionName;
        }

        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public virtual void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            
            if (filterContext == null) throw new ArgumentNullException("filterContext");
            if (filterContext.IsChildAction || !filterContext.HttpContext.IsCustomErrorEnabled) return;

            // If custom errors are disabled, we need to let the normal ASP.NET exception handler
            // execute so that the user can see useful debugging information.
            if (filterContext.ExceptionHandled) return;
            if (!ExceptionType.IsInstanceOfType(exception)) return;

            HandlerException(filterContext);
        }

        public virtual void HandlerException(ExceptionContext filterContext)
        {
            // Certain versions of IIS will sometimes use their own error page when
            // they detect a server error. Setting this property indicates that we
            // want it to try to render ASP.NET MVC's error page instead.
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            var routeValue = new RouteValueDictionary
            {
                {"context", filterContext}
            };

            filterContext.ExceptionHandled = true;
            filterContext.Result = new TransferResult(ControllerName, ActionName, routeValue);
        }
    }
}


