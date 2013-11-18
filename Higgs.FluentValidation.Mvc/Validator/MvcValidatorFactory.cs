using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentValidation;

namespace Higgs.FluentValidation.Mvc.Validator 
{
    public class MvcValidatorFactory : ValidatorFactoryBase 
    {
    	public override IValidator CreateInstance(Type modelType)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(modelType);

            return DependencyResolver.Current.GetService(validatorType) as IValidator;
    	}

        public IEnumerable<IValidator> CreateAllInstances(Type modelType)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(modelType);
            var instances = DependencyResolver.Current.GetServices(validatorType);

            if (instances == null) return Enumerable.Empty<IValidator>();

            return instances.OfType<IValidator>();
        }
    }
}