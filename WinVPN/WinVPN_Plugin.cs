using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using WinVPN.Plugin.SDK;

namespace WinVPN
{
    internal class WinVPN_Plugin
    {
        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsOn { get; set; }

        public IWinVPNPlugin WinVPNPlugin { get; }

        public IEnumerable<TabItem> _tabItems = null;
        public IEnumerable<TabItem> TabItems => GetMainWindowTabItems();

        public IEnumerable<FrameworkElement> _leftCommands = null;
        public IEnumerable<FrameworkElement> LeftCommands => GetMainWindowLeftCommands();

        public IEnumerable<FrameworkElement> _rightCommands = null;
        public IEnumerable<FrameworkElement> RightCommands => GetMainWindowRightCommands();

        public IEnumerable<FrameworkElement> _statusBarItems = null;
        public IEnumerable<FrameworkElement> StatusBarItems => GetMainWindowStatusBarItems();

        public WinVPN_Plugin(string name, bool isEnabled, bool isOn, IWinVPNPlugin plugin)
        {
            Name = name;
            IsEnabled = isEnabled;
            IsOn = isOn;
            WinVPNPlugin = plugin;
        }

        public WinVPN_Plugin(IWinVPNPlugin plugin)
        {
            WinVPNPlugin = plugin;
        }

        private IEnumerable<TabItem> GetMainWindowTabItems()
        {
            if (_tabItems == null)
            {
                _tabItems = WinVPNPlugin.GetMainWindowTabItems();
                foreach (TabItem tabItem in _tabItems)
                {
                    tabItem.Tag = this;
                }
            }
            return _tabItems;
        }

        private IEnumerable<FrameworkElement> GetMainWindowLeftCommands()
        {
            if (_leftCommands == null)
            {
                _leftCommands = WinVPNPlugin.GetMainWindowLeftCommands();
                foreach (FrameworkElement item in _leftCommands)
                {
                    item.Tag = this;
                }
            }
            return _leftCommands;
        }

        private IEnumerable<FrameworkElement> GetMainWindowRightCommands()
        {
            if (_rightCommands == null)
            {
                _rightCommands = WinVPNPlugin.GetMainWindowRightCommands();
                foreach (FrameworkElement item in _rightCommands)
                {
                    item.Tag = this;
                }
            }
            return _rightCommands;
        }

        private IEnumerable<FrameworkElement> GetMainWindowStatusBarItems()
        {
            if (_statusBarItems == null)
            {
                _statusBarItems = WinVPNPlugin.GetMainWindowStatusBarItems();
                foreach (FrameworkElement item in _statusBarItems)
                {
                    item.Tag = this;
                }
            }
            return _statusBarItems;
        }
    }
}
