using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentValidation;

namespace Higgs.FluentValidation.Mvc
{
    public class HiggsModelValidator : ModelValidator
    {
        public HiggsModelValidator(ModelMetadata metadata, ControllerContext controllerContext, Type validatorType) : base(metadata, controllerContext)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new Exception(string.Format(Resources.HiggsModelValidator_InvalidValidatorType, validatorType.Name));
            }

            CurrentValidator = Activator.CreateInstance(validatorType) as IValidator;
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var result = CurrentValidator.Validate(Metadata.Model);
            if (result.IsValid) return Enumerable.Empty<ModelValidationResult>();

            return 
            (
                from error in result.Errors 
                select new ModelValidationResult
                {
                    MemberName = error.PropertyName, 
                    Message = error.ErrorMessage
                }
            ).ToList<ModelValidationResult>();
        }

        protected IValidator CurrentValidator { get; set; }
    }
}
