using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinVPN.Model.VPN
{
    [Serializable]
    internal class L2TP : VpnServer
    {
        public override VpnProtocol Protocol { get; set; } = VpnProtocol.L2TP;
        public string PreSharedKey { get; set; }
    }
}
