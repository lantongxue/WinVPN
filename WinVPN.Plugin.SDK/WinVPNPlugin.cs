using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WinVPN.Plugin.SDK
{
    public abstract class WinVPNPlugin
    {
        public bool IsEnable { get; set; }

        public virtual IEnumerable<TabItem> GetMainWindowTabItems()
        {
            return null;
        }
    }
}
