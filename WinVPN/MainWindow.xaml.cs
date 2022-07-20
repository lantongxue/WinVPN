using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DotRas;
using WinVPN.View;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using WinVPN.Service;

namespace WinVPN
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            RasDialer dialer = new RasDialer();

            PluginService s = Ioc.Default.GetRequiredService<PluginService>();
            int dIndex = 1;
            foreach (TabItem tab in s.GetTabItems())
            {
                MainTabControl.Items.Insert(dIndex++, tab);
            }
            PluginListView.ItemsSource = s.GetPlugins();
        }
    }
}
