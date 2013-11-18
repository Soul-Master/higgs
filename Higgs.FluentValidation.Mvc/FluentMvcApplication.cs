using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Higgs.FluentValidation.Mvc.Validator;
using Higgs.Web;
using Higgs.Web.Configurations;

namespace Higgs.FluentValidation.Mvc
{
    public class FluentMvcApplication : MvcApplication
    {
        public static HiggsModelValidatorProvider HiggsValidatorProvider { get; private set; }

        public void EnableFluentValidation()
        {
            // TODO: Move configuration about validation to this project.
            var config = HiggsWebConfigSection.Current;
            if (!config.Validation.Enable) return;

            ModelBinders.Binders.DefaultBinder = new HiggsModelBinder();

            //Configure FV to use StructureMap
            var factory = new MvcValidatorFactory();

            //Tell MVC to use FV for validation
            HiggsValidatorProvider = new HiggsModelValidatorProvider(factory);
            ModelValidatorProviders.Providers.Add(HiggsValidatorProvider);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
        }

        public override void Start()
        {
            base.Start();

            EnableFluentValidation();
        }
    }
}
