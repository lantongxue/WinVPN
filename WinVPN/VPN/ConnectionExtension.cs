using DotRas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinVPN.Base;

namespace WinVPN.VPN
{
    public static class ConnectionExtension
    {
        static RasDialer dialer = null;
        static RasConnection connection = null;
        static CancellationTokenSource trafficTokenSource = null;

        public static RasVpnStrategy GetRasVpnStrategy(this BaseServer server)
        {
            RasVpnStrategy strategy = RasVpnStrategy.Default;

            switch (server.Protocol)
            {
                case Protocol.PPTP:
                    strategy = RasVpnStrategy.PptpOnly;
                    break;
                case Protocol.L2TP:
                    strategy = RasVpnStrategy.L2tpOnly;
                    break;
                case Protocol.SSTP:
                    strategy = RasVpnStrategy.SstpOnly;
                    break;
                case Protocol.IKEv2:
                    strategy = RasVpnStrategy.IkeV2Only;
                    break;
            }
            return strategy;
        }

        public static void Connect(this BaseServer server)
        {
            string deviceName = $"({server.Protocol})";
            RasDevice device = RasDevice.GetDevices().Where(d => d.Name.Contains(deviceName)).FirstOrDefault();
            NetworkCredential credential = new NetworkCredential(server.Username, server.Password);

            using (RasPhoneBook phoneBook = new RasPhoneBook())
            {
                phoneBook.Open(Global.PhoneBookPath);
                RasEntry entry = phoneBook.Entries.Where(e => e.Name == Global.EntryName).FirstOrDefault();
                if (entry == null)
                {
                    entry = RasEntry.CreateVpnEntry(Global.EntryName, server.Address, server.GetRasVpnStrategy(), device);
                    phoneBook.Entries.Add(entry);
                }
                else
                {
                    entry.PhoneNumber = server.Address;
                    entry.VpnStrategy = server.GetRasVpnStrategy();
                    entry.Device = device;
                }
                if (server.Protocol == Protocol.L2TP)
                {
                    entry.Options.UsePreSharedKey = true;
                    string psk = (server as L2TP).PreSharedKey;
                    entry.UpdateCredentials(RasPreSharedKey.Client, psk);
                }

                entry.Options.PreviewDomain = false;
                entry.Options.ShowDialingProgress = false;
                entry.Options.PromoteAlternates = false;
                entry.Options.DoNotNegotiateMultilink = false;

                // set custom DNS
                //if (IPAddress.TryParse(_appconfig.Dns1, out IPAddress dns1))
                //{
                //    entry.DnsAddress = dns1;
                //}
                //if (IPAddress.TryParse(_appconfig.Dns2, out IPAddress dns2))
                //{
                //    entry.DnsAddressAlt = dns2;
                //}

                entry.Update();

                dialer = new RasDialer();
                dialer.EntryName = Global.EntryName;
                dialer.PhoneBookPath = Global.PhoneBookPath;
                dialer.Credentials = credential;
                dialer.StateChanged += (sender, e) => 
                {
                    server.OnConnectionStateChanged(e);
                    if (e.ErrorCode > 0)
                    {
                        Disconnect(server);
                    }
                };
                dialer.DialCompleted += (sender, e) =>
                {
                    server.IsConnected = e.Connected;
                    server.IsConnecting = false;
                    if (e.Connected)
                    {
                        connection = RasConnection.GetActiveConnections().Where(ac => ac.Handle == e.Handle).FirstOrDefault();

                        RasIPInfo ip = (RasIPInfo)connection.GetProjectionInfo(RasProjectionType.IP);
                        server.LocalEndPoint = ip.IPAddress;

                        RasLinkStatistics linkStatistics = connection.GetLinkStatistics();
                        server.LinkSpeed = $"LinkSpeed：{linkStatistics.LinkSpeed / 1000 / 1000}MB";

                        trafficTokenSource = new CancellationTokenSource();
                        Task.Factory.StartNew(async () =>
                        {
                            long u = 0;
                            long d = 0;
                            while (true && !trafficTokenSource.IsCancellationRequested)
                            {
                                linkStatistics = connection.GetLinkStatistics();

                                server.UploadSpeed = linkStatistics.BytesTransmitted - u;
                                server.DownloadSpeed = linkStatistics.BytesReceived - d;

                                server.Traffic = linkStatistics.BytesReceived + linkStatistics.BytesTransmitted;

                                d = linkStatistics.BytesReceived;
                                u = linkStatistics.BytesTransmitted;
                                await Task.Delay(1000);
                            }
                        }, trafficTokenSource.Token);
                    }
                };

                dialer.DialAsync();

                server.IsConnecting = true;
            }
        }

        public static void Disconnect(this BaseServer server)
        {
            trafficTokenSource?.Cancel();

            if (dialer.IsBusy)
            {
                dialer.DialAsyncCancel();
            }
            else
            {
                connection?.HangUp(true);
            }
            dialer.Dispose();
            using (RasPhoneBook phoneBook = new RasPhoneBook())
            {
                phoneBook.Open(Global.PhoneBookPath);
                phoneBook.Entries.Clear();
            }

            server.IsConnected = false;
            server.LocalEndPoint = null;
            server.ConnectState = "WinVPN";
            server.UploadSpeed = 0;
            server.DownloadSpeed = 0;
            server.LinkSpeed = "";
        }
    }
}
