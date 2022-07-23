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
using System.Windows.Input;
using WinVPN.Plugin.SDK;
using WinVPN.Service;
using WinVPN.View;

namespace WinVPN.ViewModel
{
    internal class MainWindowViewModel : ObservableObject
    {
        private PluginService pluginService = Ioc.Default.GetRequiredService<PluginService>();

        public MainWindowViewModel()
        {
            ShowPluginSettingsCommand = new RelayCommand<IPlugin>(_showPluginSettings);
            PluginEnableCommand = new RelayCommand<IPlugin>(_pluginEnable);

            this.InitTabItems();
        }

        public ICommand ShowPluginSettingsCommand { get; }

        private void _showPluginSettings(IPlugin plugin)
        {
            plugin.Settings();
        }

        public ObservableCollection<IPlugin> Plugins
        {
            get
            {
                var a = pluginService.GetPlugins().Values.ToList();
                return new ObservableCollection<IPlugin>(a);
            }
        }

        private ObservableCollection<TabItem> _tabItems = null;

        public ObservableCollection<TabItem> TabItems => _tabItems;

        private void InitTabItems()
        {
            _tabItems = new ObservableCollection<TabItem>()
            {
                new TabItem()
                {
                    Header = "服务器"
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
            int dIndex = 1;
            foreach (TabItem tab in pluginService.GetTabItems())
            {
                _tabItems.Insert(dIndex++, tab);
            }
        }

        public ICommand PluginEnableCommand { get; }

        public Version MainVersion => Assembly.GetEntryAssembly().GetName().Version;
        private void _pluginEnable(IPlugin plugin)
        {
            WinVPNPlugin p = (WinVPNPlugin)plugin;

            if (MainVersion < plugin.MiniDependentVersion)
            {
                p.IsEnable = false;
                MessageBox.Show("请升级主程序后再使用该插件！", "启用失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            pluginService.UpdatePluginConfig(p);

            if (!p.IsEnable)
            {
                for (int i = 0; i < _tabItems.Count; i++)
                {
                    TabItem tabItem = _tabItems[i];
                    if (tabItem.Tag != null && tabItem.Tag.ToString() == p.Name)
                    {
                        _tabItems.Remove(tabItem);
                    }
                }
            }
            else
            {
                int dIndex = 1;
                foreach (TabItem tab in pluginService.GetTabItems())
                {
                    if(tab.Tag != null && tab.Tag.ToString() == p.Name)
                    {
                        _tabItems.Insert(dIndex++, tab);
                    }
                }
            }
        }
    }
}
