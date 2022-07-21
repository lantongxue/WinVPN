using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
                .AddTransient<MainWindowViewModel>()
                .BuildServiceProvider();

            Ioc.Default.ConfigureServices(s);
        }
    }
}
