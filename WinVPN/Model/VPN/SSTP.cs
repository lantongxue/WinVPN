using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinVPN.Model.VPN
{
    [Serializable]
    internal class SSTP : VpnServer
    {
        public override VpnProtocol Protocol { get; set; } = VpnProtocol.SSTP;
    }
}
