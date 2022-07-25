using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WinVPN.Model;
using WinVPN.Service;
using WinVPN.View;

namespace WinVPN.ViewModel
{
    internal class MainWindowViewModel : ObservableObject
    {
        private PluginService pluginService = Ioc.Default.GetRequiredService<PluginService>();

        public ICommand ShowPluginSettingsCommand { get; }

        private ObservableCollection<TabItem> _tabItems = null;
        public ObservableCollection<TabItem> TabItems => _tabItems;

        private ObservableCollection<FrameworkElement> _leftCommands = null;
        public ObservableCollection<FrameworkElement> LeftCommands => _leftCommands;

        private ObservableCollection<FrameworkElement> _rightCommands = null;
        public ObservableCollection<FrameworkElement> RightCommands => _rightCommands;

        private ObservableCollection<FrameworkElement> _statusBarItems = null;
        public ObservableCollection<FrameworkElement> StatusBarItems => _statusBarItems;

        public ObservableCollection<WinVPN_Plugin> Plugins
        {
            get
            {
                var a = pluginService.GetPlugins().Values.ToList();
                return new ObservableCollection<WinVPN_Plugin>(a);
            }
        }

        public ICommand PluginEnableCommand { get; }

        public Version MainVersion => Assembly.GetEntryAssembly().GetName().Version;

        private ObservableCollection<VpnServer> _servers = null;
        public ObservableCollection<VpnServer> Servers => _servers;

        public ICommand NewVpnServerCommand { get; }

        public MainWindowViewModel()
        {
            ShowPluginSettingsCommand = new RelayCommand<WinVPN_Plugin>(_showPluginSettings);
            PluginEnableCommand = new RelayCommand<WinVPN_Plugin>(_pluginEnable);
            NewVpnServerCommand = new RelayCommand(_newVpnServer);

            this.InitPluginUIItems();
        }

        /// <summary>
        /// 初始化插件对主程序UI的扩展项
        /// </summary>
        private void InitPluginUIItems()
        {
            _tabItems = new ObservableCollection<TabItem>()
            {
                new TabItem()
                {
                    Header = "服务器",
                    Content = new VpnServerView()
                },
                new TabItem()
                {
                    Header = "设置"
                },
                new TabItem()
                {
                    Header = "路由"
                },
                new TabItem()
                {
                    Header = "插件",
                    Content = new PluginView()
                }
            };

            _leftCommands = new ObservableCollection<FrameworkElement>()
            {
                new Button()
                {
                    Content = "添加服务器",
                    ToolTip = "添加服务器",
                    Command = NewVpnServerCommand
                }
            };

            _rightCommands = new ObservableCollection<FrameworkElement>()
            {
                new Button()
                {
                    Content = MainVersion.ToString(),
                    ToolTip = "检查更新",
                }
            };

            _statusBarItems = new ObservableCollection<FrameworkElement>()
            {
                new StatusBarItem()
                {
                    Content = "WinVPN",
                },
                new Separator()
            };

            int dIndex = 1;
            foreach (WinVPN_Plugin plugin in Plugins)
            {
                if(plugin.IsOn)
                {
                    foreach (TabItem tabitem in plugin.TabItems)
                    {
                        _tabItems.Insert(dIndex++, tabitem);
                    }

                    foreach (FrameworkElement lc in plugin.LeftCommands)
                    {
                        _leftCommands.Add(lc);
                    }

                    foreach (FrameworkElement rc in plugin.RightCommands)
                    {
                        _rightCommands.Add(rc);
                    }

                    foreach (FrameworkElement sbi in plugin.StatusBarItems)
                    {
                        _statusBarItems.Add(sbi);
                    }
                }
            }
        }

        private void _showPluginSettings(WinVPN_Plugin plugin)
        {
            plugin.WinVPNPlugin.Settings();
        }

        private void _pluginEnable(WinVPN_Plugin plugin)
        {
            if (MainVersion < plugin.WinVPNPlugin.MiniDependentVersion)
            {
                plugin.IsEnabled = false;
                plugin.IsOn = false;
                MessageBox.Show("请升级主程序后再使用该插件！", "启用失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            pluginService.UpdatePluginConfig(plugin);

            if (!plugin.IsOn)
            {
                for (int i = 0; i < _tabItems.Count; i++)
                {
                    TabItem tabItem = _tabItems[i];
                    if (tabItem.Tag != null && tabItem.Tag == plugin)
                    {
                        _tabItems.Remove(tabItem);
                    }
                }
            }
            else
            {
                int dIndex = 1;
                foreach (TabItem tab in plugin.TabItems)
                {
                    _tabItems.Insert(dIndex++, tab);
                }
            }
        }

        private void _newVpnServer()
        {
            VpnServerWindow window = new VpnServerWindow("添加服务器");
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
    }
}
