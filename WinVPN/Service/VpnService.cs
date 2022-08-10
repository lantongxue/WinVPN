using DotRas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using WinVPN.Model;
using System.Diagnostics;
using System.Threading;

namespace WinVPN.Service
{
    internal class VpnService
    {
        const string EntryName = "WinVPN";
        const string PhoneBookPath = "winvpn.pbk";

        RasDialer dialer = null;
        RasConnection connection = null;

        CancellationTokenSource trafficTokenSource = new CancellationTokenSource();

        public VpnConnection VpnConnection { get; set; } = new VpnConnection();

        public async Task Connect(VpnServer server)
        {
            VpnConnection.VpnServer = server;

            if (server.Protocol == VpnProtocol.WireGuard)
            {
                // WireGuard
            }
            string deviceName = $"({server.Protocol})";
            RasDevice device = RasDevice.GetDevices().Where(d => d.Name.Contains(deviceName)).FirstOrDefault();
            NetworkCredential credential = new NetworkCredential(server.Username, server.Password);
            RasPhoneBook phoneBook = new RasPhoneBook();
            phoneBook.Open(PhoneBookPath);
            RasEntry entry = phoneBook.Entries.Where(e => e.Name == EntryName).FirstOrDefault();
            if (entry == null)
            {
                entry = RasEntry.CreateVpnEntry(EntryName, server.Address, server.GetRasVpnStrategy(), device);
                phoneBook.Entries.Add(entry);
            }
            else
            {
                entry.PhoneNumber = server.Address;
                entry.VpnStrategy = server.GetRasVpnStrategy();
                entry.Device = device;
            }
            if (server.Protocol == VpnProtocol.L2TP)
            {
                entry.Options.UsePreSharedKey = true;
                entry.UpdateCredentials(RasPreSharedKey.Client, server.PreSharedKey);
            }

            entry.Options.PreviewDomain = false;
            entry.Options.ShowDialingProgress = false;
            entry.Options.PromoteAlternates = false;
            entry.Options.DoNotNegotiateMultilink = false;

            //// 手动设置DNS
            //entry.DnsAddress = IPAddress.Parse("");
            //entry.DnsAddressAlt = IPAddress.Parse("");

            entry.Update();

            dialer = new RasDialer();
            dialer.EntryName = EntryName;
            dialer.PhoneBookPath = PhoneBookPath;
            dialer.Credentials = credential;
            dialer.Error += Dialer_Error;
            dialer.StateChanged += Dialer_StateChanged;
            dialer.DialCompleted += Dialer_DialCompleted;

            await Task.Run(() =>
            {
                dialer.DialAsync();
            });
        }

        private void Dialer_Error(object sender, System.IO.ErrorEventArgs e)
        {
            Trace.WriteLine(e.ToString(), "Dialer_Error");
        }

        private void Dialer_DialCompleted(object sender, DialCompletedEventArgs e)
        {
            VpnConnection.VpnServer.IsConnected = e.Connected;
            if (e.Connected)
            {
                connection = RasConnection.GetActiveConnections().Where(ac => ac.Handle == e.Handle).FirstOrDefault();

                RasIPInfo ip = (RasIPInfo)connection.GetProjectionInfo(RasProjectionType.IP);
                VpnConnection.LocalEndPoint = ip.IPAddress;

                Task.Factory.StartNew(async () => 
                {
                    long u = 0;
                    long d = 0;
                    while (true && !trafficTokenSource.IsCancellationRequested)
                    {
                        RasLinkStatistics linkStatistics = connection.GetLinkStatistics();

                        VpnConnection.UploadSpeed = linkStatistics.BytesTransmitted - u;
                        VpnConnection.DownloadSpeed = linkStatistics.BytesReceived - d;

                        VpnConnection.VpnServer.Traffic = linkStatistics.BytesReceived + linkStatistics.BytesTransmitted;

                        d = linkStatistics.BytesReceived;
                        u = linkStatistics.BytesTransmitted;
                        await Task.Delay(1000);
                    }
                }, trafficTokenSource.Token);
            }
        }

        private void Dialer_StateChanged(object sender, StateChangedEventArgs e)
        {
            VpnConnection.ConnectState = e.State.ToString();
            if(e.ErrorCode > 0)
            {
                VpnConnection.ConnectState = e.ErrorMessage;
            }
            if(e.State == RasConnectionState.Connected)
            {
                VpnConnection.ConnectState = "连接成功";
            }
        }

        public async Task Disconnect()
        {
            await Task.Run(() =>
            {
                // 停止流量记录刷新
                trafficTokenSource.Cancel();

                if (dialer.IsBusy)
                {
                    dialer.DialAsyncCancel();
                }
                else
                {
                    connection.HangUp();
                }
                VpnConnection.VpnServer.IsConnected = false;
                VpnConnection.LocalEndPoint = null;
                VpnConnection.ConnectState = "WinVPN";
                VpnConnection.UploadSpeed = 0;
                VpnConnection.DownloadSpeed = 0;
            });
        }
    }
}
