using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WinVPN.Plugin.SDK;

namespace VpnGatePlugin
{
    public class VpnGatePlugin : IWinVPNPlugin
    {
        public string PluginName => "VpnGate";
        public Version PluginVersion => new Version("1.0.0");
        public string PluginAuthor => "kali";
        public string PluginWebsite => "https://github.com/langtongxue/WinVPN.VpnGate";
        public bool IsSupportSettings => true;

        public Version MiniDependentVersion => new Version("1.0.0");

        public void Settings()
        {
            UserControl2 u2 = new UserControl2();
            u2.Owner = Application.Current.MainWindow;
            u2.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            u2.ShowDialog();
        }

        public IEnumerable<TabItem> GetMainWindowTabItems()
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = "VpnGate";
            tabItem.Content = new UserControl1();

            return new TabItem[] { tabItem };
        }

        public IEnumerable<FrameworkElement> GetMainWindowLeftCommands()
        {
            return new List<FrameworkElement>()
            {
                new Button()
                {
                    Content = "VpnGate"
                }
            };
        }

        public IEnumerable<FrameworkElement> GetMainWindowRightCommands()
        {
            return new List<FrameworkElement>()
            {
                new Button()
                {
                    Content = "VpnGate"
                }
            };
        }

        public IEnumerable<FrameworkElement> GetMainWindowStatusBarItems()
        {
            return new List<FrameworkElement>()
            {
                new StatusBarItem()
                {
                    Content = "VpnGate"
                },
                new Separator(),
                new StatusBarItem()
                {
                    Content = "Check"
                }
            };
        }
    }
}
