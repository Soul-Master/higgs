using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Higgs.Web.Attributes
{
    public class RequireAllParameterAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return methodInfo.GetParameters()
                .All(par => controllerContext.HttpContext.Request[par.Name] != null || controllerContext.RouteData.Values[par.Name] != null);
        }
    }
}