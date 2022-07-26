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
            switch (val)
            {
                case VpnProtocol.PPTP:
                case VpnProtocol.SSTP:
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
    }
}
