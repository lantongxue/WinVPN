using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WinVPN.Model;
using System.Xml;
using Newtonsoft.Json;

namespace WinVPN.Service
{
    internal class ConfigService
    {
        string config = "WinVPN.config";
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement plugins = null;
        XmlElement servers = null;

        public ConfigService()
        {
            xmlDoc.Load(config);
            plugins = xmlDoc.DocumentElement["plugins"];
            if(plugins == null)
            {
                plugins = xmlDoc.CreateElement("plugins");
                xmlDoc.DocumentElement.AppendChild(plugins);
            }
            servers = xmlDoc.DocumentElement["servers"];
            if(servers == null)
            {
                servers = xmlDoc.CreateElement("servers");
                xmlDoc.DocumentElement.AppendChild(servers);
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

        public void AddServer(VpnServer server)
        {
            XmlElement xmlNode = xmlDoc.CreateElement(server.Id);
            string json = JsonConvert.SerializeObject(server);
            xmlNode.InnerText = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            servers.AppendChild(xmlNode);
        }

        public void UpdateServer(string id, VpnServer server)
        {
            XmlNode xmlNode = servers.SelectSingleNode(id);
            if (xmlNode == null)
            {
                return;
            }
            string json = JsonConvert.SerializeObject(server);
            xmlNode.InnerText = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }

        public IEnumerable<VpnServer> GetServers()
        {
            List<VpnServer> list = new List<VpnServer>();
            foreach(XmlNode node in servers.ChildNodes)
            {
                VpnServer server = JsonConvert.DeserializeObject<VpnServer>(Encoding.UTF8.GetString(Convert.FromBase64String(node.InnerText)));
                list.Add(server);

            }
            return list;
        }

        public void Save()
        {
            xmlDoc.Save(config);
        }
    }
}
