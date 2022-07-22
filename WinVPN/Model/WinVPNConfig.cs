using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinVPN.Model
{
    internal class WinVPNConfig
    {
        public IList<Plugin> Plugins { get; set; } = new List<Plugin>();
    }

    internal class Plugin
    {
        public Plugin()
        {
        }

        public Plugin(string name, bool isEnabled)
        {
            Name = name;
            IsEnabled = isEnabled;
        }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }
    }
}
