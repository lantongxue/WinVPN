using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WinVPN.Plugin.SDK;

namespace VpnGatePlugin
{
    public class VpnGatePlugin : WinVPNPlugin, IPlugin
    {
        public string PluginName { get => "VpnGate"; }
        public string PluginVersion { get => "1.0.0"; }
        public string PluginAuthor { get => "kali"; }
        public string PluginWebsite { get => "https://github.com/langtongxue/WinVPN.VpnGate"; }
        public bool IsSupportSettings { get => true; }

        public void Settings()
        {
            UserControl2 u2 = new UserControl2();
            u2.Owner = Application.Current.MainWindow;
            u2.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            u2.ShowDialog();
        }

        public override IEnumerable<TabItem> GetMainWindowTabItems()
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = "VpnGate";
            tabItem.Content = new UserControl1();

            return new TabItem[] { tabItem };
        }
    }
}
