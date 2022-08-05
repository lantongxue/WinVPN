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
        const string DeviceName = "WinVPN";
        const string PhoneBookPath = "winvpn.pbk";

        public async Task Connect(VpnServer server)
        {
            if(server.Protocol == VpnProtocol.WireGuard)
            {
                // WireGuard
            }
            RasDevice winvpn = RasDevice.GetDevices().Where(d => d.Name == DeviceName).FirstOrDefault();
            if(winvpn == null)
            {
                winvpn = RasDevice.Create(DeviceName, RasDeviceType.Vpn);
            }

            NetworkCredential credential = new NetworkCredential(server.Username, server.Password);

            RasPhoneBook phoneBook = new RasPhoneBook();
            phoneBook.Open(PhoneBookPath);
            RasEntry entry = phoneBook.Entries.Where(e => e.Name == DeviceName).FirstOrDefault();
            if (entry == null)
            {
                entry = RasEntry.CreateVpnEntry(DeviceName, server.Address, server.GetRasVpnStrategy(), winvpn);
                phoneBook.Entries.Add(entry);
            }
            entry.Options.PreviewDomain = false;
            entry.Options.ShowDialingProgress = false;
            entry.Options.PromoteAlternates = false;
            entry.Options.DoNotNegotiateMultilink = false;
            if (server.Protocol == VpnProtocol.L2TP)
            {
                entry.Options.UsePreSharedKey = true;
                entry.UpdateCredentials(RasPreSharedKey.Client, server.PreSharedKey);
            }
            entry.Update();


            RasDialer dialer = new RasDialer();
            dialer.EntryName = DeviceName;
            dialer.PhoneBookPath = PhoneBookPath;
            dialer.Credentials = credential;
            dialer.StateChanged += Dialer_StateChanged;
            dialer.DialCompleted += Dialer_DialCompleted;
            dialer.Error += Dialer_Error;

            await Task.Run(() =>
            {
                dialer.Dial();
            });
        }

        private void Dialer_Error(object sender, System.IO.ErrorEventArgs e)
        {
            Trace.WriteLine(e.ToString(), "Dialer_Error");
        }

        private void Dialer_DialCompleted(object sender, DialCompletedEventArgs e)
        {
            Trace.WriteLine(e.ToString(), "Dialer_DialCompleted");
        }

        private void Dialer_StateChanged(object sender, StateChangedEventArgs e)
        {
            Trace.WriteLine(e.State, "Dialer_StateChanged");
        }

        public void Disconnect()
        {

        }
    }
}
