using System.Web.ModelBinding;
using System.Web.Mvc;

namespace Higgs.Web
{
    public class TrimStringBinder : System.Web.ModelBinding.IModelBinder
    {
        public bool BindModel(ModelBindingExecutionContext modelBindingExecutionContext, System.Web.ModelBinding.ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueResult == null || string.IsNullOrEmpty(valueResult.AttemptedValue))
            {
                bindingContext.Model = null;
            }
            else
            {
                bindingContext.Model = valueResult.AttemptedValue.Trim();
            }

            return true;
        }
    }
}
