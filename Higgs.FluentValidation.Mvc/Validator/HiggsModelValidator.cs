using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Results;

namespace Higgs.FluentValidation.Mvc.Validator
{
    /// <summary>
    /// Original code from FluentValidation.Mvc.FluentValidationModelValidator.cs
    /// It is impossible to create FluentValidationModelValidator class outside FluentValidation.Mvc project because it is internal class.
    /// So, I need to copy & paste this class to this project until Fluent Validator will change accessor of FluentValidationModelValidator class.
    /// </summary>
    public class HiggsModelValidator : ModelValidator
    {
        private readonly IValidator _validator;

        public HiggsModelValidator(ModelMetadata metadata, ControllerContext controllerContext, IValidator validator)
            : base(metadata, controllerContext)
        {
            _validator = validator;
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            if (Metadata.Model != null)
            {
                var result = _validator.Validate(Metadata.Model);

                if (!result.IsValid)    return ConvertValidationResultToModelValidationResults(result);
            }
            return Enumerable.Empty<ModelValidationResult>();
        }

        protected virtual IEnumerable<ModelValidationResult> ConvertValidationResultToModelValidationResults(
            ValidationResult result)
        {
            return result.Errors.Select(x => new ModelValidationResult
            {
                MemberName = x.PropertyName,
                Message = x.ErrorMessage
            });
        }
    }
}