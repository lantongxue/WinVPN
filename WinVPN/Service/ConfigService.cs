using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WinVPN.Model;
using System.Xml;

namespace WinVPN.Service
{
    internal class ConfigService
    {
        string config = "WinVPN.config";
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement plugins = null;
        public ConfigService()
        {
            xmlDoc.Load(config);
            plugins = xmlDoc.DocumentElement["plugins"];
            if(plugins == null)
            {
                plugins = xmlDoc.CreateElement("plugins");
                xmlDoc.DocumentElement.AppendChild(plugins);
            }
        }

        public bool? GetPlugin(string pluginName)
        {
            XmlElement p = plugins[pluginName];
            if(p == null)
            {
                return null;
            }
            return bool.Parse(p.Attributes["IsEnable"].Value);
        }

        public void SetPlugin(string pluginName, bool isEnabled)
        {
            XmlElement xmlNode = plugins[pluginName];
            if(xmlNode == null)
            {
                this.AddPlugin(pluginName, isEnabled);
                return;
            }
            xmlNode.SetAttribute("IsEnable", isEnabled.ToString());
        }

        public void AddPlugin(string pluginName, bool isEnabled)
        {
            XmlElement xmlNode = xmlDoc.CreateElement(pluginName);
            xmlNode.SetAttribute("IsEnable", isEnabled.ToString());
            plugins.AppendChild(xmlNode);
        }

        public void Save()
        {
            xmlDoc.Save(config);
        }
    }
}
