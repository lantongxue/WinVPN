using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace WinVPN.Model
{
    public class VpnServer : ObservableObject
    {
        private string id = "";
        private string name = "";
        private string address = "";
        private string username = "";
        private string password = "";
        private string info = "";
        private long delay = -1;
        private VpnProtocol protocol;
        private string source = "";
        private long traffic = 0;
        private bool isConnected = false;
        private string preSharedKey = "";

        public string Id 
        { 
            get => id; 
            set => SetProperty(ref id, value); 
        }
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public string Address 
        { 
            get => address; 
            set => SetProperty(ref address, value); 
        }
        public string Username 
        { 
            get => username; 
            set => SetProperty(ref username, value); 
        }
        public string Password 
        { 
            get => password; 
            set => SetProperty(ref password, value); 
        }
        public string Info 
        { 
            get => info; 
            set => SetProperty(ref info, value); 
        }
        public long Delay
        {
            get => delay;
            set => SetProperty(ref delay, value);
        }
        public virtual VpnProtocol Protocol 
        { 
            get => protocol; 
            set => SetProperty(ref protocol, value); 
        }
        public string Source 
        { 
            get => source; 
            set => SetProperty(ref source, value); 
        }
        public long Traffic 
        { 
            get => traffic; 
            set => SetProperty(ref traffic, value); 
        }
        public bool IsConnected 
        { 
            get => isConnected; 
            set => SetProperty(ref isConnected, value); 
        }
        public string PreSharedKey
        {
            get => preSharedKey;
            set => SetProperty(ref preSharedKey, value);
        }

        public VpnServer()
        {
            this.id = "WV" + Guid.NewGuid().ToString();
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
