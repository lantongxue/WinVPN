using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomlyn.Model;
using Tomlyn;
using System.IO;
using WinVPN.Model;
using System.Xml.Linq;

namespace WinVPN.Service
{
    internal class ConfigService
    {
        WinVPNConfig model = null;

        string config = "WinVPN.toml";

        public ConfigService()
        {
            string toml = File.ReadAllText(config);
            model = Toml.ToModel<WinVPNConfig>(toml);
        }

        public void AddPlugin(Model.Plugin plugin)
        {
            if(this.GetPlugin(plugin.Name) != null)
            {
                return;
            }
            model.Plugins.Add(plugin);
        }

        public Model.Plugin GetPlugin(string name)
        {
            return model.Plugins.Where(x => x.Name == name).FirstOrDefault();
        }

        public void Save()
        {
            string toml = Toml.FromModel(model);
            File.WriteAllText(config, toml);
        }
    }
}
