using System.Configuration;

namespace Higgs.Web.Configurations
{
    public class IgnorePathElementsCollection : ConfigurationElementCollection
    {
        #region Methods

        public IgnorePathElementsCollection()
        {
            base.BaseAdd(new IgnorePathElement{ Path = "Scripts" });
            base.BaseAdd(new IgnorePathElement{ Path = "Content" });
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new IgnorePathElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IgnorePathElement) element).Path;
        }

        #endregion

        #region Properties

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public new IgnorePathElement this[string name]
        {
            get
            {
                return (IgnorePathElement) BaseGet(name);
            }
        }

        #endregion
    }
}
