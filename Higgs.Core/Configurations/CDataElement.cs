using System.Configuration;
using System.Xml;

namespace Higgs.Core.Configurations
{
    public sealed class CDataElement : ConfigurationElement
    {
        protected override void DeserializeElement(XmlReader reader, bool s)
        {
// ReSharper disable AssignNullToNotNullAttribute
            Value = reader.ReadElementContentAs(typeof(string), null) as string;
// ReSharper restore AssignNullToNotNullAttribute

            if (Value != null) Value = Value.Trim();
        }

        public string Value { get; private set; }
    }
}
