using System.Configuration;
using Higgs.Core;

namespace Higgs.Web.Configurations
{
    public class RoutingSection : ConfigurationElement
    {
        #region Fields

        private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
        private static readonly ConfigurationProperty _propStaticView = Config.Property<StaticViewSection>();
        private static readonly ConfigurationProperty _propIgnorePaths = Config.Collection<IgnorePathElementsCollection>();

        #endregion

        #region Methods

        static RoutingSection()
        {
            _properties.Add(_propStaticView);
        }

        #endregion

        #region Properties

        [ConfigurationProperty("defaultRoute", DefaultValue="{controller}/{action}/{id}")]
        public string DefaultRoute
        {
            get { return (string) base["defaultRoute"]; }
        }

        [ConfigurationProperty("staticView")]
        public StaticViewSection StaticView
        {
            get { return (StaticViewSection) base[_propStaticView]; }
        }

        [ConfigurationProperty("ignorePaths")]
        public IgnorePathElementsCollection IgnorePaths
        {
            get
            {
                return (IgnorePathElementsCollection) base[_propIgnorePaths];
            }
        }
        
        #endregion
    }
}
