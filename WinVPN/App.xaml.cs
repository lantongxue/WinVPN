using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows;
using WinVPN.Service;
using WinVPN.ViewModel;

namespace WinVPN
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IServiceProvider s = new ServiceCollection()
                .AddSingleton<PluginService>()
                .AddSingleton<ConfigService>()
                .AddSingleton<VpnService>()
                .AddSingleton<MainWindowViewModel>()
                .AddTransient<VpnServerWindowViewModel>()
                .BuildServiceProvider();

            Ioc.Default.ConfigureServices(s);
        }
    }
}
