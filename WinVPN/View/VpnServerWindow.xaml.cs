using MahApps.Metro.Controls;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
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
using WinVPN.Model.VPN;
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

        public VpnServerWindow(string title)
        {
            InitializeComponent();

            this.Title = title;
            DataContext = Ioc.Default.GetRequiredService<VpnServerWindowViewModel>();
        }

        internal VpnServerWindowViewModel ViewModel => (VpnServerWindowViewModel)DataContext;

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VpnProtocol val = (VpnProtocol)(sender as ComboBox).SelectedValue;
            VpnServer server = new VpnServer()
            {
                Protocol = val
            };
            if (ViewModel.Server != null)
            {
                server.Name = ViewModel.Server.Name;
                server.Address = ViewModel.Server.Address;
                server.Username = ViewModel.Server.Username;
                server.Password = ViewModel.Server.Password;
                server.Info = ViewModel.Server.Info;
            }
            switch (val)
            {
                case VpnProtocol.PPTP:
                    ViewModel.Server = server;
                    contentControl.Content = new PPTP_SSTP_IKEv2_EditView();
                    break;
                case VpnProtocol.SSTP:
                    ViewModel.Server = server;
                    contentControl.Content = new PPTP_SSTP_IKEv2_EditView();
                    break;
                case VpnProtocol.IKEv2:
                    ViewModel.Server = server;
                    contentControl.Content = new PPTP_SSTP_IKEv2_EditView();
                    break;
                case VpnProtocol.L2TP:
                    ViewModel.Server = new L2TP()
                    {
                        Name = server.Name,
                        Address = server.Address,
                        Username = server.Username,
                        Password = server.Password,
                        Info = server.Info
                    };
                    contentControl.Content = new L2TP_EditView();
                    break;
                case VpnProtocol.OpenVPN:
                    contentControl.Content = new OpenVPN_EditView();
                    break;
                case VpnProtocol.WireGuard:
                    server.Username = string.Empty;
                    server.Password = string.Empty;
                    ViewModel.Server = server;
                    contentControl.Content = new WireGuardEditView();
                    break;
            }
        }
    }
}
