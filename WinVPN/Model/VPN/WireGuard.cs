﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinVPN.Model.VPN
{
    internal class WireGuard : VpnServer
    {
        public override VpnProtocol Protocol { get; set; } = VpnProtocol.WireGuard;
    }
}
