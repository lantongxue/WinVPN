using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinVPN.Base;

namespace WinVPN.VPN
{
    public class WireGuard : BaseServer
    {
        public override Protocol Protocol => Protocol.WireGuard;

        public void Connect()
        {

        }

        public void Disconnect()
        {

        }
    }
}
