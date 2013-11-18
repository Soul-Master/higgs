using System.Configuration;
using Higgs.Core;

namespace Higgs.Web.Configurations
{
    public class HiggsWebConfigSection : ConfigurationSection
    {
        #region Fields

        private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
        private static readonly ConfigurationProperty _propRouting = Config.Property<RoutingSection>();
        private static readonly ConfigurationProperty _propValidation = Config.Property<ValidationSection>();

        #endregion

        #region Methods

        static HiggsWebConfigSection()
        {
            _properties.Add(_propRouting);
            _properties.Add(_propValidation);
        }

        #endregion

        #region Properties

        [ConfigurationProperty("routing")]
        public RoutingSection Routing
        {
            get { return (RoutingSection) base[_propRouting]; }
        }

        [ConfigurationProperty("validation")]
        public ValidationSection Validation
        {
            get { return (ValidationSection) base[_propValidation]; }
        }

        #endregion

        public static HiggsWebConfigSection Current
        {
            get { return ConfigurationManager.GetSection("higgs.web") as HiggsWebConfigSection; }
        }
    }
}
