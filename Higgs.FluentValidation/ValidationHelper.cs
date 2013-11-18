using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using Higgs.Core;

namespace Higgs.FluentValidation
{
    public static class ValidationHelper
    {
        public static Dictionary<string, IEnumerable<IPropertyValidator>> GetPropertyValidator(Type modelType, IEnumerable<IValidator> validatorList)
        {
            var propertySelectable = Activator.CreateInstance(modelType) as IPropertySelectable;
            var properties = modelType.GetProperties().Where(x => x.DeclaringType == modelType).Select(x => x.Name);

            var z = validatorList.Select(x => x.CreateDescriptor().GetMembersWithValidators());

            var temp =  from x in z
                                from y in x
                                where
                                propertySelectable == null ||
                                ((propertySelectable.IsSelectedProperty(y.Key) || properties.Contains(y.Key)))
                                select y;

            var result = new Dictionary<string, IEnumerable<IPropertyValidator>>();
            foreach(var item in temp)
            {
                if(!result.ContainsKey(item.Key))
                {
                    result.Add(item.Key, item);
                }
                else
                {
                    result[item.Key] = result[item.Key].Union(item);
                }
            }

            return result;
        }

        public static Dictionary<string, string> GetRuleMapping<T>(IEnumerable<IValidator> validatorList)
        {
            var mappedProperty = new Dictionary<string, string>();
            var modelType = typeof (T);
            var propertySelectable = Activator.CreateInstance(modelType) as IPropertySelectable;

            foreach(var v in validatorList)
            {
                var validator = v as BaseValidator<T>;
                if (validator == null) continue;
                if (validator.MappedProperty.Count == 0) continue;

                foreach(var key in validator.MappedProperty.Keys)
                {
                    if ((propertySelectable != null && !propertySelectable.IsSelectedProperty(key)) &&
                        modelType.GetProperty(key).DeclaringType != modelType) continue;

                    var key1 = key;

                    foreach(var data in validator.MappedProperty.Where(x => x.Key == key1))
                    {
                        mappedProperty.Add(key, data.Value);
                    }
                }
            }

            return mappedProperty;
        }

        public static IRuleBuilderOptions<T, TProperty> CollectionCount<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, int min, int max = int.MaxValue)
            where TProperty : IEnumerable
        {
            return ruleBuilder.SetValidator(new CollectionCountValidator(min, max));
        }
    }
}
