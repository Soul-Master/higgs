using System;
using StructureMap;

namespace Higgs.FluentValidation
{
    public static class ValidationHelpers
    {
        public static Type RegistryValidator(this Type someTypeInAssembly)
        {
            ObjectFactory.Configure(x => x.AddRegistry(new AutoScanValidatorRegistry(someTypeInAssembly)));

            return someTypeInAssembly;
        }

        public static void AsHttpScoped(this Type t)
        {
            ObjectFactory.Configure(x => x.For(t).HttpContextScoped());
        }
    }
}
