using System.Globalization;
using System.Web.Mvc;

namespace Higgs.Web.Binders
{
    public class DecimalBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueResult == null || string.IsNullOrEmpty(valueResult.AttemptedValue))
            {
                if (bindingContext.ModelType.IsValueType) return 0;

                return null;
            }

            return decimal.Parse(valueResult.AttemptedValue.Trim(), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands);
        }
    }
}