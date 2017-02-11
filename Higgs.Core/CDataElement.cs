using System.Configuration;
using System.Xml;

namespace Higgs.Core
{
    public sealed class CDataElement : ConfigurationElement
    {
        public string Value { get; private set; }

        protected override void DeserializeElement(XmlReader reader, bool s)
        {
            this.Value = reader.ReadElementContentAs(typeof(string), (IXmlNamespaceResolver)null) as string;
            if (this.Value == null)
                return;
            this.Value = this.Value.Trim();
        }
    }
}
