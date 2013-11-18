using System.Configuration;

namespace Higgs.Web.Configurations
{
    public class ValidationSection : ConfigurationElement
    {
        #region Properties

        [ConfigurationProperty("enable", DefaultValue=true)]
        public bool Enable
        {
            get { return (bool) base["enable"]; }
        }

        #endregion
    }
}
