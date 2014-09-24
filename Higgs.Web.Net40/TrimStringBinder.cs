﻿using System.Web.Mvc;

namespace Higgs.Web
{
    public class TrimStringBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
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
