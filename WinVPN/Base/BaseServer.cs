using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WinVPN.Interface;
using Newtonsoft.Json;
using DotRas;
using System.Net.NetworkInformation;

namespace WinVPN.Base
{
    public class BaseServer : ObservableObject, IServer
    {
        private string id = "";
        private string name = "";
        private string address = "";
        private string username = "";
        private string password = "";
        private string info = "";
        private string delay = "-1ms";
        private Protocol protocol;
        private string source = "";
        private long traffic = 0;
        private bool isConnected = false;
        private bool isConnecting = false;
        private string connectState = "WinVPN";
        private IPAddress localEndPoint = null;
        private long uploadSpeed = 0;
        private long downloadSpeed = 0;
        private string linkSpeed = "";
        private string errorMessage = "";

        public BaseServer()
        {
            id = "WV" + Guid.NewGuid().ToString();
        }

        public string Id
        {
            get => id;
            set => id = value;
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

        [JsonIgnore]
        public string Delay
        {
            get => delay;
            set => SetProperty(ref delay, value);
        }

        public virtual Protocol Protocol
        {
            get => protocol;
            set => SetProperty(ref protocol, value);
        }

        public string Source
        {
            get => source;
            set => SetProperty(ref source, value);
        }

        [JsonIgnore]
        public long Traffic
        {
            get => traffic;
            set => SetProperty(ref traffic, value);
        }

        [JsonIgnore]
        public bool IsConnected
        {
            get => isConnected;
            set => SetProperty(ref isConnected, value);
        }

        [JsonIgnore]
        public bool IsConnecting
        {
            get => isConnecting;
            set => SetProperty(ref isConnecting, value);
        }

        [JsonIgnore]
        public string ConnectState
        {
            get => connectState;
            set => SetProperty(ref connectState, value);
        }

        [JsonIgnore]
        public IPAddress LocalEndPoint
        {
            get => localEndPoint;
            set => SetProperty(ref localEndPoint, value);
        }

        [JsonIgnore]
        public long UploadSpeed
        {
            get => uploadSpeed;
            set => SetProperty(ref uploadSpeed, value);
        }

        [JsonIgnore]
        public long DownloadSpeed
        {
            get => downloadSpeed;
            set => SetProperty(ref downloadSpeed, value);
        }

        [JsonIgnore]
        public string LinkSpeed
        {
            get => linkSpeed;
            set => SetProperty(ref linkSpeed, value);
        }

        [JsonIgnore]
        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        public event EventHandler<StateChangedEventArgs> ConnectionStateChanged;

        public void OnConnectionStateChanged(StateChangedEventArgs e)
        {
            ConnectionStateChanged?.Invoke(this, e);
        }

        public async Task<long> PingAsync()
        {
            try
            {
                Delay = "Ping...";
                Ping ping = new Ping();
                PingReply reply = await ping.SendPingAsync(Address);
                Delay = reply.RoundtripTime + "ms";
                ping.Dispose();
                return reply.RoundtripTime;
            }
            catch (Exception ex)
            {
                Delay = ex.Message;
            }
            return 0;
        }
    }
}
