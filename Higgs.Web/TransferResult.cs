using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Higgs.Web
{
    public class TransferResult : ActionResult
    {
        public TransferResult(RouteValueDictionary routeValues) : this(null, null, routeValues)
        {
        }

        public TransferResult(string controllerName, string actionName, RouteValueDictionary routeValues = null)
        {
            RouteValues = routeValues ?? new RouteValueDictionary();
            if (!string.IsNullOrEmpty(controllerName))
            {
                RouteValues["controller"] = controllerName;
            }
            if (!string.IsNullOrEmpty(actionName))
            {
                RouteValues["action"] = actionName;
            }
            if (RouteValues["controller"] == null)
            {
                throw new ArgumentException(Resources.ControllerNameIsNotFoundInRouteValueDictionary, "controllerName");
            }
            if (RouteValues["action"] == null)
            {
                throw new ArgumentException(Resources.ActionNameIsNotFoundInRouteValueDictionary, "actionName");
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var routeData = new RouteData();
            foreach (var pair in RouteValues)
            {
                routeData.Values.Add(pair.Key, pair.Value);
            }
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var requestContext = new RequestContext(httpContext, routeData);
            ControllerBuilder.Current.GetControllerFactory().CreateController(context.RequestContext, RouteValues["controller"].ToString()).Execute(requestContext);
        }

        // Properties
        public RouteValueDictionary RouteValues { get; set; }
    }
}