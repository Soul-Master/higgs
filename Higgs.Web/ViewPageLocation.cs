using System;
using System.Reflection;
using System.Web;
using System.Web.Routing;

namespace Higgs.Web
{
    public class ViewPageLocation
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string RouteUrl { get; set; }
        public MethodInfo ActionMethod { get; set; }

        public string GetVirtualUrl(HttpContextBase context)
        {
            var routeDic = new RouteValueDictionary();
            routeDic["controller"] = ControllerName;
            routeDic["action"] = ActionName;

            var virtualPath = RouteTable.Routes.GetVirtualPath(new RequestContext(context, new RouteData()), routeDic);
            if (virtualPath != null) return virtualPath.VirtualPath;

            throw new Exception();
        }
    }
}
