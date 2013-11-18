using System.Configuration;

namespace Higgs.Web.Configurations
{
    public class ExcludeFolderElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired=true)]
        public string Path
        {
            get { return (string) this["path"]; }
            set { this["path"] = value; }
        }
    }
}
