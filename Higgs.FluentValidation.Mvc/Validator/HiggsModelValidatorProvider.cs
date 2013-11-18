using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Mvc;

namespace Higgs.FluentValidation.Mvc.Validator
{
    public class HiggsModelValidatorProvider : FluentValidationModelValidatorProvider
    {
        public HiggsModelValidatorProvider(IValidatorFactory validatorFactory)
            : base(validatorFactory)
        {
            _validatorFactory = validatorFactory as MvcValidatorFactory;
        }

        private readonly MvcValidatorFactory _validatorFactory;
        
        private static readonly object LockCache = new object();
        private static readonly Dictionary<Type, IEnumerable<IValidator>> ValidtorsCache = new Dictionary<Type, IEnumerable<IValidator>>();
        public IEnumerable<IValidator> GetValidators(Type modelType)
        {
            if (!ValidtorsCache.ContainsKey(modelType))
            {
                lock (LockCache)
                {
                    var allValiators = new List<IValidator>();
                    var currentType = modelType;

                    while (currentType != null)
                    {
                        var result = _validatorFactory.CreateAllInstances(currentType);
                        allValiators.AddRange(result);

                        currentType = currentType.BaseType;
                    }

                    ValidtorsCache[modelType] = from v in allValiators
                                                let validatorPriorityAttribute = v.GetType().GetCustomAttributes(typeof(ValidatePriorityAttribute), true)
                                                let validatePriority = validatorPriorityAttribute != null && validatorPriorityAttribute.Length > 0 ? ((ValidatePriorityAttribute)validatorPriorityAttribute[0]).Priority : 0
                                                orderby validatePriority descending
                                                select v;
                }
            }

            return ValidtorsCache[modelType];
        }

        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            if (metadata.ContainerType != null && !string.IsNullOrEmpty(metadata.PropertyName))
            {
                return base.GetValidators(metadata, context);
            }

            var modelValidators = new List<ModelValidator>();
            foreach (var v in GetValidators(metadata.ModelType))
            {
                modelValidators.AddRange(GetValidatorsForModel(metadata, context, v));
            }

            return modelValidators;
        }

        static IEnumerable<ModelValidator> GetValidatorsForModel(ModelMetadata metadata, ControllerContext context, IValidator validator) {
			if (validator != null) {
				yield return new HiggsModelValidator(metadata, context, validator);
			}
		}
    }
}
