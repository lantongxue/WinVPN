using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinVPN.Model.VPN
{
    internal class IKEv2 : VpnServer
    {
        public override VpnProtocol Protocol { get; set; } = VpnProtocol.IKEv2;
    }
}
