using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinVPN.Base;

namespace WinVPN.VPN
{
    public class L2TP : BaseServer
    {
        public override Protocol Protocol => Protocol.L2TP;

        private string preSharedKey = "";
        public string PreSharedKey
        {
            get => preSharedKey;
            set => SetProperty(ref preSharedKey, value);
        }
    }
}
