using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinVPN.Base;

namespace WinVPN.VPN
{
    public class PPTP : BaseServer
    {
        public override Protocol Protocol => Protocol.PPTP;
    }
}
