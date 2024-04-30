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
using WinVPN.Base;
using WinVPN.Service;
using WinVPN.ViewModel;
using WinVPN.VPN;

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

        BaseServer _server = null;
        public VpnServerWindow(string title, BaseServer server)
        {
            InitializeComponent();

            this.Title = title;
            DataContext = Ioc.Default.GetRequiredService<VpnServerWindowViewModel>();
            _server = server;
            ViewModel.SelectedProtocol = server.Protocol;
        }

        internal VpnServerWindowViewModel ViewModel => (VpnServerWindowViewModel)DataContext;

        private void Protocol_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.Server = _server ?? new BaseServer();
            Protocol val = (Protocol)(sender as ComboBox).SelectedValue;
            switch (val)
            {
                case Protocol.PPTP:
                    contentControl.Content = new PPTP_SSTP_IKEv2_EditView();
                    break;
                case Protocol.SSTP:
                    contentControl.Content = new PPTP_SSTP_IKEv2_EditView();
                    break;
                case Protocol.IKEv2:
                    contentControl.Content = new PPTP_SSTP_IKEv2_EditView();
                    break;
                case Protocol.L2TP:
                    ViewModel.Server = _server ?? new L2TP();
                    contentControl.Content = new L2TP_EditView();
                    break;
                case Protocol.OpenVPN:
                    contentControl.Content = new OpenVPN_EditView();
                    break;
                case Protocol.WireGuard:
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
            }
            else
            {
                MainWindowViewModel.Servers.Add(server);
            }

            this.Close();
        }
    }
}
