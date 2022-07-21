using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WinVPN.Plugin.SDK;
using WinVPN.Service;

namespace WinVPN.ViewModel
{
    internal class MainWindowViewModel : ObservableObject
    {
        private PluginService pluginService = Ioc.Default.GetRequiredService<PluginService>();

        public MainWindowViewModel()
        {
            ShowPluginSettingsCommand = new RelayCommand<IPlugin>(_showPluginSettings);
            PluginEnableCommand = new RelayCommand<IPlugin>(_pluginEnable);
        }

        public ICommand ShowPluginSettingsCommand { get; }

        private void _showPluginSettings(IPlugin plugin)
        {
            plugin.Settings();
        }

        public IEnumerable<IPlugin> Plugins => pluginService.GetPlugins().Values;

        public IEnumerable<TabItem> TabItems => pluginService.GetTabItems();

        public ICommand PluginEnableCommand { get; }
        private void _pluginEnable(IPlugin plugin)
        {
        }
    }
}
