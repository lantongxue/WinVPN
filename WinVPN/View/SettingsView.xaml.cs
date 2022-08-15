using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinVPN.Model;
using WinVPN.Service;
using WinVPN.ViewModel;

namespace WinVPN.View
{
    /// <summary>
    /// SettingsView.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsView : Grid
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private ConfigService configService = Ioc.Default.GetRequiredService<ConfigService>();
        private void DnsListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            CustomDns dns = (CustomDns)comboBox.SelectedItem;
            MainWindowViewModel model = (MainWindowViewModel)DataContext;
            model.AppConfig.Dns1 = dns.Dns1.ToString();
            model.AppConfig.Dns2 = dns.Dns2?.ToString();

            configService.UpdateAppConfig(model.AppConfig);
        }
    }
}
