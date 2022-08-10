using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinVPN.Model;

namespace WinVPN.ViewModel
{
    internal class VpnServerWindowViewModel : ObservableObject
    {
        public Array VpnProtocols => Enum.GetValues(typeof(VpnProtocol));

        public VpnProtocol SelectedProtocol { get; set; } = VpnProtocol.PPTP;

        public VpnServer Server { get; set; }


        public VpnServerWindowViewModel()
        {
        }
    }
}
