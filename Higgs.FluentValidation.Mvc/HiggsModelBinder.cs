using System.Linq;
using System.Web.Mvc;
using FluentValidation;
using Higgs.Core;

namespace Higgs.FluentValidation.Mvc
{
    public class HiggsModelBinder : DefaultModelBinder
    {
        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = bindingContext.Model as IPropertySelectable;
            base.OnModelUpdated(controllerContext, bindingContext);
            Enumerable.ToList<string>((
                                 from x in bindingContext.ModelState.Keys
                                 where bindingContext.ModelState[x].Errors.Count == 0
                                 select x
                             ))
            .ForEach(x => bindingContext.ModelState.Remove(x));

            if (model != null)
            {
                var modelType = bindingContext.ModelType;
                var currentProperties = from x in bindingContext.ModelType.GetProperties()
                                                   where x.DeclaringType == modelType
                                                   select x.Name;
                
                Enumerable.ToList<string>((
                                     from bindedPropertyName in bindingContext.ModelState.Keys
                                     where !model.IsSelectedProperty(bindedPropertyName) && !Enumerable.Contains<string>(currentProperties, bindedPropertyName)
                                     select bindedPropertyName
                                 ))
                .ForEach(x => bindingContext.ModelState.Remove(x));
            }

            if (ValidatorOptions.CascadeMode != CascadeMode.StopOnFirstFailure) return;

            if (bindingContext.ModelState.Keys.Count > 1)
            {
                Enumerable.Skip<string>(bindingContext.ModelState.Keys, 1).ToList().ForEach(x => bindingContext.ModelState.Remove(x));
            }

            if ((bindingContext.ModelState.Keys.Count == 1) && (bindingContext.ModelState.Values.Count > 1))
            {
                bindingContext.ModelState.Values.Skip(1).ToList().ForEach(x => bindingContext.ModelState.Values.Remove(x));
            }

            if (bindingContext.ModelState.Values.Count != 1) return;

            var current = bindingContext.ModelState.Values.Single();

            if (current.Errors.Count > 1)
            {
                current.Errors.Skip(1).ToList().ForEach(x => current.Errors.Remove(x));
            }
        }
    }
}
