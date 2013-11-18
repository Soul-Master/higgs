using System.Configuration;

namespace Higgs.Web.Configurations
{
    public class IgnorePathElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired=true)]
        public string Path
        {
            get { return (string) base["path"]; }
            set 
            {
                if (value.EndsWith("/")) value = value.Substring(0, value.Length - 1);

                base["path"] = value;
            }
        }
    }
}
