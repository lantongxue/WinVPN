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
        public ICommand ShowPluginSettingsCommand { get; }

        private PluginService pluginService = Ioc.Default.GetRequiredService<PluginService>();

        public MainWindowViewModel()
        {
            ShowPluginSettingsCommand = new RelayCommand<IPlugin>(_showPluginSettings);
        }

        private void _showPluginSettings(IPlugin plugin)
        {
            plugin.Settings();
        }

        public IEnumerable<IPlugin> Plugins => pluginService.GetPlugins().Values;

        public IEnumerable<TabItem> TabItems => pluginService.GetTabItems();
    }
}
