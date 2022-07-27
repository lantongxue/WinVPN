using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WinVPN.Model;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using WinVPN.Model.VPN;
using Newtonsoft.Json.Linq;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

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

        public VpnServer GetServer(string id)
        {
            id = "WV" + id;
            XmlElement p = servers[id];
            if (p == null)
            {
                return null;
            }

            byte[] buff = Convert.FromBase64String(p.InnerText);
            using (MemoryStream ms = new MemoryStream(buff))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                VpnServer server = (VpnServer)formatter.Deserialize(ms);
                return server;
            }
        }

        public void AddServer(VpnServer server)
        {
            XmlElement xmlNode = xmlDoc.CreateElement(server.Id);
            string json = JsonConvert.SerializeObject(server);
            xmlNode.InnerText = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            servers.AppendChild(xmlNode);
        }

        public IEnumerable<VpnServer> GetServers()
        {
            List<VpnServer> list = new List<VpnServer>();
            foreach(XmlNode node in servers.ChildNodes)
            {
                JObject jb = JsonConvert.DeserializeObject<JObject>(Encoding.UTF8.GetString(Convert.FromBase64String(node.InnerText)));
                VpnProtocol protocol = (VpnProtocol)Enum.Parse(typeof(VpnProtocol), jb.GetValue("Protocol").Value<string>());
                dynamic server = null;
                switch (protocol)
                {
                    case VpnProtocol.PPTP:
                        server = new PPTP();
                        break;
                    case VpnProtocol.L2TP:
                        server = new L2TP();
                        server.PreSharedKey = jb["PreSharedKey"].Value<string>();
                        break;
                    case VpnProtocol.SSTP:
                        server = new SSTP();
                        break;
                    case VpnProtocol.IKEv2:
                        server = new IKEv2();
                        break;
                    case VpnProtocol.WireGuard:
                        server = new WireGuard();
                        break;
                }
                if(server == null)
                {
                    server = new VpnServer();
                    server.Protocol = protocol;
                }
                server.Id = jb["Id"].Value<string>();
                server.Name = jb["Name"].Value<string>();
                server.Address = jb["Address"].Value<string>();
                server.Username = jb["Username"].Value<string>();
                server.Password = jb["Password"].Value<string>();
                server.Info = jb["Info"].Value<string>();
                server.Delay = jb["Delay"].Value<long>();
                server.Source = jb["Source"].Value<string>();
                server.Traffic = jb["Traffic"].Value<long>();

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
