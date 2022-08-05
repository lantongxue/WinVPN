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
        private ConfigService configService = Ioc.Default.GetRequiredService<ConfigService>();
        private VpnService vpnService = Ioc.Default.GetRequiredService<VpnService>();

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

        public ICommand EditVpnServerCommand { get; }

        public ICommand DeleteVpnServerCommand { get; }

        public IAsyncRelayCommand PingAsyncCommand { get; }

        public IAsyncRelayCommand PingServersAsyncCommand { get; }

        public IAsyncRelayCommand ConnectAsyncCommand { get; }

        public MainWindowViewModel()
        {
            ShowPluginSettingsCommand = new RelayCommand<WinVPN_Plugin>(_showPluginSettings);
            PluginEnableCommand = new RelayCommand<WinVPN_Plugin>(_pluginEnable);
            NewVpnServerCommand = new RelayCommand(_newVpnServer);
            EditVpnServerCommand = new RelayCommand<VpnServer>(_editVpnServer);
            DeleteVpnServerCommand = new RelayCommand<VpnServer>(_deleteVpnServer);
            PingAsyncCommand = new AsyncRelayCommand<VpnServer>(_pingVpnServer);
            PingServersAsyncCommand = new AsyncRelayCommand(_pingVpnServers);
            ConnectAsyncCommand = new AsyncRelayCommand<VpnServer>(_connectVpnServer);

            _servers = new ObservableCollection<VpnServer>(configService.GetServers());

            _initPluginUIItems();
        }

        /// <summary>
        /// 初始化插件对主程序UI的扩展项
        /// </summary>
        private void _initPluginUIItems()
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
                },
                new Button()
                {
                    Content = "服务器测速",
                    ToolTip = "服务器测速",
                    Command = PingServersAsyncCommand
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
                }
            };

            foreach (WinVPN_Plugin plugin in Plugins)
            {
                if(plugin.IsOn)
                {
                    _createPluginUIItems(plugin);
                }
            }
        }

        /// <summary>
        /// 创建由插件附加的UI元素
        /// </summary>
        /// <param name="plugin"></param>
        private void _createPluginUIItems(WinVPN_Plugin plugin)
        {
            int dIndex = 1;
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

        /// <summary>
        /// 移除由插件创建的UI元素
        /// </summary>
        /// <param name="plugin"></param>
        private void _removePluginUIItems(WinVPN_Plugin plugin)
        {
            // 所有元素采用倒序遍历进行删除，因为正常删除一个元素后，会导致集合中的索引发生变化
            for (int i = _tabItems.Count - 1; i >= 0; i--)
            {
                TabItem tabItem = _tabItems[i];
                if (tabItem.Tag != null && tabItem.Tag == plugin)
                {
                    _tabItems.Remove(tabItem);
                }
            }

            for (int i = _leftCommands.Count - 1; i >= 0; i--)
            {
                FrameworkElement lc = _leftCommands[i];
                if (lc.Tag != null && lc.Tag == plugin)
                {
                    _leftCommands.Remove(lc);
                }
            }

            for (int i = _rightCommands.Count - 1; i >= 0; i--)
            {
                FrameworkElement rc = _rightCommands[i];
                if (rc.Tag != null && rc.Tag == plugin)
                {
                    _rightCommands.Remove(rc);
                }
            }

            for (int i = _statusBarItems.Count - 1; i >= 0; i--)
            {
                FrameworkElement sbi = _statusBarItems[i];
                if (sbi.Tag != null && sbi.Tag == plugin)
                {
                    _statusBarItems.Remove(sbi);
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
                _removePluginUIItems(plugin);
            }
            else
            {
                _createPluginUIItems(plugin);
            }
        }

        private void _newVpnServer()
        {
            VpnServerWindow window = new VpnServerWindow("添加服务器");
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        private void _editVpnServer(VpnServer server)
        {
            VpnServerWindow window = new VpnServerWindow("编辑服务器", server);
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        private void _deleteVpnServer(VpnServer server)
        {
            configService.DeleteServer(server.Id);
            configService.Save();
            _servers.Remove(server);
        }

        private async Task _pingVpnServer(VpnServer server)
        {
            await server.PingAsync();
        }

        private async Task _pingVpnServers()
        {
            foreach(VpnServer server in _servers)
            {
                await server.PingAsync();
            }
        }

        private async Task _connectVpnServer(VpnServer server)
        {
            await vpnService.Connect(server);
        }
    }
}
