using System.Configuration;

namespace Higgs.Web.Configurations
{
    public class StaticViewSection : ConfigurationElement
    {
        private static readonly ConfigurationProperty _propMenus = new ConfigurationProperty(null, typeof (ExcludeFolderCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
        private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
        
        static StaticViewSection()
        {
          _properties.Add(_propMenus);
        }

        [ConfigurationProperty("enable", DefaultValue=false)]
        public bool Enable
        {
            get
            {
                return (bool) base["enable"];
            }
        }

        [ConfigurationProperty("viewsFolder", DefaultValue="~/Views")]
        public string ViewsFolder
        {
            get
            {
                return (string) base["viewsFolder"];
            }
        }

        [ConfigurationProperty("staticFilePattern", DefaultValue="*.static.*")]
        public string StaticFilePattern
        {
            get
            {
                return (string) base["staticFilePattern"];
            }
        }

        [ConfigurationProperty("defaultFileName", DefaultValue="index")]
        public string DefaultFileName
        {
            get
            {
                return (string) base["defaultFileName"];
            }
        }
        
        [ConfigurationProperty("", IsDefaultCollection = true, Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ExcludeFolderCollection ExcludeFolders
        {
          get { return (ExcludeFolderCollection) this[_propMenus]; }
        }
    }
}
