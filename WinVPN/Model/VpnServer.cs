using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace WinVPN.Model
{
    internal class VpnServer
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string PreSharedKey { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Info { get; set; }

        public long Delay { get; set; }

        public VpnProtocol Protocol { get; set; }

        public long Ping()
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(this.Address);
                this.Delay = reply.RoundtripTime;
                ping.Dispose();
            }
            catch
            {
                this.Delay = -1;
            }

            return this.Delay;
        }
    }

    internal enum VpnProtocol
    {
        PPTP,
        L2TP,
        SSTP,
        IKEV2
    }
}
