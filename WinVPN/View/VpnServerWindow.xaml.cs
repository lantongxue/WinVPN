using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
using System.Windows.Shapes;
using WinVPN.Model;
using WinVPN.Service;
using WinVPN.ViewModel;

namespace WinVPN.View
{
    /// <summary>
    /// VpnServerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class VpnServerWindow : MetroWindow
    {
        public VpnServerWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<VpnServerWindowViewModel>();
        }

        MainWindowViewModel MainWindowViewModel = Ioc.Default.GetRequiredService<MainWindowViewModel>();

        public VpnServerWindow(string title)
        {
            InitializeComponent();

            this.Title = title;
            DataContext = Ioc.Default.GetRequiredService<VpnServerWindowViewModel>();
        }

        VpnServer _server = null;
        public VpnServerWindow(string title, VpnServer server)
        {
            InitializeComponent();

            this.Title = title;
            DataContext = Ioc.Default.GetRequiredService<VpnServerWindowViewModel>();
            _server = server;
            ViewModel.SelectedProtocol = server.Protocol;
        }

        internal VpnServerWindowViewModel ViewModel => (VpnServerWindowViewModel)DataContext;

        private ConfigService configService = Ioc.Default.GetRequiredService<ConfigService>();

        private void Protocol_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.Server = _server ?? new VpnServer();
            VpnProtocol val = (VpnProtocol)(sender as ComboBox).SelectedValue;
            switch (val)
            {
                case VpnProtocol.PPTP:
                    contentControl.Content = new PPTP_SSTP_IKEv2_EditView();
                    break;
                case VpnProtocol.SSTP:
                    contentControl.Content = new PPTP_SSTP_IKEv2_EditView();
                    break;
                case VpnProtocol.IKEv2:
                    contentControl.Content = new PPTP_SSTP_IKEv2_EditView();
                    break;
                case VpnProtocol.L2TP:
                    contentControl.Content = new L2TP_EditView();
                    break;
                case VpnProtocol.OpenVPN:
                    contentControl.Content = new OpenVPN_EditView();
                    break;
                case VpnProtocol.WireGuard:
                    contentControl.Content = new WireGuardEditView();
                    break;
            }
        }

        private async void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            var server = ViewModel.Server;
            if(server.Name == string.Empty)
            {
                await this.ShowMessageAsync("提示", "名称不能为空");
                return;
            }
            if (server.Address == string.Empty)
            {
                await this.ShowMessageAsync("提示", "服务器地址不能为空");
                return;
            }
            server.Source = "UserCreate";
            server.Protocol = ViewModel.SelectedProtocol;

            if (_server != null)
            {
                configService.UpdateServer(_server.Id, server);
            }
            else
            {
                configService.AddServer(server);
                MainWindowViewModel.Servers.Add(server);
            }
            configService.Save();

            this.Close();
        }
    }
}
