using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WinVPN.Plugin.SDK;

namespace TestPlugin
{
    public class Class1 : IWinVPNPlugin
    {
        public string PluginName => "TestPlugin";

        public Version PluginVersion => new Version("1.0.0");

        public string PluginAuthor => "Class1";

        public string PluginWebsite => "21321";

        public bool IsSupportSettings => false;

        public Version MiniDependentVersion => new Version("1.2.0");

        public IEnumerable<FrameworkElement> GetMainWindowLeftCommands()
        {
            return null;
        }

        public IEnumerable<FrameworkElement> GetMainWindowRightCommands()
        {
            return null;
        }

        public IEnumerable<FrameworkElement> GetMainWindowStatusBarItems()
        {
            return null;
        }

        public IEnumerable<TabItem> GetMainWindowTabItems()
        {
            return null;
        }

        public void Settings()
        {
            throw new NotImplementedException();
        }
    }
}
