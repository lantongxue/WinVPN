using DotRas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using WinVPN.Model;
using System.Diagnostics;

namespace WinVPN.Service
{
    internal class VpnService
    {
        const string EntryName = "WinVPN";
        const string PhoneBookPath = "winvpn.pbk";

        RasDialer dialer = null;
        RasConnection connection = null;

        public async Task Connect(VpnServer server)
        {
            if(server.Protocol == VpnProtocol.WireGuard)
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
            dialer.Timeout = 3000;
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
            Trace.WriteLine(e.Connected, "Dialer_DialCompleted");
            if (e.Connected)
            {
                connection = RasConnection.GetActiveConnections().Where(ac => ac.Handle == e.Handle).FirstOrDefault();
            }
        }

        private void Dialer_StateChanged(object sender, StateChangedEventArgs e)
        {
            Trace.WriteLine(e.State, "Dialer_StateChanged");
        }

        public void Disconnect()
        {
            dialer.DialAsyncCancel();
        }
    }
}
