using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinVPN.Model.VPN
{
    internal class L2TP : VpnServer
    {
        private string preSharedKey = "";

        public override VpnProtocol Protocol { get; set; } = VpnProtocol.L2TP;
        public string PreSharedKey 
        { 
            get => preSharedKey; 
            set => SetProperty(ref preSharedKey, value); 
        }
    }
}
