using System.Configuration;

namespace Higgs.Web.Configurations
{
    [ConfigurationCollection(typeof (ExcludeFolderElement), AddItemName = "exclude", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ExcludeFolderCollection : ConfigurationElementCollection
    {
        private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        static ExcludeFolderCollection() {}

        public ExcludeFolderElement this[int index]
        {
            get { return (ExcludeFolderElement) BaseGet(index); }
            set { BaseAdd(index, value); }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }

        protected override string ElementName
        {
            get { return "exclude"; }
        }

        protected override bool ThrowOnDuplicate
        {
            get { return false; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ExcludeFolderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExcludeFolderElement) element).Path;
        }
    }
}