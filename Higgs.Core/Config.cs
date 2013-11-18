using System;
using System.Configuration;

namespace Higgs.Core
{
    public static class Config
    {
        public static ConfigurationProperty Property<TElement>(string name = "", ConfigurationPropertyOptions options = ConfigurationPropertyOptions.None)
            where TElement: ConfigurationElement
        {
            var elementType = typeof (TElement);
            if(string.IsNullOrEmpty(name))
            {
                // Convert to camel case
                name = elementType.Name.Substring(0, 1).ToLower() + elementType.Name.Substring(1);
                
                // Auto Remove Suffix name;
                if(name.EndsWith("section", StringComparison.CurrentCultureIgnoreCase) || name.EndsWith("element", StringComparison.CurrentCultureIgnoreCase))
                {
                    name = name.Substring(0, name.Length - 7);
                }
            }

            return new ConfigurationProperty(name, elementType, Activator.CreateInstance<TElement>(), options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="name" optional="true">
        ///  If optional parameter does not support complex Plural Noun, it will automatically append s after normal processed name.
        /// </param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ConfigurationProperty Collection<TElement>(string name = "", ConfigurationPropertyOptions options = ConfigurationPropertyOptions.None)
            where TElement: ConfigurationElement
        {
            var collectionType = typeof (TElement);
            if(string.IsNullOrEmpty(name))
            {
                // Convert to camel case
                name = collectionType.Name.Substring(0, 1).ToLower() + collectionType.Name.Substring(1);
                
                // Auto Remove Suffix name;
                if(name.EndsWith("elementscollection", StringComparison.CurrentCultureIgnoreCase))
                {
                    name = name.Substring(0, name.Length - 18) + "s";
                }
            }

            return new ConfigurationProperty(name, collectionType, Activator.CreateInstance<TElement>(), options);
        }
    }
}
