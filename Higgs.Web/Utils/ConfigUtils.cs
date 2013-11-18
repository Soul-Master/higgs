using System.Configuration;
using Higgs.Core.Helpers;
using System.Xml.Linq;

namespace Higgs.Web.Utils
{
    public class ConfigUtils
    {
        public static void LoadAppSettings(string configFilePath)
        {
            const string sectionName = "appSettings";
            var path = IOHelpers.GetFullPath(configFilePath);
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            var configElement = XElement.Load(path);
            var key = XName.Get("key");
            var value = XName.Get("value");

            foreach(var node in configElement.Elements())
            {
                XAttribute keyAttribute;
                switch(node.Name.LocalName)
                {
                    case "add":
                        keyAttribute = node.Attribute(key);
                        XAttribute valueAttribute = node.Attribute(value);

                        if(keyAttribute != null && valueAttribute != null)
                        {
                            config.AppSettings.Settings.Add(keyAttribute.Value, valueAttribute.Value);
                        }
                        break;
                    case "remove":
                        keyAttribute = node.Attribute(key);

                        if (keyAttribute != null)
                        {
                            config.AppSettings.Settings.Remove(keyAttribute.Value);
                        }
                        break;
                    case "clear":
                        config.AppSettings.Settings.Clear();
                        break;
                }
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(sectionName);
        }
    }
}
