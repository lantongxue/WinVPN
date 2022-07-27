using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace WinVPN.Model
{
    [Serializable]
    public class VpnServer
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string Address { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Info { get; set; }

        public long Delay { get; set; }

        public virtual VpnProtocol Protocol { get; set; }

        public string Source { get; set; }

        public long Traffic { get; set; }

        public bool IsConnected { get; set; }

        public VpnServer()
        {

        }

        public async Task<long> PingAsync()
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = await ping.SendPingAsync(this.Address);
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

    public enum VpnProtocol
    {
        PPTP,
        L2TP,
        SSTP,
        IKEv2,
        WireGuard,
        OpenVPN
    }
}
