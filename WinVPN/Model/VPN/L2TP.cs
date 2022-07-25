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
        public string PreSharedKey { get; set; }
    }
}
