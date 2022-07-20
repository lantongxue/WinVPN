using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WinVPN.Plugin;

namespace VpnGatePlugin
{
    public class VpnGatePlugin : Plugin
    {
        public override string PluginName { get => "VpnGate"; }
        public override string PluginVersion { get => "1.0.0"; }
        public override string PluginAuthor { get => "kali"; }
        public override string PluginWebsite { get => "https://github.com/langtongxue/WinVPN.VpnGate"; }
        public override bool IsSupportSettings { get => true; }

        public override void Settings()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TabItem> GetTabItems()
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = "VpnGate";
            tabItem.Content = new UserControl1();

            return new TabItem[] { tabItem };
        }
    }
}
