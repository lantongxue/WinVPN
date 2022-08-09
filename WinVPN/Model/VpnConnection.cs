using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WinVPN.Model
{
    public class VpnConnection : ObservableObject
    {
        private VpnServer _vpnServer = null;
        private string connectState = "WinVPN";
        private IPAddress localEndPoint = null;
        private long uploadSpeed = 0;
        private long downloadSpeed = 0;

        public VpnServer VpnServer 
        { 
            get => _vpnServer; 
            set => SetProperty(ref _vpnServer, value); 
        }
        public string ConnectState 
        { 
            get => connectState; 
            set => SetProperty(ref connectState, value); 
        }

        public IPAddress LocalEndPoint 
        { 
            get => localEndPoint; 
            set => SetProperty(ref localEndPoint, value); 
        }
        public long UploadSpeed 
        { 
            get => uploadSpeed; 
            set => SetProperty(ref uploadSpeed, value); 
        }
        public long DownloadSpeed 
        { 
            get => downloadSpeed; 
            set => SetProperty(ref downloadSpeed, value); 
        }
    }
}
