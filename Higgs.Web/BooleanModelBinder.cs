using System.Web.Mvc;

namespace Higgs.Web
{
    public class BooleanModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = controllerContext.RequestContext.HttpContext.Request.Form[bindingContext.ModelName];

            return value != null && (value.ToUpper() == "ON" || value.ToUpper() == "TRUE");
        }
    }
}
