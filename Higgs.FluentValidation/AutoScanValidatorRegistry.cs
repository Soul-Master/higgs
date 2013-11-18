using System;
using FluentValidation;
using StructureMap.Configuration.DSL;

namespace Higgs.FluentValidation
{
    public class AutoScanValidatorRegistry : Registry
    {
        public AutoScanValidatorRegistry(Type t)
        {
            var validators = AssemblyScanner.FindValidatorsInAssembly(t.Assembly);

            validators.ForEach
            (
                x => For(x.InterfaceType)
                            .Singleton()
                            .Use(x.ValidatorType)
            );
        }
    }

    /// <summary>
    /// Example 
    ///    ObjectFactory.Configure(cfg => cfg.AddRegistry(new AutoScanValidatorRegistry<SomeClassinAssembly>()));
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AutoScanValidatorRegistry<T> : Registry
    {
        public AutoScanValidatorRegistry()
        {
            var validators = AssemblyScanner.FindValidatorsInAssemblyContaining<T>();

            validators.ForEach
            (
                x => For(x.InterfaceType)
                            .Singleton()
                            .Use(x.ValidatorType)
            );
        }
    }
}
